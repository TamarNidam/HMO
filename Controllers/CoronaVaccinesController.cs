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
           
            return View(getVaccines());
        }

        public async  Task<List<VaccinationDTO>> getVaccines()
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
            
            return vaccines;
        }



        // GET: CoronaVaccines/Create
        public IActionResult Create(int? id)
        {
            ViewBag.texti = "good";
            if (id.HasValue)
            {
                ViewData["MemberId"] = id.Value;
            }
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
                    var CV = new CoronaVaccine
                    {
                        VaccineId = coronaVaccine.VaccineId,
                        MemberId = coronaVaccine.MemberId,
                        DateVaccine = coronaVaccine.DateVaccine,
                        ManufacturerVaccine = coronaVaccine.ManufacturerVaccine
                    };
                    //var sql = $"INSERT INTO [CoronaVaccines] (VaccineId,MemberId,DateVaccine,ManufacturerVaccine) VALUES ({coronaVaccine.VaccineId}, '{coronaVaccine.MemberId}', '{(DateOnly)coronaVaccine.DateVaccine}', '{coronaVaccine.ManufacturerVaccine}')";
                    _context.Add(CV);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
               
            }
            ViewBag.texti = "good";
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

            var m = await _context.CoronaVaccines.FindAsync(id);
            if (m == null)
            {
                return NotFound();
            }
            ViewBag.texti = "good";
            var me = new VaccinationDTO
            {
                VaccineId = m.VaccineId,
                MemberId = (int)m.MemberId,
                MemberName = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.FullName).FirstOrDefault(),
                MemberIdentityCard = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.IdentityCard).FirstOrDefault(),
                DateVaccine = (DateOnly)m.DateVaccine,
                ManufacturerVaccine = m.ManufacturerVaccine
            };

            return View(me);
        }

        // POST: CoronaVaccines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VaccineId,MemberId,MemberIdentityCard,,DateVaccine,ManufacturerVaccine")] VaccinationDTO coronaVaccine)
        {
            if (id != coronaVaccine.VaccineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ViewBag.texti = "good";
                try
                {
                    int? memberId = _context.Members
   .Where(m => m.IdentityCard == coronaVaccine.MemberIdentityCard)
   .Select(m => (int?)m.MemberId) // Cast MemberId to nullable int
   .FirstOrDefault();

                    if (memberId == null)
                    {
                        ViewBag.texti = "No member detected";
                    }
                    else if (_context.CoronaVaccines.Count(m => m.MemberId == memberId) >= NUM_VACCINE)
                    {
                        ViewBag.texti = "cant be more vaccine";
                    }
                    else
                    {
                        var me = new CoronaVaccine
                        {
                            VaccineId = coronaVaccine.VaccineId,
                            MemberId = coronaVaccine.MemberId,
                            DateVaccine = coronaVaccine.DateVaccine,
                            ManufacturerVaccine = coronaVaccine.ManufacturerVaccine
                        };
                        _context.Update(me);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
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
                
            }
            ViewBag.texti = "good";
            return View(coronaVaccine);
        }

        // GET: CoronaVaccines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var m = await _context.CoronaVaccines
                .FirstOrDefaultAsync(m => m.VaccineId == id);
            if (m == null)
            {
                return NotFound();
            }
            ViewBag.texti = "good";
            var me = new VaccinationDTO
            {
                VaccineId = m.VaccineId,
                MemberId = (int)m.MemberId,
                MemberName = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.FullName).FirstOrDefault(),
                MemberIdentityCard = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.IdentityCard).FirstOrDefault(),
                DateVaccine = (DateOnly)m.DateVaccine,
                ManufacturerVaccine = m.ManufacturerVaccine
            };
            return View(me);
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
