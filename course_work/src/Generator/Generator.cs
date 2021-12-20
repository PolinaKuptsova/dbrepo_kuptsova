using System;
using System.IO;

public static class Generator
{
    public static void GenPassingInfo(StudyingOrg org)
    {
        string filePath = "/home/polina/Projects_db/db3/genData/passingInfos.csv";
        string line = "";

        StreamReader reader = new StreamReader(filePath);
        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
            {
                break;
            }
            string[] info = line.Split(',');
            if(info.Length != 4 || info[0] == "0")
            {
                break;
            }
            PassingInfo passInfo = new PassingInfo(long.Parse(info[0]), long.Parse(info[1]), long.Parse(info[2]), info[3]);
            org.passingInfoRepository.Insert(passInfo);
        }
        reader.Close();
    }

    public static void Run(string[] args, StudyingOrg org)
    {
        string [] arg = new string[1];
        arg[0] = "load";
        if (arg[0] == "load")
        {
            // GenTeachers(org);
            GenStudents(org);
            GenTests(org);
            GenPassingInfo(org);
        }
    
    }

    private static void ValidateArgsLength(string[] args)
    {
        if(args.Length != 1)
        {
            throw new MyException("Incorrect request for generation!", new MyExceptionArguments("Generator", DateTime.Now));
        }
    }

    public static void GenTests(StudyingOrg org)
    {
        string filePath = "/home/polina/Projects_db/db3/genData/tests.csv";
        string line = "";
        StreamReader reader = new StreamReader(filePath);
        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
            {
                break;
            }
            
            string[] info = line.Split(',');
            if(info.Length != 5 || info[1] == "")
            {
                break;
            }
            Test test = new Test(long.Parse(info[0]), info[1], double.Parse(info[2]), DateTime.Parse(info[3]), long.Parse(info[4]));
            org.testRepository.Insert(test);
        }
        reader.Close();

    }
    public static void GenTeachers(StudyingOrg org)
    {
        string filePath = "/home/polina/Projects_db/db3/genData/teachers.csv";
        StreamReader reader = new StreamReader(filePath);
        string line = "";
        while (true)
        {
            Console.WriteLine(line);
            if (line == null)
            {
                break;
            }
            string[] info = line.Split(',');
            if(info.Length != 5 || info[1] == "")
            {
                break;
            }
            bool inAdministration = (info[2] == "1") ? true : false;
            int ex =  int.Parse(info[3]); 
            Teacher teacher = new Teacher(long.Parse(info[0]), info[1], inAdministration, ex, DateTime.Parse(info[4]));
            org.teacherRepository.Insert(teacher);
        }
        reader.Close();

    }
    public static void GenStudents(StudyingOrg org)
    {
        string filePath = "/home/polina/Projects_db/db3/genData/students.csv";
        string line = "";

        StreamReader reader = new StreamReader(filePath);
        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
            {
                break;
            }
            string[] info = line.Split(',');
            if(info.Length != 6 || info[1] == "")
            {
                break;
            }
            Student student = new Student(long.Parse(info[0]) , info[1], int.Parse(info[2]), info[3], double.Parse(info[4]), long.Parse(info[5]));
            org.studentRepository.Insert(student);
        }
        reader.Close();
    }
}
