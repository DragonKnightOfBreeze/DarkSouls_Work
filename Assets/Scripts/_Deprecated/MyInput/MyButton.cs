/*******
 * ［概述］
 * 封装的按钮类
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

using UnityEngine;

namespace DSWork {
	/// <summary>封装的按钮类</summary>
	public class MyButton : IMyButton {

		private readonly string button;
		
		private bool curState;
		private bool lastState;

		private readonly MyTimer extendTimer = new MyTimer();
		private readonly MyTimer delayTimer = new MyTimer();

		private float extendingDuration = 0.2f;
		private float delayingDuration = 0.2f;

		/// <summary>按下</summary>
		private bool onPressed;
		/// <summary>释放</summary>
		private bool onReleased;
		/// <summary>持续按压</summary>
		private bool isPressing;
		/// <summary>延长</summary>
		private bool isExtending;
		/// <summary>延迟</summary>
		private bool isDelaying;

		
		/// <summary>按下信号</summary>
		public bool PressDown => onPressed;
		/// <summary>释放信号</summary>
		public bool PressUp => onReleased;
		/// <summary>持续按压信号</summary>
		public bool Press => isPressing;

		/// <summary>释放延长信号信号</summary>
		/// <remarks>在释放后的一定时间内有效。</remarks>
		/// <param name="extend">延长时间</param>
		public bool ExtendPress(float extend = 0.2f) {
			extendingDuration = extend;
			return isExtending;
		}

		/// <summary>按下延迟信号</summary>
		/// <remarks>在按下后的一定时间内有效。</remarks>
		/// <param name="delay">延迟时间</param>
		public bool DelayPress(float delay = 0.2f) {
			delayingDuration = delay;
			return isDelaying;
		}

		/// <summary>整体按压延迟信号</summary>
		/// <remarks>从按下后的一定时间后，到释放后的一定时间前有效。</remarks>
		/// <param name="delay">延迟时间</param>
		/// <param name="extend">延长时间</param>
		public bool FullDelayPress(float delay = 0.2f, float extend = 0.2f) {
			return Press && !DelayPress(delay) || ExtendPress(extend);
		}

		/// <summary>快速单击信号</summary>
		/// <param name="interval">间隔时间</param>
		public bool QuickPress(float interval = 0.2f) {
			return PressUp && DelayPress(interval);
		}

		/// <summary>限时双击信号</summary>
		/// <param name="interval">间隔时间</param>
		public bool DoublePress(float interval = 0.2f) {
			return PressDown && ExtendPress(interval);
		}

		
		public MyButton(string button) {
			this.button = button;
		}
		
		
		/// <summary>更新输入状态。每帧调用。</summary>
		public void Tick() {
			extendTimer.Tick();
			delayTimer.Tick();

			lastState = curState;
			curState = Input.GetButton(button);
			isPressing = curState;
			onPressed = false;
			onReleased = false;
			if(curState != lastState) {
				if(curState) {
					onPressed = true;
					delayTimer.StartTimer(delayingDuration);
				}
				else {
					onReleased = true;
					extendTimer.StartTimer(extendingDuration);
				}
			}
			isExtending = extendTimer.curState == MyTimer.State.Run;
			isDelaying = delayTimer.curState == MyTimer.State.Run;
		}
	}
}