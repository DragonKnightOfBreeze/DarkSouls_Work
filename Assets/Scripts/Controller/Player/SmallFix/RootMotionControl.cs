/*******
 * ［概述］
 * 
 * 
 * ［用法］
 * 挂载玩家游戏对象上。
 * 
 * ［备注］ 
 *
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using DSWork.Global;
using UnityEngine;

namespace DSWork {
	public class RootMotionControl : MonoBehaviour {
		/// <summary>Root Motion更新事件</summary>
		public event AnimatorMoveDelegate RMUpdateEvent;


		private Animator animator;

		private void Awake() {
			animator = GetComponent<Animator>();
		}

		private void OnAnimatorMove() {
			//传递相对位置
			RMUpdateEvent?.Invoke(animator.deltaPosition);
		}
	}
}