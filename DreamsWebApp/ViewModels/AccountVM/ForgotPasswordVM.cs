using System.ComponentModel.DataAnnotations;

namespace DreamsWebApp.ViewModels.AccountVM;
public class ForgotPasswordVM
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
}