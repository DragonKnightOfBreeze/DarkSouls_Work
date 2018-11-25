using UnityEngine;

namespace DSWork.Utility {
	/// <summary>解决累积Trigger的问题：添加到FSM状态上，使之在进入时消除另一Trigger参数。</summary>
	/// <remarks>需要在编辑器上妥善配置。</remarks>
	/// >
	public class FSMClearSignals : StateMachineBehaviour {
		public string[] clearAtEnter;
		public string[] clearAtExit;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
			foreach(string signal in clearAtEnter)
				animator.ResetTrigger(signal); //清空信号
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
			foreach(string signal in clearAtExit)
				animator.ResetTrigger(signal); //清空信号
		}
	}
}