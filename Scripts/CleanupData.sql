-- ===================================================================
-- DotnetApiGuideline - Data Cleanup Script
-- Description: Removes all seeded data from the database
-- ===================================================================

USE DotnetApiGuidelineDb;
GO

PRINT 'Starting data cleanup...';

-- Disable foreign key constraints temporarily for easier deletion
ALTER TABLE order_items NOCHECK CONSTRAINT ALL;
ALTER TABLE orders NOCHECK CONSTRAINT ALL;
GO

-- Clear all data (in correct order to respect foreign key constraints)
DELETE FROM order_items;
DELETE FROM orders;
DELETE FROM customers;
DELETE FROM products;
GO

-- Re-enable foreign key constraints
ALTER TABLE order_items CHECK CONSTRAINT ALL;
ALTER TABLE orders CHECK CONSTRAINT ALL;
GO

-- Reset identity seeds if needed (uncomment if using IDENTITY columns)
-- DBCC CHECKIDENT ('order_items', RESEED, 0);
-- DBCC CHECKIDENT ('orders', RESEED, 0);
-- DBCC CHECKIDENT ('customers', RESEED, 0);
-- DBCC CHECKIDENT ('products', RESEED, 0);

PRINT 'Data cleanup completed successfully!';
PRINT '';

-- Verify cleanup
SELECT 'customers' as TableName, COUNT(*) as RecordCount FROM customers
UNION ALL
SELECT 'products', COUNT(*) FROM products  
UNION ALL
SELECT 'orders', COUNT(*) FROM orders
UNION ALL
SELECT 'order_items', COUNT(*) FROM order_items;

GO
