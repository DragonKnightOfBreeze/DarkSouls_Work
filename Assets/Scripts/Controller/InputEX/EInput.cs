namespace DSWork.InputEX {
	/// <summary>输入枚举</summary>
	/// <remarks>键鼠输入：以`K_`开头 手柄输入：以`J_`开头 坐标：以`Axis`结尾</remarks>
	public enum EInput {
		/*键盘输入*/

		/// <summary>向前移动</summary>
		K_Forward,
		/// <summary>向后移动</summary>
		K_Back,
		/// <summary>向左移动</summary>
		K_Left,
		/// <summary>向右移动</summary>
		K_Right,
		/// <summary>视角向上移动</summary>
		K_VUpAxis,
		/// <summary>视角向右移动</summary>
		K_VRightAxis,

		/// <summary>切换魔法</summary>
		K_ToggleMagic,
		/// <summary>切换道具</summary>
		K_ToggleItem,
		/// <summary>切换左手武器</summary>
		K_ToggleLHand,
		/// <summary>切换右手武器</summary>
		K_ToggleRHand,
		/// <summary>奔跑</summary>
		K_Run,
		/// <summary>闪避</summary>
		K_Dodge,
		/// <summary>跳跃</summary>
		K_Jump,
		/// <summary>使用物品</summary>
		K_UseItem,
		/// <summary>互动</summary>
		K_Interact,
		/// <summary>切换武器持有方式</summary>
		K_ToggleHold,
		/// <summary>左手主要动作</summary>
		K_LHandAct1,
		/// <summary>左手次要动作</summary>
		K_LHandAct2,
		/// <summary>右手主要动作</summary>
		K_RHandAct1,
		/// <summary>右手次要动作</summary>
		K_RHandAct2,
		/// <summary>静步</summary>
		K_Walk,
		/// <summary>锁定/重置视角</summary>
		K_Lock,
		/// <summary>打开菜单</summary>
		K_Menu,
		/// <summary>打开次要菜单</summary>
		K_SecMenu,

		/* 手柄输入 */

		/// <summary>左右移动</summary>
		J_RightAxis,
		/// <summary>前后移动</summary>
		J_ForwardAxis,
		/// <summary>视角左右移动</summary>
		J_VRightAxis,
		/// <summary>视角上下移动</summary>
		J_VUpAxis,

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
}