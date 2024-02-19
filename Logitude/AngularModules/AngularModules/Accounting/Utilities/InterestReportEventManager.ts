import { EventEmitter } from '@angular/core';
import { EventParams } from './ReconcileEventManager';
export class InterestReportEventManager {
  static SelectAllEvent: EventEmitter<EventParams> = new EventEmitter();
}
