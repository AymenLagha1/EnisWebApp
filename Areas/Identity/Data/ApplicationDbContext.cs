using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ASP.NETCoreIdentityCustom.Models;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace ASP.NETCoreIdentityCustom.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<ReportCard> ReportCard { get; set; }
    public DbSet<Project> Project { get; set; }
    public DbSet<TeacherDomaines> TeacherDomaines { get; set; }

    public DbSet<Domaine> Domaine { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder

        builder.Entity<ApplicationUser>()
               .HasOne<ReportCard>(b => b.ReportCard)
               .WithOne(i => i.ApplicationUser)
               .HasForeignKey<ReportCard>(b => b.ApplicationUserId);

        builder.Entity<ApplicationUser>()
              .HasOne(b => b.Project)
              .WithOne(i => i.ApplicationUser)
              .HasForeignKey<Project>(b => b.ApplicationUserId);



        //builder.Entity<ApplicationUser>()
        //        .HasMany(p => p.Domaines)
        //        .WithMany(p => p.ApplicationUsers)
        //        .UsingEntity(TeacherDomaines);

        builder.Entity<TeacherDomaines>()
            .HasOne<Domaine>(td => td.Domaine)
            .WithMany(d=>d.TeacherDomaines)
            .HasForeignKey(td => td.DomaineId);

        builder.Entity<TeacherDomaines>()
            .HasOne<ApplicationUser>(td => td.ApplicationUser)
            .WithMany(d => d.TeacherDomaines)
            .HasForeignKey(td => td.ApplicationUserId);

        builder.Entity<Project>()
               .HasOne<ApplicationUser>(td => td.Teacher)
               .WithMany(p => p.TeacherProjects)
               .HasForeignKey(td => td.TeacherId);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }


}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);
    }
}
