
import { Injectable } from '@angular/core';
import { Logger } from './tokens';

@Injectable({ providedIn: 'root' })
export class ConsoleLoggerService implements Logger {
  log(message: string): void { console.log('[APP]', message); }
}
