/*******
 * ［概述］
 * 
 * 
 * ［用法］
 * 
 * 
 * ［备注］ 
 * 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System;
using DSWork.Global;
using UnityEngine;

namespace DSWork.Utils {
	/// <summary>枚举类型的拓展类</summary>
	public static class EnumExtensions {
		
		/// <summary>枚举值转为字符串</summary>
		/// <remarks>如果要以数字开头："__1Str" -> "1Str"；
		/// <para/>如果要包含空格："Str__Str" -> "Str Str"；
		/// <para/>如果要包含路径分隔符："StrSPStr" -> "Str/Str"。
		/// </remarks>
		/// <param name="s">附加的枚举值</param>
		public static string TS(this Enum e) {
			string s = e.ToString();
			if(s.StartsWith("__"))
				s = s.Substring(2);
			else if(s.Contains("__"))
				s = s.Replace("__", " ");
			else if(s.Contains("SP"))
				s = s.Replace("SP", "/");
			return s;
		}

		
//		/// <summary>检查当前动画状态</summary>
//		/// <param name="animator">附加的动画控制器</param>
//		/// <param name="stateName">指定的状态名称</param>
//		/// <param name="layerName">指定的层级名称</param>
//		/// <returns></returns>
//		private static bool CheckState(this Animator animator,PlayerFSMState stateName, PlayerFSMLayer layerName = PlayerFSMLayer.BaseLayer) {
//			int layerIndex = animator.GetLayerIndex(layerName.TS());
//			bool result = animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName.TS());
//			return result;
//		}
	}
}