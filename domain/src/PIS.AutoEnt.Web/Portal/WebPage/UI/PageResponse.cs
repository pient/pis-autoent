using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PIS.AutoEnt.Web
{
    public class PageResponse
    {
        #region 成员属性

        private HttpResponse response;    // 页面Http响应

        #endregion

        #region 构造函数

        public PageResponse(HttpResponse response)
        {
            this.response = response;

            Initalize();
        }

        protected void Initalize()
        {

        }

        #endregion
    }
}
