using System;
using System.IO;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using static Microsoft.Xna.Framework.MathHelper;

namespace PS4Mono
{
    internal class Playstation4Input
    {
        #region Constants

        /// <summary>
        /// Used to normalize an axis value.
        /// </summary>
        private const float NORMAl = .00787401574f;

        #endregion

        #region Events

        /// <summary>
        /// Event thrown when a controller is disconnected.
        /// </summary>
        public event EventHandler Disconnected;

        #endregion

        #region Fields

        private bool _isConnected;
        private byte _leftTriggerValue;
        private byte _rightTriggerValue;
        private int _index;
        private float _lx;
        private float _ly;
        private float _rx;
        private float _ry;
        private byte[] _readData;
        private ControllerButtons _current;
        private ControllerButtons _previous;
        private ControllerButtons _asyncState;
        private HidPCaps _caps;
        private FileStream _fsRead;
        private SafeFileHandle _handleRead;

        #endregion

        #region Properties

        /// <summary>
        /// Index of the controller as far as this library is concerned.
        /// </summary>
        public int Index
        {
            get => _index;
        }

        /// <summary>
        /// Gets whether or not this controller is connected. Most likely always true.
        /// </summary>
        public bool IsConnected
        {
            get => _isConnected;
        }

        /// <summary>
        /// Gets the current state of the pressed buttons during this frame.
        /// </summary>
        internal ControllerButtons CurrentFrameState
        {
            get => _current;
        }

        /// <summary>
        /// Gets the state of the pressed buttons during the previous frame.
        /// </summary>
        internal ControllerButtons PreviousFrameState
        {
            get => _previous;
        }

        /// <summary>
        /// Gets the state of the pressed buttons as of the last controller update.
        /// </summary>
        internal ControllerButtons AsyncState
        {
            get => _asyncState;
        }

        /// <summary>
        /// Left stick X axis value.
        /// </summary>
        internal float LX
        {
            get => _lx;
        }

        /// <summary>
        /// Left stick Y axis value.
        /// </summary>
        internal float LY
        {
            get => _ly;
        }

        /// <summary>
        /// Right stick X axis value.
        /// </summary>
        internal float RX
        {
            get => _rx;
        }

        /// <summary>
        /// Right stick Y axis value.
        /// </summary>
        internal float RY
        {
            get => _ry;
        }

        /// <summary>
        /// Raw left trigger value.
        /// </summary>
        internal byte LeftTrigger
        {
            get => _leftTriggerValue;
        }

        /// <summary>
        /// Raw right trigger value.
        /// </summary>
        internal byte RightTrigger
        {
            get => _rightTriggerValue;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes a new PS4 controller.
        /// </summary>
        /// <param name="read">The "file" that the controller sends info to.</param>
        /// <param name="info">The capabilities of the controller.</param>
        internal Playstation4Input(SafeFileHandle read, HidPCaps info)
        {
            _handleRead = read;
            _caps = info;

            //Determine the controller index.
            if (InputManager.OpenSlots.Count == 0)
                _index = InputManager.DeviceCount;
            else
            {
                _index = InputManager.OpenSlots.Min();
                InputManager.OpenSlots.Remove(_index);
            }

            InputManager.Controllers.Insert(_index, this);
            InputManager.OnControllerConnected(_index);

            _fsRead = new FileStream(read, FileAccess.ReadWrite, _caps.InputReportByteLength, false);

            _current = ControllerButtons.None;
            _previous = ControllerButtons.None;
            _asyncState = ControllerButtons.None;

            Disconnected += (s, e) =>
            {
                if (InputManager.Controllers.Contains(this))
                {
                    InputManager.OpenSlots.Add(_index);
                    InputManager.Controllers[_index] = null;
                }
            };

            //Start reading the file.
            ReadAsync();
        }

        /// <summary>
        /// Dispose of all dynamic resources. Trigger Disconnect event.
        /// </summary>
        private void Close()
        {
            if (_isConnected)
            {
                if (_fsRead != null)
                    _fsRead.Close();
                if (_handleRead != null && !_handleRead.IsInvalid)
                    _handleRead.Close();

                Disconnected?.Invoke(this, EventArgs.Empty);
                _isConnected = false;
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"PS4 Controller: {_index}";
        }

        #endregion

        #region Internal/Private Methods

        internal void Reset()
        {
            _previous = _current;
            _current = _asyncState;
            _asyncState = ControllerButtons.None;
        }

        /// <summary>
        /// Set values based on info read from the input buffer.
        /// </summary>
        /// <param name="message">input buffer</param>
        /// <remarks>
        /// Information on what the buffer contains can be found here:
        /// http://www.psdevwiki.com/ps4/DS4-USB#Report_Structure
        /// </remarks>
        private void OnDataReceived(byte[] message)
        {
            _lx = Clamp((message[1] - 128) * NORMAl, -1, 1);
            _ly = Clamp((message[2] - 128) * NORMAl, -1, 1);
            _rx = Clamp((message[3] - 128) * NORMAl, -1, 1);
            _ry = Clamp((message[4] - 128) * NORMAl, -1, 1);

            _leftTriggerValue = message[8];
            _rightTriggerValue = message[9];

            byte dpad = (byte)(message[5] & 15);
            _asyncState = ControllerButtons.None;
            _asyncState = _asyncState | ((dpad == 0 || dpad == 7 || dpad == 1) ? ControllerButtons.DpadUp : ControllerButtons.None);
            _asyncState = _asyncState | ((dpad == 7 || dpad == 6 || dpad == 5) ? ControllerButtons.DpadLeft : ControllerButtons.None);
            _asyncState = _asyncState | ((dpad == 5 || dpad == 4 || dpad == 3) ? ControllerButtons.DpadDown : ControllerButtons.None);
            _asyncState = _asyncState | ((dpad == 3 || dpad == 2 || dpad == 1) ? ControllerButtons.DpapdRight : ControllerButtons.None);

            _asyncState = _asyncState | (((message[5] & 128) == 128) ? ControllerButtons.Face3 : ControllerButtons.None);
            _asyncState = _asyncState | (((message[5] & 64) == 64) ? ControllerButtons.Face4 : ControllerButtons.None);
            _asyncState = _asyncState | (((message[5] & 32) == 32) ? ControllerButtons.Face1 : ControllerButtons.None);
            _asyncState = _asyncState | (((message[5] & 16) == 16) ? ControllerButtons.Face2 : ControllerButtons.None);

            _asyncState = _asyncState | (((message[6] & 1) == 1) ? ControllerButtons.LeftButton : ControllerButtons.None);
            _asyncState = _asyncState | (((message[6] & 2) == 2) ? ControllerButtons.RightButton : ControllerButtons.None);
            _asyncState = _asyncState | (((message[6] & 4) == 4) ? ControllerButtons.LeftTrigger : ControllerButtons.None);
            _asyncState = _asyncState | (((message[6] & 8) == 8) ? ControllerButtons.RightTrigger : ControllerButtons.None);

            _asyncState = _asyncState | (((message[6] & 16) == 16) ? ControllerButtons.Select : ControllerButtons.None);
            _asyncState = _asyncState | (((message[6] & 32) == 32) ? ControllerButtons.Start : ControllerButtons.None);
            _asyncState = _asyncState | (((message[6] & 64) == 64) ? ControllerButtons.LeftStick : ControllerButtons.None);
            _asyncState = _asyncState | (((message[6] & 128) == 128) ? ControllerButtons.RightStick : ControllerButtons.None);

            _asyncState = _asyncState | (_ly <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickDown : ControllerButtons.None);
            _asyncState = _asyncState | (_lx <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickLeft : ControllerButtons.None);
            _asyncState = _asyncState | (_ly >= InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickRight : ControllerButtons.None);
            _asyncState = _asyncState | (_lx >= InputManager.GamepadAxisDeadZone ? ControllerButtons.LeftStickUp : ControllerButtons.None);

            _asyncState = _asyncState | (_ry <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickDown : ControllerButtons.None);
            _asyncState = _asyncState | (_rx <= -InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickLeft : ControllerButtons.None);
            _asyncState = _asyncState | (_ry >= InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickRight : ControllerButtons.None);
            _asyncState = _asyncState | (_rx >= InputManager.GamepadAxisDeadZone ? ControllerButtons.RightStickUp : ControllerButtons.None);

            _asyncState = _asyncState | (((message[7] & 1) == 1) ? ControllerButtons.Symbol : ControllerButtons.None);
            _asyncState = _asyncState | (((message[7] & 2) == 2) ? ControllerButtons.Trackpad : ControllerButtons.None);

            _current = _current | _asyncState;
        }

        private void ReadAsync()
        {
            _readData = new byte[_caps.InputReportByteLength];
            if (_fsRead.CanRead)
                _fsRead.BeginRead(_readData, 0, _readData.Length, new AsyncCallback(GetInputReportData), _readData);
            else
                throw new IOException("PS4 controller cannot read.");
        }

        private void GetInputReportData(IAsyncResult ar)
        {
            try
            {
                _fsRead.EndRead(ar);
            }
            catch (IOException)
            {
                //Controller has been disconnected.
                Close();
            }

            if (_fsRead.CanRead)
                _fsRead.BeginRead(_readData, 0, _readData.Length, new AsyncCallback(GetInputReportData), _readData);
            else if (IsConnected)
                throw new IOException("PS4 Controller cannot read");

            OnDataReceived(_readData);

        }

        #endregion
    }
}
