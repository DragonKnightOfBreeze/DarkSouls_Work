//TODO：优化结构 

/*******
 * ［概述］
 * 
 * 
 * ［用法］
 * 挂载到EnemyContainer上
 * 
 * ［备注］ 
 * 
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 */

using UnityEngine;

namespace DSWork.Enemy {
	public class DummyInputMgr: MonoBehaviour {
		public bool Sgn_Lock;
		
		public bool Sgn_Run;
		
		public bool Sgn_RHandAct1 = true;
		public bool Sgn_RHandAct2;
		public bool Sgn_LHandAct1;
		public bool Sgn_LHandAct2;
		
		public float Forward = 0;
		public float Right = 0;
		public float Distance = 0f;
		public Vector3 Direction = Vector3.zero;

		public EnemyCameraCtrl cameraCtrl;

		void Awake() {
			cameraCtrl = gameObject.GetComponent<EnemyCameraCtrl>();
		}
		
		void Update() {
			HandleInput();
		}
		
		
		#region ［其他私有方法］

		/// <summary>处理用户输入</summary>
		/// <param name="smooth">是否需要进行平滑过渡</param>
		/// <param name="toCircle">是否需要转化为圆形坐标，使对角线长度最大也为1</param>
		private void HandleInput(bool toCircle = true) {
			if(toCircle)
				SquareToCircle(ref Forward, ref Right);
			
			Distance = Mathf.Sqrt(Forward * Forward + Right * Right);
			//DONE：参考系的正方向应该是相机的正方向
			Direction = cameraCtrl.CameraForward * Forward + cameraCtrl.CameraYRight * Right;
		}

		/// <summary>将正方形坐标转化成圆形坐标</summary>
		private void SquareToCircle(ref float x, ref float y) {
			float rawX = x;
			float rawY = y;
			x = rawX * Mathf.Sqrt(1 - rawY * rawY / 2.0f);
			y = rawY * Mathf.Sqrt(1 - rawX * rawX / 2.0f);
		}

		#endregion
	}
}