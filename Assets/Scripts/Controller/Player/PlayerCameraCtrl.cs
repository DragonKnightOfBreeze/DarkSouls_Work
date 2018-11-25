//TODO：优化结构（相机模块和玩家模型模块）
//TODO：参考ADF中的相关代码进行重构
//TODO：参考UU中的相关代码进行重构

//DONE：角色的移动应该相对于摄像头的正方向
//TODO：锁定时跳跃，会沿着圆周方向跳出去，而非切线方向
//TODO：实现要锁定但没有可锁定的目标时，重置视角的功能

/*******
 * ［概述］
 * 相机控制器（不直接控制相机）
 * 
 * ［用法］
 * 挂载到_Scripts/_Camera上。
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
using DSWork.Utility;
using DSWork.Global;
using UnityEngine;
using UnityEngine.UI;

namespace DSWork {
	/// <summary>玩家的相机控制器</summary>
	public class PlayerCameraCtrl : MonoBehaviour {
		/// <summary>最大锁定距离</summary>
		public float MaxLockDistance { set; get; } = 10f;
		/// <summary>是否处于锁定状态</summary>
		public bool LockState => lockTarget != null;
		/// <summary>主摄像机只在平面上的正方向</summary>
		public Vector3 CameraForward => cameraRotation_Y.transform.forward;
		public Vector3 CameraYRight => cameraRotation_Y.transform.right;


		/// <summary>水平视角旋转速度</summary>
		public float horizontalSpeed = 100.0f;
		/// <summary>垂直视角旋转速度</summary>
		public float verticalSpeed = 60.0f;

		public Image _LockIcon;

		private LockTarget lockTarget;
		private GameObject model;
		private PlayerInputMgr inputMgr;

		private Camera mainCamera;
		private GameObject cameraRotation_X;
		private GameObject cameraRotation_Y;

		private float tempEulerX;
		private Vector3 cameraDampVelocity;

		private void Awake() {
			//使鼠标不可见
			Cursor.visible = false;
			_LockIcon.enabled = false;

			model = gameObject.FindChildWithTag(ETag.Player);
			inputMgr = gameObject.GetComponent<PlayerInputMgr>();

			mainCamera = Camera.main;
			if(mainCamera == null)
				throw new NullReferenceException("空引用：" + nameof(mainCamera));
			cameraRotation_X = mainCamera.transform.parent.gameObject;
			cameraRotation_Y = cameraRotation_X.transform.parent.gameObject;

			tempEulerX = 20;
		}

		private void FixedUpdate() {
			if(!LockState)
				CameraRotate();
			else
				CameraRotate_Locked();
			CameraFollow();
		}


		/// <summary>切换锁定</summary>
		public void ToggleLock() {
			//执行检测
			var modelOrigin1 = model.transform.position;
			var modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
			var boxCenter = modelOrigin2 + model.transform.forward * 10.0f;
			//从某一点，构建某个物体，指定某个层级，返回该层级中在这个物体中的碰撞体数组
			var cols = Physics.OverlapBox(boxCenter, new Vector3(4f, 5f, 10f), model.transform.rotation, LayerMask.GetMask(ELayer.Enemy.TS()));
			//TODO：如何较完美地切换锁定的敌人？
			if(cols.Length == 0) {
				lockTarget = null;
				_LockIcon.enabled = false;
			}
			//如果有可以锁定的对象，则得到首先锁定的敌人
			else {
				foreach(var col in cols) {
					//如果已经锁定当前目标，则接触锁定
					if(lockTarget != null && lockTarget.go == col.gameObject) {
						lockTarget = null;
						_LockIcon.enabled = false;
						break;
					}
					lockTarget = new LockTarget(col.gameObject);
					//定位并启用锁定图标
					var pos = lockTarget.go.transform.position;
					_LockIcon.transform.position = new Vector3(pos.x, pos.y, 0);
					_LockIcon.enabled = true;
					break;
				}
			}
		}


		/// <summary>相机跟随</summary>
		private void CameraFollow() {
			//相机跟随，使用平滑增加
			mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, model.transform.position, ref cameraDampVelocity, 0.05f);
			//方向处理
//			mainCamera.transform.LookAt(cameraRotation_Y.transform.forward);
		}


		/// <summary>相机旋转</summary>
		private void CameraRotate() {
			//水平方向视角旋转
			cameraRotation_Y.transform.Rotate(Vector3.up, inputMgr.Sgn_VRight * horizontalSpeed * Time.deltaTime);

			//垂直方向视角旋转，有旋转角度限制，有负号
			tempEulerX -= inputMgr.Sgn_VUp * verticalSpeed * Time.deltaTime;
			tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
			cameraRotation_X.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
		}


		/// <summary>相机旋转（锁定状态下）</summary>
		private void CameraRotate_Locked() {
			//锁定时调整玩家和摄像机的平面方向
			var tempForward = lockTarget.go.transform.position - model.transform.position;
			tempForward.y = 0;
			model.transform.forward = tempForward;
			cameraRotation_Y.transform.forward = tempForward;
			//锁定时调整视角高度，看向合适的位置（大约以锁定目标处的地面为中心）
			var targetPos = lockTarget.go.transform.position;
			cameraRotation_X.transform.LookAt(new Vector3(0, targetPos.y - lockTarget.halfHeight, 0));
			//DONE：定位锁定图标（大约在锁定目标的中心位置）
			//TODO：考虑模型遮挡
			var pos = lockTarget.go.transform.position + new Vector3(0, 0, 0);
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