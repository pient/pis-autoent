using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public class ValidationResults : EasyCollection<ValidationResult>
    {
        #region Constructors

        public ValidationResults() { }

        public ValidationResults(IEnumerable<ValidationResult> results)
            : base(results)
        {
        }

        #endregion

        #region Public Methods

        public bool IsValid()
        {
            return this.innerItems.Count == 0;
        }

        #endregion
    }
}
