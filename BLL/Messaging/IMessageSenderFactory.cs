using Microsoft.Extensions.Configuration;

namespace BLL.Messaging
{
    public interface IMessageSenderFactory
    {
        IMessageSender Create(IConfiguration config, string eventHubName);
    }
}
