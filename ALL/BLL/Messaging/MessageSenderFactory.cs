using Microsoft.Extensions.Configuration;

namespace BLL.Messaging
{
    public class MessageSenderFactory : IMessageSenderFactory
    {
        public IMessageSender Create(IConfiguration config, string eventHubName)
        {
            return new MessageSender(config, eventHubName); ;
        }
    }
}

