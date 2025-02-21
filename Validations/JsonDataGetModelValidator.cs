using AIIcsoftAPI.Models.RequestModels;
using FluentValidation;

namespace AIIcsoftAPI.Validations
{
    public class JsonDataGetModelValidator : AbstractValidator<JsonDataGetModel>
    {
        public JsonDataGetModelValidator()
        {
            RuleFor(x => x.DataType).NotEmpty().WithMessage("Please enter the DataType");
        }
    }
}
