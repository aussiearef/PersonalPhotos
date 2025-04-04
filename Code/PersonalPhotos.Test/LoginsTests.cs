using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;

namespace PersonalPhotos.Test;

public class LoginsTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly Mock<ILogins> _logins;
    private readonly LoginsController _loginsController;

    public LoginsTests()
    {
        _logins = new Mock<ILogins>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
        _loginsController = new LoginsController(_logins.Object, _httpContextAccessor.Object);

        //ILogins logins = Mock.Of<ILogins>();
    }

    [Fact]
    public async Task Logins_GivenModelStateIsInvalid_ReturnsLoginView()
    {
        _loginsController.ModelState.AddModelError("Test", "Test");

        var model = Mock.Of<LoginViewModel>();
        var result = await _loginsController.Login(model);

        Assert.IsType<ViewResult>(result);

        var viewResult = result as ViewResult;
        Assert.Equal("Login", viewResult.ViewName, true);
    }

    [Fact]
    public async Task Logins_GivenValidModel_RedirectsToDisplayAction()
    {
        const string email = "a@b.com";
        const string password = "123";

        var loginVideModel = Mock.Of<LoginViewModel>(x=> x.Email == email && x.Password == password);
        var user = Mock.Of<User>(x=> x.Email == email && x.Password == password);

        _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(user);

        var result = await _loginsController.Login(loginVideModel);

        Assert.IsType<RedirectToActionResult>(result);

        var redirectResult = result as RedirectToActionResult;

        Assert.Equal("Photos", redirectResult.ControllerName, ignoreCase: true);
        Assert.Equal("Display", redirectResult.ActionName, ignoreCase:true);
    }

}