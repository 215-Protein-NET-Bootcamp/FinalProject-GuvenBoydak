using FinalProject.DTO;
using FluentValidation;

namespace FinalProject.Business
{
    public class AppUserLoginDtoValidator : AbstractValidator<AppUserLoginDto>
    {
        public AppUserLoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Boş olmamlıdır.").MaximumLength(75).WithMessage("Email Alanı maksimum 75 karakter olamlıdır.").EmailAddress().WithMessage("Girilen deger Email formatinda olmalıdır.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Alanı boş olmamalıdır.").MinimumLength(8).WithMessage("Şifre alanı en az 8 karakter olamlıdır.").MaximumLength(20).WithMessage("Şifre Alanı en fazla 20 karakter olamlıdır.");
        }
    }
}
