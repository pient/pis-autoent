using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.XData
{
    /// <summary>
    /// 用于唯一确定XDataItem对象，一般由主键与XPath唯一定义一条记录
    /// </summary>
    [Serializable]
    public class XDataItemMeta
    {
        public object Id { get; set; }

        public string XPath { get; set; }
    }
}
