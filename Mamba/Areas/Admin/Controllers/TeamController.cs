using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            var teams=_teamService.GetAllTeams();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team team)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _teamService.AddTeam(team);
            }
            catch(FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
                return View();
            }
            catch(TeamNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var existteam=_teamService.GetTeam(x=>x.Id == id);
            if(existteam == null) return NotFound();
            return View(existteam);
        }
        [HttpPost]
        public IActionResult Deleteteam(int id)
        {
            var existteam=_teamService.GetTeam(x=> x.Id == id); 
            if(existteam == null) return NotFound();
            try
            {
                _teamService.DeleteTeam(id);
            }
            catch(Business.Exceptions.FileNotFoundException ex)
            {
               return NotFound();
            }
            catch(EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            var team = _teamService.GetTeam(x=>x.Id == id);
            if(team == null) return NotFound();
            return View(team);
        }
        [HttpPost]
        public IActionResult Update(int id, Team team)
        {
            if (!ModelState.IsValid) return View();
            _teamService.UpdateTeam(id, team);
            return RedirectToAction(nameof(Index));
        }
    }
}
