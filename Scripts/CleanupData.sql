-- ===================================================================
-- DotnetApiGuideline - Data Cleanup Script
-- Description: Removes all seeded data from the database
-- ===================================================================

USE DotnetApiGuidelineDb;
GO

PRINT 'Starting data cleanup...';

-- Disable foreign key constraints temporarily for easier deletion
ALTER TABLE OrderItemEntity NOCHECK CONSTRAINT ALL;
ALTER TABLE Orders NOCHECK CONSTRAINT ALL;
GO

-- Clear all data (in correct order to respect foreign key constraints)
DELETE FROM OrderItemEntity;
DELETE FROM Orders;
DELETE FROM Customers;
DELETE FROM Products;
GO

-- Re-enable foreign key constraints
ALTER TABLE OrderItemEntity CHECK CONSTRAINT ALL;
ALTER TABLE Orders CHECK CONSTRAINT ALL;
GO

-- Reset identity seeds if needed (uncomment if using IDENTITY columns)
-- DBCC CHECKIDENT ('OrderItemEntity', RESEED, 0);
-- DBCC CHECKIDENT ('Orders', RESEED, 0);
-- DBCC CHECKIDENT ('Customers', RESEED, 0);
-- DBCC CHECKIDENT ('Products', RESEED, 0);

PRINT 'Data cleanup completed successfully!';
PRINT '';

-- Verify cleanup
SELECT 'Customers' as TableName, COUNT(*) as RecordCount FROM Customers
UNION ALL
SELECT 'Products', COUNT(*) FROM Products  
UNION ALL
SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL
SELECT 'OrderItemEntity', COUNT(*) FROM OrderItemEntity;

GO
