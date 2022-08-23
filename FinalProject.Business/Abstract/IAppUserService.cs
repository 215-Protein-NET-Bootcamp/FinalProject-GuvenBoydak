using FinalProject.Base;
using FinalProject.DTO;
using FinalProject.Entities;

namespace FinalProject.Business
{
    public interface IAppUserService : IBaseService<AppUser>
    {
        Task<AppUser> RegisterAsync(AppUserRegisterDto registerDto);

        Task<AppUser> LoginAsync(AppUserLoginDto entity);

        Task<AppUser> GetByEmailAsync(string email);

        AccessToken CreateAccessToken(AppUser entity);

        Task UpdatePasswordAsync(int id, AppUserPasswordUpdateDto entity);

        Task UpdateAsync(AppUser entity);

        void Delete(int id);

        Task<AppUser> GetByActivationCode(Guid code);

        Task<List<Product>> GetAppUserProductsAsync(int id);
    }

}
