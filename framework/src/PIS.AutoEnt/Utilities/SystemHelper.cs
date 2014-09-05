using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Management;
using System.Threading;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt
{
    public static class SystemHelper
    {
        #region 应用信息

        /// <summary>
        /// 获取当前时间，由于考虑到国际化，此处可能为UTC时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取基应用名
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationBaseName()
        {
            string appBase = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.TrimEnd('\\');
            appBase = appBase.Substring((appBase.LastIndexOf('\\') + 1));

            return appBase;
        }

        /// <summary>
        /// 由相对路径获取全路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetPath(string relativePath)
        {
            string path = GetPath();

            if (!String.IsNullOrEmpty(path))
            {
                path = Path.Combine(path, relativePath);
            }

            return path;
        }

        /// <summary>
        /// 获取系统路径
        /// </summary>
        public static string GetPath()
        {
            string path = null;

            if (System.Environment.CurrentDirectory != null)
            {
                // Windows路径
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                // Mobile路径
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            }

            return path;
        }

        #endregion

        #region 机器码

        /// <summary>
        /// 获取机器码 (组合CPU编号，硬盘编号和网卡地址)
        /// </summary>
        /// <returns></returns>
        public static string GetMACCode()
        {
            try
            {
                string cpuid = GetCPUID();
                string hdid = GetHardDiskID();
                string mac = GetMACAddress();

                // 组合生成机器码
                string maccode_str = CryptoManager.GetHashString(String.Format("{0}X:X{1}X:X{2}", cpuid, hdid, mac));

                Regex regex = new Regex(@"\W");
                maccode_str = regex.Replace(maccode_str, "6").ToUpper(); // 所有非字符替换成6, 并全部转换为大写

                // 拆成3组
                string[] m_arr = new string[6];
                m_arr[0] = maccode_str.Substring(0, 2);
                m_arr[1] = maccode_str.Substring(2, 2);
                m_arr[2] = maccode_str.Substring(4, 2);
                m_arr[3] = maccode_str.Substring(6, 2);
                m_arr[4] = maccode_str.Substring(8, 2);
                m_arr[5] = maccode_str.Substring(10, 2);

                string maccode = String.Format("{0}-{1}-{2}-{3}-{4}-{5}", m_arr[0], m_arr[1], m_arr[2], m_arr[3], m_arr[4], m_arr[5]);

                return maccode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取CPU编号
        /// </summary>
        /// <returns></returns>
        public static string GetCPUID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取第一块硬盘编号
        /// </summary>
        /// <returns></returns>
        public static String GetHardDiskID()
        {
            try
            {
                //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                //ManagementObjectCollection moc = searcher.Get();
                ManagementClass mc = new ManagementClass("Win32_PhysicalMedia");
                ManagementObjectCollection moc = mc.GetInstances();

                String strHardDiskID = null;

                foreach (ManagementObject mo in moc)
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }

                return strHardDiskID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取机器MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                if (moc.Count > 0)
                {
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo["MacAddress"] != null)
                        {
                            mac = mo["MacAddress"].ToString();
                            break;
                        }

                        /*if ((bool)mo["IPEnabled"] == true)
                        {
                            mac = mo["MacAddress"].ToString();
                            break;
                        }*/
                    }
                }

                moc = null;
                mc = null;

                return mac;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 编码操作

        /// <summary>
        /// 返回按特定的时间排序的GUID，可用于数据库ID以提高检索效率
        /// COMB (GUID 与时间混合型) 类型 GUID 数据
        /// </summary>
        /// <returns></returns>
        public static Guid NewCombId()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }

        /// <summary>
        /// 从 CombId 中生成时间信息
        /// </summary>
        /// <param name="combId">包含时间信息的 CombId</param>
        /// <returns></returns>
        public static DateTime GetDateFromCombId(System.Guid combId)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);

            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = combId.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays.
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);
            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

        #endregion
    }
}
