using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS4Mono
{
    [Flags]
    internal enum MouseCursorState : int
    {
        Hidden = 0x00000000,
        Showing = 0x00000001,
        Suppressed = 0x00000002
    }

    [Flags]
    internal enum DeviceNotify : uint
    {
        Window = 0x00000000,
        Service = 0x00000001,
        AllInterfaceClasses = 0x00000004
    }

    internal enum BroadcastDeviceType : uint
    {
        DeviceInterface = 0x00000005,
        Handle = 0x00000006,
        OEM = 0x00000000,
        Port = 0x00000003,
        Volume = 0x00000002
    }

    internal enum InputDeviceType : int
    {
        Mouse = 0,
        Keyboard = 1,
        HID = 2
    }

    internal enum GetDataCommand : int
    {

        /// <summary>Get the raw data from the <see cref="RawInput"/> structure.</summary>
        Input = 0x10000003,

        /// <summary>Get the header information from the <see cref="RawInput"/> structure.</summary>
        Header = 0x10000005

    }

    internal enum GetInfoCommand : int
    {

        /// <summary>Data points to the previously parsed data.</summary>
        PreParsedData = 0x20000005,

        /// <summary>Data points to a string that contains the device name.
        /// <para>For this command only, the value in size is the character count (not the byte count).</para>
        /// </summary>
        DeviceName = 0x20000007,

        /// <summary>Data points to a <see cref="DeviceInfo"/> structure.</summary>
        DeviceInfo = 0x2000000b

    }

    internal enum TopLevelCollectionUsage : int
    {

        /// <summary>Undefined.</summary>
        None = 0,


        #region Usage page 1

        /// <summary>Pointers.</summary>
        Pointer = (1 << 16) | 1,

        /// <summary>Mice.</summary>
        Mouse = (2 << 16) | 1,

        /// <summary>Joysticks.</summary>
        Joystick = (4 << 16) | 1,

        /// <summary>Gamepads.</summary>
        Gamepad = (5 << 16) | 1,

        /// <summary>Keyboards.</summary>
        Keyboard = (6 << 16) | 1,

        /// <summary>Keypads.</summary>
        Keypad = (7 << 16) | 1,

        /// <summary>System controls.</summary>
        SystemControl = (0x80 << 16) | 1,

        #endregion Usage page 1


        #region Usage page 12

        /// <summary></summary>
        ConsumerAudioControl = (1 << 16) | 12,

        #endregion Usage page 12

    }

    [Flags]
    internal enum RawInputDeviceRegistrationOptions : int
    {

        /// <summary>No option specified.</summary>
        None = 0x00000000,

        /// <summary>If set, this removes the top level collection (TLC) from the inclusion list.
        /// <para>This tells the operating system to stop reading from a device which matches the top level collection.</para>
        /// If Remove is set and the TargetWindowHandle member is not set to NULL, then parameter validation will fail.
        /// </summary>
        Remove = 0x00000001,

        /// <summary>If set, this specifies the top level collections (TLC) to exclude when reading a complete usage page.
        /// This flag only affects a TLC whose usage page is already specified with <see cref="RawInputDeviceRegistrationOptions.PageOnly"/>.
        /// </summary>
        Exclude = 0x00000010,

        /// <summary>If set, this specifies all devices whose top level collection (TLC) is from the specified UsagePage; note that Usage must be zero.
        /// To exclude a particular top level collection, use <see cref="RawInputDeviceRegistrationOptions.Exclude"/>.
        /// </summary>
        PageOnly = 0x00000020,

        /// <summary>If set, this prevents any devices specified by UsagePage or Usage from generating legacy messages.
        /// <para>This is only for the mouse and keyboard.</para>
        /// If this value is set for a mouse or a keyboard, the system does not generate any legacy message for that device for the application.
        /// For example, if the mouse TLC is set with NoLegacy, WM_LBUTTONDOWN and related legacy mouse messages are not generated.
        /// Likewise, if the keyboard TLC is set with NoLegacy, WM_KEYDOWN and related legacy keyboard messages are not generated.
        /// </summary>
        NoLegacy = Exclude | PageOnly,

        /// <summary>If set, this enables the caller to receive the input even when the caller is not in the foreground; note that targetWindowHandle must be specified.</summary>
        InputSink = 0x00000100,

        /// <summary>If set, the mouse button click does not activate the other window.</summary>
        CaptureMouse = 0x00000200,

        /// <summary>If set, the application-defined keyboard device hotkeys are not handled.
        /// <para>However, the system hotkeys (for example, ALT+TAB and CTRL+ALT+DEL) are still handled. By default, all keyboard hotkeys are handled.</para>
        /// NoHotKeys can be specified even if <see cref="RawInputDeviceRegistrationOptions.NoLegacy"/> is not specified and TargetWindowHandle is NULL.
        /// </summary>
        NoHotkeys = CaptureMouse,

        /// <summary>If set, the application command keys are handled.
        /// AppKeys can be specified only if <see cref="RawInputDeviceRegistrationOptions.NoLegacy"/> is specified for a keyboard device.</summary>
        AppKeys = 0x00000400,

        /// <summary>If set, this enables the caller to receive input in the background only if the foreground application does not process it.
        /// <para>In other words, if the foreground application is not registered for raw input, then the background application that is registered will receive the input.</para>
        /// </summary>
        ExInputSink = 0x00001000,

        /// <summary>If set, this enables the caller to receive WindowMessage.InputDeviceChange (0x00FE) notifications for device arrival (WParam = 1) and device removal (WParam = 2).</summary>
        DevNotify = 0x00002000

    }

    [Flags]
    internal enum RawMouseButtons : short
    {

        /// <summary>No state specified.</summary>
        None = 0x0000,


        /// <summary>The left mouse button is down/pressed.</summary>
        LeftButtonDown = 0x0001,

        /// <summary>The left mouse button is up/released.</summary>
        LeftButtonUp = 0x0002,


        /// <summary>The right mouse button is down/pressed.</summary>
        RightButtonDown = 0x0004,

        /// <summary>The right mouse button is up/released.</summary>
        RightButtonUp = 0x0008,


        /// <summary>The middle mouse button is down/pressed.</summary>
        MiddleButtonDown = 0x0010,

        /// <summary>The middle mouse button is up/released.</summary>
        MiddleButtonUp = 0x0020,


        /// <summary>The extended mouse button 1 is down/pressed.</summary>
        XButton1Down = 0x0040,

        /// <summary>The extended mouse button 1 is up/released.</summary>
        XButton1Up = 0x0080,


        /// <summary>The extended mouse button 2 is down/pressed.</summary>
        XButton2Down = 0x0100,

        /// <summary>The extended mouse button 2 is up/released.</summary>
        XButton2Up = 0x0200,


        /// <summary>The mouse wheel value changed.</summary>
        Wheel = 0x0400

    }

    [Flags]
    internal enum RawMouseFlags : short
    {

        /// <summary>Mouse movement data is relative to the last mouse position.</summary>
        MoveRelative = 0x0000,

        /// <summary>Mouse movement data is based on absolute position.</summary>
        MoveAbsolute = 0x0001,

        /// <summary>Mouse coordinates are mapped to the virtual desktop (for a multiple monitor system).</summary>
        VirtualDesktop = 0x0002,

        /// <summary>Mouse attributes changed; application needs to query the mouse attributes.</summary>
        AttributesChanged = 0x0004,

    }

    [Flags]
    public enum MouseButtons : short
    {
        None = RawMouseButtons.None,
        Left = RawMouseButtons.LeftButtonDown,
        Middle = RawMouseButtons.MiddleButtonDown,
        Right = RawMouseButtons.RightButtonDown,
        X1 = RawMouseButtons.XButton1Down,
        X2 = RawMouseButtons.XButton2Down,
        WheelUp = RawMouseButtons.LeftButtonUp,
        WheelDown = RawMouseButtons.RightButtonUp
    }

    [Flags]
    internal enum RawKeyboardFlags : ushort
    {
        KeyDown = 0,
        KeyUp = 1,
        KeyE0 = 2,
        KeyE1 = 4,
        TerminalServerSetLED = 8,
        TerminalSerevrShadow = 0x10,
        TerminalServerVKPacket = 0x20
    }

    internal enum VirtualKeyCode : ushort
    {

        /// <summary>No key/button.</summary>
        None = 0,

        #region Mouse

        /// <summary>The left mouse button.</summary>
        MouseLeft = 1,

        /// <summary>The right mouse button.</summary>
        MouseRight = 2,

        /// <summary>The middle mouse button.</summary>
        MouseMiddle = 4,

        /// <summary>The extended mouse button 1.</summary>
        MouseX1 = 5,

        /// <summary>The extended mouse button 2.</summary>
        MouseX2 = 6,

        #endregion Mouse


        #region Keyboard

        /// <summary>The CANCEL key.</summary>
        Cancel = 3,


        /// <summary>The BACKSPACE key.</summary>
        Back = 8,

        /// <summary>The TAB key.</summary>
        Tab = 9,

        /// <summary>The LINE FEED key.</summary>
        Linefeed = 10,

        /// <summary>The CLEAR key.</summary>
        Clear = 12,

        /// <summary>The ENTER or RETURN key.</summary>
        Enter = 13,


        /// <summary>The SHIFT key.</summary>
        ShiftKey = 16,

        /// <summary>The CTRL key.</summary>
        ControlKey = 17,

        /// <summary>The ALT key.</summary>
        Menu = 18,

        /// <summary>The PAUSE key.</summary>
        Pause = 19,

        /// <summary>The CAPS LOCK key.</summary>
        CapsLock = 20,


        /// <summary>The IME Kana mode key.</summary>
        KanaMode = 21,

        /// <summary>The IME Hangul mode key.</summary>
        HangulMode = 21,

        /// <summary>The IME Junja mode key.</summary>
        JunjaMode = 23,

        /// <summary>The IME final mode key.</summary>
        FinalMode = 24,

        /// <summary>The IME Hanja mode key.</summary>
        HanjaMode = 25,

        /// <summary>The IME Kanji mode key.</summary>
        KanjiMode = 25,



        /// <summary>The ESC key.</summary>
        Escape = 27,


        /// <summary>The IME Convert key.</summary>
        IMEConvert = 28,

        /// <summary>The IME Nonconvert key.</summary>
        IMENonconvert = 29,

        /// <summary>The IME Accept key.</summary>
        IMEAccept = 30,

        /// <summary>The IME mode modification key.</summary>
        IMEModeChange = 31,


        /// <summary>The SPACEBAR key.</summary>
        Space = 32,

        /// <summary>The PAGE UP key.</summary>
        PageUp = 33,

        /// <summary>The PAGE DOWN key.</summary>
        PageDown = 34,

        /// <summary>The END key.</summary>
        End = 35,

        /// <summary>The HOME key.</summary>
        Home = 36,


        /// <summary>The LEFT ARROW key.</summary>
        Left = 37,

        /// <summary>The UP ARROW key.</summary>
        Up = 38,

        /// <summary>The RIGHT ARROW key.</summary>
        Right = 39,

        /// <summary>The DOWN ARROW key.</summary>
        Down = 40,


        /// <summary>The SELECT key.</summary>
        Select = 41,

        /// <summary>The PRINT key.</summary>
        Print = 42,

        /// <summary>The EXECUTE key.</summary>
        Execute = 43,

        /// <summary>The PRINT SCREEN key.</summary>
        PrintScreen = 44,

        /// <summary>The INS key.</summary>
        Insert = 45,

        /// <summary>The DEL key.</summary>
        Delete = 46,

        /// <summary>The HELP key.</summary>
        Help = 47,

        /// <summary>The 0 key.</summary>
        D0 = 48,

        /// <summary>The 1 key.</summary>
        D1 = 49,

        /// <summary>The 2 key.</summary>
        D2 = 50,

        /// <summary>The 3 key.</summary>
        D3 = 51,

        /// <summary>The 4 key.</summary>
        D4 = 52,

        /// <summary>The 5 key.</summary>
        D5 = 53,

        /// <summary>The 6 key.</summary>
        D6 = 54,

        /// <summary>The 7 key.</summary>
        D7 = 55,

        /// <summary>The 8 key.</summary>
        D8 = 56,

        /// <summary>The 9 key.</summary>
        D9 = 57,


        /// <summary>The A key.</summary>
        A = 65,

        /// <summary>The B key.</summary>
        B = 66,

        /// <summary>The C key.</summary>
        C = 67,

        /// <summary>The D key.</summary>
        D = 68,

        /// <summary>The E key.</summary>
        E = 69,

        /// <summary>The F key.</summary>
        F = 70,

        /// <summary>The G key.</summary>
        G = 71,

        /// <summary>The H key.</summary>
        H = 72,

        /// <summary>The I key.</summary>
        I = 73,

        /// <summary>The J key.</summary>
        J = 74,

        /// <summary>The K key.</summary>
        K = 75,

        /// <summary>The L key.</summary>
        L = 76,

        /// <summary>The M key.</summary>
        M = 77,

        /// <summary>The N key.</summary>
        N = 78,

        /// <summary>The O key.</summary>
        O = 79,

        /// <summary>The P key.</summary>
        P = 80,

        /// <summary>The Q key.</summary>
        Q = 81,

        /// <summary>The R key.</summary>
        R = 82,

        /// <summary>The S key.</summary>
        S = 83,

        /// <summary>The T key.</summary>
        T = 84,

        /// <summary>The U key.</summary>
        U = 85,

        /// <summary>The V key.</summary>
        V = 86,

        /// <summary>The W key.</summary>
        W = 87,

        /// <summary>The X key.</summary>
        X = 88,

        /// <summary>The Y key.</summary>
        Y = 89,

        /// <summary>The Z key.</summary>
        Z = 90,


        /// <summary>The left Windows logo key (Microsoft Natural Keyboard).</summary>
        LWin = 91,

        /// <summary>The right Windows logo key (Microsoft Natural Keyboard).</summary>
        RWin = 92,

        /// <summary>The application key (Microsoft Natural Keyboard).</summary>
        Apps = 93,

        /// <summary>The SLEEP key.</summary>
        Sleep = 95,


        /// <summary>The 0 key on the numeric keypad.</summary>
        NumPad0 = 96,

        /// <summary>The 1 key on the numeric keypad.</summary>
        NumPad1 = 97,

        /// <summary>The 2 key on the numeric keypad.</summary>
        NumPad2 = 98,

        /// <summary>The 3 key on the numeric keypad.</summary>
        NumPad3 = 99,

        /// <summary>The 4 key on the numeric keypad.</summary>
        NumPad4 = 100,

        /// <summary>The 5 key on the numeric keypad.</summary>
        NumPad5 = 101,

        /// <summary>The 6 key on the numeric keypad.</summary>
        NumPad6 = 102,

        /// <summary>The 7 key on the numeric keypad.</summary>
        NumPad7 = 103,

        /// <summary>The 8 key on the numeric keypad.</summary>
        NumPad8 = 104,

        /// <summary>The 9 key on the numeric keypad.</summary>
        NumPad9 = 105,

        /// <summary>The MULTIPLY key.</summary>
        Multiply = 106,

        /// <summary>The ADD key.</summary>
        Add = 107,

        /// <summary>The SEPARATOR key.</summary>
        Separator = 108,

        /// <summary>The SUBTRACT key.</summary>
        Subtract = 109,

        /// <summary>The DECIMAL key.</summary>
        Decimal = 110,

        /// <summary>The DIVIDE key.</summary>
        Divide = 111,


        /// <summary>The F1 key.</summary>
        F1 = 112,

        /// <summary>The F2 key.</summary>
        F2 = 113,

        /// <summary>The F3 key.</summary>
        F3 = 114,

        /// <summary>The F4 key.</summary>
        F4 = 115,

        /// <summary>The F5 key.</summary>
        F5 = 116,

        /// <summary>The F6 key.</summary>
        F6 = 117,

        /// <summary>The F7 key.</summary>
        F7 = 118,

        /// <summary>The F8 key.</summary>
        F8 = 119,

        /// <summary>The F9 key.</summary>
        F9 = 120,

        /// <summary>The F10 key.</summary>
        F10 = 121,

        /// <summary>The F11 key.</summary>
        F11 = 122,

        /// <summary>The F12 key.</summary>
        F12 = 123,

        /// <summary>The F13 key.</summary>
        F13 = 124,

        /// <summary>The F14 key.</summary>
        F14 = 125,

        /// <summary>The F15 key.</summary>
        F15 = 126,

        /// <summary>The F16 key.</summary>
        F16 = 127,

        /// <summary>The F17 key.</summary>
        F17 = 128,

        /// <summary>The F18 key.</summary>
        F18 = 129,

        /// <summary>The F19 key.</summary>
        F19 = 130,

        /// <summary>The F20 key.</summary>
        F20 = 131,

        /// <summary>The F21 key.</summary>
        F21 = 132,

        /// <summary>The F22 key.</summary>
        F22 = 133,

        /// <summary>The F23 key.</summary>
        F23 = 134,

        /// <summary>The F24 key.</summary>
        F24 = 135,


        /// <summary>The NUM LOCK key.</summary>
        NumLock = 144,

        /// <summary>The SCROLL LOCK key.</summary>
        ScrollLock = 145,

        /// <summary>The left SHIFT key.</summary>
        LShiftKey = 160,

        /// <summary>The right SHIFT key.</summary>
        RShiftKey = 161,

        /// <summary>The left CTRL key.</summary>
        LControlKey = 162,

        /// <summary>The right CTRL key.</summary>
        RControlKey = 163,

        /// <summary>The left ALT key.</summary>
        LMenu = 164,

        /// <summary>The right ALT key.</summary>
        RMenu = 165,


        /// <summary>The browser BACK key.</summary>
        BrowserBack = 166,

        /// <summary>The browser NEXT key.</summary>
        BrowserForward = 167,

        /// <summary>The browser REFRESH key.</summary>
        BrowserRefresh = 168,

        /// <summary>The browser STOP key.</summary>
        BrowserStop = 169,

        /// <summary>The browser SEARCH key.</summary>
        BrowserSearch = 170,

        /// <summary>The browser FAVORITES key.</summary>
        BrowserFavorites = 171,

        /// <summary>The browser HOME key.</summary>
        BrowserHome = 172,

        /// <summary>The VOLUME MUTE key.</summary>
        VolumeMute = 173,

        /// <summary>The VOLUME DOWN key.</summary>
        VolumeDown = 174,

        /// <summary>The VOLUME UP key.</summary>
        VolumeUp = 175,

        /// <summary>The MEDIA NEXT TRACK key.</summary>
        MediaNextTrack = 176,

        /// <summary>The MEDIA PREVIOUS TRACK key.</summary>
        MediaPreviousTrack = 177,

        /// <summary>The MEDIA STOP key.</summary>
        MediaStop = 178,

        /// <summary>The MEDIA PLAY/PAUSE key.</summary>
        MediaPlayPause = 179,

        /// <summary>The LAUNCH MAIL key.</summary>
        LaunchMail = 180,

        /// <summary>The SELECT MEDIA key.</summary>
        SelectMedia = 181,

        /// <summary>The LAUNCH APPLICATION #1 key.</summary>
        LaunchApplication1 = 182,

        /// <summary>The LAUNCH APPLICATION #2 key.</summary>
        LaunchApplication2 = 183,

        /// <summary>The OEM SEMICOLON key.</summary>
        OemSemicolon = 186,

        /// <summary>The OEM 1 key.</summary>
        Oem1 = 186,

        /// <summary>The OEM PLUS key.</summary>
        OemPlus = 187,

        /// <summary>The OEM COMMA key.</summary>
        OemComma = 188,

        /// <summary>The OEM MINUS key.</summary>
        OemMinus = 189,

        /// <summary>The OEM PERIOD key.</summary>
        OemPeriod = 190,

        /// <summary>The OEM 2 key.</summary>
        Oem2 = 191,

        /// <summary>The OEM 3 key.</summary>
        Oem3 = 192,

        /// <summary>The OEM 4 key.</summary>
        Oem4 = 219,

        /// <summary>The OEM 5 key.</summary>
        Oem5 = 220,

        /// <summary>The OEM 6 key.</summary>
        Oem6 = 221,

        /// <summary>The OEM 7 key.</summary>
        Oem7 = 222,

        /// <summary>The OEM 8 key.</summary>
        Oem8 = 223,

        /// <summary>The OEM 102 key.</summary>
        Oem102 = 226,

        /// <summary>The PROCESS key.</summary>
        ProcessKey = 229,

        /// <summary>Permet de passer des caractères Unicode comme s'il s'agissait de séquences de touches.
        /// <para>La valeur de la touche Packet est le mot inférieur d'une valeur de clé virtuelle 32 bits utilisée pour les méthodes d'entrée autres qu'au clavier.</para>
        /// </summary>
        Packet = 231,

        /// <summary>The ATTN key.</summary>
        Attn = 246,

        /// <summary>The CRSEL key.</summary>
        Crsel = 247,

        /// <summary>The EXSEL key.</summary>
        Exsel = 248,

        /// <summary>The ERASE EOF key.</summary>
        EraseEof = 249,

        /// <summary>The PLAY key.</summary>
        Play = 250,

        /// <summary>The ZOOM key.</summary>
        Zoom = 251,

        ///// <summary>Reserved for future use.</summary>
        //NoName = 252,

        /// <summary>The PA1 key.</summary>
        Pa1 = 253,

        /// <summary>The CLEAR key.</summary>
        OemClear = 254,

        #endregion Keyboard

    }

    internal enum WindowsMessages : uint
    {
        Null = 0x0000,
        Create = 0x0001,
        Destroy = 0x0002,
        Move = 0x0003,
        Size = 0x0005,
        Activate = 0x0006,
        SetFocus = 0x0007,
        KillFocus = 0x0008,
        Enable = 0x000A,
        SetRedraw = 0x000B,
        SetText = 0x000C,
        GetText = 0x000D,
        GetTextLength = 0x000E,
        Paint = 0x000F,
        Close = 0x0010,
        QueryEndSession = 0x0011,
        Quit = 0x0012,
        QueryOpen = 0x0013,
        EraseBkgnd = 0x0014,
        SysColorChange = 0x0015,
        EndSession = 0x0016,
        ShowWindow = 0x0018,
        WinIniChange = 0x001A,
        SettingChange = WinIniChange,
        DevModeChange = 0x001B,
        ActivateApp = 0x001C,
        FontChange = 0x001D,
        TimeChange = 0x001E,
        CancelMode = 0x001F,
        SetCursor = 0x0020,
        MouseActivate = 0x0021,
        ChildActivate = 0x0022,
        QueueSync = 0x0023,
        GetMinMaxInfo = 0x0024,
        PaintIcon = 0x0026,
        IconEraseBkgnd = 0x0027,
        NextDlgCtl = 0x0028,
        SpoolerStatus = 0x002A,
        DrawItem = 0x002B,
        MeasureItem = 0x002C,
        DeleteItem = 0x002D,
        VKeyToItem = 0x002E,
        CharToItem = 0x002F,
        SetFont = 0x0030,
        GetFont = 0x0031,
        SetHotkey = 0x0032,
        GetHotkey = 0x0033,
        QueryDragIcon = 0x0037,
        CompareItem = 0x0039,
        GetObject = 0x003D,
        Compacting = 0x0041,
        COMNotify = 0x0044,
        WindowPosChanging = 0x0046,
        WindowPosChanged = 0x0047,
        Power = 0x0048,
        CopyData = 0x004A,
        CancelJournal = 0x004B,
        Notify = 0x004E,
        InputLangChangeRequest = 0x0050,
        InputLangChange = 0x0051,
        TCard = 0x0052,
        Help = 0x0053,
        UserChanged = 0x0054,
        NotifyFormat = 0x0055,
        ContextMenu = 0x007B,
        StyleChanging = 0x007C,
        StyleChanged = 0x007D,
        DisplayChange = 0x007E,
        GetIcon = 0x007F,
        SetIcon = 0x0080,
        NCCreate = 0x0081,
        NCDestroy = 0x0082,
        NCCalSize = 0x0083,
        NCHitTest = 0x0084,
        NCPaint = 0x0085,
        NCActivate = 0x0086,
        GetDlgCode = 0x0087,
        SyncPaint = 0x0088,
        NCMouseMove = 0x00A0,
        NCLButtonDown = 0x00A1,
        NCLButtonUp = 0x00A2,
        NCLButtonClick = 0x00A3,
        NCRButtonDown = 0x00A4,
        NCRButtonUp = 0x00A5,
        NCRButtonClick = 0x00A6,
        NCMButtonDown = 0x00A7,
        NCMButtonUp = 0x00A8,
        NCMButtonClick = 0x00A9,
        NCXButtonDown = 0x00AB,
        NCXButtonUp = 0x00AC,
        NCXButtonClick = 0x00AD,
        InputDeviceChange = 0x00FE,
        Input = 0x00FF,
        KeyFirst = 0x0100,
        KeyDown = 0x0100,
        KeyUp = 0x0101,
        Char = 0x0102,
        DeadChar = 0x0103,
        SysKeyDown = 0x0104,
        SysKeyUp = 0x0105,
        SysChar = 0x0106,
        SysDeadChar = 0x0107,
        KeyLast = 0x0108,
        UniChar = 0x0109,
        IMEStartComposition = 0x010D,
        IMEEndComposition = 0x010E,
        IMEComposition = 0x010F,
        IMEKeylast = 0x010F,
        InitDialog = 0x0110,
        Command = 0x0111,
        SysCommand = 0x0112,
        Timer = 0x0113,
        HScroll = 0x0114,
        VScroll = 0x0115,
        InitMenu = 0x0116,
        InitMenuPopup = 0x0117,
        MenuSelect = 0x011F,
        MenuChar = 0x0120,
        EnterIdle = 0x0121,
        MenuRButtonUp = 0x0122,
        MenuDrag = 0x0123,
        MenuGetObject = 0x0124,
        UnInutMenuPopup = 0x0125,
        MenuCommand = 0x0126,
        ChangeUIState = 0x0127,
        UpdateUIState = 0x0128,
        QueryUIState = 0x0129,
        CtlColorMsgBox = 0x0132,
        CtlColorEdit = 0x0133,
        CtlColorListBox = 0x0134,
        CtlColorBtn = 0x0135,
        CtlColorDlg = 0x0136,
        CtlColorScrollBar = 0x0137,
        CtlColorStatic = 0x0138,
        MouseFirst = 0x0200,
        MouseMove = 0x0200,
        LButtonDown = 0x0201,
        LButtonUp = 0x0202,
        LbuttonDblClk = 0x0203,
        RButtonDown = 0x0204,
        RButtonUp = 0x0205,
        RButtonDblClk = 0x0206,
        MButtonDown = 0x0207,
        MButtonUp = 0x0208,
        MButtonDblClk = 0x0209,
        MouseWheel = 0x020A,
        XButtonDown = 0x020B,
        XButtonUp = 0x020C,
        XButtonDblClk = 0x020D,
        MouseLast = 0x020E,
        ParentNotify = 0x0210,
        EnterMenuLoop = 0x0211,
        ExitMenuLoop = 0x0212,
        NextMenu = 0x0213,
        Sizing = 0x0214,
        captureChanged = 0x0215,
        Moving = 0x0216,
        PowerBroadcast = 0x0218,
        DeviceChange = 0x0219,
        MDICreate = 0x0220,
        MDiDestroy = 0x0221,
        MDIActivate = 0x0222,
        MDIRestore = 0x0223,
        MDINext = 0x0224,
        MDIMaximize = 0x0225,
        MDITitle = 0x0226,
        MDICascade = 0x0227,
        MDIIconArrange = 0x0228,
        MDIGetActive = 0x0229,
        MDISetMenu = 0x0230,
        EnterSizeMove = 0x0231,
        ExitSizeMove = 0x0232,
        DropFiles = 0x0233,
        MDIRefreshMenu = 0x0234,
        IMESetContext = 0x0281,
        IMENotify = 0x0282,
        IMEControl = 0x0283,
        IMECompositionFull = 0x0284,
        IMESelect = 0x0285,
        IMEChar = 0x0286,
        IMERequest = 0x0288,
        IMEKeyDown = 0x0290,
        IMEKeyUp = 0x029,
        MouseHover = 0x02A1,
        MouseLeave = 0x02A3,
        NCMouseHover = 0x02A0,
        NCMouseLeave = 0x02A2,
        WTSessionChange = 0x02B1,
        TabletFirst = 0x02C0,
        TabletLast = 0x02df,
        Cut = 0x0301,
        Paste = 0x0302,
        Clear = 0x0303,
        Undo = 0x0304,
        RenderFormat = 0x0305,
        RenderAllFormats = 0x0306,
        DestroyClipboard = 0x0307,
        DrawClipboard = 0x0308,
        PaintClipboard = 0x0309,
        VScrollClipboard = 0x030A,
        SizeClipboard = 0x030B,
        AskCBFormatName = 0x030C,
        ChangeCBChain = 0x030D,
        HSScrollClipboard = 0x030E,
        QueryNewPalette = 0x030F,
        PaletteIsChanging = 0x0310,
        PaletteChanged = 0x0311,
        Hotkey = 0x0312,
        Print = 0x0317,
        PrintClient = 0x0318,
        AppCommand = 0x0319,
        TimeChanged = 0x031A,
        ClipboardUpdate = 0x031D,
        DWMCompositionChanged = 0x031E,
        DWMNCRenderingChanged = 0x031F,
        DWMColorizationColorChanged = 0x0320,
        DWMWindowMaximizedChanged = 0x0321,
        GetTitleBarInfoEX = 0x033F,
        HandheldFirst = 0x0358,
        HandheldLast = 0x032F,
        AFXFirst = 0x0360,
        AFXLast = 0x037F,
        PenWinFirst = 0x0380,
        PenWinLast = 0x038F,
        App = 0x8000,
        User = 0x0400,
        CPLLaunch = User + 0x1000,
        CPLLaunched = User + 0x01001,
        SysTimer = 0x0118
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
        Trackpad = 0x02000000,

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

    [Flags]
    internal enum DiGetClassFlags : uint
    {
        DIGCF_DEFAULT = 0x00000001,  // only valid with DIGCF_DEVICEINTERFACE
        DIGCF_PRESENT = 0x00000002,
        DIGCF_ALLCLASSES = 0x00000004,
        DIGCF_PROFILE = 0x00000008,
        DIGCF_DEVICEINTERFACE = 0x00000010
    }

    [Flags]
    internal enum DeviceInterfaceDataFlags : uint
    {
        SPINT_ACTIVE = 0x00000001,
        SPINT_DEFAULT = 0x00000002,
        SPINT_REMOVED = 0x00000004
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
