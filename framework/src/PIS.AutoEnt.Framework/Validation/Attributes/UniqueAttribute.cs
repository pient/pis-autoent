using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueAttribute : ValidationAttribute
    {
        #region Properties

        public FieldMeta FieldMeta { get; private set; }

        #endregion

        #region Constructors

        public UniqueAttribute()
        {
        }

        public UniqueAttribute(FieldMeta fieldMeta)
        {
            FieldMeta = fieldMeta;
        }

        #endregion

        #region ValidationAttribute Members

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IEntityObject modelObj = value as IEntityObject;

            return null;
        }

        #endregion
    }
}
