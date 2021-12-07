using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class StudentRepository
{
    private SqliteConnection connection;

    public StudentRepository(SqliteConnection connection)
    {
        this.connection = connection;

    }

    public List<Student> GetAll()
    {
        List<Student> students = new List<Student>();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM students";
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(GetStudent(reader));
        }
        reader.Close();
        return students;
    }


    public bool Update(long id, Student student)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE Students SET name = $name, age = $age, classNamber = $classNumber,
            averagePoint = $averagepoint, classTeacherId = $classTeacherId WHERE id = $id";

        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", student.Name);
        command.Parameters.AddWithValue("$age", student.age);
        command.Parameters.AddWithValue("$classNumber", student.ClassNumber);
        command.Parameters.AddWithValue("$averagePoint", student.averagePoint);
        command.Parameters.AddWithValue("$classTeacherId", student.classTeacherId);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public Student GetById(long id)
    {
        Student student = new Student();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM students WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            student = GetStudent(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return student;
    }

    public int DeleteById(long id)
    {
        Student Student = new Student();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM students WHERE id = $id";
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

    public int Insert(Student student)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO customers (name, age, classNumber, averagePoint, classTeacherId) 
            VALUES ($name, $age, $classNumber, $averagePoint, $classTeacherId);
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$name", student.Name);
        command.Parameters.AddWithValue("$age", student.age);
        command.Parameters.AddWithValue("$classNumber", student.ClassNumber); 
        command.Parameters.AddWithValue("$averagePoint", student.averagePoint);
        command.Parameters.AddWithValue("$classTeacherId", student.classTeacherId);
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

    public Teacher GetTeacherStudents(Teacher teacher)
    {
        List<Student> students = new List<Student>();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM students WHERE classTeacherId = $classTeacherId";
        command.Parameters.AddWithValue("$classTeacherId", teacher.id);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(GetStudent(reader));
        }
        reader.Close();
        teacher.students = students.ToArray();
        return teacher;

    }

    public Student GetStudent(SqliteDataReader reader)
    {
        Student student = new Student(); 
        student.id = long.Parse(reader.GetString(0));
        student.Name = reader.GetString(1);
        student.age = int.Parse(reader.GetString(2));
        student.ClassNumber = reader.GetString(3);
        student.averagePoint = double.Parse(reader.GetString(4));
        student.classTeacherId = long.Parse(reader.GetString(5));

        return student;
    }
}
