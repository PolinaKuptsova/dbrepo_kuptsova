using System;
using System.IO;
public static class Generator
{
    public static void GenTests(TestRepository testRepo)
    {
        string filePath = "";
        StreamReader reader = new StreamReader(filePath);
        while(true)
        {
            string line = reader.ReadLine();
            if(line == null)
            {
                break;
            }
            string [] info = line.Split(','); 
            Test test = new Test(info[1], double.Parse(info[2]), DateTime.Parse(info[3]), long.Parse(info[4]));
            testRepo.Insert(test);
        }
    }
    public static void GenTeachers(TeacherRepository teacherRepo)
    {
        string filePath = "";
        StreamReader reader = new StreamReader(filePath);
        while(true)
        {
            string line = reader.ReadLine();
            if(line == null)
            {
                break;
            }
            string [] info = line.Split(','); 
            bool inAdministration = (info[2] ==  "1") ? true : false; // ??
            Teacher teacher = new Teacher(info[1], inAdministration, int.Parse(info[3]), DateTime.Parse(info[4]));
            teacherRepo.Insert(teacher);
        }
    }
    public static void GenStudents(StudentRepository studentRepo)
    {
        string filePath = "";
        StreamReader reader = new StreamReader(filePath);
        while(true)
        {
            string line = reader.ReadLine();
            if(line == null)
            {
                break;
            }
            string [] info = line.Split(','); 
            Student student = new Student(info[1], int.Parse(info[2]), info[3], double.Parse(info[4]), int.Parse(info[5]));
            studentRepo.Insert(student);
        }

    }
    public static void GenPassingInfo()
    {
        


    }
}
