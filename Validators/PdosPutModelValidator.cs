using FluentValidation;
using AIIcsoftAPI.Dto.PDO;

namespace AIIcsoftAPI.Validators
{
    public class PdosPutModelValidator : AbstractValidator<PdosPutModel>
    {
        public PdosPutModelValidator()
        {
            RuleFor(x => x.grnno)
                .NotEmpty();

            RuleFor(x => x.warehouseid)
               .NotEmpty();

            RuleFor(x => x.pcid)
               .NotEmpty();

            RuleFor(x => x.pdorefdate)
               .NotEmpty();

            RuleFor(x => x.loginid)
               .NotEmpty();

            RuleFor(x => x.entryempid)
               .NotEmpty();

            RuleFor(x => x.entrycomputer)
              .NotEmpty();

            RuleFor(x => x.description)
             .NotEmpty();

            RuleFor(x => x.jobcardid)
             .NotEmpty();
        }
    }
}
