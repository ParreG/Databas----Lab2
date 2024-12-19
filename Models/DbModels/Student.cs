using System;
using System.Collections.Generic;

namespace Databas____Lab2.Models.DbModels;

public partial class Student
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public string StudentLastName { get; set; } = null!;

    public int ClassIdFk { get; set; }

    public virtual Class ClassIdFkNavigation { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<Course> CourseIdFks { get; set; } = new List<Course>();
}
