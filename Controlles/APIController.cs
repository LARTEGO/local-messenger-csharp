using Microsoft.AspNetCore.Mvc;
using LocalMessenger.Data;
using Microsoft.EntityFrameworkCore;

namespace LocalMessenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly SettingsBD _db;
        public MessagesController(SettingsBD db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _db.Messages.OrderBy(m => m.TimeStamp).ToListAsync();
            return Ok(messages);
        }
    }
}