
import { Component, ChangeDetectorRef } from '@angular/core';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { OnInit, Output, EventEmitter, ComponentRef, QueryList } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
//import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { EventParams, ReconcileEventManager } from '../../Utilities/ReconcileEventManager';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({

    templateUrl: './ManageReconciliationListTemplate.html',
})

export class ManageReconciliationListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public fieldValue: any;

    public isRTL: boolean = false;


    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;


        if (fieldName == "SelectCheckBox") {
            this.BuildCheckBox();

        }

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    BuildCheckBox() {
        var selectedItems = ReconcileEventManager.GetSelectedItems();
        if (!AppTool.IsNullOrUndefined(selectedItems) && selectedItems.includes(this.rowData)) {
            this.CheckBoxClicked(true);
        }
        else {
            if (ReconcileEventManager.GetIsAllSelected() == true) {
                this.CheckBoxClicked(true);
            }
            if (ReconcileEventManager.GetUnAllSelected() == true) {
                this.CheckBoxClicked(false);
            }
        }
    }
    CheckBoxClicked(checked: boolean) {
        this.rowData['IsChecked'] = checked;
        var reconcileEventParams = new EventParams();
        reconcileEventParams.Params = {
            line: this.rowData,
            isChecked: checked,
            RowIndex: this.AdditionalData?.rowIndex
        };

        ReconcileEventManager.ManageReconciliationCheckBoxChecked.emit(reconcileEventParams);

    }
    handleDivClick() {
        ReconcileEventManager.SetSupperssOnRowSelectedAction(true);
    }

}
