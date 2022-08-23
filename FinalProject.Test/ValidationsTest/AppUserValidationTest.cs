using FinalProject.Business;
using FinalProject.DTO;
using FluentAssertions;
using FluentValidation.Results;

namespace FinalProject.Test
{
    public class AppUserValidationTest
    {

        [Theory]
        [InlineData("test.gmail.com","1234567")]
        [InlineData("test","")]
        [InlineData("","")]
        public void WhenInvalidInputsAreGiven_AppUserLoginDtoValidator_SouldBeReturnErrors(string email,string password)
        {
            AppUserLoginDto appUserLoginDto = new AppUserLoginDto()
            {
                Email = email,
                Password = password
            };

            AppUserLoginDtoValidator validation =new AppUserLoginDtoValidator();
            ValidationResult result=validation.Validate(appUserLoginDto);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("test@gmail.com", "12345678")]
        public void WhenInvalidInputsAreGiven_AppUserLoginDtoValidator_SouldNotBeReturnErrors(string email, string password)
        {
            AppUserLoginDto appUserLoginDto = new AppUserLoginDto()
            {
                Email = email,
                Password = password
            };

            AppUserLoginDtoValidator validation = new AppUserLoginDtoValidator();
            ValidationResult result = validation.Validate(appUserLoginDto);

            result.Errors.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("12341212", "","")]
        [InlineData("12341212", "deneme1","test123")]
        public void WhenInvalidInputsAreGiven_AppUserPasswordUpdateDtoValidator_SouldBeReturnErrors(string oldPassword,string newPassword, string confirmPassword)
        {
            AppUserPasswordUpdateDto updateDto = new AppUserPasswordUpdateDto()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };

            AppUserPasswordUpdateDtoValidator validation = new AppUserPasswordUpdateDtoValidator();
            ValidationResult result = validation.Validate(updateDto);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("12341212", "12345678", "12345678")]
        public void WhenInvalidInputsAreGiven_AppUserPasswordUpdateDtoValidator_SouldNotBeReturnErrors(string oldPassword, string newPassword, string confirmPassword)
        {
            AppUserPasswordUpdateDto updateDto = new AppUserPasswordUpdateDto()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };

            AppUserPasswordUpdateDtoValidator validation = new AppUserPasswordUpdateDtoValidator();
            ValidationResult result = validation.Validate(updateDto);

            result.Errors.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("test", "test@gmail.com", "123456789","arif","sayın","55564645235")]
        public void WhenInvalidInputsAreGiven_AppUserRegisterDtoValidator_SouldNotBeReturnErrors(string userName, string email, string password,string firstname,string lastname,string phoneNumber)
        {
            AppUserRegisterDto registerDto = new AppUserRegisterDto()
            {
               Email = email,
               UserName = userName,
               Password = password,
               PhoneNumber = phoneNumber,
               FirstName=firstname,
               LastName=lastname               
            };

            AppUserRegisterDtoValidator validation = new AppUserRegisterDtoValidator();
            ValidationResult result = validation.Validate(registerDto);

            result.Errors.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("test", "testgmail.com", "", "1sadık1", "sayın1", "5556464523e")]
        [InlineData("", "testgmail.com", "1234567", "", "sayın1", "5556464523e")]
        [InlineData("dene12", "", "1234567", "", "sayın1", "5556464523e")]
        public void WhenInvalidInputsAreGiven_AppUserRegisterDtoValidator_SouldBeReturnErrors(string userName, string email, string password, string firstname, string lastname, string phoneNumber)
        {
            AppUserRegisterDto registerDto = new AppUserRegisterDto()
            {
                Email = email,
                UserName = userName,
                Password = password,
                PhoneNumber = phoneNumber,
                FirstName = firstname,
                LastName = lastname
            };

            AppUserRegisterDtoValidator validation = new AppUserRegisterDtoValidator();
            ValidationResult result = validation.Validate(registerDto);

            result.Errors.Count.Should().BeGreaterThan(0);
        }


        [Theory]
        [InlineData("deneme", "test@gmail.com","sadık", "sayın", "55564645234")]
        public void WhenInvalidInputsAreGiven_AppUserUpdateDtoValidator_SouldNotBeReturnErrors(string userName, string email, string firstname, string lastname, string phoneNumber)
        {
            AppUserUpdateDto updateDto = new AppUserUpdateDto()
            {
                Email = email,
                UserName = userName,
                PhoneNumber = phoneNumber,
                FirstName = firstname,
                LastName = lastname
            };

            AppUserUpdateDtoValidator validation = new AppUserUpdateDtoValidator();
            ValidationResult result = validation.Validate(updateDto);

            result.Errors.Count.Should().Be(0);
        }


        [Theory]
        [InlineData("test", "testgmail.com", "", "sayın1", "5h6464523e")]
        [InlineData("test1", "test23gmail.com", "1sadık1", "sayın1", "5556464523e")]
        public void WhenInvalidInputsAreGiven_AppUserUpdateDtoValidator_SouldBeReturnErrors(string userName, string email, string firstname, string lastname, string phoneNumber)
        {
            AppUserUpdateDto updateDto = new AppUserUpdateDto()
            {
                Email = email,
                UserName = userName,
                PhoneNumber = phoneNumber,
                FirstName = firstname,
                LastName = lastname
            };

            AppUserUpdateDtoValidator validation = new AppUserUpdateDtoValidator();
            ValidationResult result = validation.Validate(updateDto);

            result.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}
