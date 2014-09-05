using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// base class of all PIS context objects
    /// </summary>
    [Serializable]
    public abstract class ContextBase : StringKeyDictionaryObject<object>
    {
    }
}
