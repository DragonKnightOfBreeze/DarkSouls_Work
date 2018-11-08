//DONE：OnEnter、OnExit方法可以在这个脚本中，注册到对应事件上
//TODO：改为有限状态机（移动、跳跃、跑动、翻滚等）
//DONE：状态累积应该是可选的（或者：在第一段攻击的后摇中，才开始累积）

/*******
 * ［概述］
 * 
 * 动作控制器
 *
 * ［用法］
 * 
 * ［备注］
 * RigidBody不应该在update方法中调用
 * rigid.position = planerMoveSpeed * Time.fixedDeltaTime;
 * rigid.velocity = planerMoveSpeed; 	//会覆盖y值
 * 
 * Physics.OverlapCapsule()：检测胶囊体的碰撞。
 *
 * 更好的控制特殊FSM状态时的角色的位移速度的方法：
 * 通过FSM参数找到动画模型中的曲线的引用，然后设置为位移速度。
 *
 * 攻击时，角色要向前移动微小的距离（使用曲线）。
 *
 * 解决角色黏到墙上的Bug：当角色离开地面时，将摩擦力设为0，进入时则设为1。
 * 通过改变cc.material属性实现。
 *
 * 清除状态累积：添加动画事件，在合适的动画帧上添加方法，清除信号
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System.Collections;
using DSWork.Global;
using DSWork.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UU_Lesson;

namespace DSWork {
	/// <summary>动作控制器</summary>
	public class ActionControl : MonoBehaviour {
		#region ［字段和属性］

		/// <summary>移动速度</summary>
		public float walkSpeed = 2f;
		/// <summary>奔跑速度倍率</summary>
		public float runMultiplier = 2.0f;

		/// <summary>翻滚速度</summary>
		public float rollSpeed_Z = 3f;
		/// <summary>跳跃速度</summary>
		public float jumpSpeed_Z = 2f;

		[Space(10)]
		[Header("===Friction Settings===")]
		public PhysicMaterial frictionOne;
		public PhysicMaterial frictionZero;

		/// <summary>附加向量</summary>
		private Vector3 thrustVec = Vector3.zero;

		/// <summary>重力加速度</summary>
		private const float gravity = 9.8f;
		private float fallTime;

		private GameObject model;
		private Animator animator;
		private CharacterController cc;
		private PlayerInput pi;
		private CameraControl cameraControl;

		/// <summary>锁死平面移动</summary>
		private bool lockMove;
		/// <summary>锁死攻击</summary>
		private bool lockAction;
		/// <summary>
		/// 追踪方向
		/// </summary>
		private bool trackDir;

		/// <summary>平面移动速度</summary>
		private Vector3 planerMoveSpeed;

		/// <summary>Root Motion相关</summary>
		private Vector3 deltaPos;

		/// <summary>插值目标，用于转换FSM层级</summary>
		private float lerpTarget;

		#endregion


		#region ［Unity消息］

		private void Awake() {
			//得到组件
			model = GameObject.FindWithTag("Player");
			animator = model.GetComponent<Animator>();
			cc = model.GetComponent<CharacterController>();
			pi = ScriptTool.GetScript<PlayerInput>("_Player");
			cameraControl = ScriptTool.GetScript<CameraControl>("_Camera");
			
			RegisterFSMEvents();
			
			//控制Root Motion
			model.GetComponent<RootMotionControl>().RMUpdateEvent += OnRMUpdate;
		}

		private void OnEnable() {
			StartCoroutine(PlayerActionCr());
		}

		#endregion


		#region ［角色控制方法］

		/// <summary>角色的动作协程</summary>
		/// <returns></returns>
		private IEnumerator PlayerActionCr() {
			yield return new WaitForEndOfFrame();
			while(true) {
				PlayerAction();
				PlayerMove();
				UseGravity();
				PlayerFallRoll();
				OnGroundCheck();
				yield return new WaitForSeconds(0.02f);
			}
		}


		/// <summary>角色的移动控制，整体的速度控制</summary>
		private void PlayerMove() {
			planerMoveSpeed = Vector3.zero;
			thrustVec = Vector3.zero;
			deltaPos = Vector3.zero;
			
			//如果没有处于锁定状态
			if(!cameraControl.LockState) {
				//设置FSM混合参数（在待机、移动、奔跑状态之间切换）
				//利用插值函数实现平滑过渡效果，每一次过渡一半。
				float tempMultiply = pi.DMag * Mathf.Lerp(animator.GetFloat(EPlayer.FSMParam.forward.TS()),pi.Sgn_Run.To2_1(), 0.5f);
				animator.SetFloat(EPlayer.FSMParam.forward.TS(), tempMultiply);
				animator.SetFloat(EPlayer.FSMParam.forward.TS(), 0);
				
				//设置方向（旋转）
				if(pi.DMag > 0.1f) {
					//球面线性插值，用于变换向量
					model.transform.forward = Vector3.Slerp(model.transform.forward, pi.DVec, 0.3f);
				}
				//设置玩家速度（二维）（考虑是否奔跑）（考虑是否锁定了平面移动）
				if(!lockMove) {
					planerMoveSpeed = pi.DMag * model.transform.forward * walkSpeed * (pi.Sgn_Run ? runMultiplier : 1.0f);
					planerMoveSpeed = new Vector3(planerMoveSpeed.x, 0, planerMoveSpeed.z);
				}
			}
			
			//如果处于锁定状态
			else {
				var localDVec = transform.InverseTransformVector(pi.DVec);
				animator.SetFloat(EPlayer.FSMParam.forward.TS(),localDVec.z * pi.Sgn_Run.To2_1());
				animator.SetFloat(EPlayer.FSMParam.right.TS(),localDVec.x * pi.Sgn_Run.To2_1());
				
				//设置方向（旋转）
				//如果不需要跟踪方向（使在闪避时，以人物的移动方向为基准）
				if(!trackDir) {
					model.transform.forward = cameraControl.CameraYForward;	
				}
				else {
					model.transform.forward = planerMoveSpeed.normalized;
				}
				
				//设置玩家速度（二维）
				if(!lockMove) {
					planerMoveSpeed = pi.DVec * walkSpeed * (pi.Sgn_Run ? runMultiplier : 1.0f);
				}
			}
		
			//Root Motion相关
			cc.transform.position += deltaPos;
			
			//玩家移动（考虑跳跃、翻滚）
			cc.Move((planerMoveSpeed + thrustVec) * Time.deltaTime);
		}

		/// <summary>角色的动作控制</summary>
		private void PlayerAction() {
			//锁定
			if(pi.Sgn_Lock)
				cameraControl.ToggleLock();
			
			//跳跃
			if(pi.Sgn_Jump)
				animator.SetTrigger(EPlayer.FSMParam.jump.TS());
			//闪避
			else if(pi.Sgn_Dodge)
				animator.SetTrigger(EPlayer.FSMParam.dodge.TS());

			//攻击
			if(pi.Sgn_RHandAct1 && !lockAction)
				animator.SetTrigger(EPlayer.FSMParam.attack.TS());
			//防御
			else if(pi.Sgn_LHandAct1 && !lockAction)
				animator.SetBool(EPlayer.FSMParam.defense.TS(), true);
			else if(pi.Sgn_LHandAct1 && !lockAction)
				animator.SetBool(EPlayer.FSMParam.defense.TS(), false);
		}

		
		/// <summary>角色的下落翻滚控制</summary>
		/// <remarks>如果下落速度很快，则需要在落地时进行一次翻滚</remarks>
		private void PlayerFallRoll() {
			if(cc.velocity.magnitude >= 4.0f)
				animator.SetTrigger(EPlayer.FSMParam.fallRoll.TS());
		}


		/// <summary>角色的重力控制</summary>
		private void UseGravity() {
			if(!cc.isGrounded) {
				fallTime += Time.deltaTime;
				cc.Move(Vector3.down * 1 / 2 * gravity * fallTime * Time.deltaTime);
			}
			else if(cc.isGrounded && fallTime <= 0) {
				fallTime = 0;
			}
		}

		/// <summary>着地检查</summary>
		public void OnGroundCheck() {
			if(!animator.GetBool(EPlayer.FSMParam.isOnGround.TS()) && cc.isGrounded)
				animator.SetBool(EPlayer.FSMParam.isOnGround.TS(), true);
			else if(animator.GetBool(EPlayer.FSMParam.isOnGround.TS()) && !cc.isGrounded)
				animator.SetBool(EPlayer.FSMParam.isOnGround.TS(), false);
		}

		#endregion


		#region ［FSM事件方法 仍然需要完善］

		/// <summary>注册FSM事件</summary>
		private void RegisterFSMEvents() {
			//后跃状态的相关事件
			animator.GetBehaviour<FSMEvents_Jab>().OnEnterEvent += () => {
				StateLock();
				trackDir = true;
			};
			animator.GetBehaviour<FSMEvents_Jab>().OnExitEvent += StateUnlock;
			animator.GetBehaviour<FSMEvents_Jab>().OnUpdateEvent += () =>
				StateSetSpeed(EPlayer.FSMCurve.spd_Jab_Y, EPlayer.FSMCurve.spd_Jab_Z);

			//跳跃状态的相关事件
			animator.GetBehaviour<FSMEvents_Jump>().OnEnterEvent += () => {
				StateLock();
				trackDir = true;
			};
			animator.GetBehaviour<FSMEvents_Jump>().OnUpdateEvent += () =>
				StateSetSpeed(EPlayer.FSMCurve.spd_Jump_Y, jumpSpeed_Z);
			animator.GetBehaviour<FSMEvents_Jump>().OnExitEvent += StateUnlock;

			//翻滚状态的相关事件
			animator.GetBehaviour<FSMEvents_Roll>().OnEnterEvent += () => {
				StateLock();
				trackDir = true;
			};
			animator.GetBehaviour<FSMEvents_Roll>().OnUpdateEvent += () =>
				StateSetSpeed(EPlayer.FSMCurve.spd_Roll_Y, rollSpeed_Z);
			animator.GetBehaviour<FSMEvents_Roll>().OnExitEvent += StateUnlock;

			//掉落状态的相关事件
			animator.GetBehaviour<FSMEvents_Fall>().OnEnterEvent += StateUnlock;

			//着地状态的相关事件
			animator.GetBehaviour<FSMEvents_OnGround>().OnEnterEvent += () => {
				StateUnlock();
				trackDir = false;
				cc.material = frictionOne;
			};
			animator.GetBehaviour<FSMEvents_OnGround>().OnExitEvent += () => { cc.material = frictionZero; };


			//攻击层级的待机状态的相关事件
			animator.GetBehaviour<FSMEvents_L1_Idle>().OnEnterEvent += () => StateInitChangeLayer(0.0f);
			animator.GetBehaviour<FSMEvents_L1_Idle>().OnUpdateEvent += () => StateChangeLayer(EPlayer.FSMLayer.AttackLayer);

			//单手攻击状态的相关事件
			animator.GetBehaviour<FSMEvents_L1_1Hand_Slash>().OnEnterEvent += () => StateInitChangeLayer(1.0f);
			animator.GetBehaviour<FSMEvents_L1_1Hand_Slash>().OnUpdateEvent += () => {
				StateSetSpeed(0f, EPlayer.FSMCurve.spd_Atk_1H_Slash1_Z);
				StateChangeLayer(EPlayer.FSMLayer.AttackLayer);
			};


			//防御层级的待机状态的相关事件
			animator.GetBehaviour<FSMEvents_L2_Idle>().OnEnterEvent += () => StateInitChangeLayer(0.0f);
			animator.GetBehaviour<FSMEvents_L2_Idle>().OnUpdateEvent += () => StateChangeLayer(EPlayer.FSMLayer.DefenseLayer);

			//单手防御状态的相关事件
			animator.GetBehaviour<FSMEvents_L2_1Hand_ShieldUp>().OnEnterEvent += () => StateInitChangeLayer(1.0f);
			animator.GetBehaviour<FSMEvents_L2_1Hand_ShieldUp>().OnUpdateEvent += () => StateChangeLayer(EPlayer.FSMLayer.DefenseLayer);
		}

		/// <summary>使输入失效，锁定攻击，且锁定平面移动</summary>
		private void StateLock() {
			pi.EnableInput = false;
			lockMove = true;
			lockAction = true;
		}

		/// <summary>重置冲量速度，使输入生效，解锁攻击，且解锁平面移动</summary>
		private void StateUnlock() {
			pi.EnableInput = true;
			lockMove = false;
			lockAction = false;
		}

		/// <summary>设置对应动画状态的Y轴曲线/固定速度，以及Z轴曲线/固定速度</summary>
		/// <param name="speed_y">Y轴固定速度</param>
		/// <param name="speed_z">Z轴固定速度</param>
		private void StateSetSpeed(float speed_y, float speed_z) {
			thrustVec = model.transform.up * speed_y + model.transform.forward * speed_z;
		}

		/// <summary>设置对应动画状态的Y轴曲线/固定速度，以及Z轴曲线/固定速度</summary>
		/// <param name="speed_y">Y轴固定速度</param>
		/// <param name="speed_z">Z轴曲线速度</param>
		private void StateSetSpeed(float speed_y, EPlayer.FSMCurve speed_z) {
			thrustVec = model.transform.up * speed_y + model.transform.forward * animator.GetFloat(speed_z.TS());
		}

		/// <summary>设置对应动画状态的Y轴曲线/固定速度，以及Z轴曲线/固定速度</summary>
		/// <param name="speed_y">Y轴曲线速度</param>
		/// <param name="speed_z">Z轴固定速度</param>
		private void StateSetSpeed(EPlayer.FSMCurve speed_y, float speed_z) {
			thrustVec = model.transform.up * animator.GetFloat(speed_y.TS()) + model.transform.forward * speed_z;
		}

		/// <summary>设置对应动画状态的Y轴曲线/固定速度，以及Z轴曲线/固定速度</summary>
		/// <param name="speed_y">Y轴曲线速度</param>
		/// <param name="speed_z">Z轴曲线速度</param>
		private void StateSetSpeed(EPlayer.FSMCurve speed_y, EPlayer.FSMCurve speed_z) {
			thrustVec = model.transform.up * animator.GetFloat(speed_y.TS()) + model.transform.forward * animator.GetFloat(speed_z.TS());
		}

		/// <summary>准备切换FSM层级（使用Lerp平滑切换）</summary>
		private void StateInitChangeLayer(float target) {
			//设置初始lerp目标
			lerpTarget = target;
		}

		/// <summary>切换FSM层级（使用Lerp平滑切换）</summary>
		/// <remarks>思路：将指定层级的权重平滑增加到1.0/0</remarks>
		/// <param name="layer">改变权重的层级</param>
		/// <param name="lerpTime">插值时间</param>
		private void StateChangeLayer(EPlayer.FSMLayer layer, float lerpTime = 0.4f) {
			//使用插值，平缓切换FSM层级
			float curWeight = animator.GetLayerWeight(animator.GetLayerIndex(layer.TS()));
			curWeight = Mathf.Lerp(curWeight, lerpTarget, lerpTime);
			animator.SetLayerWeight(animator.GetLayerIndex(layer.TS()), curWeight);
		}

		#endregion


		#region ［其他方法］

		/// <summary>检查当前动画状态</summary>
		/// <param name="stateName">指定的状态名称</param>
		/// <param name="layerName">指定的层级名称</param>
		/// <returns></returns>
		private bool CheckState(EPlayer.FSMState stateName, EPlayer.FSMLayer layerName = EPlayer.FSMLayer.BaseLayer) {
			int layerIndex = animator.GetLayerIndex(layerName.TS());
			bool result = animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName.TS());
			return result;
		}

		/// <summary>更新Root Motion</summary>
		/// <remarks>仅在第三段攻击时应用</remarks>
		/// <param name="deltaPosObj"></param>
		private void OnRMUpdate(object deltaPosObj) {
			if(CheckState(EPlayer.FSMState.__1Hand_Slash3, EPlayer.FSMLayer.AttackLayer))
				deltaPos += 0.5f * deltaPos + 0.5f * (Vector3) deltaPosObj;
		}

		#endregion
	}
}