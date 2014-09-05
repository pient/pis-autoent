using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public static class ModelExtensions
    {
        public static ValidationResults IsValid(this IEntityObject modelObj)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(modelObj, null, null);

            Validator.TryValidateObject(modelObj, validationContext, validationResults, true);

            return new ValidationResults(validationResults);
        }
    }
}
