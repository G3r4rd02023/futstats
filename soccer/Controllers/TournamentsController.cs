using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Data.Entities;
using soccer.Helpers;
using soccer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IMatchHelper _matchHelper;


        public TournamentsController(DataContext context, IImageHelper imageHelper,
            IConverterHelper converterHelper,ICombosHelper combosHelper,IMatchHelper matchHelper)

        {
            _context = context;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
            _matchHelper = matchHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Tournaments
                .Include(t => t.Groups)
                .OrderBy(t => t.StartDate)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TournamentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (model.LogoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.LogoFile, "Tournaments");
                }

                var tournament = _converterHelper.ToTournament(model, path, true);
                _context.Add(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tournament tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            TournamentViewModel model = _converterHelper.ToTournamentViewModel(tournament);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TournamentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = model.LogoPath;

                if (model.LogoFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.LogoFile, "Tournaments");
                }

                Tournament tournament = _converterHelper.ToTournament(model, path, false);
                _context.Update(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .Include(t => t.Groups)
                .ThenInclude(t => t.Matches)
                .ThenInclude(t => t.Local)
                .Include(t => t.Groups)
                .ThenInclude(t => t.Matches)
                .ThenInclude(t => t.Visitor)
                .Include(t => t.Groups)
                .ThenInclude(t => t.GroupDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        public async Task<IActionResult> AddGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            var model = new GroupViewModel
            {
                Tournament = tournament,
                TournamentId = tournament.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = await _converterHelper.ToGroupAsync(model, true);
                _context.Add(group);
                await _context.SaveChangesAsync();               
                return RedirectToAction(nameof(Details), new { id = model.TournamentId });
            }

            return View(model);
        }

        public async Task<IActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToGroupViewModel(group);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = await _converterHelper.ToGroupAsync(model, false);
                _context.Update(group);
                await _context.SaveChangesAsync();
                //return RedirectToAction($"{nameof(Details)}/{model.TournamentId}");
                return RedirectToAction(nameof(Details), new { model.TournamentId });
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            //return RedirectToAction($"{nameof(Details)}/{group.Tournament.Id}");
            return RedirectToAction(nameof(Details), new { id = group.Tournament.Id });
        }

        public async Task<IActionResult> DetailsGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Matches)
                .ThenInclude(g => g.Local)
                .Include(g => g.Matches)
                .ThenInclude(g => g.Visitor)
                .Include(g => g.Tournament)
                .Include(g => g.GroupDetails)
                .ThenInclude(gd => gd.Team)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

       
        public async Task<IActionResult> AddGroupDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            var model = new GroupDetailViewModel
            {
                Group = group,
                GroupId = group.Id,
                Teams = _combosHelper.GetComboTeams()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGroupDetail(GroupDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var groupDetail = await _converterHelper.ToGroupDetailAsync(model, true);
                _context.Add(groupDetail);
                await _context.SaveChangesAsync();             
                return RedirectToAction(nameof(DetailsGroup), new { id = model.GroupId });
            }

            model.Teams = _combosHelper.GetComboTeams();
            return View(model);
        }

        public async Task<IActionResult> AddMatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            var model = new MatchViewModel
            {
                Group = group,
                GroupId = group.Id,
                Teams = _combosHelper.GetComboTeams(group.Id)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMatch(MatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.LocalId != model.VisitorId)
                {
                    var matchEntity = await _converterHelper.ToMatchAsync(model, true);
                    _context.Add(matchEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsGroup), new { id = model.GroupId });                    
                }

                ModelState.AddModelError(string.Empty, "El equipo local y el equipo visitante deben ser diferentes.");
            }

            model.Group = await _context.Groups.FindAsync(model.GroupId);
            model.Teams = _combosHelper.GetComboTeams(model.GroupId);
            return View(model);
        }

        public async Task<IActionResult> EditGroupDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupDetail = await _context.GroupDetails
                .Include(gd => gd.Group)
                .Include(gd => gd.Team)
                .FirstOrDefaultAsync(gd => gd.Id == id);
            if (groupDetail == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToGroupDetailViewModel(groupDetail);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroupDetail(GroupDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var groupDetail = await _converterHelper.ToGroupDetailAsync(model, false);
                _context.Update(groupDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsGroup), new { id = model.GroupId });               
            }

            return View(model);
        }
        
        public async Task<IActionResult> EditMatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Group)
                .Include(m => m.Local)
                .Include(m => m.Visitor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToMatchViewModel(match);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMatch(MatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var match = await _converterHelper.ToMatchAsync(model, false);
                _context.Update(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsGroup), new { id = model.GroupId });               
            }

            return View(model);
        }
        
        public async Task<IActionResult> DeleteGroupDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupDetail = await _context.GroupDetails
                .Include(gd => gd.Group)
                .FirstOrDefaultAsync(gd => gd.Id == id);
            if (groupDetail == null)
            {
                return NotFound();
            }

            _context.GroupDetails.Remove(groupDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsGroup), new { id = groupDetail.Group.Id });           
        }

        
        public async Task<IActionResult> DeleteMatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsGroup), new { id = match.Group.Id});          
        }
       
        public async Task<IActionResult> CloseMatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Group)
                .Include(m => m.Local)
                .Include(m => m.Visitor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            var model = new CloseMatchViewModel
            {
                Group = match.Group,
                GroupId = match.Group.Id,
                Local = match.Local,
                LocalId = match.Local.Id,
                MatchId = match.Id,
                Visitor = match.Visitor,
                VisitorId = match.Visitor.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseMatch(CloseMatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _matchHelper.CloseMatchAsync(model.MatchId, model.GoalsLocal.Value, model.GoalsVisitor.Value);
                return RedirectToAction(nameof(DetailsGroup), new { id = model.GroupId });               
            }

            model.Group = await _context.Groups.FindAsync(model.GroupId);
            model.Local = await _context.Teams.FindAsync(model.LocalId);
            model.Visitor = await _context.Teams.FindAsync(model.VisitorId);
            return View(model);
        }

        public async Task<IActionResult> DetailsMatch(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var matchEntity = await _context.Matches
                .Include(m => m.Group)
                .Include(m => m.Local)
                .Include(m => m.Visitor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (matchEntity == null)
            {
                return NotFound();
            }



            return View(matchEntity);
        }
    }
}
