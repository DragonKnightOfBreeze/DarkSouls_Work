using System;
using DSWork.Utility;

namespace DSWork.VInput {
	[Flags]
	public enum InputKey {
		Jump = 1,
		Attack = 2,
		Run = 4,
		Skill1 = 8,
		Skill2 = 16,
		Skill3 = 32,
		Skill4 = 64,
		Skill5 = 128,
		Skill6 = 256
	}

	public abstract class BaseInput {
		/// <summary>输入的创建</summary>
		public static T Create<T>() where T : BaseInput {
			var rType = typeof(T);
			var rInput = ReflectionTool.Construct(rType) as T;
			return rInput;
		}

		/// <summary>前后</summary>
		public abstract float Horizontal { get; }
		/// <summary>左右</summary>
		public abstract float Vertical { get; }

		/// <summary>是否有按键按下</summary>
		public abstract bool IsKeyDown(InputKey rInputKey);

		/// <summary>是否有按键弹起</summary>
		public abstract bool IsKeyUp(InputKey rInputKey);

		/// <summary>是否有按键的状态</summary>
		public abstract bool IsKey(InputKey rInputKey);
	}
}