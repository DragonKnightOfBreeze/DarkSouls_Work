//TODO：OnEnter、OnExit方法可以在这个脚本中，注册到对应事件上。
//TODO：改为有限状态机（移动、跳跃、跑动、翻滚等）

/*******
 * ［概述］
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 * 
 * 动画控制器控制
 * 
 * ［功能］
 * 
 * 
 * ［思路］ 
 *
 *
 * ［备注］
 * Physics.OverlapCapsule()：检测胶囊体的碰撞
 *
 * 更好的控制特殊FSM状态时的角色的位移速度的方法
 * 通过FSM参数找到动画模型中的曲线的引用，然后设置为位移速度
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using DSWork.Global;
using UnityEditor.Animations;
using UnityEngine;

namespace DSWork {
	/// <summary> 
	/// 动画控制器控制
	/// </summary>
	public class ActionController : MonoBehaviour {
		public GameObject Model;
		/// <summary>移动速度</summary>
		public float walkSpeed = 2f;
		/// <summary>奔跑速度倍率</summary>
		public float runMultiplier = 2.0f;

		/// <summary>
		/// 后跃速度
		/// </summary>
		public float jabSpeed = 4f;
		/// <summary>翻滚速度</summary>
		public float rollSpeed = 4f;
		/// <summary>跳跃速度</summary>
		public float jumpSpeed = 2f;
		
		private Vector3 dodgeVec = Vector3.zero;
		

		/// <summary>
		/// 重力加速度
		/// </summary>
		private float gravity =9.8f;
		private float fallTime = 0f;
		
		private Animator animator;
		private PlayerInput pi;
		private CharacterController cc;

		/// <summary>锁死平面移动速度</summary>
		private bool lockPlanar =false;
 
		/// <summary>平面移动速度</summary>
		private Vector3 planerMoveSpeed;

//		private float smallTime = 5f;
		
		


		void Awake() {
			//得到组件
			animator = Model.GetComponent<Animator>() ?? Model.AddComponent<Animator>();
			pi = Model.GetComponent<PlayerInput>() ?? Model.AddComponent<PlayerInput>();
			cc = Model.GetComponent<CharacterController>() ?? Model.AddComponent<CharacterController>();
			
			RegisterFSMEvents();
		}

		//RigidBody不应该在update方法中调用
		//rigid.position = planerMoveSpeed * Time.fixedDeltaTime;
		//rigid.velocity = planerMoveSpeed; 	//会覆盖y值


		private void OnEnable() {
			StartCoroutine(PlayerActionCr());
		}

		/// <summary>
		/// 角色的动作协程
		/// </summary>
		/// <returns></returns>
		IEnumerator PlayerActionCr() {
			yield return new WaitForEndOfFrame();
			while(true) {
				PlayerDodge_Roll_Jump();
				PlayerFallRoll();
				PlayerMove();
				UseGravity();
				OnGroundCheck();
				yield return new WaitForSeconds(0.02f);
			}
		}

		/// <summary>
		/// 角色的移动控制，整体的速度控制
		/// </summary>
		private void PlayerMove() {
			//设置混合参数（在待机、移动、奔跑状态之间切换）
			//利用插值函数实现平滑过渡效果，每一次过渡一半。
			float targetRunMulti = pi.DMag * Mathf.Lerp(animator.GetFloat(PlayerFSMParam.forward.ToString()), pi.run ? 2.0f : 1.0f, 0.5f);
			animator.SetFloat(PlayerFSMParam.forward.ToString(), targetRunMulti);

			//设置方向（旋转）
			if(pi.DMag > 0.1f) {
				//球面线性插值，用于变换向量
				var targetForward = Vector3.Slerp(Model.transform.forward, pi.DVec, 0.3f);
				Model.transform.forward = targetForward;
			}

			//设置玩家速度（二维）（考虑是否奔跑）（考虑是否锁定了平面移动）
			if(!lockPlanar) {
				var tempMoveSpeed = pi.DMag * Model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);
				planerMoveSpeed = new Vector3(tempMoveSpeed.x,0, planerMoveSpeed.z);
			}
			else {
				planerMoveSpeed = Vector3.zero;
			}
			

			//玩家移动（考虑跳跃、翻滚）
			cc.Move((planerMoveSpeed + dodgeVec) * Time.deltaTime);
		}


		/// <summary>
		/// 角色的跳跃控制
		/// </summary>
		private void PlayerDodge_Roll_Jump() {
			if(pi.jab_roll_jump)
				animator.SetTrigger(PlayerFSMParam.jab_roll_jump.ToString());
			
			
//			if(pi.jump) {
////				pi.jump = false;
//				animator.SetTrigger("jump");
////				smallTime = 0;
//			}
//			if(GetSmallTime(jumpBoostTime)) {
//				cc.Move(Vector3.up * (jumpHeight + gravity) * Time.deltaTime);
//			}
//			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
//				cc.Move(Vector3.up *(jumpHeight+gravity)* Time.deltaTime);

			//if(pi.jump)	smallTime = 0;
			//if(GetSmallTime(length))	cc.Move(Vector3.up*Time.deltaTime);
		}

		/// <summary>
		/// 角色的翻滚控制
		/// </summary>
		private void PlayerFallRoll() {
			//如果下落速度很快，则需要在落地时进行一次翻滚
			if(cc.velocity.magnitude >= 2.0f) {
				animator.SetTrigger(PlayerFSMParam.fallRoll.ToString());
			}
		}
		
		
		/// <summary>
		/// 角色的重力控制
		/// </summary>
		private void UseGravity() {
			if(!cc.isGrounded) {
				fallTime += Time.deltaTime;
				cc.Move(Vector3.down * 1/2 * gravity * fallTime*Time.deltaTime);
			}
			else if(cc.isGrounded && fallTime <= 0) {
				fallTime = 0;
			}
		}



		/// <summary>
		/// 注册FSM事件
		/// </summary>
		private void RegisterFSMEvents() {
//			var fsmOnEnter = animator.GetBehaviour<FSMOnEnter>();
//			var fsmOnExit = animator.GetBehaviour<FSMOnExit>();
//			fsmOnEnter.FSMOnEnterEvent += OnJumpEnter;
//			fsmOnEnter.FSMOnEnterEvent += OnGroundEnter;

			animator.GetBehaviour<FSMEvents_Jump>().OnEnterEvent += OnJumpEnter;
			animator.GetBehaviour<FSMEvents_Jump>().OnExitEvent += OnJumpExit;
			animator.GetBehaviour<FSMEvents_Jump>().OnUpdateEvent += OnJumpUpdate;
			
			animator.GetBehaviour<FSMEvents_Roll>().OnEnterEvent += OnRollEnter;
			animator.GetBehaviour<FSMEvents_Roll>().OnExitEvent += OnRollExit;
			animator.GetBehaviour<FSMEvents_Roll>().OnUpdateEvent += OnRollUpdate;
			
			animator.GetBehaviour<FSMEvents_Jab>().OnEnterEvent += OnJabEnter;
			animator.GetBehaviour<FSMEvents_Jab>().OnExitEvent += OnJabExit;
			animator.GetBehaviour<FSMEvents_Jab>().OnUpdateEvent += OnJabUpdate;

			animator.GetBehaviour<FSMEvents_OnGround>().OnEnterEvent += OnGroundEnter;
			animator.GetBehaviour<FSMEvents_Fall>().OnEnterEvent += OnFallEnter;
		}


		#region ［FSM事件方法 仍然需要完善］

		


		
//		private void OnJumpEnter(int nameHash) {
//			if(Animator.StringToHash("Jump") != nameHash)
//				return;
//			print("1");
//			pi.inputEnabled = false;
//			lockPlanar = true;
//		}
//		
//		private void OnGroundEnter(int nameHash) {
//			if(Animator.StringToHash("OnGround") != nameHash)
//				return;
//			//解锁移动
//			//应该在角色重新落回地面时解除锁定，而不是在跳跃动画结束时
//			print("2");
//			pi.inputEnabled = true;
//			lockPlanar = false;
//		}

		private void OnJabEnter() {
			pi.inputEnabled = false;
			lockPlanar = true;
		}	
		private void OnJabExit() {
			dodgeVec = Vector3.zero;
			pi.inputEnabled = true;
			lockPlanar = false;
		}
		private void OnJabUpdate() {
			//更好的控制特殊FSM状态时的角色的位移速度的方法
			//通过FSM参数找到动画模型中的曲线的引用，然后设置为位移速度
			dodgeVec = Model.transform.up * animator.GetFloat(PlayerFSMCurve.jabSpeed_Y.ToString()) + Model.transform.forward* jabSpeed;
		}
		
		private void OnRollEnter() {
			pi.inputEnabled = false;
			lockPlanar = true;
		}	
		private void OnRollExit() {
			dodgeVec = Vector3.zero;
			pi.inputEnabled = true;
			lockPlanar = false;
		}
		private void OnRollUpdate() {
			dodgeVec = Model.transform.up * animator.GetFloat(PlayerFSMCurve.rollSpeed_Y.ToString()) + Model.transform.forward* rollSpeed;
		}
		
		private void OnJumpEnter() {
			pi.inputEnabled = false;
			lockPlanar = true;
		}
		private void OnJumpExit() {
			dodgeVec = Vector3.zero;
			pi.inputEnabled = true;
			lockPlanar = false;
		}
		private void OnJumpUpdate() {
			dodgeVec = Model.transform.up * animator.GetFloat(PlayerFSMCurve.jumpSpeed_Y.ToString()) + Model.transform.forward* jumpSpeed;
		}
		
		
		private void OnGroundEnter() {
			pi.inputEnabled = true;
			lockPlanar = false;
		}

		private void OnFallEnter() {
			pi.inputEnabled = true;
			lockPlanar = false;
		}
		
		
//		public void IsOnGround() {
//			animator.SetBool("isOnGround",true);
//		}
//		
//		public void IsNotOnGround() {
//			animator.SetBool("isOnGround",false);
//		}

	
		public void OnGroundCheck() {
			if(!animator.GetBool(PlayerFSMParam.isOnGround.ToString()) && cc.isGrounded) {
				animator.SetBool(PlayerFSMParam.isOnGround.ToString(),true);
			}
			else if(animator.GetBool(PlayerFSMParam.isOnGround.ToString()) && !cc.isGrounded) {
				animator.SetBool(PlayerFSMParam.isOnGround.ToString(), false);
			}
		}

		#endregion

//		private bool CheckNextStateName(string stateName) {
//			bool result = animator.GetNextAnimatorStateInfo(0).IsName(stateName);
//			return result;
//		}
//		
//		
//		private bool CheckCurStateName(string stateName) {
//			bool result = animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
// 			return result;
//		}
		
//		//使代码段在触发后一定时间内仍然有效。
//		private bool GetSmallTime(float length){
//			if(smallTime <= length){
//				smallTime += Time.deltaTime;
//				return true;
//			}
//			return false;
//		}		
	}
}