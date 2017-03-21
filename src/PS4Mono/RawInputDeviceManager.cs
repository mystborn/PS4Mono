using System;
using System.Collections.Generic;
using static PS4Mono.NativeMethods;

namespace PS4Mono
{
    internal static class RawInputDeviceManager
    {
        private static HashSet<IntPtr> Ignore;
        private static RawInputDeviceDescriptor[] LastList;

        internal static void Initialize(IntPtr hwnd)
        {
            Ignore = new HashSet<IntPtr>();

            LastList = GetRawInputDeviceList();
            for (int i = 0; i < LastList.Length; i++)
            {
                var device = LastList[i];
                if (device.DeviceType == InputDeviceType.HID)
                {
                    Playstation4Input controller;
                    if (TryRegisterPS4Controller(device.DeviceHandle, out controller))
                        InputManager.Controllers.Add(controller);
                    Ignore.Add(device.DeviceHandle);
                }
            }

            //If you wanted to look for new devices more often, change the interval here to your desired time.
            var poll = new System.Timers.Timer();
            poll.Interval = 2000;
            poll.Elapsed += (s, e) => PollDevices();
            poll.AutoReset = true;
            poll.Enabled = true;
        }

        private static void PollDevices()
        {
            var devices = GetRawInputDeviceList();
            if (devices != LastList)
            {
                var temp = new HashSet<IntPtr>();
                for (int i = 0; i < devices.Length; i++)
                {
                    var device = devices[i];
                    if (device.DeviceType == InputDeviceType.HID)
                    {
                        if (!Ignore.Contains(device.DeviceHandle))
                        {
                            Playstation4Input controller;
                            if (TryRegisterPS4Controller(device.DeviceHandle, out controller))
                            {
                                InputManager.Controllers.Add(controller);
                            }
                        }
                        temp.Add(device.DeviceHandle);
                    }
                }
                Ignore = temp;
                LastList = devices;
            }
        }
    }
}
