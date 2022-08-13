﻿namespace FinalProject.DTO
{
    public class AppUserRegisterDto : BaseDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime LastActivty { get; set; }

        public string PhoneNumber { get; set; }
    }
}
