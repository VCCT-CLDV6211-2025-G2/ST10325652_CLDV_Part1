// EventController.cs
// Handles CRUD operations for Events, including image upload validation.
// Michaela Ferraris ST10325652
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using WebApplicationPOECLDV.Models;
using WebApplicationPOECLDV.Services;

namespace WebApplicationPOECLDV.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }
        public IActionResult Create()
        {
            // Populate the dropdown list with available venues (Id as value, VenueName as display text)
            ViewBag.Venues = new SelectList(_context.Venues, "Id", "VenueName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            Event eventModel, 
            IFormFile imageFile, 
            [FromServices] BlobStorageService blobService, 
            [FromServices] IConfiguration config, 
            [FromServices] EventService eventService)
        {

            // Upload image if provided
            if (imageFile != null && imageFile.Length > 0)
            {
                string containerName = config["AzureStorage:EventContainer"];
                string imageUrl = await blobService.UploadFileAsync(imageFile, containerName);
                eventModel.ImageUrl = imageUrl;
            }

            // Save to database
            _context.Add(eventModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Details(int? id)
        {
            var eventModel = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

            if(eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        public async Task<IActionResult> Delete(int?id)
        {
            var eventModel = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var eventModel = await _context.Events.FindAsync(id);

            if (eventModel == null)
            {
                return NotFound();
            }

            // Check if there are active bookings associated with this event
            var hasActiveBookings = await _context.Bookings.AnyAsync(b => b.EventId == id);

            if (hasActiveBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this event. There are active bookings associated with it.";
                return RedirectToAction("Index");
            }

            _context.Events.Remove(eventModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events.FindAsync(id);
            if(eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event eventModel)
        {
            if(id != eventModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if the event still exists using FindAsync()
                    var existingEvent = await _context.Events.FindAsync(eventModel.Id);
                    if (existingEvent == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
        }
    }
}
//ASP.NET MVC Documentation: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview
//Error Handling in ASP.NET: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling 