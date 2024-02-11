import {OnInit, Output, EventEmitter} from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
export class ReconcileEventManager {
    static CheckBoxChecked: EventEmitter<EventParams> = new EventEmitter(); // for ledger transactions
    static ExtPageCheckBoxChecked: EventEmitter<EventParams> = new EventEmitter(); // for bank account page lines
    static RowUnselected: EventEmitter<EventParams> = new EventEmitter();
    
    private static GLAccountReconcileMethodCode: Map<number,string>=new Map<number,string>();
    static SetGLAccountReconcileMethodCode(val){
        this.GLAccountReconcileMethodCode.set(SessionLocator.SelectedSession.SessionIndex, val);
    }
    static GetGLAccountReconcileMethodCode(){
        var val=this.GLAccountReconcileMethodCode.get(SessionLocator.SelectedSession.SessionIndex);
        return val;
    }
}

export class EventParams {
    SendSessionIndex:number = SessionLocator.SelectedSession.SessionIndex;
    Params:any;
}
