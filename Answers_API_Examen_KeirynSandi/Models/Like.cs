using System;
using System.Collections.Generic;

namespace Answers_API_Examen_KeirynSandi.Models;

public partial class Like
{
    public long LikeId { get; set; }

    public int UserId { get; set; }

    public long AnswerId { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
