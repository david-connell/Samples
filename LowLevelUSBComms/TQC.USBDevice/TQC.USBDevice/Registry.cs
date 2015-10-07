using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQC.USBDevice
{
    internal static class Registry
    {
        static string Root
        {
            get
            {
                return @"HKEY_CURRENT_USER\Software\TQC\USBGeneric";
            }
        }

        public static bool ReadBool(string value, bool defaultValue = false)
        {
            try
            {
                var result = Microsoft.Win32.Registry.GetValue(Root, value, defaultValue ? 1 : 0);
                return ((int)result) != 0;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int ReadInt(string value, int defaultValue = 0)
        {
            try
            {
                var result = Microsoft.Win32.Registry.GetValue(Root, value, defaultValue);
                return (int)result;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void SetValue(string value, bool defaultValue)
        {
            SetValue(value, defaultValue ? 1 : 0);
        }
        public static void SetValue(string value, int defaultValue)
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(Root, value, defaultValue);
            }
            catch
            {

            }
        }
    }

    public class Configuration
    {
        public bool UseNativeCommunication
        {
            get
            {
                return Registry.ReadBool("UseNativeCommunication", true);
            }
            set
            {
                Registry.SetValue("UseNativeCommunication", value);
            }
        }

        public int GROTimeoutInMilliseconds
        {
            get
            {
                return Registry.ReadInt("GROTimeoutInMilliseconds", 200);
            }
            set
            {
                Registry.SetValue("GROTimeoutInMilliseconds", value);
            }
        }
    }

}
