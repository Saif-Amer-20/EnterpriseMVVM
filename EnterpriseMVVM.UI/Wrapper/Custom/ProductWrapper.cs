using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseMVVM.UI.Wrapper
{
    public partial class ProductWrapper
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Please enter product name", new[] { nameof(Name) });
            }
            if (CategoryId == 0)
            {
                yield return new ValidationResult("Please select a category", new[] { nameof(CategoryId) });
            }
        }
    }
}
