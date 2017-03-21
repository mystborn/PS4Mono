using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PS4Mono
{
    public static class InputManager
    {
        #region Fields

        private static List<HidController> controllers = new List<HidController>();
        private static float deadZone = .15f;

        #endregion

        #region Properties

        internal static List<HidController> Controllers
        {
            get { return controllers; }
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
        /// Check if the specified button is being pressed.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="button">The <see cref="Buttons"/> to check for.</param>
        public static bool GamepadCheck(int index, Buttons button)
        {
            if (index >= controllers.Count)
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
            if (index >= controllers.Count)
                return false;

            var b = GamepadButtonToControllerButton(button);

            return controllers[index].State.HasFlag(b);
        }

        /// <summary>
        /// Check if the specified button has just been pressed.
        /// </summary>
        /// <param name="index">Controller index to check.</param>
        /// <param name="button">The <see cref="Buttons"/> to check for.</param>
        public static bool GamepadCheckPressed(int index, Buttons button)
        {
            if (index >= controllers.Count)
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
            if (index >= controllers.Count)
                return false;

            var b = GamepadButtonToControllerButton(button);

            if (!controllers[index].CurrentFrameState.HasFlag(b) && controllers[index].CurrentFrameState.HasFlag(b))
                return true;

            return false;
        }

        private static ControllerButtons GamepadButtonToControllerButton(Buttons button)
        {
            if (button == Buttons.A)
                return ControllerButtons.Face1;
            if (button == Buttons.B)
                return ControllerButtons.Face4;
            if (button == Buttons.Back)
                return ControllerButtons.Select;
            if (button == Buttons.BigButton)
                return ControllerButtons.Symbol;
            if (button == Buttons.DPadDown)
                return ControllerButtons.DpadDown;
            if (button == Buttons.DPadLeft)
                return ControllerButtons.DpadLeft;
            if (button == Buttons.DPadRight)
                return ControllerButtons.DpapdRight;
            if (button == Buttons.DPadUp)
                return ControllerButtons.DpadUp;
            if (button == Buttons.LeftShoulder)
                return ControllerButtons.LeftButton;
            if (button == Buttons.LeftStick)
                return ControllerButtons.LeftStick;
            if (button == Buttons.LeftThumbstickDown)
                return ControllerButtons.LeftStickDown;
            if (button == Buttons.LeftThumbstickLeft)
                return ControllerButtons.LeftStickLeft;
            if (button == Buttons.LeftThumbstickRight)
                return ControllerButtons.LeftStickRight;
            if (button == Buttons.LeftThumbstickUp)
                return ControllerButtons.LeftStickUp;
            if (button == Buttons.LeftTrigger)
                return ControllerButtons.LeftTrigger;
            if (button == Buttons.RightShoulder)
                return ControllerButtons.RightButton;
            if (button == Buttons.RightStick)
                return ControllerButtons.RightStick;
            if (button == Buttons.RightThumbstickDown)
                return ControllerButtons.RightStickDown;
            if (button == Buttons.RightThumbstickLeft)
                return ControllerButtons.RightStickLeft;
            if (button == Buttons.RightThumbstickRight)
                return ControllerButtons.RightStickRight;
            if (button == Buttons.RightThumbstickUp)
                return ControllerButtons.RightStickUp;
            if (button == Buttons.RightTrigger)
                return ControllerButtons.RightTrigger;
            if (button == Buttons.Start)
                return ControllerButtons.Start;
            if (button == Buttons.X)
                return ControllerButtons.Face2;
            if (button == Buttons.Y)
                return ControllerButtons.Face3;
            return ControllerButtons.None;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes connected controllers.
        /// </summary>
        /// <param name="game"></param>
        public static void Initialize(Game game)
        {
            RawInputDeviceManager.Initialize(game.Window.Handle);
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
