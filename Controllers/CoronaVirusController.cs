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
            var virus = await _context.CoronaViruses
               .Select(m => new VirusDTO
               {
                   VirusId = m.VirusId,
                   MemberId = m.MemberId,
                   MemberIdentityCard = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.IdentityCard).FirstOrDefault(),
                   MemberName = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.FullName).FirstOrDefault(),
                   DatePositiveResult =m.DatePositiveResult,
                   DateRecovery = m.DateRecovery
               }).OrderByDescending(m => m.DateRecovery).ToListAsync();

            return View(virus);
        }


        // GET: CoronaVirus/Create
        public IActionResult Create()
        {
            var membersNotInSick = _context.Members
        .Where(m => !_context.CoronaViruses.Any(s => s.MemberId == m.MemberId))
        .Select(m => new { m.MemberId, m.IdentityCard })
        .ToList();

            ViewData["MemberId"] = new SelectList(membersNotInSick, "MemberId", "IdentityCard");

            return View();
        }

        // POST: CoronaVirus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VirusId,MemberId,DatePositiveResult,DateRecovery")] VirusDTO coronaVirus)
        {           
                var maxId = await _context.CoronaViruses.MaxAsync(u => (int?)u.VirusId) ?? 0;
                coronaVirus.VirusId = maxId + 1;
                var CV = new CoronaVirus
                {
                    VirusId = coronaVirus.VirusId,
                    MemberId = coronaVirus.MemberId,
                    DatePositiveResult = coronaVirus.DatePositiveResult,
                    DateRecovery = coronaVirus.DateRecovery
                };
                _context.Add(CV);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "FullName", coronaVirus.MemberId);
            return View(coronaVirus);
        }

        // POST: CoronaVirus/Edit/5
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
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "FullName", coronaVirus.MemberId);
            return View(coronaVirus);
        }

        // GET: CoronaVirus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var m = await _context.CoronaViruses
                .FirstOrDefaultAsync(m => m.VirusId == id);
            if (m == null)
            {
                return NotFound();
            }
            var me = new VirusDTO
            {
                VirusId = m.VirusId,
                MemberId = m.MemberId,
                MemberIdentityCard = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.IdentityCard).FirstOrDefault(),
                MemberName = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.FullName).FirstOrDefault(),
                DatePositiveResult = m.DatePositiveResult,
                DateRecovery = m.DateRecovery
            };

            return View(me);
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
