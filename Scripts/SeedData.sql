-- ===================================================================
-- DotnetApiGuideline - Initial Data Seeding Script
-- Generated on: 2025-08-19
-- Description: Seeds initial data for all entities in the system
-- ===================================================================

USE DotnetApiGuidelineDb;
GO

-- Clear existing data (in correct order to respect foreign key constraints)
DELETE FROM OrderItemEntity;
DELETE FROM Orders;
DELETE FROM Customers;
DELETE FROM Products;
GO

-- ===================================================================
-- SEED CUSTOMER DATA
-- ===================================================================
INSERT INTO Customers (
    Id, 
    Name, 
    email, 
    Phone, 
    address_street, 
    address_city, 
    address_state, 
    address_country, 
    address_zip_code, 
    Tier, 
    CreatedBy, 
    CreatedDate, 
    UpdatedBy, 
    UpdatedDate
) VALUES 
-- Regular Customers
('11111111-1111-1111-1111-111111111111', 'John Smith', 'john.smith@email.com', '+1-555-0101', '123 Main St', 'New York', 'NY', 'USA', '10001', 0, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('22222222-2222-2222-2222-222222222222', 'Sarah Johnson', 'sarah.johnson@email.com', '+1-555-0102', '456 Oak Ave', 'Los Angeles', 'CA', 'USA', '90210', 0, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('33333333-3333-3333-3333-333333333333', 'Mike Davis', 'mike.davis@email.com', '+1-555-0103', '789 Pine Rd', 'Chicago', 'IL', 'USA', '60601', 0, NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- Silver Customers
('44444444-4444-4444-4444-444444444444', 'Emily Wilson', 'emily.wilson@email.com', '+1-555-0104', '321 Elm St', 'Houston', 'TX', 'USA', '77001', 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('55555555-5555-5555-5555-555555555555', 'David Brown', 'david.brown@email.com', '+1-555-0105', '654 Maple Dr', 'Phoenix', 'AZ', 'USA', '85001', 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- Gold Customers
('66666666-6666-6666-6666-666666666666', 'Lisa Anderson', 'lisa.anderson@email.com', '+1-555-0106', '987 Cedar Ln', 'Philadelphia', 'PA', 'USA', '19101', 2, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('77777777-7777-7777-7777-777777777777', 'Robert Taylor', 'robert.taylor@email.com', '+1-555-0107', '147 Birch Ave', 'San Antonio', 'TX', 'USA', '78201', 2, NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- VIP Customers
('88888888-8888-8888-8888-888888888888', 'Jennifer Garcia', 'jennifer.garcia@email.com', '+1-555-0108', '258 Walnut St', 'San Diego', 'CA', 'USA', '92101', 3, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('99999999-9999-9999-9999-999999999999', 'Christopher Martinez', 'christopher.martinez@email.com', '+1-555-0109', '369 Spruce Ct', 'Dallas', 'TX', 'USA', '75201', 3, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Amanda Rodriguez', 'amanda.rodriguez@email.com', '+1-555-0110', '741 Ash Blvd', 'San Jose', 'CA', 'USA', '95101', 3, NULL, GETUTCDATE(), NULL, GETUTCDATE());
GO

-- ===================================================================
-- SEED PRODUCT DATA
-- ===================================================================
INSERT INTO Products (
    Id, 
    Name, 
    Description, 
    Sku, 
    price_amount, 
    price_currency, 
    StockQuantity, 
    IsActive, 
    CreatedBy, 
    CreatedDate, 
    UpdatedBy, 
    UpdatedDate
) VALUES 
-- Electronics
('10000001-0000-0000-0000-000000000001', 'iPhone 15 Pro Max', 'Latest Apple iPhone with Pro Max features', 'IPHONE-15-PM-256', 1299.99, 'USD', 50, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('10000002-0000-0000-0000-000000000002', 'Samsung Galaxy S24 Ultra', 'Premium Samsung flagship smartphone', 'GALAXY-S24-ULTRA-256', 1199.99, 'USD', 45, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('10000003-0000-0000-0000-000000000003', 'MacBook Pro 14"', 'Apple MacBook Pro with M3 chip', 'MBP-14-M3-512', 1999.99, 'USD', 25, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('10000004-0000-0000-0000-000000000004', 'Dell XPS 13', 'Premium ultrabook laptop', 'DELL-XPS13-512', 1299.99, 'USD', 30, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('10000005-0000-0000-0000-000000000005', 'iPad Air 5th Gen', 'Apple iPad Air with M1 chip', 'IPAD-AIR-M1-256', 749.99, 'USD', 40, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- Clothing
('20000001-0000-0000-0000-000000000001', 'Nike Air Max 270', 'Comfortable running shoes', 'NIKE-AM270-BLK-10', 130.00, 'USD', 100, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('20000002-0000-0000-0000-000000000002', 'Adidas Ultraboost 22', 'Premium running shoes', 'ADIDAS-UB22-WHT-10', 180.00, 'USD', 85, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('20000003-0000-0000-0000-000000000003', 'Levi''s 501 Original Jeans', 'Classic straight leg jeans', 'LEVIS-501-BLUE-32', 69.99, 'USD', 150, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('20000004-0000-0000-0000-000000000004', 'Nike Dri-FIT T-Shirt', 'Moisture-wicking athletic shirt', 'NIKE-DFIT-TEE-L', 29.99, 'USD', 200, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('20000005-0000-0000-0000-000000000005', 'North Face Hoodie', 'Comfortable pullover hoodie', 'TNF-HOODIE-GREY-L', 89.99, 'USD', 75, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- Home & Garden
('30000001-0000-0000-0000-000000000001', 'Dyson V15 Detect', 'Cordless vacuum cleaner', 'DYSON-V15-DETECT', 749.99, 'USD', 20, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('30000002-0000-0000-0000-000000000002', 'KitchenAid Stand Mixer', 'Professional stand mixer', 'KA-MIXER-RED-5QT', 379.99, 'USD', 35, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('30000003-0000-0000-0000-000000000003', 'Instant Pot Duo 7-in-1', 'Multi-use pressure cooker', 'IP-DUO-6QT', 99.99, 'USD', 60, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('30000004-0000-0000-0000-000000000004', 'Philips Hue Smart Bulb Set', 'Color-changing smart bulbs (4-pack)', 'PHILIPS-HUE-4PK', 149.99, 'USD', 80, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('30000005-0000-0000-0000-000000000005', 'Weber Genesis Gas Grill', 'Premium outdoor gas grill', 'WEBER-GENESIS-3B', 899.99, 'USD', 15, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- Books & Media
('40000001-0000-0000-0000-000000000001', 'The Pragmatic Programmer', 'Software development best practices', 'BOOK-PRAGPROG-20TH', 49.99, 'USD', 100, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('40000002-0000-0000-0000-000000000002', 'Clean Code', 'A handbook of agile software craftsmanship', 'BOOK-CLEANCODE', 44.99, 'USD', 120, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('40000003-0000-0000-0000-000000000003', 'Design Patterns Book', 'Elements of reusable object-oriented software', 'BOOK-DESIGNPAT', 54.99, 'USD', 90, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('40000004-0000-0000-0000-000000000004', 'Sony WH-1000XM4 Headphones', 'Noise-canceling wireless headphones', 'SONY-WH1000XM4', 349.99, 'USD', 40, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('40000005-0000-0000-0000-000000000005', 'Kindle Paperwhite', 'Waterproof e-reader with built-in light', 'KINDLE-PW-11TH', 139.99, 'USD', 70, 1, NULL, GETUTCDATE(), NULL, GETUTCDATE());
GO

-- ===================================================================
-- SEED ORDER DATA
-- ===================================================================
INSERT INTO Orders (
    Id, 
    OrderNumber, 
    CustomerId, 
    Status, 
    shipping_address_street, 
    shipping_address_city, 
    shipping_address_state, 
    shipping_address_country, 
    shipping_address_zip_code, 
    Notes, 
    CreatedBy, 
    CreatedDate, 
    UpdatedBy, 
    UpdatedDate
) VALUES 
-- Completed Orders
('50000001-0000-0000-0000-000000000001', 'ORD-2025-001', '11111111-1111-1111-1111-111111111111', 3, '123 Main St', 'New York', 'NY', 'USA', '10001', 'Customer requested express delivery', NULL, DATEADD(day, -15, GETUTCDATE()), NULL, DATEADD(day, -15, GETUTCDATE())),
('50000002-0000-0000-0000-000000000002', 'ORD-2025-002', '22222222-2222-2222-2222-222222222222', 3, '456 Oak Ave', 'Los Angeles', 'CA', 'USA', '90210', 'Gift wrapping requested', NULL, DATEADD(day, -12, GETUTCDATE()), NULL, DATEADD(day, -12, GETUTCDATE())),
('50000003-0000-0000-0000-000000000003', 'ORD-2025-003', '44444444-4444-4444-4444-444444444444', 3, '321 Elm St', 'Houston', 'TX', 'USA', '77001', 'Standard delivery', NULL, DATEADD(day, -10, GETUTCDATE()), NULL, DATEADD(day, -10, GETUTCDATE())),

-- Shipped Orders
('50000004-0000-0000-0000-000000000004', 'ORD-2025-004', '66666666-6666-6666-6666-666666666666', 2, '987 Cedar Ln', 'Philadelphia', 'PA', 'USA', '19101', 'Priority shipping', NULL, DATEADD(day, -5, GETUTCDATE()), NULL, DATEADD(day, -5, GETUTCDATE())),
('50000005-0000-0000-0000-000000000005', 'ORD-2025-005', '88888888-8888-8888-8888-888888888888', 2, '258 Walnut St', 'San Diego', 'CA', 'USA', '92101', 'Handle with care', NULL, DATEADD(day, -3, GETUTCDATE()), NULL, DATEADD(day, -3, GETUTCDATE())),

-- Confirmed Orders
('50000006-0000-0000-0000-000000000006', 'ORD-2025-006', '99999999-9999-9999-9999-999999999999', 1, '369 Spruce Ct', 'Dallas', 'TX', 'USA', '75201', 'VIP customer - priority processing', NULL, DATEADD(day, -2, GETUTCDATE()), NULL, DATEADD(day, -2, GETUTCDATE())),
('50000007-0000-0000-0000-000000000007', 'ORD-2025-007', '55555555-5555-5555-5555-555555555555', 1, '654 Maple Dr', 'Phoenix', 'AZ', 'USA', '85001', 'Standard processing', NULL, DATEADD(day, -1, GETUTCDATE()), NULL, DATEADD(day, -1, GETUTCDATE())),

-- Pending Orders
('50000008-0000-0000-0000-000000000008', 'ORD-2025-008', '33333333-3333-3333-3333-333333333333', 0, '789 Pine Rd', 'Chicago', 'IL', 'USA', '60601', 'Awaiting payment confirmation', NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('50000009-0000-0000-0000-000000000009', 'ORD-2025-009', '77777777-7777-7777-7777-777777777777', 0, '147 Birch Ave', 'San Antonio', 'TX', 'USA', '78201', 'Customer inquiry pending', NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('50000010-0000-0000-0000-000000000010', 'ORD-2025-010', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 0, '741 Ash Blvd', 'San Jose', 'CA', 'USA', '95101', 'New order - processing', NULL, GETUTCDATE(), NULL, GETUTCDATE());
GO

-- ===================================================================
-- SEED ORDER ITEM DATA
-- ===================================================================
INSERT INTO OrderItemEntity (
    Id, 
    OrderId, 
    ProductId, 
    Quantity, 
    unit_price_amount, 
    unit_price_currency, 
    CreatedBy, 
    CreatedDate, 
    UpdatedBy, 
    UpdatedDate
) VALUES 
-- Order 1 Items (Electronics Bundle)
('60000001-0000-0000-0000-000000000001', '50000001-0000-0000-0000-000000000001', '10000001-0000-0000-0000-000000000001', 1, 1299.99, 'USD', NULL, DATEADD(day, -15, GETUTCDATE()), NULL, DATEADD(day, -15, GETUTCDATE())),
('60000002-0000-0000-0000-000000000002', '50000001-0000-0000-0000-000000000001', '10000005-0000-0000-0000-000000000005', 1, 749.99, 'USD', NULL, DATEADD(day, -15, GETUTCDATE()), NULL, DATEADD(day, -15, GETUTCDATE())),

-- Order 2 Items (Clothing)
('60000003-0000-0000-0000-000000000003', '50000002-0000-0000-0000-000000000002', '20000001-0000-0000-0000-000000000001', 2, 130.00, 'USD', NULL, DATEADD(day, -12, GETUTCDATE()), NULL, DATEADD(day, -12, GETUTCDATE())),
('60000004-0000-0000-0000-000000000004', '50000002-0000-0000-0000-000000000002', '20000003-0000-0000-0000-000000000003', 3, 69.99, 'USD', NULL, DATEADD(day, -12, GETUTCDATE()), NULL, DATEADD(day, -12, GETUTCDATE())),
('60000005-0000-0000-0000-000000000005', '50000002-0000-0000-0000-000000000002', '20000005-0000-0000-0000-000000000005', 1, 89.99, 'USD', NULL, DATEADD(day, -12, GETUTCDATE()), NULL, DATEADD(day, -12, GETUTCDATE())),

-- Order 3 Items (Home Appliances)
('60000006-0000-0000-0000-000000000006', '50000003-0000-0000-0000-000000000003', '30000001-0000-0000-0000-000000000001', 1, 749.99, 'USD', NULL, DATEADD(day, -10, GETUTCDATE()), NULL, DATEADD(day, -10, GETUTCDATE())),
('60000007-0000-0000-0000-000000000007', '50000003-0000-0000-0000-000000000003', '30000003-0000-0000-0000-000000000003', 2, 99.99, 'USD', NULL, DATEADD(day, -10, GETUTCDATE()), NULL, DATEADD(day, -10, GETUTCDATE())),

-- Order 4 Items (Premium Electronics)
('60000008-0000-0000-0000-000000000008', '50000004-0000-0000-0000-000000000004', '10000003-0000-0000-0000-000000000003', 1, 1999.99, 'USD', NULL, DATEADD(day, -5, GETUTCDATE()), NULL, DATEADD(day, -5, GETUTCDATE())),
('60000009-0000-0000-0000-000000000009', '50000004-0000-0000-0000-000000000004', '40000004-0000-0000-0000-000000000004', 1, 349.99, 'USD', NULL, DATEADD(day, -5, GETUTCDATE()), NULL, DATEADD(day, -5, GETUTCDATE())),

-- Order 5 Items (VIP Large Order)
('60000010-0000-0000-0000-000000000010', '50000005-0000-0000-0000-000000000005', '10000002-0000-0000-0000-000000000002', 1, 1199.99, 'USD', NULL, DATEADD(day, -3, GETUTCDATE()), NULL, DATEADD(day, -3, GETUTCDATE())),
('60000011-0000-0000-0000-000000000011', '50000005-0000-0000-0000-000000000005', '30000002-0000-0000-0000-000000000002', 1, 379.99, 'USD', NULL, DATEADD(day, -3, GETUTCDATE()), NULL, DATEADD(day, -3, GETUTCDATE())),
('60000012-0000-0000-0000-000000000012', '50000005-0000-0000-0000-000000000005', '30000005-0000-0000-0000-000000000005', 1, 899.99, 'USD', NULL, DATEADD(day, -3, GETUTCDATE()), NULL, DATEADD(day, -3, GETUTCDATE())),

-- Order 6 Items (Books & Learning)
('60000013-0000-0000-0000-000000000013', '50000006-0000-0000-0000-000000000006', '40000001-0000-0000-0000-000000000001', 2, 49.99, 'USD', NULL, DATEADD(day, -2, GETUTCDATE()), NULL, DATEADD(day, -2, GETUTCDATE())),
('60000014-0000-0000-0000-000000000014', '50000006-0000-0000-0000-000000000006', '40000002-0000-0000-0000-000000000002', 2, 44.99, 'USD', NULL, DATEADD(day, -2, GETUTCDATE()), NULL, DATEADD(day, -2, GETUTCDATE())),
('60000015-0000-0000-0000-000000000015', '50000006-0000-0000-0000-000000000006', '40000003-0000-0000-0000-000000000003', 1, 54.99, 'USD', NULL, DATEADD(day, -2, GETUTCDATE()), NULL, DATEADD(day, -2, GETUTCDATE())),

-- Order 7 Items (Sports & Fitness)
('60000016-0000-0000-0000-000000000016', '50000007-0000-0000-0000-000000000007', '20000002-0000-0000-0000-000000000002', 1, 180.00, 'USD', NULL, DATEADD(day, -1, GETUTCDATE()), NULL, DATEADD(day, -1, GETUTCDATE())),
('60000017-0000-0000-0000-000000000017', '50000007-0000-0000-0000-000000000007', '20000004-0000-0000-0000-000000000004', 3, 29.99, 'USD', NULL, DATEADD(day, -1, GETUTCDATE()), NULL, DATEADD(day, -1, GETUTCDATE())),

-- Order 8 Items (Pending Tech Order)
('60000018-0000-0000-0000-000000000018', '50000008-0000-0000-0000-000000000008', '10000004-0000-0000-0000-000000000004', 1, 1299.99, 'USD', NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('60000019-0000-0000-0000-000000000019', '50000008-0000-0000-0000-000000000008', '40000005-0000-0000-0000-000000000005', 2, 139.99, 'USD', NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- Order 9 Items (Smart Home Bundle)
('60000020-0000-0000-0000-000000000020', '50000009-0000-0000-0000-000000000009', '30000004-0000-0000-0000-000000000004', 3, 149.99, 'USD', NULL, GETUTCDATE(), NULL, GETUTCDATE()),

-- Order 10 Items (Mixed Category Order)
('60000021-0000-0000-0000-000000000021', '50000010-0000-0000-0000-000000000010', '20000001-0000-0000-0000-000000000001', 1, 130.00, 'USD', NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('60000022-0000-0000-0000-000000000022', '50000010-0000-0000-0000-000000000010', '30000003-0000-0000-0000-000000000003', 1, 99.99, 'USD', NULL, GETUTCDATE(), NULL, GETUTCDATE()),
('60000023-0000-0000-0000-000000000023', '50000010-0000-0000-0000-000000000010', '40000001-0000-0000-0000-000000000001', 1, 49.99, 'USD', NULL, GETUTCDATE(), NULL, GETUTCDATE());
GO

-- ===================================================================
-- VERIFICATION QUERIES
-- ===================================================================
PRINT 'Data seeding completed successfully!';
PRINT '';
PRINT 'Summary of seeded data:';

SELECT 'Customers' as TableName, COUNT(*) as RecordCount FROM Customers
UNION ALL
SELECT 'Products', COUNT(*) FROM Products  
UNION ALL
SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL
SELECT 'OrderItemEntity', COUNT(*) FROM OrderItemEntity;

PRINT '';
PRINT 'Customer distribution by tier:';
SELECT 
    CASE Tier 
        WHEN 0 THEN 'Regular'
        WHEN 1 THEN 'Silver'
        WHEN 2 THEN 'Gold'
        WHEN 3 THEN 'VIP'
    END as CustomerTier,
    COUNT(*) as Count
FROM Customers 
GROUP BY Tier 
ORDER BY Tier;

PRINT '';
PRINT 'Order distribution by status:';
SELECT 
    CASE Status 
        WHEN -1 THEN 'Unknown'
        WHEN 0 THEN 'Pending'
        WHEN 1 THEN 'Confirmed'
        WHEN 2 THEN 'Shipped'
        WHEN 3 THEN 'Delivered'
        WHEN 4 THEN 'Cancelled'
    END as OrderStatus,
    COUNT(*) as Count
FROM Orders 
GROUP BY Status 
ORDER BY Status;

GO
