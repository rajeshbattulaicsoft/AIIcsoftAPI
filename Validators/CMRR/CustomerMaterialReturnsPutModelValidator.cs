using FluentValidation;
using AIIcsoftAPI.Dto.CustomerMaterialReturns;

namespace AIIcsoftAPI.Validators.CMRR
{
    public class CustomerMaterialReturnsPutModelValidator : AbstractValidator<CustomerMaterialReturnsPutModel>
    {
        public CustomerMaterialReturnsPutModelValidator()
        {
            RuleFor(x => x.dcid)
               .NotEmpty();

            RuleFor(x => x.custid)
               .NotEmpty();

           
            RuleFor(x => x.pcid)
               .NotEmpty();

           
            RuleFor(x => x.loginid)
               .NotEmpty();

            RuleFor(x => x.entryempid)
               .NotEmpty();

            RuleFor(x => x.entrycomputer)
              .NotEmpty();

            RuleFor(x => x.returndate)
             .NotEmpty();

            RuleFor(x => x.companyid)
             .NotEmpty();
        }
    }
}
