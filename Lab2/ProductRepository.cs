using System;
using Npgsql;
using System.Collections.Generic;

public class ProductRepository
{
    private NpgsqlConnection connection;

    public ProductRepository(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public long GetCount()
    {
        NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM products";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public long GetSearchCount(string searchValue)
    {
        NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM products  WHERE product_name LIKE '%' || $value || '%' ";
        command.Parameters.AddWithValue("$value", searchValue);
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public int GetTotalSearchPages(long pageSize, string searchValue)
    {
        int totalPages = (int)Math.Ceiling(this.GetSearchCount(searchValue) / (double)pageSize);
        return totalPages == 0 ? 1 : totalPages;
    }

    public int GetTotalPages(long pageSize)
    {
        int totalPages = (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        return totalPages == 0 ? 1 : totalPages;
    }

    public List<Product> GetAll()
    {
        List<Product> products = new List<Product>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products";
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            products.Add(GetProduct(reader));
        }
        reader.Close();
        return products;
    }

    public List<Product> GetSortedProductsInOrder(long order_id, double low, double high)
    {
        List<Product> products = new List<Product>();

        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"SELECT products.product_id, product_name, price
            FROM products, orders, purchases WHERE orders.order_id = purchases.order_id 
            AND purchases.product_id = products.product_id AND orders.order_id = $order_id 
            WHERE price BETWEEN $low AND $high";
        command.Parameters.AddWithValue("$order_id", order_id);
        command.Parameters.AddWithValue("$low", low);
        command.Parameters.AddWithValue("$high", high);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            products.Add(GetProduct(reader));
        }

        reader.Close();
        return products;

    }

    public List<Product> GetPage(int pageNumber, long pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }
        List<Product> products = new List<Product>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Product product = GetProduct(reader);
            products.Add(product);
        }
        reader.Close();
        return products;
    }

    public bool Update(long id, Product product)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE products SET product_name = $product_name WHERE product_id = $product_id";
        command.Parameters.AddWithValue("$product_id", id);

        command.Parameters.AddWithValue("$product_name", product.product_name);
        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }

    public Product GetById(long id)
    {
        Product product = new Product();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM products WHERE product_id = $product_id";
        command.Parameters.AddWithValue("$product_id", id);
        NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            product = GetProduct(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return product;

    }
    public int DeleteById(long id)
    {
        Product product = new Product();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM products WHERE product_id = $product_id";
        command.Parameters.AddWithValue("$product_id", id);
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
    public int Insert(Product product)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO products (product_name, ) 
            VALUES ($product_name, ;
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$product_name", product.product_name);
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

    public Product GetProduct(NpgsqlDataReader reader)
    {
        Product product = new Product();
        product.product_id = long.Parse(reader.GetString(0));
        product.product_name = reader.GetString(1);

        return product;
    }
}
