using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PIS.AutoEnt.Web
{
    /// <summary>
    /// 页面请求
    /// </summary>
    public class PageRequest
    {
        #region 常量

        public const string PAGE_STATE_KEY = "PageState"; // 页面状态键
        public const string SEARCH_CRIT_STATE_KEY = "SearchCriterion";  // 页面查询状态键
        public const string REQUEST_FORM_DATA_KEY = "frmdata";  // 表单提交数据键
        public const string REQUEST_DATA_KEY = "reqdata";       // 一般请求数据键
        public const string REQUEST_ACTION_KEY = "reqaction";   // 请求的活动类型键
        public const string REQUEST_ASYNC_KEY = "AsyncReq";     // 标识是否一部请求键

        #endregion

        #region 成员属性

        private HttpRequest request;    // 页面Http请求

        /// <summary>
        /// 是否异步请求
        /// </summary>
        public bool IsAsync
        {
            get;
            private set;
        }

        /// <summary>
        /// 请求活动 （通常有create, read, update, delete, query, execute等等）
        /// </summary>
        public string ActionString
        {
            get;
            private set;
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        public EasyDictionary RequestData
        {
            get;
            private set;
        }

        /// <summary>
        /// 表单数据
        /// </summary>
        public EasyDictionary FormData
        {
            get;
            private set;
        }

        #endregion

        #region 构造函数

        public PageRequest(HttpRequest request)
        {
            this.request = request;
            this.RequestData = new EasyDictionary();

            Initalize();
        }

        protected void Initalize()
        {
            // 设置是否异步请求， 所有PIS系统异步请求都应带上此标识
            this.IsAsync = request[REQUEST_ASYNC_KEY].ToBoolean(false);

            // 设置系统标准活动
            this.ActionString = request[REQUEST_ACTION_KEY];

            // 打包请求数据
            string reqdata_str = request[REQUEST_DATA_KEY];
            if (!String.IsNullOrEmpty(reqdata_str))
            {
                RequestData = JsonConvert.DeserializeObject<EasyDictionary>(reqdata_str);
            }

            NameValueCollection nvc = request.QueryString;
            if (nvc != null && nvc.Count > 0)
            {
                string keyUpper = String.Empty;
                foreach (string key in nvc.Keys)
                {
                    if (key != REQUEST_FORM_DATA_KEY
                        && key != PAGE_STATE_KEY 
                        && key != SEARCH_CRIT_STATE_KEY 
                        && key != REQUEST_ACTION_KEY 
                        && key != REQUEST_DATA_KEY)
                    {
                        if (!String.IsNullOrEmpty(key) 
                            && !RequestData.ContainsKey(key))
                        {
                            RequestData.Set(key, nvc[key]);
                        }
                    }
                }
            }

            // 设置表单数据
            string formdata_str = request[REQUEST_FORM_DATA_KEY];
            if (!String.IsNullOrEmpty(formdata_str))
            {
                formdata_str = HttpUtility.UrlDecode(formdata_str);
                FormData = JsonConvert.DeserializeObject<EasyDictionary>(formdata_str);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取请求字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(string key)
        {
            return RequestData.Get(key);
        }

        public virtual object Get(string key, object defValue)
        {
            return RequestData.Get(key, defValue);
        }

        /// <summary>
        /// 获取请求字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            return RequestData.Get<T>(key);
        }

        public virtual T Get<T>(string key, T defValue)
        {
            return RequestData.Get<T>(key, defValue);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IList<T> GetList<T>(string key)
        {
            IList<T> rtn = null;

            JArray vals = null;
            if (RequestData[key] != null)
            {
                vals = RequestData[key] as JArray;
            }

            if (vals != null)
            {
                IEnumerable<T> ids = vals.Values<T>();
                rtn = ids.ToList();
            }

            return rtn;
        }

        /// <summary>
        /// 获取查询规则
        /// </summary>
        /// <returns></returns>
        public QueryExpr GetQueryExpr()
        {
            string schcrit_str = request[SEARCH_CRIT_STATE_KEY];
            QueryExpr schcrit = null;

            if (!String.IsNullOrEmpty(schcrit_str))
            {
                schcrit = JsonConvert.DeserializeObject<QueryExpr>(schcrit_str);
            }

            return schcrit;
        }

        /// <summary>
        /// 获取页面状态
        /// </summary>
        public PageState GetPageState()
        {
            string state_str = request[PAGE_STATE_KEY];
            PageState state = null;

            if (!String.IsNullOrEmpty(state_str))
            {
                state = JsonConvert.DeserializeObject<PageState>(state_str);
            }

            return state;
        }

        #endregion
    }
}
