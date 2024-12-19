using System;
using System.Collections.Generic;

namespace Databas____Lab2.Models.DbModels;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int StaffIdFk { get; set; }

    public virtual Staff StaffIdFkNavigation { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
