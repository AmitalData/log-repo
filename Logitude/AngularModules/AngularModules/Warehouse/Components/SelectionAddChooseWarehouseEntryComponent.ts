import {Component, OnInit, ViewChildren, QueryList} from '@angular/core';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'SelectionAddChooseWarehouseEntryComponent',
    templateUrl: './SelectionAddChooseWarehouseEntryComponent.html',
})

export class SelectionAddChooseWarehouseEntryComponent extends BaseComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {
       
    }

    NewWarehouseEntryButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("NewCrossDockEntry");
    }

    ChooseWarehouseEntryButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("ChooseCrossDockEntry");
    }
}
