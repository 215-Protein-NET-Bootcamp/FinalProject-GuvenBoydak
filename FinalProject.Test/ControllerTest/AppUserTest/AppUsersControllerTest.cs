using AutoMapper;
using FinalProject.Api;
using FinalProject.Base;
using FinalProject.Business;
using FinalProject.DTO;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinalProject.Test
{
    public class AppUsersControllerTest
    {
        private readonly Mock<IAppUserService> _appUserService;
        private readonly IMapper _mapper;
        private readonly AppUsersController _appUserController;

        public AppUsersControllerTest()
        {
            MapperConfiguration map = new MapperConfiguration(configure =>
            {
                configure.AddProfile(new MapProfile());
            });
            _appUserService = new Mock<IAppUserService>();
            _mapper = map.CreateMapper();
            _appUserController = new AppUsersController(_appUserService.Object, _mapper,null);
        }

        List<AppUser> appUsers = new List<AppUser>
        {
            new AppUser(){ID=1,UserName="guven",Email="gvn.boydak@gmail.com",PhoneNumber="5353742090",FirstName="Güven",LastName="Boydak",CreatedDate=DateTime.Now,Status=DataStatus.Inserted},
            new AppUser(){ID=2,UserName="ali",Email="ali@gmail.com",PhoneNumber="5345558980",FirstName="Ali",LastName="Satı",CreatedDate=DateTime.Now,Status=DataStatus.Inserted},
            new AppUser(){ID=3,UserName="semih",Email="semih@gmail.com",PhoneNumber="5353111525",FirstName="Semih",LastName="fark",CreatedDate=DateTime.Now,Status=DataStatus.Inserted},
        };


        [Fact]
        public async void GetAll_ActionExecutes_Return200WithAppUserListDto()
        {
            _appUserService.Setup(x => x.GetAllAsync()).ReturnsAsync(appUsers);

            IActionResult result = await _appUserController.GetAllAsync();

            ObjectResult objectResult=Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<AppUserListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<AppUserListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Fact]
        public async void GetActive_ActionExecutes_Return200WithAppUserListDto()
        {
            _appUserService.Setup(x => x.GetActiveAsync()).ReturnsAsync(appUsers);

            IActionResult result = await _appUserController.GetActiveAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<AppUserListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<AppUserListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Fact]
        public async void GetPassive_ActionExecutes_Return200WithAppUserListDto()
        {
            _appUserService.Setup(x => x.GetPassiveAsync()).ReturnsAsync(appUsers);

            IActionResult result = await _appUserController.GetPassiveAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<AppUserListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<AppUserListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        public async void GetByID_ActionExecutes_Return200WithAppUserDto(int id)
        {
            AppUser appUser = appUsers.FirstOrDefault(x => x.ID == id);
            _appUserService.Setup(x => x.GetByIDAsync(It.IsAny<int>())).ReturnsAsync(appUser);

            IActionResult result = await _appUserController.GetByIDAsync(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

             Assert.IsAssignableFrom<CustomResponseDto<AppUserDto>>(objectResult.Value);
        }


        [Fact]
        public async void Register_ActionExecutes_Return200WithAccessToken()
        {
            AppUser appUser=new AppUser();
            AccessToken accessToken=new AccessToken();
            AppUserRegisterDto dto = new AppUserRegisterDto() { Email = "test@gmail.com", Password = "12345678" };

            _appUserService.Setup(x => x.RegisterAsync(It.IsAny<AppUserRegisterDto>())).ReturnsAsync(appUser);
            _appUserService.Setup(x => x.CreateAccessToken(It.IsAny<AppUser>())).Returns(accessToken);

            IActionResult result = await _appUserController.RegisterAsync(dto);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            Assert.IsAssignableFrom<CustomResponseDto<AccessToken>>(objectResult.Value);
        }

        [Fact]
        public async void Login_ActionExecutes_Return200WithAccessToken()
        {
            AppUser appUser = new AppUser();
            AccessToken accessToken = new AccessToken();
            AppUserRegisterDto dto = new AppUserRegisterDto() { Email = "test@gmail.com", Password = "12345678" };

            _appUserService.Setup(x => x.LoginAsync(It.IsAny<AppUserLoginDto>())).ReturnsAsync(appUser);
            _appUserService.Setup(x => x.CreateAccessToken(It.IsAny<AppUser>())).Returns(accessToken);

            IActionResult result = await _appUserController.RegisterAsync(dto);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            Assert.IsAssignableFrom<CustomResponseDto<AccessToken>>(objectResult.Value);
        }

        [Theory]
        [InlineData(1)]
        public  void Delete_ActionExecutes_Return204NoContentDto(int id)
        {
            _appUserService.Setup(x => x.Delete(It.IsAny<int>()));

            IActionResult result = _appUserController.Delete(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204, objectResult.StatusCode.Value);
        }
    }
}
