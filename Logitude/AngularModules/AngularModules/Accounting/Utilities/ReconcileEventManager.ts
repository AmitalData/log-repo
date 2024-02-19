import { OnInit, Output, EventEmitter } from '@angular/core';
import { AppTool } from 'Infrastructure/Tools';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
export class ReconcileEventManager {
    static CheckBoxChecked: EventEmitter<EventParams> = new EventEmitter(); // for ledger transactions
    static ExtPageCheckBoxChecked: EventEmitter<EventParams> = new EventEmitter(); // for bank account page lines
    static RowUnselected: EventEmitter<EventParams> = new EventEmitter();

    private static GLAccountReconcileMethodCode: Map<number, string> = new Map<number, string>();
    static SetGLAccountReconcileMethodCode(val) {
        this.GLAccountReconcileMethodCode.set(SessionLocator.SelectedSession.SessionIndex, val);
    }
    static GetGLAccountReconcileMethodCode() {
        var val = this.GLAccountReconcileMethodCode.get(SessionLocator.SelectedSession.SessionIndex);
        return val;
    }

    static ManageReconciliationCheckBoxChecked: EventEmitter<EventParams> = new EventEmitter(); // for Manage Reconciliation

    private static SupperssOnRowSelectedAction: Map<number, boolean> = new Map<number, boolean>();
    static SetSupperssOnRowSelectedAction(val) {
        this.SupperssOnRowSelectedAction.set(SessionLocator.SelectedSession.SessionIndex, val);
    }
    static GetSupperssOnRowSelectedAction() {
        var val = this.SupperssOnRowSelectedAction.get(SessionLocator.SelectedSession.SessionIndex);
        return val;
    }

    private static SelectedItems: Map<number, ObservableCollection> = new Map<number, ObservableCollection>();

    static InsertIntoSelectedItems(val) {
        if (AppTool.IsNullOrUndefined(this.SelectedItems.get(SessionLocator.SelectedSession.SessionIndex))) {
            this.SelectedItems.set(SessionLocator.SelectedSession.SessionIndex, new ObservableCollection([]));
        }
        var current = this.SelectedItems.get(SessionLocator.SelectedSession.SessionIndex);
        if (!current.Collection.includes(val)) {
            current.Insert(val);
        }
    }

    static RemoveFromSelectedItems(val) {
        var selectedItems = this.SelectedItems.get(SessionLocator.SelectedSession.SessionIndex);
        if (selectedItems && selectedItems.Collection.includes(val)) {
            selectedItems.Remove(val);
        }
    }

    static GetSelectedItems() {
        return this.SelectedItems.get(SessionLocator.SelectedSession.SessionIndex)?.Collection;
    }

    private static IsAllSelected: Map<number, boolean> = new Map<number, boolean>();
    static SetIsAllSelected(val) {
        this.IsAllSelected.set(SessionLocator.SelectedSession.SessionIndex, val);
    }
    static GetIsAllSelected() {
        var val = this.IsAllSelected.get(SessionLocator.SelectedSession.SessionIndex);
        return val;
    }

    private static UnAllSelected: Map<number, boolean> = new Map<number, boolean>();
    static SetUnAllSelected(val) {
        this.UnAllSelected.set(SessionLocator.SelectedSession.SessionIndex, val);
    }
    static GetUnAllSelected() {
        var val = this.UnAllSelected.get(SessionLocator.SelectedSession.SessionIndex);
        return val;
    }
}

export class EventParams {
    SendSessionIndex: number = SessionLocator.SelectedSession.SessionIndex;
    Params: any;
}
