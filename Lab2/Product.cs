
public class Product
{
    public long product_id;
    public string product_name;
    public int department;
    public double price;

    public Product()
    {
    }

    public Product(string product_name, int department, double price)
    {
        this.product_name = product_name;
        this.department = department;
        this.price = price;
    }

    public override string ToString()
    {
        return string.Format($"({product_id}) {product_name} ");
    }
}