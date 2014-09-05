using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;

namespace PIS.AutoEnt
{
    /// <summary>
    /// PIS dictionary class
    /// </summary>
    [Serializable]
    [CollectionDataContract]
    public class EasyDictionary : StringKeyDictionaryObject<object>
    {
        #region constructors

        public EasyDictionary() { }

        public EasyDictionary(IDictionary dictionary)
            : base(dictionary)
        {
        }

        public EasyDictionary(IList<EasyDictionary> dicts, string keyField, string textField)
        {
            string keystr = String.Empty;

            foreach (EasyDictionary tdict in dicts)
            {
                keystr = CLRHelper.ConvertValue<string>(tdict.Get(keyField));

                this.Set(keystr, tdict.Get(textField));
            }
        }

        public EasyDictionary(DataTable dt, string keyColumnName, string textColumnName)
        {
            string keystr = String.Empty;

            foreach (DataRow trow in dt.Rows)
            {
                keystr = CLRHelper.ConvertValue<string>(trow[keyColumnName]);

                this.Set(keystr, trow[textColumnName]);
            }
        }

        public EasyDictionary(DataTable dt)
        {
            string keystr = String.Empty;

            if (dt.Columns.Count >= 2)
            {
                string keyColumnName = dt.Columns[0].ColumnName;
                string textColumnName = dt.Columns[1].ColumnName;

                foreach (DataRow trow in dt.Rows)
                {
                    keystr = CLRHelper.ConvertValue<string>(trow[keyColumnName]);

                    this.Set(keystr, trow[textColumnName]);
                }
            }
        }

        #endregion

        #region 公共方法

        public virtual T Get<T>(string key)
        {
            return this.Get<T>(key, default(T));
        }

        public virtual T Get<T>(string key, T defValue)
        {
            T rtn = defValue;

            if (this.InnerHashtable.ContainsKey(key))
            {
                rtn = CLRHelper.ConvertValue<T>(this[key]);
            }

            return rtn;
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class EasyDictionary<TValue> : StringKeyDictionaryObject<TValue>
    {
        #region constructors

        public EasyDictionary() { }

        public EasyDictionary(IDictionary dictionary)
            : base(dictionary)
        {
        }

        #endregion
    }

    [Serializable]
    [CollectionDataContract]
    public class EasyDictionary<TKey, TValue> : DictionaryObject<TKey, TValue>
    {
        #region constructors

        public EasyDictionary() { }

        public EasyDictionary(IDictionary dictionary)
            : base(dictionary)
        {
        }

        #endregion
    }
}
