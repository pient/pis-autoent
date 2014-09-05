using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data.Query;

namespace PIS.AutoEnt.Data
{
    public class QueryRequest
    {
        public int Start { get; set; }
        public int Limit { get; set; }

        public bool GetTotalCount { get; set; }
        public JuncCondition Conditions { get; set; }
        public QryOrderCollection Sorters { get; set; }

        public QueryRequest()
        {
            GetTotalCount = true;

            Conditions = new JuncCondition();
            Sorters = new QryOrderCollection();
        }
    }
}
