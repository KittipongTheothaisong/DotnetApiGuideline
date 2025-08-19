using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiGuideline.Migrations
{
    /// <inheritdoc />
    public partial class UseSnakeCaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemEntity_Orders_OrderId",
                table: "OrderItemEntity"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemEntity_Products_ProductId",
                table: "OrderItemEntity"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders"
            );

            migrationBuilder.DropPrimaryKey(name: "PK_Products", table: "Products");

            migrationBuilder.DropPrimaryKey(name: "PK_Orders", table: "Orders");

            migrationBuilder.DropPrimaryKey(name: "PK_Customers", table: "Customers");

            migrationBuilder.DropPrimaryKey(name: "PK_OrderItemEntity", table: "OrderItemEntity");

            migrationBuilder.RenameTable(name: "Products", newName: "products");

            migrationBuilder.RenameTable(name: "Orders", newName: "orders");

            migrationBuilder.RenameTable(name: "Customers", newName: "customers");

            migrationBuilder.RenameTable(name: "OrderItemEntity", newName: "order_items");

            migrationBuilder.RenameColumn(name: "Sku", table: "products", newName: "sku");

            migrationBuilder.RenameColumn(name: "Name", table: "products", newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "products",
                newName: "description"
            );

            migrationBuilder.RenameColumn(name: "Id", table: "products", newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "products",
                newName: "updated_date"
            );

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "products",
                newName: "updated_by"
            );

            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "products",
                newName: "stock_quantity"
            );

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "products",
                newName: "is_active"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "products",
                newName: "created_date"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "products",
                newName: "created_by"
            );

            migrationBuilder.RenameColumn(name: "Status", table: "orders", newName: "status");

            migrationBuilder.RenameColumn(name: "Notes", table: "orders", newName: "notes");

            migrationBuilder.RenameColumn(name: "Id", table: "orders", newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "orders",
                newName: "updated_date"
            );

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "orders",
                newName: "updated_by"
            );

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "orders",
                newName: "order_number"
            );

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "orders",
                newName: "customer_id"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "orders",
                newName: "created_date"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "orders",
                newName: "created_by"
            );

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "orders",
                newName: "ix_orders_customer_id"
            );

            migrationBuilder.RenameColumn(name: "Tier", table: "customers", newName: "tier");

            migrationBuilder.RenameColumn(name: "Phone", table: "customers", newName: "phone");

            migrationBuilder.RenameColumn(name: "Name", table: "customers", newName: "name");

            migrationBuilder.RenameColumn(name: "Id", table: "customers", newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "customers",
                newName: "updated_date"
            );

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "customers",
                newName: "updated_by"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "customers",
                newName: "created_date"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "customers",
                newName: "created_by"
            );

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "order_items",
                newName: "quantity"
            );

            migrationBuilder.RenameColumn(name: "Id", table: "order_items", newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "order_items",
                newName: "updated_date"
            );

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "order_items",
                newName: "updated_by"
            );

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "order_items",
                newName: "product_id"
            );

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "order_items",
                newName: "order_id"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "order_items",
                newName: "created_date"
            );

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "order_items",
                newName: "created_by"
            );

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemEntity_ProductId",
                table: "order_items",
                newName: "ix_order_items_product_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_OrderItemEntity_OrderId",
                table: "order_items",
                newName: "ix_order_items_order_id"
            );

            migrationBuilder.AddPrimaryKey(name: "pk_products", table: "products", column: "id");

            migrationBuilder.AddPrimaryKey(name: "pk_orders", table: "orders", column: "id");

            migrationBuilder.AddPrimaryKey(name: "pk_customers", table: "customers", column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_items",
                table: "order_items",
                column: "id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_order_items_orders_order_id",
                table: "order_items",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_order_items_products_product_id",
                table: "order_items",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_items_orders_order_id",
                table: "order_items"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_order_items_products_product_id",
                table: "order_items"
            );

            migrationBuilder.DropForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders"
            );

            migrationBuilder.DropPrimaryKey(name: "pk_products", table: "products");

            migrationBuilder.DropPrimaryKey(name: "pk_orders", table: "orders");

            migrationBuilder.DropPrimaryKey(name: "pk_customers", table: "customers");

            migrationBuilder.DropPrimaryKey(name: "pk_order_items", table: "order_items");

            migrationBuilder.RenameTable(name: "products", newName: "Products");

            migrationBuilder.RenameTable(name: "orders", newName: "Orders");

            migrationBuilder.RenameTable(name: "customers", newName: "Customers");

            migrationBuilder.RenameTable(name: "order_items", newName: "OrderItemEntity");

            migrationBuilder.RenameColumn(name: "sku", table: "Products", newName: "Sku");

            migrationBuilder.RenameColumn(name: "name", table: "Products", newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Products",
                newName: "Description"
            );

            migrationBuilder.RenameColumn(name: "id", table: "Products", newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                table: "Products",
                newName: "UpdatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Products",
                newName: "UpdatedBy"
            );

            migrationBuilder.RenameColumn(
                name: "stock_quantity",
                table: "Products",
                newName: "StockQuantity"
            );

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Products",
                newName: "IsActive"
            );

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Products",
                newName: "CreatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Products",
                newName: "CreatedBy"
            );

            migrationBuilder.RenameColumn(name: "status", table: "Orders", newName: "Status");

            migrationBuilder.RenameColumn(name: "notes", table: "Orders", newName: "Notes");

            migrationBuilder.RenameColumn(name: "id", table: "Orders", newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                table: "Orders",
                newName: "UpdatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Orders",
                newName: "UpdatedBy"
            );

            migrationBuilder.RenameColumn(
                name: "order_number",
                table: "Orders",
                newName: "OrderNumber"
            );

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "Orders",
                newName: "CustomerId"
            );

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Orders",
                newName: "CreatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Orders",
                newName: "CreatedBy"
            );

            migrationBuilder.RenameIndex(
                name: "ix_orders_customer_id",
                table: "Orders",
                newName: "IX_Orders_CustomerId"
            );

            migrationBuilder.RenameColumn(name: "tier", table: "Customers", newName: "Tier");

            migrationBuilder.RenameColumn(name: "phone", table: "Customers", newName: "Phone");

            migrationBuilder.RenameColumn(name: "name", table: "Customers", newName: "Name");

            migrationBuilder.RenameColumn(name: "id", table: "Customers", newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                table: "Customers",
                newName: "UpdatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Customers",
                newName: "UpdatedBy"
            );

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "Customers",
                newName: "CreatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Customers",
                newName: "CreatedBy"
            );

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "OrderItemEntity",
                newName: "Quantity"
            );

            migrationBuilder.RenameColumn(name: "id", table: "OrderItemEntity", newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                table: "OrderItemEntity",
                newName: "UpdatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "OrderItemEntity",
                newName: "UpdatedBy"
            );

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "OrderItemEntity",
                newName: "ProductId"
            );

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "OrderItemEntity",
                newName: "OrderId"
            );

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "OrderItemEntity",
                newName: "CreatedDate"
            );

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "OrderItemEntity",
                newName: "CreatedBy"
            );

            migrationBuilder.RenameIndex(
                name: "ix_order_items_product_id",
                table: "OrderItemEntity",
                newName: "IX_OrderItemEntity_ProductId"
            );

            migrationBuilder.RenameIndex(
                name: "ix_order_items_order_id",
                table: "OrderItemEntity",
                newName: "IX_OrderItemEntity_OrderId"
            );

            migrationBuilder.AddPrimaryKey(name: "PK_Products", table: "Products", column: "Id");

            migrationBuilder.AddPrimaryKey(name: "PK_Orders", table: "Orders", column: "Id");

            migrationBuilder.AddPrimaryKey(name: "PK_Customers", table: "Customers", column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItemEntity",
                table: "OrderItemEntity",
                column: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemEntity_Orders_OrderId",
                table: "OrderItemEntity",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemEntity_Products_ProductId",
                table: "OrderItemEntity",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
