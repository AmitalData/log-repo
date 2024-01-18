import {OnInit, Output, EventEmitter} from '@angular/core';
export class ReconcileEventManager {
     CheckBoxChecked: EventEmitter<any> = new EventEmitter(); // for ledger transactions
     ExtPageCheckBoxChecked: EventEmitter<any> = new EventEmitter(); // for bank account page lines
     RowUnselected: EventEmitter<any> = new EventEmitter();
     GLAccountReconcileMethodCode: string;
}
