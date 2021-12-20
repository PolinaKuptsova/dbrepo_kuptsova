using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class StudentRepository
{
    private MySqlConnection connection;

    public StudentRepository(MySqlConnection connection)
    {
        this.connection = connection;
    }
    public long GetCount()
    {
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM students";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public List<Student> GetAll()
    {
        List<Student> students = new List<Student>();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM students";
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(GetStudent(reader));
        }
        reader.Close();
        return students;
    }


    public bool Update(long id, Student student)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE students SET name = @name, age = @age, classNamber = @classNumber,
            averagePoint = @averagepoint, classTeacherId = @classTeacherId WHERE id = @id";

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@age", student.age);
        command.Parameters.AddWithValue("@classNumber", student.ClassNumber);
        command.Parameters.AddWithValue("@averagePoint", student.averagePoint);
        command.Parameters.AddWithValue("@classTeacherId", student.classTeacherId);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public Student GetById(long id)
    {
        Student student = new Student();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM students WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        MySqlDataReader reader = command.ExecuteReader();
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

    public void DeleteById(long id)
    {
        Student Student = new Student();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM students WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();

    }
    
    public void Insert(Student student)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO students (name, age, classNumber, averagePoint, classTeacherId) 
            VALUES (@name, @age, @classNumber, @averagePoint, @classTeacherId);
           SELECT last_insert_id();";

        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@age", student.age);
        command.Parameters.AddWithValue("@classNumber", student.ClassNumber); 
        command.Parameters.AddWithValue("@averagePoint", student.averagePoint);
        command.Parameters.AddWithValue("@classTeacherId", student.classTeacherId);
        command.ExecuteScalar();
    }

    public Teacher GetTeacherStudents(Teacher teacher)
    {
        List<Student> students = new List<Student>();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM students WHERE classTeacherId = @classTeacherId";
        command.Parameters.AddWithValue("@classTeacherId", teacher.id);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(GetStudent(reader));
        }
        reader.Close();
        teacher.students = students.ToArray();
        return teacher;

    }

     public List<Student> GetSortedStudents(long order_id, double low, double high)
    {
        List<Student> products = new List<Student>();

        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"  SELECT * FROM students WHERE price BETWEEN $low AND $high";
        command.Parameters.AddWithValue("$low", low);
        command.Parameters.AddWithValue("$high", high);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            products.Add(GetStudent(reader));
        }

        reader.Close();
        return products;

    }

    public Student GetStudent(MySqlDataReader reader)
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
