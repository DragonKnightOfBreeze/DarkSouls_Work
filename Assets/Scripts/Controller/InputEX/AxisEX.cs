using System;
using DSWork.Utility;
using UnityEngine;

namespace DSWork.InputEX {
	public class AxisEX : IInputEX {

		private readonly EInput axisName;
		
		private float curAxisValue;
		private float lastAxisValue;

		/// <summary>持续按压信号</summary>
		public bool Press => curAxisValue >= 0.1f;
		/// <summary>按下信号</summary>
		public bool PressDown => curAxisValue >= 0.1f && lastAxisValue < 0.1f;
		/// <summary>抬起信号</summary>
		public bool PressUp => curAxisValue <= 0.1f && lastAxisValue > 0.1f;

		/// <summary>修正过的持续按压信号（延迟按下，延长抬起）</summary>
		/// <param name="delay">延迟时间</param>
		/// <param name="extend">延长时间</param>
		/// <returns></returns>
		public bool FixedPress(float delay, float extend) {
			throw new NotImplementedException();
		}

		/// <summary>修正过的按下信号（延迟/延长，默认延迟）</summary>
		/// <param name="time">延迟/延长时间</param>
		/// <param name="isDelaying">是否延迟</param>
		/// <returns></returns>
		public bool FixedPressDown(float time, bool isDelaying = true) {
			throw new NotImplementedException();
		}

		/// <summary>修正过的抬起信号（延迟/延长，默认延迟）</summary>
		/// <param name="time">延迟/延长时间</param>
		/// <param name="isDelaying">是否延迟</param>
		/// <returns></returns>
		public bool FixedPressUp(float time, bool isDelaying = true) {
			throw new NotImplementedException();
		}

		
		/// <summary>快速点击的信号</summary>
		/// <param name="interval">用于判断的间隔时间</param>
		/// <returns></returns>
		public bool QuickClick(float interval = 0.2f) {
			throw new NotImplementedException();
		}

		/// <summary>限时双击的信号</summary>
		/// <param name="interval">用于判断的间隔时间</param>
		/// <returns></returns>
		public bool DoubleClick(float interval = 0.2f) {
			throw new NotImplementedException();
		}


		/// <summary>坐标值</summary>
		public float AxisValue => curAxisValue;


		public AxisEX(string axis) {
			Enum.TryParse(axis, out axisName);
		}

		public AxisEX(EInput axis) {
			axisName = axis;
		}
		
		
		/// <summary>更新输入。</summary>
		public void Tick() {
			lastAxisValue = curAxisValue;
			try {
				curAxisValue = Input.GetAxis(axisName.TS());
			} catch (Exception) {
				curAxisValue = 0;
			}
		}
	}
}