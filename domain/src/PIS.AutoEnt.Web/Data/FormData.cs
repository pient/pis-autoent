using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIS.AutoEnt.Web
{
    public class FormData
    {
        public FormData()
        {
        }
    }

    public class StdFormData
    {
        public Guid? Id { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public String Tag { get; set; }
    }

    public class StdStructedFormData : StdFormData
    {
        public Guid? ParentId { get; set; }
        public int? SortIndex { get; set; }
        public bool? leaf { get; set; }
        public bool? expanded { get; set; }
    }
}
