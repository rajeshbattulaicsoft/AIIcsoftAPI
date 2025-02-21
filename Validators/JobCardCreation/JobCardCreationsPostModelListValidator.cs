using FluentValidation;
using AIIcsoftAPI.Dto.JobCardCreations;
using AIIcsoftAPI.Dto.PDO;

namespace AIIcsoftAPI.Validators.JobCardCreation
{
    public class JobCardCreationsPostModelListValidator : AbstractValidator<List<JobCardCreationsPostModel>>
    {
        public JobCardCreationsPostModelListValidator()
        {
            RuleForEach(e => e).SetValidator(new JobCardCreationsPostModelValidator());
        }
    }
}
