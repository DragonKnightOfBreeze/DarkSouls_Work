namespace DSWork.InputMgr {
	/// <summary>
	/// 输入接口
	/// </summary>
	public interface IInput {
		bool Press { get; }
		bool PressDown { get; }
		bool PressUp { get; }
		
		bool FixedPress(float delay = 0, float extend = 0);
		bool FixedPressDown(float delay = 0);
		bool FixedPressUp(float extend = 0);

		bool Click(float interval = 0.2f);
		bool DoubleClick(float interval = 0.2f);
		
		float AxisValue();

		void Tick();
	}
}