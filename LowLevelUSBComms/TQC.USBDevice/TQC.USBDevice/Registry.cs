using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace TQC.USBDevice
{
    internal static class Registry
    {
        private static ILog s_Log = LogManager.GetLogger("TQC.USBDevice.Registry");
        static string Root
        {
            get
            {
                return @"HKEY_CURRENT_USER\Software\TQC\USBGeneric";
            }
        }

        public static bool ReadBool(string key, bool defaultValue = false)
        {
            try
            {
                var result = Microsoft.Win32.Registry.GetValue(Root, key, defaultValue ? 1 : 0);
                return ((int)result) != 0;
            }
            catch (Exception ex)
            {
                s_Log.Error("Get value", ex);
                return defaultValue;
            }
        }

        public static string ReadString(string value, string defaultValue = "")
        {
            try
            {
                var result = Microsoft.Win32.Registry.GetValue(Root, value, defaultValue);
                return result as string;
            }
            catch (Exception ex)
            {
                s_Log.Error("Get value", ex);
                return defaultValue;
            }
        }

        public static int ReadInt(string key, int defaultValue = 0)
        {
            s_Log.InfoFormat("Read int {0} {1}", key, defaultValue);
            try
            {
                var result = Microsoft.Win32.Registry.GetValue(Root, key, defaultValue);
                s_Log.InfoFormat("Read {0} {1}", key, (int)result);
                return (int)result;
            }
            catch (Exception ex)
            {
                s_Log.Error("Get value", ex);
                return defaultValue;
            }
        }

        public static void SetValue(string key, bool defaultValue)
        {
            SetValue(key, defaultValue ? 1 : 0);
        }
        public static void SetValue(string key, int defaultValue)
        {
            s_Log.InfoFormat("Write {0} {1}", key, defaultValue);
            try
            {
                Microsoft.Win32.Registry.SetValue(Root, key, defaultValue);
            }
            catch (Exception ex)
            {
                s_Log.Error("Set value int", ex);
            }
        }
        public static void SetValue(string key, string defaultValue)
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(Root, key, defaultValue);
            }
            catch (Exception ex)
            {
                s_Log.Error("Set value string", ex);
            }
        }
    }

    public class CacheConfiguration
    {
        public bool UseNativeCommunication { get; private set; }
        public int GROTimeoutInMilliseconds { get; private set; }
        public CacheConfiguration()
        {
            var config = new Configuration();
            UseNativeCommunication = config.UseNativeCommunication;
            GROTimeoutInMilliseconds = config.GROTimeoutInMilliseconds;
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

        public int PreSleepMilliseconds
        {
            get
            {
                return Registry.ReadInt("PreSleepInMilliseconds", 0);
            }
            set
            {
                Registry.SetValue("PreSleepInMilliseconds", value);
            }
        }

        public int PostSleepMilliseconds
        {
            get
            {
                return Registry.ReadInt("PostSleepMilliseconds", 0);
            }
            set
            {
                Registry.SetValue("PostSleepMilliseconds", value);
            }
        }

        public int NativeLoggingFlush
        {
            get
            {
                return Registry.ReadInt("Flush", 0);
            }
            set
            {
                Registry.SetValue("Flush", value);
            }
        }

        public string NativeDebugFile
        {
            get
            {
                return Registry.ReadString("DebugFileName", null);
            }
            set
            {
                Registry.SetValue("DebugFileName", value);
            }
        }

    }

}
