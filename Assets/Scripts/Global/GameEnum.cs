namespace DSWork.Global {
	public class GameEnum {
		
	}

	/// <summary>
	/// 玩家的FSM参数
	/// </summary>
	public enum PlayerFSMParam {
		forward,
		jab_roll_jump,
		isOnGround,
		fallRoll,
		attack
	}

	/// <summary>
	/// 玩家的FSM参数（曲线，用于设置位移速度）
	/// </summary>
	public enum PlayerFSMCurve {
		jabSpeed_Y,
		rollSpeed_Y,
		jumpSpeed_Y
	}

	/// <summary>
	/// 玩家的动画状态
	/// </summary>
	public enum PlayerState {
		Idle,
		Walk,
		Run,
		Dodge,
		Roll,
		Jump,
		Fall,
		
		Atk_1H_Slash1
	}
	
}