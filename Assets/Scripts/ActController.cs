//TODO：OnEnter、OnExit方法可以是lambda表达式，在这个脚本中，注册到对应事件上。
//TODO：改为有限状态机（移动、跳跃、跑动、翻滚等）

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
 * Physics.OverlapCapsule()：检测胶囊体的碰撞
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
	public class ActController : MonoBehaviour {
		public GameObject Model;
		/// <summary>移动速度</summary>
		public float walkSpeed = 2f;
		/// <summary>奔跑速度倍率</summary>
		public float runMultiplier = 2.0f;
		/// <summary>跳跃高度</summary>
		public float jumpHeight = 2f;
		
		
		private Animator animator;
		private PlayerInput pi;
		private CharacterController cc;

		/// <summary>锁死平面移动速度</summary>
		private bool lockPlanar =false;
 
		/// <summary>平面移动速度</summary>
		private Vector3 planerMoveSpeed;

		void Awake() {
			//得到组件
			animator = Model.GetComponent<Animator>() ?? Model.AddComponent<Animator>();
			pi = Model.GetComponent<PlayerInput>() ?? Model.AddComponent<PlayerInput>();
			cc = Model.GetComponent<CharacterController>() ?? Model.AddComponent<CharacterController>();
		}

		//RigidBody不应该在update方法中调用
		//rigid.position = planerMoveSpeed * Time.fixedDeltaTime;
		//rigid.velocity = planerMoveSpeed; 	//会覆盖y值


		private void OnEnable() {
			StartCoroutine(PlayerActionCr());
		}

		IEnumerator PlayerActionCr() {
			yield return new WaitForEndOfFrame();
			while(true) {
				PlayerMove();
				PlayerJump();
				yield return new WaitForSeconds(0.02f);
			}
		}

		/// <summary>
		/// 角色移动方法
		/// </summary>
		private void PlayerMove() {
			//设置混合参数（在待机、移动、奔跑状态之间切换）
			//利用插值函数实现平滑过渡效果，每一次过渡一半。
			float targetRunMulti = pi.DMag * Mathf.Lerp(animator.GetFloat("forward"), pi.run ? 2.0f : 1.0f, 0.5f);
			animator.SetFloat("forward", targetRunMulti);

			//设置方向（旋转）
			if(pi.DMag > 0.1f) {
				//球面线性插值，用于变换向量
				var targetForward = Vector3.Slerp(Model.transform.forward, pi.DVec, 0.3f);
				Model.transform.forward = targetForward;
			}

			//设置玩家速度（二维）（考虑是否奔跑）（考虑是否锁定了平面移动）
			if(!lockPlanar) {
				planerMoveSpeed = pi.DMag * Model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);
			}
			var fixedMoveSpeed = new Vector3(planerMoveSpeed.x,0, planerMoveSpeed.z);

			//玩家移动
			cc.Move(fixedMoveSpeed * Time.deltaTime);
		}




		/// <summary>
		/// 角色跳跃方法
		/// </summary>
		private void PlayerJump() {
			if(!pi.jump)
				return;
			animator.SetTrigger("jump");
//			cc.Move(Vector3.up * jumpHeight * Time.deltaTime);
		}
		
		/*消息控制方法*/
		
		public void OnJumpEnter() {
			//锁死移动
			pi.inputEnabled = false;
			lockPlanar = true;
		}
		
//		public void OnJumpExit() {
//			//解锁移动
//			pi.inputEnabled = true;
//			lockPlanar = false;
//		}
		
		public void OnGroundEnter() {
			//解锁移动
			//应该在角色重新落回地面时解除锁定，而不是在跳跃动画结束时
			pi.inputEnabled = true;
			lockPlanar = false;
		}
		
		
		//相当于在满足条件时，改变动画状态的方法，可以用lambda表达式简化表示。
		
		public void IsOnGround() {
			animator.SetBool("isOnGround",true);
		}
		
		public void IsNotOnGround() {
			animator.SetBool("isOnGround",false);
		}
		
	}
}