
// Run with: ts-node examples.ts (or compile with tsc)
interface PricingRule { apply(total: number, ctx: any): number }
class GstRule implements PricingRule { apply(t: number) { return t * 1.18; } }
class OemCampaignRule implements PricingRule { apply(t: number, ctx: any) { return ctx.oemActive ? t * 0.9 : t; } }
class Billing { constructor(private rules: PricingRule[]) {} compute(base: number, ctx: any) { return this.rules.reduce((t, r) => r.apply(t, ctx), base); } }

// SRP split
class PatientValidator { isValid(p: any) { return !!p?.name; } }
class PatientApi { constructor(private http: { post: (u: string, b: any) => Promise<any> }) {} save(m: any) { return this.http.post('/api/patient', m); } }

// LSP fixed abstraction
interface OrderPlacer { placeOrder(): void }
class RegisteredUser implements OrderPlacer { placeOrder() { console.log('Order placed'); } }
class GuestUser { /* browse only */ }

// Demo
(async () => {
  const billing = new Billing([new OemCampaignRule(), new GstRule()]);
  const final = billing.compute(1000, { oemActive: true });
  console.log('Final Amount:', final);
})();
