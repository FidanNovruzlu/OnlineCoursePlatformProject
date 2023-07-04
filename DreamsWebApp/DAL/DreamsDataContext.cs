using DreamsWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DreamsWebApp.DAL
{
	public class DreamsDataContext : IdentityDbContext<AppUser>
	{
		public DreamsDataContext(DbContextOptions<DreamsDataContext> options) : base(options)
		{

		}

		public DbSet<Slide> Slides { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Widget> Widgets { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Instructor> Instructors { get; set; }
		public DbSet<Master> Masters { get; set; }
		public DbSet<Job> Jobs { get; set; }
		public DbSet<Level> Levels { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<MailSetting> MailSettings { get; set; }
		public DbSet<Setting> Settings { get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<Knowledge> Knowledges { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			builder.Entity<MailSetting>()
				.HasKey(m => m.Id);

		}
	}
}
