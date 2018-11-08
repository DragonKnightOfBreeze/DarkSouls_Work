namespace DSWork.InputMgr {
	public class SimpleButton: IInput {
		private bool _press;
		private bool _pressDown;
		private bool _pressUp;
		
		public bool Press => _press;
		public bool PressDown => _pressDown;
		public bool PressUp => _pressUp;
		

		public void Tick() {
			throw new System.NotImplementedException();
		}
		
		
		public bool FixedPress(float delay = 0, float extend = 0) {
			throw new System.NotImplementedException();
		}

		public bool FixedPressDown(float delay = 0) {
			throw new System.NotImplementedException();
		}

		public bool FixedPressUp(float extend = 0) {
			throw new System.NotImplementedException();
		}

		public bool Click(float interval = 0.2f) {
			throw new System.NotImplementedException();
		}

		public bool DoubleClick(float interval = 0.2f) {
			throw new System.NotImplementedException();
		}

		public float AxisValue() {
			throw new System.NotImplementedException();
		}
	}
}