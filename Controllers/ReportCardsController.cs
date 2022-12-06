using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using ASP.NETCoreIdentityCustom.Models;

namespace ASP.NETCoreIdentityCustom.Controllers
{
    public class ReportCardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReportCards
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReportCard.Include(r => r.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ReportCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReportCard == null)
            {
                return NotFound();
            }

            var reportCard = await _context.ReportCard
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ReportCardId == id);
            if (reportCard == null)
            {
                return NotFound();
            }

            return View(reportCard);
        }

        // GET: ReportCards/Create
        public IActionResult Create()
        {
            var selectListItems = _context.ApplicationUser.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.FirstName + " " + r.LastName,
            });
            ViewData["ApplicationUserId"] = new SelectList(selectListItems, "Value", "Text");
            return View();
        }

        // POST: ReportCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportCardId,Url,ApplicationUserId,ApplicationUser")] ReportCard reportCard)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(reportCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "FirstName", reportCard.ApplicationUserId);
            return View(reportCard);
        }

        // GET: ReportCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReportCard == null)
            {
                return NotFound();
            }

            var reportCard = await _context.ReportCard.FindAsync(id);
            if (reportCard == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", reportCard.ApplicationUserId);
            return View(reportCard);
        }

        // POST: ReportCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportCardId,Url,ApplicationUserId")] ReportCard reportCard)
        {
            if (id != reportCard.ReportCardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportCardExists(reportCard.ReportCardId))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", reportCard.ApplicationUserId);
            return View(reportCard);
        }

        // GET: ReportCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReportCard == null)
            {
                return NotFound();
            }

            var reportCard = await _context.ReportCard
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ReportCardId == id);
            if (reportCard == null)
            {
                return NotFound();
            }

            return View(reportCard);
        }

        // POST: ReportCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReportCard == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ReportCard'  is null.");
            }
            var reportCard = await _context.ReportCard.FindAsync(id);
            if (reportCard != null)
            {
                _context.ReportCard.Remove(reportCard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportCardExists(int id)
        {
            return _context.ReportCard.Any(e => e.ReportCardId == id);
        }
    }
}