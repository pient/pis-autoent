using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.XData
{
    public interface IXDataRepository
    {
        string TableName
        {
            get;
        }

        /// <summary>
        /// XData字段名,默认XData
        /// </summary>
        string XFiledName
        {
            get;
        }

        /// <summary>
        /// XPath路径
        /// </summary>
        string XPath
        {
            get;
        }
    }
}
