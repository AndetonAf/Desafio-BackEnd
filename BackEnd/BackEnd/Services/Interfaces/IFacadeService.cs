using Data;

namespace BackEnd.Services.Interfaces
{
    public interface IFacadeService
    {
        public IHttpContextAccessor HttpContext { get; set; }
        public Context Context { get; set; }
        public IMessagingService MessagingService { get; set; }
        public IStorageService StorageService { get; set; }
        public IMotorcycleService MotoService { get; set; }
        public IUserService UserService { get; set; }
        public IRentalService RentalService { get; set; }
    }
}
