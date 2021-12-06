using System.Collections.Generic;
using System;
using System.Xml.Serialization;

public class Customer
{
    public long id;
    public string userName;
    public string phoneNumber;
    public string address;
    public  string status;
    public List<Order> orders;

    public Customer()
    {
    }

    public Customer(long id, string userName, string phoneNumber, string address, string status)
    {
        this.id = id;
        this.userName = userName;
        this.phoneNumber = phoneNumber;
        this.address = address;
        this.status = status;
    }

    public override string ToString()
    {
        return string.Format($"({id}) {userName} {address}.");
    }

}