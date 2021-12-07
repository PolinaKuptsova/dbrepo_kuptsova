public class StudingOrg
{
    public TeacherRepository teacherRepository;
    public StudentRepository studentRepository;
    public TestRepository testRepository;

    public StudingOrg()
    {
    }

    public StudingOrg(TeacherRepository teacherRepository, StudentRepository studentRepository, TestRepository testRepository)
    {
        this.teacherRepository = teacherRepository;
        this.studentRepository = studentRepository;
        this.testRepository = testRepository;
    }
}