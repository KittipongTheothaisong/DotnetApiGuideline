## **Dotnet API Guideline: เอกสารประกอบโปรเจกต์ฉบับสมบูรณ์**

### **1. "ทำไม?" ทำความเข้าใจสถาปัตยกรรมแบบ Layered**

ก่อนที่จะลงลึกในรายละเอียดของโปรเจกต์นี้ สิ่งสำคัญคือต้องเข้าใจว่า _ทำไม_ เราถึงต้องออกแบบโครงสร้างแบบนี้ ลองจินตนาการถึงการสร้างบ้าน คุณคงไม่นำระบบประปา, สายไฟ, และงานตกแต่งภายในมาปนกันจนยุ่งเหยิง แต่จะแยกเป็นระบบ (Layer) เฉพาะทางที่ทำงานร่วมกันได้อย่างเป็นอิสระ ช่างประปาไม่จำเป็นต้องรู้ว่าผนังจะทาสีอะไร และช่างไฟก็ไม่จำเป็นต้องสนใจยี่ห้อของอ่างล้างจาน

สถาปัตยกรรมซอฟต์แวร์ก็ใช้หลักการเดียวกัน เราแบ่งแอปพลิเคชันออกเป็น Layer ต่างๆ เพื่อจัดการกับความซับซ้อน, เพิ่มความสามารถในการบำรุงรักษา (Maintainability) และสร้างความยืดหยุ่น หลักการนี้เรียกว่า **Separation of Concerns**

ในโปรเจกต์นี้ เราใช้สถาปัตยกรรมแบบ Layered ที่ได้รับแรงบันดาลใจจาก **Clean Architecture** โดยมีแนวคิดหลักคือ ส่วนที่สำคัญที่สุดของแอปพลิเคชัน ซึ่งก็คือ Business Logic ไม่ควรขึ้นอยู่กับ (Depend on) รายละเอียดทางเทคนิค เช่น วิธีการแสดงผลหรือการจัดเก็บข้อมูล

---

### **2. คำอธิบายแต่ละ Layer (ฉบับเจาะลึก)**

สถาปัตยกรรมของโปรเจกต์นี้แบ่งออกเป็น 4 Layer หลัก โดยมี **Domain Layer** เป็นแกนกลาง และมีกฎการพึ่งพิง (Dependency Rule) ที่บังคับให้ทุก Layer ต้องชี้เข้าหาแกนกลางเสมอ

[TAG\_CLEAN\_ARCHITECTURE\_DIAGRAM]

#### **a. `Sources/Domain`: หัวใจของธุรกิจของคุณ**

นี่คือ Layer ที่เป็นอิสระและสำคัญที่สุด เปรียบเสมือนพิมพ์เขียวของธุรกิจคุณในรูปแบบของโค้ด มันนิยาม "อะไร" คือธุรกิจของคุณ แต่ไม่สนใจว่า "อย่างไร" ที่จะจัดเก็บหรือแสดงผลข้อมูล

- **ส่วนประกอบหลัก:**
  - **Entities**: เป็น Object ที่มีเอกลักษณ์ (Identity) และวงจรชีวิต (Lifecycle) สามารถเปลี่ยนแปลงสถานะได้ตลอดเวลา แต่ยังคงเป็นตัวเดิมเสมอ (เช่น Order ที่มี `Id` เดิม แต่ `Status` เปลี่ยนไป) ในโปรเจกต์นี้ `OrderEntity`, `CustomerEntity`, และ `ProductEntity` คือตัวแทนของแนวคิดหลักทางธุรกิจ. สิ่งสำคัญคือ `Entity` ไม่ใช่แค่ถุงใส่ข้อมูล แต่ยังห่อหุ้ม **กฎทางธุรกิจ** ไว้ด้วย เช่น `OrderEntity` มีเมธอด `ChangeStatus` ที่ป้องกันการเปลี่ยนสถานะที่ไม่ถูกต้อง.
  - **Value Objects**: เป็น Object ที่ใช้อธิบายคุณลักษณะของ `Entity` แต่ไม่มีเอกลักษณ์เป็นของตัวเอง มันถูกกำหนดโดย "ค่า" ของมัน และมักจะไม่สามารถเปลี่ยนแปลงได้ (Immutable) เช่น `Money` ประกอบด้วย `Amount` และ `Currency` ถ้า `Amount` เปลี่ยนไป มันจะกลายเป็น `Money` ก้อนใหม่ทันที. การใช้ Value Object อย่าง `Address` หรือ `Email` ช่วยให้โค้ดสื่อความหมายได้ดีขึ้นและสามารถใส่ Validation เฉพาะทางเข้าไปได้ (เช่น รูปแบบอีเมลที่ถูกต้อง)
  - **Repository Interfaces**: เป็น "แบบแปลน" หรือข้อตกลงที่กำหนดว่าการเข้าถึงข้อมูลของ `Entity` ต้องทำอะไรได้บ้าง เช่น `IOrderRepository` กำหนดว่าต้องมีเมธอด `GetOrderByIdAsync` และ `CreateOrderAsync`. `Interface` นี้อยู่ใน Domain Layer เพื่อให้ Layer ชั้นในสุดเป็นผู้กำหนด "กฎ" ของการเข้าถึงข้อมูล โดยไม่สนใจว่า Layer ด้านนอกจะไป Implement ด้วยเทคโนโลยีอะไร

#### **b. `Sources/Application`: ผู้ควบคุมการทำงาน**

Layer นี้ทำหน้าที่เป็นตัวกลางระหว่างโลกภายนอก (Presentation) และแกนกลางของธุรกิจ (Domain) มันรับคำสั่งจากผู้ใช้และควบคุม `Entity` ต่างๆ เพื่อทำให้ "Use Case" หรือกระบวนการทางธุรกิจนั้นสำเร็จ

- **ส่วนประกอบหลัก:**
  - **Services**: เป็นคลาสที่รวบรวม Logic ของ Use Case หนึ่งๆ ไว้ด้วยกัน เช่น `OrderService` รับผิดชอบกระบวนการ "สร้าง Order". มันจะทำหน้าที่ประสานงาน เช่น รับ `CreateOrderRequest`, เรียกใช้ `ICustomerRepository` เพื่อดึงข้อมูลลูกค้า, เรียกใช้ `IProductRepository` เพื่อเช็คสต็อก, และสุดท้ายสั่งให้ `IOrderRepository` บันทึก `OrderEntity` ที่สร้างขึ้นใหม่ลงฐานข้อมูล
  - **Application Models**: เป็น Model หลักที่ถูกออกแบบมาเพื่อรองรับกระบวนการทำงาน (Use Case) ของแอปพลิเคชันโดยเฉพาะ คลาสเหล่านี้ เช่น `Order`, `Customer`, และ `Product` คือโครงสร้างข้อมูลที่ Application Service ใช้ในการทำงานและส่งผลลัพธ์กลับออกไป มันกำหนด "สัญญา" ที่ชัดเจนว่าข้อมูลที่เกี่ยวข้องกับ Use Case หนึ่งๆ นั้นประกอบด้วยอะไรบ้าง ทำให้ Logic ภายใน Service มีความชัดเจนและเป็นระเบียบ
  - **Interfaces**: เป็น "แบบแปลน" ของ Service เช่น `IOrderService` ซึ่งกำหนดว่า Application Layer มี Use Case อะไรให้บริการบ้าง ทำให้ Presentation Layer สามารถเรียกใช้งานได้โดยไม่ต้องผูกติดกับ Implementation จริง

#### **c. `Sources/Infrastructure`: ส่วนของรายละเอียดทางเทคนิค**

Layer นี้คือ "ห้องเครื่อง" ของแอปพลิเคชัน เป็นที่อยู่ของโค้ดทั้งหมดที่ต้องติดต่อกับโลกภายนอก ไม่ว่าจะเป็นฐานข้อมูล, ระบบไฟล์, หรือเซอร์วิสอื่นๆ หน้าที่ของมันคือการทำให้ "แบบแปลน" ที่กำหนดไว้ใน Domain และ Application Layer เกิดขึ้นจริง

- **ส่วนประกอบหลัก:**
  - **Data Persistence**: ส่วนที่จัดการการเชื่อมต่อและบันทึกข้อมูลลงฐานข้อมูลจริง
    - **DbContext**: สำหรับ EF Core จะมี `AppDbContext` ที่ทำหน้าที่กำหนด Schema ของฐานข้อมูลและความสัมพันธ์ต่างๆ. สำหรับ MongoDB จะมี `MongoDbContext`.
    - **Repositories**: เป็นการ Implement `Interface` จาก Domain Layer เช่น `OrderRepository` จะเขียนโค้ด EF Core เพื่อบันทึกข้อมูลลง SQL Server ในขณะที่ `OrderMongoRepository` จะใช้ MongoDB Driver แทน
  - **Configurations**: เป็นคลาสที่ใช้ตั้งค่าการทำงานต่างๆ ของแอปพลิเคชัน เช่น `DatabaseConfiguration` ใช้ตั้งค่าการเชื่อมต่อ DbContext, `AuthenticationConfiguration` ใช้ตั้งค่าการเชื่อมต่อกับ Keycloak, และ `SeedDataConfiguration` ใช้สร้างข้อมูลเริ่มต้นในฐานข้อมูล

#### **d. `Sources/Presentation`: ประตูสู่แอปพลิเคชัน**

นี่คือ Layer นอกสุดที่ผู้ใช้หรือระบบอื่นจะเข้ามามีปฏิสัมพันธ์ด้วย ในโปรเจกต์นี้คือ Web API ที่รับส่งข้อมูลผ่านโปรโตคอล HTTP

- **ส่วนประกอบหลัก:**
  - **Controllers**: เป็นจุดเริ่มต้น (Entry Point) ของทุก Request เช่น `OrdersController`. หน้าที่ของมันคือรับ HTTP Request, แปลงข้อมูลที่เข้ามา (JSON) ให้อยู่ในรูปของ `Request Model`, ส่งต่อไปให้ `Application Service` ที่เหมาะสม, และสุดท้ายนำผลลัพธ์ที่ได้จาก Service มาแปลงเป็น `Response Model` เพื่อส่งกลับเป็น HTTP Response
  - **Request/Response Models**: เป็นคลาสที่กำหนดโครงสร้างของข้อมูลที่รับ-ส่งผ่าน API โดยเฉพาะ เช่น `CreateOrderRequest` คือข้อมูลที่ API คาดหวังจะได้รับสำหรับการสร้าง Order ใหม่ และ `OrderResponse` คือข้อมูลที่ API จะส่งกลับไป
  - **Middleware**: เป็นโค้ดที่ทำงานแทรกกลางระหว่างการรับ Request และการส่ง Response ใช้สำหรับจัดการงานที่ต้องทำเหมือนๆ กันในทุก Request (Cross-cutting Concerns) ตัวอย่างที่ชัดเจนคือ `GlobalExceptionHandlingMiddleware` ที่คอยดักจับ Error ที่เกิดขึ้นในส่วนต่างๆ ของแอปพลิเคชัน แล้วแปลงให้เป็น HTTP Response ที่มีมาตรฐานเดียวกัน
  - **Validators**: ใช้สำหรับตรวจสอบความถูกต้องของ `Request Model` ที่ส่งเข้ามา เช่น `CreateOrderRequestValidator` จะใช้ FluentValidation เพื่อเช็คว่าข้อมูลที่ส่งมาครบถ้วนและอยู่ในรูปแบบที่ถูกต้องหรือไม่ ก่อนที่จะส่งต่อไปยัง Application Layer

---

### **3. The Dependency Rule: หลักการสำคัญที่สุด**

กฎที่สำคัญที่สุดในสถาปัตยกรรมนี้คือ **Dependency Rule** ซึ่งก็คือ Dependencies ทั้งหมดจะต้องชี้เข้าหา Layer ด้านในเท่านั้น: **Presentation → Application → Domain** และ **Infrastructure → Application & Domain** ซึ่งหมายความว่า **Domain** Layer จะถูกแยกและป้องกันจากการเปลี่ยนแปลงใน Layer อื่นๆ อย่างสิ้นเชิง

---

### **4. ประโยชน์ในโลกแห่งความเป็นจริง: สถาปัตยกรรมที่พร้อมรับมือทุกการเปลี่ยนแปลง**

ทฤษฎีก็ส่วนหนึ่ง แต่สิ่งที่พิสูจน์คุณค่าของสถาปัตยกรรมนี้คือการรับมือกับการเปลี่ยนแปลงที่เกิดขึ้นจริงในระหว่างการพัฒนาโปรเจกต์ ลองมาดูสถานการณ์สมมติต่างๆ ที่นักพัฒนามักจะต้องเจอ และดูว่าสถาปัตยกรรมนี้ช่วยให้ชีวิตง่ายขึ้นได้อย่างไร

#### **สถานการณ์ที่ 1: การย้ายฐานข้อมูล (Database Migration)**

- **โจทย์:** ธุรกิจตัดสินใจว่าต้องการย้ายระบบจาก **SQL Server** ไปใช้ **MongoDB**
- **ผลลัพธ์:** การเปลี่ยนแปลงทางเทคนิคที่สำคัญ ถูกจำกัดขอบเขตไว้แค่ใน **Infrastructure Layer** เท่านั้น (แก้ไข Repository Implementations และ `appsettings.json`) Business Logic ทั้งหมดใน Domain และ Application Layer ปลอดภัยและไม่ต้องแตะต้องเลย

#### **สถานการณ์ที่ 2: การสร้าง Endpoint ใหม่สำหรับ Mobile App**

- **โจทย์:** ทีม Mobile App ต้องการ API ที่ส่งข้อมูลสรุปของ Order (เฉพาะ `Id`, `Status`, `CustomerName`) เพื่อลดการใช้ Bandwidth
- **ผลลัพธ์:** การเปลี่ยนแปลงทั้งหมดเกิดขึ้นที่ **Presentation Layer** เท่านั้น โดยการสร้าง `Response Model` ใหม่ และเพิ่ม `Endpoint` ใหม่ใน `OrdersController` โดยสามารถใช้ `Application Service` เดิมได้ทันที ไม่กระทบส่วนอื่นเลย

#### **สถานการณ์ที่ 3: การเพิ่มกฎทางธุรกิจใหม่**

- **โจทย์:** ธุรกิจออกนโยบายใหม่ว่า **"ไม่อนุญาตให้แก้ไขรายการสินค้าใน Order ที่ถูกจัดส่งไปแล้ว"**
- **ผลลัพธ์:** กฎทางธุรกิจที่สำคัญนี้ถูกนำไปใช้ในจุดที่ถูกต้องที่สุดคือ **Domain Layer** (โดยการเพิ่ม Logic ตรวจสอบใน `OrderEntity`) และมีการเรียกใช้กฎนี้ใน **Application Layer** ทำให้มั่นใจได้ 100% ว่าจะไม่มีใครสามารถละเมิดกฎนี้ได้ ไม่ว่าจะเรียกใช้จากช่องทางไหนก็ตาม

---

### **5. การตรวจสอบความถูกต้องอย่างมีชั้นเชิง (Defense in Depth)**

โปรเจกต์นี้ใช้กลยุทธ์ **"Defense in Depth"** ในการตรวจสอบความถูกต้องของข้อมูล โดยมีการวางด่านตรวจไว้ในหลาย Layer เพื่อให้มั่นใจว่าข้อมูลถูกต้องและสถานะของระบบยังคงสมบูรณ์อยู่เสมอ ภาพด้านล่างแสดงเส้นทางการทำงานของ Request และจุดที่มีการตรวจสอบ Validation ในแต่ละ Layer

[TAG\_REQUEST\_FLOW\_DIAGRAM]

#### **ด่านที่ 1: Presentation Layer - "ตรวจบัตรผ่านประตู"**

- **หน้าที่:** ตรวจสอบ "รูปแบบ" (Format) ของข้อมูลที่ผู้ใช้ส่งเข้ามาด้วย **FluentValidation**
- **ตัวอย่างโค้ด:** ใน `CreateOrderRequestValidator.cs` จะมีการกำหนดกฎพื้นฐาน:

  ```csharp
  public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
  {
      public CreateOrderRequestValidator()
      {
          RuleFor(x => x.CustomerId)
              .NotEmpty() // ต้องไม่เป็นค่าว่าง
              .WithMessage("Customer ID is required")
              .Must(BeValidGuid) // ต้องเป็น GUID ที่ถูกต้อง
              .WithMessage("Customer ID must be a valid GUID");

          RuleFor(x => x.Items)
              .NotEmpty() // ต้องมีรายการสินค้าอย่างน้อย 1 ชิ้น
              .WithMessage("Order must have at least one item");
      }
      // ...
  }
  ```

  หากข้อมูลที่ส่งมาไม่ผ่านกฎเหล่านี้ API จะตอบกลับเป็น `400 Bad Request` ทันที

#### **ด่านที่ 2: Application Layer - "ตรวจสอบความถูกต้องของคำสั่ง"**

- **หน้าที่:** ตรวจสอบความสมเหตุสมผลของคำสั่งในบริบทของ Use Case โดยมักจะต้องเช็คกับข้อมูลใน Database
- **ตัวอย่างโค้ด:** ใน `OrderService.cs` ก่อนจะสร้าง Order จะมีการเรียกเมธอด `EnsureCanCreateOrderAsync`:

  ```csharp
  private async Task EnsureCanCreateOrderAsync(
      CreateOrderRequest request,
      IEnumerable<ProductEntity> products
  )
  {
      var customer =
          await _customerRepository.GetCustomerByIdAsync(request.CustomerId)
          ?? throw new ValidationException("Customer not found"); // เช็คว่าลูกค้ามีจริงหรือไม่

      if (!customer.IsActive)
          throw new ValidationException("Customer account is not active"); // เช็คว่าลูกค้า Active หรือไม่

      foreach (var item in request.Items)
      {
          var product =
              products.FirstOrDefault(p => p.Id == item.ProductId)
              ?? throw new ValidationException($"Product {item.ProductId} not found");

          if (product.StockQuantity < item.Quantity) // เช็คสต็อกสินค้า
              throw new ValidationException($"Insufficient stock for product {product.Name}");
      }
  }
  ```

#### **ด่านที่ 3: Domain Layer - "ผู้พิทักษ์กฎเหล็กของธุรกิจ"**

- **หน้าที่:** ปกป้องความสมบูรณ์ (Integrity) ของ `Entity` และบังคับใช้ "กฎเหล็ก" ของธุรกิจที่ไม่ว่า Use Case ไหนๆ ก็ต้องปฏิบัติตาม
- **ตัวอย่างโค้ด:** ใน `OrderEntity.cs` มีการป้องกันการเปลี่ยนสถานะที่ไม่ถูกต้องภายในตัว `Entity` เอง:

  ```csharp
  public class OrderEntity : BaseEntity
  {
      // ... other properties

      public void ChangeStatus(OrderStatus newStatus)
      {
          if (!IsValidStatusTransition(Status, newStatus)) // ตรวจสอบกฎก่อนเปลี่ยนสถานะ
              throw new DomainException($"Cannot transition from {Status} to {newStatus}");

          Status = newStatus;
      }

      private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
      {
          return (current, next) switch // กฎเหล็กของการเปลี่ยนสถานะ
          {
              (OrderStatus.Pending, OrderStatus.Confirmed) => true,
              (OrderStatus.Confirmed, OrderStatus.Shipped) => true,
              (OrderStatus.Shipped, OrderStatus.Delivered) => true,
              (_, OrderStatus.Cancelled) => current != OrderStatus.Delivered,
              _ => false,
          };
      }
  }
  ```
