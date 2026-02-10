
# SOLID Principles — .NET Core & TypeScript (Interview‑Ready Guide)

A crisp, interview‑oriented guide with short answers, ready examples, and talking points. Includes .NET Core (C#) and TypeScript snippets you can copy‑paste.

---

## 🚀 Elevator Pitch (30 seconds)
**SOLID** is a set of 5 design principles for maintainable, testable, and extensible software:
- **S**ingle Responsibility — one reason to change per class.
- **O**pen/Closed — extend behavior without modifying existing code.
- **L**iskov Substitution — subclasses/implementations must be replaceable without breaking behavior.
- **I**nterface Segregation — prefer many small, specific interfaces over fat ones.
- **D**ependency Inversion — depend on abstractions, not concretions.

**Benefit**: Lower coupling, higher cohesion, easier testing, safer refactors.

> **Hinglish one‑liner**: “Classes ko ek kaam do, naye features extend se lao, bacche parent jaisa behave karein, chhote targeted interfaces banao, aur interfaces pe depend karo — code mast maintainable ho jayega.”

---

## 🎯 Interview Answers (60s & 120s)
**60 seconds**
> “SOLID is about writing clean, extensible code. SRP keeps classes focused. OCP lets me add new features by adding new classes, not editing old ones. LSP ensures my implementations don’t break expectations. ISP avoids forcing clients to implement unnecessary methods. DIP makes high‑level logic depend on interfaces, so I can swap implementations easily and unit test with mocks. Together they reduce regression risk and improve velocity.”

**120 seconds**
> “In my recent projects, I applied SRP by splitting calculation, persistence, and notifications into separate services. For OCP, I built a discount engine where new promotions are added as new `IDiscountRule` classes and wired via DI — no changes to checkout code. LSP guided me to design interfaces so any rule can replace another without breaking totals. ISP helped by defining small capabilities like `IPrintable`, `IScannable` for devices. Finally, DIP allowed services to rely on abstractions like `INotifier`/`PatientStore`, enabling easy switches (Email to SMS, REST to IndexedDB) and fast unit tests with mocks.”

---

## 📦 SRP — Single Responsibility Principle
**Definition**: One class/module should have one reason to change.

### ❌ Anti‑pattern (God class)
```csharp
public class ServiceJobService {
    public decimal CalculateLaborCost(ServiceJob job) { /* ... */ }
    public Task SaveAsync(ServiceJob job) { /* EF Core */ }
    public Task SendCompletionEmailAsync(ServiceJob job) { /* SMTP */ }
}
```

### ✅ Refactor (Focused classes)
```csharp
public class LaborCostCalculator { public decimal Calculate(ServiceJob job) { /* math only */ } }
public class ServiceJobRepository { private readonly AppDbContext _db; public ServiceJobRepository(AppDbContext db)=>_db=db; public Task SaveAsync(ServiceJob job)=>_db.SaveChangesAsync(); }
public class JobNotifier { public Task SendCompletionAsync(ServiceJob job) { /* email */ return Task.CompletedTask; } }
```

**TypeScript**
```ts
class PatientValidator { isValid(p: any) { /* ... */ } }
class PatientMapper { toApi(p: any) { /* ... */ } }
class PatientApi { constructor(private http: HttpClient) {} save(x: any) { return this.http.post('/api/patient', x); } }
```

**Talking point**: Focused classes are simpler to test and change.

---

## 🧩 OCP — Open/Closed Principle
**Definition**: Open for extension, closed for modification.

### ✅ .NET Core — Discount rules (extend by adding new classes)
```csharp
public interface IDiscountRule { decimal Apply(decimal total, Cart cart); }

public class NewYearDiscount : IDiscountRule { public decimal Apply(decimal total, Cart cart) => total * 0.90m; }
public class LoyaltyDiscount : IDiscountRule { public decimal Apply(decimal total, Cart cart) => cart.Customer.IsGold ? total * 0.95m : total; }

public class CheckoutService {
    private readonly IEnumerable<IDiscountRule> _rules;
    public CheckoutService(IEnumerable<IDiscountRule> rules)=>_rules=rules;
    public decimal CalculateTotal(Cart cart){
        var total = cart.Items.Sum(i => i.Price * i.Qty);
        return _rules.Aggregate(total, (t, r) => r.Apply(t, cart));
    }
}
// Program.cs: services.AddScoped<IDiscountRule, NewYearDiscount>(); services.AddScoped<IDiscountRule, LoyaltyDiscount>();
```

### ✅ TypeScript — Pluggable pricing rules
```ts
interface PricingRule { apply(total: number, ctx: any): number; }
class OemCampaignRule implements PricingRule { apply(t: number, ctx: any) { return ctx.oemActive ? t * 0.9 : t; } }
class GstRule implements PricingRule { apply(t: number) { return t * 1.18; } }
class Billing { constructor(private rules: PricingRule[]) {} compute(base: number, ctx: any){ return this.rules.reduce((t, r) => r.apply(t, ctx), base); } }
```

**Talking point**: New rules ship as new classes; no risky edits to core logic.

---

## 🔁 LSP — Liskov Substitution Principle
**Definition**: Instances of a subtype must be usable wherever the supertype is expected — without changing correctness.

### ❌ Anti‑pattern (child narrows behavior)
```ts
class User { placeOrder() {/* ... */} }
class GuestUser extends User { placeOrder() { throw new Error('Login required'); } } // breaks substitution
```

### ✅ Design with correct abstraction
```ts
interface OrderPlacer { placeOrder(): void; }
class RegisteredUser implements OrderPlacer { placeOrder(){ /* ... */ } }
class GuestUser { /* browse/cart only */ }
```

**.NET Tip**: Avoid throwing `NotSupportedException` from overridden members of an abstract base — split interfaces instead.

---

## 🪁 ISP — Interface Segregation Principle
**Definition**: Prefer many small, client‑specific interfaces.

### ✅ .NET Core — Device capabilities
```csharp
public interface IPrintable { void Print(); }
public interface IScannable { void Scan(); }
public interface IDiagnostic { void DiagnoseVehicle(); }

public class TyrePressureSensor : IDiagnostic { public void DiagnoseVehicle(){ /* ... */ } }
```

### ✅ TypeScript — Focused interfaces
```ts
interface ReadsHeartRate { readHR(): number; }
interface ReadsBP { readBP(): { sys: number; dia: number }; }
class HRWearable implements ReadsHeartRate { readHR(){ return 72; } }
class BPMonitor implements ReadsBP { readBP(){ return { sys:120, dia:80 }; } }
```

**Talking point**: No class is forced to implement methods it doesn’t use.

---

## 🧱 DIP — Dependency Inversion Principle
**Definition**: High‑level modules depend on abstractions; details depend on abstractions.

### ✅ .NET Core — Notifications via interface
```csharp
public interface INotifier { Task SendAsync(string to, string msg); }
public class EmailNotifier : INotifier { public Task SendAsync(string to, string msg)=>Task.CompletedTask; }
public class SmsNotifier : INotifier { public Task SendAsync(string to, string msg)=>Task.CompletedTask; }

public class OrderService {
    private readonly INotifier _notifier;
    public OrderService(INotifier notifier)=>_notifier=notifier;
    public async Task PlaceOrderAsync(Order order){ /* ... */ await _notifier.SendAsync(order.CustomerEmail, "Order placed"); }
}
```

**Unit test sketch**
```csharp
[Fact]
public async Task PlaceOrder_SendsNotification(){
    var mock = new Mock<INotifier>();
    var svc = new OrderService(mock.Object);
    await svc.PlaceOrderAsync(new Order{ CustomerEmail="x@y.com"});
    mock.Verify(n=>n.SendAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
}
```

### ✅ TypeScript — Storage abstraction
```ts
interface PatientStore { save(p: any): Promise<void>; get(id: string): Promise<any>; }
class RestPatientStore implements PatientStore { constructor(private http: HttpClient) {} save(p:any){return this.http.post('/api/patient', p).toPromise();} get(id:string){return this.http.get(`/api/patient/${id}`).toPromise();} }
class IndexedDbPatientStore implements PatientStore { async save(p:any){ /* idb */ } async get(id:string){ /* idb */ } }
class PatientService { constructor(private store: PatientStore) {} register(p:any){ return this.store.save(p); } }
```

**Talking point**: Swap implementations via DI/config; testing uses fakes.

---

## 🧪 Red Flags & Fixes
- **SRP smell**: Class talks to DB, calculates totals, and sends emails → split responsibilities.
- **OCP smell**: `switch`/`if-else` explosion for types → introduce strategy + DI.
- **LSP smell**: Overridden method throws `NotSupportedException` → split interface.
- **ISP smell**: One interface with many unrelated methods → break into capabilities.
- **DIP smell**: `new Concrete()` inside business logic → inject abstraction.

---

## 🧠 FAQs
- **Is SOLID only for OOP?** Mostly OOP, but ideas (cohesion, boundaries) help in FP too.
- **Is OCP over‑engineering?** Keep YAGNI in mind; add extensibility where churn is expected.
- **Interfaces vs Abstract classes?** Prefer interfaces for contracts; use abstract classes when partial implementation is shared.

---

## 📝 30‑second Whiteboard Summary
```
SRP: 1 class = 1 reason to change
OCP: Add new behavior by adding new classes
LSP: Subtypes don’t surprise callers
ISP: Small, role‑based interfaces
DIP: High‑level depends on interfaces
```

---

## 🧩 Real‑world Scenarios
- **DMS**: Cost calculators (labor/parts) + pluggable discount rules + `INotifier` for email/SMS.
- **Healthcare**: `PatientStore` abstraction (REST ↔ IndexedDB), validators and mappers split.
- **E‑commerce**: Payment provider strategy (Razorpay/Stripe) behind `IPaymentGateway`.

---

## ✅ Quick Checklist (Before PR)
- [ ] Each class has a single reason to change (SRP)
- [ ] New rules/providers are plug‑ins, not edits (OCP)
- [ ] No overridden methods throw `NotSupported` (LSP)
- [ ] Interfaces are lean and role‑based (ISP)
- [ ] No `new` inside business services; all deps injected (DIP)

---

## 📚 Further Reading
- *Agile Software Development: Principles, Patterns, and Practices* — Robert C. Martin
- Martin Fowler — Articles on refactoring, design principles

---

**Use this README in interviews**: Start with the elevator pitch, pick 1–2 concrete examples (discount rules, notifier abstraction), and end with benefits (low coupling, high testability).
