using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data.Query
{
    /// <summary>
    /// 查询条件组合类型
    /// </summary>
    public enum JuncType
    {
        Or,
        And
    }

    /// <summary>
    /// 查询方式类型分类
    /// </summary>
    public enum ConditionTypeCategory
    {
        Normal,     // 一般查询类型
        Single,     // 单值查询类型
        UnSettled   // 未设定
    }

    /// <summary>
    /// 普通查询方式(由字段和查询运算符以及条件值构成的查询运算)
    /// </summary>
    public enum ConditionType
    {
        // UnSettled = 0,   // 未设定

        // 正值为双值查询枚举
        Equal = 1,
        NotEqual = 2,
        In = 3,
        NotIn = 4,
        Like = 5,
        NotLike = 6,
        GreaterThan = 7,
        GreaterThanEqual = 8,
        LessThan = 9,
        LessThanEqual = 10,
        StartWith = 11,
        EndWith = 12,
        NotStartWith = 13,
        NotEndWith = 14,

        // 为负为单值查询枚举(只有字段和查询运算符构成的查询条件)
        IsNotNull = -1,
        IsNull = -2,
        IsNotEmpty = -3,    // 查询集合时使用
        IsEmpty = -4    // 查询集合时使用
    }
}
