using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ASP.NETCoreIdentityCustom.Models;
namespace ASP.NETCoreIdentityCustom.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string? Phone { get; set; }
    public string? Description { get; set; }
    public ReportCard ReportCard { get; set; }
    public Project Project { get; set; }

    public virtual ICollection<TeacherDomaines> TeacherDomaines { get; set; }
    public virtual ICollection<Project> TeacherProjects { get; set; }
}
public class ApplicationRole : IdentityRole
{

}
