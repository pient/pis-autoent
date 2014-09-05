using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Caching
{
    /// <summary>
    /// Cache 供应者，提供Cache项的存储于获取功能
    /// </summary>
    public interface ICacheProvider
    {
        bool Add(string key, object obj, object config = null);

        object Retrieve(string key);

        object Remove(string key);
    }
}
