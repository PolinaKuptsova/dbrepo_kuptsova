using System;
using System.Collections.Generic;

static class ConsoleApp
{
    public struct Args
    {
        public StudyingOrg studingOrg;
        public Teacher currentTeacher;

        public Args(StudyingOrg studingOrg, Teacher currentTeacher)
        {
            this.studingOrg = studingOrg;
            this.currentTeacher = currentTeacher;
        }
    }
    private static Teacher ParseArguments(TeacherRepository repo)
    {
        Console.WriteLine("Log in\r\nEnter your full name:");
        string userName = Console.ReadLine();
        ValidateTeacher(userName, repo);
        Teacher currentCustomer = ValidateTeacher(userName, repo);
        return currentCustomer;
    }

    private static Teacher ValidateTeacher(string name, TeacherRepository repo)
    {
        Teacher currentTeacher = repo.GetByName(name);
        if (currentTeacher == null)
        {
            throw new ArgumentException($"Incorrect name. No such teacher '{name}'");
        }
        return currentTeacher;

    }

    private static void ValidateTeacherStatus(Teacher teacher)
    {
        if (teacher.inAdministration == false)
        {
            throw new ArgumentException($"Incorrect status 'not in administration'. You cannot access this data!");
        }
    }

    public static void Run(StudyingOrg studingOrg, string constring)
    {
        Teacher currentTeacher = ParseArguments(studingOrg.teacherRepository);
        Args args = new Args(studingOrg, currentTeacher);
        while (true)
        {
            Console.WriteLine("Enter command:");
            string command = Console.ReadLine();
            try
            {
                switch (command)
                {
                    case "get all students":
                        {
                            ProcessGetAllStudents(args);
                            break;
                        }
                    case "get all teachers":
                        {
                            ProcessGetAllTeachers(args);
                            break;
                        }
                    case "point filter":
                    {
                            ProcessGetStudentsPointFilter(args);
                            break;
                    }
                    case "get teacher`s students":
                    {
                        ProcessGetTeacherStudents(args);
                        break;
                    }
                    case "diagram":
                    {
                        ProcessTestReportDiagram(args.studingOrg);
                        break;
                    }
                    case "report":
                    {
                        ProcessStudentSuccessGraphic(args.studingOrg);
                        break;
                    }
                    case "average points":
                    {
                        ProcessImageOfAvaregeTestPoint(args.studingOrg);
                        break;
                    }
                    case "exit":
                    {
                        Console.WriteLine("Bye!");
                        Environment.Exit(1);
                        break;
                    }
                    case "document":
                    {
                        ProcessGenerateDocument(studingOrg);
                        break;
                    }
                    case "d":
                    {
                        DeleteAll(studingOrg);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }

    private static void ProcessGenerateDocument(StudyingOrg studingOrg)
    {
        string id = ""; 
        int stId;
        Student st = new Student();
        while (true)
        {
            try
            {
                Console.WriteLine("enter student id: ");
                id = Console.ReadLine();
                
                if(!Int32.TryParse(id, out stId))
                {
                    throw new Exception("Incorrect id");
                }

                st = studingOrg.studentRepository.GetById(long.Parse(id)); 
                if(st == null)
                {
                    throw new Exception("No such student");
                }
                break;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        GenerateDocument.GenerateDoc(st,studingOrg);
    }

    private static void DeleteAll(StudyingOrg studingOrg)
    {
        List<Student> sts = studingOrg.studentRepository.GetAll();

        foreach(Student st in sts)
        {
            studingOrg.studentRepository.DeleteById(st.id);
        } 

    }

    // диаграма з тестами та їх кількістю
    private static void ProcessTestReportDiagram(StudyingOrg studingOrg)
    {
        string filepath = "";
        while (true)
        {
            Console.WriteLine("enter file path: ");
            filepath = Console.ReadLine();
            try
            {
                ValidateFilePath(filepath);
                break;
            }
            catch(MyException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        GenerateImage.GenTestReportDiagram(filepath,new ImageData(new Student(), studingOrg) );
        
    }

    // статистика успішності студента
    private static void ProcessStudentSuccessGraphic(StudyingOrg studingOrg)
    {
        string filepath = "";
        string id = ""; int stId;
        Student st = null;
        while (true)
        {
            Console.WriteLine("enter file path: ");
            filepath = Console.ReadLine();
            try
            {
                ValidateFilePath(filepath);            
                Console.WriteLine("enter student id: ");
                id = Console.ReadLine();
                
                if(!Int32.TryParse(id, out stId))
                {
                    throw new Exception("Incorrect id");
                }

                st = studingOrg.studentRepository.GetById(long.Parse(id)); 
                if(st == null)
                {
                    throw new Exception("No such student");
                }
                break;
            }

            catch(MyException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        ImageData d = new ImageData(st, studingOrg);
        GenerateImage.GenStudentSuccessGraphic(filepath, d);
    }

    // тести  та середня оцінка студентів 
    private static void ProcessImageOfAvaregeTestPoint(StudyingOrg org)
    {
        string filepath = "";
        while (true)
        {
            Console.WriteLine("enter file path: ");
            filepath = Console.ReadLine();
            try
            {
                ValidateFilePath(filepath);
                break;
            }
            catch(MyException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        ImageData d = new ImageData(new Student(), org);
        GenerateImage.ImageOfAvaregeTestPoint(filepath, d);

    }

    private static void ValidateFilePath(string filepath)
    {
        if(string.IsNullOrEmpty(filepath))
        {
            throw new MyException("Incorrect file path! It is empty.", new MyExceptionArguments("ConsoleApp", DateTime.Now));
        }
    }

    private static void ProcessGetTeacherStudents(Args args)
    {
        args.studingOrg.studentRepository.GetTeacherStudents(args.currentTeacher);
        if(args.currentTeacher.students == null)
        {
            throw new Exception("No students of teachers in the db yet! Add some and try again!");
        }
        foreach(Student st in args.currentTeacher.students)
        {
            Console.WriteLine(st);
        }
    }

    private static void ProcessGetStudentsPointFilter(Args args)
    {
        throw new NotImplementedException();
    }

    private static void ProcessGetAllTeachers(Args args)
    {
        ValidateTeacherStatus(args.currentTeacher);
        List<Teacher> teachers = args.studingOrg.teacherRepository.GetAll();
        if(teachers == null)
        {
            throw new MyException("No teachers in the db yet! Add some and try again!", new MyExceptionArguments("ConsoleApp", DateTime.Now));
        }
        foreach(Teacher t in teachers)
        {
            Console.WriteLine(t);
        }
    }

    private static void ProcessGetAllStudents(Args args) // all students
    {
        List<Student> students = args.studingOrg.studentRepository.GetAll();
        if(students == null)
        {
            throw new MyException($"No students in the data base", new MyExceptionArguments("Console App", DateTime.Now));
        }

        foreach(Student st in students)
        {
            Console.WriteLine(st);
        }

    }

}