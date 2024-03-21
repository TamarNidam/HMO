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
    public class CoronaController : Controller
    {
        private readonly HMOContext _context;

        public CoronaController(HMOContext context)
        {
            _context = context;
        }
        // GET: Corona
        public async Task<IActionResult> Index()
        {
            //ViewData["Title"] = "Index";
            var today = DateOnly.FromDateTime(DateTime.Now); 

            var members = await _context.Members
               .Select( m => new IndexCoronaDTO
               {
                   MemberId = m.MemberId,
                   FullName = m.FullName,
                   IdentityCard = m.IdentityCard,
                   NumVaccines = _context.CoronaVaccines.Count(v => v.MemberId == m.MemberId),
                   Status = _context.CoronaViruses.Any(v => v.MemberId == m.MemberId) ?
            (_context.CoronaViruses.Any(v => v.MemberId == m.MemberId && v.DateRecovery > today) ? "SICK" : "RECOVERING") :
            "NEVER BEEN SICK"
               }).OrderBy(m => m.FullName).ToListAsync();

            return View(members);
        }

        // GET: Corona/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



    }
}
