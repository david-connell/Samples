using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TQC.GOC.InterProcessCommunication.Installer
{
    internal class MSINativeMethods
    {
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_MORE_DATA = 234;

        [DllImport("msi.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern Int32 MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, [MarshalAs(UnmanagedType.U4), In, Out] ref Int32 len);

        [DllImport("msi", CharSet = CharSet.Auto)]
        public static extern uint MsiEnumRelatedProducts(
                string UpgradeCode,
                uint dwReserved,
                uint iProductIndex,
                StringBuilder ProductBuffer);


    }

    internal class InstallerInformation
    {
        private Guid m_ProductCode;
        public InstallerInformation(Guid productCode)
        {
            m_ProductCode = productCode;
        }

        public static IList<InstallerInformation> GetProducts(Guid upgradeCode)
        {
            List<InstallerInformation> result = new List<InstallerInformation>();
            foreach (var productCode in GetRelatedProducts(upgradeCode))
            {
                result.Add(new InstallerInformation(productCode));
            }
            return result;
        }

        private static IEnumerable<Guid> GetRelatedProducts(Guid upgradeCode)
        {
            uint ret = 0;
            uint i = 0;
            StringBuilder buffer = new StringBuilder(38);
            string guidToEnumerate = upgradeCode.ToString("B").ToUpper();
            while ((ret = MSINativeMethods.MsiEnumRelatedProducts(guidToEnumerate, 0, i, buffer)) == 0)
            {
                Guid thisGuid = new Guid(buffer.ToString());

                yield return thisGuid;

                i++;
            }
            i = ret;
        }


        private static string GetMsiProductInfo(string productCode, string propertyName)
        {
            int size = 0;
            int ret = MSINativeMethods.MsiGetProductInfo(productCode, propertyName, null, ref size);
            if (ret == MSINativeMethods.ERROR_SUCCESS || ret == MSINativeMethods.ERROR_MORE_DATA)
            {
                StringBuilder buffer = new StringBuilder(++size);
                ret = MSINativeMethods.MsiGetProductInfo(productCode, propertyName, buffer, ref size);
                if (ret == MSINativeMethods.ERROR_SUCCESS)
                    return buffer.ToString();
            }
            throw new System.ComponentModel.Win32Exception(ret);
        }

        public string InstalledPath
        {
            get
            {
                string installPath = GetMsiProductInfo(m_ProductCode.ToString("B").ToUpper(), "InstallLocation");
                return installPath;
            }
        }
        public string InstallSource
        {
            get
            {
                string installPath = GetMsiProductInfo(m_ProductCode.ToString("B").ToUpper(), "InstallSource");
                return installPath;
            }
        }
        public string UpdateURL
        {
            get
            {
                string installPath = GetMsiProductInfo(m_ProductCode.ToString("B").ToUpper(), "URLUpdateInfo");
                return installPath;
            }
        }


    }
}
