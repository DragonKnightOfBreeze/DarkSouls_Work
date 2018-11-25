namespace DSWork {
	/// <summary>布尔类型拓展类</summary>
	public static class BoolExt {
		/// <summary>将布尔值转换为1或0。</summary>
		public static float To1_0(this bool b) {
			return b ? 1.0f : 0f;
		}

		/// <summary>将布尔值转换为2或1。</summary>
		public static float To2_1(this bool b) {
			return b ? 2.0f : 1.0f;
		}

		/// <summary>将布尔值转换为n或1。</summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float ToN_1(this bool b, float n) {
			return b ? n : 1.0f;
		}

		/// <summary>将布尔值转换为n或0。</summary>
		public static float ToN_0(this bool b, float n) {
			return b ? n : 0f;
		}
	}
}