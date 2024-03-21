using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMO.Models;
using HMO.DTO;

namespace HMO.Controllers
{
    public class CoronaVaccinesController : Controller
    {
        private readonly HMOContext _context;
        private readonly int NUM_VACCINE =4;

        public CoronaVaccinesController(HMOContext context)
        {
            _context = context;
        }

        // GET: CoronaVaccines
        public async Task<IActionResult> Index()
        {
           var vaccines = await _context.CoronaVaccines
                .Select(m => new VaccinationDTO
                
                    
                    {
                        VaccineId = m.VaccineId,
                        VaccinationCount = _context.CoronaVaccines.Count(v => v.MemberId == m.MemberId && v.DateVaccine <= m.DateVaccine),
                        MemberId = (int)m.MemberId,
                        MemberName = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.FullName).FirstOrDefault(),
                        MemberIdentityCard = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.IdentityCard).FirstOrDefault(),
                        DateVaccine = (DateOnly)m.DateVaccine,
                        ManufacturerVaccine = m.ManufacturerVaccine
                }).OrderByDescending(m => m.DateVaccine).ToListAsync();

            return View(vaccines);
        }

        // GET: CoronaVaccines/Create
        public IActionResult Create()
        {
            ViewBag.texti = "good";
            // ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "IdentityCard");
            return View();
        }

            // POST: CoronaVaccines/Create
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VaccineId,MemberId,MemberIdentityCard,DateVaccine,ManufacturerVaccine")] VaccinationDTO coronaVaccine)
        {
            if (ModelState.IsValid)
            {
                ViewBag.texti = "good";
                int ? memberId = _context.Members
    .Where(m => m.IdentityCard == coronaVaccine.MemberIdentityCard)
    .Select(m => (int?)m.MemberId) // Cast MemberId to nullable int
    .FirstOrDefault();

                if (memberId == null)
                {
                    ViewBag.texti = "No member detected";
                }
                else if (_context.CoronaVaccines.Count(m=>m.MemberId == memberId) >= NUM_VACCINE)
                {
                    ViewBag.texti = "cant be more vaccine";
                }
                else
                {
                    coronaVaccine.MemberId = (int)memberId;
                    var maxId = await _context.CoronaVaccines.MaxAsync(u => (int?)u.VaccineId) ?? 0;
                    coronaVaccine.VaccineId = maxId + 1;
                    var sql = $"INSERT INTO [CoronaVaccines] (VaccineId,MemberId,DateVaccine,ManufacturerVaccine) VALUES ({coronaVaccine.VaccineId}, '{coronaVaccine.MemberId}', '{coronaVaccine.DateVaccine}', '{coronaVaccine.ManufacturerVaccine}')";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                    return RedirectToAction(nameof(Index));
                }
               
            }
            
            //ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "FullName", coronaVaccine.MemberId);
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
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "FullName", coronaVaccine.MemberId);
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
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "FullName", coronaVaccine.MemberId);
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
