using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PIS.AutoEnt.Web
{
    public class PageState : Hashtable
    {
        #region 属性
        


        #endregion

        #region 构造函数

        public PageState()
        {

        }

        #endregion

        #region 公共方法

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion
    }
}
