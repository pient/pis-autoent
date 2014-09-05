using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data.Query
{
    public interface IQueryBuilder
    {
        string GetQueryString(QueryExpr qryexpr);

        string GetRecordCountSQLString(QueryExpr qryexpr);

        string GetQueryString(QueryTarget qryObj, string condition = "", QueryFieldCollection fields = null);

        string GetConditionString(JuncCondition condition);

        string GetConditionString(Condition condition);

        string GetPagingSQLString(string qryStr, int pageIndex, int pageSize, string orderstr);

        string GetOrderString(QryOrderCollection orders);
    }
}
