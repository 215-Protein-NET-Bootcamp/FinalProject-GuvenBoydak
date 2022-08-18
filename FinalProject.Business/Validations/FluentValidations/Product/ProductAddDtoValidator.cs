﻿using FinalProject.DTO;
using FluentValidation;
using System.Text.RegularExpressions;

namespace FinalProject.Business
{
    public class ProductAddDtoValidator : AbstractValidator<ProductAddDto>
    {
        public ProductAddDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün adı boş geçilemez.").MaximumLength(100).WithMessage("Ürün Adı en fazla 100 karakter olmalıdır.").Matches("^[a-zA-ZiİçÇşŞğĞÜüÖö]*$").WithMessage("Sadece Harf Giriniz.");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage("Ürün Fiyati Boş geçilemez").GreaterThan(0).WithMessage("Girilen Fiyat sıfırdan büyük olamlı");
            RuleFor(x => x.UnitsInStock).NotEmpty().WithMessage("Ürün Stok alanı boş geçilemez.").GreaterThan(0).WithMessage("Girilen deger sıfırdan büyük olamlıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Ürün Açıklaması alanı boş geçilemez.").MaximumLength(500).WithMessage("Girilen deger en Fazla 500 karakter olmalıdır.").Matches("^[a-zA-ZiİçÇşŞğĞÜüÖö]*$").WithMessage("Sadece Harf Giriniz.");
            RuleFor(x => x.UsageStatus).NotEmpty().WithMessage("Kullanım Durumu boş Geçilemez");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Kategorisi Boş Geçilemez").GreaterThan(0).WithMessage("Kategori Id alını sıfırdan büyük olmalıdır.");
        }

    }
}
