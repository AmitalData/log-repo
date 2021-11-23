import {Component} from '@angular/core';
import {AppTool, DateTool, FormatTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ShipmentValidator} from '../../../../Shipment/Validators/ShipmentValidator';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {RoutingsTabComponent} from './RoutingsTabComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {PortList} from '../../../../Common/EntityLists/PortList';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {VesselList} from '../../../../Common/EntityLists/VesselList';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AirlineListService} from '../../../../Common/Services/StandardLists/AirlineListService';
import {VesselListService} from '../../../../Common/Services/StandardLists/VesselListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {GetStackWindowArgs} from '../../../../Common/Args';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {AWBStackDomainService} from '../../../../Common/Services/AWBStackDomainService';
import {MAWBStackPM} from '../../../../Common/EntityPMs/MAWBStackPM';
import { FeatureToggleList } from '../../../../Infrastructure/EntityLists/FeatureToggleList';

@Component({
    
    templateUrl: './AddEditMainCarriageComponent.html',
})

export class AddEditMainCarriageComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public DataContext: AddEditMainCarriageComponent = this;
    public ObjectTableName: string;
    public ValidationErrorsList: string[] = [];
    public FatherComponent: RoutingsTabComponent;
    public LabelWidth: number = 100;
    private CurrentSession = SessionLocator.SelectedSession;
    private oldCountryId: string = null;
    private IsContainersToggleFeatureUp: boolean = false;

    constructor() {
        super();
        this.InitServices();
    }

    private myPortListService: PortListService;
    private myCardListService: CardListService;
    private myAirlineListService: AirlineListService;
    private myVesselListService: VesselListService;
    private StackDomainService: AWBStackDomainService;
    private myPartnersDomainService: PartnersDomainService;
    InitServices() {
        this.myPortListService = new PortListService();
        this.myCardListService = new CardListService();
        this.myAirlineListService = new AirlineListService();
        this.myVesselListService = new VesselListService();
        this.StackDomainService = new AWBStackDomainService();
        this.myPartnersDomainService = new PartnersDomainService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.LabelWidth = this.EntityPM.TransportModeId == "I" ? 115 : 100;
        this.oldCountryId = this.EntityPM.ToCountryId;

        if (this.EntityPM.TransportModeId == "A") {
            this.LabelWidth = 80;
        }

        else if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "E") {
            this.LabelWidth = 150;
        }

        this.InitializeComponent();
        this.SetUIProperties();
        this.Clone();
        this.CheckContainersToggleFeatureUp();
    }

    public DirectionId: string = null;
    public TransportModeId: string = null;
    public FromTextCodeLabel: string = null;
    public Via1Label: string = null;
    public Via2Label: string = null;
    public Via3Label: string = null;
    public ToTextCodeLabel: string = null;
    public ConnectedMasterText: string = null;
    public Leg0TextCodeLabel: string = null;
    public Leg1TextCodeLabel: string = null;
    public Leg2TextCodeLabel: string = null;
    public Leg3TextCodeLabel: string = null;
    public CarrierTextCode: string = null;
    public CarrierNumberTextCode: string = null;
    public MasterTextCode: string = null;
    public MasterDateTextCode: string = null;
    public CarrierDependencyProperty1: string = null;
    public ETDLabel: string = null;
    public ETALabel: string = null;
    public ATDLabel: string = null;
    public ATALabel: string = null;
    InitializeComponent() {
        this.DirectionId = this.EntityPM.DirectionId;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.Via1Label = TextCodeTranslator.Translate("Shipment.O.Routings.Via1");
        this.Via2Label = TextCodeTranslator.Translate("Shipment.O.Routings.Via2");
        this.Via3Label = TextCodeTranslator.Translate("Shipment.O.Routings.Via3");
        this.ETDLabel = TextCodeTranslator.Translate("Shipment.O.Routings.ETD");
        this.ETALabel = TextCodeTranslator.Translate("Shipment.O.Routings.ETA");
        this.ATDLabel = TextCodeTranslator.Translate("Shipment.O.Routings.ATD");
        this.ATALabel = TextCodeTranslator.Translate("Shipment.O.Routings.ATA");
        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;

        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.Gateway");
                this.ToTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.Destination");
                this.Leg0TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg1");
                this.Leg1TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg2");
                this.Leg2TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg3");
                this.Leg3TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg4");
                this.CarrierTextCode = "Shipment.O.Routings.Airline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.FlightNo";
                this.MasterTextCode = "Shipment.O.Routings.MAWB";
                this.MasterDateTextCode = "Shipment.O.Routings.MAWBDate";
                this.CarrierDependencyProperty1 = "AL";
                break;
            }

            case "O": {
                this.FromTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.LoadingPort");
                this.ToTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.DischargePort");
                this.Leg0TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriage");
                this.Leg1TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.Transshipment1");
                this.Leg2TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.Transshipment2");
                this.Leg3TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.Transshipment3");
                this.CarrierTextCode = "Shipment.O.Routings.Shippingline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.VoyageNo";
                this.MasterTextCode = "Shipment.O.Routings.OBL";
                this.MasterDateTextCode = "Shipment.O.Routings.OBLDate";
                this.CarrierDependencyProperty1 = "SL";
                break;
            }

            case "I": {
                this.FromTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.From");
                this.ToTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.To");
                this.Leg0TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg1");
                this.Leg1TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg2");
                this.Leg2TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg3");
                this.Leg3TextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg4");
                this.CarrierTextCode = "Shipment.O.Routings.Trucker";
                this.CarrierNumberTextCode = "Shipment.O.Routings.TruckNo";
                this.MasterTextCode = "Shipment.O.Routings.CMR/RWB#";
                this.MasterDateTextCode = "Shipment.O.Routings.CMR/RWBDate";
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
        }

        this.ConnectedMasterText = "This master is departed and connected to house shipments, can't edit " + this.FromTextCodeLabel + " or " + this.ToTextCodeLabel;
    }

    get IsCloseHouseInfoVisible() {
        var myResult = false;

        if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId != null) {
            myResult = true;
        }

        return myResult;
    }
    //get IsCloseMasterInfoVisible() {
    //    var myResult = false;

    //    if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
    //        myResult = true;
    //    }

    //    return myResult;
    //}
    public IsEditingEnabled: boolean = true;
    public IsEditingEntityEnabled: boolean = true;
    public IsPortsEditingEnabled: boolean = true;
    public IsCloseMasterInfoVisible: boolean = false;

    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.IsEditingEntityEnabled = isEditingEnabled;

        if (isEditingEnabled) {
            if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                isEditingEnabled = false;
            }
        }

        var isPortsEditingEnabled = false;

        if (isEditingEnabled) {

            isPortsEditingEnabled = true;

            if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                isPortsEditingEnabled = false;
            }

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEditingEnabled = false;
            }
        }

        this.IsEditingEnabled = isEditingEnabled;
        this.IsPortsEditingEnabled = isPortsEditingEnabled;

        this.SetUIProperties_Ports();
        this.SetUIProperties_MainCarriage();
        this.SetUIProperties_Transshipment1();
        this.SetUIProperties_Transshipment2();
        this.SetUIProperties_Transshipment3();
        this.SetUIProperties_ValidateActualDates();
    }
    SetUIProperties_Ports() {
        var isPortsEditingEnabled = false;
        var isMainPortsEnabled = false;
        var isPortVia1Enabled = false;
        var isPortVia2Enabled = false;
        var isPortVia3Enabled = false;
        this.IsCloseMasterInfoVisible = false;

        if (this.IsEditingEnabled) {

            isPortsEditingEnabled = true;

            if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                isPortsEditingEnabled = false;
            }

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEditingEnabled = false;
            }
        }

        if (isPortsEditingEnabled) {

            isMainPortsEnabled = true;
            isPortVia1Enabled = true;

            if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                //isMainPortsEnabled = false;

                if (this.EntityPM.StatusWeight >= 60) {
                    isMainPortsEnabled = false;
                    this.IsCloseMasterInfoVisible = true;
                }
            }

            if (this.Transshipment1FromPortId != null || this.Transshipment2FromPortId != null || this.Transshipment3FromPortId != null) {
                isPortVia2Enabled = true;
            }

            if (this.Transshipment2FromPortId != null || this.Transshipment3FromPortId != null) {
                isPortVia3Enabled = true;
            }
        }

        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isMainPortsEnabled);
        this.UIProperties.SetEnabled("MainCarriageFinalDestinationPortId", this.ObjectTableName, isMainPortsEnabled);

        this.UIProperties.SetEnabled("Transshipment1FromPortId", this.ObjectTableName, isPortVia1Enabled);
        this.UIProperties.SetEnabled("Transshipment2FromPortId", this.ObjectTableName, isPortVia2Enabled);
        this.UIProperties.SetEnabled("Transshipment3FromPortId", this.ObjectTableName, isPortVia3Enabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageFinalDestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageFinalDestinationPortId) ? true : false);

        //var isPortVia1Required = AppTool.IsNullOrEmpty(this.Transshipment1FromPortId) && !AppTool.IsNullOrEmpty(this.Transshipment2FromPortId) ? true : false;
        //var isPortVia2Required = AppTool.IsNullOrEmpty(this.Transshipment2FromPortId) && !AppTool.IsNullOrEmpty(this.Transshipment3FromPortId) ? true : false;
        //this.UIProperties.SetRequired("Transshipment1FromPortId", this.ObjectTableName, isPortVia1Required);
        //this.UIProperties.SetRequired("Transshipment2FromPortId", this.ObjectTableName, isPortVia2Required);
    }
    SetUIProperties_MainCarriage() {
        var isCarrierEnabled = this.IsEditingEnabled;
        var isLegFieldsEnabled = this.IsEditingEnabled;
        var isInterlineEnabled = this.IsEditingEnabled;
        var isMasterEnabled = this.IsEditingEnabled;
        var isETDEnabled = this.IsEditingEnabled;

        if (this.IsEditingEnabled) {
            isLegFieldsEnabled = false;
            isMasterEnabled = false;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isCarrierEnabled = false;
                isInterlineEnabled = false;
                isETDEnabled = false;
            }

            else {
                if (this.TransportModeId == "A") {
                    var isTakenFromStock = (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) ? true : false;

                    if (isTakenFromStock || !AppTool.IsNullOrEmpty(this.Master)) {
                        isInterlineEnabled = false;
                        isCarrierEnabled = false;
                    }

                    if (!isTakenFromStock) {
                        if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) || !AppTool.IsNullOrEmpty(this.InterlineId)) {
                            isMasterEnabled = true;
                        }
                    }
                }

                else {
                    isMasterEnabled = true;
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    isLegFieldsEnabled = true;
                }

                //else {
                //    isMasterEnabled = false;
                //}
            }
        }

        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("InterlineId", this.ObjectTableName, isInterlineEnabled);
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isMasterEnabled);
        this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, isETDEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("MAWBOBLDate", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CutoffDate", this.ObjectTableName, this.IsEditingEnabled);        
        this.UIProperties.SetEnabled("MainCarriageVesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageATA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OBLTypeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DocumentsClosingDate", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_Interline();
        this.SetUIProperties_StockButton();
    }
    SetUIProperties_Transshipment1() {
        var isLegFieldsEnabled = this.IsEditingEnabled;

        if (isLegFieldsEnabled) {
            if (AppTool.IsNullOrEmpty(this.Transshipment1CarrierId)) {
                isLegFieldsEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("Transshipment1CarrierId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1AdditionalMAWBOBLBL", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1VesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ATA", this.ObjectTableName, this.IsEditingEnabled);
    }
    SetUIProperties_Transshipment2() {
        var isLegFieldsEnabled = this.IsEditingEnabled;

        if (isLegFieldsEnabled) {
            if (AppTool.IsNullOrEmpty(this.Transshipment2CarrierId)) {
                isLegFieldsEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("Transshipment2CarrierId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2AdditionalMAWBOBLBL", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2VesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ATA", this.ObjectTableName, this.IsEditingEnabled);
    }
    SetUIProperties_Transshipment3() {
        var isLegFieldsEnabled = this.IsEditingEnabled;

        if (isLegFieldsEnabled) {
            if (AppTool.IsNullOrEmpty(this.Transshipment3CarrierId)) {
                isLegFieldsEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("Transshipment3CarrierId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3CarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment3CarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment3AdditionalMAWBOBLBL", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment3VesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ATA", this.ObjectTableName, this.IsEditingEnabled);
    }
    SetUIProperties_ValidateActualDates() {

        this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment1ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment1ATA", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment2ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment2ATA", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment3ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment3ATA", this.ObjectTableName, true, null);

        // MainCarriage
        if (!DateTool.IsActualDateValid(this.MainCarriageATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.MainCarriageATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, false, errorMessage);
        }

        //Transshipment1
        if (!DateTool.IsActualDateValid(this.Transshipment1ATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("Transshipment1ATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.Transshipment1ATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("Transshipment1ATA", this.ObjectTableName, false, errorMessage);
        }

        //Transshipment2
        if (!DateTool.IsActualDateValid(this.Transshipment2ATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("Transshipment2ATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.Transshipment2ATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("Transshipment2ATA", this.ObjectTableName, false, errorMessage);
        }

        //Transshipment3
        if (!DateTool.IsActualDateValid(this.Transshipment3ATD)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("Transshipment3ATD", this.ObjectTableName, false, errorMessage);
        }

        if (!DateTool.IsActualDateValid(this.Transshipment3ATA)) {
            var errorMessage = DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("Transshipment3ATA", this.ObjectTableName, false, errorMessage);
        }
    }

    public IsFromStockVisible: boolean = false;
    public IsFromStockEnabled: boolean = false;
    public IsReturnStockVisible: boolean = false;
    public IsReturnStockEnabled: boolean = false;
    public IsAddInterlineEnabled: boolean = false;
    SetUIProperties_Interline() {
        var isAddInterlineEnabled = this.IsEditingEnabled;
        if (isAddInterlineEnabled) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isAddInterlineEnabled = false;
            }

            else if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) && this.MainCarriageIsFromStack) {
                isAddInterlineEnabled = false;
            }

            else if (!AppTool.IsNullOrEmpty(this.Master)) {
                isAddInterlineEnabled = false;
            }
        }

        this.IsAddInterlineEnabled = isAddInterlineEnabled;
    }
    SetUIProperties_StockButton() {

        var isFromStockVisible = false;
        var isReturnStockVisible = false;
        var isFromStockEnabled = this.IsEditingEnabled;
        var isReturnStockEnabled = this.IsEditingEnabled;

        if (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) {
            isReturnStockVisible = true;
        }

        isFromStockVisible = !isReturnStockVisible;

        if (this.IsEditingEnabled) {


            if (this.MainCarriageCarrierId == null && this.InterlineId == null) {
                isFromStockEnabled = false;
            }

            else if (!AppTool.IsNullOrEmpty(this.Master)) {
                isFromStockEnabled = false;
            }

            else if (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) {
                isFromStockEnabled = false;
            }

            else if (this.EntityPM.BookingId != null) {
                isFromStockEnabled = false;
                isReturnStockEnabled = false;
            }
        }

        this.IsFromStockVisible = isFromStockVisible;
        this.IsFromStockEnabled = isFromStockEnabled;
        this.IsReturnStockVisible = isReturnStockVisible;
        this.IsReturnStockEnabled = isReturnStockEnabled;
    }

    // Ports
    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(value: string) {
        if (this.EntityPM.MainCarriageFromPortId != value) {
            this.EntityPM.MainCarriageFromPortId = value;

            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get Transshipment1FromPortId() { return this.EntityPM.Transshipment1FromPortId; }
    set Transshipment1FromPortId(value: string) {
        if (this.EntityPM.Transshipment1FromPortId != value) {
            this.EntityPM.Transshipment1FromPortId = value;

            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.Transshipment1FromPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.Transshipment1FromPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.Transshipment1FromPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get Transshipment2FromPortId() { return this.EntityPM.Transshipment2FromPortId; }
    set Transshipment2FromPortId(value: string) {
        if (this.EntityPM.Transshipment2FromPortId != value) {
            this.EntityPM.Transshipment2FromPortId = value;

            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.Transshipment2FromPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.Transshipment2FromPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.Transshipment2FromPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }
   
    get Transshipment3FromPortId() { return this.EntityPM.Transshipment3FromPortId; }
    set Transshipment3FromPortId(value: string) {
        if (this.EntityPM.Transshipment3FromPortId != value) {
            this.EntityPM.Transshipment3FromPortId = value;

            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.Transshipment3FromPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.Transshipment3FromPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.Transshipment3FromPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    get MainCarriageFinalDestinationPortId() { return this.EntityPM.MainCarriageFinalDestinationPortId; }
    set MainCarriageFinalDestinationPortId(value: string) {
        if (this.EntityPM.MainCarriageFinalDestinationPortId != value) {
            this.EntityPM.MainCarriageFinalDestinationPortId = value;

            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.FinalDestinationPortChanged(this.EntityPM, list);
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.FinalDestinationPortChanged(this.EntityPM, list);
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    // Main Carrier
    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(value: string) {
        if (this.EntityPM.MainCarriageCarrierId != value) {
            this.EntityPM.MainCarriageCarrierId = value;

            this.SetUIProperties_MainCarriage();

            if (AppTool.IsNullOrEmpty(value)) {
                this.MainCarriageCarrierPrefix = null;
                this.MainCarriageCarrierNumber = null; 

                RoutingHelper.MainCarriageCarrierChanged(this.EntityPM, null);

                if (this.TransportModeId == "A") {
                    if (AppTool.IsNullOrEmpty(this.InterlineId)) {

                        this.CarrierIsCheckDigit = false;
                        this.CarrierIsLimitedLength = false;
                        this.AirlinePrefix = null;
                    }

                    this.Master = null;
                    this.CarrierIsChampRegistered = false;
                    this.CarrierIsGLSHKRegistered = false;
                    ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
                }
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            if (this.TransportModeId == "A") {
                                this.MainCarriageCarrierPrefix = list.Code;
                            }

                            RoutingHelper.MainCarriageCarrierChanged(this.EntityPM, list);

                            if (this.TransportModeId == "A") {

                                // dont get from chach: if user choosed from tenant0 it wont get it
                                this.myAirlineListService.getSingle(value).subscribe((myAirlineListResponse: ServiceResponse) => {
                                    if (myAirlineListResponse != null) {

                                        var myAirlineList: AirlineList = myAirlineListResponse.Result;
                                        if (myAirlineList != null) {
                                            if (AppTool.IsNullOrEmpty(this.InterlineId)) {

                                                this.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                                this.CarrierIsLimitedLength = myAirlineList.LimitedLength;

                                                var myPrefix: string = null;
                                                if (!AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                                    myPrefix = myAirlineList.Prefix.toString().trim();
                                                    myPrefix = AppTool.PadLeft(myPrefix, 3, '0');
                                                }

                                                this.AirlinePrefix = myPrefix;
                                            }

                                            this.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                            this.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;

                                            this.myPartnersDomainService.GetAirlineByCode(myAirlineList.Code, 0).subscribe((myResponse: ServiceResponse) => {
                                                if (!myResponse.HasError) {
                                                    ShipmentTool.MapTenantZeroAirline(this.EntityPM, myResponse.Result);
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                        }
                    }
                });
            }
        }
    }

    get MainCarriageCarrierCode() { return this.EntityPM.MainCarriageCarrierCode; }
    set MainCarriageCarrierCode(value: string) {
        if (this.EntityPM.MainCarriageCarrierCode != value) {
            this.EntityPM.MainCarriageCarrierCode = value;
        }
    }

    get MainCarriageCarrierName() { return this.EntityPM.MainCarriageCarrierName; }
    set MainCarriageCarrierName(value: string) {
        if (this.EntityPM.MainCarriageCarrierName != value) {
            this.EntityPM.MainCarriageCarrierName = value;
        }
    }

    get MainCarriageCarrierPrefix() { return this.EntityPM.MainCarriageCarrierPrefix; }
    set MainCarriageCarrierPrefix(value: string) {
        if (this.EntityPM.MainCarriageCarrierPrefix != value) {
            this.EntityPM.MainCarriageCarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get MainCarriageCarrierWebSite() { return this.EntityPM.MainCarriageCarrierWebSite; }
    set MainCarriageCarrierWebSite(value: string) {
        if (this.EntityPM.MainCarriageCarrierWebSite != value) {
            this.EntityPM.MainCarriageCarrierWebSite = value;
        }
    }

    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    set MainCarriageCarrierNumber(value: string) {
        if (this.EntityPM.MainCarriageCarrierNumber != value) {
            this.EntityPM.MainCarriageCarrierNumber = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get CarrierIsCheckDigit() { return this.EntityPM.CarrierIsCheckDigit; }
    set CarrierIsCheckDigit(value: boolean) {
        if (this.EntityPM.CarrierIsCheckDigit != value) {
            this.EntityPM.CarrierIsCheckDigit = value;
        }
    }

    get CarrierIsLimitedLength() { return this.EntityPM.CarrierIsLimitedLength; }
    set CarrierIsLimitedLength(value: boolean) {
        if (this.EntityPM.CarrierIsLimitedLength != value) {
            this.EntityPM.CarrierIsLimitedLength = value;
        }
    }

    get CarrierIsChampRegistered() { return this.EntityPM.CarrierIsChampRegistered; }
    set CarrierIsChampRegistered(value: boolean) {
        if (this.EntityPM.CarrierIsChampRegistered != value) {
            this.EntityPM.CarrierIsChampRegistered = value;
        }
    }

    get CarrierIsGLSHKRegistered() { return this.EntityPM.CarrierIsGLSHKRegistered; }
    set CarrierIsGLSHKRegistered(value: boolean) {
        if (this.EntityPM.CarrierIsGLSHKRegistered != value) {
            this.EntityPM.CarrierIsGLSHKRegistered = value;
        }
    }

    // Carrier
    get Transshipment1CarrierId() { return this.EntityPM.Transshipment1CarrierId; }
    set Transshipment1CarrierId(value: string) {
        if (this.EntityPM.Transshipment1CarrierId != value) {
            this.EntityPM.Transshipment1CarrierId = value;

            this.SetUIProperties_Transshipment1();

            if (AppTool.IsNullOrEmpty(value)) {
                this.Transshipment1CarrierPrefix = null;
                this.Transshipment1CarrierNumber = null;
                this.Transshipment1AdditionalMAWBOBLBL = null;
                RoutingHelper.Transshipment1CarrierChanged(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            if (this.TransportModeId == "A") {
                                this.Transshipment1CarrierPrefix = list.Code;
                            }

                            RoutingHelper.Transshipment1CarrierChanged(this.EntityPM, list);
                        }
                    }
                });
            }
        }
    }

    get Transshipment2CarrierId() { return this.EntityPM.Transshipment2CarrierId; }
    set Transshipment2CarrierId(value: string) {
        if (this.EntityPM.Transshipment2CarrierId != value) {
            this.EntityPM.Transshipment2CarrierId = value;

            this.SetUIProperties_Transshipment2();

            if (AppTool.IsNullOrEmpty(value)) {
                this.Transshipment2CarrierPrefix = null;
                this.Transshipment2CarrierNumber = null;
                this.Transshipment2AdditionalMAWBOBLBL = null;
                RoutingHelper.Transshipment2CarrierChanged(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            if (this.TransportModeId == "A") {
                                this.Transshipment2CarrierPrefix = list.Code;
                            }

                            RoutingHelper.Transshipment2CarrierChanged(this.EntityPM, list);
                        }
                    }
                });
            }
        }
    }

    get Transshipment3CarrierId() { return this.EntityPM.Transshipment3CarrierId; }
    set Transshipment3CarrierId(value: string) {
        if (this.EntityPM.Transshipment3CarrierId != value) {
            this.EntityPM.Transshipment3CarrierId = value;

            this.SetUIProperties_Transshipment3();

            if (AppTool.IsNullOrEmpty(value)) {
                this.Transshipment3CarrierPrefix = null;
                this.Transshipment3CarrierNumber = null;
                this.Transshipment3AdditionalMAWBOBLBL = null;
                RoutingHelper.Transshipment3CarrierChanged(this.EntityPM, null);
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {

                            if (this.TransportModeId == "A") {
                                this.Transshipment3CarrierPrefix = list.Code;
                            }

                            RoutingHelper.Transshipment3CarrierChanged(this.EntityPM, list);
                        }
                    }
                });
            }
        }
    }

    // Carrier Prefix
    get Transshipment1CarrierPrefix() { return this.EntityPM.Transshipment1CarrierPrefix; }
    set Transshipment1CarrierPrefix(value: string) {
        if (this.EntityPM.Transshipment1CarrierPrefix != value) {
            this.EntityPM.Transshipment1CarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get Transshipment2CarrierPrefix() { return this.EntityPM.Transshipment2CarrierPrefix; }
    set Transshipment2CarrierPrefix(value: string) {
        if (this.EntityPM.Transshipment2CarrierPrefix != value) {
            this.EntityPM.Transshipment2CarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get Transshipment3CarrierPrefix() { return this.EntityPM.Transshipment3CarrierPrefix; }
    set Transshipment3CarrierPrefix(value: string) {
        if (this.EntityPM.Transshipment3CarrierPrefix != value) {
            this.EntityPM.Transshipment3CarrierPrefix = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    // Carrier Number
    get Transshipment1CarrierNumber() { return this.EntityPM.Transshipment1CarrierNumber; }
    set Transshipment1CarrierNumber(value: string) {
        if (this.EntityPM.Transshipment1CarrierNumber != value) {
            this.EntityPM.Transshipment1CarrierNumber = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get Transshipment2CarrierNumber() { return this.EntityPM.Transshipment2CarrierNumber; }
    set Transshipment2CarrierNumber(value: string) {
        if (this.EntityPM.Transshipment2CarrierNumber != value) {
            this.EntityPM.Transshipment2CarrierNumber = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    get Transshipment3CarrierNumber() { return this.EntityPM.Transshipment3CarrierNumber; }
    set Transshipment3CarrierNumber(value: string) {
        if (this.EntityPM.Transshipment3CarrierNumber != value) {
            this.EntityPM.Transshipment3CarrierNumber = AppTool.IsNullOrEmpty(value) ? value : value.trim();
        }
    }

    // Master   
    get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    set AirlinePrefix(value: string) {
        if (this.EntityPM.AirlinePrefix != value) {
            this.EntityPM.AirlinePrefix = value;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(value: string) {
        if (this.EntityPM.Master != value) {
            this.EntityPM.Master = value;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get TrailerNumber() { return this.EntityPM.TrailerNumber; }
    set TrailerNumber(value: string) {
        if (this.EntityPM.TrailerNumber != value) {
            this.EntityPM.TrailerNumber = value;

        }

    }

    get LongMaster() { return this.EntityPM.LongMaster; }
    set LongMaster(newValue: string) {
        if (this.EntityPM.LongMaster != newValue) {
            this.EntityPM.LongMaster = newValue;
            this.ValidateMasterField();
            this.SetUIProperties_MainCarriage();
        }
    }

    get MAWBOBLDate() { return this.EntityPM.MAWBOBLDate; }
    set MAWBOBLDate(value: Date) {
        if (this.EntityPM.MAWBOBLDate != value) {
            this.EntityPM.MAWBOBLDate = value;
        }
    }

    get CutoffDate() { return this.EntityPM.CutoffDate; }
    set CutoffDate(value: Date) {
        if (this.EntityPM.CutoffDate != value) {
            this.EntityPM.CutoffDate = value;
        }
    }

    get Transshipment1AdditionalMAWBOBLBL() { return this.EntityPM.Transshipment1AdditionalMAWBOBLBL; }
    set Transshipment1AdditionalMAWBOBLBL(value: string) {
        if (this.EntityPM.Transshipment1AdditionalMAWBOBLBL != value) {
            this.EntityPM.Transshipment1AdditionalMAWBOBLBL = value;
        }
    }

    get Transshipment2AdditionalMAWBOBLBL() { return this.EntityPM.Transshipment2AdditionalMAWBOBLBL; }
    set Transshipment2AdditionalMAWBOBLBL(value: string) {
        if (this.EntityPM.Transshipment2AdditionalMAWBOBLBL != value) {
            this.EntityPM.Transshipment2AdditionalMAWBOBLBL = value;
        }
    }

    get Transshipment3AdditionalMAWBOBLBL() { return this.EntityPM.Transshipment3AdditionalMAWBOBLBL; }
    set Transshipment3AdditionalMAWBOBLBL(value: string) {
        if (this.EntityPM.Transshipment3AdditionalMAWBOBLBL != value) {
            this.EntityPM.Transshipment3AdditionalMAWBOBLBL = value;
        }
    }

    get DocumentsClosingDate() { return this.EntityPM.DocumentsClosingDate; }
    set DocumentsClosingDate(value: Date) {
        if (this.EntityPM.DocumentsClosingDate != value) {
            this.EntityPM.DocumentsClosingDate = value;
        }
    }

    get OBLTypeCode() { return this.EntityPM.OBLTypeCode; }
    set OBLTypeCode(newValue: string) {
        if (this.EntityPM.OBLTypeCode != newValue) {
            this.EntityPM.OBLTypeCode = newValue;
        }
    }

    // Vessels    
    get MainCarriageVesselId() { return this.EntityPM.MainCarriageVesselId; }
    set MainCarriageVesselId(value: string) {
        if (this.EntityPM.MainCarriageVesselId != value) {
            this.EntityPM.MainCarriageVesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.MainCarriageVesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.MainCarriageVesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get Transshipment1VesselId() { return this.EntityPM.Transshipment1VesselId; }
    set Transshipment1VesselId(value: string) {
        if (this.EntityPM.Transshipment1VesselId != value) {
            this.EntityPM.Transshipment1VesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.Transshipment1VesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.Transshipment1VesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get Transshipment2VesselId() { return this.EntityPM.Transshipment2VesselId; }
    set Transshipment2VesselId(value: string) {
        if (this.EntityPM.Transshipment2VesselId != value) {
            this.EntityPM.Transshipment2VesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.Transshipment2VesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.Transshipment2VesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    get Transshipment3VesselId() { return this.EntityPM.Transshipment3VesselId; }
    set Transshipment3VesselId(value: string) {
        if (this.EntityPM.Transshipment3VesselId != value) {
            this.EntityPM.Transshipment3VesselId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.EntityPM.Transshipment3VesselName = null;
            }

            else {
                this.myVesselListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: VesselList = myResponse.Result;
                        if (list) {
                            this.EntityPM.Transshipment3VesselName = list.EnglishName;
                        }
                    }
                });
            }
        }
    }

    // Dates
    get MainCarriageETD() { return this.EntityPM.MainCarriageETD; }
    set MainCarriageETD(newValue: Date) {
        if (this.EntityPM.MainCarriageETD != newValue) {
            this.EntityPM.MainCarriageETD = newValue;
            this.SetFlightDate();
        }
    }
    get MainCarriageATD() { return this.EntityPM.MainCarriageATD; }
    set MainCarriageATD(newValue: Date) {
        if (this.EntityPM.MainCarriageATD != newValue) {
            this.EntityPM.MainCarriageATD = newValue;
            this.SetFlightDate();
            this.SetUIProperties_ValidateActualDates();
        }
    }
    get MainCarriageETA() { return this.EntityPM.MainCarriageETA; }
    set MainCarriageETA(newValue: Date) {
        if (this.EntityPM.MainCarriageETA != newValue) {
            this.EntityPM.MainCarriageETA = newValue;
        }
    }
    get MainCarriageATA() { return this.EntityPM.MainCarriageATA; }
    set MainCarriageATA(newValue: Date) {
        if (this.EntityPM.MainCarriageATA != newValue) {
            this.EntityPM.MainCarriageATA = newValue;
            this.SetUIProperties_ValidateActualDates();
        }
    }
    private SetFlightDate() {
        var flightDate = this.MainCarriageETD;
        var isFlightDateActual = false;

        if (this.MainCarriageATD != null) {
            flightDate = this.MainCarriageATD;
            isFlightDateActual = true;
        }

        this.EntityPM.FlightDate = flightDate;
        this.EntityPM.IsFlightDateActual = isFlightDateActual;
    }

    get Transshipment1ETD() { return this.EntityPM.Transshipment1ETD; }
    set Transshipment1ETD(newValue: Date) {
        if (this.EntityPM.Transshipment1ETD != newValue) {
            this.EntityPM.Transshipment1ETD = newValue;
        }
    }
    get Transshipment1ATD() { return this.EntityPM.Transshipment1ATD; }
    set Transshipment1ATD(newValue: Date) {
        if (this.EntityPM.Transshipment1ATD != newValue) {
            this.EntityPM.Transshipment1ATD = newValue;
            this.SetUIProperties_ValidateActualDates();
        }
    }
    get Transshipment1ETA() { return this.EntityPM.Transshipment1ETA; }
    set Transshipment1ETA(newValue: Date) {
        if (this.EntityPM.Transshipment1ETA != newValue) {
            this.EntityPM.Transshipment1ETA = newValue;
        }
    }
    get Transshipment1ATA() { return this.EntityPM.Transshipment1ATA; }
    set Transshipment1ATA(newValue: Date) {
        if (this.EntityPM.Transshipment1ATA != newValue) {
            this.EntityPM.Transshipment1ATA = newValue;
            this.SetUIProperties_ValidateActualDates();
        }
    }

    get Transshipment2ETD() { return this.EntityPM.Transshipment2ETD; }
    set Transshipment2ETD(newValue: Date) {
        if (this.EntityPM.Transshipment2ETD != newValue) {
            this.EntityPM.Transshipment2ETD = newValue;
        }
    }
    get Transshipment2ATD() { return this.EntityPM.Transshipment2ATD; }
    set Transshipment2ATD(newValue: Date) {
        if (this.EntityPM.Transshipment2ATD != newValue) {
            this.EntityPM.Transshipment2ATD = newValue;
            this.SetUIProperties_ValidateActualDates();
        }
    }
    get Transshipment2ETA() { return this.EntityPM.Transshipment2ETA; }
    set Transshipment2ETA(newValue: Date) {
        if (this.EntityPM.Transshipment2ETA != newValue) {
            this.EntityPM.Transshipment2ETA = newValue;
        }
    }
    get Transshipment2ATA() { return this.EntityPM.Transshipment2ATA; }
    set Transshipment2ATA(newValue: Date) {
        if (this.EntityPM.Transshipment2ATA != newValue) {
            this.EntityPM.Transshipment2ATA = newValue;
            this.SetUIProperties_ValidateActualDates();
        }
    }

    get Transshipment3ETD() { return this.EntityPM.Transshipment3ETD; }
    set Transshipment3ETD(newValue: Date) {
        if (this.EntityPM.Transshipment3ETD != newValue) {
            this.EntityPM.Transshipment3ETD = newValue;
        }
    }
    get Transshipment3ATD() { return this.EntityPM.Transshipment3ATD; }
    set Transshipment3ATD(newValue: Date) {
        if (this.EntityPM.Transshipment3ATD != newValue) {
            this.EntityPM.Transshipment3ATD = newValue;
            this.SetUIProperties_ValidateActualDates();
        }
    }
    get Transshipment3ETA() { return this.EntityPM.Transshipment3ETA; }
    set Transshipment3ETA(newValue: Date) {
        if (this.EntityPM.Transshipment3ETA != newValue) {
            this.EntityPM.Transshipment3ETA = newValue;
        }
    }
    get Transshipment3ATA() { return this.EntityPM.Transshipment3ATA; }
    set Transshipment3ATA(newValue: Date) {
        if (this.EntityPM.Transshipment3ATA != newValue) {
            this.EntityPM.Transshipment3ATA = newValue;
            this.SetUIProperties_ValidateActualDates();
        }
    }

    private ValidateActualDates(errors: string[]) {

        // MainCarriage
        if (!DateTool.IsActualDateValid(this.MainCarriageATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg0TextCodeLabel + " " + this.ATDLabel));
        }

        if (!DateTool.IsActualDateValid(this.MainCarriageATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg0TextCodeLabel + " " + this.ATALabel));
        }

        //Transshipment1
        if (!DateTool.IsActualDateValid(this.Transshipment1ATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg1TextCodeLabel + " " + this.ATDLabel));
        }

        if (!DateTool.IsActualDateValid(this.Transshipment1ATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg1TextCodeLabel + " " + this.ATALabel));
        }

        //Transshipment2
        if (!DateTool.IsActualDateValid(this.Transshipment2ATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg2TextCodeLabel + " " + this.ATDLabel));
        }

        if (!DateTool.IsActualDateValid(this.Transshipment2ATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg2TextCodeLabel + " " + this.ATALabel));
        }

        //Transshipment3
        if (!DateTool.IsActualDateValid(this.Transshipment3ATD)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg3TextCodeLabel + " " + this.ATDLabel));
        }

        if (!DateTool.IsActualDateValid(this.Transshipment3ATA)) {
            errors.push(DateTool.ActualDateMessage.replace("Field", this.Leg3TextCodeLabel + " " + this.ATALabel));
        }
    }

    SetActualDateClicked(fieldName: string) {
        switch (fieldName) {
            case "MainCarriageETD": { this.MainCarriageATD = DateTool.GetDateParts(this.MainCarriageETD).DateObject; break; }
            case "MainCarriageETA": { this.MainCarriageATA = DateTool.GetDateParts(this.MainCarriageETA).DateObject; break; }
            case "Transshipment1ETD": { this.Transshipment1ATD = DateTool.GetDateParts(this.Transshipment1ETD).DateObject; break; }
            case "Transshipment1ETA": { this.Transshipment1ATA = DateTool.GetDateParts(this.Transshipment1ETA).DateObject; break; }
            case "Transshipment2ETD": { this.Transshipment2ATD = DateTool.GetDateParts(this.Transshipment2ETD).DateObject; break; }
            case "Transshipment2ETA": { this.Transshipment2ATA = DateTool.GetDateParts(this.Transshipment2ETA).DateObject; break; }
            case "Transshipment3ETD": { this.Transshipment3ATD = DateTool.GetDateParts(this.Transshipment3ETD).DateObject; break; }
            case "Transshipment3ETA": { this.Transshipment3ATA = DateTool.GetDateParts(this.Transshipment3ETA).DateObject; break; }
        }
    }

    // Interline
    private isInterlineAdded: boolean = false;
    get IsInterlineAdded() { return this.isInterlineAdded; }
    set IsInterlineAdded(value: boolean) {
        if (this.isInterlineAdded != value) {
            this.isInterlineAdded = value;
        }
    }

    get InterlineId() { return this.EntityPM.InterlineId; }
    set InterlineId(newValue: string) {
        if (this.EntityPM.InterlineId != newValue) {
            this.EntityPM.InterlineId = newValue;
            this.SetUIProperties_MainCarriage();
            this.OnInterlineChanged();
        }
    }

    AddInterlineClicked() {
        this.IsInterlineAdded = true;
    }
    OnInterlineLostFocus($event) {        
        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
    }
    OnInterlineChanged() {
        var myAirlineId: string = this.InterlineId;
        if (AppTool.IsNullOrEmpty(myAirlineId)) {
            myAirlineId = this.MainCarriageCarrierId;
        }

        if (AppTool.IsNullOrEmpty(myAirlineId)) {
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;
            this.AirlinePrefix = null;
        }

        else {
            this.myAirlineListService.getSingleFromCache(myAirlineId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: AirlineList = myResponse.Result;

                    if (list != null) {

                        this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                        this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;

                        var myPrefix: string = null;
                        if (!AppTool.IsNullOrEmpty(list.Prefix)) {
                            myPrefix = list.Prefix.toString().trim();
                            myPrefix = AppTool.PadLeft(myPrefix, 3, '0');
                        }

                        this.AirlinePrefix = myPrefix;
                    }
                }
            });
        }       
    }

    // Stock
    get MAWBStackNumber() { return this.EntityPM.MAWBStackNumber; }
    set MAWBStackNumber(newValue: string) {
        if (this.EntityPM.MAWBStackNumber != newValue) {
            this.EntityPM.MAWBStackNumber = newValue;
        }
    }

    get MAWBTakenFromStack() { return this.EntityPM.MAWBTakenFromStack; }
    set MAWBTakenFromStack(newValue: boolean) {
        if (this.EntityPM.MAWBTakenFromStack != newValue) {
            this.EntityPM.MAWBTakenFromStack = newValue;
        }
    }

    get MainCarriageIsFromStack() { return this.EntityPM.MainCarriageIsFromStack; }
    set MainCarriageIsFromStack(newValue: boolean) {
        if (this.EntityPM.MainCarriageIsFromStack != newValue) {
            this.EntityPM.MainCarriageIsFromStack = newValue;
        }
    }

    get MAWBReturnedToStack() { return this.EntityPM.MAWBReturnedToStack; }
    set MAWBReturnedToStack(newValue: boolean) {
        if (this.EntityPM.MAWBReturnedToStack != newValue) {
            this.EntityPM.MAWBReturnedToStack = newValue;
        }
    }

    private isGetFromStock: boolean = false;
    private myOldMAWBStackNumber: string;
    private myOldMAWBOBLDate: Date;
    private SaveCompletedEvent: any = null;
    AddStockClicked(airlineId: string) {
        if (!AppTool.IsNullOrEmpty(airlineId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit Airline";
            logWindow.ShowEditComponent(airlineId, "Airline", "ALST");
        }
    }
    GetStockClicked() {
        if (this.CurrentSession.CurrentEditComponent) {
            var myShipmentValidator = new ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.EntityPM);

            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myShipmentErrors;

            if (myShipmentErrors.length == 0) {
                this.SetMAWBAirline();

                var windowArgs = new GetStackWindowArgs();
                windowArgs.CardId = this.EntityPM.MAWBStackAirlineId;
                windowArgs.ShipperId = this.EntityPM.ShipperId;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 750;
                logWindow.Height = 450;
                logWindow.WindowArgs = windowArgs;
                logWindow.Title = "Select Air Waybill Number";
                logWindow.Show('./Common/Components/Partners/AWBStock/StackSelectionComponent');

                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        if (windowArgs.SelectedStack != null) {

                            var stackNumber: number = windowArgs.SelectedStack.Number;
                            this.myOldMAWBStackNumber = this.MAWBStackNumber;
                            this.myOldMAWBOBLDate = this.MAWBOBLDate;

                            this.MAWBTakenFromStack = true;
                            this.MAWBStackNumber = AppTool.PadLeft(stackNumber.toString(), 8, '0');
                            this.MAWBOBLDate = DateTool.GetCurrentDateTimeAsUtc();
                            this.isGetFromStock = true;

                            if (!this.SaveCompletedEvent) {
                                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                    if (isSaveSuccess) {
                                        this.EntityPM = null;
                                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                                        this.SetUIProperties_MainCarriage();
                                        this.myCloner.AddField('Master');
                                        this.myCloner.AddField('MAWBOBLDate');
                                        this.myCloner.AddField('MAWBStackNumber');
                                        this.myCloner.AddField('MAWBTakenFromStack');
                                        this.myCloner.AddField('MAWBReturnedToStack');
                                        this.myCloner.AddField('MainCarriageIsFromStack');
                                    }

                                    else {
                                        this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                                    }

                                    AppTool.KillEventEmitter(this.SaveCompletedEvent);
                                    this.SaveCompletedEvent = null;
                                });

                                this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                        }
                    }
                });
            }
        }
    }
    ReturnStockClicked() {
        if (this.CurrentSession.CurrentEditComponent) {
            if (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) {
                var myShipmentValidator = new ShipmentValidator();
                var myShipmentErrors = myShipmentValidator.Validate(this.EntityPM);

                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myShipmentErrors;

                if (myShipmentErrors.length == 0) {
                    this.SetMAWBAirline();
                    this.MAWBReturnedToStack = true;
                    this.MAWBStackNumber = this.Master;
                    this.isGetFromStock = false;

                    if (!this.SaveCompletedEvent) {
                        this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                            if (isSaveSuccess) {
                                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                                this.SetUIProperties_MainCarriage();
                                this.myCloner.AddField('Master');
                                this.myCloner.AddField('MAWBOBLDate');
                                this.myCloner.AddField('MAWBStackNumber');
                                this.myCloner.AddField('MAWBTakenFromStack');
                                this.myCloner.AddField('MAWBReturnedToStack');
                                this.myCloner.AddField('MainCarriageIsFromStack');
                            }

                            else {
                                this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                            }

                            AppTool.KillEventEmitter(this.SaveCompletedEvent);
                            this.SaveCompletedEvent = null;
                        });

                        this.CurrentSession.CurrentEditComponent.SaveChanges();
                    }
                }
            }
        }
    }
    SetMAWBAirline() {
        if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.MainCarriageCarrierId) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.InterlineId) {
                this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
            }
        }
    }

    // Validate Master
    private MasterFieldValidityMessage: string = null;
    private myShipmentDomainService: ShipmentDomainService;
    private ValidateMasterField() {
        if (this.TransportModeId == "A") {
            this.MasterFieldValidityMessage = null;

            if (AppTool.IsNullOrEmpty(this.Master)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
            }

            else {
                var myResult: string = AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);

                if (!AppTool.IsNullOrEmpty(myResult)) {
                    this.MasterFieldValidityMessage = myResult;
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
                }

                else {

                    this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");

                    if (!AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
                        this.ValidateMasterFieldIsUsed();
                    }
                }
            }
        }
    }
    private ValidateMasterFieldIsUsed() {
        if (this.TransportModeId == "A") {
            if (this.myShipmentDomainService == null) {
                this.myShipmentDomainService = new ShipmentDomainService();
            }

            if (!AppTool.IsNullOrEmpty(this.Master)) {
                this.myShipmentDomainService.ValidateShipmentMasterFieldExistance(this.EntityPM)
                    .subscribe((myResult: any) => {

                        if (AppTool.IsNullOrEmpty(myResult)) {
                            this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
                        }

                        else {
                            this.MasterFieldValidityMessage = myResult;
                            this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
                        }
                    });
            }
        }
    }
    private ValidateMasterStack() {
        if (this.TransportModeId == "A") {
            if (this.EntityPM != null) {
                if (!AppTool.IsNullOrEmpty(this.Master)) {
                    if (this.Master.length == 8 && !this.MainCarriageIsFromStack && !this.MAWBTakenFromStack) {
                        if (FormatTool.IsNumeric(this.Master)) {
                            this.StackDomainService.GetMAWBStackPMByNumber(+this.Master).subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    var myStackPM: MAWBStackPM = myResponse.Result;

                                    if (myStackPM != null) {
                                        var myAirlineId: string = this.MainCarriageCarrierId;
                                        if (!AppTool.IsNullOrEmpty(this.InterlineId)) {
                                            myAirlineId = this.InterlineId;
                                        }

                                        if (myStackPM.AirlineId == myAirlineId) {
                                            if (!AppTool.IsNullOrEmpty(myStackPM.AssignedToId) && myStackPM.AssignedToId != this.EntityPM.ShipperId) {
                                                var messageWindow = new MessageWindow();
                                                messageWindow.Width = 450;
                                                messageWindow.Height = 190;
                                                messageWindow.Title = "Invalid Master";
                                                messageWindow.Show("AWB is assigned to another shipper");
                                                this.Master = null;
                                            }

                                            else {
                                                var confirmWindow = new ConfirmWindow();
                                                confirmWindow.Title = "AWB exists in the stock";
                                                confirmWindow.Show("Do you want to get this awb from stock?");
                                                confirmWindow.WindowClosed.subscribe((event: any) => {
                                                    if (confirmWindow.Yes) {
                                                        this.MAWBTakenFromStack = true;
                                                        this.MAWBStackNumber = AppTool.PadLeft(myStackPM.Number.toString(), 8, '0');
                                                        this.MAWBOBLDate = DateTool.GetCurrentDateTimeAsUtc();
                                                        this.SetMAWBAirline();
                                                        this.isGetFromStock = true;

                                                        if (!this.SaveCompletedEvent) {
                                                            this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                                                if (isSaveSuccess) {
                                                                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                                                                    this.SetUIProperties_MainCarriage();
                                                                    this.myCloner.AddField('Master');
                                                                    this.myCloner.AddField('MAWBOBLDate');
                                                                    this.myCloner.AddField('MAWBStackNumber');
                                                                    this.myCloner.AddField('MAWBTakenFromStack');
                                                                    this.myCloner.AddField('MAWBReturnedToStack');
                                                                    this.myCloner.AddField('MainCarriageIsFromStack');
                                                                }

                                                                else {
                                                                    this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                                                                }

                                                                AppTool.KillEventEmitter(this.SaveCompletedEvent);
                                                                this.SaveCompletedEvent = null;
                                                            });

                                                            this.CurrentSession.CurrentEditComponent.SaveChanges();
                                                        }
                                                    }

                                                    else {
                                                        this.Master = null;
                                                    }
                                                });
                                            }
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    }
    MasterLostFocus(input: any) {
        if (this.TransportModeId == "A") {
            this.ValidateMasterStack();
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.IsContainersToggleFeatureUp && this.FatherComponent.IsFCLEntity && !AppTool.IsNullOrEmpty(this.Master) && AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            errors.push("You cannot fill the OBL without a Shipping Line. Please select a Shipping Line");
        }

        if (AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", this.FromTextCodeLabel));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", this.ToTextCodeLabel));
        }

        if (!AppTool.IsNullOrEmpty(this.Transshipment3FromPortId)) {
            if (AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
                errors.push("To Add " + this.Via3Label + " you need to add " + this.Via2Label);
            }
        }

        if (!AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
            if (AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
                errors.push("To Add " + this.Via2Label + " you need to add " + this.Via1Label);
            }
        }

        if (this.TransportModeId == "A") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierPrefix)) {
                    if (this.EntityPM.MainCarriageCarrierPrefix.length != 2) {
                        errors.push("Main Carriage Carrier Code must be 2 characters");
                    }
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierPrefix)) {
                    if (this.EntityPM.Transshipment1CarrierPrefix.length != 2) {
                        errors.push("Transshipment1 Carrier Code must be 2 characters");
                    }
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierPrefix)) {
                    if (this.EntityPM.Transshipment2CarrierPrefix.length != 2) {
                        errors.push("Transshipment2 Carrier Code must be 2 characters");
                    }
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3CarrierId)) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3CarrierPrefix)) {
                    if (this.EntityPM.Transshipment3CarrierPrefix.length != 2) {
                        errors.push("Transshipment3 Carrier Code must be 2 characters");
                    }
                }
            }
        }

        else {
            this.MainCarriageCarrierPrefix = null;
            this.Transshipment1CarrierPrefix = null;
            this.Transshipment2CarrierPrefix = null;
            this.Transshipment3CarrierPrefix = null;
        }

        if (!AppTool.IsNullOrEmpty(this.MasterFieldValidityMessage)) {
            errors.push(this.MasterFieldValidityMessage);
        }

        // Series Dates
        RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "MainCarriage");

        // Actual Dates
        this.ValidateActualDates(errors);

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if (AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
                RoutingHelper.RemoveTransshipment1Leg(this.EntityPM);
            }

            if (AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
                RoutingHelper.RemoveTransshipment2Leg(this.EntityPM);
            }

            if (AppTool.IsNullOrEmpty(this.Transshipment3FromPortId)) {
                RoutingHelper.RemoveTransshipment3Leg(this.EntityPM);
            }

            var isConfirmingPorts: boolean = false;
            var confirmationMessage: string = null
            if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                if (this.EntityPM.OriginMainCarriageFromPortId != this.EntityPM.MainCarriageFromPortId) {
                    isConfirmingPorts = true;
                }

                else if (this.EntityPM.OriginFinalDestinationPortId != this.EntityPM.MainCarriageFinalDestinationPortId) {
                    isConfirmingPorts = true;
                }

                if (isConfirmingPorts) {
                    confirmationMessage = "Updating the Master shipment ports will update the house shipment accordingly";
                }
            }

            var updateProductItems: boolean = false
            if (this.oldCountryId != this.EntityPM.ToCountryId) {
                if (this.EntityPM.ShipmentProductItems.length > 0) {
                    if (!ShipmentTool.IsShipmentProductItemsEmpty(this.EntityPM.ShipmentProductItems)) {
                        updateProductItems = true;
                        isConfirmingPorts = true;

                        if (AppTool.IsNullOrEmpty(confirmationMessage)) {
                            confirmationMessage = "All product items in this shipment will be updated";
                        }

                        else {
                            confirmationMessage = confirmationMessage + ", " + "All product items in this shipment will be updated";
                        }
                    }
                }
            }

            if (isConfirmingPorts) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Title = "Ports Changed";
                confirmWindow.Show(confirmationMessage);
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        if (updateProductItems) {
                            this.EntityPM.IsProductItemsUpdated = true;
                        }

                        if (!this.SaveCompletedEvent) {
                            this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {

                                AppTool.KillEventEmitter(this.SaveCompletedEvent);
                                this.SaveCompletedEvent = null;

                                if (isSaveSuccess) {
                                    this.CurrentSession.SessionEvent.emit("ReloadHouses");

                                    if (updateProductItems) {
                                        this.CurrentSession.FireEvent("ShipmentProductItemsUpdated");
                                    }

                                    this.CloseOk();
                                }

                                else {
                                    this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                                }
                            });

                            this.CurrentSession.CurrentEditComponent.SaveChanges();
                        }
                    }
                });
            }

            else {
                this.CloseOk();
            }
        }
    }


    private CloseOk() {
        this.FatherComponent.BuildItemsCollection();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
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
            oldItem.Notes = item.Notes;
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

        // Left Side
        this.myCloner.AddField('MainCarriageFromPortId');
        this.myCloner.AddField('Transshipment1FromPortId');
        this.myCloner.AddField('Transshipment2FromPortId');
        this.myCloner.AddField('Transshipment3FromPortId');
        this.myCloner.AddField('MainCarriageFinalDestinationPortId');

        // Main Carriage
        this.myCloner.AddField('Master');
        this.myCloner.AddField('MAWBOBLDate');
        this.myCloner.AddField('MAWBStackNumber');
        this.myCloner.AddField('MAWBTakenFromStack');
        this.myCloner.AddField('MAWBReturnedToStack');
        this.myCloner.AddField('MainCarriageIsFromStack');
        this.myCloner.AddField('MainCarriageCarrierCode');
        this.myCloner.AddField('MainCarriageCarrierName');
        this.myCloner.AddField('MainCarriageCarrierPrefix');
        this.myCloner.AddField('MainCarriageCarrierNumber');
        this.myCloner.AddField('MainCarriageCarrierWebSite');
        this.myCloner.AddField('MainCarriageCarrierId');
        this.myCloner.AddField('InterlineId');
        this.myCloner.AddField('AirlinePrefix');
        this.myCloner.AddField('CutoffDate');
        this.myCloner.AddField('MainCarriageVesselId');
        this.myCloner.AddField('MainCarriageETD');
        this.myCloner.AddField('MainCarriageETA');
        this.myCloner.AddField('MainCarriageATD');
        this.myCloner.AddField('MainCarriageATA');

        // Transshipment1
        this.myCloner.AddField('Transshipment1CarrierId');
        this.myCloner.AddField('Transshipment1CarrierPrefix');
        this.myCloner.AddField('Transshipment1CarrierNumber');
        this.myCloner.AddField('Transshipment1AdditionalMAWBOBLBL');
        this.myCloner.AddField('Transshipment1VesselId');
        this.myCloner.AddField('Transshipment1ETD');
        this.myCloner.AddField('Transshipment1ETA');
        this.myCloner.AddField('Transshipment1ATD');
        this.myCloner.AddField('Transshipment1ATA');

        // Transshipment2
        this.myCloner.AddField('Transshipment2CarrierId');
        this.myCloner.AddField('Transshipment2CarrierPrefix');
        this.myCloner.AddField('Transshipment2CarrierNumber');
        this.myCloner.AddField('Transshipment2AdditionalMAWBOBLBL');
        this.myCloner.AddField('Transshipment2VesselId');
        this.myCloner.AddField('Transshipment2ETD');
        this.myCloner.AddField('Transshipment2ETA');
        this.myCloner.AddField('Transshipment2ATD');
        this.myCloner.AddField('Transshipment2ATA');

        // Transshipment3
        this.myCloner.AddField('Transshipment3CarrierId');
        this.myCloner.AddField('Transshipment3CarrierPrefix');
        this.myCloner.AddField('Transshipment3CarrierNumber');
        this.myCloner.AddField('Transshipment3AdditionalMAWBOBLBL');
        this.myCloner.AddField('Transshipment3VesselId');
        this.myCloner.AddField('Transshipment3ETD');
        this.myCloner.AddField('Transshipment3ETA');
        this.myCloner.AddField('Transshipment3ATD');
        this.myCloner.AddField('Transshipment3ATA');

        if (this.TransportModeId == "I") {
            this.myCloner.AddField('TrailerNumber');
        }

        this.myCloner.AddEntity(this.EntityPM);

        if (this.TransportModeId == "A") {
            this.entityCloner = new Cloner(this.EntityPM);
            this.entityCloner.AddField("CarrierIsCheckDigit");
            this.entityCloner.AddField("CarrierIsLimitedLength");
            this.entityCloner.AddField("CarrierIsChampRegistered");
            this.entityCloner.AddField("CarrierIsGLSHKRegistered");
            this.entityCloner.AddField("TenantZeroAirlineId");
            this.entityCloner.AddField("TenantZeroAirlineTTY");
            this.entityCloner.AddField("TenantZeroAirlinePIMA");
            this.entityCloner.AddField("TenantZeroAirlineChampFWB");
            this.entityCloner.AddField("TenantZeroAirlineChampFHL");
            this.entityCloner.AddField("TenantZeroAirlineChampFSU");
            this.entityCloner.AddField("TenantZeroAirlineChampFVRFVA");
            this.entityCloner.AddField("TenantZeroAirlineChampFSRFSA");
            this.entityCloner.AddField("TenantZeroAirlineChampNeedsRegistration");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFWB");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFHL");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFSU");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFVRFVA");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFSRFSA");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKNeedsRegistration");
        }
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

        if (this.entityCloner) {
            this.entityCloner.RejectChanges();
        }

        this.myCloner.RejectChanges();
    }

    private CheckContainersToggleFeatureUp() {
        this.IsContainersToggleFeatureUp = false;
        var isOceanInsightsContainersFeatureToggleUp: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "OIC")[0];
        if (isOceanInsightsContainersFeatureToggleUp) {
            this.IsContainersToggleFeatureUp = true;
        }
    }
}
