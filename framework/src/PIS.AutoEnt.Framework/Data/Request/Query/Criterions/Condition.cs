using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data.Query
{
    public class Condition
    {
        #region 公共属性

        /// <summary>
        /// 字段信息
        /// </summary>
        public QueryField Field { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public ConditionType Type { get; set; }

        /// <summary>
        /// 是查询类型的分类
        /// </summary>
        public ConditionTypeCategory TypeCategory
        {
            get
            {
                int condInt = (int)this.Type;

                if (condInt > 0)
                {
                    return ConditionTypeCategory.Normal;
                }
                else if (condInt < 0)
                {
                    return ConditionTypeCategory.Single;
                }
                else
                {
                    return ConditionTypeCategory.UnSettled;
                }
            }
        }

        #endregion

        #region 构造函数

        public Condition()
        {
        }

        public Condition(string fieldName, ConditionType type = ConditionType.IsNotNull, TypeCode typeCode = TypeCode.String)
        {
            this.Field = new QueryField(fieldName, typeCode);
            this.Type = type;
        }

        public Condition(string fieldName, object value, ConditionType type = ConditionType.Equal, TypeCode typeCode = TypeCode.String)
        {
            this.Field = new QueryField(fieldName, typeCode);
            this.Value = value;
            this.Type = type;
        }

        public Condition(Condition condition)
            : this(condition.Field.Name, condition.Value, condition.Type, condition.Field.TypeCode)
        {
        }

        #endregion
    }
}
