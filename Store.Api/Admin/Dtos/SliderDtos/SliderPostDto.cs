using FluentValidation;

namespace Store.Api.Admin.Dtos.SliderDtos
{
    public class SliderPostDto
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IFormFile ImageFile { get; set; }
        public string RedirectUrl { get; set; }

    }
    public class SliderPostDtosValidator : AbstractValidator<SliderPostDto>
    {
        public SliderPostDtosValidator()
        {
            RuleFor(x => x.Order).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.RedirectUrl).NotEmpty().MaximumLength(100);
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.ImageFile == null)
                {
                    context.AddFailure("ImageFile", "ImageFile is required");
                }
                else if (x.ImageFile.ContentType != "image/png" && x.ImageFile.ContentType != "image/jpeg")
                {
                    context.AddFailure("ImageFile", "File type must be png, jpg or jpeg");
                }
                else if (x.ImageFile.Length > 2097152)
                {
                    context.AddFailure("ImageFile", "File size must be less or equal than 2MB");
                }
            });
        }
    }
}
