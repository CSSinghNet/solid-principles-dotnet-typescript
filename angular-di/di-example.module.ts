
import { NgModule } from '@angular/core';
import { APP_CONFIG, DISCOUNT_RULE, LOGGER } from './tokens';
import { ConsoleLoggerService } from './console-logger.service';
import { GstRule, OemCampaignRule } from './pricing-rules';

@NgModule({
  providers: [
    // Value provider (environment/config)
    { provide: APP_CONFIG, useValue: { apiBaseUrl: 'https://api.example.com', featureFlags: { discounts: true } } },

    // Class provider (Logger)
    { provide: LOGGER, useClass: ConsoleLoggerService },

    // Factory providers for multi token (OCP-style plugins)
    { provide: DISCOUNT_RULE, multi: true, useFactory: () => new OemCampaignRule(true) },
    { provide: DISCOUNT_RULE, multi: true, useClass: GstRule },
  ]
})
export class DiExampleModule {}
