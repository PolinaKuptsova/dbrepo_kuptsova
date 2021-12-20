using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class TeacherRepository
{
    private MySqlConnection connection;

    public TeacherRepository(MySqlConnection connection)
    {
        this.connection = connection;
    }
    public long GetCount()
    {
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM teachers";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public List<Teacher> GetAll()
    {
        List<Teacher> teachers = new List<Teacher>();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM teachers";
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            teachers.Add(GetTeacher(reader));
        }
        reader.Close();
        return teachers;
    }

    public Teacher GetByName(string name)
    {
        Teacher teacher = new Teacher();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM teachers WHERE name = @name";
        command.Parameters.AddWithValue("@name", name);
        MySqlDataReader reader = command.ExecuteReader();
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

    public bool Update(long id, Teacher teacher)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE teachers SET name = @name, inAdministration = @inAdministration,
            experience = @experience WHERE id = @id";

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", teacher.Name);
        command.Parameters.AddWithValue("@inAdministration", teacher.inAdministration == true ? 1 : 0);
        command.Parameters.AddWithValue("@experience", teacher.experience);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public Teacher GetById(long id)
    {
        Teacher teacher = new Teacher();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM teachers WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        MySqlDataReader reader = command.ExecuteReader();
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
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM teachers WHERE id = @id";
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

    public void Insert(Teacher teacher)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO teachers (name, inAdministration, experience, startedWorking) 
            VALUES (@id, @name, @inAdministration, @experience, @startedWorking);
            SELECT last_insert_id();";
        command.Parameters.AddWithValue("@name", teacher.Name);
        command.Parameters.AddWithValue("@experience", teacher.experience);
        command.Parameters.AddWithValue("@inAdministration", teacher.inAdministration == true ? 1 : 0);
        command.Parameters.AddWithValue("@startedWorking", teacher.startedWorking.ToString("o"));
        command.ExecuteScalar();

    }

    public Teacher GetTeacher(MySqlDataReader reader)
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
