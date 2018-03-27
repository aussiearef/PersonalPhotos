using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalPhotos.Filters
{
    public class LoginAttribute : Attribute, IActionFilter
    {
        private readonly IHttpContextAccessor _accessor;

        public LoginAttribute(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var currentUser = _accessor.HttpContext.Session.GetString("User");
            var currentUrl = _accessor.HttpContext.Request.GetEncodedUrl();
            if (string.IsNullOrEmpty(currentUser)) context.Result = new RedirectToActionResult("Index", "Logins", new{returnUrl=currentUrl});
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}