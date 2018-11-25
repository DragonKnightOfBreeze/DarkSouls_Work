namespace DSWork.VInput {
	public class JoystickInput : BaseInput {
		public override float Horizontal => Joystick.Instance.x;

		public override float Vertical => Joystick.Instance.y;

		public override bool IsKeyDown(InputKey rInputKey) {
			throw new System.NotImplementedException();
		}

		public override bool IsKeyUp(InputKey rInputKey) {
			throw new System.NotImplementedException();
		}

		public override bool IsKey(InputKey rInputKey) {
			throw new System.NotImplementedException();
		}
	}
}