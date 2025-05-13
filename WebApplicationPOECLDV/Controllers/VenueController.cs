
// Handles CRUD operations for Venues, including image upload validation.
// Michaela Ferraris ST10325652
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPOECLDV.Models;
using WebApplicationPOECLDV.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplicationPOECLDV.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenueController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venues.ToListAsync();
            return View(venues);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Venue venue, IFormFile imageFile, [FromServices] BlobStorageService blobService, [FromServices] IConfiguration config)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string containerName = config["AzureStorage:VenueContainer"];
                string imageUrl = await blobService.UploadFileAsync(imageFile, containerName);
                venue.ImageUrl = imageUrl;
            }

            _context.Add(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.Id == id);

            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var venue = await _context.Venues.FirstOrDefaultAsync(mbox => mbox.Id == id);

            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
            {
                return NotFound();
            }

            // Check if there are active bookings associated with this venue
            var hasActiveBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);

            if (hasActiveBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this venue. There are active bookings associated with it.";
                return RedirectToAction("Index");
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if the venue still exists using FindAsync()
                    var existingVenue = await _context.Venues.FindAsync(venue.Id);
                    if (existingVenue == null)
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
            return View(venue);
        }
    }
}
//ASP.NET MVC Documentation: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview
//Error Handling in ASP.NET: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling