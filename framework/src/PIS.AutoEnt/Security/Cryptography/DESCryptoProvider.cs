using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PIS.AutoEnt.Security
{
    /// <summary>
    /// DES 对称加解密
    /// </summary>
    public class DESCryptoProvider : ISymmetricCryptoProvider
    {
        #region 成员属性

        private string iv = "PIS2012R"; // 8位
        private string key = "XJILOVEU";    // 8位

        private byte[] ivb, keyb;

        private DES des;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数(internal, 以保护此函数不被滥用)
        /// </summary>
        internal DESCryptoProvider()
        {
            Init();
        }

        internal DESCryptoProvider(string key)
        {
            this.key = key;

            Init();
        }

        private void Init()
        {
            ivb = Encoding.ASCII.GetBytes(this.iv);
            keyb = Encoding.ASCII.GetBytes(this.key);//得到加密密钥

            des = new DESCryptoServiceProvider();
        }

        #endregion

        #region 重载

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] encrypted;

            ICryptoTransform encryptor = des.CreateEncryptor(keyb, ivb);

            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(plaintext, 0, plaintext.Length);
            csEncrypt.FlushFinalBlock();

            encrypted = msEncrypt.ToArray();
            csEncrypt.Close();
            msEncrypt.Close();

            return encrypted;
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            byte[] deCrypted = new byte[ciphertext.Length];

            ICryptoTransform deCryptor = des.CreateDecryptor(keyb, ivb);
            MemoryStream msDecrypt = new MemoryStream(ciphertext);

            CryptoStream csDecrypt = new CryptoStream(msDecrypt, deCryptor, CryptoStreamMode.Read);

            try
            {
                csDecrypt.Read(deCrypted, 0, deCrypted.Length);
            }
            catch (Exception err)
            {
                throw new ApplicationException(err.Message);
            }
            finally
            {
                try
                {
                    msDecrypt.Close();
                    csDecrypt.Close();
                }
                catch { ;}
            }

            return deCrypted;
        }

        #endregion
    }
}
