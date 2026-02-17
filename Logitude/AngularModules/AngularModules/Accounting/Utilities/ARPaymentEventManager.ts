import {OnInit, Output, EventEmitter} from '@angular/core';
export class ARPaymentEventManager {

    static ARPaymentApproved: EventEmitter<any> = new EventEmitter();
    // static IsARPaymentDisabled: boolean = false;

}
