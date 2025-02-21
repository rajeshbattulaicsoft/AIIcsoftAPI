using FluentValidation;
using AIIcsoftAPI.Dto.CustomerMaterialReturns;

namespace AIIcsoftAPI.Validators.CMRR
{
    public class CustomerMaterialReturnsPutModelListValidator : AbstractValidator<List<CustomerMaterialReturnsPutModel>>
    {
        public CustomerMaterialReturnsPutModelListValidator()
        {
            RuleForEach(e => e).SetValidator(new CustomerMaterialReturnsPutModelValidator());
        }
    }
}
