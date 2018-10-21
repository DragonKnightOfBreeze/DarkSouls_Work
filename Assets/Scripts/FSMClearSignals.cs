using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace DSWork {

	/// <summary> 
	/// 解决累积Trigger的问题：添加到动画状态上，使之在进入时消除另一Trigger参数
	/// </summary>
	public class FSMClearSignals : StateMachineBehaviour {
	
		//在编辑器上添加
		public string[] clearAtEnter;
		public string[] clearAtExit;
	
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) {
			foreach(var signal in clearAtEnter) {
				animator.ResetTrigger(signal); //清空信号
				
			}
		}
	
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) {
			foreach(var signal in clearAtExit) {
				animator.ResetTrigger(signal); //清空信号
			}
		}
	}
}
