using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class TestRepository
{
    private SqliteConnection connection;

    public TestRepository(SqliteConnection connection)
    {
        this.connection = connection;

    }

    public List<Test> GetAll()
    {
        List<Test> tests = new List<Test>();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM tests";
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            tests.Add(GetTest(reader));
        }
        reader.Close();
        return tests;
    }


    public bool Update(long id, Test test)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE tests SET subject = $subject, point = $point, date = $date,
            studentId = $studentId WHERE id = $id";

        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$subject", test.Subject);
        command.Parameters.AddWithValue("$age", test.point);
        command.Parameters.AddWithValue("$date", test.date.ToString("o"));
        command.Parameters.AddWithValue("$studentId", test.studentId);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public Test GetById(long id)
    {
        Test test = new Test();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM tests WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = command.ExecuteReader();
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
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM tests WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
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

    public int Insert(Test test)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO customers (subject, point, date, studentId) 
            VALUES ($subject, $point, $date, $studentId);
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$subject", test.Subject);
        command.Parameters.AddWithValue("$point", test.point);
        command.Parameters.AddWithValue("$date", test.date.ToString("o")); 
        command.Parameters.AddWithValue("$studentId", test.studentId); 
        long newId = (long)command.ExecuteScalar();
        if (newId == 0)
        {
            return 0;
        }
        else
        {
            return (int)newId; ;
        }

    }

    public Test GetTest(SqliteDataReader reader)
    {
        Test test = new Test(); 
        test.id = long.Parse(reader.GetString(0));
        test.Subject = reader.GetString(1);
        test.point = double.Parse(reader.GetString(2));
        test.date = DateTime.Parse(reader.GetString(4));
        test.studentId = long.Parse(reader.GetString(5));

        return test;
    }
}
