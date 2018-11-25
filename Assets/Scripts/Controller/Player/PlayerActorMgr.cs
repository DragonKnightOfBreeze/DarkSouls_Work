/**
 * ［概述］
 * 
 * 
 * ［用法］
 * 挂载到PlayerContainer上面
 * 
 * ［备注］ 
 * 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSWork.Utility;

namespace DSWork {
	/// <summary>
	/// 玩家的角色管理器
	/// </summary>
	public class PlayerActorMgr : MonoBehaviour {
		[HideInInspector]
		public PlayerActionCtrl actionCtrl;
		[HideInInspector]
		public PlayerBattleMgr battleMgr;
		[HideInInspector]
		public PlayerWeaponMgr weaponMgr;

		void Awake() {
			
			actionCtrl = GetComponent<PlayerActionCtrl>();
			battleMgr = GameObject.Find("Sensor").gameObject.SafeGetComponent<PlayerBattleMgr>();
			battleMgr.actorMgr = this;
			weaponMgr = GetComponent<PlayerWeaponMgr>();
			weaponMgr.actorMgr = this;
		}

		public void DoDamage() {
			actionCtrl.IssueTrigger("hurt");
		}
	}
}