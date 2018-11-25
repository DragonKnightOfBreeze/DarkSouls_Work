// ReSharper disable UseNullPropagation

using System.Reflection;
using System;

namespace DSWork.Utility {
	/// <summary>反射工具类</summary>
	public static class ReflectionTool {
		public static readonly BindingFlags flags_common = BindingFlags.Instance | BindingFlags.SetField | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty;
		public static readonly BindingFlags flags_public = flags_common | BindingFlags.Public;
		public static readonly BindingFlags flags_nonpublic = flags_common | BindingFlags.NonPublic;
		public static readonly BindingFlags flags_all = flags_common | BindingFlags.Public | BindingFlags.NonPublic;
		public static readonly BindingFlags flags_method = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
		public static readonly BindingFlags flags_method_instance = flags_method | BindingFlags.Instance;
		public static readonly BindingFlags flags_method_static = flags_method | BindingFlags.Static;

		public static readonly Type[] empty_types = new Type[0];


		/// <summary>得到构造器信息。</summary>
		/// <param name="rBindFlags"></param>
		/// <param name="rType"></param>
		/// <param name="rTypes"></param>
		public static ConstructorInfo GetConstructorInfo(BindingFlags rBindFlags, Type rType, Type[] rTypes) {
			return rType.GetConstructor(rBindFlags, null, rTypes, null);
		}

		/// <summary>创建实例。</summary>
		/// <param name="rType"></param>
		/// <param name="rBindFlags"></param>
		public static object CreateInstance(Type rType, BindingFlags rBindFlags) {
			var rConstructorInfo = GetConstructorInfo(rBindFlags, rType, empty_types);
			return rConstructorInfo.Invoke(null);
		}

		/// <summary>调用构造器。不带参数。</summary>
		/// <param name="rType"></param>
		public static object Construct(Type rType) {
			var rConstructorInfo = GetConstructorInfo(flags_all, rType, empty_types);
			return rConstructorInfo.Invoke(null);
		}

		/// <summary>调用构造器。带参数。</summary>
		/// <param name="rType"></param>
		/// <param name="rTypes"></param>
		/// <param name="rParams"></param>
		public static object Construct(Type rType, Type[] rTypes, params object[] rParams) {
			var rConstructorInfo = GetConstructorInfo(flags_all, rType, rTypes);
			return rConstructorInfo.Invoke(null, rParams);
		}

		/// <summary>得到指定的成员。</summary>
		/// <param name="rObject"></param>
		/// <param name="rMemberName"></param>
		/// <param name="rBindFlags"></param>
		public static object GetAttrMember(object rObject, string rMemberName, BindingFlags rBindFlags) {
			if(rObject == null) return null;
			var rType = rObject.GetType();
			return rType.InvokeMember(rMemberName, rBindFlags, null, rObject, new object[] { });
		}

		/// <summary>设置指定的成员。</summary>
		/// <param name="rObject"></param>
		/// <param name="rMemberName"></param>
		/// <param name="rBindFlags"></param>
		/// <param name="rParams"></param>
		public static void SetAttrMember(object rObject, string rMemberName, BindingFlags rBindFlags, params object[] rParams) {
			if(rObject == null) return;
			var rType = rObject.GetType();
			rType.InvokeMember(rMemberName, rBindFlags, null, rObject, rParams);
		}

		/// <summary>调用指定对象的指定方法。</summary>
		/// <param name="rObject"></param>
		/// <param name="rMemberName"></param>
		/// <param name="rBindFlags"></param>
		/// <param name="rParams"></param>
		/// <returns>方法的返回值</returns>
		public static object MethodMember(object rObject, string rMemberName, BindingFlags rBindFlags, params object[] rParams) {
			if(rObject == null) return null;
			var rType = rObject.GetType();
			return rType.InvokeMember(rMemberName, rBindFlags, null, rObject, rParams);
		}

		/// <summary>调用指定类型的指定方法。</summary>
		/// <param name="rType"></param>
		/// <param name="rMemberName"></param>
		/// <param name="rBindFlags"></param>
		/// <param name="rParams"></param>
		/// <returns>方法的返回值</returns>
		public static object MethodMember(Type rType, string rMemberName, BindingFlags rBindFlags, params object[] rParams) {
			return rType.InvokeMember(rMemberName, rBindFlags, null, null, rParams);
		}

		/// <summary>转换类型。</summary>
		/// <param name="rType"></param>
		/// <param name="rValueStr"></param>
		public static object TypeConvert(Type rType, string rValueStr) {
			return Convert.ChangeType(rValueStr, rType);
		}
	}
}