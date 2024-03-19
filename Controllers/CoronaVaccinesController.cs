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
    public class CoronaVaccinesController : Controller
    {
        private readonly HMOContext _context;

        public CoronaVaccinesController(HMOContext context)
        {
            _context = context;
        }

        // GET: CoronaVaccines
        public async Task<IActionResult> Index()
        {
            var hMOContext = _context.CoronaVaccines.Include(c => c.Member);
            return View(await hMOContext.ToListAsync());
        }

        // GET: CoronaVaccines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coronaVaccine = await _context.CoronaVaccines
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.VaccineId == id);
            if (coronaVaccine == null)
            {
                return NotFound();
            }

            return View(coronaVaccine);
        }

        // GET: CoronaVaccines/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity");
            return View();
        }

        // POST: CoronaVaccines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VaccineId,MemberId,DateVaccine,ManufacturerVaccine")] CoronaVaccine coronaVaccine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coronaVaccine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity", coronaVaccine.MemberId);
            return View(coronaVaccine);
        }

        // GET: CoronaVaccines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coronaVaccine = await _context.CoronaVaccines.FindAsync(id);
            if (coronaVaccine == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity", coronaVaccine.MemberId);
            return View(coronaVaccine);
        }

        // POST: CoronaVaccines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VaccineId,MemberId,DateVaccine,ManufacturerVaccine")] CoronaVaccine coronaVaccine)
        {
            if (id != coronaVaccine.VaccineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coronaVaccine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoronaVaccineExists(coronaVaccine.VaccineId))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "ACity", coronaVaccine.MemberId);
            return View(coronaVaccine);
        }

        // GET: CoronaVaccines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coronaVaccine = await _context.CoronaVaccines
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.VaccineId == id);
            if (coronaVaccine == null)
            {
                return NotFound();
            }

            return View(coronaVaccine);
        }

        // POST: CoronaVaccines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coronaVaccine = await _context.CoronaVaccines.FindAsync(id);
            if (coronaVaccine != null)
            {
                _context.CoronaVaccines.Remove(coronaVaccine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoronaVaccineExists(int id)
        {
            return _context.CoronaVaccines.Any(e => e.VaccineId == id);
        }
    }
}
