using System;

namespace PS4Mono
{
    /// <summary>
    /// Controller Axes
    /// </summary>
    public enum Axis
    {
        LeftX,
        LeftY,
        RightX,
        RightY
    }

    internal enum InputDeviceType : int
    {
        Mouse = 0,
        Keyboard = 1,
        HID = 2
    }

    internal enum GetInfoCommand : int
    {
        PreParsedData = 0x20000005,
        DeviceName = 0x20000007,
        DeviceInfo = 0x2000000b

    }

    internal enum TopLevelCollectionUsage : int
    {
        None = 0,
        Pointer = (1 << 16) | 1,
        Mouse = (2 << 16) | 1,
        Joystick = (4 << 16) | 1,
        Gamepad = (5 << 16) | 1,
        Keyboard = (6 << 16) | 1,
        Keypad = (7 << 16) | 1,
        SystemControl = (0x80 << 16) | 1,
        ConsumerAudioControl = (1 << 16) | 12,
    }

    [Flags]
    internal enum RawInputDeviceRegistrationOptions : int
    {
        None = 0x00000000,
        Remove = 0x00000001,
        Exclude = 0x00000010,
        PageOnly = 0x00000020,
        NoLegacy = Exclude | PageOnly,
        InputSink = 0x00000100,
        CaptureMouse = 0x00000200,
        NoHotkeys = CaptureMouse,
        AppKeys = 0x00000400,
        ExInputSink = 0x00001000,
        DevNotify = 0x00002000
    }

    [Flags]
    internal enum ControllerButtons : uint
    {
        None = 0x00000000,
        Face1 = 0x00000001,
        Face2 = 0x00000002,
        Face3 = 0x00000004,
        Face4 = 0x00000008,
        DpadUp = 0x00000010,
        DpapdRight = 0x00000020,
        DpadDown = 0x00000040,
        DpadLeft = 0x00000080,
        RightButton = 0x00000100,
        LeftButton = 0x00000200,
        RightTrigger = 0x00000400,
        LeftTrigger = 0x00000800,
        Start = 0x00001000,
        Select = 0x00002000,
        RightStick = 0x00004000,
        LeftStick = 0x00008000,
        LeftStickUp = 0x00010000,
        LeftStickLeft = 0x00020000,
        LeftStickDown = 0x00040000,
        LeftStickRight = 0x00080000,
        RightStickUp = 0x00100000,
        RightStickLeft = 0x00200000,
        RightStickDown = 0x00400000,
        RightStickRight = 0x00800000,
        Symbol = 0x01000000,
        Trackpad = 0x02000000
    }

    internal enum HidUsagePage : ushort
    {
        Undefined = 0x00,
        Generic = 0x01,
        Simulation = 0x02,
        VirtualReality = 0x03,
        Sport = 0x04,
        Gamepad = 0x05,
        Keyboard = 0x07,
        LED = 0x08
    }

    internal enum HidUsage : ushort
    {
        Undefined = 0x00,
        Pointer = 0x01,
        Mouse = 0x02,
        Joystick = 0x04,
        Gamepad = 0x05,
        Keyboard = 0x06,
        Keypad = 0x07,
        SystemControl = 0x80,
        Tablet = 0x80,
        Consumer = 0x0C
    }

    internal enum NTStatus : uint
    {
        Success = 0x110000,
        Null = 0x80110001,
        InvalidPreparsedData = 0xC0110001,
        InvalidReportType = 0xC0110002,
        InvalidReportLength = 0xC0110003,
        UsageNotFound = 0xC0110004,
        ValueOutOfRange = 0xC0110005,
        BadLogPhyValues = 0xC0110006,
        BufferTooSmall = 0xC0110007,
        InternalError = 0xC0110008,
        I8042TransUnknown = 0xC0110009,
        IncompatibleReportId = 0xC011000A,
        NotValueArray = 0xC011000B,
        IsvalueArray = 0xC011000C,
        DataIndexNotFound = 0xC011000D,
        DataIndexOutOfRange = 0xC011000E,
        ButtonNotPresseed = 0xC011000F,
        ReportDoesNotExist = 0xC0110010,
        NotImplemented = 0xC0110020
    }
}
