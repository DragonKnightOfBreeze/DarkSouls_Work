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

using UnityEngine;

namespace DSWork {
	public class JoyStickTest : MonoBehaviour {
		private void Update() {
			print(Input.GetAxis("horizontal"));
			print(Input.GetAxis("vertical"));
		}
	}
}