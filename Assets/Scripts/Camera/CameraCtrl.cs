//TODO：优化结构（相机模块和玩家模型模块）
//TODO：参考ADF中的相关代码进行重构
//TODO：参考UU中的相关代码进行重构
//TODO：角色的移动应该基于摄像头的角度，也就是Container_Y的角度
 
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
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using System;
using DSWork;
using UnityEngine;

namespace UU_Lesson {
	/// <summary>
	/// 相机控制器
	/// </summary>
	public class CameraCtrl : MonoBehaviour {
		/// <summary>水平视角旋转速度</summary>
		public float horizontalSpeed = 100.0f;
		/// <summary>垂直视角旋转速度</summary>
		public float verticalSpeed = 60.0f;

		private GameObject model;
		private PlayerInput pi;

		
		private GameObject container_Y;
		private GameObject container_X;
		private Camera mainCamera;


		private float tempEulerX;
		private Vector3 cameraDampVelocity;

		private void Awake() {
			model = GameObject.FindWithTag("Player");
			pi = GameObject.Find("_Scripts").transform.Find("_Player").GetComponent<PlayerInput>();

			mainCamera = Camera.main;
			if(mainCamera == null)
				throw new NullReferenceException("空引用：" + nameof(mainCamera));
			container_X = mainCamera.transform.parent.gameObject;
			container_Y = container_X.transform.parent.gameObject;

			tempEulerX = 20;
		}

		private void Update() {
			CameraRotate();
			CameraFollow();
		}

		/// <summary>相机跟随</summary>
		private void CameraFollow() {
			//普通的方式
			//container.transform.position = model.transform.position;
			//使用插值
			//container.transform.position = Vector3.Lerp(container.transform.position, model.transform.position, 0.25f);
			//相机跟随，使用平滑增加
			mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, model.transform.position, ref cameraDampVelocity, 0.05f);
			//方向处理
			mainCamera.transform.LookAt(container_X.transform.forward);
		}

		/// <summary>相机旋转</summary>
		private void CameraRotate() {
			//水平方向视角旋转，有负号（如果不是Model的父物体）
			container_Y.transform.Rotate(Vector3.up, -pi.Sgn_VRight * horizontalSpeed * Time.deltaTime);

			//垂直方向视角旋转，有旋转角度限制，有负号
			tempEulerX -= pi.Sgn_VUp * verticalSpeed * Time.deltaTime;
			tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
			container_X.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

			
			
//			container_X.transform.Rotate(Vector3.right,-pi.VRight*verticalSpeed* Time.deltaTime);
//			//限制垂直方向视角旋转角度（有问题）
//			container_Y.transform.eulerAngles = new Vector3(Mathf.Clamp(container_Y.transform.eulerAngles.x,-40,30),0,0);


		}
	}
}