using Npgsql;
using System;
using System.Collections.Generic;
public class DepRepository
{
    private NpgsqlConnection connection;
    public DepRepository(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public List<Department> GetAll()
    {
        List<Department> departments = new List<Department>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM departments";
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            departments.Add(GetDepartment(reader));
        }
        reader.Close();
        return departments;
    }

    public List<Department> GetPage(int pageNumber, long pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }
        List<Department> Departments = new List<Department>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM departments LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Department Department = GetDepartment(reader);
            Departments.Add(Department);
        }
        reader.Close();
        return Departments;
    }

    public int DeleteById(long id)
    {
        Department Department = new Department();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM Departments WHERE id = $id";
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


    public bool Update(long depId, Department department)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE Departments SET name = $name WHERE id = $depId";
        command.Parameters.AddWithValue("$depId", depId);

        command.Parameters.AddWithValue("$name", department.name);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public List<Department> GetDepartmentsById(long depId)
    {
        Department Department = new Department();
        List<Department> Departments = new List<Department>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM departments WHERE name = $name";
        command.Parameters.AddWithValue("$depId", depId);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Department = GetDepartment(reader);
            Departments.Add(Department);
        }
        reader.Close();
        return Departments;
    }

    public int Insert(Department department)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO Departments (name)
            VALUES ($name);
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$name", department.name);

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

    public Department GetDepartment(NpgsqlDataReader reader)
    {
        Department department = new Department();
        department.depId = long.Parse(reader.GetString(0));
        department.name = reader.GetString(1);

        return department;
    }
}