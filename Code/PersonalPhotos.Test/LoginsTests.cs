using PersonalPhotos.Controllers;
using Xunit;
using Moq;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPhotos.Models;
using System.Threading.Tasks;
using Core.Models;

namespace PersonalPhotos.Test
{
    public class LoginsTests
    {
        private readonly LoginsController _controller;
        private readonly Mock<ILogins> _logins;
        private readonly Mock<IHttpContextAccessor> _accessor;

        public LoginsTests()
        {
            _logins = new Mock<ILogins>();
            

            var session = Mock.Of<ISession>();
            var httpContext = Mock.Of<HttpContext>(x => x.Session == session);

            _accessor = new Mock<IHttpContextAccessor>();
            _accessor.Setup(x => x.HttpContext).Returns(httpContext);

            _controller = new LoginsController(_logins.Object, _accessor.Object);
        }

        [Fact]
        public void Index_GivenNorReturnUrl_ReturnLoginView()
        {
            var result = (_controller.Index() as ViewResult);

            Assert.NotNull(result);
            Assert.Equal("Login", result.ViewName, ignoreCase: true);
        }

        [Fact]
        public async Task Login_GivenModelStateInvalid_ReturnLoginView()
        {
            _controller.ModelState.AddModelError("Test", "Test");

            var result = await _controller.Login(Mock.Of<LoginViewModel>()) as ViewResult;
            Assert.Equal("Login", result.ViewName, ignoreCase: true);
        }

        [Fact]
        public async Task Login_GivenCorrectPassword_RedirectToDisplayAction()
        {
            const string password = "123";
            var modelView = Mock.Of<LoginViewModel>(x=> x.Email == "a@b.com" && x.Password== password);
            var model = Mock.Of<User>(x=> x.Password == password);

            _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.Login(modelView);

            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
