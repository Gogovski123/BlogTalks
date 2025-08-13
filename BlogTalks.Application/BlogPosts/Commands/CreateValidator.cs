using FluentValidation;

namespace BlogTalks.Application.BlogPosts.Commands
{
    public class CreateValidator : AbstractValidator<CreateRequest>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100);

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.");

            RuleFor(x => x.Tags)
                .NotEmpty().WithMessage("At least one tag is required.")
                .Must(tags => tags.Count <= 5).WithMessage("A maximum of 5 tags is allowed.")
                .ForEach(tag => tag.NotEmpty().WithMessage("Tag cannot be empty."));
        }
    }
}
