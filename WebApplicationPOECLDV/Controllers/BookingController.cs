// BookingController.cs
// Handles CRUD operations for Bookings, including validation and search functionality.
// Michaela Ferraris ST10325652
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationPOECLDV.Models;

namespace WebApplicationPOECLDV.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
            .Include(i => i.Venue)
            .Include(i => i.Event)
            .ToListAsync();

            return View(bookings);
        }

        /// <summary>
        /// Display the Create Booking form.
        /// </summary>
        public IActionResult Create()
        {
            ViewBag.Venues = _context.Venues.ToList();
            ViewBag.Events = _context.Events.ToList();
            return View();
        }

        /// <summary>
        /// Handles the creation of a new booking with validation.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            ViewBag.Venues = _context.Venues.ToList();
            ViewBag.Events = _context.Events.ToList();

            if (ModelState.IsValid)
            {
                // Check for existing bookings on the same date
                var existingBooking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.VenueId == booking.VenueId && b.BookingDate.Date == booking.BookingDate.Date);

                if (existingBooking != null)
                {
                    ModelState.AddModelError("", "A booking already exists for this venue on this date. Please choose a different date or venue.");
                    return View(booking);
                }

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(booking);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .FirstOrDefaultAsync(mbox => mbox.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.Include(b => b.Event).Include(b => b.Venue).FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            // Check if there are active bookings associated with this event or venue
            var activeBookings = await _context.Bookings.AnyAsync(b => b.EventId == booking.EventId || b.VenueId == booking.VenueId);

            if (activeBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this booking. There are active bookings associated with this event or venue.";
                return RedirectToAction(nameof(Index));
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
            ViewBag.EventId = new SelectList(_context.Events, "Id", "Name", booking.EventId);

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
                ViewBag.EventId = new SelectList(_context.Events, "Id", "Name", booking.EventId);
            }
            return View(booking);
        }
    }
}
//ASP.NET MVC Documentation: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview
//Error Handling in ASP.NET: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling