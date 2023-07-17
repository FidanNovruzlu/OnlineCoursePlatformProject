using System.Composition.Convention;

namespace DreamsWebApp.ViewModels;
public class PaginationVM<T>
{
	public int TotalPage { get; set; }
	public int CurrentPage { get; set; }
	public List<T> Courses { get; set; }
	public List<T> Companies { get; set; }
	public List<T> Categories { get; set; }
	public List<T> Instructors { get; set; }
}
