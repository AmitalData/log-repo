import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {RoutingsTabComponent} from './RoutingsTabComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {CardList} from '../../../../Common/EntityLists/CardList'; 
import {PortList} from '../../../../Common/EntityLists/PortList';
import {VesselList} from '../../../../Common/EntityLists/VesselList';  
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {VesselListService} from '../../../../Common/Services/StandardLists/VesselListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    
    templateUrl: './AddEditPreCarriageComponent.html',
})

export class AddEditPreCarriageComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;   
    public ValidationErrorsList: string[] = [];
    public FatherComponent: RoutingsTabComponent;
    public IsConnectedHouse: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.InitServices();
    }

    private myPortListService: PortListService;
    private myCardListService: CardListService;
    private myVesselListService: VesselListService;
    InitServices() {
        this.myPortListService = new PortListService();
        this.myCardListService = new CardListService();
        this.myVesselListService = new VesselListService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.Clone();

        this.SetDefaultValues();
        this.SetUIProperties();
        this.SetDependencies();
    }

    SetDefaultValues() {
        if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            this.IsConnectedHouse = true;
            this.PreCarriageToPortId = this.EntityPM.MainCarriageFromPortId;
        }
    }

    public IsEditingEnabled: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.IsEditingEnabled = isEditingEnabled;

        if (this.EntityPM.ShipmentLevelCode == "H") {
            this.SetUIProperties_Forwarding();
        }

        else {
            this.SetUIProperties_Carriage();
        }        
    }
    SetUIProperties_Carriage() {
        var isTransportFieldEnabled = false;
        var isCarrierNumberFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId)) {
                isTransportFieldEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.PreCarriageCarrierId)) {
                isCarrierNumberFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("PreCarriageTransportModeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageFromPortId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageToPortId", this.ObjectTableName, isTransportFieldEnabled && this.IsConnectedHouse == false);
        this.UIProperties.SetEnabled("PreCarriageCarrierId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageCarrierNumber", this.ObjectTableName, isCarrierNumberFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageVesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageATA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetVisibility("PreCarriageVesselId", this.ObjectTableName, this.PreCarriageTransportModeId == "O" ? true : false);

        this.SetUIProperties_Carriage_RequiredFields();
        this.SetUIProperties_Carriage_ValidateActualDates();
    }
    SetUIProperties_Forwarding() {
        var isTransportFieldEnabled = false;
        var isCarrierNumberFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.PreForwardingTransportModeId)) {
                isTransportFieldEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.PreForwardingCarrierId)) {
                isCarrierNumberFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("PreForwardingTransportModeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreForwardingFromPortId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreForwardingToPortId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreForwardingCarrierId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreForwardingCarrierNumber", this.ObjectTableName, isCarrierNumberFieldEnabled);
        this.UIProperties.SetEnabled("PreForwardingVesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreForwardingETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreForwardingETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreForwardingATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PreForwardingATA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetVisibility("PreForwardingVesselId", this.ObjectTableName, this.PreForwardingTransportModeId == "O" ? true : false);

        this.SetUIProperties_Forwarding_RequiredFields();
        this.SetUIProperties_Forwarding_ValidateActualDates();
    }
    SetUIProperties_Carriage_RequiredFields() {
        this.UIProperties.SetRequired("PreCarriageTransportModeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId) ? true : false);
        this.UIProperties.SetRequired("PreCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("PreCarriageToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreCarriageToPortId) ? true : false);
    }
    SetUIProperties_Carriage_ValidateActualDates() {
        this.UIProperties.SetValidity("PreCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("PreCarriageATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.PreCarriageATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("PreCarriageATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.PreCarriageATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("PreCarriageATA", this.ObjectTableName, false, errorMessage);
        }
    }
    SetUIProperties_Forwarding_RequiredFields() {
        this.UIProperties.SetRequired("PreForwardingTransportModeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreForwardingTransportModeId) ? true : false);
        this.UIProperties.SetRequired("PreForwardingFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreForwardingFromPortId) ? true : false);
        this.UIProperties.SetRequired("PreForwardingToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PreForwardingToPortId) ? true : false);
    }
    SetUIProperties_Forwarding_ValidateActualDates() {
        this.UIProperties.SetValidity("PreForwardingATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("PreForwardingATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.PreForwardingATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("PreForwardingATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.PreForwardingATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("PreForwardingATA", this.ObjectTableName, false, errorMessage);
        }
    }

    public CarrierDependencyProperty1: string = null;
    SetDependencies() {
        var myResult: string = null;
        var transportModeId = null;

        if (this.EntityPM.ShipmentLevelCode == "H") {
            transportModeId = this.PreForwardingTransportModeId;
        }
        else {
            transportModeId = this.PreCarriageTransportModeId;
        }

        switch (transportModeId) {
            case "A": { myResult = "AL"; break; }
            case "O": { myResult = "SL"; break; }
            case "I": { myResult = "TR"; break; }
        }

        this.CarrierDependencyProperty1 = myResult;
    }

    get PreCarriageTransportModeId() { return this.EntityPM.PreCarriageTransportModeId; }
    set PreCarriageTransportModeId(value: string) {
        if (this.EntityPM.PreCarriageTransportModeId != value) {
            this.EntityPM.PreCarriageTransportModeId = value;
            this.PreCarriageFromPortId = null;

            if (!this.IsConnectedHouse) {
                this.PreCarriageToPortId = null;
            }

            this.PreCarriageCarrierId = null;
            this.PreCarriageCarrierNumber = null;
            this.PreCarriageVesselId = null;
            this.SetUIProperties_Carriage();
            this.SetDependencies();
        }
    }

    get PreCarriageFromPortId() { return this.EntityPM.PreCarriageFromPortId; }
    set PreCarriageFromPortId(value: string) {
        if (this.EntityPM.PreCarriageFromPortId != value) {
            this.EntityPM.PreCarriageFromPortId = value;
            this.SetUIProperties_Carriage_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.PreCarriageFromPortCode = null;
                this.EntityPM.PreCarriageFromPortName = null;
                this.EntityPM.PreCarriageFromPortCountryCode = null;
                this.EntityPM.PreCarriageFromPortCountryName = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreCarriageFromPortCode = list.Code;
                            this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                            this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                            this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    if (list) {
                                        this.EntityPM.PreCarriageFromPortCode = list.Code;
                                        this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                                        this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                                        this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get PreCarriageToPortId() { return this.EntityPM.PreCarriageToPortId; }
    set PreCarriageToPortId(value: string) {
        if (this.EntityPM.PreCarriageToPortId != value) {
            this.EntityPM.PreCarriageToPortId = value;
            this.SetUIProperties_Carriage_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.PreCarriageToPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.PreCarriageToPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    RoutingHelper.PreCarriageToPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get PreCarriageCarrierId() { return this.EntityPM.PreCarriageCarrierId; }
    set PreCarriageCarrierId(value: string) {
        if (this.EntityPM.PreCarriageCarrierId != value) {
            this.EntityPM.PreCarriageCarrierId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {                
                this.EntityPM.PreCarriageCarrierCode = null;
                this.EntityPM.PreCarriageCarrierName = null;
                this.EntityPM.PreCarriageCarrierWebSite = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreCarriageCarrierCode = list.Code;
                            this.EntityPM.PreCarriageCarrierName = list.EnglishName;
                            this.EntityPM.PreCarriageCarrierWebSite = list.WebSite;
                        }
                    }
                });
            }
        }
    }

    get PreCarriageCarrierNumber() { return this.EntityPM.PreCarriageCarrierNumber; }
    set PreCarriageCarrierNumber(value: string) {
        if (this.EntityPM.PreCarriageCarrierNumber != value) {
            this.EntityPM.PreCarriageCarrierNumber = value;
        }
    }

    get PreCarriageVesselId() { return this.EntityPM.PreCarriageVesselId; }
    set PreCarriageVesselId(value: string) {
        if (this.EntityPM.PreCarriageVesselId != value) {
            this.EntityPM.PreCarriageVesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.PreCarriageVesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreCarriageVesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get PreCarriageETD() { return this.EntityPM.PreCarriageETD; }
    set PreCarriageETD(value: Date) {
        if (this.EntityPM.PreCarriageETD != value) {
            this.EntityPM.PreCarriageETD = value;
        }
    }

    get PreCarriageETA() { return this.EntityPM.PreCarriageETA; }
    set PreCarriageETA(value: Date) {
        if (this.EntityPM.PreCarriageETA != value) {
            this.EntityPM.PreCarriageETA = value;
        }
    }

    get PreCarriageATD() { return this.EntityPM.PreCarriageATD; }
    set PreCarriageATD(value: Date) {
        if (this.EntityPM.PreCarriageATD != value) {
            this.EntityPM.PreCarriageATD = value;
            this.SetUIProperties_Carriage_ValidateActualDates();
        }
    }

    get PreCarriageATA() { return this.EntityPM.PreCarriageATA; }
    set PreCarriageATA(value: Date) {
        if (this.EntityPM.PreCarriageATA != value) {
            this.EntityPM.PreCarriageATA = value;
            this.SetUIProperties_Carriage_ValidateActualDates();
        }
    }

    get PreForwardingTransportModeId() { return this.EntityPM.PreForwardingTransportModeId; }
    set PreForwardingTransportModeId(value: string) {
        if (this.EntityPM.PreForwardingTransportModeId != value) {
            this.EntityPM.PreForwardingTransportModeId = value;
            this.PreForwardingFromPortId = null;
            this.PreForwardingToPortId = null;
            this.PreForwardingCarrierId = null;
            this.PreForwardingCarrierNumber = null;
            this.PreForwardingVesselId = null;
            this.SetUIProperties_Forwarding();
            this.SetDependencies();
        }
    }

    get PreForwardingFromPortId() { return this.EntityPM.PreForwardingFromPortId; }
    set PreForwardingFromPortId(value: string) {
        if (this.EntityPM.PreForwardingFromPortId != value) {
            this.EntityPM.PreForwardingFromPortId = value;
            this.SetUIProperties_Forwarding_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.PreForwardingFromPortCode = null;
                this.EntityPM.PreForwardingFromPortName = null;
                this.EntityPM.PreForwardingFromPortCountryCode = null;
                this.EntityPM.PreForwardingFromPortCountryName = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreForwardingFromPortCode = list.Code;
                            this.EntityPM.PreForwardingFromPortName = list.EnglishName;
                            this.EntityPM.PreForwardingFromPortCountryCode = list.CountryCode;
                            this.EntityPM.PreForwardingFromPortCountryName = list.CountryName;
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    if (list) {
                                        this.EntityPM.PreForwardingFromPortCode = list.Code;
                                        this.EntityPM.PreForwardingFromPortName = list.EnglishName;
                                        this.EntityPM.PreForwardingFromPortCountryCode = list.CountryCode;
                                        this.EntityPM.PreForwardingFromPortCountryName = list.CountryName;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get PreForwardingToPortId() { return this.EntityPM.PreForwardingToPortId; }
    set PreForwardingToPortId(value: string) {
        if (this.EntityPM.PreForwardingToPortId != value) {
            this.EntityPM.PreForwardingToPortId = value;
            this.SetUIProperties_Forwarding_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.PreForwardingToPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.PreForwardingToPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    RoutingHelper.PreForwardingToPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get PreForwardingCarrierId() { return this.EntityPM.PreForwardingCarrierId; }
    set PreForwardingCarrierId(value: string) {
        if (this.EntityPM.PreForwardingCarrierId != value) {
            this.EntityPM.PreForwardingCarrierId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.PreForwardingCarrierCode = null;
                this.EntityPM.PreForwardingCarrierName = null;
                this.EntityPM.PreForwardingCarrierWebSite = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreForwardingCarrierCode = list.Code;
                            this.EntityPM.PreForwardingCarrierName = list.EnglishName;
                            this.EntityPM.PreForwardingCarrierWebSite = list.WebSite;
                        }
                    }
                });
            }
        }
    }

    get PreForwardingCarrierNumber() { return this.EntityPM.PreForwardingCarrierNumber; }
    set PreForwardingCarrierNumber(value: string) {
        if (this.EntityPM.PreForwardingCarrierNumber != value) {
            this.EntityPM.PreForwardingCarrierNumber = value;
        }
    }

    get PreForwardingVesselId() { return this.EntityPM.PreForwardingVesselId; }
    set PreForwardingVesselId(value: string) {
        if (this.EntityPM.PreForwardingVesselId != value) {
            this.EntityPM.PreForwardingVesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.PreForwardingVesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.PreForwardingVesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get PreForwardingETD() { return this.EntityPM.PreForwardingETD; }
    set PreForwardingETD(value: Date) {
        if (this.EntityPM.PreForwardingETD != value) {
            this.EntityPM.PreForwardingETD = value;
        }
    }

    get PreForwardingETA() { return this.EntityPM.PreForwardingETA; }
    set PreForwardingETA(value: Date) {
        if (this.EntityPM.PreForwardingETA != value) {
            this.EntityPM.PreForwardingETA = value;
        }
    }

    get PreForwardingATD() { return this.EntityPM.PreForwardingATD; }
    set PreForwardingATD(value: Date) {
        if (this.EntityPM.PreForwardingATD != value) {
            this.EntityPM.PreForwardingATD = value;
            this.SetUIProperties_Forwarding_ValidateActualDates();
        }
    }

    get PreForwardingATA() { return this.EntityPM.PreForwardingATA; }
    set PreForwardingATA(value: Date) {
        if (this.EntityPM.PreForwardingATA != value) {
            this.EntityPM.PreForwardingATA = value;
            this.SetUIProperties_Forwarding_ValidateActualDates();
        }
    }

    SetActualDateClicked(fieldName: string) {
        if (this.EntityPM.ShipmentLevelCode == "H") {
            switch (fieldName) {
                case "PreForwardingETD": { this.PreForwardingATD = DateTool.GetDateParts(this.PreForwardingETD).DateObject; break; }
                case "PreForwardingETA": { this.PreForwardingATA = DateTool.GetDateParts(this.PreForwardingETA).DateObject; break; }
            }
        }

        else {
            switch (fieldName) {
                case "PreCarriageETD": { this.PreCarriageATD = DateTool.GetDateParts(this.PreCarriageETD).DateObject; break; }
                case "PreCarriageETA": { this.PreCarriageATA = DateTool.GetDateParts(this.PreCarriageETA).DateObject; break; }
            }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = []; 
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.EntityPM.ShipmentLevelCode == "H") {
            errors = this.ValidatePreForwarding(msg);
        }

        else {
            errors = this.ValidatePreCarriage(msg);
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
    ValidatePreCarriage(msg: string): string[] {
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.TransportMode")));
        }

        if (AppTool.IsNullOrEmpty(this.PreCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.From")));
        }

        if (AppTool.IsNullOrEmpty(this.PreCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.To")));
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "PreCarriage");

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.PreCarriageATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD")));
        }
        if (!DateTool.IsActualDateValid(this.PreCarriageATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA")));
        }

        return errors;
    }
    ValidatePreForwarding(msg: string): string[] {
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.PreForwardingTransportModeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.TransportMode")));
        }

        if (AppTool.IsNullOrEmpty(this.PreForwardingFromPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.From")));
        }

        if (AppTool.IsNullOrEmpty(this.PreForwardingToPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.To")));
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "PreForwarding");

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.PreForwardingATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD")));
        }
        if (!DateTool.IsActualDateValid(this.PreForwardingATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA")));
        }

        return errors;
    }

    private myCloner: Cloner;
    private entityCloner: Cloner;
    private oldFollowups: ShipmentFollowUpPM[] = [];
    private Clone() {

        this.EntityPM.FollowUps.forEach(item => {
            var oldItem: ShipmentFollowUpPM = new ShipmentFollowUpPM(null);
            oldItem.Id = item.Id;
            oldItem.Date = item.Date;
            oldItem.Deleted = item.Deleted;
            oldItem.Done = item.Done;
            oldItem.DoneDateTime = item.DoneDateTime;
            oldItem.DoneNote = item.DoneNote;
            oldItem.EntityDateId = item.EntityDateId;
            oldItem.EventTypeFollowUpName = item.EventTypeFollowUpName;
            oldItem.EventTypeId = item.EventTypeId;
            oldItem.Tenant = item.Tenant;
            oldItem.ExternalDocumentId = item.ExternalDocumentId;
            oldItem.IsNew = item.IsNew;
            oldItem.JobId = item.JobId;
            oldItem.LegType = item.LegType;
            oldItem.ManualActivatedFollowUp = item.ManualActivatedFollowUp;
            oldItem.Note = item.Note;
            oldItem.OwnerUserId = item.OwnerUserId;
            oldItem.OwnerUserName = item.OwnerUserName;
            oldItem.ShipmentId = item.ShipmentId;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;
            this.oldFollowups.push(oldItem);
        });

        this.myCloner = new Cloner(this);
        this.myCloner.AddField('PreCarriageTransportModeId');
        this.myCloner.AddField('PreCarriageFromPortId');
        this.myCloner.AddField('PreCarriageToPortId');
        this.myCloner.AddField('PreCarriageCarrierId');
        this.myCloner.AddField('PreCarriageCarrierNumber');
        this.myCloner.AddField('PreCarriageVesselId');
        this.myCloner.AddField('PreCarriageETD');
        this.myCloner.AddField('PreCarriageETA');
        this.myCloner.AddField('PreCarriageATD');
        this.myCloner.AddField('PreCarriageATA');
        this.myCloner.AddField('PreForwardingTransportModeId');
        this.myCloner.AddField('PreForwardingFromPortId');
        this.myCloner.AddField('PreForwardingToPortId');
        this.myCloner.AddField('PreForwardingCarrierId');
        this.myCloner.AddField('PreForwardingCarrierNumber');
        this.myCloner.AddField('PreForwardingVesselId');
        this.myCloner.AddField('PreForwardingETD');
        this.myCloner.AddField('PreForwardingETA');
        this.myCloner.AddField('PreForwardingATD');
        this.myCloner.AddField('PreForwardingATA');
        this.myCloner.AddEntity(this.EntityPM);

        this.entityCloner = new Cloner(this.EntityPM);
        this.entityCloner.AddField('FromCountryId');
        this.entityCloner.AddField('FromCountryIsEC');
        this.entityCloner.AddField('MainCarriageFromPortId');
        this.entityCloner.AddField('MainCarriageFromPortCode');
        this.entityCloner.AddField('MainCarriageFromPortName');
        this.entityCloner.AddField('MainCarriageFromPortCountryCode');
        this.entityCloner.AddField('MainCarriageFromPortCountryName');
        this.entityCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldFollowups.forEach(item => {
            var existingItem = this.EntityPM.FollowUps.filter(f => f.LegType == item.LegType)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.FollowUps.forEach(item => {
            var oldItem = this.oldFollowups.filter(f => f.LegType == item.LegType)[0];
            if (oldItem == null) {
                addedItems.push(item);
            }
        });

        if (addedItems.length > 0 || removedItems.length > 0) {
            addedItems.forEach(item => {
                this.EntityPM.RemoveShipmentFollowUp(item);
            });

            removedItems.forEach(item => {
                this.EntityPM.AddShipmentFollowUp(item);
            });

            this.CurrentSession.FireEvent("FollowupsChanged");
        }

        this.myCloner.RejectChanges();
        this.entityCloner.RejectChanges();
    }
}
