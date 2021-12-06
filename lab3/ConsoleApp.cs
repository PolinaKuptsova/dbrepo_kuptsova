using System;
using System.Collections.Generic;
static class ProcessArguments
{
    private static Customer ParseArguments(CustomerRepository repo)
    {
        Console.WriteLine("Log in\r\nEnter your user name:");
        string userName = Console.ReadLine();

        ValidateCustomer(userName, repo);
        Customer currentCustomer = ValidateCustomer(userName, repo);

        return currentCustomer;
    }

    private static Customer ValidateCustomer(string userName, CustomerRepository repo)
    {
        Customer currentCustomer = repo.GetByUserName(userName);
        if (currentCustomer == null)
        {
            throw new ArgumentException($"Incorrect user name '{userName}'");
        }
        return currentCustomer;

    }

    public static void Run(ShopService shopService)
    {
        Customer currentCustomer = ParseArguments(shopService.customerRepository);
        while (true)
        {
            Console.WriteLine("Enter command:\r\n");
            string command = Console.ReadLine();
            try
            {
                switch (command)
                {
                    case "mylogin":
                        {
                            ProcessMyLogin(currentCustomer, shopService);
                            break;
                        }
                    case "myorders":
                        {
                            ProcessMyOrders(currentCustomer, shopService);
                            break;
                        }
                    case "makeorder":
                        {
                            ProcessMakeOrder(currentCustomer, shopService);
                            break;
                        }
                    case "updateorder":
                        {
                            ProcessUpdateOrder(currentCustomer, shopService);
                            break;
                        }
                    case "deleteorder":
                        {
                            ProcessDeleteOrder(currentCustomer, shopService);
                            break;
                        }
                    case "logout":
                        {
                            break;
                        }
                    case "getallcustomers":
                        {
                            ProcessGetAllCustomers(currentCustomer, shopService);
                            break;
                        }
                    case "getallorders":
                        {
                            ProcessGetAllOrders(currentCustomer, shopService);
                            break;
                        }
                    case "getallproducts":
                        {
                            ProcessGetAllProducts(currentCustomer, shopService);
                            break;
                        }
                    case "getsortedorders":
                        {
                            ProcessGetSortedOrders(currentCustomer, shopService);
                            break;
                        }
                    case "getproductsinorder":
                        {
                            ProcessGetProductsInOrder(currentCustomer, shopService);
                            break;
                        }
                    case "getordersforlocalpostoffice":
                        {
                            ProcessGetOrdersForLocalPostOffice(currentCustomer, shopService);
                            break;
                        }
                    default:
                        {
                            throw new Exception("Incorrect command. Try again!");
                        }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }

    private static void ProcessGetOrdersForLocalPostOffice(Customer currentCustomer, ShopService shopService)
    {
        if (currentCustomer.status == "user")
        {
            throw new Exception("You cannot access. Incorrect status");
        }
        Console.WriteLine("Enter customer id  - ");
        string idValue = Console.ReadLine();
        int id;
        if (!Int32.TryParse(idValue, out id))
        {
            throw new ArgumentException($"Incorrect value - '{id}' for order id");
        }
        id = int.Parse(idValue);

        Customer customer = shopService.customerRepository.GetById(id);
        if (customer == null)
        {
            throw new ArgumentException($"Incorrect value - '{id}'. No such customer. Try another id!");
        }

        Console.WriteLine("Enter search value - ");
        string searchValue = Console.ReadLine();
        if (string.IsNullOrEmpty(searchValue))
        {
            throw new ArgumentException($"Incorrect value for search value. Try again!");
        }

        string[] idValues = new string[2];
        Console.WriteLine("Enter start id - ");
        idValues[0] = Console.ReadLine();

        Console.WriteLine("Enter end id - ");
        idValues[1] = Console.ReadLine();
        int startId;
        int endId;

        for (int i = 0; i < 2; i++)
        {
            if (!Int32.TryParse(idValues[i], out startId) || idValues[i].StartsWith('-'))
            {
                throw new ArgumentException($"Incorrect value - '{idValues[i]}'. Try another id!");
            }
        }

        startId = int.Parse(idValues[0]);
        endId = int.Parse(idValues[1]);

        List<Order> orders = shopService.orderRepository.GetOrdersForLocalPostOffice(id, searchValue, startId, endId);
        if (orders == null)
        {
            throw new Exception("No orders gor local post office!");
        }
        foreach (Order or in orders)
        {
            Console.WriteLine(or);
        }
    }

    private static void ProcessGetProductsInOrder(Customer currentCustomer, ShopService shopService)
    {
        Console.WriteLine("Choose order id ");
        string valueId = Console.ReadLine();
        int id;
        if (!Int32.TryParse(valueId, out id))
        {
            throw new ArgumentException($"Incorrect value - '{id}' for order id");
        }
        id = int.Parse(valueId);
        Order order = shopService.orderRepository.GetById(id);

        if (order == null)
        {
            throw new ArgumentException($"Incorrect value - '{id}' for order id. Such order does not exsist");
        }

        string[] priceValues = new string[2];
        Console.WriteLine("Enter start price - ");
        priceValues[0] = Console.ReadLine();

        Console.WriteLine("Enter end price - ");
        priceValues[1] = Console.ReadLine();
        double startPrice;
        double endPrice;

        for (int i = 0; i < 2; i++)
        {
            if (!Double.TryParse(priceValues[i], out startPrice) || priceValues[i].StartsWith('-'))
            {
                throw new ArgumentException($"Incorrect value - '{priceValues[i]}'. Try another id!");
            }
        }

        startPrice = int.Parse(priceValues[0]);
        endPrice = int.Parse(priceValues[1]);
        List<Product> products = shopService.productRepository.GetSortedProductsInOrder(id, startPrice, endPrice);
        foreach (Product product in products)
        {
            Console.WriteLine(product);
        }
    }

    private static void ProcessGetSortedOrders(Customer currentCustomer, ShopService shopService)
    {
        Console.WriteLine("Enter sortins values - \r\nEnter start date - ");
        string[] dateValues = new string[2];
        dateValues[0] = Console.ReadLine();
        Console.WriteLine("Enter end date - ");
        dateValues[1] = Console.ReadLine();

        DateTime start, end;

        for (int i = 0; i < 2; i++)
        {
            if (!DateTime.TryParse(dateValues[i], out start))
            {
                throw new ArgumentException($"Incorrect value - '{dateValues[i]}'. Try another id!");
            }
        }

        start = DateTime.Parse(dateValues[0]);
        end = DateTime.Parse(dateValues[1]);

        Console.WriteLine("Enter search value - ");
        string searchValue = Console.ReadLine();
        if (string.IsNullOrEmpty(searchValue))
        {
            throw new ArgumentException($"Incorrect value for search value. Try again!");
        }

        List<Order> orders = shopService.orderRepository.GetSortedCustomerOrders(currentCustomer.id, end, start, searchValue);
        if (orders == null)
        {
            throw new Exception($"User '{currentCustomer.id}' have not made any orders yet!");
        }

        foreach (Order order in orders)
        {
            Console.WriteLine(order);
        }
    }

    private static void ProcessGetAllProducts(Customer currentCustomer, ShopService shopService)
    {
        if (currentCustomer.status == "user")
        {
            throw new Exception("You cannot access. Incorrect status");
        }

        List<Product> products = shopService.productRepository.GetAll();
        if (products == null)
        {
            throw new Exception("The shop is empty. No products");
        }

        foreach (Product p in products)
        {
            Console.WriteLine(p);
        }

    }

    private static void ProcessGetAllOrders(Customer currentCustomer, ShopService shopService)
    {
        if (currentCustomer.status == "user")
        {
            throw new Exception("You cannot access. Incorrect status");
        }

        List<Order> orders = shopService.orderRepository.GetAll();
        if (orders == null)
        {
            throw new Exception("No orders have been done yet. Try again later!");
        }

        foreach (Order or in orders)
        {
            Console.WriteLine(or);
        }
    }

    private static void ProcessGetAllCustomers(Customer currentCustomer, ShopService shopService)
    {
        if (currentCustomer.status == "user")
        {
            throw new Exception("You cannot access. Incorrect status");
        }

        List<Customer> customers = shopService.customerRepository.GetAll();
        if (customers == null)
        {
            throw new Exception("No customers have registrated yet. Try again later!");
        }

        foreach (Customer c in customers)
        {
            Console.WriteLine(c);
        }
    }

    private static void ProcessDeleteOrder(Customer currentCustomer, ShopService shopService)
    {
        ProcessMyOrders(currentCustomer, shopService);
        Console.WriteLine("Choose order id ");
        string valueId = Console.ReadLine();
        int id;
        if (!Int32.TryParse(valueId, out id))
        {
            throw new ArgumentException($"Incorrect value - '{id}' for order id");
        }
        id = int.Parse(valueId);
        Order order = shopService.orderRepository.GetById(id);

        if (order == null)
        {
            throw new ArgumentException($"Incorrect value - '{id}' for order id. Such order does not exsist");
        }
        long res = shopService.orderRepository.DeleteById(id);
    }

    private static void ProcessUpdateOrder(Customer currentCustomer, ShopService shopService)
    {
        ProcessMyOrders(currentCustomer, shopService);
        Console.WriteLine("Choose order id ");
        string valueId = Console.ReadLine();
        int id;
        if (!Int32.TryParse(valueId, out id))
        {
            throw new ArgumentException("Incorrect value for order id");
        }
        id = int.Parse(valueId);
        Order order = shopService.orderRepository.GetById(id);

        if (order == null)
        {
            throw new ArgumentException("Incorrect value for order id. Such order does not exsist");
        }

        Console.WriteLine("Enter new country for order update ");
        string newCountry = Console.ReadLine();
        shopService.orderRepository.Update(id, new Order(order.createdAt, currentCustomer.id, newCountry));


    }

    private static void ProcessMakeOrder(Customer currentCustomer, ShopService shopService)
    {
        Console.WriteLine("Enter shipping country:");
        string country = Console.ReadLine();
        if (string.IsNullOrEmpty(country))
        {
            throw new ArgumentException("Incorrect enter of value");
        }

        Console.WriteLine("Enter product id:");
        string valueId = Console.ReadLine();
        int id;
        if (!Int32.TryParse(valueId, out id))
        {
            throw new ArgumentException("Incorrect value for product id");
        }
        id = int.Parse(valueId);
        Product product = shopService.productRepository.GetById(id);
        if (product == null)
        {
            throw new ArgumentException($"No such item in the shop, with the product id {id}");
        }

        long newId = shopService.orderRepository.Insert(new Order(DateTime.Now, currentCustomer.id, country));
        long res = shopService.purchaseRepository.Insert(new Purchase(newId, id));
    }

    private static void ProcessMyOrders(Customer currentCustomer, ShopService shopService)
    {
        List<Order> myorders = shopService.orderRepository.GetCustomerOrders(currentCustomer.id);
        if (myorders == null)
        {
            throw new Exception("You have not make any orders yet");
        }

        foreach (Order or in myorders)
        {
            Console.WriteLine(or);
        }
    }

    private static void ProcessMyLogin(Customer currentCustomer, ShopService shopService)
    {
        Console.WriteLine($"Welcome, {currentCustomer.userName}\r\n My Login\r\n{currentCustomer}");
    }

}