import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralList } from '../../../Customs/EntityLists/CustomsCollateralList';
import { CustomsCollateralAnswerSharedDataService } from '../../../Customs/Services/DataChange/CustomsCollateralAnswerSharedDataService'
import { Declaration } from 'typescript';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { AppTool } from '../../../Infrastructure/Tools';
import { ContainerizationExtendedListService } from '../../../Customs/Services/ExtendedLists/ContainerizationExtendedListService';
import { ContainerizationPM } from '../../../Customs/EntityPMs/ContainerizationPM';


@Component({   
    templateUrl: 'CustomsContainerizationListTemplate.html',
})

export class CustomsContainerizationListTemplate {
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
    entityPM: ContainerizationPM;
    constructor(private CD: ChangeDetectorRef, private _containerizationExtendedListService: ContainerizationExtendedListService) {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {
            this.entityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM as ContainerizationPM;
        } else {
            this.entityPM = new ContainerizationPM();
        }
     }

    setVariables(rowData: any, fieldName: string, additionalData: any) {
        this.fieldName = fieldName;
        this.rowData = rowData;
        this.BuildDeclarationsCheckBox();       
        this.CD.detectChanges();
    }

    BuildDeclarationsCheckBox() {
        this.IsConnectedDeclarationChecked = false;
        if (this.entityPM.ConnectedDeclarations && this.entityPM.Id != null && !this._containerizationExtendedListService.connectedSelectAll) {
            this._containerizationExtendedListService.ConnectedDeclarations = this.entityPM.ConnectedDeclarations;
        }
        if (!this._containerizationExtendedListService.ConnectedDeclarations) {
            this._containerizationExtendedListService.ConnectedDeclarations = "";
        }
        if (!this._containerizationExtendedListService.AllDeclarations) {
            this._containerizationExtendedListService.AllDeclarations = "";
        }
        if (!this._containerizationExtendedListService.AllDeclarations.includes(this.rowData.Id)) {
            this._containerizationExtendedListService.AllDeclarations = this._containerizationExtendedListService.AllDeclarations + this.rowData.Id + ",";
        }
        let sConnectedDeclarations = this._containerizationExtendedListService.ConnectedDeclarations as string;
        if (!AppTool.IsNullOrEmpty(sConnectedDeclarations)) {
            let ConnectedDeclarations = sConnectedDeclarations.split(',')
            let res = ConnectedDeclarations.filter(r => r == this.rowData.Id)[0];
            this.IsConnectedDeclarationChecked = !AppTool.IsNullOrEmpty(res);
        }
        if (this._containerizationExtendedListService.connectedSelectAll == true) {
            this.IsConnectedDeclarationChecked = true;
        }
    }

    OnConnectedCheckBoxChecked($event) {
        this._containerizationExtendedListService.disconnectedSelectAll = false;
        if ($event) {
            if (!this._containerizationExtendedListService.ConnectedDeclarations.includes(this.rowData.Id)) {
                this._containerizationExtendedListService.ConnectedDeclarations = this._containerizationExtendedListService.ConnectedDeclarations + this.rowData.Id + ",";
            }
        }
        else {
            if (this._containerizationExtendedListService.ConnectedDeclarations.includes(this.rowData.Id)) {
                this._containerizationExtendedListService.ConnectedDeclarations = this._containerizationExtendedListService.ConnectedDeclarations.replace(this.rowData.Id + ",", "");
                this._containerizationExtendedListService.connectedSelectAll = false;
            }
        }
        if (AppTool.IsNullOrEmpty(this._containerizationExtendedListService.ConnectedDeclarations)) {
            this._containerizationExtendedListService.SelectedDeclarations = false;
        } else {
            this._containerizationExtendedListService.SelectedDeclarations = true;
        }
    }
}


