using System;
using System.Collections.Generic;

namespace Databas____Lab2.Models.DbModels;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int? StaffIdFk { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Staff? StaffIdFkNavigation { get; set; }

    public virtual ICollection<Student> StudentIdFks { get; set; } = new List<Student>();
}
