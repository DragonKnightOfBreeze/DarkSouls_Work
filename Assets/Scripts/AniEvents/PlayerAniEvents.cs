/*******
 * ［概述］
 * 玩家的动画事件
 * 
 * ［用法］
 * 插入到具体的动画帧上。
 * 
 * ［备注］ 
 * 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */
using UnityEngine;

namespace DSWork {
	/// <summary>玩家的动画事件</summary>
	public class PlayerAniEvents : MonoBehaviour {
		private Animator animator;

		private void Awake() {
			animator = GetComponent<Animator>();
		}

//		/// <summary>
//		/// 重置Trigger类型的FSM参数
//		/// </summary>
//		/// <param name="triggers"></param>
//		public void ResetTrigger(params PlayerFSMParam[] triggers) {
//			foreach(var trigger in triggers) {
//				//如果这个参数不是Trigger类型的怎么办？
//				animator.ResetTrigger(trigger.ToString());
//			}
//		}

		public void ResetTrigger(string trigger) {
			animator.ResetTrigger(trigger);
		}
	}
}