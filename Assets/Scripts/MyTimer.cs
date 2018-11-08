/*******
 * ［概述］
 * 计时器
 * 
 * ［用法］
 * 
 * 
 * ［备注］ 
 * 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System;
using UnityEngine;

namespace DSWork {
	public class MyTimer {
		public enum State {
			Finished,
			Run
		}

		public State curState;

		/// <summary>持续的时间</summary>
		public float duration;
		/// <summary>流逝的时间</summary>
		private float elapsedTime;

		public void Tick() {
			if(curState == State.Run) {
				elapsedTime += Time.deltaTime;
				if(elapsedTime >= duration)
					curState = State.Finished;
			}
		}

		public void StartTimer(float duration = 0.2f) {
			this.duration = duration;
			elapsedTime = 0;
			curState = State.Run;
		}
	}
}