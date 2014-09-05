using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data.Query
{
    public class JuncCondition
    {
        #region 公共属性

        /// <summary>
        /// 查询条件组合类型
        /// </summary>
        public JuncType Type
        {
            get;
            protected set;
        }

        /// <summary>
        /// 条件集合
        /// </summary>
        public IList<Condition> Conditions
        {
            get;
            protected set;
        }

        /// <summary>
        /// 组合条件集合
        /// </summary>
        public IList<JuncCondition> JuncConditions
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        public JuncCondition()
        {
            this.Conditions = new List<Condition>();
            this.JuncConditions = new List<JuncCondition>();
        }

        public JuncCondition(JuncType juncType)
            : this()
        {
            this.Type = juncType;
        }

        public JuncCondition(JuncCondition juncCodition)
            : this(juncCodition.Type)
        {
            foreach (Condition t_cdt in juncCodition.Conditions)
            {
                this.Conditions.Add(t_cdt);
            }

            foreach (JuncCondition t_cdt in juncCodition.JuncConditions)
            {
                this.JuncConditions.Add(t_cdt);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty()
        {
            return this.Conditions.Count == 0 && this.JuncConditions.Count == 0;
        }

        /// <summary>
        /// 清空查询
        /// </summary>
        public virtual void Clear()
        {
            this.Conditions.Clear();
        }

        /// <summary>
        /// 添加多个查询条件（默认IsNotNull）
        /// </summary>
        /// <param name="fieldNames"></param>
        /// <param name="searchMode"></param>
        public virtual void Add(string[] fieldNames, ConditionType conditionType = ConditionType.IsNotNull)
        {
            fieldNames.All((f) =>
            {
                this.Add(f, conditionType);
                return true;
            });
        }

        /// <summary>
        /// 添加单个查询条件（默认IsNotNull）
        /// </summary>
        /// <param name="fieldNames"></param>
        /// <param name="searchMode"></param>
        public virtual void Add(string fieldName, ConditionType conditionType = ConditionType.IsNotNull, TypeCode type = TypeCode.String)
        {
            this.Conditions.Add(new Condition(fieldName, conditionType));
        }

        /// <summary>
        /// 添加单个查询条件（默认Equal）
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="conditionType"></param>
        public virtual void Add(string fieldName, object value, ConditionType type = ConditionType.Equal, TypeCode typeCode = TypeCode.String)
        {
            this.Conditions.Add(new Condition(fieldName, value, type, typeCode));
        }

        /// <summary>
        /// 设置多个查询条件（默认IsNotNull）
        /// </summary>
        public virtual void Set(string[] fieldNames, ConditionType conditionType = ConditionType.IsNotNull)
        {
            fieldNames.All((f) =>
            {
                this.Set(f, conditionType);
                return true;
            });
        }

        /// <summary>
        /// 设置查询条件（默认IsNotNull）
        /// </summary>
        public virtual void Set(string fieldName, ConditionType conditionType)
        {
            Condition condition = GetFirst(fieldName);

            if (condition != null)
            {
                condition.Type = conditionType;
            }
            else
            {
                Add(fieldName, conditionType);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void Set(string fieldName, object value)
        {
            Condition condition = GetFirst(fieldName);

            if (condition != null)
            {
                condition.Value = value;
            }
            else
            {
                Add(fieldName, value);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void Set(string fieldName, object value, ConditionType type)
        {
            Condition condition = GetFirst(fieldName);

            if (condition != null)
            {
                condition.Value = value;
                condition.Type = type;
            }
            else
            {
                Add(fieldName, value, type);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        public virtual void Set(string fieldName, object value, ConditionType type, TypeCode typeCode)
        {
            Condition condition = GetFirst(fieldName);

            if (condition != null)
            {
                condition.Value = value;
                condition.Field = new QueryField(fieldName, typeCode);
            }
            else
            {
                Add(fieldName, value, type, typeCode);
            }
        }

        /// <summary>
        /// 删除查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void Remove(string fieldName)
        {
            this.Conditions.ToList().RemoveAll(tent => (tent.Field != null && tent.Field.Name == fieldName));
        }

        /// <summary>
        /// 删除查询条件
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void Remove(string[] fieldNames)
        {
            this.Conditions.ToList().RemoveAll(tent => (tent.Field != null && fieldNames.Contains(tent.Field.Name)));
        }

        /// <summary>
        /// 获取指定域名的条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public virtual IList<Condition> Get(string fieldName)
        {
            IList<Condition> conditions = this.Conditions.Where(v =>
                (v.Field != null && v.Field.Name == fieldName 
                && v.Value != null && !String.IsNullOrEmpty(v.Value.ToString()))).ToList();

            return conditions;
        }

        /// <summary>
        /// 获取指定域名的条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public virtual IList<Condition> Get(string fieldName, ConditionType type)
        {
            IList<Condition> conditions = this.Conditions.Where(v =>
                (v.Field != null && v.Field.Name == fieldName && v.Type == type
                && v.Value != null && !String.IsNullOrEmpty(v.Value.ToString()))).ToList();

            return conditions;
        }

        /// <summary>
        /// 获取第一个域名
        /// </summary>
        public virtual Condition GetFirst(string fieldName)
        {
            Condition condition = this.Conditions.Where(v =>
                (v.Field != null && v.Field.Name == fieldName
                && v.Value != null && !String.IsNullOrEmpty(v.Value.ToString()))).FirstOrDefault();

            return condition;
        }

        /// <summary>
        /// 获取第一个域名
        /// </summary>
        public virtual Condition GetFirst(string fieldName, ConditionType type)
        {
            Condition condition = this.Conditions.Where(v =>
                (v.Field != null && v.Field.Name == fieldName && v.Type == type
                && v.Value != null && !String.IsNullOrEmpty(v.Value.ToString()))).FirstOrDefault();

            return condition;
        }

        #endregion
    }
}
