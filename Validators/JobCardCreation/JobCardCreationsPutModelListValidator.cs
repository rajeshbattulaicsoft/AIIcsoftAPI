using FluentValidation;
using AIIcsoftAPI.Dto.JobCardCreations;

namespace AIIcsoftAPI.Validators.JobCardCreation
{
    public class JobCardCreationsPutModelListValidator : AbstractValidator<List<JobCardCreationsPutModel>>
    {
        public JobCardCreationsPutModelListValidator()
        {
            RuleForEach(e => e).SetValidator(new JobCardCreationsPutModelValidator());
        }
    }
}
