using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace ASP.NETCoreIdentityCustom.Models
{

    public class ReportCard
    {
        public int ReportCardId { get; set; }
        public string Url { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

    }
}