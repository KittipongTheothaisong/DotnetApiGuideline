#!/bin/bash

# ===================================================================
# DotnetApiGuideline - Database Schema Reader
# Description: Reads all table schemas including table names, field names and types
# ===================================================================

# Configuration
DB_SERVER="localhost"
DB_NAME="DotnetApiGuidelineDb"
DB_USER="sa"
DB_PASSWORD="YourPassword123!"
SCRIPT_DIR="$(dirname "$0")"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

print_header() {
    echo -e "${BLUE}$1${NC}"
}

print_table() {
    echo -e "${CYAN}$1${NC}"
}

# Function to check if SQL Server container is running
check_sqlserver() {
    print_status "Checking SQL Server container status..."
    
    if ! docker ps | grep -q sqlserver; then
        print_error "SQL Server container is not running!"
        print_status "Please start the container using: docker-compose up -d"
        exit 1
    fi
    
    print_status "SQL Server container is running."
}

# Function to test database connection
test_connection() {
    print_status "Testing database connection..."
    
    docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
        -S localhost \
        -U "$DB_USER" \
        -P "$DB_PASSWORD" \
        -d "$DB_NAME" \
        -C \
        -Q "SELECT 1" > /dev/null 2>&1
    
    if [ $? -eq 0 ]; then
        print_status "Database connection successful."
        return 0
    else
        print_error "Failed to connect to database!"
        print_warning "Please ensure the database exists and credentials are correct."
        return 1
    fi
}

# Function to get all table schemas
get_table_schemas() {
    local format="${1:-console}"
    local output_file="$2"
    
    print_status "Retrieving database schema information..."
    
    local sql_query="
SET NOCOUNT ON;

-- Get all user tables with their columns
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
"
    
    case "$format" in
        "console")
            # Output to console with formatting
            docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
                -S localhost \
                -U "$DB_USER" \
                -P "$DB_PASSWORD" \
                -d "$DB_NAME" \
                -C \
                -Q "$sql_query" \
                -s "," \
                -W \
                | sed 's/,/ | /g' \
                | while IFS= read -r line; do
                    if [[ $line == *"dbo"* ]] && [[ $line != *"Schema"* ]]; then
                        echo -e "${CYAN}$line${NC}"
                    else
                        echo "$line"
                    fi
                done
            ;;
        "csv")
            # Output to CSV file
            if [ -z "$output_file" ]; then
                output_file="$SCRIPT_DIR/database_schema.csv"
            fi
            
            print_status "Saving schema to CSV file: $output_file"
            
            # Get data with tab delimiter and convert to proper CSV using Python
            local temp_file=$(mktemp)
            docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
                -S localhost \
                -U "$DB_USER" \
                -P "$DB_PASSWORD" \
                -d "$DB_NAME" \
                -C \
                -Q "$sql_query" \
                -s $'\t' \
                -W \
                | grep -v "^-" \
                > "$temp_file"
            
            # Convert to proper CSV format using Python
            python3 -c "
import csv
import sys

# Read tab-delimited data and write as proper CSV
with open('$temp_file', 'r') as infile, open('$output_file', 'w', newline='') as outfile:
    reader = csv.DictReader(infile, delimiter='\t')
    if reader.fieldnames:
        writer = csv.DictWriter(outfile, fieldnames=reader.fieldnames)
        writer.writeheader()
        for row in reader:
            # Clean up any extra whitespace
            cleaned_row = {k: v.strip() if v else v for k, v in row.items()}
            writer.writerow(cleaned_row)
print('CSV export completed')
" 2>/dev/null || {
                # Fallback if Python is not available
                cat "$temp_file" | sed 's/\t/,/g' > "$output_file"
            }
            
            rm "$temp_file"
            
            if [ -f "$output_file" ]; then
                print_status "Schema exported to: $output_file"
            else
                print_error "Failed to export schema to CSV"
            fi
            ;;
        "json")
            # Output to JSON format
            if [ -z "$output_file" ]; then
                output_file="$SCRIPT_DIR/database_schema.json"
            fi
            
            print_status "Saving schema to JSON file: $output_file"
            
            # First get the data
            local temp_file=$(mktemp)
            docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
                -S localhost \
                -U "$DB_USER" \
                -P "$DB_PASSWORD" \
                -d "$DB_NAME" \
                -C \
                -Q "$sql_query" \
                -s $'\t' \
                -W \
                | grep -v "^-" \
                > "$temp_file"
            
            # Convert to JSON using a simple approach
            python3 -c "
import csv
import json
import sys

# Read tab-delimited data
data = []
with open('$temp_file', 'r') as f:
    reader = csv.DictReader(f, delimiter='\t')
    for row in reader:
        data.append(row)

# Group by table
tables = {}
for row in data:
    table_name = row.get('Table', '').strip()
    schema_name = row.get('Schema', '').strip()
    # Skip header rows and invalid data
    if (table_name and table_name != 'Table' and table_name != '-----' and 
        schema_name and schema_name != 'Schema' and schema_name != '------'):
        if table_name not in tables:
            tables[table_name] = {
                'schema': schema_name,
                'name': table_name,
                'columns': []
            }
        
        column_info = {
            'name': row.get('Column', '').strip(),
            'dataType': row.get('DataType', '').strip(),
            'fullDataType': row.get('FullDataType', '').strip(),
            'nullable': row.get('Nullable', '').strip(),
            'defaultValue': row.get('DefaultValue', '').strip(),
            'position': row.get('Position', '').strip(),
            'keyType': row.get('KeyType', '').strip(),
            'identity': row.get('Identity', '').strip()
        }
        tables[table_name]['columns'].append(column_info)

# Output JSON
with open('$output_file', 'w') as f:
    json.dump({'database': '$DB_NAME', 'tables': list(tables.values())}, f, indent=2)

print('Schema exported to JSON successfully')
" 2>/dev/null || echo "Python3 not available, skipping JSON export"
            
            rm "$temp_file"
            ;;
        *)
            print_error "Unknown format: $format"
            return 1
            ;;
    esac
}

# Function to get table summary
get_table_summary() {
    print_status "Getting table summary..."
    
    local sql_query="
SET NOCOUNT ON;

SELECT 
    t.TABLE_SCHEMA as [Schema],
    t.TABLE_NAME as [TableName],
    COUNT(c.COLUMN_NAME) as [ColumnCount],
    STUFF((
        SELECT ', ' + c2.COLUMN_NAME 
        FROM INFORMATION_SCHEMA.COLUMNS c2 
        WHERE c2.TABLE_NAME = t.TABLE_NAME 
        AND c2.TABLE_SCHEMA = t.TABLE_SCHEMA
        ORDER BY c2.ORDINAL_POSITION
        FOR XML PATH('')
    ), 1, 2, '') as [Columns]
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
"
    
    docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
        -S localhost \
        -U "$DB_USER" \
        -P "$DB_PASSWORD" \
        -d "$DB_NAME" \
        -C \
        -Q "$sql_query" \
        -s "|" \
        -W
}

# Function to show usage
show_help() {
    echo "Usage: $0 [command] [options]"
    echo ""
    echo "Commands:"
    echo "  schema     - Show detailed schema information (default)"
    echo "  summary    - Show table summary with column count"
    echo "  export     - Export schema to file"
    echo "  help       - Show this help message"
    echo ""
    echo "Options for 'schema' and 'export':"
    echo "  --format   - Output format: console, csv, json (default: console)"
    echo "  --output   - Output file path (for csv and json formats)"
    echo ""
    echo "Examples:"
    echo "  $0 schema                              # Show schema in console"
    echo "  $0 schema --format csv                 # Show schema as CSV"
    echo "  $0 export --format json --output ./my_schema.json"
    echo "  $0 summary                             # Show table summary"
    echo ""
}

# Main script logic
main() {
    echo "====================================================================="
    echo "DotnetApiGuideline - Database Schema Reader"
    echo "Database: $DB_NAME"
    echo "====================================================================="
    
    # Check prerequisites
    check_sqlserver
    test_connection || exit 1
    
    # Parse command line arguments
    local command="${1:-schema}"
    local format="console"
    local output_file=""
    
    # Parse additional arguments
    shift
    while [[ $# -gt 0 ]]; do
        case $1 in
            --format)
                format="$2"
                shift 2
                ;;
            --output)
                output_file="$2"
                shift 2
                ;;
            *)
                print_error "Unknown option: $1"
                show_help
                exit 1
                ;;
        esac
    done
    
    case "$command" in
        "schema"|"export")
            print_header "Database Schema Information"
            echo ""
            get_table_schemas "$format" "$output_file"
            ;;
        "summary")
            print_header "Database Table Summary"
            echo ""
            get_table_summary
            ;;
        "help"|"-h"|"--help")
            show_help
            ;;
        *)
            print_error "Unknown command: $command"
            show_help
            exit 1
            ;;
    esac
    
    echo ""
    echo "====================================================================="
    print_status "Schema reading completed."
}

# Run the main function
main "$@"
