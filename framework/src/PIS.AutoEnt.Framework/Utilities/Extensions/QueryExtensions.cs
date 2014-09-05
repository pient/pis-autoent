using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt
{
    public static class QueryExtensions
    {
        public static QueryExpr GetExpression(this QueryRequest request, QueryTarget queryTarget)
        {
            if (request == null)
            {
                request = new QueryRequest();
            }

            var expr = new QueryExpr()
            {
                Target = queryTarget,
                GetTotalCount = request.GetTotalCount,
                Start = request.Start,
                Limit = request.Limit,
                Conditions = request.Conditions,
                Sorters = request.Sorters,
            };

            return expr;
        }

        public static QueryExpr GetExpression(this QueryRequest request, string targetName, QueryTargetType targetType = QueryTargetType.TableOrView)
        {
            var target = new QueryTarget(targetName, targetType);

            var expr = GetExpression(request, target);

            return expr;
        }
    }
}
