﻿
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyForum.Models;
using MyForum.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace homework_59.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private IWebHostEnvironment _appEnvironment;

        private ForumContext _db;






        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ForumContext db, IWebHostEnvironment appEnvironment)

        {

            _userManager = userManager;

            _signInManager = signInManager;

            _db = db;

            _appEnvironment = appEnvironment;

        }
        private ImageModel ReadIForm(IFormFile ImgFile)
        {
            string path = "/Avatars/" + ImgFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                ImgFile.CopyTo(fileStream);
            }
            ImageModel file = new ImageModel { Name = ImgFile.FileName, Path = path, };
            return file;
        }

        [HttpGet]

        public IActionResult Register()

        {

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile ImgFile)
        {
            ImageModel file = ReadIForm(ImgFile);
            User user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Avatar = file.Path,
                Login = model.Login
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            //Создание пользователя средствами Identity на основе объекта пользователя и его пароля

            //Возвращает результат выполнения в случае успеха впускаем пользователя в систему

            if (result.Succeeded)

            {
                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");

            }

            foreach (var error in result.Errors)

                ModelState.AddModelError(string.Empty, error.Description);



            return View(model);

        }
        [HttpGet]

        public IActionResult Login(string returnUrl = null)

        {

            return View(new LoginViewModel { ReturnUrl = returnUrl });

        }


        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model)
            
        {
            if (ModelState.IsValid)

            {
                User user = _db.ForumUsers.FirstOrDefault(u => u.Email == model.Login || u.Login == model.Login);

                var result = await _signInManager.PasswordSignInAsync(

                    user,

                    model.Password,

                    model.RememberMe,

                    false

                    );

                if (result.Succeeded)

                {

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))

                    {

                        return Redirect(model.ReturnUrl);

                    }


                    return RedirectToAction("Index", "Home");

                }

                ModelState.AddModelError("", "Неправильный логин и (или) пароль");

            }

            return View(model);

        }


        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> LogOff()

        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }
    }
}
