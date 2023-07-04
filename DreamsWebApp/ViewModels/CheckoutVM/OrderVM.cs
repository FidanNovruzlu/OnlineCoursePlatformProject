using DreamsWebApp.Models;
using DreamsWebApp.ViewModels.CourseVM;
using System.ComponentModel.DataAnnotations;

namespace DreamsWebApp.ViewModels.CheckoutVM;

public class OrderVM
{
    [Required]
    public string Address { get; set; }
}
