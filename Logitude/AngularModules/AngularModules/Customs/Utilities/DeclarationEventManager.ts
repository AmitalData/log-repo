import { OnInit, Output, EventEmitter } from '@angular/core';
export class DeclarationEventManager {
    static DisplayModeChanged: EventEmitter<any> = new EventEmitter();
    static DeclarationSplitDocumentSelection: EventEmitter<any> =
        new EventEmitter();
    static DeclarationSplitDocumentItemSelection: EventEmitter<any> =
        new EventEmitter();
    static ConsignmentsChanged: EventEmitter<any> = new EventEmitter();
    static DeclarationAmendmentCancelled: EventEmitter<any> =
        new EventEmitter();
    static AddDeclarationToContainerization: EventEmitter<any> =
        new EventEmitter();
    static SavePendingAfterDeclarationSaved: EventEmitter<any> =
        new EventEmitter();
    static DeclarationSplitDocumentItemSelectionByInvoice: EventEmitter<any> =
        new EventEmitter();
}
