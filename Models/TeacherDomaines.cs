using ASP.NETCoreIdentityCustom.Areas.Identity.Data;

namespace ASP.NETCoreIdentityCustom.Models
{
    public class TeacherDomaines
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser  ApplicationUser { get; set; }
        public Domaine Domaine { get; set; }
        public int DomaineId { get; set; }
    }
}
