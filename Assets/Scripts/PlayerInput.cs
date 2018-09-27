//FINISH：解决斜向移动时的速度更快的问题
//TODO：转向应该和四元数挂钩，而不是向量！
//TODO：将状态信号由布尔组改为枚举。

 /*******
 * ［概述］
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 * 
 * 玩家输入模块
 * 
 * ［功能］
 * 
 * 
 * ［思路］ 
 *
 *
 * ［备注］
 * SmoothDamp：平滑增加
 *
 */
using System.Collections;
using UnityEngine;

namespace DSWork {
	/// <summary>玩家输入模块</summary>
	public class PlayerInput : MonoBehaviour {
		//按键的具体设置
		[Header("===Key settings===")]
		public string keyForward = "w";
		public string keyBackward = "s";
		public string keyLeft = "a";
		public string keyRight = "d";

		public string keyA = "left shift";
		public string keyB = "space";
		public string keyC = "e";
		public string keyD = "f";


		//二维信号
		[Header("===Output Signals===")]
		public float DForward;
		public float DRight;
		/// <summary>原点到二维信号的距离</summary>
		public float DMag;
		/// <summary>移动方向</summary>
		public Vector3 DVec;

		//按压信号
		/// <summary>跑步信号</summary>
		public bool run = false;
		//单击触发信号
		/// <summary>跳跃信号</summary>
		public bool jump = false;
//		private bool lastJump = false;
		//双击触发信号


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
				yield return new WaitForSeconds(0.02f);
			}
		}

		/// <summary>得到用户输入</summary>
		private void GetInput() {
			//判断模块是否启用
			if(!inputEnabled)
				return;

			//计算二维信号的目标位置
			targetDForward = (Input.GetKey(keyForward) ? 1.0f : 0) - (Input.GetKey(keyBackward) ? 1.0f : 0);
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

			run = Input.GetKey(keyA);



//			bool newJump = Input.GetKey(keyB);
//			//这一段是什么意思又有何作用？
//			if(newJump != lastJump && newJump) {
//				jump = true;
//			}else {
//				jump = false;
//			}
//			lastJump = newJump;
			jump = Input.GetKeyDown(keyB);

		}

		/// <summary>
		/// 将正方形坐标转化成圆形坐标
		/// </summary>
		private Vector2 SquareToCircle(Vector2 input) {
			var output = Vector2.zero;
			//使用公式
			output.x = input.x * Mathf.Sqrt(1 - input.y * input.y / 2.0f);
			output.y = input.y * Mathf.Sqrt(1 - input.x * input.x / 2.0f);
			return output;
		}
	}
}