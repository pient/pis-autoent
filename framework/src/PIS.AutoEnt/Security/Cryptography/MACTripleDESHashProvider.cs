using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace PIS.AutoEnt.Security
{
    /// <summary>
    /// 数字签名
    /// </summary>
    public class MACTripleDESHashProvider : IHashProvider
    {
        private MACTripleDES mact;
        private string key = "PIS2012R";

        #region 构造函数 

        internal MACTripleDESHashProvider()
        {
            Init();
        }

        public MACTripleDESHashProvider(string key)
        {
            this.SetKey(key);

            Init();
        }

        private void Init()
        {
            mact = new MACTripleDES();

            byte[] bypt_key = Encoding.ASCII.GetBytes(this.key);

            mact.Key = bypt_key;
        }

        #endregion

        #region 重载 

        public bool CompareHash(byte[] plaintext, byte[] hashedtext)
        {
            throw new NotImplementedException();
        }

        public byte[] CreateHash(byte[] plaintext)
        {
            byte[] hash_b = this.mact.ComputeHash(this.mact.ComputeHash(plaintext));

            return hash_b;
        }

        #endregion

        #region 私有方法

        private void SetKey(string key)
        {
            int keyLength = key.Length;

            int[] keyAllowLengths = new int[] { 8, 16, 24 };
            bool isRight = false;

            foreach (int i in keyAllowLengths)
            {
                if (keyLength == keyAllowLengths[i])
                {
                    isRight = true;
                    break;
                }
            }

            if (!isRight)
            {
                throw new ApplicationException("The length of key must be in 8,16,24.");
            }
            else
            {
                this.key = key;
            }
        }

        #endregion
    }
}
