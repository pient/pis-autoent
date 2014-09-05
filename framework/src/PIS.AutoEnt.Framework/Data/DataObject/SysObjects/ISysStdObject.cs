using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    /// <summary>
    /// 标准对象，包含Code, Name和Tag
    /// </summary>
    public interface ISysStdObject : ISysMetaObject
    {
        string Code { get; set; }
        string Name { get; set; }
        string Tag { get; set; }
    }
}
