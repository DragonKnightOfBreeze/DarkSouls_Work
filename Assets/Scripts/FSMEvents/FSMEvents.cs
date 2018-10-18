using DSWork.Global;
using UnityEngine;
using UnityEngine.Animations;

namespace DSWork {
	/// <summary>
	/// 挂载在某一动画状态上，以使进入该动画状态时，可以自动调用指定的方法。
	/// </summary>
	public class FSMEvents : StateMachineBehaviour {

		public event FSMDelegate OnEnterEvent;
		public event FSMDelegate OnExitEvent;
		public event FSMDelegate OnUpdateEvent;

		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) {
			OnEnterEvent?.Invoke();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			OnExitEvent?.Invoke();
		}
		
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			OnUpdateEvent?.Invoke();
		}
	}
}