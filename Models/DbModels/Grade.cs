using System;
using System.Collections.Generic;

namespace Databas____Lab2.Models.DbModels;

public partial class Grade
{
    public int GradeId { get; set; }

    public int CourseIdFk { get; set; }

    public int StudentIdFk { get; set; }

    public string? Grade1 { get; set; }

    public DateOnly? GradeDate { get; set; }

    public virtual Course CourseIdFkNavigation { get; set; } = null!;

    public virtual Student StudentIdFkNavigation { get; set; } = null!;
}
