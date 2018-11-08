using UnityEngine;

namespace DSWork {
	/// <summary>不要在场景加载时被销毁</summary>
	public class NoDestroyOnLoad : MonoBehaviour {
		private void Awake() {
			DontDestroyOnLoad(gameObject);
		}
	}
}