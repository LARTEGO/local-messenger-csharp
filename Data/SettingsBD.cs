using Microsoft.EntityFrameworkCore;
using LocalMessenger.Models;

namespace LocalMessenger.Data
{
    public class SettingsBD : DbContext
    {
        public SettingsBD(DbContextOptions<SettingsBD> options) : base(options)
        {}
        

       
        public DbSet<Message> Messages { get; set; }
    }
}