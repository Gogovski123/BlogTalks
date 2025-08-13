using FluentValidation;

namespace BlogTalks.Application.Comments.Commands
{
    public class CreateValidator : AbstractValidator<CreateRequest>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Comment text cannot be empty.")
                .MaximumLength(500).WithMessage("Comment text cannot exceed 500 characters.");
            
        }
    }
}
