using System;

namespace DSWork.InputMgr {
	public class FullButton:IInput {
		private bool _press;
		private bool _pressDown;
		private bool _pressUp;
		public bool Press => _press;
		public bool PressDown => _pressDown;
		public bool PressUp => _pressUp;

		public bool FixedPress(float delay = 0, float extend = 0) {
			throw new NotImplementedException();
		}

		public bool FixedPressDown(float delay = 0) {
			throw new NotImplementedException();
		}

		public bool FixedPressUp(float extend = 0) {
			throw new NotImplementedException();
		}

		public bool Click(float interval = 0.2f) {
			throw new NotImplementedException();
		}

		public bool DoubleClick(float interval = 0.2f) {
			throw new NotImplementedException();
		}

		
		public void Tick() {
			throw new NotImplementedException();
		}
		
		
		public float AxisValue() {
			throw new NotImplementedException();
		}
	}
}