using System;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// base class of data transfer object
    /// </summary>
    [Serializable]
    public abstract class DtoBase : StringKeyDictionaryObject<object>
    {
    }
}
