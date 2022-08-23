using FinalProject.Business;
using FinalProject.DTO;
using FluentAssertions;
using FluentValidation.Results;

namespace FinalProject.Test
{
    public class OfferValidationTest
    {
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,2)]
        public void WhenInvalidInputsAreGiven_OfferAddDtoValidator_SouldNotBeReturnErrors(decimal price,int productId)
        {
            OfferAddDto offerAddDto = new OfferAddDto()
            {
                Price = price,
                ProductID = productId
            };

            OfferAddDtoValidator validation = new OfferAddDtoValidator();
            ValidationResult result=validation.Validate(offerAddDto);

            result.Errors.Count.Should().Be(0);
        }

        [Theory]
        [InlineData( 0,0)]
        [InlineData( 1, -4)]
        public void WhenInvalidInputsAreGiven_OfferAddDtoValidator_SouldBeReturnErrors( decimal price, int productId)
        {
            OfferAddDto offerAddDto = new OfferAddDto()
            {
                Price = price,
                ProductID = productId
            };

            OfferAddDtoValidator validation = new OfferAddDtoValidator();
            ValidationResult result = validation.Validate(offerAddDto);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(-1, 0, 0)]
        [InlineData(0, 1, -4)]
        public void WhenInvalidInputsAreGiven_OfferUpdateDtoValidator_SouldBeReturnErrors(int id, decimal price, int productId)
        {
            OfferUpdateDto offerUpdateDto = new OfferUpdateDto()
            {
                ID = id,
                Price = price,
                ProductID = productId
            };

            OfferUpdateDtoValidator validation = new OfferUpdateDtoValidator();
            ValidationResult result = validation.Validate(offerUpdateDto);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(1,1, 3)]
        [InlineData(1,3, 2)]
        public void WhenInvalidInputsAreGiven_OfferUpdateDtoValidator_SouldNotBeReturnErrors(int id, decimal price, int productId)
        {
            OfferUpdateDto offerUpdateDto = new OfferUpdateDto()
            {   
                ID = id,
                Price = price,
                ProductID = productId
            };

            OfferUpdateDtoValidator validation = new OfferUpdateDtoValidator();
            ValidationResult result = validation.Validate(offerUpdateDto);

            result.Errors.Count.Should().Be(0);
        }
    }
}
