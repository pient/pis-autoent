using System;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt.Security
{
    public interface ICryptoManager
    {
        string EncryptString(string base64Str);

        string DecryptString(string base64Str);
    }
}
