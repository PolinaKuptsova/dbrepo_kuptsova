using Npgsql;
public class ShopService
{
    public NpgsqlConnection connection;
    public ProductRepository productRepository;
    public CustomerRepository customerRepository;
    public OrderRepository orderRepository;
    public PurchaseRepository purchaseRepository;

    public ShopService()
    {
        this.productRepository = new ProductRepository(this.connection);
        this.customerRepository = new CustomerRepository(this.connection);
        this.orderRepository = new OrderRepository(this.connection);
        this.purchaseRepository = new PurchaseRepository(this.connection);
    }

    public ShopService(ProductRepository productRepository, CustomerRepository customerRepository, 
        OrderRepository orderRepository, PurchaseRepository purchaseRepository)
    {
        this.productRepository = productRepository;
        this.customerRepository = customerRepository;
        this.orderRepository = orderRepository;
        this.purchaseRepository = purchaseRepository;
    }
}