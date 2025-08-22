using DotnetApiGuideline.Sources.Domain.Entities;
using DotnetApiGuideline.Sources.Domain.Enums;
using DotnetApiGuideline.Sources.Domain.Interfaces;
using DotnetApiGuideline.Sources.Domain.ValueObjects;

namespace DotnetApiGuideline.Sources.Infrastructure.Configurations;

public static class SeedDataConfiguration
{
    public static async Task ConfigureSeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        await CleanupAsync(serviceProvider);
        await SeedCustomersAsync(customerRepository);
        await SeedProductsAsync(productRepository);
        await SeedOrdersAsync(orderRepository, customerRepository, productRepository);
    }

    private static async Task CleanupAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

        await DeleteAllOrdersAsync(orderRepository);
        await DeleteAllProductsAsync(productRepository);
        await DeleteAllCustomersAsync(customerRepository);
    }

    private static async Task DeleteAllProductsAsync(IProductRepository productRepository)
    {
        var products = await productRepository.GetProductsAsync();
        foreach (var product in products)
        {
            await productRepository.DeleteProductAsync(product.Id);
        }
    }

    private static async Task SeedProductsAsync(IProductRepository productRepository)
    {
        var products = new List<ProductEntity>
        {
            new(
                name: "iPhone 15 Pro",
                description: "Latest iPhone with Pro features",
                sku: "IP15P-001",
                price: new Money(39900, "THB"),
                stockQuantity: 50
            ),
            new(
                name: "MacBook Air M3",
                description: "Lightweight laptop with M3 chip",
                sku: "MBA-M3-001",
                price: new Money(42900, "THB"),
                stockQuantity: 25
            ),
            new(
                name: "AirPods Pro",
                description: "Wireless earbuds with noise cancellation",
                sku: "APP-001",
                price: new Money(8990, "THB"),
                stockQuantity: 100
            ),
            new(
                name: "iPad Air",
                description: "Powerful tablet for work and play",
                sku: "IPA-001",
                price: new Money(21900, "THB"),
                stockQuantity: 75
            ),
            new(
                name: "Apple Watch Series 9",
                description: "Advanced health and fitness tracking",
                sku: "AWS9-001",
                price: new Money(13900, "THB"),
                stockQuantity: 40
            ),
        };

        await productRepository.CreateProductsAsync(products);
    }

    private static async Task DeleteAllCustomersAsync(ICustomerRepository customerRepository)
    {
        var customers = await customerRepository.GetCustomersAsync();
        foreach (var customer in customers)
        {
            await customerRepository.DeleteCustomerAsync(customer.Id);
        }
    }

    private static async Task SeedCustomersAsync(ICustomerRepository customerRepository)
    {
        var customers = new List<CustomerEntity>
        {
            new()
            {
                Name = "John Smith",
                Email = new Email("john.smith@email.com"),
                Phone = "+66-81-234-5678",
                Address = new Address(
                    "123 Sukhumvit Road",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10110"
                ),
                Tier = CustomerTier.Gold,
            },
            new()
            {
                Name = "Sarah Johnson",
                Email = new Email("sarah.johnson@email.com"),
                Phone = "+66-82-345-6789",
                Address = new Address("456 Silom Road", "Bangkok", "Bangkok", "Thailand", "10500"),
                Tier = CustomerTier.VIP,
            },
            new()
            {
                Name = "Michael Brown",
                Email = new Email("michael.brown@email.com"),
                Phone = "+66-83-456-7890",
                Address = new Address(
                    "789 Thonglor Street",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10110"
                ),
                Tier = CustomerTier.Silver,
            },
            new()
            {
                Name = "Emma Wilson",
                Email = new Email("emma.wilson@email.com"),
                Phone = "+66-84-567-8901",
                Address = new Address(
                    "321 Ratchadaphisek Road",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10400"
                ),
                Tier = CustomerTier.Regular,
            },
            new()
            {
                Name = "David Lee",
                Email = new Email("david.lee@email.com"),
                Phone = "+66-85-678-9012",
                Address = new Address(
                    "654 Phetchaburi Road",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10400"
                ),
                Tier = CustomerTier.Gold,
            },
            new()
            {
                Name = "Lisa Chen",
                Email = new Email("lisa.chen@email.com"),
                Phone = "+66-86-789-0123",
                Address = new Address(
                    "987 Sathorn Road",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10120"
                ),
                Tier = CustomerTier.VIP,
            },
            new()
            {
                Name = "James Anderson",
                Email = new Email("james.anderson@email.com"),
                Phone = "+66-87-890-1234",
                Address = new Address("147 Asoke Road", "Bangkok", "Bangkok", "Thailand", "10110"),
                Tier = CustomerTier.Silver,
            },
            new()
            {
                Name = "Maria Garcia",
                Email = new Email("maria.garcia@email.com"),
                Phone = "+66-88-901-2345",
                Address = new Address(
                    "258 Ploenchit Road",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10330"
                ),
                Tier = CustomerTier.Regular,
            },
            new()
            {
                Name = "Robert Taylor",
                Email = new Email("robert.taylor@email.com"),
                Phone = "+66-89-012-3456",
                Address = new Address(
                    "369 Rama IV Road",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10110"
                ),
                Tier = CustomerTier.Gold,
            },
            new()
            {
                Name = "Jennifer Davis",
                Email = new Email("jennifer.davis@email.com"),
                Phone = "+66-90-123-4567",
                Address = new Address(
                    "741 Wireless Road",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10330"
                ),
                Tier = CustomerTier.VIP,
            },
        };

        await customerRepository.CreateCustomersAsync(customers);
    }

    private static async Task DeleteAllOrdersAsync(IOrderRepository orderRepository)
    {
        var orders = await orderRepository.GetOrdersAsync();
        foreach (var order in orders)
        {
            await orderRepository.DeleteOrderAsync(order.Id);
        }
    }

    private static async Task SeedOrdersAsync(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository
    )
    {
        var customers = (await customerRepository.GetCustomersAsync()).ToList();
        var products = (await productRepository.GetProductsAsync()).ToList();

        if (customers.Count == 0 || products.Count == 0)
        {
            throw new InvalidOperationException(
                "Customers and products must be seeded before orders."
            );
        }

        var orders = new List<OrderEntity>
        {
            new(
                orderNumber: "ORD-2024-001",
                customer: customers[0],
                items:
                [
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "iPhone 15 Pro"), 1),
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "AirPods Pro"), 1),
                ],
                status: OrderStatus.Delivered,
                shippingAddress: customers[0].Address,
                notes: "Customer requested express delivery"
            ),
            new(
                orderNumber: "ORD-2024-002",
                customer: customers[1],
                items:
                [
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "MacBook Air M3"), 1),
                    OrderItemEntity.FromProduct(
                        products.First(p => p.Name == "Apple Watch Series 9"),
                        1
                    ),
                ],
                status: OrderStatus.Shipped,
                shippingAddress: customers[1].Address,
                notes: "VIP customer - priority shipping"
            ),
            new(
                orderNumber: "ORD-2024-003",
                customer: customers[2],
                items: [OrderItemEntity.FromProduct(products.First(p => p.Name == "iPad Air"), 2)],
                status: OrderStatus.Confirmed,
                shippingAddress: customers[2].Address,
                notes: null
            ),
            new(
                orderNumber: "ORD-2024-004",
                customer: customers[3],
                items:
                [
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "AirPods Pro"), 2),
                    OrderItemEntity.FromProduct(
                        products.First(p => p.Name == "Apple Watch Series 9"),
                        1
                    ),
                ],
                status: OrderStatus.Pending,
                shippingAddress: customers[3].Address,
                notes: "Gift wrapping requested"
            ),
            new(
                orderNumber: "ORD-2024-005",
                customer: customers[4],
                items:
                [
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "iPhone 15 Pro"), 1),
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "MacBook Air M3"), 1),
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "iPad Air"), 1),
                ],
                status: OrderStatus.Delivered,
                shippingAddress: new Address(
                    "999 Alternative Address",
                    "Bangkok",
                    "Bangkok",
                    "Thailand",
                    "10400"
                ),
                notes: "Bulk purchase for office"
            ),
            new(
                orderNumber: "ORD-2024-006",
                customer: customers[5],
                items:
                [
                    OrderItemEntity.FromProduct(
                        products.First(p => p.Name == "Apple Watch Series 9"),
                        3
                    ),
                ],
                status: OrderStatus.Cancelled,
                shippingAddress: customers[5].Address,
                notes: "Customer cancelled due to delay"
            ),
            new(
                orderNumber: "ORD-2024-007",
                customer: customers[6],
                items:
                [
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "iPad Air"), 1),
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "AirPods Pro"), 1),
                ],
                status: OrderStatus.Shipped,
                shippingAddress: customers[6].Address,
                notes: null
            ),
            new(
                orderNumber: "ORD-2024-008",
                customer: customers[7],
                items:
                [
                    OrderItemEntity.FromProduct(products.First(p => p.Name == "iPhone 15 Pro"), 2),
                ],
                status: OrderStatus.Confirmed,
                shippingAddress: customers[7].Address,
                notes: "Family plan purchase"
            ),
        };

        await orderRepository.CreateOrdersAsync(orders);
    }
}
