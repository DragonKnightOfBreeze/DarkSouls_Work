//TODO：优化结构（相机模块和玩家模型模块）
//TODO：参考ADF中的相关代码进行重构
//TODO：参考UU中的相关代码进行重构

//TODO：角色的移动应该相对于摄像头的正方向
//TODO：锁定时跳跃，会沿着圆周方向跳出去，而非切线方向
//TODO：实现要锁定但没有可锁定的目标时，重置视角的功能

/*******
 * ［概述］
 * 相机控制器（不直接控制相机）
 * 
 * ［用法］
 * 挂载到_Scripts上。
 * 
 * ［备注］ 
 * 角色的移动应该基于摄像头的角度，也就是Container_Y的角度
 * - 最直接了当的方法：将Container_Y设为Model的父GO
 * - 另一种方法：得到mainCamera的世界位置，玩家基于这个世界位置进行移动
 * 
 * 处理在小幅位移时的镜头晃动问题：在Animator中把Update Mode改为Animate Physics
 *
 * 画面正中心的位置大概是锁定敌人的脚底，锁定图标位置则在上方半高处。
 * 敌人的Y坐标在这里是0。
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System;
using DSWork;
using DSWork.Utility;
using DSWork.Global;
using UnityEngine;
using UnityEngine.UI;

namespace DSWork {
	/// <summary>相机控制器</summary>
	public class CameraControl : MonoBehaviour {

		/// <summary>最大锁定距离</summary>
		public float MaxLockDistance { set; get; } = 10f;
		/// <summary>是否处于锁定状态</summary>
		public bool LockState => lockTarget != null;
		/// <summary>主摄像机只在平面上的正方向</summary>
		public Vector3 CameraYForward => cameraRotation_Y.transform.forward;

		
		/// <summary>水平视角旋转速度</summary>
		public float horizontalSpeed = 100.0f;
		/// <summary>垂直视角旋转速度</summary>
		public float verticalSpeed = 60.0f;

		public Image _LockIcon;

		private LockTarget lockTarget;
		private GameObject model;
		private PlayerInput pi;

		private Camera mainCamera;
		private GameObject cameraOffset;
		private GameObject cameraRotation_X;
		private GameObject cameraRotation_Y;

		private float tempEulerX;
		private Vector3 cameraDampVelocity;

		private void Awake() {
			//使鼠标不可见
			Cursor.visible = false;
			_LockIcon.enabled = false;

			model = GameObject.FindWithTag("Player");
			pi = ScriptTool.GetScript<PlayerInput>("_Player");

			mainCamera = Camera.main;
			if(mainCamera == null)
				throw new NullReferenceException("空引用：" + nameof(mainCamera));
			cameraOffset = mainCamera.transform.parent.gameObject;
			cameraRotation_X = cameraOffset.transform.parent.gameObject;
			cameraRotation_Y = cameraRotation_X.transform.parent.gameObject;

			tempEulerX = 20;
		}

		private void FixedUpdate() {
			if(lockTarget == null)
				CameraRotate();
			else
				CameraRotate_Locked();

			CameraFollow();
		}

		
		/// <summary>切换锁定</summary>
		public void ToggleLock() {
			var modelOrigin1 = model.transform.position;
			var modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
			var boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
			//从某一点，构建某个物体，指定某个层级，返回该层级中在这个物体中的碰撞体数组
			var cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, LayerMask.GetMask(Layer.Enemy.TS()));

			//TODO：如何较完美地切换锁定的敌人？
			if(cols.Length == 0) {
				lockTarget = null;
				_LockIcon.enabled = false;
			}
			//如果有可以锁定的对象，则得到首先锁定的敌人
			else {
				foreach(var col in cols) {
					if(lockTarget != null && lockTarget.go == col.gameObject) {
						lockTarget = null;
						_LockIcon.enabled = false;
						break;
					}
					lockTarget = new LockTarget(col.gameObject);
					//启用并定位锁定图标
					_LockIcon.enabled = true;
					var pos = lockTarget.go.transform.position;
					_LockIcon.transform.position = new Vector3(pos.x, pos.y, 0);
					break;
				}
			}

		}


		/// <summary>相机跟随</summary>
		private void CameraFollow() {
			//普通的方式
			//container.transform.position = model.transform.position;
			//使用插值
			//container.transform.position = Vector3.Lerp(container.transform.position, model.transform.position, 0.25f);
			//相机跟随，使用平滑增加
			cameraOffset.transform.position = Vector3.SmoothDamp(cameraOffset.transform.position, model.transform.position, ref cameraDampVelocity, 0.05f);
			//方向处理
//			cameraOffset.transform.LookAt(cameraRotation_Y.transform.forward);
		}


		/// <summary>相机旋转</summary>
		private void CameraRotate() {
			//水平方向视角旋转
			cameraRotation_Y.transform.Rotate(Vector3.up, pi.Sgn_VRight * horizontalSpeed * Time.deltaTime);

			//垂直方向视角旋转，有旋转角度限制，有负号
			tempEulerX -= pi.Sgn_VUp * verticalSpeed * Time.deltaTime;
			tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
			cameraRotation_X.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
		}


		/// <summary>相机旋转（处于锁定状态时）</summary>
		private void CameraRotate_Locked() {
			var tempForward = lockTarget.go.transform.forward - model.transform.position;
			tempForward.y = 0;
			//锁定时调整方向
			model.transform.forward = tempForward;
			cameraRotation_Y.transform.forward = tempForward;

			//锁定时看向合适的位置（大约以锁定目标处的地面为中心）
			cameraRotation_X.transform.LookAt(lockTarget.go.transform);

			//TODO：定位锁定图标，并且考虑模型遮挡。
			var pos = lockTarget.go.transform.position + new Vector3(0, lockTarget.halfHeight, 0);
			_LockIcon.rectTransform.position = mainCamera.WorldToScreenPoint(pos);

			//当距离过远时解除锁定
			float distance = Vector3.Distance(model.transform.position, lockTarget.go.transform.position);
			if(distance > MaxLockDistance) {
				lockTarget = null;
				_LockIcon.enabled = false;
			}
		}


		/// <summary>锁定目标对象</summary>
		private class LockTarget {
			public readonly GameObject go;
			public readonly float halfHeight;

			public LockTarget(GameObject go) {
				this.go = go;
				//得到该游戏对象的角色控制器的半高
				halfHeight = go.GetComponent<CharacterController>().bounds.extents.y;

			}
		}
	}
}