import { EventEmitter } from '@angular/core';
import { EventParams } from './ReconcileEventManager';
export class AccountingEventManager {
  static CustomerChangedEvent: EventEmitter<EventParams> = new EventEmitter();
  static TasksSchedulerId: EventEmitter<string> = new EventEmitter();

}
