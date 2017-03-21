using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace PS4Mono
{
    internal static class NativeMethods
    {
        private const string Lib = "User32.dll";
        private const string hid = "Hid.dll";
        private const string setup = @"SetupAPI.dll";
        private const long InvalidHandleValue = -1;

        #region Win32

        internal static Exception GetExceptionForLastWin32Error()
        {
            int errorCode = Marshal.GetLastWin32Error();
            Exception ex = Marshal.GetExceptionForHR(errorCode);
            if (ex == null)
                ex = new Win32Exception(errorCode);
            return ex;
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern int GetRawInputDeviceList(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1), Optional] RawInputDeviceDescriptor[] rawInputDeviceList,
            [In, Out] ref int deviceCount,
            [In] int size
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern int GetRawInputDeviceInfoW(
            [In, Optional] IntPtr deviceHandle,
            [In] GetInfoCommand command,
            [In, Out, Optional] ref DeviceInfo data,
            [In, Out] ref int dataSize
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern int GetRawInputDeviceInfoW(
            [In, Optional] IntPtr devHandle,
            [In] GetInfoCommand command,
            [In, Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 3), Optional] string data,
            [In, Out] ref int dataSize
        );


        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RegisterRawInputDevices(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] RawInputDevice[] rawInputDevices,
            [In] int deviceCount,
            [In] int size
        );

        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern int GetRegisteredRawInputDevices(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1), Optional] RawInputDevice[] rawInputDevices,
            [In, Out] ref int deviceCount,
            [In] int structSize
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern int GetRawInputData(
            [In] IntPtr rawInputHandle,
            [In] GetDataCommand command,
            [Out, Optional] out RawInput data,
            [In, Out] ref int size,
            [In] int headerSize
        );

        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        private static extern int GetRawInputBuffer(
            [Out, MarshalAs(UnmanagedType.LPArray), Optional] RawInput[] data,
            [In, Out] ref int size,
            [In] int headerSize
        );

        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr RegisterDeviceNotificationW(
            [In] IntPtr windowHandle,
            [In] IntPtr notificationFilter,
            [In] DeviceNotify flags
        );

        [DllImport(Lib, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterDeviceNotification(
            [In] IntPtr deviceNotificationFilter
        );

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        internal static extern SafeFileHandle CreateFileW(
            [In] string fileName,
            [In] FileAccess desiredAccess,
            [In] FileShare shareMode,
            [In, Optional] IntPtr securityAttribs,
            [In] FileMode creationDisposition,
            [In] FileAttributes flagsAndAttribs,
            [In, Optional] IntPtr templateFile
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        internal static extern void HidD_GetHidGuid(
            [Out] out Guid HidGuid
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool HidD_GetPreparsedData(
            [In] SafeFileHandle HidDevice,
            [In, Out] ref IntPtr preparsedData
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool HidD_FreePreparsedData(
            [In] ref IntPtr preparsedData
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        internal static extern NTStatus HidP_GetCaps(
            [In] IntPtr preparsedData,
            [In, Out] ref HidPCaps caps
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool HidD_GetAttributes(
            [In] SafeFileHandle handle,
            [In, Out] ref HidDAttributes attributes
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static extern bool HidD_GetProductString(
            [In] SafeFileHandle hidDevice,
            [In, Out] char* buffer,
            [In] int bufferLength
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static extern bool HidD_GetSerialNumberString(
            [In] SafeFileHandle handle,
            [In, Out] char* buffer,
            [In] int bufferLength
        );

        [DllImport(hid, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal unsafe static extern bool HidD_GetManufacturerString(
            [In] SafeFileHandle handle,
            [In, Out] char* buffer,
            [In] int bufferLength
        );

        [DllImport(setup, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr SetupDiGetClassDevsW(
            [In, Optional] ref Guid classGuid,
            [In, MarshalAs(UnmanagedType.LPTStr), Optional] string enumerator,
            [In, Optional] IntPtr hwnd,
            [In] DiGetClassFlags flags
        );

        [DllImport(setup, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiDestroyDeviceInfoList(
            [In] IntPtr deviceInfoList
        );

        [DllImport(setup, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiEnumDeviceInterfaces(
            [In] IntPtr deviceInfoList,
            [In, Optional] ref SpDevInfoData deviceInfoData,
            [In] ref Guid interfaceGuid,
            [In] uint memberIndex,
            [Out] out SpDeviceInterfaceData devideInterfaceData
        );

        [DllImport(setup, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiEnumDeviceInterfaces(
            [In] IntPtr deviceInfoList,
            [In, Optional] IntPtr deviceInfoData,
            [In] ref Guid interfaceGuid,
            [In] uint memberIndex,
            [Out] out SpDeviceInterfaceData devideInterfaceData
        );

        [DllImport(setup, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceInterfaceDetailW(
            [In] IntPtr deviceInfoList,
            [In] ref SpDeviceInterfaceData deviceInterfaceData,
            [In, Out, Optional] ref SpDeviceInterfaceDetailData deviceInterfaceDetailData,
            [In] int deviceInterfaceDetailDataSize,
            [Out, Optional] out int requiredSize,
            [In, Out, Optional] ref SpDevInfoData deviceInfoData
        );

        #endregion

        #region Wrappers

        internal static RawInputDeviceDescriptor[] GetRawInputDeviceList()
        {
            var size = Marshal.SizeOf(typeof(RawInputDeviceDescriptor));
            var deviceCount = 0;

            var result = GetRawInputDeviceList(null, ref deviceCount, size);
            if (result == -1)
                throw new Win32Exception("Failed to retrieve raw input device count", GetExceptionForLastWin32Error());

            if (deviceCount < 0)
                throw new InvalidDataException("Invalid raw input device count: " + deviceCount.ToString(System.Globalization.CultureInfo.InvariantCulture));

            var list = new RawInputDeviceDescriptor[deviceCount];
            result = GetRawInputDeviceList(list, ref deviceCount, size);

            if (result == -1)
                throw new Win32Exception("Failed to retrieve raw input device count", GetExceptionForLastWin32Error());

            if (result == list.Length)
                return list;

            throw new InvalidDataException("Failed to retrieve raw input device count: device count mismatch.");
        }

        internal static DeviceInfo GetRawInputDeviceInfo(IntPtr deviceHandle, bool preParsed)
        {
            var devInfo = DeviceInfo.Default;
            var devInfoStructSize = devInfo.StructureSize;

            var result = GetRawInputDeviceInfoW(deviceHandle, preParsed ? GetInfoCommand.PreParsedData : GetInfoCommand.DeviceInfo, ref devInfo, ref devInfoStructSize);

            if (result == devInfoStructSize)
                return devInfo;

            if (result == -1)
                throw new NotSupportedException("Failed to retrieve raw input device info: DeviceInfo size mismatch.");
            else if (result == 0)
                throw new NotSupportedException("Failed to retrieve raw input device info: bad implementation.");
            else
                throw new Win32Exception("Failed to retrieve raw input device info.", GetExceptionForLastWin32Error());
        }

        internal static string GetRawInputDeviceName(IntPtr devHandle)
        {
            var charCount = 0;

            var result = GetRawInputDeviceInfoW(devHandle, GetInfoCommand.DeviceName, null, ref charCount);
            if (result != 0)
            {
                if (result > 0)
                    throw new NotSupportedException("Failed to retrieve raw input device name buffer size: bad implementation.");
                throw new Win32Exception("Failed to retrieve raw input device name buffer size.", GetExceptionForLastWin32Error());
            }

            if (charCount <= 0)
                throw new NotSupportedException("Invalid raw input device name character count.");

            var buffer = new string('\0', charCount);
            result = GetRawInputDeviceInfoW(devHandle, GetInfoCommand.DeviceName, buffer, ref charCount);
            if (result == charCount)
                return buffer;

            if (result == 0)
                throw new NotSupportedException("Failed to retrieve raw input device name: bad implementation.");

            if (result == -1)
                throw new Win32Exception("Failed to retrieve raw input device name.", GetExceptionForLastWin32Error());

            throw new NotSupportedException("Failed to retrieve raw input device name: string length mismatch (bad implementation).");
        }

        internal static void RegisterRawInputDevices(params RawInputDevice[] rawInputDevices)
        {
            if (rawInputDevices == null)
                throw new ArgumentNullException("rawInputDevices");

            var devCount = rawInputDevices.Length;

            if (devCount == 0)
                throw new ArgumentException("Array is empty.", "rawInputDevices");

            RawInputDevice device;
            for (var d = 0; d < devCount; d++)
            {
                device = rawInputDevices[d];
                if (device.RegistrationOptions.HasFlag(RawInputDeviceRegistrationOptions.Remove) && device.TargetWindowHandle != IntPtr.Zero)
                    throw new ArgumentException("Target window handle must be null when Remove flag is set.");
            }

            if (!RegisterRawInputDevices(rawInputDevices, devCount, Marshal.SizeOf(typeof(RawInputDevice))))
                throw new Win32Exception("Failed to register raw input device(s).", GetExceptionForLastWin32Error());
        }

        internal static RawInputDevice[] GetRegisteredRawInputDevices()
        {
            int structSize = Marshal.SizeOf(typeof(RawInputDevice));
            int devCount = 0;
            var result = GetRegisteredRawInputDevices(null, ref devCount, structSize);
            if (result != 0)
            {
                if (result == -1)
                    throw new Win32Exception("Failed to retrieve registered raw input device count.", GetExceptionForLastWin32Error());
                throw new InvalidDataException("Failed to retrieve raw input device count.");
            }

            if (devCount < 0)
                throw new InvalidDataException("Invalid registered raw input device count.");

            var rawInputDevices = new RawInputDevice[devCount];
            result = GetRegisteredRawInputDevices(rawInputDevices, ref devCount, structSize);

            if (result == -1)
                throw new Win32Exception("Failed to retrieve registered raw input devices.", GetExceptionForLastWin32Error());

            return rawInputDevices;
        }

        internal static void GetRawInputData(IntPtr devHandle, out RawInput rawInput)
        {
            var headerSize = Marshal.SizeOf(typeof(RawInputHeader));
            var size = Marshal.SizeOf(typeof(RawInput));

            var result = GetRawInputData(devHandle, GetDataCommand.Input, out rawInput, ref size, headerSize);
            if (result == -1)
                throw new Win32Exception("Failed to retrieve raw input data.", GetExceptionForLastWin32Error());
        }

        internal static RawInput[] GetRawInputBuffer()
        {
            var headerSize = Marshal.SizeOf(typeof(RawInputHeader));
            var size = 0;
            var result = GetRawInputBuffer(null, ref size, headerSize);
            if (result == -1)
                throw new Win32Exception("Failed to retrieve raw input buffer size.", GetExceptionForLastWin32Error());

            var count = size / Marshal.SizeOf(typeof(RawInput));
            var buffer = new RawInput[count];

            result = GetRawInputBuffer(buffer, ref size, headerSize);
            if (result == -1)
                throw new Win32Exception("Failed to retrieve raw input buffer.", GetExceptionForLastWin32Error());

            return buffer;
        }

        internal unsafe static bool TryRegisterPS4Controller(IntPtr handle, out Playstation4Input controller)
        {
            controller = null;
            var devName = GetRawInputDeviceName(handle);
            char[] arr = new char[126];
            string product = "";
            var devHandle = CreateFileW(devName, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);
            fixed (char* p = arr)
            {
                if (HidD_GetProductString(devHandle, p, 126))
                {
                    product = new string(p);
                    var index = product.IndexOf('\0');
                    if (index != -1)
                        product.Remove(index);
                }
            }

            if (product == "Wireless Controller")
            {
                var preparsedData = IntPtr.Zero;
                if (!HidD_GetPreparsedData(devHandle, ref preparsedData))
                {
                    devHandle.Close();
                    return false;
                }

                HidPCaps caps = new HidPCaps();
                if (HidP_GetCaps(preparsedData, ref caps) != NTStatus.Success)
                {
                    devHandle.Close();
                    return false;
                }

                if (caps.InputReportByteLength == 64)
                {
                    controller = new Playstation4Input(devHandle, caps);
                    return true;
                }
                else
                {
                    devHandle.Close();
                    return false;
                }
            }
            devHandle.Close();
            return false;

        }

        #endregion
    }
}
