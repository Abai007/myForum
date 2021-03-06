
using MyForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.ViewModel
{
    public class RegisterViewModel
    {
        [Required]

        [Display(Name = "Email")]

        public string Email { get; set; }

        [Required]

        [Display(Name = "Login")]

        public string Login { get; set; }


        [Required]

        [Display(Name = "Name")]

        public string Name { get; set; }


        [Required]

        [DataType(DataType.Password)]

        [Display(Name = "Пароль")]

        public string Password { get; set; }


        [Required]

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]

        [DataType(DataType.Password)]

        [Display(Name = "Подтвердить пароль")]

        public string PasswordConfirm { get; set; }
        public User User { get; set; }
    }
}
