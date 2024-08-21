using System;
using System.Collections.Generic;

namespace Answers_API_Examen_KeirynSandi.Models;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public string UserRole1 { get; set; } = null!;

    public bool IsUserSelectable { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
