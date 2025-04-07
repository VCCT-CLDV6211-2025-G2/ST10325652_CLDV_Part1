using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using WebApplicationPOECLDV.Models;

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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Event eventModel)
        {
            if(ModelState.IsValid)
            {
                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
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
            var eventModel = await _context.Events.FirstOrDefaultAsync(mbox => mbox.Id == id);

            if(eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var eventModel = await _context.Events.FindAsync(id);
            _context.Events.Remove(eventModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
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

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!EventExists(eventModel.Id))
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
 