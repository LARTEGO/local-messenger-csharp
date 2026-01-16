using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Data;
using Microsoft.EntityFrameworkCore;
using LocalMessenger.Hubs;
using Microsoft.AspNetCore.SignalR;
using LocalMessenger.Models;

namespace LocalMessenger.Controllers
{
     [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly SettingsBD _db;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(SettingsBD db, IHubContext<ChatHub> hubContext)
        {
            _db = db;
            _hubContext = hubContext;
        }

        // GET: api/messages
        // Получить все сообщения
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            var messages = await _db.Messages
                .OrderBy(m => m.TimeStamp)
                .ToListAsync();

            return Ok(messages);
        }

        // GET: api/MessagesApi/5
        // Получить сообщение по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _db.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new { error = "Сообщение не найдено" });
            }

            return Ok(message);
        }

        // POST: api/MessagesApi
        // Отправить новое сообщение
        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage([FromBody] MessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.User) || string.IsNullOrWhiteSpace(dto.Text))
            {
                return BadRequest(new { error = "Поля User и Text обязательны" });
            }

            var message = new Message
            {
                UserName = dto.User,
                Text = dto.Text,
                TimeStamp = DateTime.Now
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            // Отправляем сообщение всем через SignalR
            await _hubContext.Clients.All.SendAsync(
                "ReceiveMessage",
                message.UserName,
                message.Text,
                message.TimeStamp.ToShortTimeString()
            );

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }

        // PUT: api/MessagesApi/5
        // Обновить сообщение
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] MessageDto dto)
        {
            var message = await _db.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new { error = "Сообщение не найдено" });
            }

            if (!string.IsNullOrWhiteSpace(dto.User))
                message.UserName = dto.User;

            if (!string.IsNullOrWhiteSpace(dto.Text))
                message.Text = dto.Text;

            _db.Entry(message).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            // Уведомляем всех об обновлении через SignalR
            await _hubContext.Clients.All.SendAsync(
                "MessageUpdated",
                message.Id,
                message.UserName,
                message.Text,
                message.TimeStamp.ToShortTimeString()
            );

            return Ok(message);
        }

        // DELETE: api/MessagesApi/5
        // Удалить сообщение
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _db.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new { error = "Сообщение не найдено" });
            }

            _db.Messages.Remove(message);
            await _db.SaveChangesAsync();

            // Уведомляем всех об удалении через SignalR
            await _hubContext.Clients.All.SendAsync("MessageDeleted", id);

            return Ok(new { success = true, message = "Сообщение удалено" });
        }
    }

    // DTO для запросов (чтобы не передавать полную модель)
    public class MessageDto
    {
        public required string User { get; set; }
        public required string Text { get; set; }
    }
}