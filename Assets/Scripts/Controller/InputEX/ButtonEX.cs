using System;
using DSWork.Utility;
using UnityEngine;

namespace DSWork.InputEX {
	/// <summary>封装的按钮类</summary>
	public class ButtonEX : IInputEX {
		private readonly EInput buttonName;

		private bool lastState;
		private bool isToggling;

		private readonly SimpleTimer timerA = new SimpleTimer();
		private readonly SimpleTimer timerB = new SimpleTimer();

		private float timeA;
		private float timeB;
		private float _axisValue;

		/// <summary>持续按压信号</summary>
		public bool Press { get; private set; }
		/// <summary>按下信号</summary>
		public bool PressDown => Press && isToggling;
		/// <summary>抬起信号</summary>
		public bool PressUp => !Press && isToggling;

		/// <summary>修正过的持续按压信号（延迟按下，延长抬起）</summary>
		/// <param name="delay">延迟时间</param>
		/// <param name="extend">延长时间</param>
		/// <returns></returns>
		public bool FixedPress(float delay, float extend) {
			timeA = delay;
			timeB = extend;
			return Press && timerA.State != SimpleTimer.EState.Running || timerB.State == SimpleTimer.EState.Running;
		}

		/// <summary>修正过的按下信号（延迟/延长，默认延迟）</summary>
		/// <param name="time">延迟/延长时间</param>
		/// <param name="isDelaying">是否延迟</param>
		/// <returns></returns>
		public bool FixedPressDown(float time, bool isDelaying = true) {
			timeA = time;
			if(isDelaying)
				return timerA.State == SimpleTimer.EState.End;
			return timerA.State == SimpleTimer.EState.Running;
		}

		/// <summary>修正过的抬起信号（延迟/延长，默认延迟）</summary>
		/// <param name="time">延迟/延长时间</param>
		/// <param name="isDelaying">是否延迟</param>
		/// <returns></returns>
		public bool FixedPressUp(float time, bool isDelaying = true) {
			timeB = time;
			if(isDelaying)
				return timerB.State == SimpleTimer.EState.End;
			return timerB.State == SimpleTimer.EState.Running;
		}


		/// <summary>坐标值</summary>
		public float AxisValue => Press.To1_0();


		/// <summary>快速点击的信号</summary>
		/// <param name="interval">用于判断的间隔时间</param>
		/// <returns></returns>
		public bool QuickClick(float interval = 0.1f) {
			return PressUp && FixedPressDown(interval, false);
		}

		/// <summary>限时双击的信号</summary>
		/// <param name="interval">用于判断的间隔时间</param>
		/// <returns></returns>
		public bool DoubleClick(float interval = 0.1f) {
			return PressDown && FixedPressUp(interval, false);
		}


		public ButtonEX(string button) {
			Enum.TryParse(button, out buttonName);
		}

		public ButtonEX(EInput button) {
			buttonName = button;
		}


		/// <summary>更新输入。</summary>
		public void Tick() {
			timerA.Tick();
			timerB.Tick();
			lastState = Press;
			try {
				Press = Input.GetButton(buttonName.TS());
			} catch (Exception e) {
				Debug.Log(e.Message);
				Press = false;
			}
			isToggling = false;
			if(Press != lastState) {
				isToggling = true;
				if(Press)
					timerA.StartTimer(timeA);
				else
					timerB.StartTimer(timeB);
			}
		}
	}
}