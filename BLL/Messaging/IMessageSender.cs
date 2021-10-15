using System;
using System.Threading.Tasks;

namespace BLL.Messaging
{
    public interface IMessageSender : IDisposable
    {
        Task CreateEventBatchAsync();
        bool AddMessage(object data);
        Task SendMessage();
    }
}
