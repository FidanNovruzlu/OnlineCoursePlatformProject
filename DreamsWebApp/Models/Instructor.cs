using System.ComponentModel.DataAnnotations;

namespace DreamsWebApp.Models;
public class Instructor
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Surname { get; set; } = null!;
	public string About { get; set; } = null!;
	public string Phone { get; set; } = null!;
	public string Email { get; set; }=null!;
	public string Address { get; set; } = null!;
	public string ProfileImageName { get; set; } = null!;
	public int CategoryId { get; set; }
	public Category Category { get; set; } = null!;
	public List<Course> Courses { get; set; }
	public int JobId { get; set; }
	public Job Job { get; set; }
}
