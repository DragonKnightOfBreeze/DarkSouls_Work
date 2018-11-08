namespace DSWork {
	public interface IMyAxis {
		/// <summary>坐标值信号</summary>
		float AxisValue { get; }
		
		/// <summary>更新输入状态。每帧调用。</summary>
		void Tick();
	}
}