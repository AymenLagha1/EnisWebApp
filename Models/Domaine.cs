using ASP.NETCoreIdentityCustom.Areas.Identity.Data;

namespace ASP.NETCoreIdentityCustom.Models
{
    public class Domaine
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TeacherDomaines>? TeacherDomaines { get; set; }
    }
}
