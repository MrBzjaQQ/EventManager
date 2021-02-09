using EventManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class EventManagerContext: IdentityDbContext<UserModel>
    {
        public EventManagerContext(DbContextOptions<EventManagerContext> options): base(options)
        {
            
        }
    }
}