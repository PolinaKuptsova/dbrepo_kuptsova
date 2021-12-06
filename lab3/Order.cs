using System;
using System.Collections.Generic;
public class Order
{
    public long order_id;
    public DateTime createdAt;    
    public long customer_id;
    private string shippingCountry;
    public List<Product> products;

    public Purchase purchase;

    public string ShippingCountry 
    {
        get
        {
            return shippingCountry;
        }
        set 
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Incorrect value for shipping country. Try again!");
            }
        }
    }

    public Order()
    {
    }

    public Order(DateTime createdAt, long customer_id, string shippingCountry)
    {
        this.createdAt = createdAt;
        this.customer_id = customer_id;
        this.shippingCountry = shippingCountry;
    }

    public override string ToString()
    {
        return string.Format($"({order_id}) ");
    }
}