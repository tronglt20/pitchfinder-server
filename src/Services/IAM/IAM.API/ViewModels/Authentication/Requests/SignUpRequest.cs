﻿using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace IAM.API.ViewModels.Authentication.Requests
{
    public class SignUpRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[_@$!%*#?&])[A-Za-z\d_@$!%*#?&]{8,}$"
            , ErrorMessage = "Mật khẩu của bạn phải từ 8 ký tự, có ít nhất 1 ký tự số, chữ thường và một ký tự đặc biệt")]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }

        public bool IsCustomer { get; set; } = true;

        public int RoleId()
        {
            if (IsCustomer)
                return (int)RoleEnum.Customer;

            return (int)RoleEnum.Owner;
        }
    }
}
