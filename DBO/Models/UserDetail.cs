using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBO.Models;

public partial class UserDetail
{
    public int Udid { get; set; }

    public int UserId { get; set; }

    public byte[]? Photo { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? PostCode { get; set; }

    public string? Phone { get; set; }


    public virtual User User { get; set; } = null!;
}
