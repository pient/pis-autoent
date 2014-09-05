using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data.Query
{
    public class OracleQueryBuilder : IQueryBuilder
    {
        public string GetQueryString(QueryExpr qryexpr)
        {
            throw new NotImplementedException();
        }

        public string GetQueryString(QueryTarget qryObj, string condition = "", QueryFieldCollection fields = null)
        {
            throw new NotImplementedException();
        }

        public string GetPagingSQLString(string qryStr, int pageIndex, int pageSize, string orderstr)
        {
            throw new NotImplementedException();
        }

        public string GetOrderString(QryOrderCollection orders)
        {
            throw new NotImplementedException();
        }


        public string GetConditionString(JuncCondition condition)
        {
            throw new NotImplementedException();
        }

        public string GetConditionString(Condition condition)
        {
            throw new NotImplementedException();
        }


        public string GetRecordCountSQLString(QueryExpr qryexpr)
        {
            throw new NotImplementedException();
        }
    }
}
