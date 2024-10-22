using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using Data.Models;

namespace BackEnd.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthAttribute(UserType UserType) : Attribute, IAuthorizationFilter
    {
        private UserType _userType = UserType;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userType = context.HttpContext.User.FindFirstValue("type");
            if (userType == ((int)_userType).ToString())
            {
                return;
            }

            context.Result = new ForbidResult();
            return;
        }
    }
}
