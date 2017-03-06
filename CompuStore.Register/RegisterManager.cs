using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Register
{
    public class RegisterManager
    {
        public static bool IsRegistered(Dictionary<string, string> serials)
        {
            string finger = Finger.Value;
            foreach (var key in serials.Keys)
                if (key == finger && key == Cryptor.Decrypt(serials[key]))
                    return true;
            return false;
        }
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
