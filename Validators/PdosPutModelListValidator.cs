using FluentValidation;
using AIIcsoftAPI.Dto.PDO;

namespace AIIcsoftAPI.Validators
{
    public class PdosPutModelListValidator : AbstractValidator<List<PdosPutModel>>
    {
        public PdosPutModelListValidator()
        {
            RuleForEach(e => e).SetValidator(new PdosPutModelValidator());
        }
    }
}
