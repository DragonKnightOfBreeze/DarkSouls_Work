/*******
 * ［概述］
 * 封装的坐标类
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
	/// <summary>封装的坐标类</summary>
	public class MyAxis : IMyAxis {
		/// <summary>原始的坐标轴</summary>
		private readonly string axis;
		/// <summary>原始的正向按钮</summary>
		private readonly string button1;
		/// <summary>原始的反向按钮</summary>
		private readonly string button2;
		/// <summary>坐标值</summary>
		private float axisValue;


		/// <summary>坐标值信号</summary>
		public float AxisValue => axisValue;


		public MyAxis(string axis) {
			this.axis = axis;
		}

		public MyAxis(string button1, string button2) {
			this.button1 = button1;
			this.button2 = button2;
		}


		/// <summary>更新输入状态。每帧调用。</summary>
		public void Tick() {
			if(axis != null)
				axisValue = Input.GetAxis(axis);
			else
				axisValue = Input.GetButton(button1).To1_0() - Input.GetButton(button2).To1_0();
		}
	}
}