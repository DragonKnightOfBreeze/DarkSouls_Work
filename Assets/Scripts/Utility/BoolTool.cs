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
using System.Collections;
using System.Collections.Generic;

namespace DSWork {
	public static class BoolTool {
		
		/// <summary>转换为0或1</summary>
		public static float To1_0(this bool b) {
			return b ? 1.0f : 0f;
		}

		/// <summary>转换为2或0</summary>
		public static float To2_1(this bool b) {
			return b ? 2.0f : 1.0f;
		}
	}
}