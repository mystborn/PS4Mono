using System;
using System.Runtime.InteropServices;
using Point = Microsoft.Xna.Framework.Point;

namespace PS4Mono
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 32)]
    internal struct DeviceInfo
    {
        #region Nested Class

        [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 24)]
        private struct Info
        {
            [FieldOffset(0)]
            internal KeyboardDeviceInfo Keyboard;

            [FieldOffset(0)]
            internal MouseDeviceInfo Mouse;

            [FieldOffset(0)]
            internal HumanInterfaceDeviceInfo HID;

            internal static readonly Info Empty;
        }

        #endregion

        #region Fields

        private int structSize;
        private InputDeviceType type;
        private Info info;

        #endregion

        #region Initialize

        private DeviceInfo(int structureSize)
        {
            structSize = structureSize;
            type = InputDeviceType.Mouse;
            info = Info.Empty;
        }

        internal int StructureSize
        {
            get { return structSize; }
        }

        public InputDeviceType DeviceType
        {
            get { return type; }
        }

        public MouseDeviceInfo? MouseInfo
        {
            get
            {
                if (type == InputDeviceType.Mouse)
                    return info.Mouse;
                return null;
            }
        }

        public KeyboardDeviceInfo? KeyboardInfo
        {
            get
            {
                if (type == InputDeviceType.Keyboard)
                    return info.Keyboard;
                return null;
            }
        }

        public HumanInterfaceDeviceInfo? HidInfo
        {
            get
            {
                if (type == InputDeviceType.HID)
                    return info.HID;
                return null;
            }
        }

        #endregion

        public static readonly DeviceInfo Default = new DeviceInfo(Marshal.SizeOf(typeof(DeviceInfo)));
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    internal struct HumanInterfaceDeviceInfo
    {

        /// <summary>The vendor identifier for the HID.</summary>
        internal int VendorId;
        /// <summary>The product identifier for the HID.</summary>
        internal int ProductId;
        /// <summary>The version number for the HID.</summary>
        internal int VersionNumber;
        /// <summary>The top-level collection (TLC usage page and usage) for the device.</summary>
        internal TopLevelCollectionUsage TopLevelCollection;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 24)]
    internal struct KeyboardDeviceInfo
    {

        private int type;
        private int subType;
        private int mode;
        private int functionKeyCount;
        private int indicatorCount;
        private int totalKeyCount;


        /// <summary>Gets the type of the keyboard.</summary>
        public int KeyboardType
        {
            get { return type; }
        }

        /// <summary>Gets the subtype of the keyboard.</summary>
        public int KeyboardSubtype
        {
            get { return subType; }
        }

        /// <summary>Gets the scan code mode.</summary>
        public int Mode
        {
            get { return mode; }
        }

        /// <summary>Gets the number of function keys on the keyboard.</summary>
        public int FunctionKeyCount
        {
            get { return functionKeyCount; }
        }

        /// <summary>Gets the number of LED indicators on the keyboard.</summary>
        public int IndicatorCount
        {
            get { return indicatorCount; }
        }

        /// <summary>Gets the total number of keys on the keyboard.</summary>
        public int TotalKeyCount
        {
            get { return totalKeyCount; }
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    internal struct MouseDeviceInfo
    {
        private int id;
        private int buttonCount;
        private int sampleRate;
        [MarshalAs(UnmanagedType.Bool)]
        private bool hasHorizontalWheel;

        /// <summary>Gets the identifier of the mouse device.</summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>Gets the number of buttons for the mouse.</summary>
        public int ButtonCount
        {
            get { return buttonCount; }
        }

        /// <summary>Gets the number of data points per second.
        /// <para>This information may not be applicable for every mouse device.</para>
        /// </summary>
        public int SampleRate
        {
            get { return sampleRate; }
        }

        /// <summary>Gets a value indicating whether the mouse has a wheel for horizontal scrolling.
        /// <para>This member is only supported starting with Windows Vista.</para>
        /// </summary>
        public bool HasHorizontalWheel
        {
            get { return hasHorizontalWheel; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct RawInput
    {

        [StructLayout(LayoutKind.Explicit, Pack = 4)] // Size = 24 bytes (22 rounded to the nearest greater multiple of 4, due to the packing)
        private struct Data
        {

            [FieldOffset(0)]
            internal RawMouse Mouse;

            [FieldOffset(0)]
            internal RawKeyboard Keyboard;

        }


        private RawInputHeader header; // 16/24 bytes (x86/x64)
        private Data data;



        /// <summary>Gets the type of the raw input device.</summary>
        public InputDeviceType DeviceType { get { return header.DeviceType; } }


        /// <summary>Gets a handle to the device generating the raw input data.</summary>
        public IntPtr DeviceHandle { get { return header.DeviceHandle; } }


        /// <summary>The value passed in the WParam parameter of the WM_INPUT message.</summary>
        public IntPtr WParam { get { return header.WParameter; } }


        /// <summary>When <see cref="DeviceType"/> is <see cref="InputDeviceType.Mouse"/>, gets ...</summary>
        public RawMouse? Mouse
        {
            get
            {
                if (header.DeviceType == InputDeviceType.Mouse)
                    return data.Mouse;
                return null;
            }
        }


        /// <summary>When <see cref="DeviceType"/> is <see cref="InputDeviceType.Keyboard"/>, gets ...</summary>
        public RawKeyboard? Keyboard
        {
            get
            {
                if (header.DeviceType == InputDeviceType.Keyboard)
                    return data.Keyboard;
                return null;
            }
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct RawInputDevice
    {
        internal TopLevelCollectionUsage tlc;
        internal RawInputDeviceRegistrationOptions options;
        internal IntPtr targetWindowHandle;

        internal RawInputDevice(TopLevelCollectionUsage tlc, RawInputDeviceRegistrationOptions options, IntPtr targetWindowHandle)
        {
            this.tlc = tlc;
            this.options = options;
            this.targetWindowHandle = targetWindowHandle;
        }

        public TopLevelCollectionUsage TLC
        {
            get { return tlc; }
        }

        public RawInputDeviceRegistrationOptions RegistrationOptions
        {
            get { return options; }
        }

        public IntPtr TargetWindowHandle
        {
            get { return targetWindowHandle; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RawInputDeviceDescriptor
    {

        private IntPtr deviceHandle;
        private InputDeviceType deviceType;


        /// <summary>Gets a handle to the raw input device.</summary>
        internal IntPtr DeviceHandle
        {
            get { return deviceHandle; }
        }


        /// <summary>Gets a value indicating the device type.</summary>
        internal InputDeviceType DeviceType
        {
            get { return deviceType; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct RawInputHeader
    {

        /// <summary>The type of raw input.</summary>
        internal InputDeviceType DeviceType;

        /// <summary>The size, in bytes, of the entire input packet of data.
        /// <para>This includes RAWINPUT plus possible extra input reports in the RAWHID variable length array.</para>
        /// </summary>
        internal int Size;

        /// <summary>A handle to the device generating the raw input data.</summary>
        internal IntPtr DeviceHandle;

        /// <summary>The value passed in the WParam parameter of the WM_INPUT message.</summary>
        internal IntPtr WParameter;



        internal RawInputHeader(InputDeviceType deviceType, int structSize, IntPtr device, IntPtr param)
        {
            DeviceType = deviceType;
            Size = structSize;
            DeviceHandle = device;
            this.WParameter = param;
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 16)]
    internal struct RawKeyboard
    {

        private short makeCode;
        private RawKeyboardFlags flags;
        private short reserved; // must be 0
        private VirtualKeyCode vKey;
        private WindowsMessages message;
        private int extraInformation;

        public short MakeCode
        {
            get { return makeCode; }
        }

        internal RawKeyboardFlags Flags
        {
            get { return flags; }
        }

        internal VirtualKeyCode VKey
        {
            get { return vKey; }
        }

        internal WindowsMessages Message
        {
            get { return message; }
        }

        public int ExtraInformation
        {
            get { return extraInformation; }
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RawMouse
    {
        private RawMouseFlags flags;
        private Data data;
        private uint rawButtons;
        private int xPos;
        private int yPos;
        private uint extraInfo;

        [StructLayout(LayoutKind.Explicit)]
        struct Data
        {
            [FieldOffset(0)]
            public uint Buttons;

            [FieldOffset(2)]
            public short ButtonData;

            [FieldOffset(0)]
            public RawMouseButtons buttonFlags;
        }

        internal RawMouseFlags Flags
        {
            get { return flags; }
        }

        internal RawMouseButtons Buttons
        {
            get { return data.buttonFlags; }
        }

        public short WheelDelta
        {
            get { return data.ButtonData; }
        }

        public int LastX
        {
            get { return xPos; }
        }

        public int LastY
        {
            get { return yPos; }
        }

        public uint ExtraInformation
        {
            get { return extraInfo; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct CursorInfo
    {
        private int structSize;
        private MouseCursorState cursorState;
        private IntPtr cursor;
        private Point screenPosition;

        private CursorInfo(int structSize)
        {
            this.structSize = structSize;
            cursorState = MouseCursorState.Hidden;
            cursor = IntPtr.Zero;
            screenPosition = Point.Zero;
        }

        public MouseCursorState State
        {
            get { return cursorState; }
        }

        public IntPtr Cursor
        {
            get { return cursor; }
        }

        public Point ScreenPosition
        {
            get { return screenPosition; }
            internal set { screenPosition = value; }
        }

        public static readonly CursorInfo Default = new CursorInfo(Marshal.SizeOf(typeof(CursorInfo)));
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HidPCaps
    {
        internal HidUsage Usage;
        internal HidUsagePage UsagePage;
        internal ushort InputReportByteLength;
        internal ushort OutputReportByteLength;
        internal ushort FeatureReportByteLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        internal ushort[] Reserved;
        internal ushort NumberLinkCollectionNodes;
        internal ushort NumberInputButtonCaps;
        internal ushort NumberInputValueCaps;
        internal ushort NumberInputDataIndices;
        internal ushort NumberOutputButtonCaps;
        internal ushort NumberOutputValueCaps;
        internal ushort NumberOutputDataIndices;
        internal ushort NumberFeatureButtonCaps;
        internal ushort NumberFeatureValueCaps;
        internal ushort NumberFeatureDataIndices;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SpDeviceInterfaceData
    {
        public uint cbSize;
        public Guid InterfaceClassGuid;
        internal DeviceInterfaceDataFlags Flags;
        private UIntPtr Reserved;

        private SpDeviceInterfaceData(int size)
        {
            cbSize = (uint)size;
            InterfaceClassGuid = Guid.Empty;
            Flags = DeviceInterfaceDataFlags.SPINT_DEFAULT;
            Reserved = UIntPtr.Zero;
        }

        public static SpDeviceInterfaceData Default
        {
            get { return new SpDeviceInterfaceData(Marshal.SizeOf(typeof(SpDeviceInterfaceData))); }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SpDevInfoData
    {
        public uint cbSize;
        public Guid ClassGuid;
        public uint DevInst;
        private UIntPtr Reserved;

        private SpDevInfoData(int size)
        {
            cbSize = (uint)size;
            ClassGuid = Guid.Empty;
            DevInst = 0;
            Reserved = UIntPtr.Zero;
        }

        public static SpDevInfoData Default
        {
            get { return new SpDevInfoData(Marshal.SizeOf(typeof(SpDevInfoData))); }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct SpDeviceInterfaceDetailData
    {
        public int cbSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DevicePath;

        private SpDeviceInterfaceDetailData(int size)
        {
            cbSize = size;
            DevicePath = string.Empty;
        }

        public static SpDeviceInterfaceDetailData Default
        {
            get { return new SpDeviceInterfaceDetailData(Marshal.SizeOf(typeof(SpDeviceInterfaceDetailData))); }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HidDAttributes
    {
        public ulong Size;
        public ushort VendorId;
        public ushort ProductId;
        public ushort VersionNumber;
    }
}
