using System;
using DSWork.Global;
using JetBrains.Annotations;
using UnityEngine;

namespace DSWork.Utility {
	/// <summary>游戏对象拓展类</summary>
	public static class GameObjectExt {
		/// <summary>得到该游戏对象上的指定组件，如果没有则添加。</summary>
		public static T SafeGetComponent<T>(this GameObject go) where T : Component {
			return go.GetComponent<T>() ?? go.AddComponent<T>();
		}


		/// <summary>查找该游戏对象的子游戏对象。</summary>
		/// <param name="name">指定的子游戏对象的名字</param>
		public static GameObject FindChild(this GameObject go, string name) {
			var child = go.transform.Find(name);
			if(child == null)
				Debug.LogWarning(nameof(GameObjectExt) + "\t找不到指定的子游戏对象！");
			return child.gameObject;
		}

		/// <summary>遍历查找该游戏对象的子游戏对象。</summary>
		/// <param name="name">指定的子游戏对象的名字</param>
		public static GameObject FindChildInAll(this GameObject go, string name) {
			foreach(Transform child in go.transform)
				if(name == child.name)
					return child.gameObject;
				else
					return FindChildInAll(child.gameObject, child.name);
			Debug.LogWarning(nameof(GameObjectExt) + "\t找不到指定的子游戏对象！");
			return null;
		}

		/// <summary>根据给定的标签，查找子游戏对象。</summary>
		/// <param name="tagName">指定的标签</param>
		/// <returns></returns>
		public static GameObject FindChildWithTag(this GameObject go, string tagName) {
			foreach(Transform child in go.transform)
				if(child.CompareTag(tagName))
					return child.gameObject;
			Debug.LogWarning(nameof(GameObjectExt) + "\t找不到指定的子游戏对象！");
			return null;
		}

		/// <summary>根据给定的标签，查找子游戏对象。</summary>
		/// <param name="tagName">指定的标签</param>
		/// <returns></returns>
		public static GameObject FindChildWithTag(this GameObject go, ETag tagName) {
			foreach(Transform child in go.transform)
				if(child.CompareTag(tagName.TS()))
					return child.gameObject;
			Debug.LogWarning(nameof(GameObjectExt) + "\t找不到指定的子游戏对象！");
			return null;
		}

		/// <summary>查找该游戏对象的父游戏对象。</summary>
		public static GameObject FindParent(this GameObject go) {
			var parent = go.transform.parent;
			if(parent == null)
				Debug.LogWarning(nameof(GameObjectExt) + "\t找不到父游戏对象！");
			return parent.gameObject;
		}


		/// <summary>遍历查找一个游戏对象的父游戏对象。</summary>
		/// <param name="name">指定的父游戏对象的名字</param>
		public static GameObject FindParentInAll(this GameObject go, string name) {
			var parent = go.transform.parent;
			while(parent.name != name && parent != null)
				parent = go.transform.parent;
			if(parent == null)
				Debug.LogWarning(nameof(GameObjectExt) + "\t找不到父游戏对象！");
			return parent.gameObject;

		}
	}
}