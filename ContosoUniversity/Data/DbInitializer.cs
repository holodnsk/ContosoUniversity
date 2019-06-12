using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.Student.Any())
            {
                return;   // DB has been seeded
            }

            var students = new Student[]
            {
                new Student { FirstMidName = "Сергей",   LastName = "Сидоров", EnrollmentDate = DateTime.Parse("2010-09-01") },
                new Student { FirstMidName = "Алексей", LastName = "Петров", EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Александр",   LastName = "Иванов", EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstMidName = "Елена",    LastName = "Матвеева", EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Витория",      LastName = "Степаненко", EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Виктор",    LastName = "Лытысов", EnrollmentDate = DateTime.Parse("2011-09-01") },
                new Student { FirstMidName = "Иван",    LastName = "Васильков", EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstMidName = "Юлия",     LastName = "Оливетто", EnrollmentDate = DateTime.Parse("2005-09-01") }
            };

            foreach (Student s in students)
            {
                context.Student.Add(s);
            }
            context.SaveChanges();

            var instructors = new Instructor[]
            {
                new Instructor { FirstMidName = "Масим",     LastName = "Аберкромбель", HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstMidName = "Игорь",    LastName = "Факоров", HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstMidName = "Василий",   LastName = "Харуй", HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstMidName = "Генадий", LastName = "Капор", HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstMidName = "Александра",   LastName = "Женг", HireDate = DateTime.Parse("2004-02-12") }
            };

            foreach (Instructor i in instructors)
            {
                context.Instructors.Add(i);
            }
            context.SaveChanges();

            var departments = new Department[]
            {
                new Department { Name = "Аберкромбель",     Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = instructors.Single( i => i.LastName == "Аберкромбель").ID },
                new Department { Name = "Факоров", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = instructors.Single( i => i.LastName == "Факоров").ID },
                new Department { Name = "Харуй", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = instructors.Single( i => i.LastName == "Харуй").ID },
                new Department { Name = "Капор",   Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = instructors.Single( i => i.LastName == "Капор").ID },
                new Department { Name = "Женг",   Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = instructors.Single( i => i.LastName == "Женг").ID }

            };

            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();

            var courses = new Course[]
            {
                new Course {CourseID = 1050, Title = "Сергей_Сидоров",      Credits = 3, DepartmentID = departments.Single( s => s.Name == "Аберкромбель").DepartmentID},
                new Course {CourseID = 4022, Title = "Алексей_Петров", Credits = 3, DepartmentID = departments.Single( s => s.Name == "Аберкромбель").DepartmentID},
                new Course {CourseID = 4041, Title = "Александр_Иванов", Credits = 3, DepartmentID = departments.Single( s => s.Name == "Факоров").DepartmentID},
                new Course {CourseID = 1045, Title = "Елена_Матвеева",       Credits = 4, DepartmentID = departments.Single( s => s.Name == "Харуй").DepartmentID},
                new Course {CourseID = 3141, Title = "Витория_Степаненко",   Credits = 4, DepartmentID = departments.Single( s => s.Name == "Харуй").DepartmentID},
                new Course {CourseID = 2021, Title = "Виктор_Лытысов",    Credits = 3, DepartmentID = departments.Single( s => s.Name == "Капор").DepartmentID},
                new Course {CourseID = 2042, Title = "Иван_Васильков",     Credits = 4, DepartmentID = departments.Single( s => s.Name == "Капор").DepartmentID},
                new Course {CourseID = 2043, Title = "Юлия_Оливетто",     Credits = 4, DepartmentID = departments.Single( s => s.Name == "Женг").DepartmentID},
            };

            foreach (Course c in courses)
            {
                context.Course.Add(c);
            }
            context.SaveChanges();

            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment {InstructorID = instructors.Single( i => i.LastName == "Аберкромбель").ID, Location = "Советская 64/1 1 этаж" },
                new OfficeAssignment {InstructorID = instructors.Single( i => i.LastName == "Факоров").ID, Location = "Советская 64/1 1 этаж" },
                new OfficeAssignment {InstructorID = instructors.Single( i => i.LastName == "Харуй").ID,Location = "Советская 64/1 1 этаж" },
                new OfficeAssignment {InstructorID = instructors.Single( i => i.LastName == "Капор").ID,Location = "Советская 64/1 1 этаж" },
                new OfficeAssignment {InstructorID = instructors.Single( i => i.LastName == "Женг").ID,Location = "Советская 64/1 1 этаж" },
            };

            foreach (OfficeAssignment o in officeAssignments)
            {
                context.OfficeAssignments.Add(o);
            }
            context.SaveChanges();

            var courseInstructors = new CourseAssignment[]
            {new CourseAssignment{CourseID = courses.Single(c => c.Title == "Сергей_Сидоров").CourseID,InstructorID = instructors.Single(i => i.LastName == "Аберкромбель").ID},
            new CourseAssignment{CourseID = courses.Single(c => c.Title == "Алексей_Петров").CourseID,InstructorID = instructors.Single(i => i.LastName == "Аберкромбель").ID},
            new CourseAssignment{CourseID = courses.Single(c => c.Title == "Александр_Иванов").CourseID,InstructorID = instructors.Single(i => i.LastName == "Факоров").ID},
            new CourseAssignment{CourseID = courses.Single(c => c.Title == "Елена_Матвеева").CourseID,InstructorID = instructors.Single(i => i.LastName == "Харуй").ID},
            new CourseAssignment{CourseID = courses.Single(c => c.Title == "Витория_Степаненко").CourseID,InstructorID = instructors.Single(i => i.LastName == "Харуй").ID},
            new CourseAssignment{CourseID = courses.Single(c => c.Title == "Виктор_Лытысов").CourseID,InstructorID = instructors.Single(i => i.LastName == "Капор").ID},
            new CourseAssignment{CourseID = courses.Single(c => c.Title == "Иван_Васильков").CourseID,InstructorID = instructors.Single(i => i.LastName == "Капор").ID},
            new CourseAssignment{CourseID = courses.Single(c => c.Title == "Юлия_Оливетто").CourseID,InstructorID = instructors.Single(i => i.LastName == "Женг").ID}
            };

            foreach (CourseAssignment ci in courseInstructors)
            {
                context.CourseAssignments.Add(ci);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment {StudentID = students.Single(s => s.LastName == "Сидоров").ID,CourseID = courses.Single(c => c.Title == "Сергей_Сидоров" ).CourseID,Grade = Grade.A},
                    new Enrollment {StudentID = students.Single(s => s.LastName == "Петров").ID,CourseID = courses.Single(c => c.Title == "Алексей_Петров" ).CourseID,Grade = Grade.C},
                    new Enrollment {StudentID = students.Single(s => s.LastName == "Иванов").ID,CourseID = courses.Single(c => c.Title == "Александр_Иванов" ).CourseID,Grade = Grade.B},
                    new Enrollment {StudentID = students.Single(s => s.LastName == "Матвеева").ID,CourseID = courses.Single(c => c.Title == "Елена_Матвеева" ).CourseID,Grade = Grade.B},
                    new Enrollment {StudentID = students.Single(s => s.LastName == "Степаненко").ID,CourseID = courses.Single(c => c.Title == "Витория_Степаненко" ).CourseID,Grade = Grade.B},
                    new Enrollment {StudentID = students.Single(s => s.LastName == "Лытысов").ID,CourseID = courses.Single(c => c.Title == "Виктор_Лытысов" ).CourseID,Grade = Grade.B},
                    new Enrollment {StudentID = students.Single(s => s.LastName == "Васильков").ID,CourseID = courses.Single(c => c.Title == "Иван_Васильков" ).CourseID},
                    new Enrollment {StudentID = students.Single(s => s.LastName == "Оливетто").ID,CourseID = courses.Single(c => c.Title == "Юлия_Оливетто").CourseID,Grade = Grade.B}
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollment.Where(
                    s =>
                            s.Student.ID == e.StudentID &&
                            s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollment.Add(e);
                }
            }
            context.SaveChanges();
        }
    }
}