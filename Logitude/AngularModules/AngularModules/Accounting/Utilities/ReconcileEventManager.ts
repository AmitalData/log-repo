import {OnInit, Output, EventEmitter} from '@angular/core';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
export class ReconcileEventManager {

    static CheckBoxChecked: EventEmitter<any> = new EventEmitter(); // for ledger transactions
    static ExtPageCheckBoxChecked: EventEmitter<any> = new EventEmitter(); // for bank account page lines
    static RowUnselected: EventEmitter<any> = new EventEmitter();
    static GLAccountReconcileMethodCode: string;


    static ManageReconciliationCheckBoxChecked: EventEmitter<any> = new EventEmitter(); // for Manage Reconciliation
    static SupperssOnRowSelectedAction: boolean = false; // for Manage Reconciliation
    static _SelectedItems: ObservableCollection = new ObservableCollection([]); // for Manage Reconciliation
    static IsAllSelected:boolean=false; // for Manage Reconciliation
 

}
