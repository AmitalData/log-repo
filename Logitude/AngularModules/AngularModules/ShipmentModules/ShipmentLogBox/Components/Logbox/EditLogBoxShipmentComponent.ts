import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'

import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogBoxDocumentsComponent} from './LogBoxDocumentsComponent';
import {ShipmentDomainService, ImporterQueriesDataCounts} from '../../../../Shipment/Services/ShipmentDomainService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityStatusListService} from '../../../../Infrastructure/Services/StandardLists/EntityStatusListService';
import {BranchListService} from '../../../../Common/Services/StandardLists/BranchListService';
import {DepartmentListService} from '../../../../Common/Services/StandardLists/DepartmentListService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {PortExtendedPMService} from '../../../../Common/Services/ExtendedPMs/PortExtendedPMService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    
    templateUrl: './EditLogBoxShipmentComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class EditLogBoxShipmentComponent extends BaseComponent implements OnInit, AfterViewInit {
    private myShipmentDomainService: ShipmentDomainService;
    EntityPm: ShipmentPM = new ShipmentPM();
    DataContext: EditLogBoxShipmentComponent = this;
    ValidationErrorsList: any[]; 
    public _ShipmentPMService: ShipmentPMService;
    IsPrivate: boolean = false;
    public PLShortName: string = "";
    public IsShownCustomerReference3: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityListService: EntityListService) {
        super();
        if (SessionLocator.PrivateLableSettings) {
            this.IsPrivate = true;
            this.PLShortName = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
        } 
        this.ValidationErrorsList = []; 
        this._ShipmentPMService = new ShipmentPMService();
        
    }

    ngOnInit() {

    }
    ngAfterViewInit() {

    }
    SetWindowArgs(args: any) {
    
        if (args.EntityPm) {
            this.EntityPm = args.EntityPm;
        }

        //if (this.EntityPm && this.EntityPM.DirectionId != 'C') {
        //    this.IsShownCustomerReference3 = !AppTool.IsNullOrEmpty(this.EntityPm.CustomerReference3);
        //}
    }

    CustomerReferenceChanged: boolean = false;

    public get DirectionId() { return this.EntityPm.DirectionId }
    public set DirectionId(newValue: string) {
        if (this.EntityPm.DirectionId != newValue) {
            this.EntityPm.DirectionId = newValue;
        }
    }

    public get CustomerReference2() { return this.EntityPm.CustomerReference2 }
    public set CustomerReference2(newValue: string) {
        if (this.EntityPm.CustomerReference2 != newValue) {
            this.EntityPm.CustomerReference2 = newValue;
            this.CustomerReferenceChanged = true;
        }
        else {
            this.CustomerReferenceChanged = false;
        }
    }

    public get CustomerReference3() { return this.EntityPm.CustomerReference3 }
    public set CustomerReference3(newValue: string) {
        if (this.EntityPm.CustomerReference3 != newValue) {
            this.EntityPm.CustomerReference3 = newValue;
            this.CustomerReferenceChanged = true;
        }
        else {
            this.CustomerReferenceChanged = false;
        }
    }

    public get SendUpdatesToAgentEnabled() { return this.EntityPm.SendUpdatesToAgentEnabled }
    public set SendUpdatesToAgentEnabled(newValue: boolean) {
        
        if (this.EntityPm.SendUpdatesToAgentEnabled != newValue) {
            this.EntityPm.SendUpdatesToAgentEnabled = newValue;
            this.CustomerReferenceChanged = true;
        }
        else {
            this.CustomerReferenceChanged = false;
        }
    }
    
     
    SaveChanges() {
        this.ValidationErrorsList = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        //var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //if (AppTool.IsNullOrEmpty(this.CustomerReference2)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "My Reference"));
        //}
        if (this.CustomerReferenceChanged == true) {
            this.SaveData();
        }
        else { 
            this.CurrentSession.CloseCurrentWindow();
        }

    }

    SaveData() { 
       
        
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.EntityPm.IsImporterShipment = true;
            this.EntityPm.MainCarriageFromPortId = this.EntityPm.FromPortId;
            this.EntityPm.MainCarriageToPortId = this.EntityPm.ToPortId;
            this.EntityPm.ShipperReference2 = this.EntityPm.CustomerReference2;
            this.EntityPm.ConsigneeReference2 = this.EntityPm.CustomerReference2;
            if (this.IsShownCustomerReference3) {
                this.SetCustomerReference3Fields();
            }
            this.EntityPm.UpdateSendUpdatesToAgentEnabledField = true; 
            this.EntityPm.GrossWeightUnitCode = "KG";
            this.EntityPm.DimensionsUnitCode = "Cm";
            this.EntityPm.ChargeableWeightUnitCode = "KG";
            this.EntityPm.VolumeUnitCode = "CBF";
                 
                this._ShipmentPMService.update(this.EntityPm).subscribe((myResult:any) => {
                    if (!myResult.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        //this.CurrentSession.CloseCurrentWindow();
                        //this.CurrentSession.CloseCurrentWindowEmit("CustomReloadShipments");
                        this.CurrentSession.SessionEvent.emit({ Name: "CustomReloadShipments" });
                        this.CurrentSession.CurrentWindow.Close("");
                    }
                    else {
                        //this.ValidationErrorsList = myResult.ErrorsArray;
                        this.ValidationErrorsList = myResult.ErrorsArray;//.push("There Are Validation Errors.");
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                });
          
             

        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    }

    

    private SetCustomerReference3Fields() {
        this.EntityPm.ShipperReference3 = this.EntityPm.CustomerReference3;
        this.EntityPm.ConsigneeReference3 = this.EntityPm.CustomerReference3;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    } 

}
