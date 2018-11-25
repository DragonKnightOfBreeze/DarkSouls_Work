/**
 * ［概述］
 * 
 * 
 * ［用法］
 * 挂载在PlayerContainer/Sensor上面？
 * 要求有一个cc
 * 
 * ［备注］ 
 * 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System.Collections;
using System.Collections.Generic;
using DSWork.Global;
using DSWork.Utility;
using UnityEngine;

namespace DSWork{
	/// <summary>
	/// 玩家的战斗管理器
	/// </summary>
	[RequireComponent(typeof(CapsuleCollider))]
	public class PlayerBattleMgr : MonoBehaviour {
		[HideInInspector]
		public PlayerActorMgr actorMgr;
		
		
		private CapsuleCollider defCol;
		
		private void Start() {
			defCol = GetComponent<CapsuleCollider>();
			defCol.center = new Vector3(0, 0.9f, 0);
			defCol.height = 1.8f;
			defCol.radius = 0.3f;
			defCol.isTrigger = true;
		}
		
		private void OnTriggerEnter(Collider other) {
			//print(other.name);
			if(other.CompareTag(ETag.Weapon.TS())) {
				
			}
		}
	}
}