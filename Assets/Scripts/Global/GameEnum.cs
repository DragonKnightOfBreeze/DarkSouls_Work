//TODO：需要自己用手柄调试，看看输入是否正确
//TODO：重构：JInput重命名

namespace DSWork.Global {
	
	#region ［输入］

	/// <summary>键鼠输入坐标</summary>
	/// <remarks>需要在InputManager中妥善配置。</remarks>
	public enum KAxis {
		/// <summary>向前移动</summary>
		Forward,
		/// <summary>向后移动</summary>
		Back,
		/// <summary>向左移动</summary>
		Left,
		/// <summary>向右移动</summary>
		Right,
		/// <summary>视角向上移动</summary>
		VUp,
		/// <summary>视角向下移动</summary>
		VDown,
		/// <summary>视角向左移动</summary>
		VLeft,
		/// <summary>视角向下移动</summary>
		VRight
	}
	
	
	/// <summary>键鼠输入按钮</summary>
	/// <remarks>需要在InputManager中妥善配置。</remarks>
	public enum KButton {
		/// <summary>切换魔法</summary>
		ToggleMagic,
		/// <summary>切换道具</summary>
		ToggleItem,
		/// <summary>切换左手武器</summary>
		ToggleLHand,
		/// <summary>切换右手武器</summary>
		ToggleRHand,

		/// <summary>奔跑</summary>
		Run,
		/// <summary>闪避</summary>
		Dodge,
		/// <summary>跳跃</summary>
		Jump,
		/// <summary>使用物品</summary>
		UseItem,
		/// <summary>互动</summary>
		Interact,
		/// <summary>切换武器持有方式</summary>
		ToggleHold,

		/// <summary>左手主要动作</summary>
		LHandAct1,
		/// <summary>左手次要动作</summary>
		LHandAct2,
		/// <summary>右手主要动作</summary>
		RHandAct1,
		/// <summary>右手次要动作</summary>
		RHandAct2,

		/// <summary>静步</summary>
		Walk,
		/// <summary>锁定/重置视角</summary>
		Lock,
		/// <summary>打开菜单</summary>
		Menu,
		/// <summary>打开次要菜单</summary>
		SecMenu
	}

	/// <summary>手柄输入坐标</summary>
	/// <remarks>需要在InputManager中妥善配置。</remarks>
	public enum JAxis {
		/// <summary>左右移动</summary>
		J_Right_Left,
		/// <summary>前后移动</summary>
		J_Forward_Back,
		/// <summary>视角左右移动</summary>
		J_VRight_Left,
		/// <summary>视角上下移动</summary>
		J_VUp_Down
	}
	

	/// <summary>手柄输入按钮</summary>
	/// <remarks>需要在InputManager中妥善配置。</remarks>
	public enum JButton {
		/// <summary>切换魔法</summary>
		J_ToggleMagic,
		/// <summary>切换道具</summary>
		J_ToggleItem,
		/// <summary>切换左手武器</summary>
		J_ToggleLHand,
		/// <summary>切换右手武器</summary>
		J_ToggleRHand,

		/// <summary>奔跑（默认：JS_Button0）</summary>
		J_Run,
		/// <summary>闪避（默认：JS_Button0）</summary>
		J_Dodge,
		/// <summary>跳跃（默认：JS_Button0）</summary>
		J_Jump,
		/// <summary>使用物品（默认：JS_Button1）</summary>
		J_UseItem,
		/// <summary>互动（默认：JS_Button2）</summary>
		J_Interact,
		/// <summary>切换武器持有方式（默认：JS_Button3）</summary>
		J_ToggleHold,

		/// <summary>左手主要动作（默认：JS_LB）</summary>
		J_LHandAct1,
		/// <summary>左手次要动作（默认：JS_LT）</summary>
		J_LHandAct2,
		/// <summary>右手主要动作（默认：JS_RB）</summary>
		J_RHandAct1,
		/// <summary>右手次要动作（默认：JS_RT）</summary>
		J_RHandAct2,

		/// <summary>静步（默认：JS_L3）</summary>
		J_Walk,
		/// <summary>锁定/重置视角（默认：JS_R3）</summary>
		J_Lock,
		/// <summary>打开菜单（默认：JS_Start）</summary>
		J_Menu,
		/// <summary>打开次要菜单（默认：JS_Select）</summary>
		J_SecMenu
	}

	#endregion

	
	#region ［游戏全局的枚举］

	/// <summary>游戏对象的标签</summary>
	public enum Tag {
		Player,
		Enemy
	}

	/// <summary>游戏对象的层级</summary>
	public enum Layer {
		Default = 0,
		TransparentFX = 1,
		Ignore__Raycast = 2,
		Water = 4,
		UI = 5,

		PostProcessing = 8,
		Ground = 9,
		Weapon = 10,
		Enemy = 11
	}

	#endregion


	#region ［玩家的枚举］

	/// <summary>玩家的枚举类</summary>
	public static class EPlayer {
		/// <summary>玩家的FSM层级</summary>
		public enum FSMLayer {
			BaseLayer,
			AttackLayer,
			DefenseLayer
		}


		/// <summary>玩家的FSM状态</summary>
		public enum FSMState {
//			Idle,
//			Walk,
//			Run,

			OnGround,
			Dodge,
			Roll,
			Jump,
			Fall,

			Idle,
			__1Hand_Slash1,
			__1Hand_Slash2,
			__1Hand_Slash3,

			__1Hand_ShieldUp
		}


		/// <summary>玩家的FSM参数</summary>
		public enum FSMParam {
			forward,
			right,
			dodge,
			jump,
			isOnGround,
			fallRoll,
			attack,
			defense
		}


		/// <summary>玩家的FSM曲线参数</summary>
		/// <remarks>一般用于设置某一状态中的位移。</remarks>
		public enum FSMCurve {
			spd_Jab_Y,
			spd_Jab_Z,
			spd_Roll_Y,
			spd_Jump_Y,

			spd_Atk_1H_Slash1_Z
		}


		/// <summary>玩家的FSM属性控制参数</summary>
		/// <remarks>一般用于控制动画的属性</remarks>
		public enum FSMProp { }
	}

	#endregion
}