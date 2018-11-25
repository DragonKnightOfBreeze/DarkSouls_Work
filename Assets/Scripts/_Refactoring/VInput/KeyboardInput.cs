using System.Collections.Generic;
using UnityEngine;

namespace DSWork.VInput {
    /// <summary>键盘输入</summary>
    public class KeyboardInput : BaseInput {
		public Dictionary<InputKey, KeyCode> mKeys = new Dictionary<InputKey, KeyCode>{
			{InputKey.Jump, KeyCode.Space},
			{InputKey.Attack, KeyCode.C},
			{InputKey.Run, KeyCode.LeftShift},
			{InputKey.Skill1, KeyCode.Keypad1},
			{InputKey.Skill2, KeyCode.Keypad2},
			{InputKey.Skill3, KeyCode.Keypad3},
			{InputKey.Skill4, KeyCode.Keypad4},
			{InputKey.Skill5, KeyCode.R},
			{InputKey.Skill6, KeyCode.F}
		};

		public override float Horizontal => Input.GetAxis("Horizontal");

		public override float Vertical => Input.GetAxis("Vertical");

		public override bool IsKeyDown(InputKey rInputKey) {
			var rKeyCode = KeyCode.None;
			if(mKeys.TryGetValue(rInputKey, out rKeyCode))
				return Input.GetKeyDown(rKeyCode);
			return false;
		}

		public override bool IsKeyUp(InputKey rInputKey) {
			var rKeyCode = KeyCode.None;
			if(mKeys.TryGetValue(rInputKey, out rKeyCode))
				return Input.GetKeyUp(rKeyCode);
			return false;
		}

		public override bool IsKey(InputKey rInputKey) {
			var rKeyCode = KeyCode.None;
			if(mKeys.TryGetValue(rInputKey, out rKeyCode))
				return Input.GetKey(rKeyCode);
			return false;
		}
	}
}