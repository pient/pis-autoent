using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace PIS.AutoEnt
{
    public static class CLRHelper
    {
        #region 程序集处理

        /// <summary>
        /// 根据程序集文件名获取程序集
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static Assembly[] GetAssemblysByNames(params string[] assemblyNames)
        {
            if (assemblyNames == null || assemblyNames.Length == 0)
            {
                return null;
            }

            Assembly[] assms = new Assembly[assemblyNames.Length];

            for (int i = 0; i < assemblyNames.Length; i++)
            {
                assms[i] = Assembly.Load(assemblyNames[i]);
            }

            return assms;
        }

        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            try
            {
                string fullName = nameSpace + "." + className;  //命名空间.类型名

                //加载程序集，创建程序集里面的 命名空间.类型名 实例
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);
                return (T)ect;  //类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值 
                return default(T);
            }
        }

        public static T CreateInstance<T>(Type type, params object[] args)
        {
            try
            {
                object obj = Activator.CreateInstance(type, args);//根据类型创建实例
                return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值 
                return default(T);
            }
        }

        public static T CreateInstance<T>(params object[] args)
        {
            return CreateInstance<T>(typeof(T), args);
        }

        #endregion

        #region 类型处理

        /// <summary>
        /// 过滤Null值
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T FilterNull<T>(object val, T def)
        {
            if (val == null)
            {
                return def;
            }
            else
            {
                return (T)val;
            }
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr, T defValue, bool ignoreCase)
        {
            if (enumStr == null)
            {
                return defValue;
            }

            T enumVal = default(T);

            try
            {
                enumVal = (T)Enum.Parse(typeof(T), enumStr.ToString(), ignoreCase);
            }
            catch
            {
                enumVal = defValue;
            }

            return enumVal;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr, T defValue)
        {
            T enumVal = GetEnum<T>(enumStr, defValue, true);

            return enumVal;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr, bool ignoreCase)
        {
            T enumVal = GetEnum<T>(enumStr, default(T), ignoreCase);

            return enumVal;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr)
        {
            T enumVal = GetEnum<T>(enumStr, true);

            return enumVal;
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertValue<T>(object value)
        {
            return ConvertValue<T>(value, default(T));
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static T ConvertValue<T>(object value, T defValue)
        {
            T objVal = defValue;

            if (value != null)
            {
                try
                {
                    objVal = (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    try
                    {
                        objVal = (T)value;
                    }
                    catch { }
                }
            }

            return objVal;
        }

        /// <summary>
        /// 合并对象数据(指定属性)
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        public static T MergeObject<T>(T target, object source, IEnumerable<string> keys = null)
        {
            var _t_props = typeof(T).GetProperties();
            var _s_props = source.GetType().GetProperties();

            if (keys == null)
            {
                keys = _s_props.Select(p => p.Name);
            }

            foreach (PropertyInfo pi in _t_props)
            {
                var _s_pi = _s_props.FirstOrDefault(p => p.Name == pi.Name);

                if (pi.CanWrite && keys.Contains(pi.Name) && _s_pi != null)
                {
                    pi.SetValue(target, _s_pi.GetValue(source, null), null);
                }
            }

            return target;
        }

        #endregion

        #region 序列化/反序列化

        /// <summary>
        /// 序列化对象为Base64类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToBase64(object obj)
        {
            return Convert.ToBase64String(SerializeToBytes(obj));
        }

        /// <summary>
        /// 序列化对象为byte数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        /// <summary>
        /// 从byte数组反序列化成指定类型对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeFromBytes<T>(byte[] bytes)
        {
            object obj = DeserializeFromBytes(bytes);
            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        /// <summary>
        /// 从byte数组反序列化对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object DeserializeFromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }

            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new UBinder();
            MemoryStream ms = new MemoryStream(bytes);

            ms.Position = 0;

            return formatter.Deserialize(ms);
        }

        /// <summary>
        /// 从base64字符串反序列化成指定类型对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeFromBase64<T>(string str)
        {
            object obj = DeserializeFromBase64(str);

            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        /// <summary>
        /// 从Base64反序列化对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object DeserializeFromBase64(string str)
        {
            byte[] bs = Convert.FromBase64String(str);

            return DeserializeFromBytes(bs);
        }

        /// <summary>
        /// 序列化对象到文件 (默认BinaryFormatter)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SerializeToBinaryFile(object obj, string filePath)
        {
            using (FileStream file = File.Open(filePath, FileMode.Create))
            {
                IFormatter bf = new BinaryFormatter();
                bf.Serialize(file, obj);

                file.Close();
            }
        }

        /// <summary>
        /// 从文件反序列化对象 (BinaryFormatter)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeserializeFromBinaryFile(string filePath)
        {
            object obj = null;

            using (FileStream file = File.Open(filePath, FileMode.Open))
            {
                IFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(file);
            }

            return obj;
        }

        /// <summary>
        /// 从文件反序列化对象 (BinaryFormatter)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeserializeFromBinaryFile<T>(string filePath)
        {
            object obj = DeserializeFromBinaryFile(filePath);

            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        /// <summary>
        /// 序列化对象为Xml文件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SerializeToSoapFile(object obj, string filePath)
        {
            using (FileStream file = File.Open(filePath, FileMode.Create))
            {
                IFormatter serializer = new SoapFormatter();

                serializer.Serialize(file, obj);

                file.Close();
            }
        }

        /// <summary>
        /// 从Xml文件反序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeserializeFromSoapFile(string filePath)
        {
            object obj = null;

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                IFormatter formatter = new SoapFormatter();

                obj = formatter.Deserialize(fs);
            }

            return obj;
        }

        /// <summary>
        /// 从文件反序列化对象 (BinaryFormatter)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeserializeFromSoapFile<T>(string filePath)
        {
            object obj = DeserializeFromSoapFile(filePath);

            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        /// <summary>
        /// 序列化对象为Xml文件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SerializeToXmlFile(object obj, string filePath)
        {
            using (FileStream file = File.Open(filePath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());

                serializer.Serialize(file, obj);

                file.Close();
            }
        }

        /// <summary>
        /// 从Xml文件反序列化对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeserializeFromXmlFile<T>(string filePath)
        {
            T obj = default(T);

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {

                XmlSerializer formatter = new XmlSerializer(typeof(T));

                obj = (T)formatter.Deserialize(fs);
            }

            return obj;
        }

        /// <summary>
        /// 序列化为Xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXmlString(object obj)
        {
            string xmlstr = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                ser.Serialize(sw, obj);
                xmlstr = sw.ToString();
            }

            return xmlstr;
        }

        /// <summary>
        /// 由Xml字符串反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeserializeFromXmlString<T>(string xmlstr)
        {
            T rtn = default(T);

            using (StringReader sr = new StringReader(xmlstr))
            {
                XmlSerializer xz = new XmlSerializer(typeof(T));
                rtn = (T)xz.Deserialize(sr);
            }

            return rtn;
        }

        public class UBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                return ass.GetType(typeName);
            }
        }

        #endregion

        #region IO操作

        public static byte[] ReadStream(Stream stream)
        {
            return ReadStream(stream, 10000);
        }

        public static byte[] ReadStream(Stream stream, int bufferSize)
        {
            byte[] sbytes = new byte[stream.Length];
            byte[] buffer;

            if (stream.Length < bufferSize)
            {
                buffer = new byte[stream.Length];
            }
            else
            {
                buffer = new byte[bufferSize];
            }

            long dataToRead = stream.Length;
            long dataReaded = 0;
            int length = 0;

            stream.Position = 0;

            while (dataToRead > 0)
            {
                length = stream.Read(buffer, 0, buffer.Length);

                buffer.CopyTo(sbytes, dataReaded);

                dataReaded = dataReaded + length;
                dataToRead = dataToRead - length;

                if (dataToRead < bufferSize)
                {
                    buffer = new byte[dataToRead];
                }
                else
                {
                    buffer = new byte[bufferSize];
                }
            }

            return sbytes;
        }

        /// <summary>
        /// 复制字节
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyBytes(byte[] from, byte[] to, long fromindex, long toindex)
        {
            if (to == null)
            {
                toindex = 0;

                to = new byte[from.Length - fromindex];
            }

            long dataToCopy = from.Length - fromindex;  // 需要拷贝的数据长度
            long spaceLeft = to.Length - toindex;   // 还剩下的空间

            long copylength = (dataToCopy < spaceLeft) ? dataToCopy : spaceLeft;

            for (int i = 0; i < copylength; i++)
            {
                to[toindex + i] = from[fromindex + i];
            }
        }

        #endregion
    }
}
