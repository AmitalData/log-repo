declare var window: any;
import { Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { EntityArgs } from  '../../../../Infrastructure/DataContracts/EntityArgs';
import { LogTab } from      '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { VehicleExtendedPMService } from '../../../../Customs/Services/ExtendedPMs/VehicleExtendedPMService';
import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';

// Send Request
import { INF_MSG_GenericResponseData } from '../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';

import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './VehicleGeneralComponent.html',
})

export class VehicleGeneralComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: VehiclePM;
    public ObjectTableName: string = "Customs.Vehicle";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    public CurrentEditComponentId: string;
    
    ResponseData: INF_MSG_GenericResponseData;
    _VehicleExtendedPMService: VehicleExtendedPMService = new VehicleExtendedPMService();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        
        this.EntityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.Listen();
                this.SetFieldsEditability();
            });
        });

    }

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.RefreshEntity();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            this.RefreshEntity();
                        }
                    }
                })
            );
        }
    }

    //public SetTabArgs(args: any, ValidationErrorsList: any[]) {
    public SetTabArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;

        console.log("EntityPM", this.EntityPM);


        this.SetFieldsEditability();
    }

    RefreshEntity() {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
        this.SetFieldsEditability();
    }

    SetFieldsEditability() {
        
        this.SetImporterIdentityIdFieldsEditability();

        this.SetImporterPassportNumberFieldsEditability();

    }

    SetImporterIdentityIdFieldsEditability() {
        
        if (!AppTool.IsNullOrEmpty(this.ImporterIdentityId)){

            this.ImporterPassportNumber = null;
            this.ImporterPassCountryCode = null;
            this.ImporterPassportTypeCode = null;
            this.PassportName = null;
            this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ImporterPassportTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PassportName", this.ObjectTableName, false);
            
            this.UIProperties.SetEnabled("ImporterIdentityId", this.ObjectTableName, true);
            
        }
        else{
            this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassportTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PassportName", this.ObjectTableName, true);
        }
    }

    SetImporterPassportNumberFieldsEditability() {
        if (!AppTool.IsNullOrEmpty(this.ImporterPassportNumber)){
            this.EntityPM.ImporterIdentityId = null;
            this.ImporterIdentityId = null;
            this.UIProperties.SetEnabled("ImporterIdentityId", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassportTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PassportName", this.ObjectTableName, true);
            
        }
        else{
            this.UIProperties.SetEnabled("ImporterIdentityId", this.ObjectTableName, true);
        }

    }


                                    
    //#endregion




    private checkForChassisNumberOp_Completed(exists: boolean) {

        if (exists) {
            this.UIProperties.SetValidity("InternalCode", "Customs.CustomBank",  false, TextCodeTranslator.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist"));
            this.InvalidChassisNumber = true;
        }
        else {
            this.InvalidChassisNumber = false;
        }
    }

    ///#region Properties



    public  SendButtonsVisibility: boolean = false;
    

    public get VehicleChassisNumber(): string { return this.EntityPM != null ?this.EntityPM.VehicleChassisNumber : null; }
    public set VehicleChassisNumber(value: string) {
        if (this.EntityPM.VehicleChassisNumber != value) {
            this.EntityPM.VehicleChassisNumber = value;

        }
    }
     

    get InvalidChassisNumber(){ return this.EntityPM != null ?this.EntityPM.InvalidChassisNumber : false; }
    set InvalidChassisNumber(value) { this.EntityPM.InvalidChassisNumber = value; }      

    get VehiclePoolTypeCode() { return this.EntityPM != null ?this.EntityPM.VehiclePoolTypeCode : null; }
    set VehiclePoolTypeCode(value: string){this.EntityPM.VehiclePoolTypeCode = value; }
        
    get ModelCode(){ return this.EntityPM != null ?this.EntityPM.ModelCode : null; }
    set ModelCode(value:string){this.EntityPM.ModelCode = value;  }
        
    get ModelDescription() :string { return this.EntityPM != null ?this.EntityPM.ModelDescription : null; }
    set ModelDescription(value:string){this.EntityPM.ModelDescription = value; }
        
    get NumberOfWheels() { return this.EntityPM != null ? this.EntityPM.NumberOfWheels : null; }
    set NumberOfWheels(value: number) { this.EntityPM.NumberOfWheels = value;  }
        
    get RichbitFileNumber (){ return this.EntityPM != null ?this.EntityPM.RichbitFileNumber : null; }
    set RichbitFileNumber(value: string){this.EntityPM.RichbitFileNumber = value;  }

    get EngineCapacity(){ return this.EntityPM != null ?this.EntityPM.EngineCapacity : null; }
    set EngineCapacity(value:number){this.EntityPM.EngineCapacity = value;  }
    
    get CommercialNickname(){ return this.EntityPM != null ?this.EntityPM.CommercialNickname : null; }
    set CommercialNickname(value: string){this.EntityPM.CommercialNickname = value;  }

    get VehicleManufacturerCode(){ return this.EntityPM != null ?this.EntityPM.VehicleManufacturerCode : null; }
    set VehicleManufacturerCode(value: string){this.EntityPM.VehicleManufacturerCode = value; }

    get VehicleManufactureDate() { return this.EntityPM != null ? this.EntityPM.VehicleManufactureDate : null; }
    set VehicleManufactureDate(value: Date) { this.EntityPM.VehicleManufactureDate = value; }
    
    get FuelTypeCode(){ return this.EntityPM != null ?this.EntityPM.FuelTypeCode : null; }
    set FuelTypeCode(value: string){this.EntityPM.FuelTypeCode = value; }
 
    get ManufactureCountryCode(){ return this.EntityPM != null ?this.EntityPM.ManufactureCountryCode : null; }
    set ManufactureCountryCode(value: string){this.EntityPM.ManufactureCountryCode = value; }
     
    get TotalVehicleWeight(){ return this.EntityPM != null ? this.EntityPM.TotalVehicleWeight : null; }
    set TotalVehicleWeight(value: number){this.EntityPM.TotalVehicleWeight = value; }

    get VehicleWindowNumber(){ return this.EntityPM != null ?this.EntityPM.VehicleWindowNumber : null; }
    set VehicleWindowNumber(value: string){this.EntityPM.VehicleWindowNumber = value; }

    get VehicleTypeCode(){ return this.EntityPM != null ?this.EntityPM.VehicleTypeCode : null; }
    set VehicleTypeCode(value: string){this.EntityPM.VehicleTypeCode = value;  }

    get ImporterPassportNumber() { return this.EntityPM != null ? this.EntityPM.ImporterPassportNumber : null; }
    set ImporterPassportNumber(value: string)
    {
        this.EntityPM.ImporterPassportNumber = value;
        this.SetImporterPassportNumberFieldsEditability();
    }

    get ImporterPassCountryCode() { return this.EntityPM != null ? this.EntityPM.ImporterPassCountryCode : null; }
    set ImporterPassCountryCode(value: string) { this.EntityPM.ImporterPassCountryCode = value; }

    get ImporterPassportTypeCode() { return this.EntityPM != null ? this.EntityPM.ImporterPassportTypeCode : null; }
    set ImporterPassportTypeCode(value: string) { this.EntityPM.ImporterPassportTypeCode = value; }
        
    get ImporterIdentityId()
    {
        if (this.EntityPM == null) return null;
        return this.EntityPM.ImporterIdentityId;
    }
    set ImporterIdentityId(value:string)
    {
        this.EntityPM.ImporterIdentityId = value;
        this.SetImporterIdentityIdFieldsEditability(); 
    }

    get PassportName() { return this.EntityPM != null ? this.EntityPM.PassportName : null; }
    set PassportName(value: string) { this.EntityPM.PassportName = value; }


//#endregion

    line = 0;
   
    //#region Send + Delete
    SendButtonClicked() {
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs

        // validate Vehicle
        Validator.TryValidateObject(this.EntityPM, "Customs.Vehicle", errors);

        //if (this.EntityPM.VehicleCommunications.length == 0) {
        //    errors.push(TextCodeTranslator.Translate("Customs.Vehicle.O.RequierdCommunication"));
        //} else {
        //    // validate Vehicle communication items
        //    this.EntityPM.VehicleCommunications.forEach((item) => {
        //        Validator.TryValidateObject(item, "Customs.VehicleCommunication", errors);
        //    });
        //}

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            this.FillValidationErrorList.emit(errors);
        } else {
            // send request
            //this.SendRequest(false);
        }

    }

    
    OnSendCompleted() {
        if (this.IsDelete) {
            this.ApplyDeleteVehicle();
        }
    }

    ApplyDeleteVehicle() {
        this.IsDelete = false;

        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vehicle", DateTime.UtcNow, true);
        //this.Dispose();
    }

    OnVehicleChassisNumberLostFocus(vehicleChassisNumberTextBox: any) {

        if (!AppTool.IsNullOrEmpty(this.VehicleChassisNumber)) {
            this._VehicleExtendedPMService.CheckIfVehicleExistByChassisNumber(this.VehicleChassisNumber).subscribe((response:any) => {
                if (response != null) {
                    if (!AppTool.IsNullOrEmpty(response.Result)) {
                        if (this.EntityPM.Id != response.Result) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                            messageWindow.Width = 250;
                            messageWindow.Height = 150;
                            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                            messageWindow.WindowClosed.subscribe(($event: any) => this.OnCheckIfVehicleExistWindowClosed($event, vehicleChassisNumberTextBox));
                            messageWindow.Show("קיים כבר רכב עם אותו מספר שלדה");
                            return;
                        }
                    }
                }
            });
        }
    }


    OnCheckIfVehicleExistWindowClosed(arg: any, vehicleChassisNumberTextBox : any) {
        if (!AppTool.IsNullOrEmpty(vehicleChassisNumberTextBox)) {
            SessionLocator.SustainFocusOnCell = true;
            console.log(vehicleChassisNumberTextBox.InputId);
            var element = document.getElementById(vehicleChassisNumberTextBox.InputId);
            if (element) {
                element.focus();
            }
        }
    }
    //#endregion
}

