using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace PIS.AutoEnt.Security
{
    public class CryptoManager : ICryptoManager
    {
        #region 成员

        private Encoding encoding = null;

        private IHashProvider hashProvider = null;
        private ISymmetricCryptoProvider cryptoProvider = null;

        #endregion

        #region 构造函数

        private CryptoManager()
        {
            Init();

            cryptoProvider = new DESCryptoProvider();
        }

        private CryptoManager(string cryptoKey)
        {
            Init();

            cryptoProvider = new DESCryptoProvider(cryptoKey);
        }

        private CryptoManager(Encoding encoding)
        {
            Init();
            this.encoding = encoding;

            cryptoProvider = new DESCryptoProvider();
        }

        private CryptoManager(string cryptoKey, Encoding encoding)
        {
            Init();
            this.encoding = encoding;

            cryptoProvider = new DESCryptoProvider(cryptoKey);
        }

        private void Init()
        {
            encoding = UTF8Encoding.UTF8;

            hashProvider = new MD5HashProvider();
        }

        #endregion

        #region 重载

        public byte[] CreateHash(byte[] plaintext)
        {
            return hashProvider.CreateHash(plaintext);
        }

        /// <summary>
        /// 加密字符串并返回加密后的结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string EncryptString(string str)
        {
            byte[] toEncrypt = this.encoding.GetBytes(str);//得到要加密的内容

            byte[] encrypted = cryptoProvider.Encrypt(toEncrypt);

            string encryptedstr = Convert.ToBase64String(encrypted);

            return encryptedstr;
        }

        /// <summary>
        /// 解密给定的字符串
        /// </summary>
        /// <param name="str">要解密的字符</param>
        /// <returns></returns>
        public string DecryptString(string str)
        {
            byte[] toDecrypt = Convert.FromBase64String(str);
            byte[] deCrypted = cryptoProvider.Decrypt(toDecrypt);

            string decryptedstr = this.encoding.GetString(deCrypted);

            return decryptedstr;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 创建Hash字符串
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetHashString(string plaintext)
        {
            return GetHashString(plaintext, Encoding.UTF8);
        }

        /// <summary>
        /// 创建Hash字符串
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetHashString(string plaintext, Encoding encoding)
        {
            CryptoManager cryptoManager = new CryptoManager(encoding);

            string hashstr = Convert.ToBase64String(cryptoManager.CreateHash(encoding.GetBytes(plaintext)));

            return hashstr;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetEncryptString(string str)
        {
            CryptoManager cryptoManager = new CryptoManager();

            string encryptStr = cryptoManager.EncryptString(str);

            return encryptStr;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetEncryptString(string str, string key)
        {
            CryptoManager cryptoManager = new CryptoManager(key);

            string encryptStr = cryptoManager.EncryptString(str);

            return encryptStr;
        }

        /// <summary>
        /// 根据给定的MAC地址加密数据
        /// </summary>
        /// <returns></returns>
        public static string GetEncryptStringByMAC(string str, string mac)
        {
            string key = GetEncryptKey(mac);

            return GetEncryptString(str, key);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDecryptString(string str)
        {
            CryptoManager cryptoManager = new CryptoManager();

            return cryptoManager.DecryptString(str);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDecryptString(string str, string key)
        {
            CryptoManager cryptoManager = new CryptoManager(key);

            return cryptoManager.DecryptString(str);
        }

        /// <summary>
        /// 根据给定的MAC地址解密数据
        /// </summary>
        /// <returns></returns>
        public static string GetDecryptStringByMAC(string str, string mac)
        {
            string key = GetEncryptKey(mac);

            return GetDecryptString(str, key);
        }

        /// <summary>
        /// 根据指定字符串获取加密Key
        /// </summary>
        /// <returns></returns>
        public static string GetEncryptKey(string code)
        {
            return GetEncryptKey(code, 8);
        }

        /// <summary>
        /// 根据指定字符串获取加密Key
        /// </summary>
        /// <param name="code"></param>
        /// <param name="length">加密key长度(默认为8)</param>
        /// <returns></returns>
        public static string GetEncryptKey(string code, int length)
        {
            // 替换所有非字符类型
            Regex regex = new Regex(@"\W");
            string mackey = regex.Replace(code, "");

            if (mackey.Length > length)
            {
                mackey = mackey.Substring(0, length);
            }
            else if (mackey.Length < length)
            {
                string addstr = mackey.Repeat((length - mackey.Length));

                mackey = mackey + addstr;
            }

            mackey = mackey.ToUpper();

            return mackey;
        }

        #endregion
    }
}
