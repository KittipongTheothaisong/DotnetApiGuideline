-- ===================================================================
-- DotnetApiGuideline - Database Schema Query
-- Description: SQL queries to retrieve table schemas with field names and types
-- ===================================================================

USE DotnetApiGuidelineDb;
GO

-- ===================================================================
-- DETAILED SCHEMA INFORMATION
-- Shows all tables with their columns, data types, and constraints
-- ===================================================================

PRINT 'Database Schema Information for: DotnetApiGuidelineDb';
PRINT '=======================================================';
PRINT '';

-- Main schema query
SELECT 
    t.TABLE_SCHEMA as [Schema],
    t.TABLE_NAME as [Table],
    c.COLUMN_NAME as [Column],
    c.DATA_TYPE as [DataType],
    CASE 
        WHEN c.CHARACTER_MAXIMUM_LENGTH IS NOT NULL 
        THEN c.DATA_TYPE + '(' + CAST(c.CHARACTER_MAXIMUM_LENGTH as VARCHAR(10)) + ')'
        WHEN c.NUMERIC_PRECISION IS NOT NULL AND c.NUMERIC_SCALE IS NOT NULL
        THEN c.DATA_TYPE + '(' + CAST(c.NUMERIC_PRECISION as VARCHAR(10)) + ',' + CAST(c.NUMERIC_SCALE as VARCHAR(10)) + ')'
        WHEN c.NUMERIC_PRECISION IS NOT NULL
        THEN c.DATA_TYPE + '(' + CAST(c.NUMERIC_PRECISION as VARCHAR(10)) + ')'
        ELSE c.DATA_TYPE
    END as [FullDataType],
    CASE WHEN c.IS_NULLABLE = 'YES' THEN 'NULL' ELSE 'NOT NULL' END as [Nullable],
    c.COLUMN_DEFAULT as [DefaultValue],
    c.ORDINAL_POSITION as [Position],
    CASE 
        WHEN pk.COLUMN_NAME IS NOT NULL THEN 'PRIMARY KEY'
        WHEN fk.COLUMN_NAME IS NOT NULL THEN 'FOREIGN KEY'
        ELSE ''
    END as [KeyType],
    CASE 
        WHEN COLUMNPROPERTY(OBJECT_ID(t.TABLE_SCHEMA + '.' + t.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') = 1 
        THEN 'IDENTITY'
        ELSE ''
    END as [Identity]
FROM 
    INFORMATION_SCHEMA.TABLES t
    INNER JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME AND t.TABLE_SCHEMA = c.TABLE_SCHEMA
    LEFT JOIN (
        SELECT 
            ku.TABLE_SCHEMA,
            ku.TABLE_NAME,
            ku.COLUMN_NAME
        FROM 
            INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku ON tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
        WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
    ) pk ON c.TABLE_SCHEMA = pk.TABLE_SCHEMA AND c.TABLE_NAME = pk.TABLE_NAME AND c.COLUMN_NAME = pk.COLUMN_NAME
    LEFT JOIN (
        SELECT 
            ku.TABLE_SCHEMA,
            ku.TABLE_NAME,
            ku.COLUMN_NAME
        FROM 
            INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku ON tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
        WHERE tc.CONSTRAINT_TYPE = 'FOREIGN KEY'
    ) fk ON c.TABLE_SCHEMA = fk.TABLE_SCHEMA AND c.TABLE_NAME = fk.TABLE_NAME AND c.COLUMN_NAME = fk.COLUMN_NAME
WHERE 
    t.TABLE_TYPE = 'BASE TABLE'
    AND t.TABLE_SCHEMA != 'sys'
ORDER BY 
    t.TABLE_SCHEMA,
    t.TABLE_NAME,
    c.ORDINAL_POSITION;

GO

PRINT '';
PRINT '=======================================================';
PRINT 'TABLE SUMMARY';
PRINT '=======================================================';
PRINT '';

-- ===================================================================
-- TABLE SUMMARY
-- Shows count of tables and columns
-- ===================================================================

SELECT 
    t.TABLE_SCHEMA as [Schema],
    t.TABLE_NAME as [TableName],
    COUNT(c.COLUMN_NAME) as [ColumnCount]
FROM 
    INFORMATION_SCHEMA.TABLES t
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME AND t.TABLE_SCHEMA = c.TABLE_SCHEMA
WHERE 
    t.TABLE_TYPE = 'BASE TABLE'
    AND t.TABLE_SCHEMA != 'sys'
GROUP BY 
    t.TABLE_SCHEMA,
    t.TABLE_NAME
ORDER BY 
    t.TABLE_SCHEMA,
    t.TABLE_NAME;

GO

PRINT '';
PRINT '=======================================================';
PRINT 'FOREIGN KEY RELATIONSHIPS';
PRINT '=======================================================';
PRINT '';

-- ===================================================================
-- FOREIGN KEY RELATIONSHIPS
-- Shows relationships between tables
-- ===================================================================

SELECT 
    fk.name AS [ForeignKeyName],
    tp.name AS [ParentTable],
    cp.name AS [ParentColumn],
    tr.name AS [ReferencedTable],
    cr.name AS [ReferencedColumn]
FROM 
    sys.foreign_keys fk
    INNER JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
    INNER JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
    INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    INNER JOIN sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
    INNER JOIN sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
ORDER BY 
    tp.name, 
    cp.name;

GO

PRINT '';
PRINT '=======================================================';
PRINT 'INDEX INFORMATION';
PRINT '=======================================================';
PRINT '';

-- ===================================================================
-- INDEX INFORMATION
-- Shows indexes on all tables
-- ===================================================================

SELECT 
    t.name AS [TableName],
    i.name AS [IndexName],
    i.type_desc AS [IndexType],
    i.is_unique AS [IsUnique],
    i.is_primary_key AS [IsPrimaryKey],
    c.name AS [ColumnName],
    ic.index_column_id AS [ColumnPosition],
    ic.is_descending_key AS [IsDescending]
FROM 
    sys.tables t
    INNER JOIN sys.indexes i ON t.object_id = i.object_id
    INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE 
    t.is_ms_shipped = 0
    AND i.type > 0  -- Exclude heaps
ORDER BY 
    t.name,
    i.name,
    ic.index_column_id;

GO

PRINT '';
PRINT 'Schema analysis completed successfully!';
GO
