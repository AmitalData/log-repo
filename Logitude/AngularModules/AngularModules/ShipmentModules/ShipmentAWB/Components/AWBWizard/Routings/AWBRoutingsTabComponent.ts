import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, FormatTool, DateTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {ShipmentTool, RoutingHelper} from '../../../../../Shipment/Tools';
import {PortList} from '../../../../../Common/EntityLists/PortList';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../../../Common/EntityLists/AirlineList';
import {AirlinePM} from '../../../../../Common/EntityPMs/AirlinePM';
import {MAWBStackPM} from '../../../../../Common/EntityPMs/MAWBStackPM';
import {PortListService} from '../../../../../Common/Services/StandardLists/PortListService';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {AirlineListService} from '../../../../../Common/Services/StandardLists/AirlineListService';
import {PartnersDomainService} from '../../../../../Common/Services/PartnersDomainService';
import {AWBStackDomainService} from '../../../../../Common/Services/AWBStackDomainService';
import {ShipmentDomainService} from '../../../../../Shipment/Services/ShipmentDomainService';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {GetStackWindowArgs} from '../../../../../Common/Args';
import {FlightsSchedulesArgs} from '../../../../../CommonModules/CommonFlightsSchedules/Args';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    selector: 'AWBRoutingsTabComponent',
    templateUrl: './AWBRoutingsTabComponent.html',   
})

export class AWBRoutingsTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: AWBRoutingsTabComponent = this;
    public ObjectTableName: string;
    public LabelColumnWidth: number = 110;
    public ControlColumnWidth: number = 165;
    public StockColumnWidth: number = 90;
    public TransportModeId: string = null;
    constructor() {
        super();
        this.InitializeServices();
    }

    private myPortListService: PortListService;
    private myCardService: CardListService;
    private myAirlineService: AirlineListService;
    private myPartnersDomainService: PartnersDomainService;
    private StackDomainService: AWBStackDomainService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private InitializeServices() {

        this.StackDomainService = new AWBStackDomainService();

        if (this.myPortListService == null) {
            this.myPortListService = new PortListService();            
        }

        if (this.myCardService == null) {
            this.myCardService = new CardListService();            
        }

        if (this.myAirlineService == null) {
            this.myAirlineService = new AirlineListService();            
        }

        if (this.myPartnersDomainService == null) {
            this.myPartnersDomainService = new PartnersDomainService();
        }
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.SetLabels();
        this.Listen();
        this.Validate();
        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
        this.SetFlightSchedules();
    }

    RefreshTab() {
        this.Validate();
        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
        this.isFlightSchedulesRequested = false;
    }

    private isSaveRequested: boolean = false;
    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                    this.OnEntityListenSuccess();
                }

                this.StopListenFlags();
            });
            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                    this.OnEntityListenSuccess();
                }
              
                this.StopListenFlags();                
            });
        }
    }
    private OnEntityListenSuccess() {

        if (this.isFlightSchedulesRequested) {
            this.isFlightSchedulesRequested = false;
            this.RunFlightSchedules();
        }

        else if (this.isSaveRequested) {

            this.isSaveRequested = false;

            //if (this.isGetFromStock) {

            //    this.isGetFromStock = false;

            //    this.EntityPM.MAWBTakenFromStack = false;
            //    this.EntityPM.MAWBStackNumber = this.myOldMAWBStackNumber;
            //    this.MAWBOBLDate = this.myOldMAWBOBLDate;
            //}

            this.FireWizardEvent();
            this.SetUIProperties_Carriers();
        }

        //else if (this.isReloadRequested) {
        //    this.isReloadRequested = false;

        //    this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        //    this.Validate();
        //    this.FireWizardEvent();
        //    this.SetUIProperties_Carriers();
        //}
    }
     
    private StopListenFlags() {
        //this.isGetFromStock = false;
        this.isSaveRequested = false;
        this.isFlightSchedulesRequested = false;
    }
    private Save() {
        this.isSaveRequested = true;
        this.Wizard.SaveClicked();
    }

    // SetUIProperties
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
    public IsEditingEnabled: boolean = false;
    public IsMainCarrierFieldsEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.SetUIProperties_Ports();
        this.SetUIProperties_Carriers();
    }
    private SetUIProperties_Ports() {
        var isPortsEnabled = this.IsEditingEnabled;
        var isPortVia1Enabled = false;
        var isPortVia2Enabled = false;

        if (isPortsEnabled) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEnabled = false;
            }
        }

        if (isPortsEnabled) {

            isPortVia1Enabled = true;

            //if (this.MainCarriageFromPortId != null) {
            //    isPortVia1Enabled = true;
            //}

            if (this.Transshipment2FromPortId != null) {
                isPortVia2Enabled = true;
            }

            else if (this.Transshipment1FromPortId != null) {
                isPortVia2Enabled = true;
            }
        }

        if (isPortsEnabled) {
            if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                if (this.EntityPM.StatusWeight >= 60) {
                    isPortsEnabled = false;
                }
            }
        }

        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("Transshipment1FromPortId", this.ObjectTableName, isPortVia1Enabled);
        this.UIProperties.SetEnabled("Transshipment2FromPortId", this.ObjectTableName, isPortVia2Enabled);
        this.UIProperties.SetEnabled("MainCarriageFinalDestinationPortId", this.ObjectTableName, isPortsEnabled);

        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageFinalDestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageFinalDestinationPortId) ? true : false);
    }
    private SetUIProperties_Carriers() {
        this.SetUIProperties_CarrierM();
        this.SetUIProperties_CarrierT1();
        this.SetUIProperties_CarrierT2();        
    }
    private SetUIProperties_CarrierM() {

        var isCarrierEnabled = this.IsEditingEnabled;
        var isCarrierFieldsEnabled = this.IsEditingEnabled;
        var isInterlineEnabled = this.IsEditingEnabled;        
        var isMasterEnabled = this.IsEditingEnabled;
        var isETDEnabled = this.IsEditingEnabled;

        if (this.IsEditingEnabled) {
            isCarrierFieldsEnabled = false;           
            isMasterEnabled = false;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isCarrierEnabled = false;                
                isInterlineEnabled = false;
                isETDEnabled = false;                
            }

            else {
                var isTakenFromStock = (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) ? true : false;
                if (isTakenFromStock || !AppTool.IsNullOrEmpty(this.Master)) {
                    isInterlineEnabled = false;
                    isCarrierEnabled = false;
                }

                if (!isTakenFromStock) {
                    if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) || !AppTool.IsNullOrEmpty(this.InterlineId)) {
                        isMasterEnabled = true;
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    isCarrierFieldsEnabled = true;
                }
            }
        }

        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("InterlineId", this.ObjectTableName, isInterlineEnabled);
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isMasterEnabled);
        this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, isETDEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierPrefix", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("MAWBOBLDate", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageATD", this.ObjectTableName, this.IsEditingEnabled);

        if (!AppTool.IsNullOrEmpty(this.InterlineId)) {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Master));
        }
        else {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, false);
        }

        this.SetUIProperties_Interline();
        this.SetUIProperties_StockButton();
    }
    private SetUIProperties_CarrierT1() {
        var isCarrierEnabled = this.IsEditingEnabled;
        var isCarrierFieldsEnabled = (this.IsEditingEnabled && this.Transshipment1CarrierId != null) ? true : false;
        this.UIProperties.SetEnabled("Transshipment1CarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierPrefix", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierNumber", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1AdditionalMAWBOBLBL", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETD", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment1ATD", this.ObjectTableName, isCarrierEnabled);
    }
    private SetUIProperties_CarrierT2() {
        var isCarrierEnabled = this.IsEditingEnabled;
        var isCarrierFieldsEnabled = (this.IsEditingEnabled && this.Transshipment2CarrierId != null) ? true : false;
        this.UIProperties.SetEnabled("Transshipment2CarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierPrefix", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierNumber", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2AdditionalMAWBOBLBL", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2ETD", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment2ATD", this.ObjectTableName, isCarrierEnabled);       
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

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId) && this.EntityPM.MainCarriageIsFromStack) {
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

        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
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

            else if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
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

    // Labels
    public FromTextCode: string;
    public ToTextCode: string;
    public MainCarriageTextCode: string;
    public Transshipment1TextCode: string;
    public Transshipment2TextCode: string;
    public Transshipment3TextCode: string;
    public CarrierTextCode: string;
    public CarrierNumberTextCode: string;
    public MasterTextCode: string;
    public MasterDateTextCode: string;
    private SetLabels() {
        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.FromTextCode = "Shipment.O.Routings.Gateway";
                this.ToTextCode = "Shipment.O.Routings.Destination";
                this.MainCarriageTextCode = "Shipment.O.Routings.MainCarriageLeg1";
                this.Transshipment1TextCode = "Shipment.O.Routings.MainCarriageLeg2";
                this.Transshipment2TextCode = "Shipment.O.Routings.MainCarriageLeg3";
                this.Transshipment3TextCode = "Shipment.O.Routings.MainCarriageLeg4";
                this.CarrierTextCode = "Shipment.O.Routings.Airline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.FlightNo";
                this.MasterTextCode = "Shipment.O.Routings.MAWB";
                this.MasterDateTextCode = "Shipment.O.Routings.MAWBDate";
                break;
            }

            case "O": {
                this.FromTextCode = "Shipment.O.Routings.LoadingPort";
                this.ToTextCode = "Shipment.O.Routings.DischargePort";
                this.MainCarriageTextCode = "Shipment.O.Routings.MainCarriage";
                this.Transshipment1TextCode = "Shipment.O.Routings.Transshipment1";
                this.Transshipment2TextCode = "Shipment.O.Routings.Transshipment2";
                this.Transshipment3TextCode = "Shipment.O.Routings.Transshipment3";
                this.CarrierTextCode = "Shipment.O.Routings.Shippingline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.VoyageNo";
                this.MasterTextCode = "Shipment.O.Routings.OBL";
                this.MasterDateTextCode = "Shipment.O.Routings.OBLDate";
                break;
            }

            case "I": {
                this.FromTextCode = "Shipment.O.Routings.From";
                this.ToTextCode = "Shipment.O.Routings.To";
                this.MainCarriageTextCode = "Shipment.O.Routings.MainCarriageLeg1";
                this.Transshipment1TextCode = "Shipment.O.Routings.MainCarriageLeg2";
                this.Transshipment2TextCode = "Shipment.O.Routings.MainCarriageLeg3";
                this.Transshipment3TextCode = "Shipment.O.Routings.MainCarriageLeg4";
                this.CarrierTextCode = "Shipment.O.Routings.Trucker";
                this.CarrierNumberTextCode = "Shipment.O.Routings.TruckNo";
                this.MasterTextCode = "Shipment.O.Routings.CMR/RWB#";
                this.MasterDateTextCode = "Shipment.O.Routings.CMR/RWBDate";
                break;
            }
        }
    }

    // Validate
    public ShowWarning_MainCarriageCarrierId: boolean = false;
    public ShowWarning_MainCarriageCarrierNumber: boolean = false;
    public ShowWarning_Master: boolean = false;
    public ShowWarning_MAWBOBLDate: boolean = false;
    public ShowWarning_MainCarriageETD: boolean = false;
    public ShowWarning_Transshipment1CarrierId: boolean = false;
    public ShowWarning_Transshipment2CarrierId: boolean = false;
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_PAR();
        this.Wizard.ValidateScreen_ROU();
        this.Wizard.ValidateScreen_GEN();
        this.Wizard.ValidateScreen_PAC();
        this.Wizard.ValidateScreen_OCI();
    }
    private Validate() {
        this.ShowWarning_MainCarriageCarrierId = false;
        this.ShowWarning_MainCarriageCarrierNumber = false;
        this.ShowWarning_Master = false;
        this.ShowWarning_MAWBOBLDate = false;
        this.ShowWarning_MainCarriageETD = false;
        this.ShowWarning_Transshipment1CarrierId = false;
        this.ShowWarning_Transshipment2CarrierId = false;

        if (this.MainCarriageCarrierId == null) {
            this.ShowWarning_MainCarriageCarrierId = true;
        }

        if (AppTool.IsNullOrEmpty(this.Master)) {
            this.ShowWarning_Master = true;
        }

        if (this.MAWBOBLDate == null) {
            this.ShowWarning_MAWBOBLDate = true;
        }

        if (this.MainCarriageETD == null) {
            this.ShowWarning_MainCarriageETD= true;
        }

        if (this.Transshipment1CarrierId == null) {
            this.ShowWarning_Transshipment1CarrierId = true;
        }

        if (this.Transshipment2CarrierId == null) {
            this.ShowWarning_Transshipment2CarrierId = true;
        }

        var isCarrierNumberFormatValid = FormatTool.Validate_FlightNumber(this.MainCarriageCarrierNumber);
        if (this.MainCarriageCarrierId == null) {
            if (!isCarrierNumberFormatValid) {
                this.ShowWarning_MainCarriageCarrierNumber = true;
            }
        }

        else {
            var codePrefix: string = AppTool.IsNullOrEmpty(this.MainCarriageCarrierPrefix) ? this.MainCarriageCarrierPrefix : this.MainCarriageCarrierPrefix.trim();
            var isValidcodePrefix = !AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;
            if (!isValidcodePrefix || !isCarrierNumberFormatValid) {
                this.ShowWarning_MainCarriageCarrierNumber = true;
            }
        }
    }

    // Ports
    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(value: string) {
        if (this.EntityPM.MainCarriageFromPortId != value) {
            this.EntityPM.MainCarriageFromPortId = value;
            this.Validate();
            this.FireWizardEvent();
            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);

                if (this.Wizard) {
                    this.Wizard.SetCargonautDEXXVisibility();
                }
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, list);

                            if (this.Wizard) {
                                this.Wizard.SetCargonautDEXXVisibility();
                            }
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, list);

                                    if (this.Wizard) {
                                        this.Wizard.SetCargonautDEXXVisibility();
                                    }
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
            this.Validate();
            this.FireWizardEvent();
            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.Transshipment1FromPortChanged(this.EntityPM, null);
                RoutingHelper.RemoveTransshipment1Leg(this.EntityPM);
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
            this.Validate();
            this.FireWizardEvent();
            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.Transshipment2FromPortChanged(this.EntityPM, null);
                RoutingHelper.RemoveTransshipment2Leg(this.EntityPM);
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
            this.Validate();
            this.FireWizardEvent();
            this.SetUIProperties_Ports();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.Transshipment3FromPortChanged(this.EntityPM, null);
                RoutingHelper.RemoveTransshipment3Leg(this.EntityPM);
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

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
                this.OnFinalDestinationChanged();
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;

                        if (list) {
                            RoutingHelper.FinalDestinationPortChanged(this.EntityPM, list);
                            this.OnFinalDestinationChanged();
                        }

                        else {
                            this.myPortListService.getSingle(value).subscribe((myResponse2: ServiceResponse) => {
                                if (!myResponse2.HasError) {
                                    var list: PortList = myResponse2.Result;
                                    RoutingHelper.FinalDestinationPortChanged(this.EntityPM, list);
                                    this.OnFinalDestinationChanged();
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    OnFinalDestinationChanged() {
        this.Validate();
        this.FireWizardEvent();
        this.SetUIProperties_Ports();
    }

    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierId != newValue) {
            this.EntityPM.MainCarriageCarrierId = newValue;
            this.SetUIProperties_Carriers();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.MainCarriageCarrierCode = null;
                this.EntityPM.MainCarriageCarrierName = null;
                this.EntityPM.MainCarriageCarrierPrefix = null;
                this.EntityPM.MainCarriageCarrierWebSite = null;
                this.EntityPM.AccountNumber = null;
                this.EntityPM.MainCarriageCarrierNumber = null;

                if (AppTool.IsNullOrEmpty(this.InterlineId)) {
                    this.EntityPM.CarrierIsCheckDigit = false;
                    this.EntityPM.CarrierIsLimitedLength = false;
                    this.AirlinePrefix = null;
                }

                this.Master = null;
                this.EntityPM.CarrierIsChampRegistered = false;
                this.EntityPM.CarrierIsGLSHKRegistered = false;
                ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);

                this.Validate();
                this.FireWizardEvent();
                this.SetUIProperties_Carriers();
            }

            else {
                this.myCardService.getSingle(newValue).subscribe((myCardListResponse: ServiceResponse) => {
                    if (myCardListResponse != null) {

                        var myCardList: CardList = myCardListResponse.Result;
                        if (myCardList != null) {
                            this.EntityPM.MainCarriageCarrierCode = myCardList.Code;
                            this.EntityPM.MainCarriageCarrierName = myCardList.EnglishName;
                            this.EntityPM.MainCarriageCarrierPrefix = myCardList.Code;
                            this.EntityPM.MainCarriageCarrierWebSite = myCardList.WebSite;
                            this.EntityPM.AccountNumber = myCardList.AirlineAccountNumber;
                            this.Wizard.LoadAirlineRules(myCardList.Code);

                            this.Validate();
                            this.FireWizardEvent();
                            this.SetUIProperties_Carriers();

                            // dont get from chach: if user choosed from tenant0 it wont get it
                            this.myAirlineService.getSingle(newValue).subscribe((myAirlineListResponse: ServiceResponse) => {
                                if (myAirlineListResponse != null) {

                                    var myAirlineList: AirlineList = myAirlineListResponse.Result;
                                    if (myAirlineList != null) {
                                        if (AppTool.IsNullOrEmpty(this.InterlineId)) {
                                            this.EntityPM.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                            this.EntityPM.CarrierIsLimitedLength = myAirlineList.LimitedLength;

                                            var myPrefix: string = null;
                                            if (!AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                                myPrefix = myAirlineList.Prefix.toString().trim();
                                                myPrefix = AppTool.PadLeft(myPrefix, 3, '0');
                                            }

                                            this.AirlinePrefix = myPrefix;
                                        }

                                        this.EntityPM.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                        this.EntityPM.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;

                                        this.Validate();
                                        this.FireWizardEvent();

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
                });
            }
        }
    }

    get MainCarriageCarrierPrefix() { return this.EntityPM.MainCarriageCarrierPrefix; }
    set MainCarriageCarrierPrefix(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierPrefix != newValue) {
            this.EntityPM.MainCarriageCarrierPrefix = newValue;
            this.FireWizardEvent();
        }
    }

    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    set MainCarriageCarrierNumber(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierNumber != newValue) {
            this.EntityPM.MainCarriageCarrierNumber = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    set AirlinePrefix(newValue: string) {
        if (this.EntityPM.AirlinePrefix != newValue) {
            this.EntityPM.AirlinePrefix = newValue;
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(newValue: string) {
        if (this.EntityPM.Master != newValue) {
            this.EntityPM.Master = newValue;                        
            this.LongMaster = ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
        }
    }

    get LongMaster() { return this.EntityPM.LongMaster; }
    set LongMaster(newValue: string) {
        if (this.EntityPM.LongMaster != newValue) {
            this.EntityPM.LongMaster = newValue;

            this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
            this.Validate();
            this.FireWizardEvent();           
            this.ValidateMasterField();
            this.SetUIProperties_Carriers();
        }
    }

    get MAWBOBLDate() { return this.EntityPM.MAWBOBLDate; }
    set MAWBOBLDate(newValue: Date) {
        if (this.EntityPM.MAWBOBLDate != newValue) {
            this.EntityPM.MAWBOBLDate = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(newValue: string) {
        if (this.EntityPM.AccountNumber != newValue) {
            this.EntityPM.AccountNumber = newValue;
        }
    }

    // InterlineId
    public IsInterlineAdded: boolean = false;
    AddInterlineClicked() {
        this.IsInterlineAdded = true;
    }

    get InterlineId() { return this.EntityPM.InterlineId; }
    set InterlineId(newValue: string) {
        if (this.EntityPM.InterlineId != newValue) {
            this.EntityPM.InterlineId = newValue;

            this.OnInterlineChanged();
        }
    }

    public OnInterlineLostFocus($event) {
        // this.OnInterlineChanged();
        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
    }

    private OnInterlineChanged() {
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
            this.myAirlineService.getSingleFromCache(myAirlineId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;

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

        this.SetUIProperties_Carriers();
    }

    // Transshipment1
    private transshipment1Carrier: CardList = null;
    get Transshipment1Carrier() { return this.transshipment1Carrier; }
    set Transshipment1Carrier(list: CardList) {
        if (this.transshipment1Carrier != list) {
            this.transshipment1Carrier = list;

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.Transshipment1CarrierCode) {
                if (list == null) {
                    this.EntityPM.Transshipment1CarrierCode = null;
                    this.EntityPM.Transshipment1CarrierName = null;
                    this.EntityPM.Transshipment1CarrierPrefix = null;
                    this.EntityPM.Transshipment1CarrierWebSite = null;
                    this.EntityPM.Transshipment1CarrierNumber = null;
                    this.EntityPM.Transshipment1AdditionalMAWBOBLBL = null;
                }

                else {
                    this.EntityPM.Transshipment1CarrierCode = list.Code;
                    this.EntityPM.Transshipment1CarrierName = list.EnglishName;
                    this.EntityPM.Transshipment1CarrierPrefix = list.Code;
                    this.EntityPM.Transshipment1CarrierWebSite = list.WebSite;
                }
            }
        }
    }

    get Transshipment1CarrierId() { return this.EntityPM.Transshipment1CarrierId; }
    set Transshipment1CarrierId(newValue: string) {
        if (this.EntityPM.Transshipment1CarrierId != newValue) {
            this.EntityPM.Transshipment1CarrierId = newValue;           
            this.Validate();
            this.FireWizardEvent();
            this.SetUIProperties_CarrierT1();          
        }
    }

    get Transshipment1CarrierPrefix() { return this.EntityPM.Transshipment1CarrierPrefix; }
    set Transshipment1CarrierPrefix(newValue: string) {
        if (this.EntityPM.Transshipment1CarrierPrefix != newValue) {
            this.EntityPM.Transshipment1CarrierPrefix = newValue;
            this.FireWizardEvent();
        }
    }
    
    get Transshipment1CarrierNumber() { return this.EntityPM.Transshipment1CarrierNumber; }
    set Transshipment1CarrierNumber(newValue: string) {
        if (this.EntityPM.Transshipment1CarrierNumber != newValue) {
            this.EntityPM.Transshipment1CarrierNumber = newValue;
            this.FireWizardEvent();
        }
    }

    get Transshipment1AdditionalMAWBOBLBL() { return this.EntityPM.Transshipment1AdditionalMAWBOBLBL; }
    set Transshipment1AdditionalMAWBOBLBL(newValue: string) {
        if (this.EntityPM.Transshipment1AdditionalMAWBOBLBL != newValue) {
            this.EntityPM.Transshipment1AdditionalMAWBOBLBL = newValue;
            this.FireWizardEvent();
        }
    }

    // Transshipment2
    private transshipment2Carrier: CardList = null;
    get Transshipment2Carrier() { return this.transshipment2Carrier; }
    set Transshipment2Carrier(list: CardList) {
        if (this.transshipment2Carrier != list) {
            this.transshipment2Carrier = list;

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.Transshipment2CarrierCode) {
                if (list == null) {
                    this.EntityPM.Transshipment2CarrierCode = null;
                    this.EntityPM.Transshipment2CarrierName = null;
                    this.EntityPM.Transshipment2CarrierPrefix = null;
                    this.EntityPM.Transshipment2CarrierWebSite = null;
                    this.EntityPM.Transshipment2CarrierNumber = null;
                    this.EntityPM.Transshipment2AdditionalMAWBOBLBL = null;
                }

                else {
                    this.EntityPM.Transshipment2CarrierCode = list.Code;
                    this.EntityPM.Transshipment2CarrierName = list.EnglishName;
                    this.EntityPM.Transshipment2CarrierPrefix = list.Code;
                    this.EntityPM.Transshipment2CarrierWebSite = list.WebSite;
                }
            }
        }
    }

    get Transshipment2CarrierId() { return this.EntityPM.Transshipment2CarrierId; }
    set Transshipment2CarrierId(newValue: string) {
        if (this.EntityPM.Transshipment2CarrierId != newValue) {
            this.EntityPM.Transshipment2CarrierId = newValue;            
            this.Validate();
            this.FireWizardEvent();
            this.SetUIProperties_CarrierT2();           
        }
    }

    get Transshipment2CarrierPrefix() { return this.EntityPM.Transshipment2CarrierPrefix; }
    set Transshipment2CarrierPrefix(newValue: string) {
        if (this.EntityPM.Transshipment2CarrierPrefix != newValue) {
            this.EntityPM.Transshipment2CarrierPrefix = newValue;
            this.FireWizardEvent();
        }
    }

    get Transshipment2CarrierNumber() { return this.EntityPM.Transshipment2CarrierNumber; }
    set Transshipment2CarrierNumber(newValue: string) {
        if (this.EntityPM.Transshipment2CarrierNumber != newValue) {
            this.EntityPM.Transshipment2CarrierNumber = newValue;
            this.FireWizardEvent();
        }
    }

    get Transshipment2AdditionalMAWBOBLBL() { return this.EntityPM.Transshipment2AdditionalMAWBOBLBL; }
    set Transshipment2AdditionalMAWBOBLBL(newValue: string) {
        if (this.EntityPM.Transshipment2AdditionalMAWBOBLBL != newValue) {
            this.EntityPM.Transshipment2AdditionalMAWBOBLBL = newValue;
            this.FireWizardEvent();
        }
    }

    // Dates
    get MainCarriageETD() { return this.EntityPM.MainCarriageETD; }
    set MainCarriageETD(newValue: Date) {
        if (this.EntityPM.MainCarriageETD != newValue) {
            this.EntityPM.MainCarriageETD = newValue;
            this.Validate();
            this.FireWizardEvent();
            this.SetFlightDate();
        }
    }
    get MainCarriageATD() { return this.EntityPM.MainCarriageATD; }
    set MainCarriageATD(newValue: Date) {
        if (this.EntityPM.MainCarriageATD != newValue) {
            this.EntityPM.MainCarriageATD = newValue;
            this.SetFlightDate();
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
        }
    }

    public MasterFieldValidityMessage: string = null;
    private myShipmentDomainService: ShipmentDomainService;
    private ValidateMasterField() {

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
    private ValidateMasterFieldIsUsed() {
        if (this.myShipmentDomainService == null) {
            this.myShipmentDomainService = new ShipmentDomainService();
        }

        if (!AppTool.IsNullOrEmpty(this.Master)) {
            this.myShipmentDomainService.ValidateShipmentMasterFieldExistance(this.EntityPM.Id, this.EntityPM.BookingId, this.EntityPM.Master, this.EntityPM.AirlinePrefix, this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode, this.EntityPM.IsCancelled)
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

    MasterLostFocus(input: any) {
        this.ValidateMasterStack();
    }
    private ValidateMasterStack() {
        if (this.EntityPM != null) {
            if (!AppTool.IsNullOrEmpty(this.Master)) {
                if (this.Master.length == 8 && !this.EntityPM.MainCarriageIsFromStack && !this.EntityPM.MAWBTakenFromStack) {
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
                                                    this.EntityPM.MAWBTakenFromStack = true;
                                                    this.EntityPM.MAWBStackNumber = AppTool.PadLeft(myStackPM.Number.toString(), 8, '0');
                                                    this.MAWBOBLDate = DateTool.GetCurrentDateTimeAsUtc();
                                                    this.SetMAWBAirline();
                                                    //this.isGetFromStock = true;
                                                    this.Save();
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

    // Commands
    AddStockClicked(airlineId: string) {
        if (!AppTool.IsNullOrEmpty(airlineId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit Airline";
            logWindow.ShowEditComponent(airlineId, "Airline", "ALST");
        }
    }

    //private isGetFromStock: boolean = false;
    //private myOldMAWBStackNumber: string;
    //private myOldMAWBOBLDate: Date;
    GetStockClicked() {
        var isValid = this.Wizard.ValidateShipment();
        if (isValid) {
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
                        //this.myOldMAWBStackNumber = this.EntityPM.MAWBStackNumber;
                        //this.myOldMAWBOBLDate = this.EntityPM.MAWBOBLDate;

                        this.EntityPM.MAWBTakenFromStack = true;
                        this.EntityPM.MAWBStackNumber = AppTool.PadLeft(stackNumber.toString(), 8, '0');
                        this.MAWBOBLDate = DateTool.GetCurrentDateTimeAsUtc();

                        //this.isGetFromStock = true;
                        this.Save();                        
                    }
                }
            });
        }
    }
    ReturnStockClicked() {
        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            var isValid = this.Wizard.ValidateShipment();
            if (isValid) {
                this.SetMAWBAirline();
                this.EntityPM.MAWBReturnedToStack = true;
                this.EntityPM.MAWBStackNumber = this.Master;
                //this.isGetFromStock = false;
                this.Save();
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

    // Flights Schedules
    private isMainLegFlightSchedules: boolean = false;
    private isFlightSchedulesRequested: boolean = false;
    public IsFlightSchedulesVisible: boolean = false;
    SetFlightSchedules() {
        if (FeatureLocator.HasFeaturePermession("General", "FlightsSchedules")) {
            this.IsFlightSchedulesVisible = true;
        }
    }

    FlightSchedulesClicked(isMainLeg: boolean) {
        if (!this.isFlightSchedulesRequested) {
            this.isMainLegFlightSchedules = isMainLeg;
            this.isFlightSchedulesRequested = true;
            this.Wizard.SaveClicked();
        }
    }
    private RunFlightSchedules() {
        var args = new FlightsSchedulesArgs();
        args.ShipmentPM = this.EntityPM;
        args.IsMainLeg = this.isMainLegFlightSchedules;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = args;
        logWindow.Title = "Send FVA Response";
        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(response=> {
            logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FlightsSchedulesComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                if (args.IsFlightSelected) {
                    this.Validate();
                    this.FireWizardEvent();
                    this.SetUIProperties();
                }
            });
        });
    }

}
