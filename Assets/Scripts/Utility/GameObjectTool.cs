using UnityEngine;

namespace DSWork.Utility {
	public static class GameObjectTool {
		
		/// <summary>查找一个游戏对象的子游戏对象</summary>
		/// <param name="name">指定的子游戏对象的名字</param>
		public static GameObject FindChild(this GameObject go, string name) {
			var child = go.transform.Find(name).gameObject;
			if(child == null) {
				Debug.LogWarning(nameof(FindChild) + "\t找不到指定的子游戏对象！");
				return null;
			}
			return child;
		}


		/// <summary>查找一个游戏对象的父游戏对象</summary>
		public static GameObject FindParent(this GameObject go) {
			var parent = go.transform.parent.gameObject;
			if(parent == null) {
				Debug.LogWarning(nameof(FindParent) + "\t找不到父游戏对象！");
				return null;
			}
			return parent;
		}

		/// <summary>
		/// 遍历查找一个游戏对象的子游戏对象
		/// </summary>
		/// <param name="name">指定的子游戏对象的名字</param>
		public static GameObject FindChildInAll(this GameObject go, string name) {
			//TODO
			return null;
		}

		/// <summary>
		/// 遍历查找一个游戏对象的父游戏对象
		/// </summary>
		/// <param name="name">指定的父游戏对象的名字</param>
		public static GameObject FindParentInAll(this GameObject go, string name) {
			//TODO
			return null;
		}
	}
}