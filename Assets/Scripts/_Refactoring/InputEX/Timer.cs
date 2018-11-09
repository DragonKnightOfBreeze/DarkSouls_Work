//using System;
//using UnityEngine;
//
//namespace DSWork.InputEX {
//	
//	/// <summary>简单计时器</summary>
//	internal class Timer {
//		/// <summary>持续时间</summary>
//		private float duration;
//		/// <summary>流逝时间</summary>
//		private float elapsedTime;
//		/// <summary>是否仍在计时</summary>
//		internal bool isRunning;
//		/// <summary>计时结束之后要做的Action委托</summary>
//		private Action nextAction;
//
//		/// <summary>更新计时器。</summary>
//		internal void Tick() {
//			if(!isRunning)
//				return;
//			elapsedTime += Time.deltaTime;
//			if(elapsedTime >= duration) {
//				isRunning = false;
//				nextAction?.Invoke();
//				nextAction = null;
//			}
//		}
//
//		/// <summary>开始计时器。</summary>
//		/// <param name="duration">指定的持续时间</param>
//		/// <param name="next">计时结束之后要做的Action委托</param>
//		internal void StartTimer(float duration, Action next = null) {
//			if(duration <= 0) {
//				next?.Invoke();
//				return;
//			}
//			nextAction = next;
//			this.duration = duration;
//			elapsedTime = 0;
//			isRunning = true;
//		}
//	}
// }