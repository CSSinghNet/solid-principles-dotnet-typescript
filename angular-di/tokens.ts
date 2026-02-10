
import { InjectionToken } from '@angular/core';

export interface AppConfig {
  apiBaseUrl: string;
  featureFlags: { discounts: boolean };
}

export const APP_CONFIG = new InjectionToken<AppConfig>('APP_CONFIG');

export interface Logger { log(message: string): void; }
export const LOGGER = new InjectionToken<Logger>('LOGGER');

export interface PricingRule {
  apply(total: number, ctx: any): number;
}

// Multi-token for OCP-style rule plugins
export const DISCOUNT_RULE = new InjectionToken<PricingRule>('DISCOUNT_RULE');
