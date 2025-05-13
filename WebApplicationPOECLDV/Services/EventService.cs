using Microsoft.EntityFrameworkCore;
using WebApplicationPOECLDV.Models;

namespace WebApplicationPOECLDV.Services
{
    public class EventService
    {
            private readonly ApplicationDbContext _context;

            public EventService(ApplicationDbContext context)
            {
                _context = context;
            }

    
    }
}
