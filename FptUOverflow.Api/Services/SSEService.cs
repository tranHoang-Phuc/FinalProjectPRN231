using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

namespace FptUOverflow.Api.Services
{
    public class SSEService : ISSEService
    {
        private readonly ConcurrentDictionary<Guid, List<Channel<string>>> _userChannels = new();

        public ChannelReader<string> RegisterUser(Guid userId)
        {
            var channel = Channel.CreateUnbounded<string>();
            _userChannels.AddOrUpdate(userId,
                _ => new List<Channel<string>> { channel },
                (_, list) =>
                {
                    list.Add(channel);
                    return list;
                });
            return channel.Reader;
        }

        public void RemoveUser(Guid userId)
        {
            if (_userChannels.TryRemove(userId, out var channels))
            {
                foreach (var channel in channels)
                {
                    channel.Writer.Complete();
                }
            }
        }

        public async Task SendEventToUser(Guid userId, string message)
        {
            if (_userChannels.TryGetValue(userId, out var channels))
            {
                foreach (var channel in channels)
                {
                    await channel.Writer.WriteAsync(message);
                }
            }
        }
    }

}
