using System;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt.Security
{
    public interface ISymmetricCryptoProvider
    {
        byte[] Decrypt(byte[] ciphertext);

        byte[] Encrypt(byte[] plaintext);
    }
}
