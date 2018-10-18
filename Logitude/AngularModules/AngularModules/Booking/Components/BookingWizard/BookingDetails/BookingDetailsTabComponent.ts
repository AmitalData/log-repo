import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {AWBUtilities} from '../../../Utilities/AWBUtilities';
import {BookingPM} from '../../../EntityPMs/BookingPM';
import {BookingWizardComponent} from '../BookingWizardComponent';
import {PortList} from '../../../../Common/EntityLists/PortList';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {AirlineListService} from '../../../../Common/Services/StandardLists/AirlineListService';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {AWBStackDomainService} from '../../../../Common/Services/AWBStackDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {GetStackWindowArgs} from '../../../../Common/Args';
import {FlightsSchedulesArgs} from '../../../../CommonModules/CommonFlightsSchedules/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {BookingTool} from '../../../Tools';
import {MAWBStackPM} from '../../../../Common/EntityPMs/MAWBStackPM';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {BookingDomainService} from '../../../Services/BookingDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'BookingDetailsTabComponent',
    moduleId: module.id,
    templateUrl: './BookingDetailsTabComponent.html',
})

export class BookingDetailsTabComponent extends BaseComponent {
    public Wizard: BookingWizardComponent;
    public EntityPM: BookingPM;
    public DataContext: BookingDetailsTabComponent = this;
    public ObjectTableName: string;
    public IsFlightSchedulesVisible: boolean = false;
    constructor() {
        super();
        this.InitializeServices();

        if (FeatureLocator.HasFeaturePermession("General", "FlightsSchedules")) {
            this.IsFlightSchedulesVisible = true;
        }
    }

    private myPortService: PortListService;
    private myCardService: CardListService;
    private myAirlineService: AirlineListService;
    private myPartnersDomainService: PartnersDomainService;
    private StackDomainService: AWBStackDomainService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private InitializeServices() {

        this.StackDomainService = new AWBStackDomainService();

        if (this.myPortService == null) {
            this.myPortService = new PortListService();
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

    InitTab(wizard: BookingWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.Validate();
        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
    }

    RefreshTab() {
        this.Validate();
        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
    }

    private isEditingEnabled_MasterPorts: boolean;
    private isEditingEnabled_Others: boolean;
    public IsApplyAllEnabled: boolean = false;
    public SetUIProperties() {
        this.isEditingEnabled_MasterPorts = BookingTool.IsEditingFieldsEnabled_MasterPorts(this.EntityPM);
        this.isEditingEnabled_Others = BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);

        this.SetUIProperties_Carriers();
        this.SetUIProperties_Ports();
        this.SetUIProperties_Others();

    }
    SetUIProperties_Carriers() {
        var isInterlineEnabled = this.isEditingEnabled_MasterPorts;
        var isMainCarrierEnabled = this.isEditingEnabled_MasterPorts;
        var isMasterFieldEnabled = this.isEditingEnabled_MasterPorts;

        var isMainCarrierNumberEnabled = false;
        var isMainCarriageCarrierPrefixEnabled = false;
        var isMaincarriageETDEnabled = false;
        var isTransshipment1FieldsEnabled = false;
        var isTransshipment2FieldsEnabled = false;       
        
        if (this.isEditingEnabled_MasterPorts) {
            isInterlineEnabled = true;
            isMainCarrierEnabled = true;
            isMasterFieldEnabled = false;

            var isTakenFromStock = (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) ? true : false;
            if (isTakenFromStock || !AppTool.IsNullOrEmpty(this.Master)) {
                isInterlineEnabled = false;
                isMainCarrierEnabled = false;
            }

            if (!isTakenFromStock) {
                if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) || !AppTool.IsNullOrEmpty(this.InterlineId)) {
                    isMasterFieldEnabled = true;
                }
            }
        }

        if (this.isEditingEnabled_Others) {
            if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                if (AppTool.IsNullOrEmpty(this.InterlineId)) {
                    if (this.IsManualDataEntryEnabled) {
                        isMainCarrierNumberEnabled = true;
                        isMainCarriageCarrierPrefixEnabled = true;
                    }
                }

                else {
                    isMainCarrierNumberEnabled = true;
                    isMainCarriageCarrierPrefixEnabled = true;
                }
            }

            //if (!AppTool.IsNullOrEmpty(this.Transshipment1CarrierId)) {
                if (!AppTool.IsNullOrEmpty(this.InterlineId)) {
                    if (this.IsManualDataEntryEnabled_Via1) {
                        isTransshipment1FieldsEnabled = true;
                    }
                }

                else {
                    isTransshipment1FieldsEnabled = true;
                }
            //}

            if (!AppTool.IsNullOrEmpty(this.Transshipment2CarrierId)) {
                isTransshipment2FieldsEnabled = true;
            }
            
            if (this.EntityPM.BookingStatusCode != "AWB") {
                if (AppTool.IsNullOrEmpty(this.InterlineId)) {
                    if (this.IsManualDataEntryEnabled) {
                        isMaincarriageETDEnabled = true;
                    }
                }
                else {
                    isMaincarriageETDEnabled = true;
                }
            }
        }

        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isMasterFieldEnabled);
        this.UIProperties.SetEnabled("InterlineId", this.ObjectTableName, isInterlineEnabled);

        if (!AppTool.IsNullOrEmpty(this.InterlineId)) {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Master));
        }
        else {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, false);
        }

        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isMainCarrierEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isMainCarrierNumberEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierPrefix", this.ObjectTableName, isMainCarriageCarrierPrefixEnabled);
        this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, isMaincarriageETDEnabled);

        this.UIProperties.SetEnabled("Transshipment1CarrierNumber", this.ObjectTableName, isTransshipment1FieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierPrefix", this.ObjectTableName, isTransshipment1FieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETD", this.ObjectTableName, isTransshipment1FieldsEnabled);

        this.UIProperties.SetEnabled("Transshipment2CarrierNumber", this.ObjectTableName, isTransshipment2FieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierPrefix", this.ObjectTableName, isTransshipment2FieldsEnabled);

        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, this.isEditingEnabled_Others);
                
        this.SetUIProperties_StockButton();
        this.Validate_Carrier();
    }
    SetUIProperties_Ports() {
        var isMainLegEnabled = this.isEditingEnabled_MasterPorts;
        var isLeg1Enabled = false;
        var isLeg2Enabled = false;
        var isLastLegEnabled = this.isEditingEnabled_MasterPorts;

        if (this.isEditingEnabled_MasterPorts) {
            if (!AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
                isLeg1Enabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
                isLeg2Enabled = true;
            }
        }

        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isMainLegEnabled);
        this.UIProperties.SetEnabled("Transshipment1FromPortId", this.ObjectTableName, isLeg1Enabled);
        this.UIProperties.SetEnabled("Transshipment2FromPortId", this.ObjectTableName, isLeg2Enabled);
        this.UIProperties.SetEnabled("MainCarriageFinalDestinationPortId", this.ObjectTableName, isLastLegEnabled);

        this.Validate_Ports();
    }
    SetUIProperties_Others() {
        this.UIProperties.SetEnabled("SpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("MainCarriageSpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("Transshipment1SpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("Transshipment2SpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);

        var isMainCarriageIdentificationEnabled = false;
        var isTransshipment1IdentificationEnabled = false;
        var isTransshipment2IdentificationEnabled = false;
        var isApplyAllEnabled = false;

        if (this.isEditingEnabled_Others) {
            if (this.MainCarriageSpaceAllocationCode == "CA") {
                isMainCarriageIdentificationEnabled = true;
            }

            if (this.Transshipment1SpaceAllocationCode == "CA") {
                isTransshipment1IdentificationEnabled = true;
            }

            if (this.Transshipment2SpaceAllocationCode == "CA") {
                isTransshipment2IdentificationEnabled = true;
            }

            if (!AppTool.IsNullOrEmpty(this.SpaceAllocationCode)) {
                isApplyAllEnabled = true;
            }
        }

        this.IsApplyAllEnabled = isApplyAllEnabled;
        this.UIProperties.SetEnabled("MainCarriageAllotmentIdentification", this.ObjectTableName, isMainCarriageIdentificationEnabled);
        this.UIProperties.SetEnabled("Transshipment1AllotmentIdentification", this.ObjectTableName, isTransshipment1IdentificationEnabled);
        this.UIProperties.SetEnabled("Transshipment2AllotmentIdentification", this.ObjectTableName, isTransshipment2IdentificationEnabled);

        this.Validate_Others();
    }

    public IsAddStockVisible_Mn: boolean = false;
    public IsAddStockVisible_In: boolean = false;
    public IsFromStockVisible: boolean = false;
    public IsFromStockEnabled: boolean = false;
    public IsReturnStockVisible: boolean = false;
    public IsReturnStockEnabled: boolean = false;
    public IsFlightSchedulesEnabled: boolean = false;
    public IsAddInterlineEnabled: boolean = false;
    private SetUIProperties_StockButton() {
        var isAddStockVisible_Mn = false;
        var isAddStockVisible_In = false;
        var isFromStockVisible = false;
        var isReturnStockVisible = false;
        var isFromStockEnabled = this.isEditingEnabled_MasterPorts;
        var isReturnStockEnabled = this.isEditingEnabled_MasterPorts;
        var isFlightSchedulesEnabled = this.isEditingEnabled_Others;
        var isAddInterlineEnabled = this.isEditingEnabled_MasterPorts;

        isAddStockVisible_Mn = !AppTool.IsNullOrEmpty(this.MainCarriageCarrierId);
        isAddStockVisible_In = !AppTool.IsNullOrEmpty(this.InterlineId);

        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            isReturnStockVisible = true;
        }

        isFromStockVisible = !isReturnStockVisible;

        //interline
        if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) && this.EntityPM.MainCarriageIsFromStack) {
            isAddInterlineEnabled = false;
        }

        else if (!AppTool.IsNullOrEmpty(this.Master)) {
            isAddInterlineEnabled = false;
        }

        //get from stock
        if (AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) && AppTool.IsNullOrEmpty(this.InterlineId)) {
            isFromStockEnabled = false;
        }

        else if (!AppTool.IsNullOrEmpty(this.Master)) {
            isFromStockEnabled = false;
        }

        else if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            isFromStockEnabled = false;
        }

        this.IsAddStockVisible_Mn = isAddStockVisible_Mn;
        this.IsAddStockVisible_In = isAddStockVisible_In;
        this.IsFromStockVisible = isFromStockVisible;
        this.IsFromStockEnabled = isFromStockEnabled;
        this.IsReturnStockVisible = isReturnStockVisible;
        this.IsReturnStockEnabled = isReturnStockEnabled;
        this.IsFlightSchedulesEnabled = isFlightSchedulesEnabled;
        this.IsAddInterlineEnabled = isAddInterlineEnabled;
    }

    public ShowWarning_Master: boolean = false;
    public ShowWarning_FromPort: boolean = false;
    public ShowWarning_FinalPort: boolean = false;
    public ShowWarning_CarrierNumber: boolean = false;
    public ShowWarning_ETD: boolean = false;
    public ShowWarning_MainAllocation: boolean = false;
    public ShowWarning_MainAllotment: boolean = false;
    public ShowWarning_Trans1CarrierId: boolean = false;
    public ShowWarning_Trans1CarrierNumber: boolean = false;
    public ShowWarning_Trans1ETD: boolean = false;
    public ShowWarning_Trans1Allocation: boolean = false;
    public ShowWarning_Trans1Allotment: boolean = false;
    public ShowWarning_Trans2CarrierId: boolean = false;
    public ShowWarning_Trans2CarrierNumber: boolean = false;
    public ShowWarning_Trans2ETD: boolean = false;
    public ShowWarning_Trans2Allocation: boolean = false;
    public ShowWarning_Trans2Allotment: boolean = false;

    private Validate() {
        this.Validate_Carrier();
        this.Validate_Ports();
        this.Validate_Others();
    }
    private Validate_Carrier() {

        this.ShowWarning_Trans1CarrierId = !AppTool.IsNullOrEmpty(this.Transshipment1CarrierId) ? false : true;
        this.ShowWarning_Trans2CarrierId = !AppTool.IsNullOrEmpty(this.Transshipment2CarrierId) ? false : true;

        var isValidNumberFormat = FormatTool.Validate_FlightNumber(this.MainCarriageCarrierNumber);
        var codePrefix = AppTool.IsNullOrEmpty(this.MainCarriageCarrierPrefix) ? this.MainCarriageCarrierPrefix : this.MainCarriageCarrierPrefix.trim();
        var isValidcodePrefix = !AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;

        var isValidNumberFormat1 = FormatTool.Validate_FlightNumber(this.Transshipment1CarrierNumber);
        var codePrefix1 = AppTool.IsNullOrEmpty(this.Transshipment1CarrierPrefix) ? this.Transshipment1CarrierPrefix : this.Transshipment1CarrierPrefix.trim();
        var isValidcodePrefix1 = !AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;

        var isValidNumberFormat2 = FormatTool.Validate_FlightNumber(this.Transshipment2CarrierNumber);
        var codePrefix2 = AppTool.IsNullOrEmpty(this.Transshipment2CarrierPrefix) ? this.Transshipment2CarrierPrefix : this.Transshipment2CarrierPrefix.trim();
        var isValidcodePrefix2 = !AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;

        this.ShowWarning_Master = !AppTool.IsNullOrEmpty(this.Master) ? false : true;
        this.ShowWarning_CarrierNumber = isValidNumberFormat && isValidcodePrefix ? false : true;
        this.ShowWarning_Trans1CarrierNumber = isValidNumberFormat1 && isValidcodePrefix1 ? false : true;
        this.ShowWarning_Trans2CarrierNumber = isValidNumberFormat2 && isValidcodePrefix2 ? false : true;

        this.FireWizardEvent();
    }
    private Validate_Ports() {
        this.ShowWarning_FromPort = !AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? false : true;
        this.ShowWarning_FinalPort = !AppTool.IsNullOrEmpty(this.MainCarriageFinalDestinationPortId) ? false : true;

        this.FireWizardEvent();
    }
    private Validate_Others() {
        this.ShowWarning_ETD = this.MainCarriageETD != null ? false : true;
        this.ShowWarning_Trans1ETD = this.Transshipment1ETD != null ? false : true;
        this.ShowWarning_Trans2ETD = this.Transshipment2ETD != null ? false : true;

        this.ShowWarning_MainAllocation = !AppTool.IsNullOrEmpty(this.MainCarriageSpaceAllocationCode) ? false : true;
        this.ShowWarning_Trans1Allocation = !AppTool.IsNullOrEmpty(this.Transshipment1SpaceAllocationCode) ? false : true;
        this.ShowWarning_Trans2Allocation = !AppTool.IsNullOrEmpty(this.Transshipment2SpaceAllocationCode) ? false : true;

        if (this.MainCarriageSpaceAllocationCode == "CA") {
            if (!AppTool.IsNullOrEmpty(this.MainCarriageAllotmentIdentification)) {
                this.ShowWarning_MainAllotment = false;
            }
            else {
                this.ShowWarning_MainAllotment = true;
            }
        }
        else {
            this.ShowWarning_MainAllotment = false;
        }

        if (this.Transshipment1SpaceAllocationCode == "CA") {
            if (!AppTool.IsNullOrEmpty(this.Transshipment1AllotmentIdentification)) {
                this.ShowWarning_Trans1Allotment = false;
            }
            else {
                this.ShowWarning_Trans1Allotment = true;
            }
        }
        else {
            this.ShowWarning_Trans1Allotment = false;
        }

        if (this.Transshipment2SpaceAllocationCode == "CA") {
            if (!AppTool.IsNullOrEmpty(this.Transshipment2AllotmentIdentification)) {
                this.ShowWarning_Trans2Allotment = false;
            }
            else {
                this.ShowWarning_Trans2Allotment = true;
            }
        }
        else {
            this.ShowWarning_Trans2Allotment = false;
        }

        this.FireWizardEvent();
    }

    private FireWizardEvent() {
        this.Wizard.ValidateScreen_BKD();
        this.Wizard.ValidateScreen_GEN();
        this.Wizard.ValidateScreen_PAC();
    }

    private isSaveRequested: boolean = false;
    private isReloadRequested: boolean = false;
    private Save() {
        this.isSaveRequested = true;
        this.Wizard.SaveClicked();
    }
    private Reload() {
        this.isReloadRequested = true;
        this.Wizard.ReloadEntity();
    }
    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                }

                if (this.isSaveRequested) {
                    this.isSaveRequested = false;

                    if (isSaveSuccess) {
                        this.Reload();
                    }

                    else {
                        if (this.isGetFromStock) {
                            this.EntityPM.MAWBTakenFromStack = false;
                            this.EntityPM.MAWBStackNumber = this.myOldMAWBStackNumber;
                            this.isGetFromStock = false;
                        }
                        
                        this.FireWizardEvent();
                        this.SetUIProperties_Carriers();
                    }
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;

                    if (this.isReloadRequested) {
                        this.isReloadRequested = false;

                        this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
                        this.Validate();
                        this.FireWizardEvent();
                        this.SetUIProperties_Carriers();
                    }
                }
            });
        }
    }

    //General Properties
    get LongMaster() { return this.EntityPM.LongMaster; }
    set LongMaster(newValue: string) {
        if (this.EntityPM.LongMaster != newValue) {
            this.EntityPM.LongMaster = newValue;
        }
    }

    get SpaceAllocationCode() { return this.EntityPM.SpaceAllocationCode; }
    set SpaceAllocationCode(newValue: string) {
        if (this.EntityPM.SpaceAllocationCode != newValue) {
            this.EntityPM.SpaceAllocationCode = newValue;

            this.SetUIProperties_Others();
        }
    }

    ApplyAllClicked() {
        this.MainCarriageSpaceAllocationCode = this.SpaceAllocationCode;

        if (!AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
            this.Transshipment1SpaceAllocationCode = this.SpaceAllocationCode;
        }

        if (!AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
            this.Transshipment2SpaceAllocationCode = this.SpaceAllocationCode;
        }
    }

    //Main Carriage
    get MainCarriageCarrierId() { return this.EntityPM.MainCarriageCarrierId; }
    set MainCarriageCarrierId(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierId != newValue) {
            this.EntityPM.MainCarriageCarrierId = newValue;

            this.OnMainCarriageCarrierChanged();
        }
    }

    private OnMainCarriageCarrierChanged() {
        if (AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
                this.EntityPM.CarrierIsCheckDigit = false;
                this.EntityPM.CarrierIsLimitedLength = false;
                this.AirlinePrefix = null;
            }

            this.Master = null;
            this.AccountNumber = null;
            this.MainCarriageCarrierPrefix = null;
            this.MainCarriageCarrierNumber = null;
            this.EntityPM.MainCarriageCarrierName = null;
            this.EntityPM.TenantZeroAirlineId = null;
            this.EntityPM.TenantZeroAirlineTTY = null;
            this.EntityPM.TenantZeroAirlinePIMA = null;
            this.EntityPM.TenantZeroAirlineChampFFR = false;
            this.EntityPM.CarrierIsChampRegistered = false;
            this.EntityPM.ZeroChampNeedsRegistration = false;
            this.EntityPM.TenantZeroAirlineGLSHKFFR = false;
            this.EntityPM.CarrierIsGLSHKRegistered = false;
            this.EntityPM.ZeroGLSHKNeedsRegistration = false;
            this.EntityPM.TenantZeroIsManagingProduct = false;
            this.EntityPM.TenantZeroIsProductMandatory = false;
            this.EntityPM.ZeroIsDescOfGoodsFromList = false;
        }

        else {
            this.GetCard("M");
            this.GetAirline();
        }

        this.SetUIProperties_Carriers();
        this.FireWizardEvent();
    }

    get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    set AirlinePrefix(newValue: string) {
        if (this.EntityPM.AirlinePrefix != newValue) {
            this.EntityPM.AirlinePrefix = newValue;

            this.LongMaster = this.ComputeLongMaster();
            this.ValidateMasterField();
            this.FireWizardEvent();
        }
    }

    get Master() { return this.EntityPM.Master; }
    set Master(newValue: string) {
        if (this.EntityPM.Master != newValue) {
            this.EntityPM.Master = newValue;

            this.LongMaster = this.ComputeLongMaster();

            this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
            this.ValidateMasterField();
            this.FireWizardEvent();
            this.SetUIProperties_Carriers();
        }
    }

    get MainCarriageSpaceAllocationCode() { return this.EntityPM.MainCarriageSpaceAllocationCode; }
    set MainCarriageSpaceAllocationCode(newValue: string) {
        if (this.EntityPM.MainCarriageSpaceAllocationCode != newValue) {
            this.EntityPM.MainCarriageSpaceAllocationCode = newValue;
            this.SetUIProperties_Others();
        }
    }

    get MainCarriageAllotmentIdentification() { return this.EntityPM.MainCarriageAllotmentIdentification; }
    set MainCarriageAllotmentIdentification(newValue: string) {
        if (this.EntityPM.MainCarriageAllotmentIdentification != newValue) {
            this.EntityPM.MainCarriageAllotmentIdentification = newValue;
            this.SetUIProperties_Others();
        }
    }

    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(newValue: string) {
        if (this.EntityPM.AccountNumber != newValue) {
            this.EntityPM.AccountNumber = newValue;
        }
    }

    get MainCarriageCarrierPrefix() { return this.EntityPM.MainCarriageCarrierPrefix; }
    set MainCarriageCarrierPrefix(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierPrefix != newValue) {
            this.EntityPM.MainCarriageCarrierPrefix = newValue;

            this.SetFirstFlightFeild();
            this.SetUIProperties_Carriers();
        }
    }

    get MainCarriageCarrierNumber() { return this.EntityPM.MainCarriageCarrierNumber; }
    set MainCarriageCarrierNumber(newValue: string) {
        if (this.EntityPM.MainCarriageCarrierNumber != newValue) {
            this.EntityPM.MainCarriageCarrierNumber = newValue;

            this.SetFirstFlightFeild();
            this.SetUIProperties_Carriers();
        }
    }

    get MainCarriageETD() { return this.EntityPM.MainCarriageETD; }
    set MainCarriageETD(newValue: Date) {
        if (this.EntityPM.MainCarriageETD != newValue) {
            this.EntityPM.MainCarriageETD = newValue;

            this.SetUIProperties_Others();
        }
    }

    //Interline
    public IsInterlineAdded: boolean = false;
    AddInterlineClicked() {
        this.IsInterlineAdded = true;
    }

    get InterlineId() { return this.EntityPM.InterlineId; }
    set InterlineId(newValue: string) {
        if (this.EntityPM.InterlineId != newValue) {
            this.EntityPM.InterlineId = newValue;

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.IsManualDataEntryVisible = false;
                this.IsManualDataEntryVisible_Via1 = true;
            }
            else {
                this.IsManualDataEntryVisible_Via1 = false;
            }
            
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
            this.myAirlineService.getSingleFromCache(myAirlineId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
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

                    this.SetUIProperties_Carriers();
                }
            });
        }       
    }

    //Transshipment 1
    get Transshipment1CarrierId() { return this.EntityPM.Transshipment1CarrierId; }
    set Transshipment1CarrierId(newValue: string) {
        if (this.EntityPM.Transshipment1CarrierId != newValue) {
            this.EntityPM.Transshipment1CarrierId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.Transshipment1CarrierPrefix = null;
                this.Transshipment1CarrierNumber = null;

                this.SetUIProperties_Carriers();
            }

            else {
                this.GetCard("T1");
            }
        }
    }

    get Transshipment1CarrierPrefix() { return this.EntityPM.Transshipment1CarrierPrefix; }
    set Transshipment1CarrierPrefix(newValue: string) {
        if (this.EntityPM.Transshipment1CarrierPrefix != newValue) {
            this.EntityPM.Transshipment1CarrierPrefix = AppTool.IsNullOrEmpty(newValue) ? newValue : newValue.trim();
        }
    }

    get Transshipment1CarrierNumber() { return this.EntityPM.Transshipment1CarrierNumber; }
    set Transshipment1CarrierNumber(newValue: string) {
        if (this.EntityPM.Transshipment1CarrierNumber != newValue) {
            this.EntityPM.Transshipment1CarrierNumber = newValue;

            this.SetUIProperties_Carriers();
        }
    }

    get Transshipment1ETD() { return this.EntityPM.Transshipment1ETD; }
    set Transshipment1ETD(newValue: Date) {
        if (this.EntityPM.Transshipment1ETD != newValue) {
            this.EntityPM.Transshipment1ETD = newValue;

            this.SetUIProperties_Others();
        }
    }

    get Transshipment1SpaceAllocationCode() { return this.EntityPM.Transshipment1SpaceAllocationCode; }
    set Transshipment1SpaceAllocationCode(newValue: string) {
        if (this.EntityPM.Transshipment1SpaceAllocationCode != newValue) {
            this.EntityPM.Transshipment1SpaceAllocationCode = newValue;
            this.SetUIProperties_Others();
        }
    }

    get Transshipment1AllotmentIdentification() { return this.EntityPM.Transshipment1AllotmentIdentification; }
    set Transshipment1AllotmentIdentification(newValue: string) {
        if (this.EntityPM.Transshipment1AllotmentIdentification != newValue) {
            this.EntityPM.Transshipment1AllotmentIdentification = newValue;
            this.SetUIProperties_Others();
        }
    }

    //Transshipment 2
    get Transshipment2CarrierId() { return this.EntityPM.Transshipment2CarrierId; }
    set Transshipment2CarrierId(newValue: string) {
        if (this.EntityPM.Transshipment2CarrierId != newValue) {
            this.EntityPM.Transshipment2CarrierId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.Transshipment2CarrierPrefix = null;
                this.Transshipment2CarrierNumber = null;

                this.SetUIProperties_Carriers();
            }

            else {
                this.GetCard("T2");
            }
        }
    }

    get Transshipment2CarrierPrefix() { return this.EntityPM.Transshipment2CarrierPrefix; }
    set Transshipment2CarrierPrefix(newValue: string) {
        if (this.EntityPM.Transshipment2CarrierPrefix != newValue) {
            this.EntityPM.Transshipment2CarrierPrefix = AppTool.IsNullOrEmpty(newValue) ? newValue : newValue.trim();
        }
    }

    get Transshipment2CarrierNumber() { return this.EntityPM.Transshipment2CarrierNumber; }
    set Transshipment2CarrierNumber(newValue: string) {
        if (this.EntityPM.Transshipment2CarrierNumber != newValue) {
            this.EntityPM.Transshipment2CarrierNumber = newValue;

            this.SetUIProperties_Carriers();
        }
    }

    get Transshipment2ETD() { return this.EntityPM.Transshipment2ETD; }
    set Transshipment2ETD(newValue: Date) {
        if (this.EntityPM.Transshipment2ETD != newValue) {
            this.EntityPM.Transshipment2ETD = newValue;

            this.SetUIProperties_Others();
        }
    }

    get Transshipment2SpaceAllocationCode() { return this.EntityPM.Transshipment2SpaceAllocationCode; }
    set Transshipment2SpaceAllocationCode(newValue: string) {
        if (this.EntityPM.Transshipment2SpaceAllocationCode != newValue) {
            this.EntityPM.Transshipment2SpaceAllocationCode = newValue;
            this.SetUIProperties_Others();
        }
    }

    get Transshipment2AllotmentIdentification() { return this.EntityPM.Transshipment2AllotmentIdentification; }
    set Transshipment2AllotmentIdentification(newValue: string) {
        if (this.EntityPM.Transshipment2AllotmentIdentification != newValue) {
            this.EntityPM.Transshipment2AllotmentIdentification = newValue;
            this.SetUIProperties_Others();
        }
    }

    //Ports
    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(newValue: string) {
        if (this.EntityPM.MainCarriageFromPortId != newValue) {
            this.EntityPM.MainCarriageFromPortId = newValue;

            this.BuildLegs();
            this.FireWizardEvent();
            this.BuildRoutingField();
            this.SetUIProperties_Ports();            
        }
    }

    get Transshipment1FromPortId() { return this.EntityPM.Transshipment1FromPortId; }
    set Transshipment1FromPortId(newValue: string) {
        if (this.EntityPM.Transshipment1FromPortId != newValue) {
            this.EntityPM.Transshipment1FromPortId = newValue;

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.Transshipment1SpaceAllocationCode = "NN";
            }

            this.BuildLegs();
        }
    }

    get Transshipment2FromPortId() { return this.EntityPM.Transshipment2FromPortId; }
    set Transshipment2FromPortId(newValue: string) {
        if (this.EntityPM.Transshipment2FromPortId != newValue) {
            this.EntityPM.Transshipment2FromPortId = newValue;

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.Transshipment2SpaceAllocationCode = "NN";
            }

            this.BuildLegs();
        }
    }

    get MainCarriageFinalDestinationPortId() { return this.EntityPM.MainCarriageFinalDestinationPortId; }
    set MainCarriageFinalDestinationPortId(newValue: string) {
        if (this.EntityPM.MainCarriageFinalDestinationPortId != newValue) {
            if (this.EntityPM.Transshipment2ToPortId != null) {
                this.EntityPM.Transshipment2ToPortId = newValue;
            }
            else if (this.EntityPM.Transshipment1ToPortId != null) {
                this.EntityPM.Transshipment1ToPortId = newValue;
            }
            else {
                this.EntityPM.MainCarriageToPortId = newValue;
            }

            this.EntityPM.MainCarriageFinalDestinationPortId = newValue;

            this.BuildLegs();
            this.FireWizardEvent();
            this.BuildRoutingField();
            this.SetUIProperties_Ports();
        }
    }

    BuildLegs() {
        /*[1]*/
        if (this.EntityPM.Transshipment1FromPortId != null && this.EntityPM.Transshipment2FromPortId == null) {
            this.EntityPM.Transshipment1ToPortId = this.MainCarriageFinalDestinationPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.Transshipment1FromPortId;

            this.EntityPM.Transshipment2ToPortId = null;
            this.EntityPM.Transshipment2FromPortId = null;
            this.EntityPM.Transshipment2CarrierId = null;
            this.EntityPM.Transshipment2CarrierNumber = null;
            this.EntityPM.Transshipment2ETD = null;
        }

        /*[2]*/
        else if (this.EntityPM.Transshipment1FromPortId == null && this.EntityPM.Transshipment2FromPortId != null) {
            this.EntityPM.Transshipment2ToPortId = this.MainCarriageFinalDestinationPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.Transshipment2FromPortId;

            this.EntityPM.Transshipment1ToPortId = null;
            this.EntityPM.Transshipment1FromPortId = null;
            this.EntityPM.Transshipment1CarrierId = null;
            this.EntityPM.Transshipment1CarrierNumber = null;
            this.EntityPM.Transshipment1ETD = null;
        }

        /*[1:2]*/
        else if (this.EntityPM.Transshipment1FromPortId != null && this.EntityPM.Transshipment2FromPortId != null) {
            this.EntityPM.Transshipment2ToPortId = this.MainCarriageFinalDestinationPortId;
            this.EntityPM.Transshipment1ToPortId = this.EntityPM.Transshipment2FromPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.Transshipment1FromPortId;
        }

        /*[0]*/
        else if (this.EntityPM.Transshipment1FromPortId == null && this.EntityPM.Transshipment2FromPortId == null) {
            this.EntityPM.MainCarriageToPortId = this.MainCarriageFinalDestinationPortId;

            this.EntityPM.Transshipment1ToPortId = null;
            this.EntityPM.Transshipment1FromPortId = null;
            this.EntityPM.Transshipment1CarrierId = null;
            this.EntityPM.Transshipment1CarrierNumber = null;
            this.EntityPM.Transshipment2ToPortId = null;
            this.EntityPM.Transshipment2FromPortId = null;
            this.EntityPM.Transshipment2CarrierId = null;
            this.EntityPM.Transshipment2CarrierNumber = null;
            this.EntityPM.Transshipment1ETD = null;
            this.EntityPM.Transshipment2ETD = null;
        }

        this.LoadPortsData();
        this.SetUIProperties_Ports();
    }

    LoadPortsData() {
        this.LoadMainCarriageFromPort();
        this.LoadMainCarriageToPort();
        this.LoadTransshipment1FromPort();
        this.LoadTransshipment1ToPort();
        this.LoadTransshipment2FromPort();
        this.LoadTransshipment2ToPort();
    }
    LoadMainCarriageFromPort() {
        var myPortId = this.EntityPM.MainCarriageFromPortId;

        if (myPortId == null) {
            this.EntityPM.MainFromPortCode = null;
            this.EntityPM.MainFromPortName = null;
            this.EntityPM.MainFromPortCountryCode = null;
            this.EntityPM.MainFromPortCountryName = null;

            this.BuildRoutingField();
        }

        else {
            var myService: PortListService = new PortListService();
            myService.getSingle(myPortId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: PortList = myResponse.Result;
                    if (list != null) {
                        this.EntityPM.MainFromPortCode = list.Code;
                        this.EntityPM.MainFromPortName = list.EnglishName;
                        this.EntityPM.MainFromPortCountryCode = list.CountryCode;
                        this.EntityPM.MainFromPortCountryName = list.CountryName;

                        this.BuildRoutingField();
                    }
                }
            });
        }
    }
    LoadMainCarriageToPort() {
        var myPortId = this.EntityPM.MainCarriageToPortId;

        if (myPortId == null) {
            this.EntityPM.MainToPortCode = null;
            this.EntityPM.MainToPortName = null;
            this.EntityPM.MainToPortCountryCode = null;
            this.EntityPM.MainToPortCountryName = null;

            this.BuildRoutingField();
        }

        else {
            var myService: PortListService = new PortListService();
            myService.getSingle(myPortId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: PortList = myResponse.Result;

                    if (list != null) {
                        this.EntityPM.MainToPortCode = list.Code;
                        this.EntityPM.MainToPortName = list.EnglishName;
                        this.EntityPM.MainToPortCountryCode = list.CountryCode;
                        this.EntityPM.MainToPortCountryName = list.CountryName;

                        this.BuildRoutingField();
                    }
                }
            });
        }
    }
    LoadTransshipment1FromPort() {
        var myPortId = this.EntityPM.Transshipment1FromPortId;

        if (myPortId == null) {
            this.EntityPM.Trans1FromPortCode = null;
            this.EntityPM.Trans1FromPortName = null;
            this.EntityPM.Trans1FromPortCountryCode = null;
            this.EntityPM.Trans1FromPortCountryName = null;

            this.BuildRoutingField();
        }

        else {
            var myService: PortListService = new PortListService();
            myService.getSingle(myPortId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: PortList = myResponse.Result;

                    if (list != null) {
                        this.EntityPM.Trans1FromPortCode = list.Code;
                        this.EntityPM.Trans1FromPortName = list.EnglishName;
                        this.EntityPM.Trans1FromPortCountryCode = list.CountryCode;
                        this.EntityPM.Trans1FromPortCountryName = list.CountryName;

                        this.BuildRoutingField();
                    }
                }
            });
        }
    }
    LoadTransshipment1ToPort() {
        var myPortId = this.EntityPM.Transshipment1ToPortId;

        if (myPortId == null) {
            this.EntityPM.Trans1ToPortCode = null;
            this.EntityPM.Trans1ToPortName = null;
            this.EntityPM.Trans1ToPortCountryCode = null;
            this.EntityPM.Trans1ToPortCountryName = null;

            this.BuildRoutingField();
        }

        else {
            var myService: PortListService = new PortListService();
            myService.getSingle(myPortId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: PortList = myResponse.Result;

                    if (list != null) {
                        this.EntityPM.Trans1ToPortCode = list.Code;
                        this.EntityPM.Trans1ToPortName = list.EnglishName;
                        this.EntityPM.Trans1ToPortCountryCode = list.CountryCode;
                        this.EntityPM.Trans1ToPortCountryName = list.CountryName;

                        this.BuildRoutingField();
                    }
                }
            });
        }
    }
    LoadTransshipment2FromPort() {
        var myPortId = this.EntityPM.Transshipment2FromPortId;

        if (myPortId == null) {
            this.EntityPM.Trans2FromPortCode = null;
            this.EntityPM.Trans2FromPortName = null;
            this.EntityPM.Trans2FromPortCountryCode = null;
            this.EntityPM.Trans2FromPortCountryName = null;
        }

        else {
            var myService: PortListService = new PortListService();
            myService.getSingle(myPortId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: PortList = myResponse.Result;

                    if (list != null) {
                        this.EntityPM.Trans2FromPortCode = list.Code;
                        this.EntityPM.Trans2FromPortName = list.EnglishName;
                        this.EntityPM.Trans2FromPortCountryCode = list.CountryCode;
                        this.EntityPM.Trans2FromPortCountryName = list.CountryName;
                    }
                }
            });
        }
    }
    LoadTransshipment2ToPort() {
        var myPortId = this.EntityPM.Transshipment2ToPortId;

        if (myPortId == null) {
            this.EntityPM.Trans2ToPortCode = null;
            this.EntityPM.Trans2ToPortName = null;
            this.EntityPM.Trans2ToPortCountryCode = null;
            this.EntityPM.Trans2ToPortCountryName = null;

            this.BuildRoutingField();
        }

        else {
            var myService: PortListService = new PortListService();
            myService.getSingle(myPortId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: PortList = myResponse.Result;

                    if (list != null) {
                        this.EntityPM.Trans2ToPortCode = list.Code;
                        this.EntityPM.Trans2ToPortName = list.EnglishName;
                        this.EntityPM.Trans2ToPortCountryCode = list.CountryCode;
                        this.EntityPM.Trans2ToPortCountryName = list.CountryName;

                        this.BuildRoutingField();
                    }
                }
            });
        }
    }

    ComputeLongMaster() {
        var myField: string = null;

        if (this.EntityPM.TransportModeCode == "A") {
            myField = this.AirlinePrefix + "-";
            if (!AppTool.IsNullOrEmpty(this.Master)) {
                myField = myField + this.Master;
            }
        }

        else {
            myField = this.Master;
        }

        return myField;
    }

    public MasterErrorMessage: string = null;
    public IsMasterValid: boolean = true;
    ValidateMasterField() {
        if (AppTool.IsNullOrEmpty(this.Master)) {
            this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
        }

        else {
            this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");

            var myMasterFieldError = AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeCode, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);

            if (!AppTool.IsNullOrEmpty(myMasterFieldError)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myMasterFieldError);

                this.IsMasterValid = false;
                this.MasterErrorMessage = myMasterFieldError;
            }

            else {
                if (AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
                }

                else {
                    this.ValidateMasterStack();
                    this.ValidateMasterFieldIsUsed();
                }
            }
        }
    }
    private ValidateMasterFieldIsUsed() {
        var myBookingDomainService = new BookingDomainService();

        if (!AppTool.IsNullOrEmpty(this.Master)) {
            var myMasterFieldError: string = "";

            myBookingDomainService.ValidateBookingMasterFieldExistance(this.EntityPM.Id, this.EntityPM.Master, this.EntityPM.AirlinePrefix, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode, this.EntityPM.IsCancelled, this.EntityPM.Tenant).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    myMasterFieldError = myResponse.Result;

                    if (AppTool.IsNullOrEmpty(myMasterFieldError)) {
                        this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
                    }

                    else {
                        this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myMasterFieldError);
                        this.IsMasterValid = false;
                        this.MasterErrorMessage = myMasterFieldError;
                    }
                }
            }
                , error => {
                    this.MasterErrorMessage = error;
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
                        this.StackDomainService.GetMAWBStackPMByNumber(+this.Master).subscribe(myResult => {
                            var myResponse: ServiceResponse = myResult;

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
                                                    this.SetMAWBAirline();
                                                    this.isGetFromStock = true;
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

    private BuildRoutingField() {
        var fromCode = "";
        var toCode = "";

        if (!AppTool.IsNullOrEmpty(this.EntityPM.MainFromPortCode)) {
            fromCode = this.EntityPM.MainFromPortCode;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Trans2ToPortCode)) {
            toCode = this.EntityPM.Trans2ToPortCode;
        }
        else if (!AppTool.IsNullOrEmpty(this.EntityPM.Trans1ToPortCode)) {
            toCode = this.EntityPM.Trans1ToPortCode;
        }
        else if (!AppTool.IsNullOrEmpty(this.EntityPM.MainToPortCode)) {
            toCode = this.EntityPM.MainToPortCode;
        }

        this.EntityPM.Routing = fromCode + " , " + toCode;
    }
    private SetFirstFlightFeild() {
        if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierNumber) && !AppTool.IsNullOrEmpty(this.MainCarriageCarrierPrefix)) {
            this.EntityPM.FirstFlight = this.MainCarriageCarrierPrefix + this.MainCarriageCarrierNumber;
        }
    }

    //Get Card
    private mainCarriageCarrier: CardList = null;
    private GetCard(myCode: string) {
        var myCardId = null;

        switch (myCode) {
            case "M":
                {
                    myCardId = this.MainCarriageCarrierId;
                    break;
                }
            case "T1":
                {
                    myCardId = this.Transshipment1CarrierId;
                    break;
                }
            case "T2":
                {
                    myCardId = this.Transshipment2CarrierId;
                    break;
                }
        }

        this.myCardService.getSingle(myCardId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list: CardList = myResponse.Result;
                    this.mainCarriageCarrier = list;

                    if (list != null) {
                        switch (myCode) {
                            case "M":
                                {
                                    this.MainCarriageCarrierPrefix = list.Code;
                                    this.EntityPM.MainCarriageCarrierName = list.EnglishName;
                                    this.EntityPM.MainCarriageCarrierCode = list.Code;
                                    this.AccountNumber = list.AirlineAccountNumber;

                                    //this.LoadMessageRules(list.Code);

                                    break;
                                }

                            case "T1": { this.Transshipment1CarrierPrefix = list.Code; break; }
                            case "T2": { this.Transshipment2CarrierPrefix = list.Code; break; }
                            default: { break; }
                        }
                    }
                }
            }

            this.SetUIProperties_Carriers();
        });
    }

    private GetAirline() {
        if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            this.myAirlineService.getSingleFromCache(this.MainCarriageCarrierId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    var list: AirlineList = myResponse.Result;

                    if (list != null) {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
                            this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                            this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;

                            var myPrefix = null;
                            if (!AppTool.IsNullOrEmpty(list.Prefix)) {
                                myPrefix = list.Prefix;
                                myPrefix = AppTool.PadLeft(myPrefix, 3, '0');
                            }

                            this.AirlinePrefix = myPrefix;
                        }

                        this.EntityPM.CarrierIsChampRegistered = list.IsChampRegistered;
                        this.EntityPM.CarrierIsGLSHKRegistered = list.IsGLSHKRegistered;

                        this.GetTenantZeroAirline(list.Code);

                    }
                }
            });
        }
    }
    private GetTenantZeroAirline(airlineCode: string) {
        this.myPartnersDomainService.GetAirlineByCode(airlineCode, 0).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var airlinePM: AirlinePM = myResponse.Result;

                if (airlinePM != null) {
                    this.EntityPM.TenantZeroAirlineId = airlinePM.Id;
                    this.EntityPM.TenantZeroAirlineTTY = airlinePM.TTY;
                    this.EntityPM.TenantZeroAirlinePIMA = airlinePM.GLSHKPIMA;
                    this.EntityPM.TenantZeroAirlineChampFFR = airlinePM.ChampFFRFFA;
                    this.EntityPM.TenantZeroAirlineChampFVR = airlinePM.ChampFVRFVA;
                    this.EntityPM.ZeroChampNeedsRegistration = airlinePM.ChampNeedsRegistration;
                    this.EntityPM.TenantZeroAirlineGLSHKFFR = airlinePM.GLSHKFFRFFA;
                    this.EntityPM.TenantZeroAirlineGLSHKFVR = airlinePM.GLSHKFVRFVA;
                    this.EntityPM.ZeroGLSHKNeedsRegistration = airlinePM.GLSHKNeedsRegistration;
                    this.EntityPM.TenantZeroIsManagingProduct = airlinePM.IsManagingProduct;
                    this.EntityPM.TenantZeroIsProductMandatory = airlinePM.IsProductMandatory;
                    this.EntityPM.ZeroIsDescOfGoodsFromList = airlinePM.IsDescriptionOfGoodsFromList;

                    if (!AppTool.IsNullOrEmpty(this.EntityPM.DescriptionOfGoodsId)) {
                        this.EntityPM.DescriptionOfGoodsId = null;
                        this.EntityPM.DescriptionOfGoodsService = null;
                        this.EntityPM.DescriptionOfGoods = null;
                    }
                }
            }
        });
    }

    //Stock
    AddStockClicked(airlineId: string) {
        if (!AppTool.IsNullOrEmpty(airlineId)) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit Airline";
            logWindow.ShowEditComponent(airlineId, "Airline", "ALST");
        }
    }

    private myOldMAWBStackNumber: string;
    private isGetFromStock: boolean = false;
    GetStockClicked() {
        var isValid = this.Wizard.ValidateBooking();
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
                        this.myOldMAWBStackNumber = this.EntityPM.MAWBStackNumber;
                        this.EntityPM.MAWBTakenFromStack = true;
                        this.EntityPM.MAWBStackNumber = AppTool.PadLeft(stackNumber.toString(), 8, '0');
                        this.isGetFromStock = true;
                        this.Save();
                    }
                }
            });
        }
    }
    ReturnStockClicked() {
        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            var isValid = this.Wizard.ValidateBooking();
            if (isValid) {
                this.SetMAWBAirline();
                this.EntityPM.MAWBReturnedToStack = true;
                this.EntityPM.MAWBStackNumber = this.Master;
                this.isGetFromStock = false;
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

    private isManualDataEntryEnabled: boolean = false;
    get IsManualDataEntryEnabled() { return this.isManualDataEntryEnabled; }
    set IsManualDataEntryEnabled(enabled: boolean) {
        this.isManualDataEntryEnabled = enabled;

        if (this.isManualDataEntryEnabled) {
            this.SetUIProperties();
        }
    }

    private isManualDataEntryEnabled_Via1: boolean = false;
    get IsManualDataEntryEnabled_Via1() { return this.isManualDataEntryEnabled_Via1; }
    set IsManualDataEntryEnabled_Via1(enabled: boolean) {
        this.isManualDataEntryEnabled_Via1 = enabled;

        if (this.isManualDataEntryEnabled_Via1) {
            this.SetUIProperties();
        }
    }

    public IsManualDataEntryVisible: boolean = false;
    public IsManualDataEntryVisible_Via1: boolean = false;
    AllowManualDataEntry(leg: string) {
        if (leg == "M") {
            this.IsManualDataEntryEnabled = true;
        }

        else {
            this.IsManualDataEntryEnabled_Via1 = true;
        }
    }

    FindFlightsClicked(isMainLeg: boolean) {
        this.IsManualDataEntryEnabled = false;
        this.IsManualDataEntryEnabled_Via1 = false;
        this.IsManualDataEntryVisible = false;
        this.IsManualDataEntryVisible_Via1 = false;

        var args = new FlightsSchedulesArgs();
        args.BookingPM = this.EntityPM;
        args.IsMainLeg = isMainLeg;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = args;
        logWindow.Title = "Flights Schedules";
        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(response=> {
            logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FlightsSchedulesComponent');
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe((event: any) => {
                    if (args.IsFlightSelected) {
                        this.Validate();
                        this.FireWizardEvent();
                        this.SetUIProperties();
                    }

                    if (logWindow.WindowArgs.IsClosedFomProgress || logWindow.WindowArgs.IsCancelledFomProgress || comp.IsNoFlightsResult) {
                        if (this.EntityPM.BookingStatusCode != "AWB") {

                            if (AppTool.IsNullOrEmpty(this.InterlineId)) {
                                this.IsManualDataEntryVisible = true;
                            }

                            else {
                                this.IsManualDataEntryVisible_Via1 = true;
                            }
                        }
                    }
                });
            });
        });
    }
}