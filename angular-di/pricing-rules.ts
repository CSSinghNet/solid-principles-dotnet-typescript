
import { PricingRule } from './tokens';

export class OemCampaignRule implements PricingRule {
  constructor(private active: boolean) {}
  apply(total: number): number { return this.active ? total * 0.9 : total; }
}

export class GstRule implements PricingRule {
  apply(total: number): number { return total * 1.18; }
}
