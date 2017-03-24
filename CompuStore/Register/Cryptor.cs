using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Register
{
    internal static class Cryptor
    {
        internal static string Encrypt(string value)
        {
            try
            {
                PasswordDeriveBytes pdb = new PasswordDeriveBytes("Zeroth88", new byte[] { 1, 8, 2, 7, 3, 6, 4, 5 });
                var sa = SymmetricAlgorithm.Create("DES");
                sa.Key = pdb.GetBytes(sa.Key.Length);
                sa.IV = pdb.GetBytes(sa.IV.Length);
                var data = Convert.FromBase64String(value);
                var cipher = sa.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(cipher);
            }
            catch
            {
                return "";
            }
        }
        internal static string Decrypt(string value)
        {
            try
            {
                PasswordDeriveBytes pdb = new PasswordDeriveBytes("Zeroth88", new byte[] { 1, 8, 2, 7, 3, 6, 4, 5 });
                var sa = SymmetricAlgorithm.Create("DES");
                sa.Key = pdb.GetBytes(sa.Key.Length);
                sa.IV = pdb.GetBytes(sa.IV.Length);
                var data = Convert.FromBase64String(value);
                var cipher = sa.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(cipher);
            }
            catch { return ""; }
        }

    }
}
