using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data.Query
{
    public class SqlQueryBuilder : IQueryBuilder
    {
        /// <summary>
        /// 获取SQL查询字符串
        /// </summary>
        /// <param name="qryexpr"></param>
        /// <returns></returns>
        public string GetQueryString(QueryExpr qryexpr)
        {
            string cdtstr = this.GetConditionString(qryexpr.Conditions);

            string qrystr = GetQueryString(qryexpr.Target, cdtstr, qryexpr.QueryFields);

            if (qryexpr.Sorters.Count == 0)
            {
                qryexpr.Sorters.Add(new QryOrder("Id", false));  // 默认按id倒序排列
            }

            string orderstr = this.GetOrderString(qryexpr.Sorters);

            qrystr = this.GetPagingSQLString(qrystr, qryexpr.Start, qryexpr.Limit, orderstr);

            return qrystr;
        }

        public string GetRecordCountSQLString(QueryExpr qryexpr)
        {
            string cdtstr = this.GetConditionString(qryexpr.Conditions);

            var qrystr = this.GetRecordCountSQLString(qryexpr.Target, cdtstr);

            return qrystr;
        }

        /// <summary>
        /// 获取组合查询字符串
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetConditionString(JuncCondition condition)
        {
            StringBuilder sql = new StringBuilder();
            string exp = condition.Type.ToString();

            foreach (Condition t_cdt in condition.Conditions)
            {
                if (t_cdt.TypeCategory == ConditionTypeCategory.Normal && t_cdt.Value != null && !String.IsNullOrEmpty(t_cdt.Value.ToString())
                    || t_cdt.TypeCategory == ConditionTypeCategory.Single)
                {
                    var t_cdt_str = GetConditionString(t_cdt);

                    if (!String.IsNullOrEmpty(t_cdt_str))
                    {
                        sql.Append(t_cdt_str + exp);
                    }
                }
            }

            foreach (JuncCondition t_jcdt in condition.JuncConditions)
            {
                string t_sqlstr = GetConditionString(t_jcdt);

                if (!String.IsNullOrEmpty(t_sqlstr))
                {
                    sql.Append(t_sqlstr + exp);
                }
            }

            string sqlstr = sql.ToString().TrimEnd(exp);

            if (!String.IsNullOrEmpty(sqlstr))
            {
                sqlstr = " (" + sqlstr + ") ";
            }

            return sqlstr;
        }

        /// <summary>
        /// 获取Condition值
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetConditionString(Condition condition)
        {
            string sql = String.Empty;

            if (condition.Field == null || String.IsNullOrEmpty(condition.Field.Name))
            {
                return sql;
            }

            if (condition.TypeCategory == ConditionTypeCategory.Single)
            {
                string exp = String.Empty;

                switch (condition.Type)
                {
                    case ConditionType.IsNull:
                        exp = " IS NULL ";
                        break;
                    case ConditionType.IsNotNull:
                        exp = " IS NOT NULL ";
                        break;
                    case ConditionType.IsEmpty:
                    case ConditionType.IsNotEmpty:
                        throw new NotImplementedException(" IsEmpty 和 IsNotEmpty方法未实现查询 ");
                }

                sql = String.Format(" {0} {1} ", condition.Field.Name, exp);
            }
            else if (condition.TypeCategory == ConditionTypeCategory.Normal)
            {
                string exp = String.Empty;
                object val = null;

                TypeCode valtcode = condition.Field.TypeCode;

                switch (condition.Type)
                {
                    case ConditionType.Equal:
                        exp = " = ";
                        break;
                    case ConditionType.NotEqual:
                        exp = " <> ";
                        break;
                    case ConditionType.GreaterThan:
                        exp = " > ";
                        break;
                    case ConditionType.GreaterThanEqual:
                        exp = " >= ";
                        break;
                    case ConditionType.LessThan:
                        exp = " < ";
                        break;
                    case ConditionType.LessThanEqual:
                        exp = " <= ";
                        break;
                    case ConditionType.In:
                        exp = " IN ";
                        break;
                    case ConditionType.NotIn:
                        exp = " NOT IN ";
                        break;
                    case ConditionType.StartWith:
                    case ConditionType.EndWith:
                    case ConditionType.Like:
                        exp = " LIKE ";
                        break;
                    case ConditionType.NotStartWith:
                    case ConditionType.NotEndWith:
                    case ConditionType.NotLike:
                        exp = " NOT LIKE ";
                        val = String.Format("'%{0}%'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                        break;
                }

                switch (condition.Type)
                {
                    case ConditionType.Equal:
                    case ConditionType.NotEqual:
                    case ConditionType.GreaterThan:
                    case ConditionType.GreaterThanEqual:
                    case ConditionType.LessThan:
                    case ConditionType.LessThanEqual:
                        switch (valtcode)
                        {
                            case TypeCode.Char:
                            case TypeCode.DateTime:
                            case TypeCode.String:
                            case TypeCode.Object:
                                val = String.Format("'{0}'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                                break;
                        }
                        break;
                    case ConditionType.StartWith:
                        val = String.Format("'{0}%'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                        break;
                    case ConditionType.EndWith:
                        val = String.Format("'%{0}'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                        break;
                    case ConditionType.Like:
                        val = String.Format("'%{0}%'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                        break;
                    case ConditionType.NotStartWith:
                        val = String.Format("'{0}%'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                        break;
                    case ConditionType.NotEndWith:
                        val = String.Format("'%{0}'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                        break;
                    case ConditionType.NotLike:
                        val = String.Format("'%{0}%'", StringHelper.ToNullFilteredString(condition.Value, string.Empty));
                        break;
                    case ConditionType.In:
                    case ConditionType.NotIn:
                        throw new NotImplementedException(" In 和 NotIn方法未实现查询 ");
                }

                sql = String.Format(" {0} {1} {2} ", condition.Field.Name, exp, val.ToString());
            }

            if (!String.IsNullOrEmpty(sql))
            {
                sql = " (" + sql + ") ";
            }

            return sql;
        }

        /// <summary>
        /// 获取行数查询字符串
        /// </summary>
        /// <param name="qryObj"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public string GetRecordCountSQLString(QueryTarget qryObj, string condition)
        {
            string qrystr = String.Empty;   // 获取查询字符串

            string objstr = qryObj.String;
            string cdtstr = condition;

            if (!String.IsNullOrWhiteSpace(condition))
            {
                cdtstr = " WHERE " + condition;
            }

            switch (qryObj.Type)
            {
                case QueryTargetType.Table:
                case QueryTargetType.View:
                case QueryTargetType.TableOrView:
                case QueryTargetType.StoreProcedure:
                    qrystr = "SELECT COUNT(*) FROM " + objstr + " " + qryObj.Alias + " " + cdtstr;
                    break;
                case QueryTargetType.QueryString:
                    qrystr = "SELEC COUNT(*) FROM (" + objstr + ") " + qryObj.Alias + " " + cdtstr;
                    break;
            }

            return qrystr;
        }

        /// <summary>
        /// 获取SQL查询字符串
        /// </summary>
        /// <param name="qryObj"></param>
        /// <param name="cdtStr"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public string GetQueryString(QueryTarget qryObj, string condition, QueryFieldCollection fields = null)
        {
            string qrystr = String.Empty;   // 获取查询字符串

            IList<string> fstrs = fields.Select(f => f.Name).ToList();

            string objstr = qryObj.String;
            string cdtstr = condition;

            if (!String.IsNullOrWhiteSpace(condition))
            {
                cdtstr = " WHERE " + condition;
            }

            string fldstr = "*";

            if (fields != null && fields.Count > 0)
            {
                fldstr = fstrs.Join();
            }

            switch (qryObj.Type)
            {
                case QueryTargetType.Table:
                case QueryTargetType.View:
                case QueryTargetType.TableOrView:
                case QueryTargetType.StoreProcedure:
                    qrystr = "SELECT " + fldstr + " FROM " + objstr + " " + qryObj.Alias + " " + cdtstr;
                    break;
                case QueryTargetType.QueryString:
                    qrystr = "SELECT " + fldstr + " FROM (" + objstr + ") " + qryObj.Alias + " " + cdtstr;
                    break;
            }

            return qrystr;
        }

        /// <summary>
        /// 获取分页的SQL语句(目前只支持SQLServer)
        /// </summary>
        /// <param name="qryStr">原来sql语句（必须是完整的，可执行的sql语句）</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderstr">排序字符串(必须单独提供)</param>
        /// <returns></returns>
        public string GetPagingSQLString(string qryStr, int start, int limit, string orderstr)
        {
            string pgsql = String.Format("SELECT TOP {0} * FROM  ( SELECT *, ROW_NUMBER() OVER (ORDER BY {1}) AS " + SysConsts.DefaultNumberRowName + " FROM ({2}) AS iq ) as q WHERE q." + SysConsts.DefaultNumberRowName + " > {3} ORDER BY q." + SysConsts.DefaultNumberRowName,
                limit, orderstr, qryStr, start);

            return pgsql;
        }

        /// <summary>
        /// 获取排序字符串
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public string GetOrderString(QryOrderCollection orders)
        {
            string orderstr = String.Empty;

            foreach (QryOrder item in orders)
            {
                if (!String.IsNullOrEmpty(item.FieldName))
                {
                    orderstr += item.FieldName;

                    if (item.Ascending)
                    {
                        orderstr += " ASC";
                    }
                    else
                    {
                        orderstr += " DESC";
                    }

                    orderstr += ",";
                }
            }

            orderstr = orderstr.TrimEnd(',');

            return orderstr;
        }
    }
}
