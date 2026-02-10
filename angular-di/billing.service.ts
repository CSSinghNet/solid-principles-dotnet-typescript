
import { Inject, Injectable } from '@angular/core';
import { APP_CONFIG, DISCOUNT_RULE, Logger, PricingRule } from './tokens';

@Injectable({ providedIn: 'root' })
export class BillingService {
  constructor(
    @Inject(DISCOUNT_RULE) private readonly rules: PricingRule[],
    @Inject(APP_CONFIG) private readonly cfg: { apiBaseUrl: string; featureFlags: { discounts: boolean } },
    @Inject('LOGGER') private readonly logger: Logger
  ) {}

  total(base: number, ctx: any): number {
    this.logger.log(`API: ${this.cfg.apiBaseUrl}`);
    if (!this.cfg.featureFlags.discounts) return base;
    return this.rules.reduce((t, r) => r.apply(t, ctx), base);
  }
}
