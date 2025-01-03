using Databas____Lab2.Data;
using Databas____Lab2.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Linq;

namespace Databas____Lab2
{
    public class Program
    {
        public static string ValidStringInput(string massage)
        {
            while (true)
            {
                Console.WriteLine($"{massage}");
                Console.Write("Ditt svar: ");
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input)) // Kontrollera att inmatningen inte är tom eller bara mellanslag
                {
                    return input; // Returnera den giltiga strängen
                }
                else
                {
                    Console.WriteLine("Fel: Inmatningen får inte vara tom eller en siffra. Försök igen.");
                    Console.WriteLine(massage);
                }
            }
        }
        public static int ValidNumberInput(int min, int max)
        {
            string choosenNumber = Console.ReadLine();
            int choice = 0;
            bool validInput = true;


            while (validInput)
            {
                if (int.TryParse(choosenNumber, out choice) && choice >= min && choice <= max)
                {
                    validInput = false;
                }
                else
                {
                    Console.WriteLine($"Ogiltig inmatning. Välj mellan siffran {min} till {max}");
                    Console.Write("Ditt val: ");
                    choosenNumber = Console.ReadLine();

                }
            }
            return choice;
        }
        public static double ConvertGradeToNumeric(string grade)
        {
            return grade switch
            {
                "A" => 5.0,
                "B" => 4.0,
                "C" => 3.0,
                "D" => 2.0,
                "E" => 1.0,
                "F" => 0.0,
            };
        }

        public static void DisplayStaffInformation(IEnumerable<dynamic> staffList, string category)
        {
            Console.WriteLine();
            Console.WriteLine($"Antal anställda i denna kategori är: {staffList.Count()}");
            Console.WriteLine($"Denna kategor består av {category}");
            Console.WriteLine("Vill du se deras information?");
            string answer = ValidStringInput("Svara med 'Ja' eller 'Nej' ").ToLower();

            while (true)
            {
                if (answer == "ja")
                {
                    foreach (var staff in staffList)
                    {
                        Console.WriteLine($"\nAnställdens namn: {staff.StaffName}\nEfternamn: {staff.StaffLastName}\nJobbtitel: {staff.JobTitle}");
                        Console.WriteLine();
                    }
                    Console.WriteLine("Tryck på Enter för att gå vidare....");
                    Console.ReadLine();
                    
                    break;
                }
                else if (answer == "nej")
                {
                    break;
                }
                else
                {
                    answer = ValidStringInput("Svara med 'Ja' eller 'Nej' ").ToLower();
                }
            }
        } // Denna är ny

        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Hej och välkommen till huvudmenyn!");
                Console.WriteLine("Välj det alternativet du vill genom att skriva tillhörande siffra och tryck på Enter!");
                Console.WriteLine("1. Anställda!");
                Console.WriteLine("2. Elever!");
                Console.WriteLine("3. Betyg!");
                Console.WriteLine("4. Kurser!"); // Denna är ny
                Console.WriteLine("5. Avsluta.");
                Console.Write("Ditt val: ");

                int choice = ValidNumberInput(1, 5);

                switch (choice)
                {
                    case 1:
                        StaffMenu();
                        break;
                    case 2:
                        StudentMenu();
                        break;
                    case 3:
                        GradeMenu();
                        break;

                    case 4:
                        CoursesMenu();
                        break;
                    case 5:
                        Console.WriteLine("Programmet avslutas!");
                        Thread.Sleep(2000);
                        Console.Clear();
                        System.Environment.Exit(0);
                        break;
                }




            }
        }

        public static void StaffMenu() 
        {
            using (var context = new DatabasLab2Context())
            {

                bool openMenu = true;

                while (openMenu)
                {
                    Console.Clear();
                    Console.WriteLine("Vad vill du göra?");
                    Console.WriteLine("1. Visa alla anställda.");
                    Console.WriteLine("2. Lägg till en ny anställd.");
                    Console.WriteLine("3. Visa en viss kategori av alla anställda!");
                    Console.WriteLine("4. Hur många lärare jobbar på de olika avdelningarna?"); //DENNA ÄR NY!
                    Console.WriteLine("5. HuvudMeny");
                    Console.Write("Ditt val: ");

                    int choice = ValidNumberInput(1, 5);

                    switch (choice)
                    {
                        case 1: //Klar
                            Console.Clear();
                            DateTime now = DateTime.Now;
                            Console.WriteLine("Visar alla anställda:");

                            var allStaff = context.Staff
                            .Select(s => new { s.StaffName, s.StaffLastName, s.JobTitle, s.EmployedDate })

                            .ToList();
                            foreach (var staff in allStaff)
                            {
                                Console.WriteLine($"\nAnställdens namn: {staff.StaffName}" +
                                                    $"\nEfternamn: {staff.StaffLastName}" +
                                                    $"\nJobbtitel: {staff.JobTitle}");



                                // DENNA DEL ÄR NY! VISAR NÄR MAN ANSTÄLLDES OCH HUR LÄNGE PERSONEN HAR VARIT ANSTÄLLD!!!
                                int yearsEmployed = now.Year - staff.EmployedDate.Year;

                                if (now < staff.EmployedDate.AddYears(yearsEmployed))
                                {
                                    yearsEmployed--;
                                }

                                Console.WriteLine($"Anställningsdatum: {staff.EmployedDate:yyyy-MM-dd}" +
                                                  $"\nAntal år anställd: {yearsEmployed} år");
                                Console.WriteLine();
                            }

                            break;
                      
                        case 2: //Klar
                            Console.Clear();
                            Console.WriteLine("Nu ska du få lägga till en ny anställd!");

                            
                            string firstName = ValidStringInput("Vad heter anstäldas förnamn?");

                            string lastname = ValidStringInput("Vad heter anställdas efternamn?");

                            string jobTitle = ValidStringInput("Vad har anställden för jobbtitel? ex Lärare");

                            Console.WriteLine("Hur mycket får anställden i lön?");
                            int salary = ValidNumberInput(1, 60000);

                            var newStaff = new Staff
                            {
                                StaffName = firstName,
                                StaffLastName = lastname,
                                JobTitle = jobTitle,
                                Salary = salary
                            };

                            context.Staff.Add(newStaff);
                            context.SaveChanges();

                            Console.WriteLine("Nu är anstälden tillagd!");
                            Console.WriteLine("Du går tillbaka till menyn om: ");

                            for (int i = 3; i > 0; i--)
                            {
                                Console.WriteLine($"{i}");
                                Thread.Sleep(2000);
                            }

                                
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine("Här kan du välja en specifik kategori för att visa all personal bara inom den kategorin!");
                            Console.WriteLine("För att hjälpa dig mer så är detta alla de kategorier som finns!");
                            Console.WriteLine();
                            Console.WriteLine("Jobbtitlar:");
                            Console.WriteLine("IT-samordnare, Lärare, Studievägledare, Projektledare, IT-tekniker,");
                            Console.WriteLine("Bibliotekarie, Skolsköterska, HR-chef, Kurator, Studiekoordinator,");
                            Console.WriteLine("Administratör, Specialpedagog, Rektor, Ekonom, Kökschef,");
                            Console.WriteLine("Elevassistent, Vaktmästare, Vägg-Ansvarig, Städerska.");
                            Console.WriteLine();

                            string jobTitleChoice = ValidStringInput("Vilken av dessa väljer du? Skriv in ditt svar nedan!").ToLower();

                            var jobTitleCategori = context.Staff
                                .Where(j =>  j.JobTitle == jobTitleChoice)
                                .Select(j => new { j.StaffName, j.StaffLastName, j.JobTitle});

                            foreach (var staff in jobTitleCategori)
                            {
                                Console.WriteLine($"\nAnstäldens namn: {staff.StaffName}\n Efternamn: {staff.StaffLastName}\n Jobtitel: {staff.JobTitle}");
                                Console.WriteLine();
                            }
                            Console.WriteLine("Tryck Enter för att gå vidare....");
                            Console.ReadLine();
                            break;

                        case 4: // DENNA CASE ÄR NY!!!!
                            Console.Clear();
                            Console.WriteLine("Nedan kan du se vilka olika kategorier som finns! Skriv den tillhörande siffran,\n" +
                                "och tryck på ENTER för att se hur många som arbetar i den avdelningen!");

                            Console.WriteLine("1. Lärarteamet!"); //Lärare, Specialpedagog, Elevassistent, Bibliotekarie
                            Console.WriteLine("2. IT-Avdelning!"); // It-samordnare, It-Tekniker
                            Console.WriteLine("3. Elevhälsa!"); // Kurator, Skolsköterska, Studievägledare, Studiekoordinator,
                            Console.WriteLine("4. Administration och ledning!"); //Adminstratör, projektledare, HR-chef, Ekonom, Rektor
                            Console.WriteLine("5. Support och drift!"); // Vaktmästare, Vägg-Ansvarig, Städerska, Kökschef
                            Console.Write("Ditt svar: ");

                            int option = ValidNumberInput(1, 5);

                            if (option == 1)
                            {
                                var countStaff = context.Staff
                                     .Where(j => j.JobTitle == "Lärare" ||
                                            j.JobTitle == "Specialpedagog" ||
                                            j.JobTitle == "Elevassistent" ||
                                            j.JobTitle == "Bibliotekarie")
                                    .Select(l => new { l.StaffName, l.StaffLastName, l.JobTitle })
                                    .ToList();

                                DisplayStaffInformation(countStaff, "Lärare, Specialpedagoger, Elevassistenter och Bibliotekarie.");

                            }
                            else if (option == 2)
                            {
                                var itStaff = context.Staff
                                    .Where(j => j.JobTitle == "It-samordnare" ||
                                           j.JobTitle == "It-Tekniker")
                                    .Select(l => new { l.StaffName, l.StaffLastName, l.JobTitle })
                                    .ToList();

                                DisplayStaffInformation(itStaff, "It-samordnare och It-Tekniker.");
                            }
                            else if (option == 3)
                            {
                                var studentHealth = context.Staff
                                    .Where(j => j.JobTitle == "Kurator" ||
                                           j.JobTitle == "Skolsköterska" ||
                                           j.JobTitle == "Studievägledare" ||
                                           j.JobTitle == "Studiekoordinator")
                                    .Select(l => new { l.StaffName, l.StaffLastName, l.JobTitle })
                                    .ToList();

                                DisplayStaffInformation(studentHealth, "Kurator, Skolsköterska, Studievägledare och Studiekoordinator.");
                            }
                            else if (option == 4)
                            {
                                var adminstration = context.Staff
                                    .Where(j => j.JobTitle == "Adminstratör" ||
                                           j.JobTitle == "Projektledare" ||
                                           j.JobTitle == "HR-chef" ||
                                           j.JobTitle == "Ekonom" ||
                                           j.JobTitle == "Rektor")
                                    .Select(l => new { l.StaffName, l.StaffLastName, l.JobTitle })
                                    .ToList();
                                DisplayStaffInformation(adminstration, "Adminstratör, projektledare, HR-chef, Ekonom och Rektorn");
                            }
                            else if (option == 5)
                            {
                                var support = context.Staff
                                    .Where (j => j.JobTitle == "Vaktmästare" ||
                                                 j.JobTitle == "Vägg-Ansvarig" ||
                                                 j.JobTitle == "Städerska" ||
                                                 j.JobTitle == "Kökschef")
                                    .Select ( l => new { l.StaffName, l.StaffLastName, l.JobTitle })
                                    .ToList();
                                DisplayStaffInformation(support, "Lärarteamet");
                            }
                            

                            break;

                        case 5:
                            Console.WriteLine("Skickar dig tillbaka till huvud Menyn!");
                            Console.WriteLine("Laddar...");
                            openMenu = false;
                            Thread.Sleep(3000);
                            Console.Clear();
                            break;
                    }

                }
            }
        }

        public static void StudentMenu()
        {
            bool openMenu = true;
            using (var Context = new DatabasLab2Context())
            { 

                while (openMenu)
                {
                    Console.Clear();
                    Console.WriteLine("Meny för elver!");
                    Console.WriteLine("1. Hämta alla elever!");
                    Console.WriteLine("2. Lägg till nya elever!");
                    Console.WriteLine("3. Hämta alla elever i en viss klass!");
                    Console.WriteLine("4. Tillbaka till huvudmenyn!");
                    
                    Console.Write("Ditt val: ");
                    int choice = ValidNumberInput(1, 4);
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Här får du chansen att välja hur du vill sortera eleverna!");
                            Console.WriteLine("Hur vill du sortera eleverna?");
                            Console.WriteLine("1. Bokstavsordning genom förnamn!");
                            Console.WriteLine("2. Bokstavsorning genom efternamn!");
                            Console.Write("Ditt svar: ");

                            int menuChoice = ValidNumberInput(1, 2);

                            if (menuChoice == 1)
                            {
                                var allStudentsWithClasses = Context.Students
                                    .Join(Context.Classes,
                                          student => student.ClassIdFk,
                                          classInfo => classInfo.ClassId,
                                          (student, classInfo) => new
                                          {
                                              student.StudentName,
                                              student.StudentLastName,
                                              classInfo.ClassName
                                          })
                                    .OrderBy(s => s.StudentName);

                                foreach (var student in allStudentsWithClasses)
                                {
                                    Console.WriteLine($"\nStudent Namn: {student.StudentName}" +
                                                      $"\nStudent Efternamn: {student.StudentLastName}" +
                                                      $"\nKlass: {student.ClassName}\n");
                                }
                            }
                            else if (menuChoice == 2)
                            {
                                var allStudentsWithClasses = Context.Students
                                    .Join(Context.Classes,
                                          student => student.ClassIdFk,
                                          classInfo => classInfo.ClassId,
                                          (student, classInfo) => new
                                          {
                                              student.StudentName,
                                              student.StudentLastName,
                                              classInfo.ClassName
                                          })
                                    .OrderBy(s => s.StudentLastName);

                                foreach (var student in allStudentsWithClasses)
                                {
                                    Console.WriteLine($"\nStudent Namn: {student.StudentName}" +
                                                      $"\nStudent Efternamn: {student.StudentLastName}" +
                                                      $"\nKlass: {student.ClassName}\n");
                                }
                            }

                            Console.WriteLine("Tryck Enter för att fortsätta tillbaka till menyn!");
                            Console.ReadLine();

                            break;

                        case 2:
                            string name = ValidStringInput("Vad heter studentens första namn?");
                            string lastname = ValidStringInput("Vad heter studentens efternamn?");
                            Console.WriteLine();
                            var allClasses = Context.Classes
                                .Select(c => new { c.ClassId, c.ClassName });

                            foreach (var Class in allClasses)
                            {
                                Console.WriteLine($"{Class.ClassId}. {Class.ClassName}");
                            }
                            Console.WriteLine();
                            Console.WriteLine("Vilken av ovanstående klasser ska studenten registreras i? Skriv nummret på klassen");
                            Console.Write("Ditt svar: ");
                            int classId = ValidNumberInput(1, 16);

                            var addNewStudent = new Student
                            {
                                StudentName = name,
                                StudentLastName = lastname,
                                ClassIdFk = classId
                            };

                            Context.Students.Add(addNewStudent);
                            Context.SaveChanges();

                            Console.WriteLine("Tryck Enter för att fortsätta tillbaka till menyn!");
                            Console.ReadLine();
                            break;

                        case 3:
                            Console.WriteLine("Nedan ser du alla klassar som finns:\n");
                            var viewAllClasses = Context.Classes
                                .Select(c => new { c.ClassId, c.ClassName });

                            foreach (var Class in viewAllClasses)
                            {
                                Console.WriteLine($"{Class.ClassId}. {Class.ClassName}");
                            }

                            Console.WriteLine("\nVilken av klassarnas elever vill du se?");
                            Console.WriteLine("Välj genom att skriva nummret på klassen.");
                            Console.Write("Ditt svar: ");
                            int chosenClass = ValidNumberInput(1, 16);

                            var classStudents = Context.Students
                                .Join(Context.Classes,
                                  student => student.ClassIdFk,  
                                  classItem => classItem.ClassId,  
                                  (student, classItem) => new  
                                  {
                                      StudentName = student.StudentName,
                                      StudentLastName = student.StudentLastName,
                                      ClassName = classItem.ClassName,
                                      ClassId = classItem.ClassId
                                  })
                                .Where(s => s.ClassId == chosenClass)  
                                .Select(s => new { s.StudentName, s.StudentLastName, s.ClassName });

                            foreach (var student in classStudents)
                            {
                                Console.WriteLine($"\nStudent Namn: {student.StudentName}\nStudent Efternamn: {student.StudentLastName}\nKlass namn: {student.ClassName}\n");
                            }

                            Console.WriteLine("Tryck Enter för att fortsätta tillbaka till menyn!");
                            Console.ReadLine();
                            break;

                        case 4:
                            Console.WriteLine("Skickar dig tillbaka till huvud Menyn!");
                            Console.WriteLine("Laddar...");
                            openMenu = false;
                            Thread.Sleep(3000);
                            Console.Clear();
                            break;
                    }
                }
            }
        }

        public static void CoursesMenu()
        {
            Console.Clear();

            using (var Context = new DatabasLab2Context())
            {
                var allCourses = Context.Courses
                    .Select(j => new { j.CourseName })
                    .ToList();
                Console.WriteLine("Visar alla kurser: ");
                Console.WriteLine();

                foreach (var course in allCourses)
                {
                    Console.WriteLine($"Kurs namn: {course.CourseName}");
                    Console.WriteLine();
                }

                Console.WriteLine("Tryck Enter för att gå tillbaka till menyn...");
                Console.ReadLine();
            }
        }

        public static void GradeMenu()
        {
            // Betyg meny
            // Hämta alla betyg som sattes senaste månaden, Elevens namn, kursnamn och betyg ska framgå

            // Hämta en lista med alla kurser och det snittbetyg som eleverna fått-
            // -på den kursen samt det högsta och lägsta betyget som någon fått i kursen.
            // Här får användaren direkt upp en lista med alla kurser i databasen,
            // snittbetyget samt det högsta och lägsta betyget för varje kurs.
            
            using (var context = new DatabasLab2Context())
            {
                Console.Clear();
                bool menuOpen = true;

                while (menuOpen)
                {

                
                    Console.WriteLine("Välkommen till Betygs Menyn:");
                    Console.WriteLine("Vad vill du göra?");
                    Console.WriteLine("1. Hämta alla betyg som sattes senaste månaden!");
                    Console.WriteLine("2. Hämta en lista med alla kurser!");
                    Console.WriteLine("3. Gör rent sidan!");
                    Console.WriteLine("4. Till baka till huvudmenyn!");
                    Console.Write("Ditt svar: ");
                    int menuChoice = ValidNumberInput(1, 4);

                    switch (menuChoice)
                    {
                        case 1:
                            Console.WriteLine();
                            DateTime oneMonthAgoDateTime = DateTime.Now.AddMonths(-1);
                            DateOnly oneMonthAgo = DateOnly.FromDateTime(oneMonthAgoDateTime);

                            var allGrades = context.Grades
                                .Join(
                                    context.Students,
                                    grade => grade.StudentIdFk,
                                    student => student.StudentId,
                                    (grade, student) => new
                                    {
                                        Grade = grade.Grade1,
                                        GradeDate = grade.GradeDate,
                                        StudentName = student.StudentName,
                                        studentLastName = student.StudentLastName


                                    })
                                .Where (d => d.GradeDate > oneMonthAgo)
                                    .ToList();

                            Console.WriteLine();
                            foreach (var studentGrade in allGrades)
                            {

                                Console.WriteLine($"Grade: {studentGrade.Grade}\n" +
                                    $"Date: {studentGrade.GradeDate}\n" +
                                    $"Student: {studentGrade.StudentName} {studentGrade.studentLastName}");
                                Console.WriteLine();
                            }

                            break;

                        case 2:
                            Console.WriteLine();
                            var grades = context.Grades.ToList(); // Hämta data till minnet

                            var courseStatistics = grades
                                .GroupBy(g => g.CourseIdFk)
                                .Select(group => new
                                {
                                    CourseId = group.Key,
                                    AverageGrade = group.Average(g => ConvertGradeToNumeric(g.Grade1)),
                                    MaxGrade = group.Max(g => g.Grade1),
                                    MinGrade = group.Min(g => g.Grade1)
                                })
                                .ToList();

                            foreach (var course in courseStatistics)
                            {
                                Console.WriteLine($"Kurs ID: {course.CourseId}");
                                if (course.AverageGrade >= 0 && course.AverageGrade < 1)
                                {
                                    Console.WriteLine($"Snittbetyget: F+");
                                }
                                else if (course.AverageGrade >= 1 && course.AverageGrade < 2)
                                {
                                    Console.WriteLine($"Snittbetyget: E");
                                }
                                else if (course.AverageGrade >= 2 && course.AverageGrade < 3)
                                {
                                    Console.WriteLine($"Snittbetyget: D");
                                }
                                else if (course.AverageGrade >= 3 && course.AverageGrade < 4)
                                {
                                    Console.WriteLine($"Snittbetyget: C");
                                }
                                else if (course.AverageGrade >= 4 && course.AverageGrade < 5)
                                {
                                    Console.WriteLine($"Snittbetyget: B");
                                }
                                else if (course.AverageGrade == 5)
                                {
                                    Console.WriteLine($"Snittbetyget: A");
                                }
                                Console.WriteLine($"Högsta betyget: {course.MaxGrade}");
                                Console.WriteLine($"Minsta betyget: {course.MinGrade}");
                                Console.WriteLine();
                            }
                            break;

                        case 3:
                            Console.Clear();
                            break;
                        case 4:
                            Console.WriteLine("Skickar dig tillbaka till huvud Menyn!");
                            Console.WriteLine("Laddar...");
                            menuOpen = false;
                            Thread.Sleep(3000);
                            Console.Clear();
                            break;
                        
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            MainMenu();
        }
        /*Lista på det som är ny för det individuella projektet i VS jämfört med Labb3!!
         * 
         * Ny DisplayStaffInformation funktion som gör min kod enklare och mer lättläst så jag inte behöver upprepa koden flera gånger.
         * 
         * I MainMenu har jag lagt till ny meny alternativ för att visa alla kurser på nummer 4!
         Den har i sin tu en egen funktion som heter CousesMenu som också är ny!
         *
         * I StaffMenu, alternativ 1 där programmet ska visa anställda har jag lagt in ny funktion för att se hur länge en person har varit anställd.
         För att detta ska vara möjligt uppdaterade jag min databas där jag la in ett anställningsdatum på alla anställda. 
         *
         * Case 4 är också ny i StaffMenu. Det gör det nu möjligt för användaren att se anställda som arbetar inom en specifik del i skolan, som lärare eller ledning.
         För att göra detta skapade jag 5 olika avdelningar i skolan som användaren får välja mellan. Läs mer om det på rad 267 - 272!
         */





    }
}
