using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace PIS.AutoEnt.Security
{
    /// <summary>
    /// MD5 Hash
    /// </summary>
    public class MD5HashProvider : IHashProvider
    {
        public bool CompareHash(byte[] plaintext, byte[] hashedtext)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateHash(byte[] plaintext)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashed = md5.ComputeHash(plaintext, 0, plaintext.Length);

            return hashed;
        }
    }
}
