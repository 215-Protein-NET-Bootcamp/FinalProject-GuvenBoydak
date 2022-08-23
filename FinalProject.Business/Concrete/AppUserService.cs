using FinalProject.Base;
using FinalProject.DataAccess;
using FinalProject.DTO;
using FinalProject.Entities;

namespace FinalProject.Business
{
    public class AppUserService : GenericService<AppUser>, IAppUserService
    {
        private readonly IAppUserRepository _userRepository;
        private readonly ITokenHelper _tokenHelper;

        public AppUserService( IAppUserRepository userRepository, ITokenHelper tokenHelper) : base(userRepository)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        /// <summary>
        /// Token Oluşturma işlemi
        /// </summary>
        /// <param name="entity">Kullanıcı bilgisi</param>
        public AccessToken CreateAccessToken(AppUser entity)
        {
            AccessToken accessToken = _tokenHelper.CreateToken(entity.UserName, entity.ID);
            return accessToken;
        }

        /// <summary>
        /// Silme işlemi
        /// </summary>
        /// <param name="id">Silinicek id bilgisi</param>
        public void Delete(int id)
        {
            try
            {
                _userRepository.Delete(id);
            }
            catch (Exception e)
            {

                throw new InvalidOperationException($"Delete_Error  =>  {e.Message}");
            }
        }

        /// <summary>
        /// Kulanıcının Ürünleri bilgisi
        /// </summary>
        /// <param name="id">Kullanıcı id bilgisi</param>
        public async Task<List<Product>> GetAppUserProductsAsync(int id)
        {
          return  await _userRepository.GetAppUserProductsAsync(id);
        }


        /// <summary>
        /// İlgili activasyon kodlu Kullanıcıyı Bilgisini alıyoruz
        /// </summary>
        /// <param name="code">kullanıcı aktivasyon kodu bilgisi</param>
        public async Task<AppUser> GetByActivationCode(Guid code)
        {
           AppUser appUser = await _userRepository.GetByActivationCode(code);

            if (appUser.IsLock == true)//Kullanıcı hesabındakı kilidi kaldırıyoruz.
            {
                appUser.IsLock = false;
                appUser.IncorrectEntry = 1;
            }
            else if (appUser.Active == false)//Kullanıcıyı activasyonunu tamamlıyoruz.
                appUser.Active = true;
           await UpdateAsync(appUser);

            return appUser;
        }

        /// <summary>
        /// İlgili Emaili olan Kullanıcıyı Bilgisini alıyoruz
        /// </summary>
        /// <param name="email">Kullanıcı email bilgisi</param>
        public async Task<AppUser> GetByEmailAsync(string email)
        {
            AppUser appUser = await _userRepository.GetByEmailAsync(email);
            return appUser;
        }


        /// <summary>
        /// Giriş işlemleri
        /// </summary>
        /// <param name="entity">Kulanıcı giriş bilgileri</param>
        public async Task<AppUser> LoginAsync(AppUserLoginDto entity)
        {
            AppUser appUser = await GetByEmailAsync(entity.Email);

            if (appUser == null)
                throw new InvalidOperationException($"({entity.Email}) Kullanıcı Bulunamadı");
            else if (appUser.IncorrectEntry == 4 && appUser.IsLock == true)
                throw new InvalidOperationException($"({entity.Email}) Hesabınız Askıya alındı");

            //Kulanıcı gridigi Password'u Databaseden gelen PasswordHash ve PasswordSalt ile hashleyip kontrol ediyoruz.
            if (!HashingHelper.VerifyPasswordHash(entity.Password, appUser.PasswordHash, appUser.PasswordSalt))
            {
                appUser.IncorrectEntry++; //Şifre yanlış girildiyse 1 atırıyoruz.

                if (appUser.IncorrectEntry == 4)
                {
                    appUser.IsLock = true; //3 kere yanlış girilen kulanıcıyı Lock ediyoruz.
                    DelayedJob.SendMailJob(appUser);//Mail Gönderiyoruz.
                    await UpdateAsync(appUser);//IsLock Güncelliyoruz
                    throw new InvalidOperationException($" Şifreniz 3 kez yanlış girildi Hesap Askıya alındı");
                }
                await UpdateAsync(appUser);//IncorrectEntry Günceliyoruz
                throw new InvalidOperationException($" Paralonanız Yanlış");
            }

            appUser.IncorrectEntry = 1;//Kullanıcı başarılı giriş yaptı ise IncorrectEntry Güncelliyoruz.
            appUser.LastActivty = DateTime.UtcNow;
            await UpdateAsync(appUser);

            return appUser;
        }

        /// <summary>
        /// Kayıt Olma İşlemi
        /// </summary>
        /// <param name="registerDto">Kayıt olucak kullanıcı bilgileri</param>
        public async Task<AppUser> RegisterAsync(AppUserRegisterDto registerDto)
        {
            AppUser appUser = await GetByEmailAsync(registerDto.Email);

            //Girilen Email'i databasede varmı Kontrol ediyoruz.
            if (appUser != null)
                throw new InvalidOperationException($"({registerDto.Email}) Bu Email zaten Kayıtlı");

            //PasswordHash ve PasswordSalt oluşturuyoruz.
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            AppUser user = new AppUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            try
            {
                _userRepository.Add(user);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Register_Error  =>  {e.Message}");
            }
            return user;
        }

        /// <summary>
        /// Güncelleme işlemleri.
        /// </summary>
        /// <param name="entity">Güncellenecek Kullanıcı</param>
        public async Task UpdateAsync(AppUser entity)
        {
            try
            {
                await _userRepository.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Update_Error  =>  {e.Message}");

            }
        }

        /// <summary>
        /// Password günceleme işlemleri
        /// </summary>
        /// <param name="id">Güncellenicek kullanıcı id</param>
        /// <param name="entity">Yeni Şifre bilgileri</param>
        public async Task UpdatePasswordAsync(int id, AppUserPasswordUpdateDto entity)
        {
            AppUser appUser = await _userRepository.GetByIDAsync(id);
            if (appUser == null) //Kulanıcıyı kontrol edıyoruz
                throw new InvalidOperationException($"({id}) Kullanıcı Bulunamadı");

            //Kulanıcı gridigi Password'u Databaseden gelen PasswordHash ve PasswordSalt ile hashleyip kontrol ediyoruz.
            if (!HashingHelper.VerifyPasswordHash(entity.OldPassword, appUser.PasswordHash, appUser.PasswordSalt))
                throw new InvalidOperationException($" Parolanız Yanlış");

            //Yeni PasswordHash ve PasswordSalt oluşturuyoruz.
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(entity.NewPassword, out passwordHash, out passwordSalt);

            appUser.PasswordHash = passwordHash;
            appUser.PasswordSalt = passwordSalt;

            await UpdateAsync(appUser); //Yeni Şifreyle kullanıyı güncelliyoruz.
        }
    }
}
