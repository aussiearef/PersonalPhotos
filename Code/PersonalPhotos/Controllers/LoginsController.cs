using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PersonalPhotos.Models;

namespace PersonalPhotos.Controllers;

public class LoginsController(ILogins loginService, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    public IActionResult Index(string? returnUrl)
    {
        var model = new LoginViewModel { ReturnUrl = returnUrl ?? "" , Email="", Password="" };
        return View("Login", model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid login details");
            return View("Login", model);
        }

        var user = await loginService.GetUser(model.Email);
        if (user != null)
        {
            if (user.Password == model.Password)
            {
                //ToDo: redirect to home page
                httpContextAccessor?.HttpContext?.Session.SetString("User", model.Email);
            }
            else
            {
                ModelState.AddModelError("", "Invalid password");
                return View("Login", model);
            }
        }
        else
        {
            ModelState.AddModelError("", "User was not found");
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
    public async Task<IActionResult> Create(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid user details");
            return View(model);
        }

        var existingUser = await loginService.GetUser(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "This email address is already registered");
            return View(model);
        }

        await loginService.CreateUser(model.Email, model.Password);

        return RedirectToAction("Index", "Logins");
    }
}