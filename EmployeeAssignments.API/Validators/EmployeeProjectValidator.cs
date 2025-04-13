using EmployeeAssignments.API.Dtos;
using FluentValidation;

namespace EmployeeAssignments.API.Validators
{

    public class EmployeeProjectValidator : AbstractValidator<EmployeeProjectsDto>
    {
        public EmployeeProjectValidator()
        {
            RuleFor(x => x.EmpID).GreaterThan(0);
            RuleFor(x => x.ProjectID).GreaterThan(0);
            RuleFor(x => x.DateFrom)
                .NotEmpty().WithMessage("DateFrom is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("DateFrom must be in the past");
            RuleFor(x => x)
                .Must(x => !x.DateTo.HasValue || x.DateTo > x.DateFrom)
                .WithMessage("DateTo must be after DateFrom if provided");
        }
    }

}
