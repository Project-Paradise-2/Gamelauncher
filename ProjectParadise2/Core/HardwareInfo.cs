using ProjectParadise2.Core.JsonClasses;
using System;

namespace ProjectParadise2.Core
{
    internal class HardwareInfo
    {
        public static DeviceInfo deviceInfo;

        public static void Getinfo()
        {
            try
            {
                deviceInfo = new DeviceInfo();
            }
            catch (Exception) { }
        }
    }
}