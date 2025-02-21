using FluentValidation;
using AIIcsoftAPI.Dto.JobCardCreations;

namespace AIIcsoftAPI.Validators.JobCardCreation
{

    public class JobCardCreationsPostModelValidator : AbstractValidator<JobCardCreationsPostModel>
    {
        public JobCardCreationsPostModelValidator()
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
