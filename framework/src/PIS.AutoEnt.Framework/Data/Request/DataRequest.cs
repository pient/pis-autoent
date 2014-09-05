using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PIS.AutoEnt.Data
{
    public class DataRequest
    {
        public string QryRequest { get; set; }

        public string Params { get; set; }

        public DataRequest()
        {
        }

        public QueryRequest GetQueryRequest()
        {
            var qryReq = new QueryRequest();

            if (!String.IsNullOrEmpty(QryRequest))
            {
                qryReq = JsonConvert.DeserializeObject<QueryRequest>(QryRequest);
            }

            return qryReq;
        }

        public T GetParams<T>()
            where T : new()
        {
            var p = new T();

            if (String.IsNullOrEmpty(Params))
            {
                p = JsonConvert.DeserializeObject<T>(Params);
            }

            return p;
        }
    }
}
