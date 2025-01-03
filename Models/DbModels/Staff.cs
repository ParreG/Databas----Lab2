using System;
using System.Collections.Generic;

namespace Databas____Lab2.Models.DbModels;

public partial class Staff
{
    public int StaffId { get; set; }

    public string StaffName { get; set; } = null!;

    public string StaffLastName { get; set; } = null!;

    public string JobTitle { get; set; } = null!;

    public int Salary { get; set; }

    public DateTime EmployedDate { get; set; } //Denna är ny jämfört med Lab3

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
