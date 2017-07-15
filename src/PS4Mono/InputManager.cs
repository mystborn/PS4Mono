using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PS4Mono
{
    public static class InputManager
    {
        #region Events

        private static event EventHandler<int> ControllerConnected;

        #endregion

        #region Fields

        private static List<Playstation4Input> controllers = new List<Playstation4Input>();
        private static List<int> openSlots = new List<int>();
        private static float deadZone = .15f;

        #endregion

        #region Properties

        internal static List<Playstation4Input> Controllers
        {
            get { return controllers; }
        }

        internal static List<int> OpenSlots
        {
            get { return openSlots; }
        }

        /// <summary>
        /// Returns the amount of currently connected devices
        /// </summary>
        public static int DeviceCount
        {
            get { return controllers.Count - openSlots.Count; }
        }

        /// <summary>
        /// Get or set the joystick deadzone.
        /// </summary>
        public static float GamepadAxisDeadZone
        {
            get { return deadZone; }
            set { deadZone = MathHelper.Clamp(value, 0, 1); }
        }

        #endregion

        #region Controller

        /// <summary>
        /// Determine if the specified controller is plugged in.
        /// </summary>
        /// <param name="index">The index to check</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool GamepadIsConnected(int index)
        {
            if (index >= controllers.Count || index < 0)
                return false;
            return controllers[index] != null;
        }

        /// <summary>
        /// Get the raw axis value of the specified controller.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="axis">Axis to check.</param>
        /// <returns></returns>
        public static float GamepadRawAxis(int index, Axis axis)
        {
            if (!GamepadIsConnected(index))
                return 0;

            switch(axis)
            {
                case Axis.LeftX:
                    return controllers[index].LX;
                case Axis.LeftY:
                    return controllers[index].LY;
                case Axis.RightX:
                    return controllers[index].RX;
                case Axis.RightY:
                    return controllers[index].RY;
                default:
                    throw new ArgumentException("Invalid axis input", "axis");
            }
        }

        /// <summary>
        /// Gets the raw trigger value of the specified controller.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="trigger">Trigger to check.</param>
        /// <returns></returns>
        public static byte GamepadRawTrigger(int index, Buttons trigger)
        {
            if (!GamepadIsConnected(index))
                return 0;

            switch(trigger)
            {
                case Buttons.LeftTrigger:
                    return controllers[index].LeftTrigger;
                case Buttons.RightTrigger:
                    return controllers[index].RightTrigger;
                default:
                    throw new ArgumentException("Not a valid axis button.", "trigger");
            }
        }

        /// <summary>
        /// Check if the specified button is being pressed.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="button">The <see cref="Buttons"/> to check for.</param>
        public static bool GamepadCheck(int index, Buttons button)
        {
            if (!GamepadIsConnected(index))
                return false;

            var b = GamepadButtonToControllerButton(button);

            if (controllers[index].CurrentFrameState.HasFlag(b))
                return true;

            return false;
        }

        /// <summary>
        /// Check if the specified button is being pressed at the time of the function call.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="button">The <see cref="Buttons"/> to check for.</param>
        public static bool GamepadCheckAsnyc(int index, Buttons button)
        {
            if (!GamepadIsConnected(index))
                return false;

            var b = GamepadButtonToControllerButton(button);

            return controllers[index].AsyncState.HasFlag(b);
        }

        /// <summary>
        /// Check if the specified button has just been pressed.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="button">The <see cref="Buttons"/> to check for.</param>
        public static bool GamepadCheckPressed(int index, Buttons button)
        {
            if (!GamepadIsConnected(index))
                return false;

            var b = GamepadButtonToControllerButton(button);

            if (controllers[index].CurrentFrameState.HasFlag(b) && !controllers[index].CurrentFrameState.HasFlag(b))
                return true;

            return false;
        }

        /// <summary>
        /// Check if the specified button has just been released.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="button">The <see cref="Buttons"/> to check for.</param>
        public static bool GamepadCheckReleased(int index, Buttons button)
        {
            if (!GamepadIsConnected(index))
                return false;

            var b = GamepadButtonToControllerButton(button);

            if (!controllers[index].CurrentFrameState.HasFlag(b) && controllers[index].CurrentFrameState.HasFlag(b))
                return true;

            return false;
        }

        /// <summary>
        /// Register an event that gets triggered when the specified controller disconnects.
        /// </summary>
        /// <param name="index">Controller index.</param>
        /// <param name="disconnectHandler">Event triggered on controller disconnect.</param>
        /// <returns></returns>
        public static bool RegisterControllerDisconnectHandler(int index, EventHandler disconnectHandler)
        {
            if (GamepadIsConnected(index))
                return false;
            controllers[index].Disconnected += disconnectHandler;
            return true;
        }

        /// <summary>
        /// Register an event that gets triggered when a controller is noticed.
        /// </summary>
        /// <param name="connectHandler">Event triggered on controller connect (int value is controller index).</param>
        public static void RegisterControllerConnectHandler(EventHandler<int> connectHandler)
        {
            ControllerConnected += connectHandler;
        }

        internal static void OnControllerConnected(int index)
        {
            ControllerConnected?.Invoke(null, index);
        }

        /// <summary>
        /// Converts a monogame/xna button to the button type used internally.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private static ControllerButtons GamepadButtonToControllerButton(Buttons button)
        {
            switch(button)
            {
                case Buttons.A:
                    return ControllerButtons.Face1;
                case Buttons.B:
                    return ControllerButtons.Face4;
                case Buttons.Back:
                    return ControllerButtons.Select;
                case Buttons.BigButton:
                    return ControllerButtons.Symbol;
                case Buttons.DPadDown:
                    return ControllerButtons.DpadDown;
                case Buttons.DPadLeft:
                    return ControllerButtons.DpadLeft;
                case Buttons.DPadRight:
                    return ControllerButtons.DpapdRight;
                case Buttons.DPadUp:
                    return ControllerButtons.DpadUp;
                case Buttons.LeftShoulder:
                    return ControllerButtons.LeftButton;
                case Buttons.LeftStick:
                    return ControllerButtons.LeftStick;
                case Buttons.LeftThumbstickDown:
                    return ControllerButtons.LeftStickDown;
                case Buttons.LeftThumbstickLeft:
                    return ControllerButtons.LeftStickLeft;
                case Buttons.LeftThumbstickRight:
                    return ControllerButtons.LeftStickRight;
                case Buttons.LeftThumbstickUp:
                    return ControllerButtons.LeftStickUp;
                case Buttons.LeftTrigger:
                    return ControllerButtons.LeftTrigger;
                case Buttons.RightShoulder:
                    return ControllerButtons.RightButton;
                case Buttons.RightStick:
                    return ControllerButtons.RightStick;
                case Buttons.RightThumbstickDown:
                    return ControllerButtons.RightStickDown;
                case Buttons.RightThumbstickLeft:
                    return ControllerButtons.RightStickLeft;
                case Buttons.RightThumbstickRight:
                    return ControllerButtons.RightStickRight;
                case Buttons.RightThumbstickUp:
                    return ControllerButtons.RightStickUp;
                case Buttons.RightTrigger:
                    return ControllerButtons.RightTrigger;
                case Buttons.Start:
                    return ControllerButtons.Start;
                case Buttons.X:
                    return ControllerButtons.Face2;
                case Buttons.Y:
                    return ControllerButtons.Face3;
                default:
                    return ControllerButtons.None;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes connected controllers.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="pollInterval">How often it checks for incoming devices in milliseconds.</param>
        public static void Initialize(Game game, int pollInterval = 2000)
        {
            RawInputDeviceManager.Initialize(game.Window.Handle, pollInterval);
        }

        /// <summary>
        /// Updates connected controllers
        /// </summary>
        public static void Update()
        {
            foreach (var c in controllers)
                c.Reset();
        }

        #endregion
    }
}
