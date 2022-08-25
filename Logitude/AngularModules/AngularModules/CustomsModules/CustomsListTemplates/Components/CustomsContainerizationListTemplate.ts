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
import { ContainerizationDetails, ContainerizationRequestParams } from 'Customs/DataContract/RequestParams/ContainerizationRequestParams';


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
    countDeclarationUi:number; 
    
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
        if (!this._containerizationExtendedListService.IsDirectCharging) {
            this._containerizationExtendedListService.IsDirectCharging = "";
        }
        if (!this._containerizationExtendedListService.AllDeclarations) {
            this._containerizationExtendedListService.AllDeclarations = "";
        }
        if (!this._containerizationExtendedListService.containerizationRequestParams) {
            this._containerizationExtendedListService.containerizationRequestParams = new ContainerizationRequestParams();
            this._containerizationExtendedListService.containerizationRequestParams.ContainerizationList = [];

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
        
        this.countDeclarationUi = this._containerizationExtendedListService?.ConnectedDeclarations?.split(',').length-1;
        
    }
    OnNotChecked($event){
        if(!this.rowData.IsSubmitDeclaration)
        {
            $event.target.checked = false;
        }
    }
    OnConnectedCheckBoxChecked($event) {  
          
        if(!this.rowData.IsSubmitDeclaration)
        {
            this._containerizationExtendedListService.IsError=true;
        }
        else{
            this._containerizationExtendedListService.IsError=false;
       
           this._containerizationExtendedListService.disconnectedSelectAll = false;
           if ($event) {
               if (!this._containerizationExtendedListService.ConnectedDeclarations.includes(this.rowData.Id)) {
                   this._containerizationExtendedListService.ConnectedDeclarations = this._containerizationExtendedListService.ConnectedDeclarations + this.rowData.Id + ",";
                   this.AddUniqueConsignmentToRequestParams(this.rowData.CargoTypeCode, this.rowData.ManifestNumber, this.rowData.SecondCargoID, this.rowData.ThirdCargoID, this.rowData.Id)
               }
               if (this.rowData.ProcedureCurrentName != null && !this._containerizationExtendedListService.IsDirectCharging.includes(this.rowData.Id) &&
                   this.rowData.ProcedureCurrentName.includes("טעינה ישירה")) {
                   this._containerizationExtendedListService.IsDirectCharging = this._containerizationExtendedListService.IsDirectCharging + this.rowData.Id + ",";
               }
           }
           else {
               if (this._containerizationExtendedListService.ConnectedDeclarations.includes(this.rowData.Id)) {
                   this._containerizationExtendedListService.ConnectedDeclarations = this._containerizationExtendedListService.ConnectedDeclarations.replace(this.rowData.Id + ",", "");
                   this._containerizationExtendedListService.connectedSelectAll = false;
                   this.RemoveUniqueConsignmentToRequestParams(this.rowData.CargoTypeCode, this.rowData.ManifestNumber, this.rowData.SecondCargoID, this.rowData.ThirdCargoID, this.rowData.Id)
   
               }
               if (this._containerizationExtendedListService.IsDirectCharging.includes(this.rowData.Id)) {
                   this._containerizationExtendedListService.IsDirectCharging = this._containerizationExtendedListService.IsDirectCharging.replace(this.rowData.Id + ",", "");
               }
           }       
           if (AppTool.IsNullOrEmpty(this._containerizationExtendedListService.ConnectedDeclarations) || (this._containerizationExtendedListService.ConnectedDeclarations.split(',').length -1 - this.countDeclarationUi ==0)) {
           
               this._containerizationExtendedListService.SelectedDeclarations = false;
           } else {
               this._containerizationExtendedListService.SelectedDeclarations = true;
           }
        }
    }

    AddUniqueConsignmentToRequestParams(cargoTypeCode: string, manifestNumber: string, secondCargoId: string, thirdCargoId: string, decId: string) {
        let ConsignmentExist = false;
        this._containerizationExtendedListService.containerizationRequestParams.ContainerizationList.forEach(containerization => {
            if (containerization.CargoTypeCode == cargoTypeCode && containerization.ManifestNumber == manifestNumber && containerization.SecondCargoId == secondCargoId && containerization.ThirdCargoId == thirdCargoId) {
                ConsignmentExist = true;
                containerization.DeclarationList.push(decId);
            }
        });
        if (!ConsignmentExist) {
            let containerizationDetails = new ContainerizationDetails();
            containerizationDetails.CargoTypeCode = cargoTypeCode;
            containerizationDetails.ManifestNumber = manifestNumber;
            containerizationDetails.SecondCargoId = secondCargoId;
            containerizationDetails.ThirdCargoId = thirdCargoId;
            containerizationDetails.DeclarationList = [];
            containerizationDetails.DeclarationList.push(decId);
            this._containerizationExtendedListService.containerizationRequestParams.ContainerizationList.push(containerizationDetails);
        } 
    }
    RemoveUniqueConsignmentToRequestParams(cargoTypeCode: string, manifestNumber: string, secondCargoId: string, thirdCargoId: string, decId: string) {
        this._containerizationExtendedListService.containerizationRequestParams.ContainerizationList.forEach(containerization => {
            if (containerization.CargoTypeCode == cargoTypeCode && containerization.ManifestNumber == manifestNumber && containerization.SecondCargoId == secondCargoId && containerization.ThirdCargoId == thirdCargoId) {
                const DecIndex: number = containerization.DeclarationList.indexOf(decId);
                if (DecIndex !== -1) {
                    containerization.DeclarationList.splice(DecIndex, 1);
                }
                if(containerization.DeclarationList.length==0){
                    const ContIndex: number = this._containerizationExtendedListService.containerizationRequestParams.ContainerizationList.indexOf(containerization);
                    if (ContIndex !== -1) {
                        this._containerizationExtendedListService.containerizationRequestParams.ContainerizationList.splice(ContIndex, 1);
                    }
                }
            }
        });
    }
}


