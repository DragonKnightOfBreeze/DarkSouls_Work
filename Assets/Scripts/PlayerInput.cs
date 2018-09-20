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
 * TODO：解决斜向移动时的速度更快的问题
 * TODO：转向应该和四元数挂钩，而不是向量！
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DSWork {
	/// <summary> 
	/// 玩家输入模块
	/// </summary>
	public class PlayerInput : MonoBehaviour {

		//按键的具体设置
		
		public string keyForward = "w";
		public string keyBackward = "s";
		public string keyLeft = "a";
		public string keyRight = "d";

		//二维信号
		public float DForward = 0f;
		[HideInInspector]
		public float DRight = 0f;
		/// <summary>原点到二维信号的距离</summary>
		public float DMag;
		/// <summary>移动方向</summary>
		public Vector3 DVec;

		
		/// <summary>
		/// 是否启用模块 
		/// </summary>
		public bool inputEnabled = true;

		//二维信号的目标位置
		private float targetDForward;
		private float targetDRight;
		//二维信号的移动速度
		private float velocityDForward = 0f;
		private float velocityDRight = 0f;

		//平滑变换时间
		private readonly float smoothTime = 0.1f;
		
//		/// <summary>
//		///二维信号到原点的距离 
//		/// </summary>
//		public float DDistance => Mathf.Sqrt(Dup * Dup + DRight * DRight);


		private void OnEnable() {
			StartCoroutine(GetInputCr());
		}

		IEnumerator GetInputCr() {
			while(true) {
				GetInput();
				yield return new WaitForSeconds(0.02f);
			}
		}
		
		/// <summary>
		/// 得到用户输入
		/// </summary>
		private void GetInput () {

			//判断模块是否启用
			if (!inputEnabled)
				return;

			//计算二维信号的目标位置
			targetDForward = (Input.GetKey (keyForward) ? 1.0f : 0) - (Input.GetKey (keyBackward) ? 1.0f : 0);
			targetDRight = (Input.GetKey (keyRight) ? 0.5f : 0) - (Input.GetKey (keyLeft) ? 0.5f : 0);
			
			//计算真正的二维信号（对于目标位置，按照移动速度）
			DForward = Mathf.SmoothDamp (DForward, targetDForward, ref velocityDForward, smoothTime);
			DRight = Mathf.SmoothDamp (DRight, targetDRight, ref velocityDRight, smoothTime);

			DMag =  Mathf.Sqrt(DForward * DForward + DRight * DRight);
			DMag = DMag > 1.0f ? 1.0f : DMag;
			
			DVec = DRight * transform.right + DForward * transform.forward;


		}

	}
}