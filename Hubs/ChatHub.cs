using Microsoft.AspNetCore.SignalR;
using LocalMessenger.Models;
using System.Threading.Tasks;
using LocalMessenger.Data;
using LocalMessenger.Services;
using System.Data.Common;


namespace LocalMessenger.Hubs
{

    public class ChatHub : Hub
    {
    private readonly SettingsBD _db;

    private readonly OllamaServices _ollama;


    public ChatHub(SettingsBD db, OllamaServices ollama)
        {
            _db = db;
            _ollama = ollama;
        }

        public async Task SendMessage(string user, string text)
        {
            
            var message = new Message
            {
                UserName = user,
                Text = text,
                TimeStamp = DateTime.Now
            };
            //сохраняем в базу
        _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            
            // Рассылаем сообщение всем подключённым клиентам
            await Clients.All.SendAsync(
                "ReceiveMessage",
                 message.UserName,
                 message.Text,
                  message.TimeStamp.ToShortTimeString());


                  
            if(text.StartsWith("/ai"))
            {
                var prompt = text.Replace("/ai","").Trim();


                try
                {
                    var aiText = await _ollama.GenerateAsync(prompt);

                    var aiMessage = new Message
                    {
                        UserName = "AI",
                        Text = aiText,
                        TimeStamp = DateTime.Now    
                    };

                    _db.Messages.Add(aiMessage);
                    await _db.SaveChangesAsync();

                    await Clients.All.SendAsync(
                    "ReceiveMessage",
                        "AI",
                        aiText,
                        aiMessage.TimeStamp.ToShortTimeString());
            
                }
                catch(Exception ex)
                {
                    await Clients.All.SendAsync(
                        "ReceiveMessage",
                        $"Ошибка AI:{ex.Message}");
                }
              

                
            


            }
        }
    }
}