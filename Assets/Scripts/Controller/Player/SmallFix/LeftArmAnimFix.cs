/**
 * ［概述］
 * 修正左手动画，改为未举盾的模样
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

using DSWork.Global;
using DSWork.Utility;
using UnityEngine;

namespace DSWork {
	public class LeftArmAnimFix : MonoBehaviour {
		//new Vector3(0,-60,15)
		public Vector3 v;
		
		private Animator animator;
		private PlayerActionCtrl actionCtrl;
		
		
		void Awake() {
			animator = gameObject.GetComponent<Animator>();
			actionCtrl = gameObject.FindParent().GetComponent<PlayerActionCtrl>();
		}

		
		void OnAnimatorIK(int layerIndex) {
			if(!actionCtrl.useShield_LHand)
				return;
			if(animator.GetBool(EPlayer_FSMParam.defense.TS()))
				return;
			Transform leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
			leftLowerArm.localEulerAngles += v;
			//设置骨骼的本地旋转，欧拉角转为四元数
			animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm,Quaternion.Euler(leftLowerArm.localEulerAngles));
			
		}

	}
}