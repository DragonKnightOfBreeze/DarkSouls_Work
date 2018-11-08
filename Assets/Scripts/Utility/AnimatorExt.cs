using DSWork.Global;
using DSWork.Utility;
using UnityEngine;

namespace DSWork {
	/// <summary>动画状态机拓展类</summary>
	public static class AnimatorExt {
		/// <summary>检查当前的动画状态。</summary>
		/// <param name="stateName">指定的状态名称</param>
		/// <param name="layerName">指定的层级名称</param>
		private static bool CheckState(this Animator animator, EPlayer.FSMState stateName, EPlayer.FSMLayer layerName = EPlayer.FSMLayer.BaseLayer) {
			int layerIndex = animator.GetLayerIndex(layerName.TS());
			bool result = animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName.TS());
			return result;
		}
	}
}