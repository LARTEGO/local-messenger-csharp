using Microsoft.AspNetCore.SignalR;
using LocalMessenger.Models;
using System.Threading.Tasks;

namespace LocalMessenger.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string text)
        {
            var message = new Message
            {
                UserName = user,
                Text = text,
                TimeStamp = DateTime.Now
            };

            // Добавляем сообщение в хранилище (как раньше)
            ChatController.messages.Add(message);

            // Рассылаем сообщение всем подключённым клиентам
            await Clients.All.SendAsync("ReceiveMessage", message.UserName, message.Text, message.TimeStamp.ToShortTimeString());
        }
    }
}