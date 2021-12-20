using System;
public class Test
{
    public long id;
    private string subject;
    public double point;
    public DateTime date;
    public long studentId;
    public Student student;

    public Test()
    {
    }

    public Test(string subject, double point, DateTime date, long studentId)
    {
        Subject = subject;
        this.point = point;
        this.date = date;
        this.studentId = studentId;
    }

    public Test(long id, string subject, double point, DateTime date, long studentId)
    {
        this.id = id;
        Subject = subject;
        this.point = point;
        this.date = date;
        this.studentId = studentId;
    }

    public string Subject 
    {
        get
        {
            return subject;
        }
        set
        { 
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Subject cannot be empty!");
            }
            subject = value;
        }
    }

    public override string ToString()
    {
        return string.Format($"(Student: {studentId} got '{point}' in {subject} test");
    }
}
