using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    public class QueryResult
    {
        public int TotalCount { get; set; }

        public object Data { get; set; }

        public QueryResult()
        {
        }
    }
}
