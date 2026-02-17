import {OnInit, Output, EventEmitter} from '@angular/core';
export class DeclarationEventManager {
    static DisplayModeChanged: EventEmitter<any> = new EventEmitter();
    static DeclarationSplitDocumentSelection: EventEmitter<any> = new EventEmitter();
    static ConsignmentsChanged: EventEmitter<any> = new EventEmitter();
}