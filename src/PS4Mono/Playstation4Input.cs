using System;
using System.IO;
using Microsoft.Win32.SafeHandles;
using static Microsoft.Xna.Framework.MathHelper;

namespace PS4Mono
{
    internal class Playstation4Input : HidController
    {
        #region Constants

        private const float NORMAl = .00787401574f;

        #endregion

        #region Properties

        public override InputDeviceType DeviceType
        {
            get { return InputDeviceType.HID; }
        }

        public override string DisplayName
        {
            get { return product; }
        }

        #endregion

        #region Initialize

        public Playstation4Input(SafeFileHandle read, HidPCaps info) : base(-1)
        {
            handleRead = read;
            caps = info;

            fsRead = new FileStream(read, FileAccess.ReadWrite, caps.InputReportByteLength, false);

            current = ControllerButtons.None;
            previous = ControllerButtons.None;
            asyncState = ControllerButtons.None;

            Disconnected += (s, e) =>
            {
                if (InputManager.Controllers.Contains(this))
                    InputManager.Controllers.Remove(this);
            };
            ReadAsync();
        }

        public void Close()
        {
            if (fsRead != null)
                fsRead.Close();
            if (handleRead != null && !handleRead.IsInvalid)
                handleRead.Close();

            IsDisconnected = true;
        }

        #endregion

        #region Receive Data

        internal override void Reset()
        {
            previous = current;
            current = asyncState;
            asyncState = ControllerButtons.None;
        }

        private void OnDataReceived(byte[] message)
        {
            lx = Clamp((message[1] - 128) * NORMAl, -1, 1);
            ly = Clamp((message[2] - 128) * NORMAl, -1, 1);
            rx = Clamp((message[3] - 128) * NORMAl, -1, 1);
            ry = Clamp((message[4] - 128) * NORMAl, -1, 1);

            leftTriggerValue = message[8];
            rightTriggerValue = message[9];

            byte dpad = (byte)(message[5] & 15);
            asyncState = ControllerButtons.None;
            asyncState = asyncState | ((dpad == 0 || dpad == 7 || dpad == 1) ? ControllerButtons.DpadUp : ControllerButtons.None);
            asyncState = asyncState | ((dpad == 7 || dpad == 6 || dpad == 5) ? ControllerButtons.DpadLeft : ControllerButtons.None);
            asyncState = asyncState | ((dpad == 5 || dpad == 4 || dpad == 3) ? ControllerButtons.DpadDown : ControllerButtons.None);
            asyncState = asyncState | ((dpad == 3 || dpad == 2 || dpad == 1) ? ControllerButtons.DpapdRight : ControllerButtons.None);

            asyncState = asyncState | (((message[5] & 128) == 128) ? ControllerButtons.Face3 : ControllerButtons.None);
            asyncState = asyncState | (((message[5] & 64) == 64) ? ControllerButtons.Face4 : ControllerButtons.None);
            asyncState = asyncState | (((message[5] & 32) == 32) ? ControllerButtons.Face1 : ControllerButtons.None);
            asyncState = asyncState | (((message[5] & 16) == 16) ? ControllerButtons.Face2 : ControllerButtons.None);

            asyncState = asyncState | (((message[6] & 1) == 1) ? ControllerButtons.LeftButton : ControllerButtons.None);
            asyncState = asyncState | (((message[6] & 2) == 2) ? ControllerButtons.RightButton : ControllerButtons.None);
            asyncState = asyncState | (((message[6] & 4) == 4) ? ControllerButtons.LeftTrigger : ControllerButtons.None);
            asyncState = asyncState | (((message[6] & 8) == 8) ? ControllerButtons.RightTrigger : ControllerButtons.None);

            asyncState = asyncState | (((message[6] & 16) == 16) ? ControllerButtons.Select : ControllerButtons.None);
            asyncState = asyncState | (((message[6] & 32) == 32) ? ControllerButtons.Start : ControllerButtons.None);
            asyncState = asyncState | (((message[6] & 64) == 64) ? ControllerButtons.LeftStick : ControllerButtons.None);
            asyncState = asyncState | (((message[6] & 128) == 128) ? ControllerButtons.RightStick : ControllerButtons.None);

            asyncState = asyncState | (ly <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickDown : ControllerButtons.None);
            asyncState = asyncState | (lx <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickLeft : ControllerButtons.None);
            asyncState = asyncState | (ly >= InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickRight : ControllerButtons.None);
            asyncState = asyncState | (lx >= InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickUp : ControllerButtons.None);

            asyncState = asyncState | (ry <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickDown : ControllerButtons.None);
            asyncState = asyncState | (rx <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickLeft : ControllerButtons.None);
            asyncState = asyncState | (ry >= InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickRight : ControllerButtons.None);
            asyncState = asyncState | (rx >= InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickUp : ControllerButtons.None);

            asyncState = asyncState | (((message[7] & 1) == 1) ? ControllerButtons.Symbol : ControllerButtons.None);
            asyncState = asyncState | (((message[7] & 2) == 2) ? ControllerButtons.Trackpad : ControllerButtons.None);

            current = current | asyncState;
        }

        private void ReadAsync()
        {
            readData = new byte[caps.InputReportByteLength];
            if (fsRead.CanRead)
                fsRead.BeginRead(readData, 0, readData.Length, new AsyncCallback(GetInputReportData), readData);
            else
                throw new IOException("PS4 controller cannot read.");
        }

        private void GetInputReportData(IAsyncResult ar)
        {
            try
            {
                fsRead.EndRead(ar);
            }
            catch (IOException)
            {
                Close();
            }

            if (fsRead.CanRead)
                fsRead.BeginRead(readData, 0, readData.Length, new AsyncCallback(GetInputReportData), readData);
            else if (!IsDisconnected)
                throw new IOException("PS4 Controller cannot read");

            OnDataReceived(readData);

        }

        #endregion
    }
}
