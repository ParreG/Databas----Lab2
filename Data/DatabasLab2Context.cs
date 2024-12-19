using System;
using System.Collections.Generic;
using Databas____Lab2.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Databas____Lab2.Data;

public partial class DatabasLab2Context : DbContext
{
    public DatabasLab2Context()
    {
    }

    public DatabasLab2Context(DbContextOptions<DatabasLab2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=School_Lab2;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927C03C328BFC");

            entity.Property(e => e.ClassName).HasMaxLength(10);
            entity.Property(e => e.StaffIdFk).HasColumnName("StaffId_FK");

            entity.HasOne(d => d.StaffIdFkNavigation).WithMany(p => p.Classes)
                .HasForeignKey(d => d.StaffIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Classes__StaffId__267ABA7A");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7AC7EE751");

            entity.Property(e => e.CourseName).HasMaxLength(35);
            entity.Property(e => e.StaffIdFk).HasColumnName("StaffId_FK");

            entity.HasOne(d => d.StaffIdFkNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.StaffIdFk)
                .HasConstraintName("FK__Courses__StaffId__2C3393D0");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grade__54F87A57DEDF7D88");

            entity.Property(e => e.CourseIdFk).HasColumnName("CourseId_FK");
            entity.Property(e => e.Grade1)
                .HasMaxLength(3)
                .HasColumnName("Grade");
            entity.Property(e => e.StudentIdFk).HasColumnName("StudentId_FK");

            entity.HasOne(d => d.CourseIdFkNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.CourseIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grade__CourseId___2F10007B");

            entity.HasOne(d => d.StudentIdFkNavigation).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grade__StudentId__300424B4");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB17D51DE8B6");

            entity.Property(e => e.JobTitle).HasMaxLength(35);
            entity.Property(e => e.StaffLastName).HasMaxLength(35);
            entity.Property(e => e.StaffName).HasMaxLength(35);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B99B3EB855E");

            entity.Property(e => e.ClassIdFk).HasColumnName("ClassId_FK");
            entity.Property(e => e.StudentLastName).HasMaxLength(35);
            entity.Property(e => e.StudentName).HasMaxLength(35);

            entity.HasOne(d => d.ClassIdFkNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__ClassI__29572725");

            entity.HasMany(d => d.CourseIdFks).WithMany(p => p.StudentIdFks)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseIdFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__StudentCo__Cours__33D4B598"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentIdFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__StudentCo__Stude__32E0915F"),
                    j =>
                    {
                        j.HasKey("StudentIdFk", "CourseIdFk").HasName("PK__StudentC__4820F1822480964B");
                        j.ToTable("StudentCourses");
                        j.IndexerProperty<int>("StudentIdFk").HasColumnName("StudentId_FK");
                        j.IndexerProperty<int>("CourseIdFk").HasColumnName("CourseId_FK");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
