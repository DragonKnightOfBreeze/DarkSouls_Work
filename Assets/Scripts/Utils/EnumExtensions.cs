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

namespace DSWork.Utils {
	/// <summary>枚举类型的拓展类</summary>
	public static class EnumExtensions {
		
		/// <summary>枚举值转为字符串</summary>
		public static string S(this Enum e) {
			return e.ToString();
		}

		/// <summary>
		/// 枚举值转为字符串（将下划线替换为空格）
		/// </summary>
		public static string SR(this Enum e) {
			return e.ToString().Replace("_", " ");
		}
	}
}