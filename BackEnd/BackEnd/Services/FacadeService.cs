using BackEnd.Services.Interfaces;
using Data;

namespace BackEnd.Services
{
    public class FacadeService(IHttpContextAccessor httpContext, Context context, IMessagingService messagingService, IStorageService storageService, IMotorcycleService motoService, IUserService userService, IRentalService rentalService) : IFacadeService
    {
        public IHttpContextAccessor HttpContext { get; set; } = httpContext;
        public Context Context { get; set; } = context;
        public IMessagingService MessagingService { get; set; } = messagingService;
        public IStorageService StorageService { get; set; } = storageService;
        public IMotorcycleService MotoService { get; set; } = motoService;
        public IUserService UserService { get; set; } = userService;
        public IRentalService RentalService { get; set; } = rentalService;
    }
}
