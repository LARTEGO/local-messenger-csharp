using Microsoft.AspNetCore.SignalR;
using LocalMessenger.Models;
using System.Threading.Tasks;
using LocalMessenger.Data;

namespace LocalMessenger.Hubs
{
    public class ChatHub : Hub
    {
    private readonly SettingsBD _db;

    public ChatHub(SettingsBD db)
        {
            _db = db;
        }
        
        public async Task SendMessage(string user, string text)
        {
            // foreach(var Users in UserController.users)
            // {
            //     if ()
            //     {
            //         var message = new Message
            //     {
            //         UserName = user,
            //         Text = text,
            //         TimeStamp = DateTime.Now
            //     };
            //     }
            // }
            var message = new Message
            {
                UserName = user,
                Text = text,
                TimeStamp = DateTime.Now
            };
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            // Рассылаем сообщение всем подключённым клиентам
            await Clients.All.SendAsync("ReceiveMessage", message.UserName, message.Text, message.TimeStamp.ToShortTimeString());
        }
    }
}