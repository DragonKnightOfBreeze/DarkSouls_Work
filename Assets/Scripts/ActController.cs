/*******
 * ［概述］
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 * 
 * 动画控制器控制
 * 
 * ［功能］
 * 
 * 
 * ［思路］ 
 *
 *
 * ［备注］
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSWork {
	/// <summary> 
	/// 动画控制器控制
	/// </summary>
	public class ActController:MonoBehaviour {
		public GameObject Model;
		public float walkSpeed = 2f;

		private Animator animator;
		private PlayerInput pi;
		private CharacterController cc;
		/// <summary>
		/// 移动速度
		/// </summary>
		private Vector3 moveSpeed;

		void Awake() {
			//得到组件
			animator = Model.GetComponent<Animator>() ?? Model.AddComponent<Animator>();
			pi = Model.GetComponent<PlayerInput>() ?? Model.AddComponent<PlayerInput>();
			cc = Model.GetComponent<CharacterController>() ?? Model.AddComponent<CharacterController>();
		}

		//RigidBody不应该在update方法中调用
		//rigid.position = moveSpeed * Time.fixedDeltaTime;
		//rigid.velocity = moveSpeed; 	//会覆盖y值


		private void OnEnable() {
			StartCoroutine(PlayerMoveCr());
		}
		
		IEnumerator PlayerMoveCr() {
			while(true) {
				PlayerMove();
				yield return new WaitForSeconds(0.02f);
			}
		}
		
		private void PlayerMove() {
			//设置方向
			if(pi.DMag >0.1f)
				Model.transform.forward = pi.DVec;
			//设置混合参数
			animator.SetFloat("forward",pi.DMag);

			//设置玩家速度（二维）
			moveSpeed = pi.DMag * Model.transform.forward * walkSpeed ;
			var fixedMoveSpeed = new Vector3(moveSpeed.x, transform.position.y, moveSpeed.z);
			//移动
			cc.Move(fixedMoveSpeed * Time.deltaTime);	
		}


	}
}