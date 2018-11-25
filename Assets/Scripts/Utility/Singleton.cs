using System;
using System.Reflection;

namespace DSWork.Utility {
	/// <summary>泛型单例类（带有无参构造方法）</summary>
	public class Singleton<T> where T : new() {
		private static T instance;
		private static readonly object syncLock = new object();


		/// <summary>类的单例。（线程安全）</summary>
		public static T Instance {
			get {
				if(instance == null) {
					lock(syncLock) {
						if(instance == null)
							instance = new T();
					}
				}
				return instance;
			}
		}
	}


	/// <summary>泛型单例类（带有私有构造方法）</summary>
	public class TSingleton<T> where T : class {
		private static T instance;
		private static readonly object syncLock = new object();
		private static readonly Type[] emptyTypes = new Type[0];


		/// <summary>类的单例。（线程安全）</summary>
		public static T Instance {
			get {
				if(instance == null){
					lock(syncLock) {
						if(instance == null) {
							var ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, emptyTypes, null);
							if(ci == null)
								throw new InvalidOperationException(nameof(TSingleton<T>) + "\t类必须带有一个私有构造方法！");
							instance = ci.Invoke(null) as T;
						}
					}
				}
				return instance;
			}
		}
	}
}