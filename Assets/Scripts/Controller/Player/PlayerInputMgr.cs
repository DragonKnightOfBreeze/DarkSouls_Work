//DONE：解决斜向移动时的速度更快的问题
//TODO：转向应该和四元数挂钩，而不是向量！
//Done：将状态信号由布尔值改为枚举
//Done：使用输入枚举
//TODO：检查坐标轴输入是否匹配

/*******
 * ［概述］
 * 
 * 玩家输入模块
 * 
 * ［用法］
 * 挂载到PlayerContainer上面
 * 
 * 
 * ［备注］
 * 跑步：按住Ctrl
 * 跳跃：跑步时按下Space
 * 翻滚：按下Space
 * SmoothDamp：平滑增加
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */
using System;
using System.Collections;
using System.Collections.Generic;
using DSWork.Global;
using DSWork.InputEX;
using DSWork.Utility;
using UnityEngine;

namespace DSWork {
	/// <summary>玩家的输入管理器</summary>
	public class PlayerInputMgr : MonoBehaviour {
		private readonly Dictionary<EInput, IInputEX> KInputDict = new Dictionary<EInput, IInputEX>();
		private readonly Dictionary<EInput, IInputEX> JInputDict = new Dictionary<EInput, IInputEX>();

		# region ［输入信号］

		/// <summary>持续信号：玩家前后移动</summary>
		[Header("【输入信号】")]
		[SerializeField]
		private float Sgn_Forward;
		/// <summary>持续信号：左右移动</summary>
		[SerializeField]
		private float Sgn_Right;
		/// <summary>持续信号：视角上下移动</summary>
		public float Sgn_VUp;
		/// <summary>持续信号：视角左右移动</summary>
		public float Sgn_VRight;

		/// <summary>点击信号：切换魔法</summary>
		public bool Sgn_ToggleMagic;
		/// <summary>点击信号：切换道具</summary>
		public bool Sgn_ToggleItem;
		/// <summary>点击信号：切换左手武器</summary>
		public bool Sgn_ToggleLHand;
		/// <summary>点击信号：切换右手武器</summary>
		public bool Sgn_ToggleRHand;

		/// <summary>持续信号：奔跑</summary>
		public bool Sgn_Run;
		/// <summary>双击信号：闪避（后跃/翻滚）</summary>
		public bool Sgn_Dodge;
		/// <summary>跳跃信号</summary>
		public bool Sgn_Jump;
		/// <summary>点击信号： 使用物品</summary>
		public bool Sgn_UseItem;
		/// <summary>点击信号：互动</summary>
		public bool Sgn_Interact;
		/// <summary>切换武器的持有方式</summary>
		public bool Sgn_ToggleHold;

		/// <summary>点击/持续信号：左手主要动作（一般为防御）</summary>
		public bool Sgn_LHandAct1;
		/// <summary>点击/持续信号：左手次要动作（一般为战技）</summary>
		public bool Sgn_LHandAct2;
		/// <summary>点击/持续信号：右手主要动作（一般为普通攻击）</summary>
		public bool Sgn_RHandAct1;
		/// <summary>点击/持续信号：右手次要动作（一般为重攻击）</summary>
		public bool Sgn_RHandAct2;

		/// <summary>持续信号：静步</summary>
		public bool Sgn_Walk;
		/// <summary>持续信号：目标锁定/视角重置</summary>
		public bool Sgn_Lock;
		/// <summary>点击信号：打开菜单</summary>
		public bool Sgn_Menu;
		/// <summary>点击信号：打开次要菜单</summary>
		public bool Sgn_SecMenu;

		#endregion


		#region ［变量和属性］

		[Header("【处理后的信号】")]
		public float Forward;
		public float Right;
		/// <summary>移动距离（移动方向向量的模长，原点到二维信号的距离）</summary>
		public float Distance;
		/// <summary>移动方向向量（世界坐标系）</summary>
		public Vector3 Direction;

		/// <summary>是否启用模块</summary>
		[Header("【其他】")]
		public bool LockInput;
		/// <summary>是否使用键鼠</summary>
		public bool UseKeyboard = true;
		/// <summary>是否使用手柄</summary>
		public bool UseJoystick;


//		private GameObject model;
		private PlayerCameraCtrl cameraCtrl;

		private float velocityDForward;
		private float velocityDRight;
		private readonly float smoothTime = 0.1f;

		#endregion


		private void Awake() {
			InitInputDict();

			cameraCtrl = gameObject.GetComponent<PlayerCameraCtrl>();
		}

		private void OnEnable() {
			StartCoroutine(GetInputCr());
		}


		#region ［得到用户输入］

		/// <summary>初始化输入字典</summary>
		private void InitInputDict() {
			foreach(EInput ev in Enum.GetValues(typeof(EInput)))
				if(ev.TS().StartsWith("K_")) {
					if(ev.TS().EndsWith("Axis"))
						KInputDict.Add(ev, new AxisEX(ev));
					else
						KInputDict.Add(ev, new ButtonEX(ev));
				}
				else {
					if(ev.TS().EndsWith("Axis"))
						JInputDict.Add(ev, new AxisEX(ev));
					else
						JInputDict.Add(ev, new ButtonEX(ev));
				}
		}

		/// <summary>协程：得到用户输入</summary>
		/// <returns></returns>
		private IEnumerator GetInputCr() {
			while(true) {
				yield return new WaitForSeconds(0.02f);
				if(LockInput)
					continue;

				//遍历计时，然后得到用户输入
				if(UseKeyboard) {
					foreach(var input in KInputDict)
						input.Value.Tick();
					GetInput_KM();
				}
				if(UseJoystick) {
					foreach(var input in JInputDict)
						input.Value.Tick();
					GetInput_JS();
				}
				HandleInput();

				print("防御：" + KInputDict[EInput.K_LHandAct1].Press);
				print("攻击：" + KInputDict[EInput.K_RHandAct1].PressDown);
			}
		}


		/// <summary>得到用户输入（键鼠）</summary>
		private void GetInput_KM() {
			Sgn_Forward = KInputDict[EInput.K_Forward].AxisValue - KInputDict[EInput.K_Back].AxisValue;
			Sgn_Right = KInputDict[EInput.K_Right].AxisValue - KInputDict[EInput.K_Left].AxisValue;
			Sgn_VUp = KInputDict[EInput.K_VUpAxis].AxisValue * GlobalSetting.CameraRotationSpeed;
			Sgn_VRight = KInputDict[EInput.K_VRightAxis].AxisValue * GlobalSetting.CameraRotationSpeed;

			Sgn_Run = KInputDict[EInput.K_Run].FixedPress(0.2f, 0.2f);
			Sgn_Jump = KInputDict[EInput.K_Dodge].DoubleClick();
			Sgn_Dodge = KInputDict[EInput.K_Jump].QuickClick();
			Sgn_Interact = KInputDict[EInput.K_Interact].PressDown;
			Sgn_UseItem = KInputDict[EInput.K_UseItem].PressDown;
			Sgn_ToggleHold = KInputDict[EInput.K_ToggleHold].PressDown;
			//TODO：重构：信号的触发方式也是不固定的，与动作和技能有关
			Sgn_LHandAct1 = KInputDict[EInput.K_LHandAct1].Press;     //防御
			Sgn_LHandAct2 = KInputDict[EInput.K_LHandAct2].PressDown; //武器战技
			Sgn_RHandAct1 = KInputDict[EInput.K_RHandAct1].PressDown; //普通攻击	
			Sgn_RHandAct2 = KInputDict[EInput.K_RHandAct2].Press;     //重攻击
			Sgn_Walk = KInputDict[EInput.K_Walk].PressDown;
			Sgn_Lock = KInputDict[EInput.K_Lock].PressDown;
			Sgn_Menu = KInputDict[EInput.K_Menu].PressDown;
			Sgn_SecMenu = KInputDict[EInput.K_SecMenu].PressDown;
		}


		/// <summary>得到用户输入（手柄）</summary>
		private void GetInput_JS() {
			Sgn_Forward = JInputDict[EInput.J_ForwardAxis].AxisValue;
			Sgn_Right = JInputDict[EInput.J_RightAxis].AxisValue;
			Sgn_VUp = JInputDict[EInput.J_VUpAxis].AxisValue * GlobalSetting.CameraRotationSpeed;
			Sgn_VRight = JInputDict[EInput.J_VRightAxis].AxisValue * GlobalSetting.CameraRotationSpeed;

			Sgn_Run = JInputDict[EInput.J_Run].FixedPress(0.2f, 0.2f);
			Sgn_Jump = JInputDict[EInput.J_Dodge].DoubleClick();
			Sgn_Dodge = JInputDict[EInput.J_Jump].QuickClick();
			Sgn_Interact = JInputDict[EInput.J_Interact].PressDown;
			Sgn_UseItem = JInputDict[EInput.J_UseItem].PressDown;
			Sgn_ToggleHold = JInputDict[EInput.J_ToggleHold].PressDown;
			//TODO：重构：信号的触发方式也是不固定的，与动作和技能有关
			Sgn_LHandAct1 = JInputDict[EInput.J_LHandAct1].Press;     //防御
			Sgn_LHandAct2 = JInputDict[EInput.J_LHandAct2].PressDown; //武器战技
			Sgn_RHandAct1 = JInputDict[EInput.J_RHandAct1].PressDown; //普通攻击	
			Sgn_RHandAct2 = KInputDict[EInput.J_RHandAct2].Press;     //重攻击
			Sgn_Walk = JInputDict[EInput.J_Walk].PressDown;
			Sgn_Lock = JInputDict[EInput.J_Lock].PressDown;
			Sgn_Menu = JInputDict[EInput.J_Menu].PressDown;
			Sgn_SecMenu = JInputDict[EInput.J_SecMenu].PressDown;
		}

		#endregion


		#region ［其他私有方法］

		/// <summary>处理用户输入</summary>
		/// <param name="smooth">是否需要进行平滑过渡</param>
		/// <param name="toCircle">是否需要转化为圆形坐标，使对角线长度最大也为1</param>
		private void HandleInput(bool smooth = true, bool toCircle = true) {
			if(smooth) {
				Forward = Mathf.SmoothDamp(Forward, Sgn_Forward, ref velocityDForward, smoothTime);
				Right = Mathf.SmoothDamp(Right, Sgn_Right, ref velocityDRight, smoothTime);
			}
			if(toCircle)
				SquareToCircle(ref Forward, ref Right);

			Distance = Mathf.Sqrt(Forward * Forward + Right * Right);
			//DONE：参考系的正方向应该是相机的正方向
			Direction = cameraCtrl.CameraForward * Forward + cameraCtrl.CameraYRight * Right;
		}

		/// <summary>将正方形坐标转化成圆形坐标</summary>
		private void SquareToCircle(ref float x, ref float y) {
			float rawX = x;
			float rawY = y;
			x = rawX * Mathf.Sqrt(1 - rawY * rawY / 2.0f);
			y = rawY * Mathf.Sqrt(1 - rawX * rawX / 2.0f);
		}

		#endregion
	}
}