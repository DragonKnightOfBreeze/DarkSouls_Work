//TODO：需要自己用手柄调试，看看输入是否正确
//TODO：重构：JInput重命名

namespace DSWork.Global {
	#region ［游戏全局的枚举］

	/// <summary>游戏对象的标签</summary>
	public enum ETag {
		Player,
		Enemy,
		Weapon
	}

	/// <summary>游戏对象的层级</summary>
	public enum ELayer {
		Default = 0,
		TransparentFX = 1,
		Ignore__Raycast = 2,
		Water = 4,
		UI = 5,

		PostProcessing = 8,
		Ground = 9,
		Weapon = 10,
		Enemy = 11,
		Player = 12
	}

	#endregion


	#region ［玩家的枚举］

	/// <summary>玩家的FSM层级</summary>
	public enum EPlayer_FSMLayer {
		BaseLayer,
//			AttackLayer,
		DefenseLayer
	}


	/// <summary>玩家的FSM状态</summary>
	public enum EPlayer_FSMState {
		OnGround,
		Roll,
		Jab,
		Jump,
		Fall,

		RHand_Idle,
		RHand_Slash1,
		RHand_Slash2,
		RHand_Slash3,

		LHand_Idle,
		LHand_ShieldUp
	}

	/// <summary>玩家的FSM状态的标签</summary>
	public enum EPlayer_FSMStateTag {
		Attack
	}


	/// <summary>玩家的FSM参数</summary>
	public enum EPlayer_FSMParam {
		forward,
		right,
		dodge,
		jump,
		isOnGround,
		fallRoll,
		attack,
		defense,
		useLeftHand,
		hurt
	}


	/// <summary>玩家的FSM曲线参数，一般用于设置某一状态中的位移。</summary>
	public enum EPlayer_FSMCurve {
		spd_Jab_Y,
		spd_Jab_Z,
		spd_Roll_Y,
		spd_Jump_Y,

		spd_Atk_1H_Slash1_Z
	}


	/// <summary>玩家的FSM属性控制参数，用于控制动画的属性。</summary>
	public enum EPlayer_FSMProp { }

	#endregion
}