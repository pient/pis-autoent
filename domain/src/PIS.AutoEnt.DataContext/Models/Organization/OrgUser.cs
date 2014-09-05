using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.DataContext
{
    [MetadataType(typeof(OrgUserValidation))]
    public partial class OrgUser : ISysStdObject
    {
        #region Enums & Classes

        public class UserStatus
        {
            public const string Enabled = "Enabled";
            public const string Disabled = "Disabled";
        }

        public class UserTag
        {
            public string Password { get; set; }
        }

        #endregion
    }

    public class OrgUserValidation
    {
        [Unique]
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [RegularExpression(RegularExprs.EMail)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
