using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIS.AutoEnt.Web
{
    public class PageData
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }

        public EasyDictionary Tag { get; set; }

        public PageData()
        {
            Tag = new EasyDictionary();
        }
    }
}
