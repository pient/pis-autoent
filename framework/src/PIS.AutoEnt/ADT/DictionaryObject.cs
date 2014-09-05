using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace PIS.AutoEnt
{
    public interface IDictionaryObject : IDictionary
    {
    }

    /// <summary>
    /// base dictionary object
    /// </summary>
    [Serializable]
    [CollectionDataContract]
    [XmlRoot("dict")]
    public abstract class DictionaryObject : DictionaryBase, IDictionaryObject, IXmlSerializable
    {
        #region fields

        public const string XML_INNER = "inner";
        public const string XML_ITEM = "item";
        public const string XML_KEY = "key";
        public const string XML_VALUE = "value";

        #endregion

        #region properties

        public ICollection Keys
        {
            get
            {
                return this.InnerHashtable.Keys;
            }
        }

        public ICollection Values
        {
            get
            {
                return this.InnerHashtable.Values;
            }
        }

        #endregion

        #region constructors

        public DictionaryObject() { }

        public DictionaryObject(IDictionary dictionary)
        {
            foreach (object key in dictionary.Keys)
            {
                this.InnerHashtable.Add(key, dictionary[key]);
            }
        }

        #endregion

        #region members

        /// <summary>
        /// get value, if key not exists, return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(object key)
        {
            return this.Get(key, null);
        }

        /// <summary>
        /// get value, if key not exists, return the default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue">default value</param>
        /// <returns></returns>
        public virtual object Get(object key, object defValue)
        {
            object rtn = defValue;

            if (base.InnerHashtable.ContainsKey(key))
            {
                rtn = this.InnerHashtable[key];
            }

            return rtn;
        }

        /// <summary>
        /// set value, if the key already exists, replace the old value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void Set(object key, object value)
        {
            if (base.InnerHashtable.ContainsKey(key))
            {
                base.InnerHashtable[key] = value;
            }
            else
            {
                base.InnerHashtable.Add(key, value);
            }
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        public void Remove(object key)
        {
            if (base.InnerHashtable.ContainsKey(key))
            {
                base.InnerHashtable.Remove(key);
            }
        }

        /// <summary>
        /// 是否包含指定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(object key)
        {
            return base.InnerHashtable.ContainsKey(key);
        }

        /// <summary>
        /// 是否包含指定值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(object value)
        {
            return base.InnerHashtable.ContainsValue(value);
        }

        #endregion

        #region indexes

        protected bool isChanged;
        protected bool isInEditMode;

        public virtual object this[object key]
        {
            get
            {
                return base.InnerHashtable[key];
            }
            set
            {
                base.InnerHashtable[key] = value;
            }
        }

        #endregion

        #region IXmlSerializable members

        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// generic base dictionary object
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    [CollectionDataContract]
    public class DictionaryObject<TKey, TValue> : DictionaryObject
    {
        #region constructors

        public DictionaryObject() { }

        public DictionaryObject(IDictionary dictionary)
            : base(dictionary)
        {
        }

        #endregion

        #region members

        /// <summary>
        /// get value, if key not exists, return null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TValue Get(TKey key)
        {
            return this.Get(key, default(TValue));
        }

        /// <summary>
        /// get value, if key not exists, return the default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue">default value</param>
        /// <returns></returns>
        public virtual TValue Get(TKey key, TValue defValue)
        {
            TValue rtn = defValue;

            if (base.Dictionary.Contains(key))
            {
                rtn = CLRHelper.ConvertValue<TValue>(base.Dictionary[key], defValue);
            }

            return rtn;
        }

        #endregion

        #region IXmlSerializable members

        public override void ReadXml(XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof (TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof (TValue));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (String.Compare(DictionaryObject.XML_INNER, reader.Name, true) == 0)
                {
                    reader.ReadStartElement(DictionaryObject.XML_INNER);

                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.ReadStartElement(DictionaryObject.XML_ITEM);

                        reader.ReadStartElement(DictionaryObject.XML_KEY);
                        TKey key = (TKey)keySerializer.Deserialize(reader);
                        reader.ReadEndElement();

                        reader.ReadStartElement(DictionaryObject.XML_VALUE);
                        TValue value = (TValue)valueSerializer.Deserialize(reader);
                        reader.ReadEndElement();

                        reader.ReadEndElement();

                        this.Set(key, value);
                        reader.MoveToContent();
                    }

                    reader.ReadEndElement();
                }
                else
                {
                    reader.Skip();
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            if (this.Dictionary.Count > 0)
            {
                writer.WriteStartElement(DictionaryObject.XML_INNER);

                foreach (TKey key in this.Dictionary.Keys)
                {
                    writer.WriteStartElement(DictionaryObject.XML_ITEM);

                    writer.WriteStartElement(DictionaryObject.XML_KEY);
                    keySerializer.Serialize(writer, key);
                    writer.WriteEndElement();

                    writer.WriteStartElement(DictionaryObject.XML_VALUE);
                    TValue value = this.Get(key);
                    valueSerializer.Serialize(writer, value);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

        }

        #endregion
    }

    /// <summary>
    /// generic base dictionary object with string key
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    [CollectionDataContract]
    public class StringKeyDictionaryObject<TValue> : DictionaryObject<string, TValue>
    {
        #region constructors

        public StringKeyDictionaryObject() { }

        public StringKeyDictionaryObject(IDictionary dictionary)
            : base(dictionary)
        {
        }

        #endregion
    }

    /// <summary>
    /// generic base dictionary object with string key and value
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    [CollectionDataContract]
    public class StringDictionaryObject : DictionaryObject<string, string>
    {
        #region constructors

        public StringDictionaryObject() { }

        public StringDictionaryObject(IDictionary dictionary)
            : base(dictionary)
        {
        }

        #endregion
    }
}
