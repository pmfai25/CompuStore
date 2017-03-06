using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Register
{
    public class Finger
    {
        private static string value;
        public static string Value
        {
            get
            {
                if (value == null)
                    value = GetHash(HardDriveSerial());
                return value;
            }
        }
        private static string GetHash(string s)
        {
            var value = MD5.Create().ComputeHash(Encoding.Default.GetBytes(s));
            return Convert.ToBase64String(value);
        }
        #region Original Device ID Getting Code
        //Return a hardware identifier
        private static string GetProperties(string wmiClass, string property)
        {
            var mc = new ManagementClass(wmiClass);
            StringBuilder sb = new StringBuilder();
            var moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                sb.Append(mo.Properties[property].Value);
                break;
            }
            return sb.ToString();
        }
        private static string HardDriveSerial()
        {
            return GetProperties("Win32_DiskDrive", "SerialNumber");
        }
        #endregion
    }
}
