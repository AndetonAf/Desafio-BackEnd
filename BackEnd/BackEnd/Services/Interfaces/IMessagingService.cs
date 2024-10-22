using BackEnd.DTOs;
using Data.Models;

namespace BackEnd.Services.Interfaces
{
    public interface IMessagingService
    {
        public Task PublishMessagesAsync(IEnumerable<string> messageTexts, string topicId);
    }
}
