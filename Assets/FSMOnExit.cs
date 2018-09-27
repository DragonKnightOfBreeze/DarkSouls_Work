using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace DSWork {
	/// <summary>
	/// 挂载在某一动画状态上，以使退出该动画状态时，可以自动调用指定的方法。
	/// </summary>
	public class FSMOnExit : StateMachineBehaviour {

		public string[] onExitMessages;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) {
			foreach(var msg in onExitMessages) {
				animator.gameObject.SendMessageUpwards(msg);
			}
		}
	}
}


