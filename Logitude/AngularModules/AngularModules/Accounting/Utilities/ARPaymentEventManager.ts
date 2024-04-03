import {OnInit, Output, EventEmitter} from '@angular/core';
import { EventParams } from './ReconcileEventManager';
export class ARPaymentEventManager {

    static ARPaymentApproved: EventEmitter<EventParams> = new EventEmitter();
    // static IsARPaymentDisabled: boolean = false;

}
