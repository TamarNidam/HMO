using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMO.Models;
using HMO.Controllers;
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
               .Select(m => new IndexCoronaDTO
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
            var today = DateOnly.FromDateTime(DateTime.Now);
            DetailsCoronaDTO detailsCorona = new DetailsCoronaDTO
            {
                Details = new IndexCoronaDTO
                {
                    MemberId = member.MemberId,
                    FullName = member.FullName,
                    IdentityCard = member.IdentityCard,
                    NumVaccines = _context.CoronaVaccines.Count(v => v.MemberId == member.MemberId),
                    Status = _context.CoronaViruses.Any(v => v.MemberId == member.MemberId) ?
            (_context.CoronaViruses.Any(v => v.MemberId == member.MemberId && v.DateRecovery > today) ? "SICK" : "RECOVERING") :
            "NEVER BEEN SICK"
                },
                Vaccines = await _context.CoronaVaccines.Where(c => c.MemberId == id)
                            .Select(m => new VaccinationDTO
                            {
                                VaccineId = m.VaccineId,
                                VaccinationCount = _context.CoronaVaccines.Count(v => v.MemberId == m.MemberId && v.DateVaccine <= m.DateVaccine),
                                MemberId = (int)id,
                                MemberName = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.FullName).FirstOrDefault(),
                                MemberIdentityCard = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.IdentityCard).FirstOrDefault(),
                                DateVaccine = (DateOnly)m.DateVaccine,
                                ManufacturerVaccine = m.ManufacturerVaccine
                            }).OrderBy(m => m.VaccinationCount).ToListAsync()
            };

            if(detailsCorona.Details.Status != "NEVER BEEN SICK")
            {
                detailsCorona.Virus = _context.CoronaViruses.Where(c => c.MemberId == id).FirstOrDefault();
            }

            return View(detailsCorona);
        }

        //    private DetailsCoronaDTO? CastingDTO(Member m)
        //    {
        //        var today = DateOnly.FromDateTime(DateTime.Now);
        //        DetailsCoronaDTO detailsCorona = new DetailsCoronaDTO
        //        {
        //            Details = new IndexCoronaDTO
        //            {
        //                MemberId = m.MemberId,
        //                FullName = m.FullName,
        //                IdentityCard = m.IdentityCard,
        //                NumVaccines = _context.CoronaVaccines.Count(v => v.MemberId == m.MemberId),
        //                Status = _context.CoronaViruses.Any(v => v.MemberId == m.MemberId) ?
        //        (_context.CoronaViruses.Any(v => v.MemberId == m.MemberId && v.DateRecovery > today) ? "SICK" : "RECOVERING") :
        //        "NEVER BEEN SICK"
        //            },
        //            Vaccines = getVaccines(m.MemberId);
        //        };

        //        return detailsCorona;
        //    }
        //public async Task<List<VaccinationDTO>> getVaccines(int id)
        //{
        //    var vaccines = await _context.CoronaVaccines.Where(c => c.MemberId == id)
        //            .Select(m => new VaccinationDTO
        //            {
        //                VaccineId = m.VaccineId,
        //                VaccinationCount = _context.CoronaVaccines.Count(v => v.MemberId == m.MemberId && v.DateVaccine <= m.DateVaccine),
        //                MemberId = id,
        //                MemberName = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.FullName).FirstOrDefault(),
        //                MemberIdentityCard = _context.Members.Where(v => v.MemberId == m.MemberId).Select(v => v.IdentityCard).FirstOrDefault(),
        //                DateVaccine = (DateOnly)m.DateVaccine,
        //                ManufacturerVaccine = m.ManufacturerVaccine
        //            }).OrderByDescending(m => m.DateVaccine).ToListAsync();
        //    return vaccines;
        //}

    }
}
