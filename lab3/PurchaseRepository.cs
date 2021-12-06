using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
public class PurchaseRepository
{
    private NpgsqlConnection connection;
    public PurchaseRepository(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public long GetCount()
    {
        NpgsqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM purchases";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public int GetTotalPages(long pageSize)
    {
        int totalPages = (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        return totalPages == 0 ? 1 : totalPages;
    }

    public List<Purchase> GetAll()
    {
        List<Purchase> purchases = new List<Purchase>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM purchases";
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            purchases.Add(GetPurchase(reader));
        }
        reader.Close();
        return purchases;
    }

    public List<Purchase> GetPage(int pageNumber, long pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }
        List<Purchase> purchases = new List<Purchase>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM purchases LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Purchase Purchase = GetPurchase(reader);
            purchases.Add(Purchase);
        }
        reader.Close();
        return purchases;
    }

    public int DeleteById(long id)
    {
        Purchase purchase = new Purchase();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM purchases WHERE id = $id";
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


    public bool Update(long id, Purchase purchase)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE purchases SET id = $id, order_id = $order_id, product_id = $product_id WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        command.Parameters.AddWithValue("$order_id", purchase.order_id);
        command.Parameters.AddWithValue("$product_id", purchase.product_id);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public List<Purchase> GetPurchasesById(long orderId)
    {
        Purchase purchase = new Purchase();
        List<Purchase> purchases = new List<Purchase>();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM purchases WHERE order_id = $order_id";
        command.Parameters.AddWithValue("$order_id", orderId);
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            purchase = GetPurchase(reader);
            purchases.Add(purchase);
        }
        reader.Close();
        return purchases;
    }

    public int Insert(Purchase purchase)
    {
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO purchases (order_id , product_id)
            VALUES ($order_id, $product_id);
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$order_id", purchase.order_id);
        command.Parameters.AddWithValue("$product_id", purchase.product_id);

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

    public Purchase ValidatePurchase(long orderId, long productId)
    {
        Purchase purchase = new Purchase();
        NpgsqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM purchases WHERE order_id = $order_id AND product_id = $product_id";
        command.Parameters.AddWithValue("$order_id", orderId);
        command.Parameters.AddWithValue("$product_id", productId);
        NpgsqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            purchase = GetPurchase(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return purchase;
    }

    public Purchase GetPurchase(NpgsqlDataReader reader)
    {
        Purchase purchase = new Purchase();
        purchase.id = long.Parse(reader.GetString(0));
        purchase.order_id = long.Parse(reader.GetString(1));
        purchase.product_id = long.Parse(reader.GetString(2));

        return purchase;
    }
}