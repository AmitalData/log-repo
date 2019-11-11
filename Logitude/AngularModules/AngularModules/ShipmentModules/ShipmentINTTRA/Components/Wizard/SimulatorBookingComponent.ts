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


    constructor(public entityArgs: EntityArgs) {
        super();
    }

    SetWindowArgs(args) {
        this.EntityPM = args['Shipment'];
        this.entityArgs.EntityPM = this.EntityPM;
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
    }

    // Cargo Information
    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }

    get Volume() { return AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; }
    get GrossWeight() { return AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; }
    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; }
    get NumberOfContainers() { return AppTool.IsNullOrZero(this.EntityPM.NumberOfContainers) ? 0 : this.EntityPM.NumberOfContainers; }

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
                                    this.CommunicationsPage.IsForINTTRA = true;
                                });
                        });
                    }
                }
                break;
            }
        }
    }


    // Requests
    get MainCarriageCarrierName() { return this.EntityPM.MainCarriageCarrierName; }
    get MainCarriageVesselName() { return this.EntityPM.MainCarriageVesselName; }
    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    get MainCarriageETD() { return this.EntityPM.MainCarriageETD; }
    get MainCarriageFromPortCountryCode() { return this.EntityPM.MainCarriageFromPortCountryCode; }
    get MainCarriageFromPortCountryName() { return this.EntityPM.MainCarriageFromPortCountryName; }
    get MainCarriageToPortCountryCode() { return this.EntityPM.MainCarriageToPortCountryCode; }
    get MainCarriageToPortCountryName() { return this.EntityPM.MainCarriageToPortCountryName; }


    get Transshipment1FromPortId() { return this.EntityPM.Transshipment1FromPortId; }
    get Transshipment1CarrierName() { return this.EntityPM.Transshipment1CarrierName; }
    get Transshipment1VesselName() { return this.EntityPM.Transshipment1VesselName; }
    get Transshipment1CarrierNumber() { return this.EntityPM.Transshipment1CarrierNumber; }
    get Transshipment1ETD() { return this.EntityPM.Transshipment1ETD; }
    get Transshipment1FromPortCountryCode() { return this.EntityPM.Transshipment1FromPortCountryCode; }
    get Transshipment1FromPortCountryName() { return this.EntityPM.Transshipment1FromPortCountryName; }
    get Transshipment1ToPortCountryCode() { return this.EntityPM.Transshipment1ToPortCountryCode; }
    get Transshipment1ToPortCountryName() { return this.EntityPM.Transshipment1ToPortCountryName; }



    get Transshipment2FromPortId() { return this.EntityPM.Transshipment2FromPortId; }
    get Transshipment2CarrierName() { return this.EntityPM.Transshipment2CarrierName; }
    get Transshipment2VesselName() { return this.EntityPM.Transshipment2VesselName; }
    get Transshipment2CarrierNumber() { return this.EntityPM.Transshipment2CarrierNumber; }
    get Transshipment2ETD() { return this.EntityPM.Transshipment2ETD; }
    get Transshipment2FromPortCountryCode() { return this.EntityPM.Transshipment2FromPortCountryCode; }
    get Transshipment2FromPortCountryName() { return this.EntityPM.Transshipment2FromPortCountryName; }
    get Transshipment2ToPortCountryCode() { return this.EntityPM.Transshipment2ToPortCountryCode; }
    get Transshipment2ToPortCountryName() { return this.EntityPM.Transshipment2ToPortCountryName; }


    get Transshipment3FromPortId() { return this.EntityPM.Transshipment3FromPortId; }
    get Transshipment3CarrierName() { return this.EntityPM.Transshipment3CarrierName; }
    get Transshipment3VesselName() { return this.EntityPM.Transshipment3VesselName; }
    get Transshipment3CarrierNumber() { return this.EntityPM.Transshipment3CarrierNumber; }
    get Transshipment3ETD() { return this.EntityPM.Transshipment3ETD; }
    get Transshipment3FromPortCountryCode() { return this.EntityPM.Transshipment3FromPortCountryCode; }
    get Transshipment3FromPortCountryName() { return this.EntityPM.Transshipment3FromPortCountryName; }
    get Transshipment3ToPortCountryCode() { return this.EntityPM.Transshipment3ToPortCountryCode; }
    get Transshipment3ToPortCountryName() { return this.EntityPM.Transshipment3ToPortCountryName; }
   

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

                    if (myResult.Success) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Show("Simulated Successfully");
                    }

                    else {
                        this.ValidationErrorsList = myResult.Errors;
                    }
                }
            });
        }
    }

    RefreshAnswersClicked() {

    }
}
