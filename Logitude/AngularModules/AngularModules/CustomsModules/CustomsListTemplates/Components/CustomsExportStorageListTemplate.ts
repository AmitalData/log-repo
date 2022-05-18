import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralList } from '../../../Customs/EntityLists/CustomsCollateralList';
import { CustomsCollateralAnswerSharedDataService } from '../../../Customs/Services/DataChange/CustomsCollateralAnswerSharedDataService'
import { Declaration } from 'typescript';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { AppTool } from '../../../Infrastructure/Tools';
import { ContainerizationExtendedListService } from '../../../Customs/Services/ExtendedLists/ContainerizationExtendedListService';
import { ExportStoragePM } from 'Customs/EntityPMs/ExportStoragePM';
import { ExportStorageExtendedListService } from 'Customs/Services/ExtendedLists/ExportStorageExtendedListService';


@Component({   
    templateUrl: 'CustomsExportStorageListTemplate.html',
})

export class CustomsExportStorageListTemplate {
    public rowData: any;
    public IsDisplayOnly: boolean = false;
    public CustomsContainerizationRecord: DeclarationList;
    public fieldName: any;
    public isAnswer: boolean;
    public isDisable: boolean;
    TableUpdateButtonIsEnabled: boolean = false;
    UpdateButtonVisibility: boolean = false;
    TableUpdateButtonOpacity: string = "1";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    IsConnectedExportStorageChecked: boolean = true;

    //, private _customsCollateralAnswerSharedDataService: CustomsCollateralAnswerSharedDataService
    entityPM: ExportStoragePM
    constructor(private CD: ChangeDetectorRef, private _exportStorageExtendedListService: ExportStorageExtendedListService) {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {
            this.entityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM as ExportStoragePM;
        } else {
            this.entityPM = new ExportStoragePM();
        }
     }

    setVariables(rowData: any, fieldName: string, additionalData: any) {
        this.fieldName = fieldName;
        this.rowData = rowData;
        this.BuildDeclarationsCheckBox();       
        this.CD.detectChanges();
    }

    BuildDeclarationsCheckBox() {
        this.IsConnectedExportStorageChecked = false;
        if (this.entityPM.StorageNo && this.entityPM.Id != null && !this._exportStorageExtendedListService.connectedSelectAll) {
            this._exportStorageExtendedListService.ConnectedExportStorage = this.entityPM.StorageNo;
        }
        if (!this._exportStorageExtendedListService.ConnectedExportStorage) {
            this._exportStorageExtendedListService.ConnectedExportStorage = "";
        }
        if (!this._exportStorageExtendedListService.IsDirectCharging) {
            this._exportStorageExtendedListService.IsDirectCharging = "";
        }
        if (!this._exportStorageExtendedListService.AllExportStorage) {
            this._exportStorageExtendedListService.AllExportStorage = "";
        }
        if (!this._exportStorageExtendedListService.AllExportStorage.includes(this.rowData.Id)) {
            this._exportStorageExtendedListService.AllExportStorage = this._exportStorageExtendedListService.AllExportStorage + this.rowData.Id + ",";
        }
        let sConnectedDeclarations = this._exportStorageExtendedListService.ConnectedExportStorage as string;
        if (!AppTool.IsNullOrEmpty(sConnectedDeclarations)) {
            let ConnectedExportStorage = sConnectedDeclarations.split(',')
            let res = ConnectedExportStorage.filter(r => r == this.rowData.Id)[0];
            this.IsConnectedExportStorageChecked = !AppTool.IsNullOrEmpty(res);
        }
        if (this._exportStorageExtendedListService.connectedSelectAll == true) {
            this.IsConnectedExportStorageChecked = true;
        }
    }

    OnConnectedCheckBoxChecked($event) {
        this._exportStorageExtendedListService.disconnectedSelectAll = false;
         if ($event) {
             if (!this._exportStorageExtendedListService.ConnectedExportStorage.includes(this.rowData.Id)) {
                 this._exportStorageExtendedListService.ConnectedExportStorage = this._exportStorageExtendedListService.ConnectedExportStorage + this.rowData.Id + ",";
             }
             if (this.rowData.ProcedureCurrentName != null && !this._exportStorageExtendedListService.IsDirectCharging.includes(this.rowData.Id) &&
                 this.rowData.ProcedureCurrentName.includes("טעינה ישירה")) {
                 this._exportStorageExtendedListService.IsDirectCharging = this._exportStorageExtendedListService.IsDirectCharging + this.rowData.Id + ",";
             }
         }
         else {
             if (this._exportStorageExtendedListService.ConnectedExportStorage.includes(this.rowData.Id)) {
                 this._exportStorageExtendedListService.ConnectedExportStorage = this._exportStorageExtendedListService.ConnectedExportStorage.replace(this.rowData.Id + ",", "");
                 this._exportStorageExtendedListService.connectedSelectAll = false;
             }
             if (this._exportStorageExtendedListService.IsDirectCharging.includes(this.rowData.Id)) {
                 this._exportStorageExtendedListService.IsDirectCharging = this._exportStorageExtendedListService.IsDirectCharging.replace(this.rowData.Id + ",", "");
             }
         }
        if (AppTool.IsNullOrEmpty(this._exportStorageExtendedListService.ConnectedExportStorage)) {
            this._exportStorageExtendedListService.SelectedExportStorage = false;
         } else {
            this._exportStorageExtendedListService.SelectedExportStorage = true;
         }
    }
}


