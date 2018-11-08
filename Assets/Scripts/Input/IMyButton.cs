namespace DSWork {
	public interface IMyButton {
		/// <summary>按下信号</summary>
		bool PressDown { get; }
		/// <summary>释放信号</summary>
		bool PressUp { get; }
		/// <summary>持续按压信号</summary>
		bool Press { get; }

		/// <summary>释放延长信号信号</summary>
		/// <remarks>在释放后的一定时间内有效。</remarks>
		/// <param name="extend">延长时间</param>
		bool ExtendPress(float extend = 0.2f);

		/// <summary>按下延迟信号</summary>
		/// <remarks>在按下后的一定时间内有效。</remarks>
		/// <param name="delay">延迟时间</param>
		bool DelayPress(float delay = 0.2f);

		/// <summary>整体按压延迟信号</summary>
		/// <remarks>从按下后的一定时间后，到释放后的一定时间前有效。</remarks>
		/// <param name="delay">延迟时间</param>
		/// <param name="extend">延长时间</param>
		bool FullDelayPress(float delay = 0.2f, float extend = 0.2f);

		/// <summary>快速单击信号</summary>
		/// <param name="interval">间隔时间</param>
		bool QuickPress(float interval = 0.2f);

		/// <summary>限时双击信号</summary>
		/// <param name="interval">间隔时间</param>
		bool DoublePress(float interval = 0.2f);
		
		/// <summary>更新输入状态。每帧调用。</summary>
		void Tick();
	}
}