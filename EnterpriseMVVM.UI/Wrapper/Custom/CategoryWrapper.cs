using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseMVVM.UI.Wrapper
{
    public partial class CategoryWrapper
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Please enter category name", new[] { nameof(Name) });
            }
        }
    }
}
