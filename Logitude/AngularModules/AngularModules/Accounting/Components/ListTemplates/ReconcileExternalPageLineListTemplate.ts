import {Component,ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
//import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {ReconcileEventManager} from '../../Utilities/ReconcileEventManager';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({

    templateUrl: './ReconcileExternalPageLineListTemplate.html',
})

export class ReconcileExternalPageLineListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;


    public isRTL: boolean = false;

    checkBoxState: boolean = false;
    get CheckBoxState() { return this.checkBoxState; }
    set CheckBoxState(isChecked: boolean) {
        console.log("Changed to: ", isChecked);
        this.checkBoxState = isChecked;

    }
    reconcileEventManager: ReconcileEventManager;
    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;
        this.reconcileEventManager = this.AdditionalData?.GridAdditionalData?.ReconcileEventManager;
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    CheckBoxClicked(checked: boolean) {
        console.log("clicked: ", checked);
        this.rowData['IsChecked'] = checked;
        if (!this.reconcileEventManager.CheckBoxChecked)
            this.reconcileEventManager.CheckBoxChecked = new EventEmitter();
        this.reconcileEventManager.CheckBoxChecked.emit({ line: this.rowData, isChecked: checked, RowIndex: this.AdditionalData.rowIndex });

    }
    ExtPageCheckBoxClicked(checked: boolean) {
        console.log("clicked: ", checked);
        // this.rowData['IsChecked'] = checked;


        this.reconcileEventManager.ExtPageCheckBoxChecked.emit({ line: this.rowData, isChecked: checked, RowIndex: this.AdditionalData.rowIndex });

    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }



}
