﻿using FinalProject.DTO;
using FluentValidation;

namespace FinalProject.Business
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.ID).NotEmpty().WithMessage("ID alanı boş geçilemez").GreaterThan(0).WithMessage("ID deger 0 dan buyuk olmalıdır.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori İsmi boş geçilemez").MaximumLength(50).WithMessage("Kategori ismi en fazla 50 karakter olamlıdır.").Matches("^[a-zA-ZiİçÇşŞğĞÜüÖö]*$").WithMessage("Sadece Harf Giriniz.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Kategori Açıklaması alanı boş geçilemez.").MaximumLength(500).WithMessage("Girilen deger en Fazla 500 karakter olmalıdır.").Matches("^[a-zA-ZiİçÇşŞğĞÜüÖö]*$").WithMessage("Sadece Harf Giriniz.");
        }
    }
}