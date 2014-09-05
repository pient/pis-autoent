using System;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt.Security
{
    public interface IAuthPackage
    {
        /// <summary>
        /// auth name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// password
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// is password encrypted
        /// </summary>
        bool PasswordEncrypted { get; set; }

        /// <summary>
        /// auth type
        /// </summary>
        string AuthType { get; set; }
    }

    /// <summary>
    /// authenticate package
    /// </summary>
    public class AuthPackage : DTOBase, IAuthPackage
    {
        #region fields

        public const string LOGIN_NAME = "LOGIN_NAME";
        public const string PASSWORD = "PASSWORD";
        public const string PASSWORD_ENCRYPTED = "PASSWORD_ENCRYPTED";
        public const string AUTH_TYPE = "AUTH_TYPE";

        #endregion

        #region properties

        public string Name
        {
            get
            {
                return CLRHelper.ConvertValue<string>(base.Get(LOGIN_NAME));
            }
            set
            {
                base.Set(LOGIN_NAME, value);
            }
        }

        public string Password
        {
            get
            {
                return CLRHelper.ConvertValue<string>(base.Get(PASSWORD));
            }
            set
            {
                base.Set(PASSWORD, value);
            }
        }

        public bool PasswordEncrypted
        {
            get
            {
                return CLRHelper.ConvertValue<bool>(base.Get(PASSWORD_ENCRYPTED));
            }
            set
            {
                base.Set(PASSWORD_ENCRYPTED, value);
            }
        }

        public string AuthType
        {
            get
            {
                return CLRHelper.ConvertValue<string>(base.Get(AUTH_TYPE));
            }
            set
            {
                base.Set(AUTH_TYPE, value);
            }
        }

        #endregion

        #region 构造函数

        public AuthPackage()
        {
        }

        public AuthPackage(string name, string password)
            : this(name, password, false)
        {
        }

        public AuthPackage(string name, string password, bool passwordEncrypted)
            : this(name, password, passwordEncrypted, "Forms")
        {
        }

        public AuthPackage(string name, string password, bool passwordEncrypted, string authType)
        {
            this.Name = name;
            this.Password = password;
            this.PasswordEncrypted = passwordEncrypted;
            this.AuthType = authType;
        }

        #endregion
    }
}
