public class StudyingOrg
{
    public TeacherRepository teacherRepository;
    public StudentRepository studentRepository;
    public TestRepository testRepository;
    public PassingInfoRepository passingInfoRepository;

    public StudyingOrg()
    {
    }

    public StudyingOrg(TeacherRepository teacherRepository, StudentRepository studentRepository, 
                            TestRepository testRepository, PassingInfoRepository passingInfoRepository)
    {
        this.teacherRepository = teacherRepository;
        this.studentRepository = studentRepository;
        this.testRepository = testRepository;
        this.passingInfoRepository = passingInfoRepository;
    }
    
}