using ASP.NETCoreIdentityCustom.Areas.Identity.Data;

namespace ASP.NETCoreIdentityCustom.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string DescriptionProjet { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public int DomaineId { get; set; }
        public Domaine? Domaine { get; set; }
        public string? TeacherId { get; set; }
        public ApplicationUser? Teacher { get; set; }

    }
}
