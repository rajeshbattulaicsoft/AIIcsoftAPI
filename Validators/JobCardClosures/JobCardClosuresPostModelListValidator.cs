using FluentValidation;
using AIIcsoftAPI.Dto.JobCardClosures;

namespace AIIcsoftAPI.Validators.JobCardClosures
{
    public class JobCardClosuresPostModelListValidator : AbstractValidator<List<JobCardClosuresPostModel>>
    {
        public JobCardClosuresPostModelListValidator()
        {
            RuleForEach(e => e).SetValidator(new JobCardClosuresPostModelValidator());
        }
    }
}
