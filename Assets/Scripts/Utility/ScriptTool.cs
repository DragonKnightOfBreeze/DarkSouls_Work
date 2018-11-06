using UnityEngine;

namespace DSWork.Utility {
	/// <summary>
	/// 脚本工具类
	/// </summary>
	public static class ScriptTool {
		private const string ROOT = "_Scripts";

		private static GameObject root;

		/// <summary>得到挂载到空游戏对象上的脚本</summary>
		/// <para>如果没有，则自动添加。</para>
		/// <param name="path">脚本路径</param>
		/// <returns></returns>
		public static T GetScript<T>(params string[] path)
			where T : MonoBehaviour {
			var child = GetRoot();
			foreach(string p in path) {
				child = child.FindChild(p);
				if(child == null) {
					Debug.LogWarning(nameof(GetScript) + "\t找不到指定的路径！");
					return null;
				}
			}
			var script = child.GetComponent<T>() ?? child.AddComponent<T>();
			return script;
		}


		/// <summary>得到挂载脚本的游戏对象组的根节点</summary>
		/// <returns></returns>
		private static GameObject GetRoot() {
			if(root == null) {
				root = GameObject.Find(ROOT);
				if(root == null)
					Debug.LogWarning(nameof(GetRoot) + "\t找不到指定的根路径！");
			}
			return root;
		}
	}
}