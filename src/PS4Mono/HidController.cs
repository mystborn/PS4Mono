using System.IO;
using Microsoft.Win32.SafeHandles;

namespace PS4Mono
{
    internal abstract class HidController : InputDevice
    {
        #region Fields

        protected SafeFileHandle handleRead;
        protected FileStream fsRead;
        protected HidPCaps caps;
        protected byte[] readData;
        protected string product;
        protected ControllerButtons current;
        protected ControllerButtons previous;
        protected ControllerButtons asyncState;
        protected float lx;
        protected float ly;
        protected float rx;
        protected float ry;
        protected byte leftTriggerValue;
        protected byte rightTriggerValue;

        #endregion

        #region Properties

        internal virtual ControllerButtons CurrentFrameState
        {
            get { return current; }
        }

        internal virtual ControllerButtons PreviousFrameState
        {
            get { return previous; }
        }

        internal virtual ControllerButtons State
        {
            get { return asyncState; }
        }

        #endregion

        #region Initialize

        public HidController(int index) : base(index) { }

        internal abstract void Reset();

        #endregion
    }
}
