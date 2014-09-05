using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.App
{
    public interface IRegDataObject
    {
        string RegDataType { get; set; }
        string Editable { get; set; }
        string EditPage { get; set; }
        string Description { get; set; }
    }
}
