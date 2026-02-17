declare var window: any;
import { Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Validator } from '../../../../Infrastructure/Validators/Validator'; 

import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';




// Send Request
import { INF_MSG_GenericResponseData } from '../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';


import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './VehicleMoreDetailsTabComponent.html',
})

export class VehicleMoreDetailsTabComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: VehiclePM;
    public ObjectTableName: string = "Customs.Vehicle";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    public CurrentEditComponentId: string;

    //RequestParams: VehicleInsertUpdateDeleteMessageRequestParams;
    ResponseData: INF_MSG_GenericResponseData;
    //VehicleMessagesService: VehicleMessagesService = new VehicleMessagesService();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.Listen();
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
                        //this.DisplayOnlyCheck();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            //this.DisplayOnlyCheck();
                        }
                    }
                })
            );
        }
    }

    public SetTabArgs(args: any, valdationErrorList: any[]=null) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;

        console.log("EntityPM", this.EntityPM);


        this.SetFieldsEditability();
    }


    SetFieldsEditability() {
        //this.UIProperties.SetEnabled("VehicleTypeCode", this.ObjectTableName, this.IsNewEntity);
        //this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityPM.CountryCode));
    }



    //#endregion




    private checkForChassisNumberOp_Completed(exists: boolean) {

        if (exists) {
            this.UIProperties.SetValidity("InternalCode", "Customs.CustomBank", false, TextCodeTranslator.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist"));
            //this.InvalidChassisNumber = true;
        }
        else {
            //this.InvalidChassisNumber = false;
        }
    }


    /// #region Properties

    get IsABS() { return this.EntityPM != null ? this.EntityPM.IsABS : false; }
    set IsABS(value: boolean) { this.EntityPM.IsABS = value; }

    //public int ? AirBagsNumber        {
    get AirBagsNumber() { return this.EntityPM != null ? this.EntityPM.AirBagsNumber : null; }
    set AirBagsNumber(value: number) { this.EntityPM.AirBagsNumber = value; }

    //        public string ConverterTypeCode

    get ConverterTypeCode() { return this.EntityPM != null ? this.EntityPM.ConverterTypeCode : null; }
    set ConverterTypeCode(value: string) { this.EntityPM.ConverterTypeCode = value; }

    //public decimal ? GreenIndex

    get GreenIndex() { return this.EntityPM != null ? this.EntityPM.GreenIndex : null; }
    set GreenIndex(value: number) { this.EntityPM.GreenIndex = value; }

    //public int ? GreenIndexGroup

    get GreenIndexGroup() { return this.EntityPM != null ? this.EntityPM.GreenIndexGroup : null; }
    set GreenIndexGroup(value: number) { this.EntityPM.GreenIndexGroup = value; }

    //public bool IsStabilityControl

    get IsStabilityControl() { return this.EntityPM != null ? this.EntityPM.IsStabilityControl : false; }
    set IsStabilityControl(value: boolean) { this.EntityPM.IsStabilityControl = value; }

    //public string VehicleTecnologyTypeCode

    get VehicleTecnologyTypeCode() { return this.EntityPM != null ? this.EntityPM.VehicleTecnologyTypeCode : null; }
    set VehicleTecnologyTypeCode(value) { this.EntityPM.VehicleTecnologyTypeCode = value; }

    //public decimal ? VehicleSafetyAccessoryPoints

    get VehicleSafetyAccessoryPoints() { return this.EntityPM != null ? this.EntityPM.VehicleSafetyAccessoryPoints : null; }
    set VehicleSafetyAccessoryPoints(value: number) { this.EntityPM.VehicleSafetyAccessoryPoints = value; }

    //public string VehiclePriceListTypeCode

    get VehiclePriceListTypeCode() { return this.EntityPM != null ? this.EntityPM.VehiclePriceListTypeCode : null; }
    set VehiclePriceListTypeCode(value: string) { this.EntityPM.VehiclePriceListTypeCode = value; }

    //        public bool IsArmoredVehicle

    get IsArmoredVehicle() { return this.EntityPM != null ? this.EntityPM.IsArmoredVehicle : false; }
    set IsArmoredVehicle(value: boolean) { this.EntityPM.IsArmoredVehicle = value; }

    ///public bool IsLoweringVehicleForInvalid

    get IsLoweringVehicleForInvalid() { return this.EntityPM != null ? this.EntityPM.IsLoweringVehicleForInvalid : false; }
    set IsLoweringVehicleForInvalid(value: boolean) { this.EntityPM.IsLoweringVehicleForInvalid = value; }

    //public int ? SelfVehicleWeight

    get SelfVehicleWeight() { return this.EntityPM != null ? this.EntityPM.SelfVehicleWeight : null; }
    set SelfVehicleWeight(value: number) { this.EntityPM.SelfVehicleWeight = value; }

    //public int ? VehiclePowerKW

    get VehiclePowerKW() { return this.EntityPM != null ? this.EntityPM.VehiclePowerKW : null; }
    set VehiclePowerKW(value: number) {
        this.EntityPM.VehiclePowerKW = value;
        this.OnVehiclePowerKWLostFocus(value);}

    //        public string MedalNumber

    get MedalNumber() { return this.EntityPM != null ? this.EntityPM.MedalNumber : null; }
    set MedalNumber(value: string) { this.EntityPM.MedalNumber = value; }

    //        public DateTime ? DateOnRoadAbroad

    get DateOnRoadAbroad() { return this.EntityPM != null ? this.EntityPM.DateOnRoadAbroad : null; }
    set DateOnRoadAbroad(value: Date) { this.EntityPM.DateOnRoadAbroad = value; }


    //public DateTime ? IsraelEnterDate

    get IsraelEnterDate() { return this.EntityPM != null ? this.EntityPM.IsraelEnterDate : null; }
    set IsraelEnterDate(value: Date) { this.EntityPM.IsraelEnterDate = value; }


    //        public DateTime ? TransmissionDateWithoutTax

    get TransmissionDateWithoutTax() { return this.EntityPM != null ? this.EntityPM.TransmissionDateWithoutTax : null; }
    set TransmissionDateWithoutTax(value: Date) { this.EntityPM.TransmissionDateWithoutTax = value; }


    //    public int ? NumberOfSeats

    get NumberOfSeats() { return this.EntityPM != null ? this.EntityPM.NumberOfSeats : null; }
    set NumberOfSeats(value: number) { this.EntityPM.NumberOfSeats = value; }


    //    public bool IsThreeWheeledForReduction

    get IsThreeWheeledForReduction() { return this.EntityPM != null ? this.EntityPM.IsThreeWheeledForReduction : false; }
    set IsThreeWheeledForReduction(value: boolean) { this.EntityPM.IsThreeWheeledForReduction = value; }

    //    public bool IsCBS

    get IsCBS() { return this.EntityPM != null ? this.EntityPM.IsCBS : false; }
    set IsCBS(value: boolean) { this.EntityPM.IsCBS = value; }

    //    public bool IsSlipperClutch

    get IsSlipperClutch() { return this.EntityPM != null ? this.EntityPM.IsSlipperClutch : false; }
    set IsSlipperClutch(value: boolean) { this.EntityPM.IsSlipperClutch = value; }

    //    public bool IsSteeringDamper

    get IsSteeringDamper() { return this.EntityPM != null ? this.EntityPM.IsSteeringDamper : false; }
    set IsSteeringDamper(value: boolean) { this.EntityPM.IsSteeringDamper = value; }

    //    public bool IsTCS

    get IsTCS() { return this.EntityPM != null ? this.EntityPM.IsTCS : false; }
    set IsTCS(value: boolean) { this.EntityPM.IsTCS = value; }

    //    public bool IsTPS

    get IsTPS() { return this.EntityPM != null ? this.EntityPM.IsTPS : false; }
    set IsTPS(value: boolean) { this.EntityPM.IsTPS = value; }

    //    public string VehicleCategory

    get VehicleCategory() { return this.EntityPM != null ? this.EntityPM.VehicleCategory : null; }
    set VehicleCategory(value: string) { this.EntityPM.VehicleCategory = value; }

    //    public decimal ? VehicleMaxPowerKW

    get VehicleMaxPowerKW() { return this.EntityPM != null ? this.EntityPM.VehicleMaxPowerKW : null; }
    set VehicleMaxPowerKW(value: number) { this.EntityPM.VehicleMaxPowerKW = value; }

    //#endregion
    

    public SendButtonsVisibility: boolean = false;




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
    //#endregion

    valid: boolean = true;

    VehiclePowerKWKeyUp(event, VehiclePowerKWTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnVehiclePowerKWLostFocus(VehiclePowerKWTextBox);
        }
    }

    OnVehiclePowerKWLostFocus(VehiclePowerKWTextBox: any) {
        var newValue = this.VehiclePowerKW;
        this.valid = true;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
        }
        else {
            this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1) strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (AppTool.IsNullOrEmpty(strValue)) {
                this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
            }
            else if (strValue.length > 5) {
                this.valid = false;
                var str1 = strValue.substring(0, 5);
                var str2 = newValue.toString();
                var str3 = str2.replace(strValue, str1);
                newValue = +str3;
                this.VehiclePowerKW = newValue;
                this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", false, "הספק לא יכול להיות ארוך מחמישה תווים");

            }
            else {
                this.valid = true;
                this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
            }
        }
        if (this.valid != true) {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: "", LogTextBoxId: VehiclePowerKWTextBox.InputId });

        }
    }

    VehicleMaxPowerKWKeyUp(event, VehicleMaxPowerKWTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnVehicleMaxPowerKWLostFocus(VehicleMaxPowerKWTextBox);
        }
    }

    OnVehicleMaxPowerKWLostFocus(VehicleMaxPowerKWTextBox: any) {
        var newValue = this.VehicleMaxPowerKW;
        this.valid = true;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
        }
        else {
            this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1) strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (AppTool.IsNullOrEmpty(strValue)) {
                this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
            }
            else if (strValue.length > 5) {
                this.valid = false;
                var str1 = strValue.substring(0, 5);
                var str2 = newValue.toString();
                var str3 = str2.replace(strValue, str1);
                newValue = +str3;
                this.VehiclePowerKW = newValue;
                this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", false, "הספק לא יכול להיות ארוך מחמישה תווים");
            }
            else {
                this.valid = true;
                this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
            }
        }
        if (this.valid != true) {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: "", LogTextBoxId: VehicleMaxPowerKWTextBox.InputId });

        }
    }
}

