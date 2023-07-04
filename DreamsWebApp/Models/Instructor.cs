using System.ComponentModel.DataAnnotations;

namespace DreamsWebApp.Models;
public class Instructor:AppUser
{
	public string? About { get; set; } 
	public string? Address { get; set; } 
	public string? ProfileImageName { get; set; } 
	public int? CategoryId { get; set; }
	public Category? Category { get; set; }
	public List<Course>? Courses { get; set; }
	public int? JobId { get; set; }
	public Job? Job { get; set; }
}