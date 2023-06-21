using System.ComponentModel.DataAnnotations;

namespace DreamsWebApp.ViewModels.AccountVM;
public class LoginVM
{
    [Required, MaxLength(25), MinLength(3)]
    public string UserName { get; set; } = null!;
    [Required, DataType(DataType.Password), MinLength(8)]
    public string Password { get; set; } = null!;
}
