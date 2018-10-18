////TODO：将Unity消息变为委托事件。
////TODO：将stateInfo.shotNameHash作为参数，然后利用Animator.StringToHash(stateName)来判断状态名是否对应。
//
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using DSWork.Global;
//using UnityEngine;
//using UnityEngine.Animations;
//using Debug = UnityEngine.Debug;
//
//namespace DSWork {
//	/// <summary>
//	/// 挂载在某一动画状态上，以使进入该动画状态时，可以自动调用指定的方法。
//	/// </summary>
//	public class FSMOnEnter : StateMachineBehaviour {
//
//		/// <summary>
//		/// 动画状态进入事件
//		/// </summary>
//		public event FSMDelegate FSMOnEnterEvent;
//
//		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) {
////			Debug.Log("状态测试：Jump");
////			Debug.Log(Animator.StringToHash("Jump") == stateInfo.shortNameHash);
//			
////			FSMOnEnterEvent?.Invoke(stateInfo.shortNameHash);
//		}
//	}
//}
//
///*
//	stateInfo.IsName(string name){
//		int hash = Animator.StringToHash(name);
//		return hash == this.m_FullPath || hash == this.m_Name || hash == this.m_Path;
//	}
//
//	public int shortNameHash {
//		get {return this.m_Name;}
//	}
//	
//	public string getStateName(){
//	
//	}
//	
//	FSMOnEnterEvent?.Invoke(stateInfo.shotNameHash);
//	
//	private void OnJumpEnter(int nameHash) {
//		if(Animator.StringToHash("Jump") != nameHash) 
//			return;
//		......
//	}
//
// */
//
//
