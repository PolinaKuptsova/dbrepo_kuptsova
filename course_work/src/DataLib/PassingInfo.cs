public class PassingInfo
{
    public long id;
    public long studentId;
    public long testId;
    public string classroomNumber;

    public PassingInfo()
    {
    }

    public PassingInfo(long studentId, long testId, string classroomNumber)
    {
        this.studentId = studentId;
        this.testId = testId;
        this.classroomNumber = classroomNumber;
    }

    public PassingInfo(long id, long studentId, long testId, string classroomNumber)
    {
        this.id = id;
        this.studentId = studentId;
        this.testId = testId;
        this.classroomNumber = classroomNumber;
    }
}