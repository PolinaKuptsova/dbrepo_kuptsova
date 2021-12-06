using System;
public class History
{
    int id;
    int prodId;
    string operation;
    DateTime createdAt;

    public History()
    {
    }

    public History(int id, int prodId, string operation, DateTime createdAt)
    {
        this.id = id;
        this.prodId = prodId;
        this.operation = operation;
        this.createdAt = createdAt;
    }
}
