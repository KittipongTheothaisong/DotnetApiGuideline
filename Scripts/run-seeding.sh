#!/bin/bash

# ===================================================================
# DotnetApiGuideline - Database Seeding Script Runner
# Description: Executes SQL scripts for database initialization
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

# Function to execute SQL script
execute_sql_script() {
    local script_file="$1"
    local description="$2"
    
    print_status "Executing $description..."
    
    if [ ! -f "$script_file" ]; then
        print_error "Script file not found: $script_file"
        return 1
    fi
    
    # Execute the SQL script using docker exec
    cat "$script_file" | docker exec -i sqlserver /opt/mssql-tools18/bin/sqlcmd \
        -S localhost \
        -U "$DB_USER" \
        -P "$DB_PASSWORD" \
        -d "$DB_NAME" \
        -C
    
    if [ $? -eq 0 ]; then
        print_status "$description completed successfully!"
        return 0
    else
        print_error "$description failed!"
        return 1
    fi
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

# Main script logic
main() {
    echo "====================================================================="
    echo "DotnetApiGuideline - Database Seeding Script"
    echo "====================================================================="
    
    # Check prerequisites
    check_sqlserver
    test_connection || exit 1
    
    # Parse command line arguments
    case "${1:-seed}" in
        "seed")
            execute_sql_script "$SCRIPT_DIR/SeedData.sql" "Data Seeding"
            ;;
        "cleanup")
            print_warning "This will delete ALL data from the database!"
            read -p "Are you sure? (y/N): " -n 1 -r
            echo
            if [[ $REPLY =~ ^[Yy]$ ]]; then
                execute_sql_script "$SCRIPT_DIR/CleanupData.sql" "Data Cleanup"
            else
                print_status "Cleanup cancelled."
            fi
            ;;
        "reset")
            print_warning "This will delete ALL data and re-seed the database!"
            read -p "Are you sure? (y/N): " -n 1 -r
            echo
            if [[ $REPLY =~ ^[Yy]$ ]]; then
                execute_sql_script "$SCRIPT_DIR/CleanupData.sql" "Data Cleanup"
                if [ $? -eq 0 ]; then
                    execute_sql_script "$SCRIPT_DIR/SeedData.sql" "Data Seeding"
                fi
            else
                print_status "Reset cancelled."
            fi
            ;;
        "help"|"-h"|"--help")
            echo "Usage: $0 [command]"
            echo ""
            echo "Commands:"
            echo "  seed     - Seed the database with initial data (default)"
            echo "  cleanup  - Remove all data from the database"
            echo "  reset    - Cleanup and then seed the database"
            echo "  help     - Show this help message"
            echo ""
            echo "Examples:"
            echo "  $0 seed     # Seed the database"
            echo "  $0 cleanup  # Clean the database"
            echo "  $0 reset    # Reset and seed the database"
            ;;
        *)
            print_error "Unknown command: $1"
            print_status "Use '$0 help' for usage information."
            exit 1
            ;;
    esac
    
    echo "====================================================================="
    print_status "Script execution completed."
}

# Run the main function
main "$@"
