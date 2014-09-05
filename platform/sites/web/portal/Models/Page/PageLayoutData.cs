using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Web;

namespace PIS.AutoEnt.Portal.Models.Page
{
    public class PageLayoutData : PageData
    {
        #region Properties

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Layout { get; set; }

        #endregion

        #region Constructors

        public PageLayoutData()
        {
        }

        #endregion
    }
}
