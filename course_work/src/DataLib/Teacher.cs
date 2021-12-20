using System;
public class Teacher
{
    public long id;
    private string name;
    public bool inAdministration;
    public int experience;
    public DateTime startedWorking;
    public Student [] students;

    public Teacher()
    {
    }

    public Teacher(string name, bool inAdministration, int experience, DateTime startedWorking)
    {
        Name = name;
        this.inAdministration = inAdministration;
        this.experience = experience;
        this.startedWorking = startedWorking;
    }

    public Teacher(long id, string name, bool inAdministration, int experience, DateTime startedWorking)
    {
        this.id = id;
        this.inAdministration = inAdministration;
        this.experience = experience;
        this.startedWorking = startedWorking;
        Name = name;
    }

    public string Name
    {
        get
        {
            return name;
        }
        set
        { 
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Name cannot be empty!");
            }
            name = value;
        }
    }


    public override string ToString()
    {
        return string.Format($"({id}) {name} Experience period {experience}");
    }
}
