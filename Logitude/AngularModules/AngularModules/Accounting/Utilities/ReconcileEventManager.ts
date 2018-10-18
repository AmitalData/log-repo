import {OnInit, Output, EventEmitter} from '@angular/core';
export class ReconcileEventManager {

    static CheckBoxChecked: EventEmitter<any> = new EventEmitter(); // for ledger transactions
    static BankCheckBoxChecked: EventEmitter<any> = new EventEmitter(); // for bank account page lines
    static RowUnselected: EventEmitter<any> = new EventEmitter();
    static GLAccountReconcileMethodCode: string;

}