using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Web;

namespace PIS.AutoEnt.Web
{
    public class PageBase : System.Web.UI.Page
    {
        #region 变量 

        private PageStateContainer stateContainer;

        #endregion

        #region 属性

        /// <summary>
        /// 是否检查登录信息
        /// </summary>
        public bool IsCheckLogon
        {
            get;
            protected set;
        }

        /// <summary>
        /// 是否检查权限信息
        /// </summary>
        public bool IsCheckAuth
        {
            get;
            protected set;
        }

        /// <summary>
        /// PIS 页面请求
        /// </summary>
        public PageRequest PageRequest
        {
            get;
            private set;
        }

        /// <summary>
        /// PIS 页面响应
        /// </summary>
        public PageResponse PageResponse
        {
            get;
            private set;
        }

        /// <summary>
        /// 页面状态
        /// </summary>
        public PageState PageState
        {
            get;
            protected set;
        }

        /// <summary>
        /// 查询规则
        /// </summary>
        public QueryExpr QueryExpr
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        public PageBase()
        {
            this.IsCheckLogon = true;
            this.IsCheckAuth = false;
        }

        #endregion

        #region ASP.NET 事件

        /// <summary>
        /// 初始化之前触发
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        /// <summary>
        /// 初始化方法（先于Page_Load和OnLoad执行）
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.EnableViewState = false;   // 禁用ViewState, PIS系统很少使用服务器端控件

            this.PageRequest = new PageRequest(this.Request);   // 准备请求
            this.PageResponse = new PageResponse(this.Response); // 准备响应

            // 设置页面状态
            PageState pgstate = PageRequest.GetPageState();
            if (pgstate != null)
            {
                this.PageState = pgstate;
            }

            // 设置页面状态
            QueryExpr qryexpr = PageRequest.GetQueryExpr();
            if (qryexpr != null)
            {
                this.QueryExpr = qryexpr;
            }

            if (!PageRequest.IsAsync)
            {
                if (this.stateContainer == null && Form != null)
                {
                    Form.Attributes.Add("pisctrl", "form"); // 初始化pisctrl时用
                    Form.Attributes.Add("dsname", PageRequest.REQUEST_FORM_DATA_KEY);

                    this.stateContainer = new PageStateContainer();
                    this.Form.Controls.Add(stateContainer);
                }
            }
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            /*--写事件日志开始--*/
            if (ex is PISInfoException)
            {
                PageState.Add(WebExceptionMessage.DefaultMessageLabel, new WebExceptionMessage(ex));
            }
            else
            {
                // LogService.Log(String.Format("Message:{0}\r\n\r\nStackTrace:{1}", ex.Message, ex.StackTrace), LogTypeEnum.Error);

                /*--写事件日志结束--*/

                if (ex is SecurityException)
                {
                    PageState.Add(WebExceptionMessage.SecurityMessageLabel, new WebExceptionMessage(ex));
                }
                else
                {
                    PageState.Add(WebExceptionMessage.DefaultMessageLabel, new WebExceptionMessage(ex));
                }
            }

            // 这里只对异步的异常做处理，同步的可以自定义处理方式
            if (PageRequest.IsAsync)
            {
                Server.ClearError();

                // Response.Write(this.PackPageState());
                Response.End();
            }
        }

        /// <summary>
        /// 渲染前触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_PreRender(object sender, EventArgs e)
        {
            // 异步请求时，直接输出数据
            if (PageRequest.IsAsync)
            {
                Response.Write(this.PageState.ToJsonString());
                Response.End();
            }
            else if (stateContainer != null)
            {
                stateContainer.Value = this.PageState.ToJsonString();
            }
        }

        #endregion

    }
}
