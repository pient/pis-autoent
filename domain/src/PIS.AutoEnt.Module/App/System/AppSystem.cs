using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.App.Interfaces;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 涵盖应用启动的相关信息
    /// </summary>
    [PISInterceptor]
    public class AppSystem
    {
        #region 事件



        #endregion

        #region 成员属性

        private IAppProvider appProvider = null;

        public static IAppProvider AppProvider
        {
            get { return Instance.appProvider; }
        }

        #endregion

        #region 构造函数，单体模式

        static AppSystem instance;

        internal static AppSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppSystem();
                }

                return instance;
            }
        }

        protected AppSystem()
        {
            appProvider = ObjectFactory.Resolve<IAppProvider>();
        }

        #endregion

        #region 公共方法

        #endregion

        #region 私有方法


        #endregion

        #region 静态方法

        /// <summary>
        /// 检查系统是否有效
        /// </summary>
        /// <returns></returns>
        [PISLogging]
        internal static void VerifySystem()
        {
            VerifySysLData();
        }

        /// <summary>
        /// 验证License
        /// </summary>
        [PISLogging]
        internal static void VerifySysLData()
        {
            try
            {
                string lsrc = String.Empty;

                Config.ConfigManager.AppSettings.TryGetValue("License", out lsrc);

                if (String.IsNullOrEmpty(lsrc))
                {
                    lsrc = GlobalConsts.DefaultServerLicensePath;
                }

                lsrc = SystemHelper.GetPath(lsrc);

                if (File.Exists(lsrc))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(lsrc);

                    XmlDocument xmlLDoc = new XmlDocument();
                    string ldata = DecryptSysLData(xmlDoc.DocumentElement);
                    xmlLDoc.LoadXml(ldata);

                    XmlNode eDateNode = xmlLDoc.SelectSingleNode("./License/ExpiredDate");

                    if (eDateNode != null)
                    {
                        DateTime eDate = CLRHelper.ConvertValue<DateTime>(eDateNode.InnerText, DateTime.Now);

                        if (DateTime.Now >= eDate)
                        {
                            throw new PISInnerException(ErrorCode.LicenseExpired);
                        }
                    }
                    else
                    {
                        throw new PISInnerException(ErrorCode.LicenseBadFormat);
                    }
                }
                else
                {
                    throw (new PISInnerException(ErrorCode.LicenseNotFind));
                }
            }
            catch (PISLicException lex)
            {
                throw lex;
            }
            catch (Exception ex)
            {
                string macInfo = String.Format("CPU 号：{0}\r\n硬盘号：{1}\r\nMAC地址：{2}\r\nMac Code：{3}\r\n",
                    SystemHelper.GetCPUID(), SystemHelper.GetHardDiskID(), SystemHelper.GetMACAddress(), SystemHelper.GetMACCode());

                throw (new PISLicException(ErrorCode.LicenseInvalid, "Mac info:" + macInfo + ex.Message));
            }
        }

        internal static string DecryptSysLData(XmlNode lData)
        {
            string maccode = SystemHelper.GetMACCode();

            return DecryptSysLData(lData, maccode);
        }

        internal static string DecryptSysLData(XmlNode lData, string maccode)
        {
            string lstr = lData.InnerXml;

            lstr = CryptoManager.GetDecryptStringByMAC(lstr, maccode);

            return lstr;
        }

        internal static string EncryptSysLData(XmlNode lData, string maccode)
        {
            string lstr = lData.OuterXml;

            lstr = CryptoManager.GetEncryptStringByMAC(lstr, maccode);

            lstr = "<License IsProtected=\"true\">" + lstr + "</License>";

            return lstr;
        }

        #endregion
    }
}
