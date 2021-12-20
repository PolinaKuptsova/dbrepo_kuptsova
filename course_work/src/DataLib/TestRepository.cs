using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
public class TestRepository
{
    private MySqlConnection connection;

    public TestRepository(MySqlConnection connection)
    {
        this.connection = connection;
    }
    public long GetCount()
    {
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM tests";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public List<Test> GetSTestInSubject(string subject)
    {
        List<Test> tests = new List<Test>();
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM tests WHERE subject = @subject";
        command.Parameters.AddWithValue("@subject", subject);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            tests.Add(GetTest(reader));
        }
        reader.Close();
        return tests;
    }

    public List<Test> GetAll()
    {
        List<Test> tests = new List<Test>();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM tests";
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            tests.Add(GetTest(reader));
        }
        reader.Close();
        return tests;
    }


    public bool Update(long id, Test test)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE tests SET subject = @subject, point = @point, date = @date,
            studentId = @studentId WHERE id = @id";

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@subject", test.Subject);
        command.Parameters.AddWithValue("@age", test.point);
        command.Parameters.AddWithValue("@date", test.date.ToString("o"));
        command.Parameters.AddWithValue("@studentId", test.studentId);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public Test GetById(long id)
    {
        Test test = new Test();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM tests WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        MySqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            test = GetTest(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return test;
    }

    public int DeleteById(long id)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM tests WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        int nChanged = command.ExecuteNonQuery();
        if (nChanged == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public void Insert(Test test)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO tests (subject, point, date, studentId) 
            VALUES (@subject, @point, @date, @studentId);
            SELECT last_insert_id();";
        command.Parameters.AddWithValue("@subject", test.Subject);
        command.Parameters.AddWithValue("@point", test.point);
        command.Parameters.AddWithValue("@date", test.date.ToString("o"));
        command.Parameters.AddWithValue("@studentId", test.studentId);
        command.ExecuteScalar();

    }

    public List<Test> GetStudentsTests(long id)
    {
        List<Test> tests = new List<Test>();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT tests.id , subject, tests.point, date, tests.studentId FROM tests, students, passingInfos
        WHERE tests.studentId = students.id AND students.id = @id AND passingInfos.studentId = students.id";
        command.Parameters.AddWithValue("@id", id);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            tests.Add(GetTest(reader));
        }
        reader.Close();
        return tests;

    }

    public Test GetTest(MySqlDataReader reader)
    {
        Test test = new Test();
        test.id = long.Parse(reader.GetString(0));
        test.Subject = reader.GetString(1);
        test.point = double.Parse(reader.GetString(2));
        test.date = DateTime.Parse(reader.GetString(3));
        test.studentId = long.Parse(reader.GetString(4));

        return test;
    }

    public List<Test> GetSubjectTest(string value)
    {
        List<Test> orders = new List<Test>();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"SELECT * FROM tests WHERE subject LIKE '%' || $value || '%' ";
        command.Parameters.AddWithValue("$value", value);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Test test = GetTest(reader);
            orders.Add(test);
        }

        reader.Close();
        return orders;

    }
}