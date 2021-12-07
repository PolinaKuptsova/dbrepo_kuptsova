using System;
public class Student
{
    public long id;
    private string name;
    public int age;
    private string classNumber;
    public double averagePoint;
    public long classTeacherId;
    public Teacher classTeacher;

    public Student(string name, int age, string classNumber, double averagePoint, long classTeacherId)
    {
        Name = name;
        this.age = age;
        ClassNumber = classNumber;
        this.averagePoint = averagePoint;
        this.classTeacherId = classTeacherId;
        ClassNumber = classNumber;
    }

    public Student()
    {
    }

    public string ClassNumber
    {
        get
        {
            return classNumber;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("ClassNumber cannot be empty!");
            }
        }
    }
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Name of a student cannot be empty!");
            }
        }
    }

}