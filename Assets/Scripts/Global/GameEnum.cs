namespace DSWork.Global {
	public class GameEnum { }

	#region ［输入］

	/// <summary>键鼠输入</summary>
	public enum KMInput {
		//Input.GetKeyDown()
		KW_Froward,
		KW_Back,
		KW_Left,
		KW_Right,

		KW_RHandAct1,
		KW_RHandAct2,
		KW_LHandAct1,
		KW_LHandAct2,

		KW_Walk,

		KW_Run,
		KW_Dodge,
		KW_Use,
		KW_Inter,

		KW_Lock,

		KW_Menu
		//Input.GetMouseDown()
	}

	/// <summary>手柄输入</summary>
	/// <remarks>需要在InputManager中妥善配置</remarks>
	public enum JSInput {
		//TODO：需要自己用手柄调试
		//Input.GetAxis()
		JS_LeftAxis_X,
		JS_LeftAxis_Y,
		JS_RightAxis_X,
		JS_RightAxis_Y,
		JS_Direction_X,
		JS_Direction_Y,
		//Input.GetButtonDown()
		JS_Button0,
		JS_Button1,
		JS_Button2,
		JS_Button3,
		JS_LB,
		JS_RB,
		JS_LT,
		JS_RT,
		JS_L3,
		JS_R3,
		JS_Select,
		JS_Start
	}

	#endregion

	

	#region ［玩家的动画状态机］

	/// <summary>玩家的FSM层级</summary>
	public enum PlayerFSMLayer {
		BaseLayer,
		AttackLayer
	}

	/// <summary>玩家的FSM状态</summary>
	public enum PlayerFSMState {
//		Idle,
//		Walk,
//		Run,
		OnGround,
		Dodge,
		Roll,
		Jump,
		Fall,

		Idle,
		Atk_1H_Slash1,
		Atk_1H_Slash2,
		Atk_1H_Slash3
	}

	/// <summary>玩家的FSM参数</summary>
	public enum PlayerFSMParam {
		forward,
		jab_roll_jump,
		isOnGround,
		fallRoll,
		attack
	}

	/// <summary>玩家的FSM曲线参数</summary>
	/// <remarks>一般用于设置某一状态中的位移。</remarks>
	public enum PlayerFSMCurve {
		spd_Jab_Y,
		spd_Jab_Z,
		spd_Roll_Y,
		spd_Jump_Y,

		spd_Atk_1H_Slash1_Z
	}

	/// <summary>
	/// 玩家的FSM属性控制参数
	/// </summary>
	/// <remarks>一般用于控制动画的属性</remarks>
	public enum PlayerFSMProp {
		
	}
	

	#endregion
}