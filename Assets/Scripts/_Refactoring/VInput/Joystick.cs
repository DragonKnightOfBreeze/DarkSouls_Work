using DSWork.Utility;

namespace DSWork.VInput {
	public class Joystick : TSingleton<Joystick> {
		public float x;
		public float y;

		private Joystick() { }

		public void Reset() {
			x = 0.0f;
			y = 0.0f;
		}

		public void Set(float x, float y) {
			this.x = x;
			this.y = y;
		}
	}
}