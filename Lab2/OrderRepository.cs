using Npgsql;
using System;
using System.Collections.Generic;

public class OrderRepository
{
    private NpgsqlConnection connection;

    public OrderRepository(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public long GetCount()
    {
        NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM orders";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public long GetCountById(long customerId)
    {
        NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM orders WHERE customer_id = $customer_id";
        command.Parameters.AddWithValue("$customer_id", customerId);
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public int GetTotalPagesById(long pageSize, long customerId)
    {
        int totalPages = (int)Math.Ceiling(this.GetCountById(customerId) / (double)pageSize);
        return totalPages == 0 ? 1 : totalPages;
    }

    public int GetTotalPages(long pageSize)
    {
        int totalPages = (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        return totalPages == 0 ? 1 : totalPages;
    }

    public List<Order> GetAll()
    {
        List<Order> Orders = new List<Order>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders";
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Orders.Add(GetOrder(reader));
        }
        reader.Close();
        return Orders;
    }

    public List<Order> GetPage(int pageNumber, long pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }
        List<Order> orders = new List<Order>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Order order = GetOrder(reader);
            orders.Add(order);
        }
        reader.Close();
        return orders;
    }

    public bool Update(long id, Order order)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE orders SET createdAt = $createdAt,
        customer_id = $customer_id WHERE order_id = $order_id";
        command.Parameters.AddWithValue("$order_id", order.order_id);

        command.Parameters.AddWithValue("$createdAt", order.createdAt.ToString("o"));
        command.Parameters.AddWithValue("$customer_id", order.customer_id);
        int nChanged = command.ExecuteNonQuery();

        return nChanged == 1;
    }

    public List<Order> GetOrdersById(long customer_id)
    {
        Order order = new Order();
        List<Order> orders = new List<Order>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders WHERE customer_id = $customer_id";
        command.Parameters.AddWithValue("$customer_id", customer_id);
        NpgsqlDataReader reader = command.ExecuteReader();
        while(reader.Read())
        {
            order = GetOrder(reader);
            orders.Add(order);
        }
        reader.Close();
        return orders;

    }

    public Order GetById(long id)
    {
        Order order = new Order();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM orders WHERE order_id = $order_id";
        command.Parameters.AddWithValue("$order_id", id);
        NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            order = GetOrder(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return order;

    }
    public int DeleteById(long id)
    {
        Order order = new Order();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM orders WHERE order_id = $order_id";
        command.Parameters.AddWithValue("$order_id", id);
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

    public List<Order> GetCustomerOrders(long customer_id)
    {
        List<Order> orders = new List<Order>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"SELECT order_id, order_time, order_price, customers.customer_id FROM customers, orders 
            WHERE orders.customer_id = customers.customer_id AND customers.customer_id = $customer_id";
        command.Parameters.AddWithValue("$customer_id", customer_id);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Order order = GetOrder(reader);
            orders.Add(order);
        }

        reader.Close();
        return orders;

    }

    public List<Order> GetSortedCustomerOrders(long customer_id, DateTime endDate, DateTime startDate, string searchValue)
    {
        List<Order> orders = new List<Order>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"SELECT order_id, order_time, order_price, customers.customer_id FROM customers, orders 
            WHERE orders.customer_id = customers.customer_id AND customers.customer_id = $customer_id
            AND createdAt BETWEEN $startDate AND $endDate AND country LIKE '%' || $value || '%' ";
        command.Parameters.AddWithValue("$customer_id", customer_id);
        command.Parameters.AddWithValue("$startDate", startDate.ToString("o"));
        command.Parameters.AddWithValue("$endDate", endDate.ToString("o"));
        command.Parameters.AddWithValue("$value",searchValue);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Order order = GetOrder(reader);
            orders.Add(order);
        }

        reader.Close();
        return orders;

    }

    public List<Order> GetOrdersForLocalPostOffice(long customer_id, string searchValue, int startValue, int endValue)
    {
        List<Order> orders = new List<Order>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"SELECT order_id, order_time, customers.customer_id, customers.address FROM customers, orders 
            WHERE orders.customer_id = customers.customer_id AND customers.customer_id = $customer_id
            AND address LIKE '%' || $value || '%'  AND customer_id BETWEEN $startValue AND $endValue" ;
        command.Parameters.AddWithValue("$customer_id", customer_id);
        command.Parameters.AddWithValue("$value",searchValue);
        command.Parameters.AddWithValue("$startValue",startValue);
        command.Parameters.AddWithValue("$endValue",endValue);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Order order = GetOrder(reader);
            orders.Add(order);
        }

        reader.Close();
        return orders;

    }



    public long Insert(Order order)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO orders (createdAt, customer_id) 
            VALUES ($createdAt, $customer_id);
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$createdAt", order.createdAt.ToString("o"));
        command.Parameters.AddWithValue("$customer_id", order.customer_id);
        long newId = (long)command.ExecuteScalar();
        if (newId == 0)
        {
            return 0;
        }
        else
        {
            return newId; ;
        }

    }

    public Order GetOrder(NpgsqlDataReader reader)
    {
        Order order = new Order();
        order.order_id = long.Parse(reader.GetString(0));
        order.createdAt = DateTime.Parse(reader.GetString(1));
        order.customer_id = int.Parse(reader.GetString(3));

        return order;
    }

}