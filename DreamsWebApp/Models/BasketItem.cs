using Stripe;

namespace DreamsWebApp.Models;

public class BasketItem
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public int? OrderId { get; set; }
    public Order Order { get; set; }
    public string Title { get; set; }
}
