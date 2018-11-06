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
using System.CodeDom;
using DSWork.Global;
using UnityEngine;

namespace DSWork.Utility {
	/// <summary>枚举类型的拓展类</summary>
	public static class EnumTool {
		
		private const string REP_Num = "__";
		private const string REP_Space = "__";
		private const string REP_Sep = "SP"; 
		
		
		/// <summary>枚举值转为字符串</summary>
		/// <remarks>（默认）如果要以数字开头："__1Str" -> "1Str"；</remarks>
		/// <remarks>（默认）如果要包含空格："Str__Str" -> "Str Str"；</remarks>
		/// <remarks>（默认）如果要包含路径分隔符："StrSPStr" -> "Str/Str"。</remarks>
		/// <param name="s">附加的枚举值</param>
		public static string TS(this Enum e) {
			string s = e.ToString();
			if(s.StartsWith(REP_Num, StringComparison.Ordinal))
				s = s.Substring(REP_Num.Length);
			else if(s.Contains(REP_Space))
				s = s.Replace(REP_Space, " ");
			else if(s.Contains(REP_Sep))
				s = s.Replace(REP_Sep, "/");
			return s;
		}


		
		

	}
}