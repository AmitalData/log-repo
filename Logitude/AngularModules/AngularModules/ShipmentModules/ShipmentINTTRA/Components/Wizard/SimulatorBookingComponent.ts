import { Component, ViewChildren, QueryList } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { INTRAWebService, INTRAResult } from '../../../../Shipment/Services/INTRAWebService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './SimulatorBookingComponent.html',
    providers: [EntityArgs]
})

export class SimulatorBookingComponent extends BaseComponent {
    public ValidationErrorsList: string[] = [];
    public EntityPM: ShipmentPM;
    private CurrentSession = SessionLocator.SelectedSession;
    private ShipmentId: string;
    private INTRAWebService: INTRAWebService;
    public DataContext = this;
    public ObjectTableName: string = "Shipment";
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public IsSendingBookingEnabled = false;
    public IsUpdatingBookingEnabled = false;
    public IsEditMode = false;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.Listen();
    }

    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.CheckSendingBookingEnabled();
                    this.CheckUpdatingBookingEnabled();
                }
                else {
                    this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.CheckSendingBookingEnabled();
                    this.CheckUpdatingBookingEnabled();
                }
            });
        }
    }

    SetWindowArgs(args) {
        this.EntityPM = args['Shipment'];
        this.IsEditMode = args['IsEditMode'];
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.EntityPM.IsForINTTRA = true;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        this.ShipmentId = this.EntityPM.Id;
        this.INTRAWebService = new INTRAWebService();
        this.INTRAWebService.ValidateBooking(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.CurrentSession.StopBusyIndicator();
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                var myResult: INTRAResult = myResponse.Result;

                if (myResult.Errors.length > 0) {
                    this.CurrentSession.StopBusyIndicator();
                    this.ValidationErrorsList = myResult.Errors;
                }
            }
        });

        this.CheckSendingBookingEnabled();
        this.CheckUpdatingBookingEnabled();
        this.CheckApplyChanges();
    }

    CheckSendingBookingEnabled() {
        this.IsSendingBookingEnabled = false;
        if ((this.EntityPM.INTTRABookingStatusCode == "NS" && this.EntityPM.INTTRABookingTransStatusCode == "NST") ||
            (this.EntityPM.INTTRABookingStatusCode == "ER" && this.EntityPM.INTTRABookingTransStatusCode == "BRS") ||
            (this.EntityPM.INTTRABookingStatusCode == "DC" && this.EntityPM.INTTRABookingTransStatusCode == "BRR") ||
            (this.EntityPM.INTTRABookingStatusCode == "RU" && this.EntityPM.INTTRABookingTransStatusCode == "BCD")) {
            this.IsSendingBookingEnabled = true;
        }
    }

    CheckUpdatingBookingEnabled() {
        this.IsUpdatingBookingEnabled = false;
        if (this.EntityPM.INTTRABookingStatusCode == "WC") {
            this.IsUpdatingBookingEnabled = true;
        }
    }

    // Cargo Information
    get MasterDepartureDate() { return this.EntityPM.MainCarriageATD != null ? this.EntityPM.MainCarriageATD : this.EntityPM.MainCarriageETD; }
    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }

    get Volume() { return AppTool.IsNullOrZero(this.EntityPM.BookingVolume) ? 0 : this.EntityPM.BookingVolume; }
    get GrossWeight() { return AppTool.IsNullOrZero(this.EntityPM.OrderGrossWeight) ? 0 : this.EntityPM.OrderGrossWeight; }
    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.OrderChargeableWeight) ? 0 : this.EntityPM.OrderChargeableWeight; }
    get NumberOfContainers() { return AppTool.IsNullOrZero(this.EntityPM.BookingNumberOfPackages) ? 0 : this.EntityPM.BookingNumberOfPackages; }

    get INTTRABookingStatusName() { return this.EntityPM.INTTRABookingStatusName; }
    set INTTRABookingStatusName(value: string) {
        if (this.EntityPM.INTTRABookingStatusName != value) {
            this.EntityPM.INTTRABookingStatusName = value;
        }
    }

    get INTTRABookingTransStatusName() { return this.EntityPM.INTTRABookingTransStatusName; }
    set INTTRABookingTransStatusName(value: string) {
        if (this.EntityPM.INTTRABookingTransStatusName != value) {
            this.EntityPM.INTTRABookingTransStatusName = value;
        }
    }

    // Tabs
    private CommunicationsPage: any = null;
    private selectedTabCode: string = "RA";
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }
    private SelectionChanged() {
        switch (this.SelectedTabCode) {
            case "RA": {

                break;
            }
            case "CM": {
                if (this.CommunicationsPage == null) {
                    let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
                    if (myLocation != null) {
                        this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(response => {
                            SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent", myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.CommunicationsPage = cmpRef.instance;
                                    this.CommunicationsPage.IsTitleHidden = true;
                                });
                        });
                    }
                }
                break;
            }
        }
    }


    // Requests

    get MainCarriageCarrierCode() { return this.EntityPM.MainCarriageCarrierCode; }
    get MainCarriageVesselName() { return this.EntityPM.MainCarriageVesselName; }
    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    get MainCarriageETD() { return this.EntityPM.MainCarriageETD; }
    get MainCarriageFromPortCode() { return this.EntityPM.MainCarriageFromPortCode; }
    get MainCarriageFromPortCountryName() { return this.EntityPM.MainCarriageFromPortCountryName; }
    get MainCarriageToPortCode() { return this.EntityPM.MainCarriageToPortCode; }
    get MainCarriageToPortCountryName() { return this.EntityPM.MainCarriageToPortCountryName; }
    get MainCarriageToPortCountryCode() { return this.EntityPM.MainCarriageToPortCountryCode; }
    get MainCarriageFromPortCountryCode() { return this.EntityPM.MainCarriageFromPortCountryCode; }
    get MainCarriageVesselVoyage() {

        var value = "";
        if (!AppTool.IsNullOrEmpty(this.MainCarriageVesselName)) {
            value = this.MainCarriageVesselName;
        }
        if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierNumber)) {
            if (!AppTool.IsNullOrEmpty(value)) {
                value = value + "/" + this.MainCarriageCarrierNumber;
            }
            else {
                value = this.MainCarriageCarrierNumber;
            }
        }
        return value;
    }

    get Transshipment1FromPortId() { return this.EntityPM.Transshipment1FromPortId; }
    get Transshipment1CarrierCode() { return this.EntityPM.Transshipment1CarrierCode; }
    get Transshipment1VesselName() { return this.EntityPM.Transshipment1VesselName; }
    get Transshipment1CarrierNumber() { return this.EntityPM.Transshipment1CarrierNumber; }
    get Transshipment1ETD() { return this.EntityPM.Transshipment1ETD; }
    get Transshipment1FromPortCode() { return this.EntityPM.Transshipment1FromPortCode; }
    get Transshipment1FromPortCountryName() { return this.EntityPM.Transshipment1FromPortCountryName; }
    get Transshipment1ToPortCode() { return this.EntityPM.Transshipment1ToPortCode; }
    get Transshipment1ToPortCountryName() { return this.EntityPM.Transshipment1ToPortCountryName; }
    get Transshipment1ToPortCountryCode() { return this.EntityPM.Transshipment1ToPortCountryCode; }
    get Transshipment1FromPortCountryCode() { return this.EntityPM.Transshipment1FromPortCountryCode; }
    get Transshipment1VesselVoyage() {
        var value = "";
        if (!AppTool.IsNullOrEmpty(this.Transshipment1VesselName)) {
            value = this.Transshipment1VesselName;
        }
        if (!AppTool.IsNullOrEmpty(this.Transshipment1CarrierNumber)) {
            if (!AppTool.IsNullOrEmpty(value)) {
                value = value + "/" + this.Transshipment1CarrierNumber;
            }
            else {
                value = this.Transshipment1CarrierNumber;
            }
        }
        return value;
    }


    get Transshipment2FromPortId() { return this.EntityPM.Transshipment2FromPortId; }
    get Transshipment2CarrierCode() { return this.EntityPM.Transshipment2CarrierCode; }
    get Transshipment2VesselName() { return this.EntityPM.Transshipment2VesselName; }
    get Transshipment2CarrierNumber() { return this.EntityPM.Transshipment2CarrierNumber; }
    get Transshipment2ETD() { return this.EntityPM.Transshipment2ETD; }
    get Transshipment2FromPortCode() { return this.EntityPM.Transshipment2FromPortCode; }
    get Transshipment2FromPortCountryName() { return this.EntityPM.Transshipment2FromPortCountryName; }
    get Transshipment2ToPortCode() { return this.EntityPM.Transshipment2ToPortCode; }
    get Transshipment2ToPortCountryName() { return this.EntityPM.Transshipment2ToPortCountryName; }
    get Transshipment2ToPortCountryCode() { return this.EntityPM.Transshipment2ToPortCountryCode; }
    get Transshipment2FromPortCountryCode() { return this.EntityPM.Transshipment2FromPortCountryCode; }
    get Transshipment2VesselVoyage() {
        var value = "";
        if (!AppTool.IsNullOrEmpty(this.Transshipment2VesselName)) {
            value = this.Transshipment2VesselName;
        }
        if (!AppTool.IsNullOrEmpty(this.Transshipment2CarrierNumber)) {
            if (!AppTool.IsNullOrEmpty(value)) {
                value = value + "/" + this.Transshipment2CarrierNumber;
            }
            else {
                value = this.Transshipment2CarrierNumber;
            }
        }
        return value;
    }

    get Transshipment3FromPortId() { return this.EntityPM.Transshipment3FromPortId; }
    get Transshipment3CarrierCode() { return this.EntityPM.Transshipment3CarrierCode; }
    get Transshipment3VesselName() { return this.EntityPM.Transshipment3VesselName; }
    get Transshipment3CarrierNumber() { return this.EntityPM.Transshipment3CarrierNumber; }
    get Transshipment3ETD() { return this.EntityPM.Transshipment3ETD; }
    get Transshipment3FromPortCode() { return this.EntityPM.Transshipment3FromPortCode; }
    get Transshipment3FromPortCountryName() { return this.EntityPM.Transshipment3FromPortCountryName; }
    get Transshipment3ToPortCode() { return this.EntityPM.Transshipment3ToPortCode; }
    get Transshipment3ToPortCountryCode() { return this.EntityPM.Transshipment3ToPortCountryCode; }
    get Transshipment3FromPortCountryCode() { return this.EntityPM.Transshipment3FromPortCountryCode; }
    get Transshipment3ToPortCountryName() { return this.EntityPM.Transshipment3ToPortCountryName; }
    get Transshipment3VesselVoyage() {
        var value = "";
        if (!AppTool.IsNullOrEmpty(this.Transshipment3VesselName)) {
            value = this.Transshipment3VesselName;
        }
        if (!AppTool.IsNullOrEmpty(this.Transshipment3CarrierNumber)) {
            if (!AppTool.IsNullOrEmpty(value)) {
                value = value + "/" + this.Transshipment3CarrierNumber;
            }
            else {
                value = this.Transshipment3CarrierNumber;
            }
        }
        return value;
    }

    get INTTRABookingResponse_Voyage() { return this.EntityPM.INTTRABookingResponse_Voyage; }
    get INTTRABookingResponse_POLDate() { return this.EntityPM.INTTRABookingResponse_POLDate; }
    get INTTRABookingResponse_POFPort() { return this.EntityPM.INTTRABookingResponse_POFPort; }
    get INTTRABookingResponse_POFPortCode() { return this.EntityPM.INTTRABookingResponse_POFPortCode; }
    get INTTRABookingResponse_POFCCode() { return this.EntityPM.INTTRABookingResponse_POFCCode; }
    get INTTRABookingResponse_POFCName() { return this.EntityPM.INTTRABookingResponse_POFCName; }
    get INTTRABookingResponse_PODDate() { return this.EntityPM.INTTRABookingResponse_PODDate; }
    get INTTRABookingResponse_PODPort() { return this.EntityPM.INTTRABookingResponse_PODPort; }
    get INTTRABookingResponse_PODPortCode() { return this.EntityPM.INTTRABookingResponse_PODPortCode; }
    get INTTRABookingResponse_PODCCode() { return this.EntityPM.INTTRABookingResponse_PODCCode; }
    get INTTRABookingResponse_PODCName() { return this.EntityPM.INTTRABookingResponse_PODCName; }
    get INTTRABookingResponse_ShippingLine() { return this.EntityPM.INTTRABookingResponse_ShippingLine; }

    UpdateButtonClicked() {
        var confirmMsg = "Are you sure you want to update the routing?";
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.INTTRABookingStatusCode = "CD";
                this.EntityPM.MainCarriageETD = this.INTTRABookingResponse_POLDate;
                this.EntityPM.MainCarriageETA = this.INTTRABookingResponse_PODDate;
                this.EntityPM.MainCarriageCarrierNumber = this.INTTRABookingResponse_Voyage;
                this.EntityPM.MainCarriageFromPortId = this.INTTRABookingResponse_POFPort;
                this.EntityPM.MainCarriageToPortId = this.INTTRABookingResponse_PODPort;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            if (confirmWindow.No) {
                this.EntityPM.INTTRABookingStatusCode = "RU";
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    }

    public IsApplyChanges = false;
    private CheckApplyChanges() {
        this.IsApplyChanges = false;
        if (this.EntityPM.INTTRABookingStatusCode != "SI" && this.INTTRABookingResponse_POFPortCode != null && this.INTTRABookingResponse_PODPortCode != null) {
            // Compare the Main leg
            if ((this.MainCarriageCarrierNumber != this.INTTRABookingResponse_Voyage) || (this.MainCarriageETD != this.INTTRABookingResponse_POLDate) ||
                (this.MainCarriageFromPortCode != this.INTTRABookingResponse_POFPortCode) || (this.MainCarriageToPortCode != this.INTTRABookingResponse_PODPortCode)) {
                this.IsApplyChanges = true;
            }

            // Compare the leg 2
        }
    }

    CloseButtonClicked() {
        this.Close();
    }
    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
    SendButtonClicked() {
        var errors: string[] = [];

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending e-Booking...");
            if (this.INTRAWebService == null) {
                this.INTRAWebService = new INTRAWebService();
            }
            this.INTRAWebService.SendEBooking(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myResult = myResponse.Result;

                    if (myResult.Errors != null && myResult.Errors.length > 0) {
                        this.ValidationErrorsList = myResult.Errors;

                    }

                    else {
                        //var messageWindow = new MessageWindow();
                        //messageWindow.Show("Simulated Successfully");
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            });
        }
    }

    RefreshAnswersClicked() {

    }

    EditShipmentClicked() {
        var entityId: string = this.EntityPM.Id;
        if (!AppTool.IsNullOrEmpty(entityId)) {
            var objectTableName = "Shipment";
            var editWindow = new LogitudeWindow();
            editWindow.Title = TextCodeTranslator.TranslateTable("General.B.Edit") + " " + TextCodeTranslator.TranslateTable(objectTableName);
            editWindow.ShowEditComponent(entityId, objectTableName);
            editWindow.IsEditComponent = true;
            editWindow.IsFillScreen = true;
            editWindow.ComponentLoaded.subscribe(s => {
                editWindow.WindowClosed.subscribe(d => {
                    this.EntityPM = s.EntityPM;
                });
            });      
        }
    }
}
