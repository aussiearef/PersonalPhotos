using System;
using Microsoft.AspNetCore.Http;
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
            if (string.IsNullOrEmpty(currentUser)) context.Result = new RedirectToActionResult("Index", "Logins", null);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}