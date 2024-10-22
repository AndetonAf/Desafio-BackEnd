using BackEnd.Configurations;
using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Services.Interfaces;
using BackEnd.Validators;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd.Controllers.v1
{
#if DEBUG
    [AllowAnonymous]
    public class MockAuthController(IFacadeService facadeService) : AbstractV1Controller(facadeService)
    {
        [HttpGet("Administrator")]
        public ActionResult<string> Administrator()
        {
            var deliveryMan = Facade.Context.Users.First(x => x.Type == Data.Models.UserType.Administrator);
            List<Claim> claims = [
                new("sub", deliveryMan.Id.ToString()),
                new("type", ((int)UserType.Administrator).ToString())
            ];
            return Ok(AuthService.GenerateJwt(claims, GlobalConfigurations.Settings.Auth.SecretJwt));
        }

        [HttpGet("DeliveryMan")]
        public ActionResult<string> DeliveryMan()
        {
            var deliveryMan = Facade.Context.Users.First(x => x.Type == Data.Models.UserType.DeliveryMan);
            List<Claim> claims = [
                new("sub", deliveryMan.Id.ToString()),
                new("type", ((int)UserType.DeliveryMan).ToString())
            ];
            return Ok(AuthService.GenerateJwt(claims, GlobalConfigurations.Settings.Auth.SecretJwt));
        }
    }
#endif
}
