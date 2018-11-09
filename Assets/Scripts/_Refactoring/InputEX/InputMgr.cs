////TODO：测试并通过
////TODO：添加注释
////TODO：加强异常处理
//
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using DSWork.Utility;
//using UnityEngine;
//
//namespace DSWork.InputEX {
//	/// <summary>
//	/// 输入管理器
//	/// </summary>
//	public class InputMgr : MonoBehaviour {
//		/// <summary>脚本的单例</summary>
//		public static InputMgr Instance { get; private set; }
//
////		/// <summary>延长计时器</summary>
////		private readonly Timer delayTimer = new Timer();
////		/// <summary>延时计时器</summary>
////		private readonly Timer extendTimer = new Timer();
//
//
//		private Dictionary<InputEnum,Timer> delayTimerDict = new Dictionary<InputEnum, Timer>();
//		private Dictionary<InputEnum,Timer> extendTimerDict = new Dictionary<InputEnum, Timer>();
//		
//		private bool isDelayCrEnd;
//		private static bool isExtendCrEnd;
//		
//		private void Awake() {
//			if(Instance == null)
//				Instance = this;
//		}
//
//		private void Update() {
//			if(PressDown(InputEnum.Forward)) 
//				print("PressDown Forward");
//			if(PressUp(InputEnum.Forward)) 
//				print("PressUp Forward");
//			if(Press(InputEnum.Forward))
//				print("Press Forward");
//			if(FixedPressDown(InputEnum.Forward,2f)) 
//				print("FixedPressDown Forward Delay 2s");
//			if(FixedPressUp(InputEnum.Forward,2f))
//				print("FixedPressUp Forward Delay 2s");
//			if(FixedPress(InputEnum.Forward,2f,1f))
//				print("FixedPress Forward Delay 2s Extend 1s");
//
//			foreach(var timer in delayTimerList) {
//				timer.Tick();
//			}
//			foreach(var timer in extendTimerList) {
//				timer.Tick();
////			}
//			
////			delayTimer.Tick();
////			extendTimer.Tick();
//		}
//
//
//		public static bool PressDown(InputEnum input) {
//			return Input.GetButtonDown(input.TS());
//		}
//
//		public static bool PressUp(InputEnum input) {
//			return Input.GetButtonUp(input.TS());
//		}
//
//		public static bool Press(InputEnum input) {
//			return Input.GetButton(input.TS());
//		}
//
//
//		public static bool FixedPressDown(InputEnum input, float delay = 0f, float extend = 0.002f) {
//			if(delay <= 0.02f && extend <= 0.02f)
//				return PressDown(input);
//			else {
//				
//			}
//			if(PressDown(input))
//				delayTimer.StartTimer(delay, () => extendTimer.StartTimer(extend));
//			return extendTimer.isRunning;
//				
//		}
//
//		public static bool FixedPressUp(InputEnum input, float delay = 0f, float extend = 0.002f) {
//			if(PressUp(input))
//				delayTimer.StartTimer(delay, () => extendTimer.StartTimer(extend));
//			return extendTimer.isRunning;
//		}
//
//		public static bool FixedPress(InputEnum input, float delay = 0f, float extend = 0.002f) {
//			if(PressDown(input))
//				delayTimer.StartTimer(delay);
//			else if(PressUp(input))
//				extendTimer.StartTimer(extend);
//			return !delayTimer.isRunning && Press(input) || extendTimer.isRunning;
//		}
//
//		public static bool Click(InputEnum input, float duration = 0.2f) {
//			return PressUp(input) && FixedPressDown(input, 0, duration);
//		}
//
//		public static bool DoubleClick(InputEnum input, float interval = 1f) {
//			return PressDown(input) && FixedPressUp(input, 0, interval);
//		}
//
//
//		public static float AxisValue(InputEnum axis) {
//			try {
//				return Input.GetAxis(axis.TS());
//			}
//			catch (Exception) {
//				return 0;
//			}
//		}
//
//		public static float AxisValue(InputEnum button1, InputEnum button2) {
//			try {
//				return Press(button1).To1_0() - Press(button2).To1_0();
//			}
//			catch (Exception) {
//				return 0;
//			}
//		}
//
////		IEnumerator TimerCr() {
////			
////		}
//		
//	}
//
//
//}