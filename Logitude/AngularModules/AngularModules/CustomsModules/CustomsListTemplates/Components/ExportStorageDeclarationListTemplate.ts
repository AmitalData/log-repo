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
import { ExportStorageDeclarationExtendedList } from 'Customs/Services/ExtendedLists/ExportStorageDeclarationExtendedListService';


@Component({   
    templateUrl: 'ExportStorageDeclarationListTemplate.html',
})

export class ExportStorageDeclarationListTemplate {
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

    IsConnectedDeclarationChecked: boolean = true;

    //, private _customsCollateralAnswerSharedDataService: CustomsCollateralAnswerSharedDataService
    entityPM: ExportStoragePM;
    constructor(private CD: ChangeDetectorRef, private _exportStorageDeclarationExtendedListService: ExportStorageDeclarationExtendedList) {
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
        // this.IsConnectedDeclarationChecked = false;
        // if (this.entityPM.StorageNo && this.entityPM.Id != null && !this._containerizationExtendedListService.connectedSelectAll) {
        //     this._exportStorageDeclarationExtendedListService.ConnectedDeclarations = this.entityPM.StorageNo;
        // }
        // if (!this._exportStorageDeclarationExtendedListService.ConnectedDeclarations) {
        //     this._exportStorageDeclarationExtendedListService.ConnectedDeclarations = "";
        // }
        // if (!this._exportStorageDeclarationExtendedListService.IsDirectCharging) {
        //     this._exportStorageDeclarationExtendedListService.IsDirectCharging = "";
        // }
        // if (!this._exportStorageDeclarationExtendedListService.AllDeclarations) {
        //     this._exportStorageDeclarationExtendedListService.AllDeclarations = "";
        // }
        // if (!this._exportStorageDeclarationExtendedListService.AllDeclarations.includes(this.rowData.Id)) {
        //     this._exportStorageDeclarationExtendedListService.AllDeclarations = this._exportStorageDeclarationExtendedListService.AllDeclarations + this.rowData.Id + ",";
        // }
        // let sConnectedDeclarations = this._exportStorageDeclarationExtendedListService.ConnectedDeclarations as string;
        // if (!AppTool.IsNullOrEmpty(sConnectedDeclarations)) {
        //     let ConnectedDeclarations = sConnectedDeclarations.split(',')
        //     let res = ConnectedDeclarations.filter(r => r == this.rowData.Id)[0];
        //     this.IsConnectedDeclarationChecked = !AppTool.IsNullOrEmpty(res);
        // }
        // if (this._exportStorageDeclarationExtendedListService.connectedSelectAll == true) {
        //     this.IsConnectedDeclarationChecked = true;
        // }
    }

    OnConnectedCheckBoxChecked($event) {
        // this._containerizationExtendedListService.disconnectedSelectAll = false;
        // if ($event) {
        //     if (!this._containerizationExtendedListService.ConnectedDeclarations.includes(this.rowData.Id)) {
        //         this._containerizationExtendedListService.ConnectedDeclarations = this._containerizationExtendedListService.ConnectedDeclarations + this.rowData.Id + ",";
        //     }
        //     if (this.rowData.ProcedureCurrentName != null && !this._containerizationExtendedListService.IsDirectCharging.includes(this.rowData.Id) &&
        //         this.rowData.ProcedureCurrentName.includes("טעינה ישירה")) {
        //         this._containerizationExtendedListService.IsDirectCharging = this._containerizationExtendedListService.IsDirectCharging + this.rowData.Id + ",";
        //     }
        // }
        // else {
        //     if (this._containerizationExtendedListService.ConnectedDeclarations.includes(this.rowData.Id)) {
        //         this._containerizationExtendedListService.ConnectedDeclarations = this._containerizationExtendedListService.ConnectedDeclarations.replace(this.rowData.Id + ",", "");
        //         this._containerizationExtendedListService.connectedSelectAll = false;
        //     }
        //     if (this._containerizationExtendedListService.IsDirectCharging.includes(this.rowData.Id)) {
        //         this._containerizationExtendedListService.IsDirectCharging = this._containerizationExtendedListService.IsDirectCharging.replace(this.rowData.Id + ",", "");
        //     }
        // }
        // if (AppTool.IsNullOrEmpty(this._containerizationExtendedListService.ConnectedDeclarations)) {
        //     this._containerizationExtendedListService.SelectedDeclarations = false;
        // } else {
        //     this._containerizationExtendedListService.SelectedDeclarations = true;
        // }
    }
}


