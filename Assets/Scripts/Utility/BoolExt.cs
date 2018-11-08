namespace DSWork {
	/// <summary>布尔类型拓展类</summary>
	public static class BoolExt {
		/// <summary>将布尔值转换为0或1。</summary>
		public static float To1_0(this bool b) {
			return b ? 1.0f : 0f;
		}

		/// <summary>将布尔值转换为2或1。</summary>
		public static float To2_1(this bool b) {
			return b ? 2.0f : 1.0f;
		}
	}
}