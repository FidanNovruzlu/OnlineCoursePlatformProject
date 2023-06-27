﻿using Microsoft.AspNetCore.Identity;

namespace DreamsWebApp.Models;
public class AppUser:IdentityUser
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; }= null!;
	public bool? IsTeacher { get; set; }
    public List<Comment>? Comments { get; set; }
}