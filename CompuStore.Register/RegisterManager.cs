using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Register
{
    public class RegisterManager
    {
        public static bool IsValidSerial(string serial)
        {
            return Finger.Value == Cryptor.Decrypt(serial);
        }
        public static string GetSerial()
        {
            return Cryptor.Encrypt(Finger.Value);
        }
    }

}
