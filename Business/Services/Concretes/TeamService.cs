using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Data.RepositoryConcretes;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeamService(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public void AddTeam(Team team)
        {
            if (!team.ImageFile.ContentType.Contains("image/"))
                throw new FileContentTypeException("ImageFile", "File content type error");
            if (team.ImageFile.Length > 2097152)
                throw new FileSizeException("ImageFile", "File size error");
            if (team == null) throw new TeamNullException("", "Team null ola bilmez.");
            string path = _webHostEnvironment.WebRootPath + @"\Upload\Team\" + team.ImageFile.FileName;
            using(FileStream file=new FileStream(path,FileMode.Create))
            {
                team.ImageFile.CopyTo(file);
            }
            team.ImageUrl = team.ImageFile.FileName;
            _repository.Add(team);
            _repository.Commit();
        }

        public void DeleteTeam(int id)
        {
           var existTeam=_repository.Get(x => x.Id == id);
            if (existTeam == null) throw new EntityNotFoundException("", "Team not found");
            string path=_webHostEnvironment.WebRootPath+@"\Upload\Team\"+existTeam.ImageUrl;
            if (!File.Exists(path)) throw new Business.Exceptions.FileNotFoundException("ImageFile", "File not found");
            File.Delete(path);
            _repository.Delete(existTeam);
            _repository.Commit();
        }

        public List<Team> GetAllTeams(Func<Team, bool>? func = null)
        {
            return _repository.GetAll(func);
        }

        public Team GetTeam(Func<Team, bool>? func = null)
        {
            return _repository.Get(func);
        }

        public void UpdateTeam(int id, Team team)
        {
           var olsTeam=_repository.Get(x=>x.Id == id);
            if (olsTeam == null) throw new EntityNotFoundException("", "Entity not foun");
            if (team.ImageFile != null)
            {
                if (!team.ImageFile.ContentType.Contains("image/"))
                    throw new FileContentTypeException("ImageFile", "File content type error");
                if (team.ImageFile.Length > 2097152)
                    throw new FileSizeException("ImageFile", "File size error");
            }
            string path = _webHostEnvironment.WebRootPath + @"\Upload\Team\" + team.ImageFile.FileName;
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                team.ImageFile.CopyTo(file);
            }
            olsTeam.ImageUrl = team.ImageFile.FileName;
            olsTeam.Name = team.Name;
            olsTeam.Description = team.Description;
            _repository.Commit();
        }
    }
}
