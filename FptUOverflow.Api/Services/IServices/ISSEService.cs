using System.Threading.Channels;

namespace FptUOverflow.Api.Services.IServices
{
    public interface ISSEService 
    {
        
        ChannelReader<string> RegisterUser(Guid userId);
        Task SendEventToUser(Guid userId, string message);
        void RemoveUser(Guid userId);

    }
}
