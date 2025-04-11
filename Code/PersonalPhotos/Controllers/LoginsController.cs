using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PersonalPhotos.Models;

namespace PersonalPhotos.Controllers;

public class LoginsController(ILogins loginService)
    : Controller
{
    public IActionResult Index(string? returnUrl)
    {
        var model = new LoginViewModel { ReturnUrl = returnUrl ?? "" , Email="", Password="" };
        return View("Login", model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid login details");
            return View("Login", model);
        }

        var loginResult = await loginService.Login(model.Email, model.Password, token);
        if (loginResult == UserLoginResult.UserNotFound)
        {
            ModelState.AddModelError("", "User was not found");
            return View("Login", model);
        }
        if (loginResult == UserLoginResult.InvalidPassword)
        {
            ModelState.AddModelError("", "Invalid password");
            return View("Login", model);
        }
        

        if (!string.IsNullOrEmpty(model.ReturnUrl))
            return Redirect(model.ReturnUrl);
        return RedirectToAction("Display", "Photos");
    }

    public IActionResult Create()
    {
        return View("Create");
    }

    [HttpPost]
    public async Task<IActionResult> Create(LoginViewModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid user details");
            return View(model);
        }

        var existingUser = await loginService.GetUser(model.Email, token);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "This email address is already registered");
            return View(model);
        }

        await loginService.CreateUser(model.Email, model.Password, token);

        return RedirectToAction("Index", "Logins");
    }
}