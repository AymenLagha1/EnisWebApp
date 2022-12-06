using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NETCoreIdentityCustom.Models
{
    public class TeacherDomaineViewModel
    {
        public IList<SelectListItem> Domaines { get; set; }
        public ApplicationUser ApplicationUser { get; set; }   

    }
}
