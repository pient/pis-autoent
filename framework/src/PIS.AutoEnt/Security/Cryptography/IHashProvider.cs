using System;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt.Security
{
    public interface IHashProvider
    {
        bool CompareHash(byte[] plaintext, byte[] hashedtext);

        byte[] CreateHash(byte[] plaintext);
    }
}
