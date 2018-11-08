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
 *
 * 挂载到_Scripts/_Player上。
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
using DSWork.Utility;
using UnityEngine;

namespace DSWork {
	/// <summary>玩家输入模块</summary>
	public class PlayerInput : MonoBehaviour {
		[HideInInspector]
		public Dictionary<KButton, IMyButton> KButtonDict;
		[HideInInspector]
		public Dictionary<KAxis, IMyAxis> KAxisDict;
		[HideInInspector]
		public Dictionary<JButton, IMyButton> JButtonDict;
		[HideInInspector]
		public Dictionary<JAxis, IMyAxis> JAxisDict;

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
		public float DForward;
		public float DRight;
		/// <summary>移动方向向量的模长，原点到二维信号的距离</summary>
		public float DMag;
		/// <summary>移动方向向量</summary>
		public Vector3 DVec;

		/// <summary>是否启用模块</summary>
		[Header("【其他】")]
		public bool EnableInput = true;
		/// <summary>是否使用键鼠</summary>
		public bool UseKM = true;

		private GameObject model;
		
		private float velocityDForward;
		private float velocityDRight;
		private readonly float smoothTime = 0.1f;

		

		#endregion

		
		private void Awake() {
			InitInputDict();
			model = GameObject.FindWithTag(Tag.Player.TS());
		}

		private void OnEnable() {
			StartCoroutine(GetInputCr());
		}


		#region ［得到用户输入］
		
		/// <summary>
		/// 初始化输入字典
		/// </summary>
		private void InitInputDict() {
			KAxisDict = new Dictionary<KAxis, IMyAxis> {
				[KAxis.Forward] = new MyAxis(KAxis.Forward.TS(),KAxis.Back.TS()),
				[KAxis.Right] = new MyAxis(KAxis.Right.TS(),KAxis.Left.TS()),
				[KAxis.VUp] = new MyAxis(KAxis.VUp.TS(),KAxis.VDown.TS()),
				[KAxis.VRight] = new MyAxis(KAxis.VRight.TS(),KAxis.VLeft.TS())
			};
			
			KButtonDict = new Dictionary<KButton, IMyButton>();
			foreach(KButton ev in Enum.GetValues(typeof(KButton)))
				KButtonDict.Add(ev,new MyButton(ev.TS()));
			
			JAxisDict = new Dictionary<JAxis, IMyAxis>();
			foreach(JAxis ev in Enum.GetValues(typeof(JAxis)))
				JAxisDict.Add(ev,new MyAxis(ev.TS()));
			
			JButtonDict = new Dictionary<JButton, IMyButton>();
			foreach(JButton ev in Enum.GetValues(typeof(JButton)))
				JButtonDict.Add(ev,new MyButton(ev.TS()));
		}
		
		/// <summary>
		/// 协程：得到用户输入
		/// </summary>
		/// <returns></returns>
		private IEnumerator GetInputCr() {
			while(true) {
				yield return new WaitForSeconds(0.02f);
				if(!EnableInput)
					continue;
				
				//遍历计时，然后得到用户输入
				if(UseKM) {
					foreach(var axis in KAxisDict)
						axis.Value.Tick();
					foreach(var button in KButtonDict)
						button.Value.Tick();
					GetInput_KM();
				}
				else {
					foreach(var axis in KAxisDict)
						axis.Value.Tick();
					foreach(var button in JButtonDict)
						button.Value.Tick();
					GetInput_JS();
				}
				HandleInput();

			}
		}


		/// <summary>得到用户输入（键鼠）</summary>
		private void GetInput_KM() {
			//处理输入信号
			Sgn_Forward = KAxisDict[KAxis.Forward].AxisValue;
			Sgn_Right = KAxisDict[KAxis.Right].AxisValue;
			Sgn_VUp = KAxisDict[KAxis.VUp].AxisValue;
			Sgn_VRight = KAxisDict[KAxis.VRight].AxisValue;

			Sgn_Run = ((MyButton)KButtonDict[KButton.Run]).FullDelayPress();
			Sgn_Jump = KButtonDict[KButton.Dodge].DoublePress();
			Sgn_Dodge = KButtonDict[KButton.Jump].QuickPress();
			Sgn_Interact = KButtonDict[KButton.Interact].PressDown;
			Sgn_UseItem = KButtonDict[KButton.UseItem].PressDown;
			Sgn_ToggleHold = KButtonDict[KButton.ToggleHold].PressDown;
			//TODO：重构：信号的触发方式也是不固定的，与动作和技能有关
			Sgn_LHandAct1 = KButtonDict[KButton.LHandAct1].Press;     //防御
			Sgn_LHandAct2 = KButtonDict[KButton.LHandAct2].PressDown; //武器战技
			Sgn_RHandAct1 = KButtonDict[KButton.RHandAct1].PressDown; //普通攻击	
			Sgn_RHandAct2 = KButtonDict[KButton.RHandAct2].Press;     //重攻击

			Sgn_Walk = KButtonDict[KButton.Walk].PressDown;
			Sgn_Lock = KButtonDict[KButton.Lock].PressDown;
			Sgn_Menu = KButtonDict[KButton.Menu].PressDown;
			Sgn_SecMenu = KButtonDict[KButton.SecMenu].PressDown;
		}


		/// <summary>得到用户输入（手柄）</summary>
		private void GetInput_JS() {
			if(!EnableInput)
				return;

			//计算二维信号的目标位置
			Sgn_Forward =  JAxisDict[JAxis.J_Forward_Back].AxisValue;
			Sgn_Right = JAxisDict[JAxis.J_Right_Left].AxisValue;
			//计算视角移动的信号
			Sgn_VUp = JAxisDict[JAxis.J_VUp_Down].AxisValue * GlobalSetting.CameraRotationSpeed;
			Sgn_VRight = JAxisDict[JAxis.J_VRight_Left].AxisValue * GlobalSetting.CameraRotationSpeed;

			//计算其他信号
			Sgn_Run = Input.GetButton(JButton.J_Run.TS());
			Sgn_Dodge = Input.GetButtonUp(JButton.J_Run.TS());
			Sgn_Interact = Input.GetButtonDown(JButton.J_Dodge.TS());
			Sgn_UseItem = Input.GetButtonDown(JButton.J_Interact.TS());
			Sgn_ToggleHold = Input.GetButtonDown(JButton.J_ToggleHold.TS());

			Sgn_RHandAct1 = Input.GetButtonDown(JButton.J_LHandAct2.TS());
			Sgn_LHandAct1 = Input.GetButton(JButton.J_LHandAct1.TS());
			//TODO：补充代码
		}

		#endregion


		#region ［其他私有方法］

		/// <summary>处理用户输入</summary>
		private void HandleInput() {
			if(!EnableInput)
				return;

			//计算真正的二维信号（对于目标位置，按照移动速度）
			DForward = Mathf.SmoothDamp(DForward, Sgn_Forward, ref velocityDForward, smoothTime);
			DRight = Mathf.SmoothDamp(DRight, Sgn_Right, ref velocityDRight, smoothTime);

			//将正方形坐标转化成圆形坐标，使对角线最长也是1.0
			var tempDAxis = SquareToCircle(new Vector2(DRight, DForward));
			DForward = tempDAxis.y;
			DRight = tempDAxis.x;

			DMag = Mathf.Sqrt(DForward * DForward + DRight * DRight);
			//TODO：如果要和黑魂中的设计一致，仅需这里，参考的正方向应该是相机的正方向
			//cameraControl.CameraYForward
			DVec = DRight * model.transform.right + DForward * model.transform.forward;
		}


		/// <summary>将正方形坐标转化成圆形坐标</summary>
		private Vector2 SquareToCircle(Vector2 input) {
			var output = Vector2.zero;
			//使用公式
			output.x = input.x * Mathf.Sqrt(1 - input.y * input.y / 2.0f);
			output.y = input.y * Mathf.Sqrt(1 - input.x * input.x / 2.0f);
			return output;
		}

		#endregion
	}
}