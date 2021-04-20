import {Component} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
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
import {OnCarriageDateComponent} from './OnCarriageDateComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {PackagesTabComponent, ShipmentPackageItem} from '../../../ShipmentPackages/Components/Packages/PackagesTabComponent';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

@Component({    
    templateUrl: './AddEditOnCarriageComponent.html',
})

export class AddEditOnCarriageComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public FatherComponent: RoutingsTabComponent;
    public IsConnectedHouse: boolean = false;
    public IsFCLEntity: boolean = false;
    public ItemsSource: OnCarriagePackageItem[] = [];
    public DepartureHeader: string = "Departure";
    public ArrivalHeader: string = "Arrival";
    private CurrentSession = SessionLocator.SelectedSession;
    public LegType: string;
    public IsOkButtonEnabled: boolean = true;
    private SaveCompletedEvent: any = null;
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
        this.LegType = args['LegType'];
        this.Clone();

        this.SetDefaultValues();
        this.SetUIProperties();
        this.SetDependencies();
        this.ComputeHeaders();
        this.BuildData();
    }

    SetDefaultValues() {
        this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);

        if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            this.IsConnectedHouse = true;
            
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageToPortId)) {
                this.OnForwardingFromPortId = this.EntityPM.OnCarriageToPortId;
            }

            else {
                this.OnForwardingFromPortId = this.EntityPM.ToPortId;
            }
        }
    }

    private ComputeHeaders() {
        if (this.EntityPM.ShipmentLevelCode == "H") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OnForwardingFromPortName)) {
                this.DepartureHeader = "Departure from " + this.EntityPM.OnForwardingFromPortName;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.OnForwardingToPortName)) {
                this.ArrivalHeader = "Arrival to " + this.EntityPM.OnForwardingToPortName;
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageFromPortName)) {
                this.DepartureHeader = "Departure from " + this.EntityPM.OnCarriageFromPortName;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageToPortName)) {
                this.ArrivalHeader = "Arrival to " + this.EntityPM.OnCarriageToPortName;
            }
        }
    }

    get IsCloseHouseInfoVisible() {
        var myResult = false;

        if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId != null && this.LegType == "On Carriage") {
            myResult = true;
        }

        return myResult;
    }

    BuildData() {
        this.ItemsSource = [];
        var packagesSource: ShipmentPackagePM[];
        var myItems: ShipmentPackagePM[];

        if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId != null && this.LegType == "On Carriage") {
            packagesSource = this.EntityPM.ConnectedMasterPackages;
        }

        else {
            packagesSource = this.EntityPM.ShipmentPackages;
        }       

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            myItems = packagesSource.filter(f => f.ContainerNumber != null && f.ContainerNumber.toLowerCase().startsWith(this.SearchText.toLowerCase()));
        }

        else {
            myItems = packagesSource;
        }

        myItems.forEach((item) => {
            this.ItemsSource.push(new OnCarriagePackageItem(item, this));
        });
    }

    public IsEditingEnabled: boolean = false;
    public IsCloseMasterInfoVisible: boolean = false;
    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.IsEditingEnabled = isEditingEnabled;

        if (this.EntityPM.ShipmentLevelCode == "H") {
            this.SetUIProperties_Forwarding();

            if (!AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId) && this.LegType == "On Carriage") {
                this.SetUIProperties_Carriage_ConnectedMaster();
                this.IsOkButtonEnabled = false;
            }
        }

        else {
            this.SetUIProperties_Carriage();
        }
    }
    SetUIProperties_Carriage() {
        var isTransportFieldEnabled = false;
        var isCarrierNumberFieldEnabled = false;
        var isConnectedMasterPortsEnabled = false;
        this.IsCloseMasterInfoVisible = false;

        if (this.IsEditingEnabled) {
            isConnectedMasterPortsEnabled = true;

            if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                if (this.EntityPM.StatusWeight >= 60) {
                    isConnectedMasterPortsEnabled = false;
                    this.IsCloseMasterInfoVisible = true;
                }
            }

            if (!AppTool.IsNullOrEmpty(this.OnCarriageTransportModeId)) {
                isTransportFieldEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.OnCarriageCarrierId)) {
                isCarrierNumberFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("OnCarriageTransportModeId", this.ObjectTableName, isConnectedMasterPortsEnabled);
        this.UIProperties.SetEnabled("OnCarriageFromPortId", this.ObjectTableName, isConnectedMasterPortsEnabled && !this.IsConnectedHouse && isTransportFieldEnabled);
        this.UIProperties.SetEnabled("OnCarriageToPortId", this.ObjectTableName, isConnectedMasterPortsEnabled && isTransportFieldEnabled);
        this.UIProperties.SetEnabled("OnCarriageCarrierId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("OnCarriageCarrierNumber", this.ObjectTableName, isCarrierNumberFieldEnabled);
        this.UIProperties.SetEnabled("OnCarriageVesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageATA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("SplitOnCarriage", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageAdditionalTransportModeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetVisibility("OnCarriageVesselId", this.ObjectTableName, this.OnCarriageTransportModeId == "O" ? true : false);
        this.SetUIProperties_Carriage_RequiredFields();
        this.SetUIProperties_Carriage_ValidateActualDates();
    }
    SetUIProperties_Forwarding() {
        var isTransportFieldEnabled = false;
        var isCarrierNumberFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.OnForwardingTransportModeId)) {
                isTransportFieldEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.OnForwardingCarrierId)) {
                isCarrierNumberFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("OnForwardingTransportModeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnForwardingFromPortId", this.ObjectTableName, isTransportFieldEnabled && this.IsConnectedHouse == false);
        this.UIProperties.SetEnabled("OnForwardingToPortId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("OnForwardingCarrierId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("OnForwardingCarrierNumber", this.ObjectTableName, isCarrierNumberFieldEnabled);
        this.UIProperties.SetEnabled("OnForwardingVesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnForwardingETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnForwardingETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnForwardingATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnForwardingATA", this.ObjectTableName, this.IsEditingEnabled);        
        this.UIProperties.SetEnabled("OnForwardingAdditionalTransportModeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetVisibility("OnForwardingVesselId", this.ObjectTableName, this.OnForwardingTransportModeId == "O" ? true : false);
        this.SetUIProperties_Forwarding_RequiredFields();
        this.SetUIProperties_Forwarding_ValidateActualDates();
    }
    SetUIProperties_Carriage_RequiredFields() {
        this.UIProperties.SetRequired("OnCarriageTransportModeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OnCarriageTransportModeId) ? true : false);
        this.UIProperties.SetRequired("OnCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OnCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("OnCarriageToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OnCarriageToPortId) ? true : false);
    }
    SetUIProperties_Carriage_ValidateActualDates() {
        this.UIProperties.SetValidity("OnCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("OnCarriageATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.OnCarriageATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("OnCarriageATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.OnCarriageATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("OnCarriageATA", this.ObjectTableName, false, errorMessage);
        }
    }
    SetUIProperties_Forwarding_RequiredFields() {
        this.UIProperties.SetRequired("OnForwardingTransportModeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OnForwardingTransportModeId) ? true : false);
        this.UIProperties.SetRequired("OnForwardingFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OnForwardingFromPortId) ? true : false);
        this.UIProperties.SetRequired("OnForwardingToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OnForwardingToPortId) ? true : false);
    }
    SetUIProperties_Forwarding_ValidateActualDates() {
        this.UIProperties.SetValidity("OnForwardingATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("OnForwardingATA", this.ObjectTableName, true, null);

        if (!DateTool.IsActualDateValid(this.OnForwardingATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("OnForwardingATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.OnForwardingATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("OnForwardingATA", this.ObjectTableName, false, errorMessage);
        }
    }
    SetUIProperties_Carriage_ConnectedMaster() {
        this.UIProperties.SetEnabled("OnCarriageTransportModeId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageFromPortId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageToPortId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageCarrierId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageCarrierNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageVesselId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageETD", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageETA", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageATD", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageATA", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("OnCarriageVesselId", this.ObjectTableName, this.OnCarriageTransportModeId == "O" ? true : false);
        this.UIProperties.SetEnabled("SplitOnCarriage", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OnCarriageAdditionalTransportModeCode", this.ObjectTableName, false);
    }

    public CarrierDependencyProperty1: string = null;
    SetDependencies() {
        var myResult: string = null;
        var transportModeId = null;

        if (this.EntityPM.ShipmentLevelCode == "H") {
            transportModeId = this.OnForwardingTransportModeId;
        }
        else {
            transportModeId = this.OnCarriageTransportModeId;
        }

        switch (transportModeId) {
            case "A": { myResult = "AL"; break; }
            case "O": { myResult = "SL"; break; }
            case "I": { myResult = "TR"; break; }
        }

        this.CarrierDependencyProperty1 = myResult;
    }

    get OnCarriageTransportModeId() { return this.EntityPM.OnCarriageTransportModeId; }
    set OnCarriageTransportModeId(value: string) {
        if (this.EntityPM.OnCarriageTransportModeId != value) {
            this.EntityPM.OnCarriageTransportModeId = value;

            if (!this.IsConnectedHouse) {
                this.OnCarriageFromPortId = null;
            }

            this.OnCarriageToPortId = null;
            this.OnCarriageCarrierId = null;
            this.OnCarriageCarrierNumber = null;
            this.OnCarriageVesselId = null;
            this.SetUIProperties_Carriage();
            this.SetDependencies();
        }
    }

    get OnCarriageAdditionalTransportModeCode() { return this.EntityPM.OnCarriageAdditionalTransportModeCode; }
    set OnCarriageAdditionalTransportModeCode(value: string) {
        if (this.EntityPM.OnCarriageAdditionalTransportModeCode != value) {
            this.EntityPM.OnCarriageAdditionalTransportModeCode = value;
        }
    }

    get OnCarriageFromPortId() { return this.EntityPM.OnCarriageFromPortId; }
    set OnCarriageFromPortId(value: string) {
        if (this.EntityPM.OnCarriageFromPortId != value) {
            this.EntityPM.OnCarriageFromPortId = value;
            this.SetUIProperties_Carriage_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.OnCarriageFromPortChanged(this.EntityPM, null);
                this.ComputeHeaders();
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            RoutingHelper.OnCarriageFromPortChanged(this.EntityPM, list);
                            this.ComputeHeaders();
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    RoutingHelper.OnCarriageFromPortChanged(this.EntityPM, list);
                                    this.ComputeHeaders();
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get OnCarriageToPortId() { return this.EntityPM.OnCarriageToPortId; }
    set OnCarriageToPortId(value: string) {
        if (this.EntityPM.OnCarriageToPortId != value) {
            this.EntityPM.OnCarriageToPortId = value;
            this.SetUIProperties_Carriage_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.OnCarriageToPortCode = null;
                this.EntityPM.OnCarriageToPortName = null;
                this.EntityPM.OnCarriageToPortCountryCode = null;
                this.EntityPM.OnCarriageToPortCountryName = null;
                this.ComputeHeaders();
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.EntityPM.OnCarriageToPortCode = list.Code;
                            this.EntityPM.OnCarriageToPortName = list.EnglishName;
                            this.EntityPM.OnCarriageToPortCountryCode = list.CountryCode;
                            this.EntityPM.OnCarriageToPortCountryName = list.CountryName;
                            this.ComputeHeaders();
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    if (list) {
                                        this.EntityPM.OnCarriageToPortCode = list.Code;
                                        this.EntityPM.OnCarriageToPortName = list.EnglishName;
                                        this.EntityPM.OnCarriageToPortCountryCode = list.CountryCode;
                                        this.EntityPM.OnCarriageToPortCountryName = list.CountryName;
                                        this.ComputeHeaders();
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get OnCarriageCarrierId() { return this.EntityPM.OnCarriageCarrierId; }
    set OnCarriageCarrierId(value: string) {
        if (this.EntityPM.OnCarriageCarrierId != value) {
            this.EntityPM.OnCarriageCarrierId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.OnCarriageCarrierCode = null;
                this.EntityPM.OnCarriageCarrierName = null;
                this.EntityPM.OnCarriageCarrierWebSite = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.EntityPM.OnCarriageCarrierCode = list.Code;
                            this.EntityPM.OnCarriageCarrierName = list.EnglishName;
                            this.EntityPM.OnCarriageCarrierWebSite = list.WebSite;
                        }
                    }
                });
            }
        }
    }

    get OnCarriageCarrierNumber() { return this.EntityPM.OnCarriageCarrierNumber; }
    set OnCarriageCarrierNumber(value: string) {
        if (this.EntityPM.OnCarriageCarrierNumber != value) {
            this.EntityPM.OnCarriageCarrierNumber = value;
        }
    }

    get OnCarriageVesselId() { return this.EntityPM.OnCarriageVesselId; }
    set OnCarriageVesselId(value: string) {
        if (this.EntityPM.OnCarriageVesselId != value) {
            this.EntityPM.OnCarriageVesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.OnCarriageVesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.OnCarriageVesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get OnCarriageETD() { return this.EntityPM.OnCarriageETD; }
    set OnCarriageETD(value: Date) {
        if (this.EntityPM.OnCarriageETD != value) {
            this.EntityPM.OnCarriageETD = value;
        }
    }

    get OnCarriageETA() { return this.EntityPM.OnCarriageETA; }
    set OnCarriageETA(value: Date) {
        if (this.EntityPM.OnCarriageETA != value) {
            this.EntityPM.OnCarriageETA = value;
        }
    }

    get OnCarriageATD() { return this.EntityPM.OnCarriageATD; }
    set OnCarriageATD(value: Date) {
        if (this.EntityPM.OnCarriageATD != value) {
            this.EntityPM.OnCarriageATD = value;
            this.SetUIProperties_Carriage_ValidateActualDates();
        }
    }

    get OnCarriageATA() { return this.EntityPM.OnCarriageATA; }
    set OnCarriageATA(value: Date) {
        if (this.EntityPM.OnCarriageATA != value) {
            this.EntityPM.OnCarriageATA = value;
            this.SetUIProperties_Carriage_ValidateActualDates();
        }
    }

    get SplitOnCarriage() { return this.EntityPM.SplitOnCarriage; }
    set SplitOnCarriage(value: boolean) {
        if (this.EntityPM.SplitOnCarriage != value) {
            this.EntityPM.SplitOnCarriage = value;

            this.EntityPM.ShipmentPackages.forEach((item) => {
                if (item.OnCarriageETD == null) {
                    item.OnCarriageETD = this.OnCarriageETD;
                }

                if (item.OnCarriageATD == null) {
                    item.OnCarriageATD = this.OnCarriageATD;
                }

                if (item.OnCarriageETA == null) {
                    item.OnCarriageETA = this.OnCarriageETA;
                }

                if (item.OnCarriageATA == null) {
                    item.OnCarriageATA = this.OnCarriageATA;
                }
            });   

            this.BuildData();
        }
    }

    get OnForwardingTransportModeId() { return this.EntityPM.OnForwardingTransportModeId; }
    set OnForwardingTransportModeId(value: string) {
        if (this.EntityPM.OnForwardingTransportModeId != value) {
            this.EntityPM.OnForwardingTransportModeId = value;

            if (!this.IsConnectedHouse) {
                this.OnForwardingFromPortId = null;
            }

            this.OnForwardingToPortId = null;
            this.OnForwardingCarrierId = null;
            this.OnForwardingCarrierNumber = null;
            this.OnForwardingVesselId = null;
            this.SetUIProperties_Forwarding();
            this.SetDependencies();
        }
    }

    get OnForwardingAdditionalTransportModeCode() { return this.EntityPM.OnForwardingAdditionalTransportModeCode; }
    set OnForwardingAdditionalTransportModeCode(value: string) {
        if (this.EntityPM.OnForwardingAdditionalTransportModeCode != value) {
            this.EntityPM.OnForwardingAdditionalTransportModeCode = value;
        }
    }

    get OnForwardingFromPortId() { return this.EntityPM.OnForwardingFromPortId; }
    set OnForwardingFromPortId(value: string) {
        if (this.EntityPM.OnForwardingFromPortId != value) {
            this.EntityPM.OnForwardingFromPortId = value;
            this.SetUIProperties_Forwarding_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.OnForwardingFromPortChanged(this.EntityPM, null);
                this.ComputeHeaders();
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            RoutingHelper.OnForwardingFromPortChanged(this.EntityPM, list);
                            this.ComputeHeaders();
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    RoutingHelper.OnForwardingFromPortChanged(this.EntityPM, list);
                                    this.ComputeHeaders();
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get OnForwardingToPortId() { return this.EntityPM.OnForwardingToPortId; }
    set OnForwardingToPortId(value: string) {
        if (this.EntityPM.OnForwardingToPortId != value) {
            this.EntityPM.OnForwardingToPortId = value;
            this.SetUIProperties_Forwarding_RequiredFields();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.OnForwardingToPortCode = null;
                this.EntityPM.OnForwardingToPortName = null;
                this.EntityPM.OnForwardingToPortCountryCode = null;
                this.EntityPM.OnForwardingToPortCountryName = null;
                this.ComputeHeaders();
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.EntityPM.OnForwardingToPortCode = list.Code;
                            this.EntityPM.OnForwardingToPortName = list.EnglishName;
                            this.EntityPM.OnForwardingToPortCountryCode = list.CountryCode;
                            this.EntityPM.OnForwardingToPortCountryName = list.CountryName;
                            this.ComputeHeaders();
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    list = myResponse2.Result;
                                    if (list) {
                                        this.EntityPM.OnForwardingToPortCode = list.Code;
                                        this.EntityPM.OnForwardingToPortName = list.EnglishName;
                                        this.EntityPM.OnForwardingToPortCountryCode = list.CountryCode;
                                        this.EntityPM.OnForwardingToPortCountryName = list.CountryName;
                                        this.ComputeHeaders();
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get OnForwardingCarrierId() { return this.EntityPM.OnForwardingCarrierId; }
    set OnForwardingCarrierId(value: string) {
        if (this.EntityPM.OnForwardingCarrierId != value) {
            this.EntityPM.OnForwardingCarrierId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.OnForwardingCarrierCode = null;
                this.EntityPM.OnForwardingCarrierName = null;
                this.EntityPM.OnForwardingCarrierWebSite = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.EntityPM.OnForwardingCarrierCode = list.Code;
                            this.EntityPM.OnForwardingCarrierName = list.EnglishName;
                            this.EntityPM.OnForwardingCarrierWebSite = list.WebSite;
                        }
                    }
                });
            }
        }
    }

    get OnForwardingCarrierNumber() { return this.EntityPM.OnForwardingCarrierNumber; }
    set OnForwardingCarrierNumber(value: string) {
        if (this.EntityPM.OnForwardingCarrierNumber != value) {
            this.EntityPM.OnForwardingCarrierNumber = value;
        }
    }

    get OnForwardingVesselId() { return this.EntityPM.OnForwardingVesselId; }
    set OnForwardingVesselId(value: string) {
        if (this.EntityPM.OnForwardingVesselId != value) {
            this.EntityPM.OnForwardingVesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.OnForwardingVesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.OnForwardingVesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get OnForwardingETD() { return this.EntityPM.OnForwardingETD; }
    set OnForwardingETD(value: Date) {
        if (this.EntityPM.OnForwardingETD != value) {
            this.EntityPM.OnForwardingETD = value;
        }
    }

    get OnForwardingETA() { return this.EntityPM.OnForwardingETA; }
    set OnForwardingETA(value: Date) {
        if (this.EntityPM.OnForwardingETA != value) {
            this.EntityPM.OnForwardingETA = value;
        }
    }

    get OnForwardingATD() { return this.EntityPM.OnForwardingATD; }
    set OnForwardingATD(value: Date) {
        if (this.EntityPM.OnForwardingATD != value) {
            this.EntityPM.OnForwardingATD = value;
            this.SetUIProperties_Forwarding_ValidateActualDates();
        }
    }

    get OnForwardingATA() { return this.EntityPM.OnForwardingATA; }
    set OnForwardingATA(value: Date) {
        if (this.EntityPM.OnForwardingATA != value) {
            this.EntityPM.OnForwardingATA = value;
            this.SetUIProperties_Forwarding_ValidateActualDates();
        }
    }

    private searchText: string = null;
    public get SearchText() { return this.searchText; }
    public set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }

    SearchTextChanged(text: string) {
        this.SearchText = text;
        this.BuildData();
    }

    SetActualDateClicked(fieldName: string) {
        if (this.EntityPM.ShipmentLevelCode == "H") {
            switch (fieldName) {
                case "OnForwardingETD": { this.OnForwardingATD = DateTool.GetDateParts(this.OnForwardingETD).DateObject; break; }
                case "OnForwardingETA": { this.OnForwardingATA = DateTool.GetDateParts(this.OnForwardingETA).DateObject; break; }
            }
        }

        else {
            switch (fieldName) {
                case "OnCarriageETD": { this.OnCarriageATD = DateTool.GetDateParts(this.OnCarriageETD).DateObject; break; }
                case "OnCarriageETA": { this.OnCarriageATA = DateTool.GetDateParts(this.OnCarriageETA).DateObject; break; }
            }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidationErrorsList = this.Validate();

        if (this.ValidationErrorsList.length == 0) {
            this.AssignAdditionalTransportModeToPackages();
            var isShowChangePortsWindow: boolean = this.CheckToShowChangePortsWindow();            

            if (isShowChangePortsWindow) {
                this.ShowConfirmChangePortsWindow();
            }

            else {
                this.CloseWindowOnOk();
            }
        }
    }
    private Validate(): string[] {
        var errors: string[] = []; 
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.EntityPM.ShipmentLevelCode == "H") {
            errors = this.ValidateOnForwarding(msg);
        }

        else {
            errors = this.ValidateOnCarriage(msg);
        }

        return errors;
    }
    private ValidateOnCarriage(msg: string): string[] {
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.OnCarriageTransportModeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.TransportMode")));
        }

        if (AppTool.IsNullOrEmpty(this.OnCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.From")));
        }

        if (AppTool.IsNullOrEmpty(this.OnCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.To")));
        }

        if (!this.SplitOnCarriage) {
            this.EntityPM.ShipmentPackages.forEach((item) => {
                item.OnCarriageETD = null;
                item.OnCarriageATD = null;
                item.OnCarriageETA = null;
                item.OnCarriageATA = null;
            });
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "OnCarriage");

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.OnCarriageATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD")));
        }

        if (!DateTool.IsActualDateValid(this.OnCarriageATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA")));
        }

        return errors;
    }
    private ValidateOnForwarding(msg: string): string[] {
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.OnForwardingTransportModeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.TransportMode")));
        }

        if (AppTool.IsNullOrEmpty(this.OnForwardingFromPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.From")));
        }

        if (AppTool.IsNullOrEmpty(this.OnForwardingToPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.To")));
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "OnForwarding");

        // Actual Dates
        if (!DateTool.IsActualDateValid(this.OnForwardingATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATD")));
        }

        if (!DateTool.IsActualDateValid(this.OnForwardingATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", TextCodeTranslator.Translate("Shipment.O.Routings.ATA")));
        }

        return errors;
    }
    private AssignAdditionalTransportModeToPackages() {
        var transportMode = this.EntityPM.ShipmentLevelCode == "H" ? this.OnForwardingAdditionalTransportModeCode : this.OnCarriageAdditionalTransportModeCode;

        if (!AppTool.IsNullOrEmpty(transportMode)) {
            this.EntityPM.ShipmentPackages.forEach(item => {

                if (AppTool.IsNullOrEmpty(item.DeliveryId)) {
                    item.DeliveryTransportModeCode = transportMode;
                }

                if (AppTool.IsNullOrEmpty(item.EmptyContainerReturnId)) {
                    item.ECRTransportModeCode = transportMode;
                }
            });
        }
    }
    private CheckToShowChangePortsWindow(): boolean {
        var isShowChangePortsWindow: boolean = false;
        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            if (this.EntityPM.ShipmentConsoleShipments.filter(d => !AppTool.IsNullOrEmpty(d.OnForwardingFromPortId) && !AppTool.IsNullOrEmpty(d.OnForwardingToPortId)).length > 0) {
                if (this.EntityPM.OriginOnCarriageToPortId != this.EntityPM.OnCarriageToPortId) {
                    isShowChangePortsWindow = true;
                }
            }

            else {
                if (this.EntityPM.OriginOnCarriageFromPortId != this.EntityPM.OnCarriageFromPortId) {
                    isShowChangePortsWindow = true;
                }
            }
        }

        return isShowChangePortsWindow;
    }
    private ShowConfirmChangePortsWindow() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Ports Changed";
        confirmWindow.Show("Updating the Master shipment ports will update the house shipment accordingly");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SaveMasterHousesPorts();
            }
        });
    }
    private SaveMasterHousesPorts() {
        if (!this.SaveCompletedEvent) {
            this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                AppTool.KillEventEmitter(this.SaveCompletedEvent);
                this.SaveCompletedEvent = null;

                if (isSaveSuccess) {
                    this.CurrentSession.SessionEvent.emit("ReloadHouses");
                    this.CloseWindowOnOk();
                }

                else {
                    this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                }
            });

            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
    private CloseWindowOnOk() {
        this.FatherComponent.BuildItemsCollection();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }    

    EditContainer(itemComponent: OnCarriagePackageItem) {
        var tabComponent: PackagesTabComponent = new PackagesTabComponent(this.FatherComponent.entityArgs, new EntityResourceService());
        var packagecomponent: ShipmentPackageItem = new ShipmentPackageItem(itemComponent.EntityPM, tabComponent)

        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("ShipmentPackage.O.EditContainer");
        logWindow.Width = 940;
        logWindow.Height = 550;
        logWindow.DataContext = packagecomponent;
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent");
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
        this.myCloner.AddField('OnCarriageTransportModeId');
        this.myCloner.AddField('OnCarriageFromPortId');
        this.myCloner.AddField('OnCarriageToPortId');
        this.myCloner.AddField('OnCarriageCarrierId');
        this.myCloner.AddField('OnCarriageCarrierNumber');
        this.myCloner.AddField('OnCarriageVesselId');
        this.myCloner.AddField('OnCarriageETD');
        this.myCloner.AddField('OnCarriageETA');
        this.myCloner.AddField('OnCarriageATD');
        this.myCloner.AddField('OnCarriageATA');
        this.myCloner.AddField('SplitOnCarriage');
        this.myCloner.AddField('OnForwardingTransportModeId');
        this.myCloner.AddField('OnForwardingFromPortId');
        this.myCloner.AddField('OnForwardingToPortId');
        this.myCloner.AddField('OnForwardingCarrierId');
        this.myCloner.AddField('OnForwardingCarrierNumber');
        this.myCloner.AddField('OnForwardingVesselId');
        this.myCloner.AddField('OnForwardingETD');
        this.myCloner.AddField('OnForwardingETA');
        this.myCloner.AddField('OnForwardingATD');
        this.myCloner.AddField('OnForwardingATA');
        this.myCloner.AddEntity(this.EntityPM);

        this.entityCloner = new Cloner(this.EntityPM);
        this.entityCloner.AddField('Transshipment3ToPortId');
        this.entityCloner.AddField('Transshipment3ToPortCode');
        this.entityCloner.AddField('Transshipment3ToPortName');
        this.entityCloner.AddField('Transshipment3ToPortCountryCode');
        this.entityCloner.AddField('Transshipment3ToPortCountryName');

        this.entityCloner.AddField('Transshipment2ToPortId');
        this.entityCloner.AddField('Transshipment2ToPortCode');
        this.entityCloner.AddField('Transshipment2ToPortName');
        this.entityCloner.AddField('Transshipment2ToPortCountryCode');
        this.entityCloner.AddField('Transshipment2ToPortCountryName');

        this.entityCloner.AddField('Transshipment1ToPortId');
        this.entityCloner.AddField('Transshipment1ToPortCode');
        this.entityCloner.AddField('Transshipment1ToPortName');
        this.entityCloner.AddField('Transshipment1ToPortCountryCode');
        this.entityCloner.AddField('Transshipment1ToPortCountryName');

        this.entityCloner.AddField('MainCarriageToPortId');
        this.entityCloner.AddField('MainCarriageToPortCode');
        this.entityCloner.AddField('MainCarriageToPortName');
        this.entityCloner.AddField('MainCarriageToPortCountryCode');
        this.entityCloner.AddField('MainCarriageToPortCountryName');

        this.entityCloner.AddField('ToCountryId');
        this.entityCloner.AddField('ToCountryIsEC');
        this.entityCloner.AddField('FinalDistenationPortId');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortId');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortCode');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortName');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortCountryCode');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortCountryName');

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

        this.ItemsSource.forEach(item => {
            item.RejectChanges();
        });

        this.myCloner.RejectChanges();
        this.entityCloner.RejectChanges();
    }
}

export class OnCarriagePackageItem extends BaseComponent {
    public EntityPM: ShipmentPackagePM;
    public ShipmentPM: ShipmentPM;
    public DataContext = this;
    public ObjectTableName: string = "ShipmentPackage";
    constructor(item: ShipmentPackagePM, public fatherComponent: AddEditOnCarriageComponent) {
        super();
        this.EntityPM = item;
        this.ShipmentPM = fatherComponent.EntityPM;

        this.Clone();
        this.ComputeDepartureArrival();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('OnCarriageETD');
        this.myCloner.AddField('OnCarriageETA');
        this.myCloner.AddField('OnCarriageATD');
        this.myCloner.AddField('OnCarriageATA');
        this.myCloner.AddEntity(this.EntityPM);
    }
    public RejectChanges() {
        this.myCloner.RejectChanges();
    }

    public DepartureDate: Date;
    public DepartureColor: string;
    public ArrivalDate: Date;
    public ArrivalColor: string;
    ComputeDepartureArrival() {        
        var myDepartureColor: string = FontTool.Orange;
        var myArrivalColor: string = FontTool.Orange;

        var myDepartureDate: Date;
        var myArrivalDate: Date;
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        var myETD: Date = this.EntityPM.OnCarriageETD;
        var myATD: Date = this.EntityPM.OnCarriageATD;
        var myETA: Date = this.EntityPM.OnCarriageETA;
        var myATA: Date = this.EntityPM.OnCarriageATA;

        if (myATD != null) {
            myDepartureDate = myATD;
            myDepartureColor = FontTool.Green;
        }

        else if (myETD != null) {
            myDepartureDate = myETD;

            if (DateTool.GetDateParts(myDepartureDate).DateTicks < DateTool.GetDateParts(todayDate).DateTicks) {
                myDepartureColor = FontTool.Red;
            }
        }

        if (myATA != null) {
            myArrivalDate = myATA;
            myArrivalColor = FontTool.Green;
        }

        else if (myETA != null) {
            myArrivalDate = myETA;

            if (DateTool.GetDateParts(myArrivalDate).DateTicks < DateTool.GetDateParts(todayDate).DateTicks) {
                myArrivalColor = FontTool.Red;
            }
        }

        this.DepartureDate = myDepartureDate;
        this.DepartureColor = myDepartureColor;
        this.ArrivalDate = myArrivalDate;
        this.ArrivalColor = myArrivalColor;
    }

    OnDateComponentClosed(dateComponent: OnCarriageDateComponent) {
        if (dateComponent) {
            this.EntityPM = dateComponent.EntityPM;

            this.ComputeDepartureArrival();
        }
    }

    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    get ContainerType() { return this.EntityPM.PackageTypeCode; }   
}
