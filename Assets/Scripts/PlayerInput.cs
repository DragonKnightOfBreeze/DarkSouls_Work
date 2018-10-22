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
using UnityEngine;

namespace DSWork {
	/// <summary>玩家输入模块</summary>
	public class PlayerInput : MonoBehaviour {
		//按键的具体设置
		[Header("===Key settings===")]
		//人物移动
		public string keyForward = "w";
		public string keyBack = "s";
		public string keyLeft = "a";
		public string keyRight = "d";

		public string keyA = "left ctrl";
		public string keyB = "space";
		public string keyC = "e";
		public string keyD = "f";


		//视角移动（后期要替换成用鼠标的位移控制）
		public string KeyVUp = "up";
		public string KeyVDown = "down";
		public string KeyVLeft = "left";
		public string KeyVRight = "right";


		//二维信号
		[Header("===Output Signals===")]
		public float DForward;
		public float DRight;
		/// <summary>原点到二维信号的距离</summary>
		public float DMag;
		/// <summary>移动方向</summary>
		public Vector3 DVec;

		public float VUp;
		public float VRight;

		//按压信号
		/// <summary>跑步信号</summary>
		public bool run;
		//单击触发信号
		/// <summary>后跃/翻滚/跳跃信号</summary>
		public bool jab_roll_jump;
//		private bool lastJump = false;

		public bool attack;


		/// <summary>是否启用模块</summary>
		[Header("===Others===")]
		public bool inputEnabled = true;


		//二维信号的目标位置
		private float targetDForward;
		private float targetDRight;
		//二维信号的移动速度
		private float velocityDForward;
		private float velocityDRight;

		//平滑变换时间
		private readonly float smoothTime = 0.1f;

//		/// <summary>
//		///二维信号到原点的距离 
//		/// </summary>
//		public float DDistance => Mathf.Sqrt(Dup * Dup + DRight * DRight);


		private void OnEnable() {
			StartCoroutine(GetInputCr());
		}

		private IEnumerator GetInputCr() {
			while(true) {
				GetInput();
				GetInput_View();
				yield return new WaitForSeconds(0.02f);
			}
		}

		/// <summary>得到用户输入</summary>
		private void GetInput() {
			//判断模块是否启用
			if(!inputEnabled)
				return;

			//计算二维信号的目标位置
			targetDForward = (Input.GetKey(keyForward) ? 1.0f : 0) - (Input.GetKey(keyBack) ? 1.0f : 0);
			targetDRight = (Input.GetKey(keyRight) ? 0.5f : 0) - (Input.GetKey(keyLeft) ? 0.5f : 0);

			//计算真正的二维信号（对于目标位置，按照移动速度）
			DForward = Mathf.SmoothDamp(DForward, targetDForward, ref velocityDForward, smoothTime);
			DRight = Mathf.SmoothDamp(DRight, targetDRight, ref velocityDRight, smoothTime);

			//将正方形坐标转化成圆形坐标，使对角线最长也是1.0
			var tempDAxis = SquareToCircle(new Vector2(DRight, DForward));
			DRight = tempDAxis.x;
			DForward = tempDAxis.y;

			DMag = Mathf.Sqrt(DForward * DForward + DRight * DRight);
			DVec = DRight * transform.right + DForward * transform.forward;

//			//这一段是什么意思又有何作用？有什么必要吗？
//			bool newJump = Input.GetKey(keyB);		
//			if(newJump != lastJump && newJump) {
//				jump = true;
//			}else {
//				jump = false;
//			}
//			lastJump = newJump;

			//跑步：按住Ctrl
			//后跃：待机时按下Space
			//跳跃：跑步时按下Space
			//翻滚：按下Space
			run = Input.GetKey(keyA);
			jab_roll_jump = Input.GetKeyDown(keyB);

			attack = Input.GetMouseButtonDown(0);

		}


		/// <summary>得到用户输入，控制视角移动</summary>
		private void GetInput_View() {
			//计算视角移动的信号
			VUp = (Input.GetKey(KeyVUp) ? 1.0f : 0) - (Input.GetKey(KeyVDown) ? 1.0f : 0);
			VRight = (Input.GetKey(KeyVRight) ? 1.0f : 0) - (Input.GetKey(KeyVLeft) ? 1.0f : 0);
		}


		/// <summary>将正方形坐标转化成圆形坐标</summary>
		private Vector2 SquareToCircle(Vector2 input) {
			var output = Vector2.zero;
			//使用公式
			output.x = input.x * Mathf.Sqrt(1 - input.y * input.y / 2.0f);
			output.y = input.y * Mathf.Sqrt(1 - input.x * input.x / 2.0f);
			return output;
		}
	}
}