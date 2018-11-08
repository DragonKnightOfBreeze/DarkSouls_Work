using System.Collections.Generic;
using UnityEngine;

namespace DSWork.VInput {
    /// <summary>
    /// 键盘输入
    /// </summary>
    public class KeyboardInput : BaseInput {
        public Dictionary<InputKey, KeyCode> mKeys = new Dictionary<InputKey, KeyCode>() {
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

        public KeyboardInput()
            : base() { }

        public override float Horizontal {
            get { return UnityEngine.Input.GetAxis("Horizontal"); }
        }

        public override float Vertical {
            get { return UnityEngine.Input.GetAxis("Vertical"); }
        }

        public override bool IsKeyDown(InputKey rInputKey) {
            KeyCode rKeyCode = KeyCode.None;
            if(mKeys.TryGetValue(rInputKey, out rKeyCode)) {
                return UnityEngine.Input.GetKeyDown(rKeyCode);
            }
            return false;
        }

        public override bool IsKeyUp(InputKey rInputKey) {
            KeyCode rKeyCode = KeyCode.None;
            if(mKeys.TryGetValue(rInputKey, out rKeyCode)) {
                return UnityEngine.Input.GetKeyUp(rKeyCode);
            }
            return false;
        }

        public override bool IsKey(InputKey rInputKey) {
            KeyCode rKeyCode = KeyCode.None;
            if(mKeys.TryGetValue(rInputKey, out rKeyCode)) {
                return UnityEngine.Input.GetKey(rKeyCode);
            }
            return false;
        }
    }
}
