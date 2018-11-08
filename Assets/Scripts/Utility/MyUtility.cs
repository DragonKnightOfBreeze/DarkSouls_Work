using DSWork.Global;
using UnityEngine;

namespace DSWork.Utility {
	public static class MyUtility {
		
		/// <summary>检查当前动画状态</summary>
		/// <param name="animator">附加的动画控制器</param>
		/// <param name="stateName">指定的状态名称</param>
		/// <param name="layerName">指定的层级名称</param>
		/// <returns></returns>
		private static bool CheckState(this Animator animator,EPlayer.FSMState stateName, EPlayer.FSMLayer layerName = EPlayer.FSMLayer.BaseLayer) {
			int layerIndex = animator.GetLayerIndex(layerName.TS());
			bool result = animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName.TS());
			return result;
		}

		
		/// <summary>
		/// 得到遮罩层级
		/// </summary>
		/// <param name="name">遮罩层级的名字</param>
		/// <returns></returns>
		private static int GetMask(params string[] name) {
			return LayerMask.GetMask(name);
		}
	}
}