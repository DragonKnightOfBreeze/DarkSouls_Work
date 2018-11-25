//TODO：优化结构

/*******
 * ［概述］
 * AI的相机控制器
 * 
 * ［用法］
 * 挂载到EnemyContainer上
 * 
 * ［备注］ 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using DSWork.Utility;
using DSWork.Global;
using UnityEngine;

namespace DSWork.Enemy {
	/// <summary>AI的相机控制器</summary>
	public class EnemyCameraCtrl : MonoBehaviour {
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


		private LockTarget lockTarget;
		private GameObject model;
		private PlayerInputMgr pi;

//		private Camera enemyCamera;
		private GameObject cameraOffset;
		private GameObject cameraRotation_X;
		private GameObject cameraRotation_Y;

		private float tempEulerX;
		private Vector3 cameraDampVelocity;

		private void Awake() {
//			_LockIcon.enabled = false;

//			model = gameObject;
			pi = ScriptTool.GetScript<PlayerInputMgr>("_Player");

			model = gameObject.FindChild("TestEnemy");
			cameraRotation_Y = gameObject.FindChild("CameraRotation_Y");
			cameraRotation_X = cameraRotation_Y.FindChild("CameraRotation_X");
			cameraOffset = cameraRotation_X.FindChild("CameraOffset");
//			enemyCamera = gameObject.GetComponent<Camera>() ?? gameObject.AddComponent<Camera>();
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
			var modelOrigin1 = model.transform.position;
			var modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
			var boxCenter = modelOrigin2 + model.transform.forward * 10.0f;
			//从某一点，构建某个物体，指定某个层级，返回该层级中在这个物体中的碰撞体数组
			var cols = Physics.OverlapBox(boxCenter, new Vector3(4f, 5f, 10f), model.transform.rotation, LayerMask.GetMask(ELayer.Player.TS()));
			//TODO：如何较完美地切换锁定的敌人？
			if(cols.Length == 0) {
				lockTarget = null;
			}
			//如果有可以锁定的对象，则得到首先锁定的敌人
			else {
				foreach(var col in cols) {
					if(lockTarget != null && lockTarget.go == col.gameObject) {
						lockTarget = null;
						break;
					}
					lockTarget = new LockTarget(col.gameObject);
					//定位并启用锁定图标
					var pos = lockTarget.go.transform.position;
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


		/// <summary>相机旋转（锁定状态下）</summary>
		private void CameraRotate_Locked() {
			var tempForward = lockTarget.go.transform.position - model.transform.position;
			tempForward.y = 0;
			//锁定时调整方向
			model.transform.forward = tempForward;
			cameraRotation_Y.transform.forward = tempForward;

			//锁定时看向合适的位置（大约以锁定目标处的地面为中心）
			var targetPos = lockTarget.go.transform.position;
			cameraRotation_X.transform.LookAt(new Vector3(0, targetPos.y - lockTarget.halfHeight, 0));

			//TODO：定位锁定图标，并且考虑模型遮挡。（在目标的中心位置）
			var pos = lockTarget.go.transform.position + new Vector3(0, 0, 0);

			//当距离过远时解除锁定
			float distance = Vector3.Distance(model.transform.position, lockTarget.go.transform.position);
			if(distance > MaxLockDistance) {
				lockTarget = null;
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