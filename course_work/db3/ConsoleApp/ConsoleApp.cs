using System;
using System.Diagnostics;
using System.Collections.Generic;
static class ProcessArguments
{
    public struct Args
    {
        public StudingOrg studingOrg;
        public Teacher currentTeacher;

        public Args(StudingOrg studingOrg, Teacher currentTeacher)
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


    public static void Run(StudingOrg studingOrg)
    {
        Teacher currentTeacher = ParseArguments(studingOrg.teacherRepository);
        Args args = new Args(studingOrg, currentTeacher);
        while (true)
        {
            Console.WriteLine("Enter command:\r\n");
            string command = Console.ReadLine();

            try
            {
                switch (command)
                {
                    case "getStudents":
                        {
                            ProcessGetStudents(args);
                            break;
                        }
                    case "getTeachers":
                        {
                            ProcessGetTeachers(args);
                            break;
                        }
                    case "getStudentsPointFilter":
                    {
                            ProcessGetStudentsPointFilter(args);
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

    private static void ProcessGetStudentsPointFilter(Args args)
    {
        throw new NotImplementedException();
    }

    private static void ProcessGetTeachers(Args args)
    {
        ValidateTeacherStatus(args.currentTeacher);
        List<Teacher> teachers = args.studingOrg.teacherRepository.GetAll();
        if(teachers == null)
        {
            throw new Exception("No teachers in the db yet! Add some and try again!");
        }
        foreach(Teacher t in teachers)
        {
            Console.WriteLine(t);
        }
    }

    private static void ProcessGetStudents(Args args)
    {
        args.studingOrg.studentRepository.GetTeacherStudents(args.currentTeacher);
        if(args.currentTeacher.students == null)
        {
            throw new ArgumentException($"No students of the teacher {args.currentTeacher.Name}");
        }

        foreach(Student st in args.currentTeacher.students)
        {
            Console.WriteLine(st);
        }

    }

}