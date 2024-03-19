using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMO.Models;

namespace HMO.Controllers
{
    public class CoronaVirusController : Controller
    {
        private readonly HMOContext _context;

        public CoronaVirusController(HMOContext context)
        {
            _context = context;
        }

        // GET: CoronaVirus
        public async Task<IActionResult> Index()
        {
            var hMOContext = _context.CoronaViruses.Include(c => c.Member);
            return View(await hMOContext.ToListAsync());
        }

        // GET: CoronaVirus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coronaVirus = await _context.CoronaViruses
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.VirusId == id);
            if (coronaVirus == null)
            {
                return NotFound();
            }

            return View(coronaVirus);
        }

        // GET: CoronaVirus/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity");
            return View();
        }

        // POST: CoronaVirus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VirusId,MemberId,DatePositiveResult,DateRecovery")] CoronaVirus coronaVirus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coronaVirus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity", coronaVirus.MemberId);
            return View(coronaVirus);
        }

        // GET: CoronaVirus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coronaVirus = await _context.CoronaViruses.FindAsync(id);
            if (coronaVirus == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity", coronaVirus.MemberId);
            return View(coronaVirus);
        }

        // POST: CoronaVirus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VirusId,MemberId,DatePositiveResult,DateRecovery")] CoronaVirus coronaVirus)
        {
            if (id != coronaVirus.VirusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coronaVirus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoronaVirusExists(coronaVirus.VirusId))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity", coronaVirus.MemberId);
            return View(coronaVirus);
        }

        // GET: CoronaVirus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coronaVirus = await _context.CoronaViruses
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.VirusId == id);
            if (coronaVirus == null)
            {
                return NotFound();
            }

            return View(coronaVirus);
        }

        // POST: CoronaVirus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coronaVirus = await _context.CoronaViruses.FindAsync(id);
            if (coronaVirus != null)
            {
                _context.CoronaViruses.Remove(coronaVirus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoronaVirusExists(int id)
        {
            return _context.CoronaViruses.Any(e => e.VirusId == id);
        }
    }
}
