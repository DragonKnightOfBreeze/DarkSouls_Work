//TODO：需要自己用手柄调试，看看输入是否正确
//TODO：重构：JInput重命名

namespace DSWork.Global {
	public class GameEnum { }

	#region ［输入］

	/// <summary>键鼠输入</summary>
	/// <remarks>需要在InputManager中妥善配置</remarks>
	public enum KInput {
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
		VRight,

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
		/// <summary>闪避（后跃/翻滚）</summary>
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
		/// <summary>目标锁定/视角重置</summary>
		Lock,
		/// <summary>打开菜单</summary>
		Menu,
		/// <summary>打开次要菜单</summary>
		SecMenu
	}


	/// <summary>手柄输入</summary>
	/// <remarks>需要在InputManager中妥善配置</remarks>
	public enum JInput {
		/// <summary>左右移动</summary>
		JS_LeftAxis_X,
		/// <summary>前后移动</summary>
		JS_LeftAxis_Y,
		/// <summary>视角左右移动</summary>
		JS_RightAxis_X,
		/// <summary>视角上下移动</summary>
		JS_RightAxis_Y,

		/// <summary>切换左手武器/右手武器</summary>
		JS_Direction_X,
		/// <summary>切换魔法/道具</summary>
		JS_Direction_Y,

		JS_Button0,
		JS_Button1,
		JS_Button2,
		JS_Button3,

		/// <summary>左手主要动作</summary>
		JS_LB,
		/// <summary>左手次要动作</summary>
		JS_RB,
		/// <summary>右手主要动作</summary>
		JS_LT,
		/// <summary>右手次要动作</summary>
		JS_RT,

		/// <summary>静步</summary>
		JS_L3,
		/// <summary>目标锁定/视角重置</summary>
		JS_R3,

		/// <summary>打开菜单</summary>
		JS_Start,
		/// <summary>打开次要菜单</summary>
		JS_Select
	}

	#endregion

	
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
	

	#region ［玩家的动画状态机］

	/// <summary>玩家的FSM层级</summary>
	public enum PlayerFSMLayer {
		BaseLayer,
		AttackLayer,
		DefenseLayer
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
		__1Hand_Slash1,
		__1Hand_Slash2,
		__1Hand_Slash3,
		
		__1Hand_ShieldUp
	}


	/// <summary>玩家的FSM参数</summary>
	public enum PlayerFSMParam {
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
	public enum PlayerFSMCurve {
		spd_Jab_Y,
		spd_Jab_Z,
		spd_Roll_Y,
		spd_Jump_Y,

		spd_Atk_1H_Slash1_Z
	}


	/// <summary>玩家的FSM属性控制参数</summary>
	/// <remarks>一般用于控制动画的属性</remarks>
	public enum PlayerFSMProp { }

	#endregion
}