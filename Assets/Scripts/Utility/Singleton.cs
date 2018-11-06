namespace DSWork.Utility {
	public class Singleton<T> where T : new() {
		private static T instance;
		private static readonly object syncLock = new object();

		/// <summary>得到类的单例（线程安全）</summary>
		public static T Instance {
			get {
				if(instance == null)
					lock(syncLock) {
						if(instance == null)
							instance = new T();
					}
				return instance;
			}
		}
	}
}