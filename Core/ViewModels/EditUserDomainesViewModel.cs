using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NETCoreIdentityCustom.Core.ViewModels
{
    public class EditUserDomainesViewModel
    {
        public ApplicationUser User { get; set; }

        public IList<SelectListItem> Domaines { get; set; }
    }
}
