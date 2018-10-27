//DONE：解决斜向移动时的速度更快的问题
//TODO：转向应该和四元数挂钩，而不是向量！
//TODO：将状态信号由布尔值改为枚举
//TODO：使用输入枚举

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
using System.Collections;
using DSWork.Global;
using DSWork.Utils;
using UnityEngine;

namespace DSWork {
	/// <summary>玩家输入模块</summary>
	public class PlayerInput : MonoBehaviour {
		
		# region ［输入信号］
		
		/// <summary>持续信号：玩家前后移动</summary>
		[Header("===Input Signals===")]
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
		/// <summary>双击信号：闪避（后跃/翻滚/跳跃）</summary>
		public bool Sgn_Dodge;
		/// <summary>点击信号： 使用物品</summary>
		public bool Sgn_UseItem;
		/// <summary>点击信号：互动</summary>
		public bool Sgn_Interact;

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

		[Header("===Handled Signals===")]
		public float DForward;
		public float DRight;
		/// <summary>原点到二维信号的距离</summary>
		public float DMag;
		/// <summary>移动方向向量</summary>
		public Vector3 DVec;

		/// <summary>是否启用模块</summary>
		[Header("===Others===")]
		public bool inputEnabled = true;

		//二维信号的移动速度
		private float velocityDForward;
		private float velocityDRight;
		//平滑变换时间
		private readonly float smoothTime = 0.1f;

		#endregion
		

		private void OnEnable() {
			StartCoroutine(GetInputCr());
		}

		
		#region ［私有方法］
		
		private IEnumerator GetInputCr() {
			while(true) {
				//GetInput_KM();
				GetInput_JS();
				HandleInput();
				yield return new WaitForSeconds(0.02f);
			}
		}
		
		
		/// <summary>得到用户输入（键鼠）</summary>
		private void GetInput_KM() {
			if(!inputEnabled)
				return;

			//计算二维信号的目标位置
			Sgn_Forward = (Input.GetKey(KMInput.KW_Forward.TS()) ? 1.0f : 0) - (Input.GetKey(KMInput.KW_Back.TS()) ? 1.0f : 0);
			Sgn_Right = (Input.GetKey(KMInput.KW_Right.TS()) ? 0.5f : 0) - (Input.GetKey(KMInput.KW_Left.TS()) ? 0.5f : 0);
			//计算视角移动的信号
			Sgn_VUp = (Input.GetKey(KMInput.KW_VUp.TS()) ? 1.0f : 0) - (Input.GetKey(KMInput.KW_VDown.TS()) ? 1.0f : 0);
			Sgn_VRight = (Input.GetKey(KMInput.KW_VRight.TS()) ? 1.0f : 0) - (Input.GetKey(KMInput.KW_VLeft.TS()) ? 1.0f : 0);

			//计算其他信号
			Sgn_Run = Input.GetKey(KMInput.KW_Run.TS());
			Sgn_Dodge = Input.GetKeyDown(KMInput.KW_Dodge.TS());
			
			Sgn_RHandAct1 = Input.GetMouseButtonDown(0);	//普通攻击
			Sgn_LHandAct1 = Input.GetMouseButton(1);		//防御
			//TODO：补充代码
		}


		/// <summary>得到用户输入（手柄）</summary>
		private void GetInput_JS() {
			if(!inputEnabled)
				return;

			//计算二维信号的目标位置
			Sgn_Forward = Input.GetAxis(JSInput.JS_LeftAxis_Y.TS());
			Sgn_Right = Input.GetAxis(JSInput.JS_LeftAxis_Y.TS());
			//计算视角移动的信号
			Sgn_VUp = -1 * Input.GetAxis(JSInput.JS_RightAxis_Y.TS());
			Sgn_VRight = Input.GetAxis(JSInput.JS_RightAxis_X.TS());

			//计算其他信号
			Sgn_Run = Input.GetButton(JSInput.JS_Button0.TS());
			Sgn_Dodge = Input.GetButtonUp(JSInput.JS_Button0.TS());

			Sgn_RHandAct1 = Input.GetButtonDown(JSInput.JS_RT.TS());
			//TODO：补充代码
		}


		/// <summary>处理用户输入</summary>
		private void HandleInput() {
			if(!inputEnabled)
				return;

			//计算真正的二维信号（对于目标位置，按照移动速度）
			DForward = Mathf.SmoothDamp(DForward, Sgn_Forward, ref velocityDForward, smoothTime);
			DRight = Mathf.SmoothDamp(DRight, Sgn_Right, ref velocityDRight, smoothTime);

			//将正方形坐标转化成圆形坐标，使对角线最长也是1.0
			var tempDAxis = SquareToCircle(new Vector2(DRight, DForward));
			DForward = tempDAxis.y;
			DRight = tempDAxis.x;

			DMag = Mathf.Sqrt(DForward * DForward + DRight * DRight);
			DVec = DRight * transform.right + DForward * transform.forward;
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