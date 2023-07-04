namespace DreamsWebApp.ViewModels.CourseVM;

public class CartVM
{
	public int Id { get; set; }
	public int Count { get; set; }
	public string Title { get; set; } = null!;
	public double Price { get; set; }
	public string ImageUrl { get; set; }=null!;
	public string CategoryName { get; set; }= null!;
     public string CourseName { get; set; } = null!;
    public string CatagoryName { get; set; } = null!;
	public string Description { get; set; } = null!;
}