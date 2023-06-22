using System.ComponentModel.DataAnnotations;

namespace DreamsWebApp.ViewModels.AccountVM;
public class StudentRegisterVM
{

	[Required, MaxLength(25), MinLength(3)]
	public string UserName { get; set; } = null!;
	[Required, MinLength(3)]
	public string Name { get; set; } = null!;
	[Required, MinLength(3)]
	public string Surname { get; set; } = null!;
	[EmailAddress, Required]
	public string Email { get; set; } = null!;
	[Required, DataType(DataType.Password), MinLength(8)]
	public string Password { get; set; } = null!;
}
