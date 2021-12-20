using System;
using MySql.Data.MySqlClient;
class Program
{
    static void Main(string[] args)
    {
        string constring = "server=localhost;user=root;database=mydb;";
        MySqlConnection connection = new MySqlConnection(constring);
        
        TeacherRepository teacherRepository = new TeacherRepository(connection);
        StudentRepository studentRepository = new StudentRepository(connection);
        TestRepository testRepository = new TestRepository(connection);
        PassingInfoRepository passingInfoRepository = new PassingInfoRepository(connection);

        StudyingOrg org = new StudyingOrg(
            teacherRepository,
            studentRepository,
            testRepository,
            passingInfoRepository);

            try 
            {
                connection.Open();
                Generator.Run(args, org);
                connection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    }
}