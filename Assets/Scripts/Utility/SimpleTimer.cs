using UnityEngine;

namespace DSWork.Utility {
	/// <summary>简单计时器</summary>
	public class SimpleTimer {
		/// <summary>计时器的状态</summary>
		public enum EState {
			NotRunning,
			Running,
			End
		}

		public EState State;

		/// <summary>计时器的持续时间</summary>
		private float durationTime;
		/// <summary>已经流逝的时间</summary>
		private float elapsedTime;

		/// <summary>更新计时。</summary>
		public void Tick() {
			switch(State) {
				case EState.NotRunning:
					break;
				case EState.Running:
					elapsedTime += Time.deltaTime;
					if(elapsedTime >= durationTime)
						State = EState.End;
					break;
				case EState.End:
					State = EState.NotRunning;
					break;
			}
		}

		/// <summary>开始计时。</summary>
		public void StartTimer(float duration) {
			durationTime = duration;
			elapsedTime = 0;
			State = EState.Running;
		}
	}
}