using FluentValidation;
using AIIcsoftAPI.Dto.JobCardClosures;

namespace AIIcsoftAPI.Validators.JobCardClosures
{
    public class JobCardClosuresPostModelValidator : AbstractValidator<JobCardClosuresPostModel>
    {
        public JobCardClosuresPostModelValidator()
        {
            RuleFor(x => x.companyid)
                .NotEmpty();

            RuleFor(x => x.warehouseid)
               .NotEmpty();

            RuleFor(x => x.pcid)
               .NotEmpty();

            RuleFor(x => x.refdate)
               .NotEmpty();

            RuleFor(x => x.deptid)
               .NotEmpty();
        }
    }
}
