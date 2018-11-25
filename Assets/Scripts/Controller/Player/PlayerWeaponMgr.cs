/**
 * ［概述］
 * 
 * 
 * ［用法］
 * 挂载到PlayerContainer上
 * 
 * ［备注］ 
 * 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using DSWork.Utility;
using UnityEngine;

namespace DSWork {
	/// <summary>玩家的武器管理器</summary>
	public class PlayerWeaponMgr : MonoBehaviour {
		[HideInInspector]
		public PlayerActorMgr actorMgr;

		private GameObject weaponContainer_L;
		private GameObject weaponContainer_R;

		private Collider weaponCol_L;
		private Collider weaponCol_R;

		private void Start() {
			weaponContainer_L = gameObject.FindChildInAll("weaponContainer_L");
			weaponContainer_R = gameObject.FindChildInAll("weaponContainer_R");

			weaponCol_L = weaponContainer_L.GetComponentInChildren<Collider>();
			weaponCol_R = weaponContainer_R.GetComponentInChildren<Collider>();
		}

		/// <summary>启用武器触发器</summary>
		public void EnableWeapon(string hand = "right") {
			if(hand == "left")
				weaponCol_L.enabled = true;
			else if(hand == "right")
				weaponCol_R.enabled = true;
		}


		/// <summary>禁用武器触发器</summary>
		public void DisableWeapon(string hand = "right") {
			if(hand == "left")
				weaponCol_L.enabled = false;
			else if(hand == "right")
				weaponCol_R.enabled = false;
		}
	}
}