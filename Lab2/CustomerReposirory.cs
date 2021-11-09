using System;
using System.Collections.Generic;
using Npgsql;

public class CustomerRepository
{
    private NpgsqlConnection connection;

    public CustomerRepository(NpgsqlConnection connection)
    {
        this.connection = connection;

    }

    public long GetCount()
    {
        NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM customers";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public int GetTotalPages(long pageSize)
    {
        int totalPages = (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        return totalPages == 0 ? 1 : totalPages;
    }

    public Customer GetByUserName(string userName)
    {
        Customer customer = new Customer();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM customers WHERE userName = $userName";
        command.Parameters.AddWithValue("$username", userName);
        NpgsqlDataReader reader = command.ExecuteReader();
        customer = GetCustomer(reader);
        reader.Close();
        return customer;
    }

    public List<Customer> GetAll()
    {
        List<Customer> customers = new List<Customer>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM customers";
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            customers.Add(GetCustomer(reader));
        }
        reader.Close();
        return customers;
    }

    public List<Customer> GetPage(int pageNumber, long pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }
        List<Customer> Customers = new List<Customer>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM customers LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Customer Customer = GetCustomer(reader);
            Customers.Add(Customer);
        }
        reader.Close();
        return Customers;
    }

    public bool Update(long id, Customer customer, bool passwordChanged)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE customers SET userName = $userNname, phoneNumber = $phoneNumber, 
            password = $password, address = $address WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        if (passwordChanged)
        {
            customer.password = Authentication.GetHash(customer.password);
        }
        command.Parameters.AddWithValue("$username", customer.userName);
        command.Parameters.AddWithValue("$password", customer.password);
        command.Parameters.AddWithValue("$phoneNumber", customer.phoneNumber);
        command.Parameters.AddWithValue("$address", customer.address);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public Customer GetById(long id)
    {
        Customer customer = new Customer();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM customers WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            customer = GetCustomer(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return customer;
    }

    public int DeleteById(long id)
    {
        Customer customer = new Customer();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM customers WHERE id = $id";
        command.Parameters.AddWithValue("$customer_id", id);
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
    public int Insert(Customer customer)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO customers (password, phoneNumber, address, userName, status) 
            VALUES ($password, $phoneNumber, $address, $userName, $status);
            
            SELECT last_insert_rowid();
            ";
        string hash = Authentication.GetHash(customer.password);
        command.Parameters.AddWithValue("$password", hash);
        command.Parameters.AddWithValue("$phoneNumber", customer.phoneNumber);
        command.Parameters.AddWithValue("$address", customer.address);
        command.Parameters.AddWithValue("$userName", customer.userName);
        command.Parameters.AddWithValue("$userStatus", customer.status);
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
    public Customer GetCustomer(NpgsqlDataReader reader)
    {
        Customer customer = new Customer();
        customer.id = long.Parse(reader.GetString(0));
        customer.phoneNumber = reader.GetString(3);
        customer.address = reader.GetString(4);
        customer.userName = reader.GetString(5);
        customer.status = int.Parse(reader.GetString(6)) == 1 ? "admin" : "user";

        return customer;
    }
}