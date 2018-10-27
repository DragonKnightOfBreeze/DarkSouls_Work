//TODO：需要自己用手柄调试，看看输入是否正确

namespace DSWork.Global {
	public class GameEnum { }

	#region ［输入］

	/// <summary>键鼠输入</summary>
	/// <remarks>需要在InputManager中妥善配置</remarks>
	public enum KMInput {
		/// <summary>向前移动</summary>
		KW_Forward,
		/// <summary>向后移动</summary>
		KW_Back,
		/// <summary>向左移动</summary>
		KW_Left,
		/// <summary>向右移动</summary>
		KW_Right,
		/// <summary>视角向上移动</summary>
		KW_VUp,
		/// <summary>视角向下移动</summary>
		KW_VDown,
		/// <summary>视角向左移动</summary>
		KW_VLeft,
		/// <summary>视角向下移动</summary>
		KW_VRight,

		/// <summary>切换魔法</summary>
		KW_ToggleMagic,
		/// <summary>切换道具</summary>
		KW_ToggleItem,
		/// <summary>切换左手武器</summary>
		KW_ToggleLHand,
		/// <summary>切换右手武器</summary>
		KW_ToggleRHand,


		/// <summary>奔跑</summary>
		KW_Run,
		/// <summary>闪避（后跃/翻滚/跳跃）</summary>
		KW_Dodge,
		/// <summary>使用物品</summary>
		KW_UseItem,
		/// <summary>互动</summary>
		KW_Interact,

		/// <summary>左手主要动作</summary>
		KW_LHandAct1,
		/// <summary>左手次要动作</summary>
		KW_LHandAct2,
		/// <summary>右手主要动作</summary>
		KW_RHandAct1,
		/// <summary>右手次要动作</summary>
		KW_RHandAct2,

		/// <summary>静步</summary>
		KW_Walk,
		/// <summary>目标锁定/视角重置</summary>
		KW_Lock,

		/// <summary>打开菜单</summary>
		KW_Menu,
		/// <summary>打开次要菜单</summary>
		KW_SecMenu
	}


	/// <summary>手柄输入</summary>
	/// <remarks>需要在InputManager中妥善配置</remarks>
	public enum JSInput {
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
		jab_roll_jump,
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