using System;

namespace PS4Mono
{
    internal abstract class InputDevice
    {
        #region Fields

        protected int _index;
        private bool _isDisconnected;

        #endregion

        #region Delegates

        public event EventHandler Disconnected;

        #endregion

        #region Properties

        public int Index
        {
            get { return _index; }
        }

        public abstract string DisplayName { get; }

        public abstract InputDeviceType DeviceType { get; }

        public bool IsDisconnected
        {
            get { return _isDisconnected; }
            protected set
            {
                if (value != _isDisconnected)
                {
                    _isDisconnected = value;
                    OnDisconnectedChanged();
                }
            }
        }

        #endregion

        #region Initialize

        internal InputDevice(int controllerIndex)
        {
            _index = controllerIndex;
        }

        #endregion

        #region Methods

        protected virtual void OnDisconnectedChanged()
        {
            if (_isDisconnected)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public sealed override string ToString()
        {
            return DisplayName;
        }

        #endregion
    }
}
