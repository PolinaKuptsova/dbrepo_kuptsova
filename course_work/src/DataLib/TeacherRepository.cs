using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class TeacherRepository
{
    private SqliteConnection connection;

    public TeacherRepository(SqliteConnection connection)
    {
        this.connection = connection;

    }

    public List<Teacher> GetAll()
    {
        List<Teacher> teachers = new List<Teacher>();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM teachers";
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            teachers.Add(GetTeacher(reader));
        }
        reader.Close();
        return teachers;
    }

    public Teacher GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public bool Update(long id, Teacher teacher)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE teachers SET name = $name, inAdministration = $inAdministration,
            experience = $experience WHERE id = $id";

        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", teacher.Name);
        command.Parameters.AddWithValue("$inAdministration", teacher.inAdministration);
        command.Parameters.AddWithValue("$experience", teacher.experience);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public Teacher GetById(long id)
    {
        Teacher teacher = new Teacher();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM teachers WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            teacher = GetTeacher(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return teacher;
    }

    public int DeleteById(long id)
    {
        Teacher teacher = new Teacher();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM teachers WHERE id = $id";
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

    public int Insert(Teacher teacher)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO customers (name, inAdministration, experience, startedWorking) 
            VALUES ($name, $inAdministration, $experience, $startedWorking);
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$name", teacher.Name);
        command.Parameters.AddWithValue("$experience", teacher.experience);
        command.Parameters.AddWithValue("$inAdministration", teacher.inAdministration); // ??
        command.Parameters.AddWithValue("$registrationTime", teacher.startedWorking.ToString("o"));
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

    public Teacher GetTeacher(SqliteDataReader reader)
    {
        Teacher teacher = new Teacher(); 
        teacher.id = long.Parse(reader.GetString(0));
        teacher.Name = reader.GetString(1);
        teacher.inAdministration = int.Parse(reader.GetString(2)) == 1 ? true : false;
        teacher.experience = int.Parse(reader.GetString(3));
        teacher.startedWorking = DateTime.Parse(reader.GetString(4));

        return teacher;
    }
}
