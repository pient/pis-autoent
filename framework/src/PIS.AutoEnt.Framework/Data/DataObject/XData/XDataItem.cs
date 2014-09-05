using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.XData
{
    /// <summary>
    /// XDataItem相当于XData版的实体类根基
    /// </summary>
    [Serializable]
    public class XDataItem
    {
        #region 成员属性

        public const string XDATA_ITEM_ROOT_PATH = "/";

        protected XItem Data { get; set; }

        protected XDataItemMeta Meta { get; set; }

        #endregion

        #region 构造函数

        public XDataItem()
        {

        }

        #endregion

        #region 方法

        public string GetValue(string attrName)
        {
            return this.Data.GetValue(XDATA_ITEM_ROOT_PATH, attrName);
        }

        public T GetValue<T>(string attrName)
        {
            return this.Data.GetValue<T>(XDATA_ITEM_ROOT_PATH, attrName);
        }

        public void SetValue(string attrName, string value)
        {
            this.Data.SetValue(XDATA_ITEM_ROOT_PATH, value);
        }

        #endregion
    }
}
