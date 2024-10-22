using BackEnd.Configurations;
using BackEnd.Services;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Test.UnitTests
{
    public class FixTests
    {
        [Fact]
        public void StorageService_CheckKey()
        {
            GlobalConfigurations.Initialize(new Settings(new Auth(""), new ConnectionStrings(""), new Gcp(false, "", "", "")));
            var storage = new StorageService();
            _ = storage.UploadCnh("", "", "", "");

            var loggerFactory = new LoggerFactory();
            var messagingService = new MessagingService(loggerFactory.CreateLogger<MessagingService>());
            _ = messagingService.PublishMessagesAsync([""], "");
        }

        [Fact]
        public void Context_UpdateLogs()
        {
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor { };
            Context.GetUserId(httpContextAccessor);

            HttpContextAccessor httpContextAccessor2 = new HttpContextAccessor { HttpContext = new DefaultHttpContext() { } };
            Context.GetUserId(httpContextAccessor2);

            HttpContextAccessor httpContextAccessor3 = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "") }))
                }
            };
            Context.GetUserId(httpContextAccessor3);
        }
    }
}
