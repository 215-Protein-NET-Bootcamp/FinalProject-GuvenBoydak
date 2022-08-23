using AutoMapper;
using FinalProject.Base;
using FinalProject.Business;
using FinalProject.DTO;
using FinalProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;


namespace FinalProject.Api
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : CustomBaseController
    {

        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;
        private readonly IFireAndForgetJob _fireAndForgetJob;

        public AppUsersController(IAppUserService appUserService, IMapper mapper, IFireAndForgetJob fireAndForgetJob)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _fireAndForgetJob = fireAndForgetJob;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<AppUser> appUsers = await _appUserService.GetAllAsync();

            List<AppUserListDto> appUserListDtos = _mapper.Map<List<AppUser>, List<AppUserListDto>>(appUsers);

            return CreateActionResult(CustomResponseDto<List<AppUserListDto>>.Success(200,appUserListDtos,"Tüm Kulanıcılar Listelendi"));
        }

        [Authorize]
        [HttpGet]
        [Route("GetActive")]
        public async Task<IActionResult> GetActiveAsync()
        {
            List<AppUser> appUsers = await _appUserService.GetActiveAsync();

            List<AppUserListDto> appUserListDtos = _mapper.Map<List<AppUser>, List<AppUserListDto>>(appUsers);

            return CreateActionResult(CustomResponseDto<List<AppUserListDto>>.Success(200, appUserListDtos, "Active Kulanıcılar Listelendi"));
        }

        [Authorize]
        [HttpGet]
        [Route("GetPassive")]
        public async Task<IActionResult> GetPassiveAsync()
        {
            List<AppUser> appUsers = await _appUserService.GetPassiveAsync();

            List<AppUserListDto> appUserListDtos = _mapper.Map<List<AppUser>, List<AppUserListDto>>(appUsers);

            return CreateActionResult(CustomResponseDto<List<AppUserListDto>>.Success(200, appUserListDtos, "Passive Kulanıcılar Listelendi"));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIDAsync([FromRoute] int id)
        {
            AppUser appUser = await _appUserService.GetByIDAsync(id);

            AppUserDto appUserDto = _mapper.Map<AppUser, AppUserDto>(appUser);

            return CreateActionResult(CustomResponseDto<AppUserDto>.Success(200, appUserDto, $"{id} numaralı Kullanıcı Listelendi"));
        }


        [HttpGet]
        [Route("Activation/{code}")]
        public async Task<IActionResult> GetByActivationCode([FromRoute]Guid code)
        {
           AppUser appUser=  await _appUserService.GetByActivationCode(code);

           if(appUser.Active==true)
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "Activasyon Başarıyla Gerçekleşti"));
           else if(appUser.IsLock==false)
                return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "Hesabınızın Erişim Kısıtlaması Kaldırıldı"));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(404, "Activasyon Gerçekleştirilemedi"));
        }

        [HttpGet]
        [Route("GetAppUserProducts")]
        public async Task<IActionResult> GetAppUserProductsAsync()
        {
            string appUserID = (User.Identity as ClaimsIdentity).FindFirst("AppUserID").Value;

           List<Product> products= await _appUserService.GetAppUserProductsAsync(int.Parse(appUserID));

            List<AppUserProductsDto> appUserProducts = _mapper.Map<List<Product>, List<AppUserProductsDto>>(products);

            return CreateActionResult(CustomResponseDto<List<AppUserProductsDto>>.Success(200,appUserProducts, "Kullanıcının Ürünleri Listelendi"));
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]AppUserRegisterDto registerDto)
        {
            Log.Information($"{User.Identity?.Name}: Register a AppUser.");

            AppUser appUser =await _appUserService.RegisterAsync(registerDto);

            AccessToken token = _appUserService.CreateAccessToken(appUser);

            ProducerService.Producer(appUser);//RabbitMq ile activasyon linki gönderiyruz.

            return CreateActionResult(CustomResponseDto<AccessToken>.Success(200, token, "Kayıt Başarılı Token oluşturuldu"));
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] AppUserLoginDto loginDto)
        {
            Log.Information($"{User.Identity?.Name}: Login a AppUser.");

            AppUser appUser =await _appUserService.LoginAsync(loginDto);

            AccessToken token = _appUserService.CreateAccessToken(appUser);

           await _fireAndForgetJob.SendMailJobAsync(appUser);//Hangfire ile Hoşgeldin mesajı yolluyoruz.

            return CreateActionResult(CustomResponseDto<AccessToken>.Success(200, token, "Giriş Başarılı Token olışturuldu"));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] AppUserUpdateDto updateDto)
        {
            Log.Information($"{User.Identity?.Name}: Update a AppUser ID is {(User.Identity as ClaimsIdentity).FindFirst("AppUserId").Value}.");

            AppUser appUser = _mapper.Map<AppUserUpdateDto, AppUser>(updateDto);

            //user claimlerinden AppUserId'nin degerini alıyoruz.
            string userId = (User.Identity as ClaimsIdentity).FindFirst("AppUserId").Value;

           appUser.ID = int.Parse(userId);
           await _appUserService.UpdateAsync(appUser);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204,"Gücelleme İşlemi Başarılı"));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _appUserService.Delete(id);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204, "Silme işlemi Başarılı"));
        }

        [Authorize]
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] AppUserPasswordUpdateDto passwordUpdateDto)
        {
            Log.Information($"{User.Identity?.Name}: Change Password a AppUser ID is {(User.Identity as ClaimsIdentity).FindFirst("AppUserId").Value}.");


            if (!CheckPassword.CheckingPassword(passwordUpdateDto.NewPassword, passwordUpdateDto.ConfirmPassword))
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400,"Girilen Şifreler Uyuşmuyor"));

            //user claimlerinden AppUserId'nin degerini alıyoruz.
            string userId = (User.Identity as ClaimsIdentity).FindFirst("AppUserId").Value;

            await _appUserService.UpdatePasswordAsync(int.Parse(userId), passwordUpdateDto);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204,"Şifre Başarıyla Degiştirildi"));
        }
    }
}
