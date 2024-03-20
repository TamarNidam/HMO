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
    public class MembersController : Controller
    {
        private readonly HMOContext _context;

        public MembersController(HMOContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                .Select(m => new IndexMemberDTO
                    {
                        MemberId = m.MemberId,
                        FullName = m.FullName,
                        ACity = m.ACity
                    }).ToListAsync();

            return View(members);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }
            
            return View(CastingDTO(member));
        }


        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,FullName,IdentityCard,ACity,AStreet,ANumber,DateBirth,Telephone,MobilePhone")] DetailsMemberDTO member)
        {
            if (ModelState.IsValid)
            {
                var maxId = await _context.Members.MaxAsync(u => (int?)u.MemberId) ?? 0;
                member.MemberId = maxId + 1;
                var memberDTO = Casting(member);
                _context.Add(memberDTO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            
            return View(CastingDTO(member));
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,FullName,IdentityCard,ACity,AStreet,ANumber,DateBirth,Telephone,MobilePhone")] DetailsMemberDTO member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var memberDTO = Casting(member);
                try
                {
                    _context.Update(memberDTO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(CastingDTO(member));
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        //casting from Member to MemberDTO
        private DetailsMemberDTO? CastingDTO(Member member)
        {
            var m = new DetailsMemberDTO
            {
                MemberId = member.MemberId,
                FullName = member.FullName,
                IdentityCard = member.IdentityCard,
                ACity = member.ACity,
                AStreet = member.AStreet,
                ANumber = member.ANumber,
                DateBirth = member.DateBirth,
                Telephone = member.Telephone,
                MobilePhone = member.MobilePhone
            };
            return m;
        }

        //casting from MemberDTO to Member
        private Member? Casting(DetailsMemberDTO member)
        {
            var m = new Member
            {
                MemberId = member.MemberId,
                FullName = member.FullName,
                IdentityCard = member.IdentityCard,
                ACity = member.ACity,
                AStreet = member.AStreet,
                ANumber = member.ANumber,
                DateBirth = member.DateBirth,
                Telephone = member.Telephone,
                MobilePhone = member.MobilePhone
            };
            return m;
        }
    }
}
