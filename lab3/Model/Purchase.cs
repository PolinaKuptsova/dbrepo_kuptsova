public class Purchase
{
    public long id;
    public long order_id;
    public long product_id;

    public Purchase()
    {
        this.order_id = 0;
        this.product_id = 0;
    }

    public Purchase(long order_id, long product_id)
    {
        this.order_id = order_id;
        this.product_id = product_id;
    }

    public override string ToString()
    {
        return string.Format($"({id}) .");
    }
}