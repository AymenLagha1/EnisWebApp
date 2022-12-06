using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using ASP.NETCoreIdentityCustom.Core;
using ASP.NETCoreIdentityCustom.Core.ViewModels;
using ASP.NETCoreIdentityCustom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static ASP.NETCoreIdentityCustom.Core.Constants;



namespace ASP.NETCoreIdentityCustom.Controllers
{
    public class PfeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;



        public PfeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        [Authorize(Roles = $"{Constants.Roles.Teacher},{Constants.Roles.Student}")]
        public async Task<IActionResult> IndexAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId.Value);
            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole[0] == "Student")
                {
                    return RedirectToAction("Student");
                }
                else
                {
                    return RedirectToAction(nameof(Teacher));
                }
            return RedirectToAction("Index","Home");
        }

        [Authorize(Policy = Constants.Policies.RequireStudent)]
        public async Task<IActionResult> StudentAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var project = await _context.Project.Where(p => p.ApplicationUserId == user.Id).FirstOrDefaultAsync();

            if(project == null)
            {
                ViewBag.ProjectDescription = "No project";
                ViewBag.ProjectDomaine = "No project";
                ViewBag.Supervisor = "No Supervisor";
            }
            else
            {
                var projectDomaine = _context.Domaine.Where(d => d.Id == project.DomaineId).First();
                ViewBag.ProjectDescription = project.DescriptionProjet;
                ViewBag.ProjectDomaine = projectDomaine.Name;
                var supervisorId = project.TeacherId;
                if (supervisorId == null)
                {
                    ViewBag.Supervisor = "No Supervisor";
                }
                else
                {
                    var supervisor = await _context.ApplicationUser.FindAsync(supervisorId);
                    ViewBag.Supervisor = supervisor.FirstName + " " +supervisor.LastName;
                }
            }
           
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.Description = user.Description;
            ViewBag.Email = user.Email;

            return View();

        }

        [Authorize(Policy = Constants.Policies.RequireStudent)]
        public async Task<IActionResult> MyProjectAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//current user id
            var project = await _context.Project
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ApplicationUserId == userId);

            var selectListItems = _context.Domaine.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            });
            ViewData["DomaineId"] = new SelectList(selectListItems, "Value", "Text");
            ViewBag.UserID = userId;
            return View(project);
        }

        [Authorize(Policy = Constants.Policies.RequireStudent)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject([Bind("ProjectId,DescriptionProjet,DomaineId,Title")] Project project)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//current user id
            var StudentProject = await _context.Project
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(m => m.ApplicationUserId == userId);
            if (StudentProject == null)
            {
                project.ApplicationUserId = userId;
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("Student");
            }
            else
            {
                var projectId = StudentProject.ProjectId;
                _context.ChangeTracker.Clear();
                project.ApplicationUserId = userId;

                _context.Update(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("Student");
            }
        }

        [Authorize(Policy = "RequireTeacher")]
        public IActionResult Teacher()
        {
            var user = _userManager.GetUserAsync(User);
            ViewBag.FullName = user.Result.FirstName + user.Result.LastName;
            ViewBag.Email = user.Result.Email;
            return View();
        }
       

        [Authorize(Policy = "RequireTeacher")]
        public IActionResult ProjectsList()
        {
            var user = _userManager.GetUserAsync(User);
            var teacherProjects = (from a in _context.ApplicationUser
                                       where a.Id == user.Result.Id
                                       join td in _context.TeacherDomaines
                                       on a.Id equals td.ApplicationUserId
                                       join d in _context.Domaine
                                       on td.DomaineId equals d.Id
                                       join p in _context.Project
                                       on d.Id equals p.DomaineId
                                       where p.TeacherId == null
                                       join s in _context.ApplicationUser
                                       on p.ApplicationUserId equals s.Id
                                       select new {
                                       s.FirstName,
                                       s.LastName,
                                       p.ProjectId,
                                       a.Id,
                                       p.DescriptionProjet,
                                       p.Title,
                                       d.Name
                                       } ).ToList();
            
            ViewBag.TeacherProjects = teacherProjects;

            return View();
        }
        [Authorize(Policy = Constants.Policies.RequireTeacher)]
        [HttpPost]
        public async Task<IActionResult> AcceptProject(int ProjectId,string TeacherId)
        {
            var numberOfAcceptedProjects = _context.Project.Where(p => p.TeacherId == TeacherId).Count();
            if(numberOfAcceptedProjects == 5) 
                {
                    TempData ["Message"] = "You have already chose 5 projects";
                    return RedirectToAction("ProjectsList");
                }
            var project = await _context.Project.FindAsync(ProjectId);
            project.TeacherId = TeacherId;
            _context.Update(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProjectsList");
        }

        [Authorize(Policy = "RequireTeacher")]
        public async Task<IActionResult> DomainesAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var teacherdomaines = (from a in _context.ApplicationUser
                                   where a.Id == userId
                                    join td in _context.TeacherDomaines
                                    on a.Id equals td.ApplicationUserId
                                    join d in _context.Domaine
                                    on td.DomaineId equals d.Id
                                    select d.Name).ToList();

            var domaines = _context.Domaine.ToList();

            var domaineItems = domaines.Select(domaine =>
                new SelectListItem(
                    domaine.Name,
                    domaine.Id.ToString(),
                    teacherdomaines.Any(td => td.Contains(domaine.Name)))).ToList();

            TeacherDomaineViewModel TdModel = new TeacherDomaineViewModel
            {
                ApplicationUser = user,
                Domaines = domaineItems,
            };

            return View(TdModel);
        }



        [Authorize(Policy = Constants.Policies.RequireTeacher)]
        public async Task<IActionResult> AcceptedProjects()
        {
            var user = await _userManager.GetUserAsync(User);

            //var acceptedProjects = (from a in _context.ApplicationUser)

            var acceptedProjects = (from a in _context.ApplicationUser
                                   where a.Id == user.Id
                                   join td in _context.TeacherDomaines
                                   on a.Id equals td.ApplicationUserId
                                   join d in _context.Domaine
                                   on td.DomaineId equals d.Id
                                   join p in _context.Project
                                   on d.Id equals p.DomaineId
                                   where p.TeacherId == user.Id
                                   join s in _context.ApplicationUser
                                   on p.ApplicationUserId equals s.Id
                                   select new
                                   {
                                       s.FirstName,
                                       s.LastName,
                                       p.ProjectId,
                                       a.Id,
                                       p.DescriptionProjet,
                                       p.Title,
                                       d.Name
                                   }).ToList();
            ViewBag.acceptedProjects = acceptedProjects;

            return View();
        }

        [Authorize(Policy = Constants.Policies.RequireTeacher)]
        [HttpPost]
        public async Task<IActionResult> ChangeDomainsAsync(TeacherDomaineViewModel data)
        {
            var user = _context.ApplicationUser.Find(data.ApplicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }

            var teacherdomainesInDb = (from a in _context.ApplicationUser
                                       where a.Id == data.ApplicationUser.Id
                                       join td in _context.TeacherDomaines
                                   on a.Id equals td.ApplicationUserId
                                   join d in _context.Domaine
                                   on td.DomaineId equals d.Id
                                   select d.Name).ToList();

            var domainesToAdd = new List<string>();
            var domainesToDelete = new List<string>();

            foreach (var domaine in data.Domaines)
            {
                var assignedInDb = teacherdomainesInDb.FirstOrDefault(td => td == domaine.Text);
                if (domaine.Selected)
                {
                    if (assignedInDb == null)
                    {
                        domainesToAdd.Add(domaine.Value);
                    }
                }
                else
                {
                    if (assignedInDb != null)
                    {
                        domainesToDelete.Add(domaine.Value);
                    }
                }
            }

            if (domainesToAdd.Any())
            {
                foreach(var domaine in domainesToAdd)
                {
                    await _context.TeacherDomaines.AddAsync(new TeacherDomaines { ApplicationUserId = data.ApplicationUser.Id, DomaineId = int.Parse(domaine) });
                }
                
            }

            if (domainesToDelete.Any())
            {
                foreach (var domaine in domainesToDelete)
                {
                    var TeacherDomaineId = _context.TeacherDomaines.FirstOrDefault(td => (td.DomaineId == int.Parse(domaine)) && (td.ApplicationUserId == data.ApplicationUser.Id)).Id;
                    var teacherDomaineToDelete = await _context.TeacherDomaines.FindAsync(TeacherDomaineId);
                    _context.TeacherDomaines.Remove(teacherDomaineToDelete);

                }
                
               
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Domaines");
        }
    }
}
