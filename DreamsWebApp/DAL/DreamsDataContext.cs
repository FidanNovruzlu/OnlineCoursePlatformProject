using DreamsWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DreamsWebApp.DAL;
public class DreamsDataContext:IdentityDbContext<AppUser>
{
	public DreamsDataContext(DbContextOptions<DreamsDataContext> options):base(options)
	{

	}
	public DbSet<Slide> Slides { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Student> Students { get; set; }
	public DbSet<Widget> Widgets { get; set; }
	public DbSet<Course> Courses { get; set; }
	public DbSet<Instructor> Instructors { get; set; }
}
