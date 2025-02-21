using FluentValidation;
using AIIcsoftAPI.Dto.CustomerMaterialReturns;
using AIIcsoftAPI.Dto.PDO;

namespace AIIcsoftAPI.Validators.CMRR
{
    public class CustomerMaterialReturnsPostModelListValidator : AbstractValidator<List<CustomerMaterialReturnsPostModel>>
    {
        public CustomerMaterialReturnsPostModelListValidator()
        {
            RuleForEach(e => e).SetValidator(new CustomerMaterialReturnsPostModelValidator());
        }
    }
}
