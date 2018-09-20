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
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSWork {
	/// <summary> 
	/// 玩家输入模块
	/// </summary>
	public class PlayerInput : MonoBehaviour {

		//按键的具体设置
		

		public string keyUp = "w";
		public string keyDown = "s";
		public string keyLeft = "a";
		public string keyRight = "d";

		//二维信号
		public float Dup;
		public float Dright;

		
		//是否启用模块
		public bool inputEnabled = true;

		//二维信号的目标位置
		private float targetDup = 0;
		private float targetDright = 0;
		//二维信号的移动速度
		private float velocityDup;
		private float velocityDright;

		//平滑变换时间
		private readonly float smoothTime = 0.1f;
		
		void Update () {
			GetInput ();
		}

		/// <summary>
		/// 得到用户输入
		/// </summary>
		private void GetInput () {

			//判断模块是否启用
			if (!inputEnabled)
				return;

			//计算二维信号的目标位置
			targetDup = (Input.GetKey (keyUp) ? 1.0f : 0) - (Input.GetKey (keyDown) ? 1.0f : 0);
			targetDright = (Input.GetKey (keyRight) ? 1.0f : 0) - (Input.GetKey (keyLeft) ? 1.0f : 0);
			
			//计算真正的二维信号（对于目标位置，按照移动速度）
			Dup = Mathf.SmoothDamp (Dup, targetDup, ref velocityDup, smoothTime);
			Dright = Mathf.SmoothDamp (Dright, targetDup, ref velocityDright, smoothTime);
		}

	}
}