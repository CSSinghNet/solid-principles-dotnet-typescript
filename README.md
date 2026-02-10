# SOLID Principles — .NET Core & TypeScript (Interview‑Ready Kit)

This kit contains:
- **README.md** with interview answers (30s/60s/120s), red flags, FAQs, and checklists.
- **/dotnet/** minimal .NET Core examples (OCP + DIP + unit test sketch).
- **/typescript/** minimal TypeScript examples (SRP/ISP/LSP and pricing rule pipeline).

Use this in an interview: start with the Elevator Pitch, then walk through one concrete example (Discount Rules + DI), and close with benefits (low coupling, high testability).

---

## 🚀 Elevator Pitch (30 seconds)
SOLID are 5 principles for maintainable, testable, extensible code:
- **S**ingle Responsibility — one reason to change per class.
- **O**pen/Closed — extend without modifying existing code.
- **L**iskov Substitution — implementations are replaceable without breaking behavior.
- **I**nterface Segregation — small, role-based interfaces.
- **D**ependency Inversion — depend on abstractions, not concretes.

**Hinglish:** "Class ko ek kaam do, naye features extend se lao, child parent jaisa behave kare, chhote interfaces banao, aur interfaces pe depend karo."

---

## 🧩 .NET Core — OCP + DIP Example (Discount Engine)
```csharp
public interface IDiscountRule { decimal Apply(decimal total, Cart cart); }
public class NewYearDiscount : IDiscountRule { public decimal Apply(decimal t, Cart c) => t * 0.90m; }
public class LoyaltyDiscount : IDiscountRule { public decimal Apply(decimal t, Cart c) => c.Customer.IsGold ? t * 0.95m : t; }
public class CheckoutService {
    private readonly IEnumerable<IDiscountRule> _rules;
    public CheckoutService(IEnumerable<IDiscountRule> rules)=>_rules=rules;
    public decimal CalculateTotal(Cart cart){ var total = cart.Items.Sum(i=>i.Price*i.Qty); return _rules.Aggregate(total,(t,r)=>r.Apply(t,cart)); }
}
// Program.cs:
// services.AddScoped<IDiscountRule, NewYearDiscount>();
// services.AddScoped<IDiscountRule, LoyaltyDiscount>();
```

## 🪁 TypeScript — ISP/LSP/SRP Quickies
```ts
interface PricingRule { apply(total:number, ctx:any): number }
class GstRule implements PricingRule { apply(t:number){ return t*1.18; } }
class OemCampaignRule implements PricingRule { apply(t:number, ctx:any){ return ctx.oemActive? t*0.9 : t; } }
class Billing { constructor(private rules: PricingRule[]){} compute(base:number, ctx:any){ return this.rules.reduce((t,r)=>r.apply(t,ctx), base); } }

// SRP split
class PatientValidator { isValid(p:any){ return !!p?.name; } }
class PatientApi { constructor(private http:any){} save(m:any){ return this.http.post('/api/patient', m); } }

// LSP: correct abstraction
interface OrderPlacer { placeOrder():void }
class RegisteredUser implements OrderPlacer { placeOrder(){ /* ... */ } }
class GuestUser { /* browse only */ }
```

---

## ✅ Checklist (Before PR)
- [ ] SRP: each class has one reason to change
- [ ] OCP: new behavior via new classes, not edits
- [ ] LSP: no overridden members throwing NotSupported
- [ ] ISP: small capability interfaces
- [ ] DIP: no `new` concretes in business layer; use DI

---

## 🧠 Red Flags
- God classes, switch/if-else explosions, fat interfaces, `new` inside services, children that restrict parent behavior.

## 🧪 Unit Testing Tip
Mock abstractions (`INotifier`, `IDiscountRule`) to test high-level services in isolation.
