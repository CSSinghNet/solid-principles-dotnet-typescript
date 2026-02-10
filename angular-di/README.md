
# Angular DI — Providers Array + InjectionToken (NgModule-based)

## Files
- `tokens.ts`: defines `APP_CONFIG`, `LOGGER`, `DISCOUNT_RULE` and interfaces
- `console-logger.service.ts`: `Logger` implementation (class provider)
- `pricing-rules.ts`: `OemCampaignRule`, `GstRule` (pluggable strategy rules)
- `billing.service.ts`: injects `APP_CONFIG`, `LOGGER`, and multi `DISCOUNT_RULE[]`
- `di-example.module.ts`: providers array with **value**, **class**, and **factory** (multi) providers

## Use
1. Add `DiExampleModule` to your app module imports or any feature module.
2. Inject `BillingService` in any component and call `total(base, ctx)`.

```ts
constructor(private billing: BillingService) {}
ngOnInit(){
  const amount = this.billing.total(1000, { oemActive: true });
  // Logs API base URL and applies Oem + GST rules
}
```

> Works in Angular v15+ with NgModules. For standalone apps, move the providers into `bootstrapApplication` or a `provide*` function.
