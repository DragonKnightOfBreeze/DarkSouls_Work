/*******
 * ［概述］
 * 项目：《黑暗之魂》复刻教程
 * 作者：微风的龙骑士 风游迩
 * 
 * 地面接触检测
 * 
 * ［功能］
 * 
 * 
 * ［思路］ 
 * 另外绘制一个胶囊体，检测是否发生碰撞。
 *
 * ［备注］
 * 一个简便许多的方法：if(cc.isGrounded) ...
 */

using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Experimental.PlayerLoop;

namespace DSWork {
	/// <summary> 
	/// 地面接触检测
	/// </summary>
	public class OnGroundSensor : MonoBehaviour {

//		/// <summary>
//		/// 角色的胶囊碰撞体，真正所用的应该比原本的大一些
//		/// </summary>
//		public CapsuleCollider Capsule;
		/// <summary>
		/// 改进：直接使用角色控制器的碰撞体
		/// </summary>
		public CharacterController cc;
		public float offset = 0.1f;
		
		private Vector3 point1;		//下方的球形的球心
		private Vector3 point2;		//上方的球形的球心
		private float radius;


		private void Awake() {
//			radius = cc.radius - 0.05f;
			radius = cc.radius;
		}
		
		private void FixedUpdate() {
			bool result = OnGroundCheck();
			if( !result) {
				SendMessage("IsNotOnGround");
			}
			else {
				SendMessage("IsOnGround");
			}
		}

		/// <summary>
		/// 判断是否在地面上的具体方法
		/// <para/>如果两个胶囊体之间发生了碰撞，单位在地面上
		/// </summary>
		/// <remarks>后续可能需要更改、优化</remarks>
		/// <returns>true：在地面上，false：不再地面上</returns>
		private bool OnGroundCheck() {
//			radius = Capsule.radius - offset;
//			point1 = transform.position + transform.up * (radius - offset);
//			point2 = transform.position + transform.up * Capsule.height + transform.up * (radius - offset);
			
			point1 = transform.position + transform.up * (radius - offset);
			point2 = transform.position + transform.up * cc.height - transform.up * (radius - offset);
			Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius,LayerMask.GetMask("Ground"));
			
			bool result = outputCols.Length > 0;
			return result;
		}
	}
}