/* 
 *
 * 使用方法：
 * 		在脚本中创建InputEnum - IMyInputEX字典，使用对应的键进行构造。
 * 		在脚本的Update方法中遍历调用Tick()方法
 * 		在脚本的Update方法或者协程中通过指定的属性或方法，得到想要的输出信号。
 */

namespace DSWork.InputEX {
	/// <summary>
	/// 封装的输入接口
	/// </summary>
	public interface IMyInputEX {
		
		/// <summary>持续按压信号</summary>
		bool Press { get; }
		/// <summary>按下信号</summary>
		bool PressDown { get; }
		/// <summary>抬起信号</summary>
		bool PressUp { get; }

		/// <summary>修正过的持续按压信号（延迟按下，延长抬起）</summary>
		/// <param name="delay">延迟时间</param>
		/// <param name="extend">延长时间</param>
		/// <returns></returns>
		bool FixedPress(float delay, float extend);

		/// <summary>修正过的按下信号（延迟/延长，默认延迟）</summary>
		/// <param name="time">延迟/延长时间</param>
		/// <param name="isDelaying">是否延迟</param>
		/// <returns></returns>
		bool FixedPressDown(float time, bool isDelaying = true);

		/// <summary>修正过的抬起信号（延迟/延长，默认延迟）</summary>
		/// <param name="time">延迟/延长时间</param>
		/// <param name="isDelaying">是否延迟</param>
		/// <returns></returns>
		bool FixedPressUp(float time, bool isDelaying = true);
		
		
		/// <summary>快速点击的信号</summary>
		/// <param name="interval">用于判断的间隔时间</param>
		/// <returns></returns>
		bool QuickClick(float interval = 0.1f);

		/// <summary>限时双击的信号</summary>
		/// <param name="interval">用于判断的间隔时间</param>
		/// <returns></returns>
		bool DoubleClick(float interval = 0.1f);


		/// <summary>坐标值</summary>
		float AxisValue { get; }


		/// <summary>更新输入。</summary>
		void Tick();
	}
}