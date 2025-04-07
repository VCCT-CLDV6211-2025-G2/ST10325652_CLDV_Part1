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
        public IActionResult Create()
        {
            ViewBag.Venues = _context.Venues.ToList();
            ViewBag.Events = _context.Events.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Venues = _context.Venues.ToList();
            ViewBag.Events = _context.Events.ToList();
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
            var booking = await _context.Bookings.FindAsync(id);
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
