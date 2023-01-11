using FluentValidation;

namespace Store.Api.Admin.Dtos.AccountDtos
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().MinimumLength(6).MaximumLength(18);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(6).MaximumLength(20);

        }
    }
}
