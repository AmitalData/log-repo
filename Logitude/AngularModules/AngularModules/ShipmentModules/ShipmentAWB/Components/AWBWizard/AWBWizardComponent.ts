declare var window: any;
import { Component, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, FormatTool} from '../../../../Infrastructure/Tools';
import {ShipmentTool, AWBHelper, AWBCCSValidator} from '../../../../Shipment/Tools';
import {AWBWizardArgs, SendAWBArgs} from '../../../../Shipment/Args';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBOCIPM} from '../../../../Shipment/EntityPMs/AWBOCIPM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {StateList} from '../../../../Common/EntityLists/StateList';
import {CCSWebService, AWBPrintResult} from '../../../../Infrastructure/Services/WebServices/CCSWebService';
import {DocumentOutPM} from '../../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypeList} from '../../../../Common/EntityLists/DocumentTypeList';
import {DocsOutDataViewModel} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import {DocumentTypeListExtendedService} from '../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import {DocumentOutPMService} from '../../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {DocumentTypePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {StateListService} from '../../../../Common/Services/StandardLists/StateListService';
import {AirlineListService} from '../../../../Common/Services/StandardLists/AirlineListService';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {PartnersDomainService, AirlineMessagingRuleList} from '../../../../Common/Services/PartnersDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {BranchList} from '../../../../Common/EntityLists/BranchList';
import { BranchListService } from '../../../../Common/Services/StandardLists/BranchListService';
import { IATACodeList } from '../../../../Infrastructure/EntityLists/IATACodeList';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { DueTypeList } from '../../../../Common/EntityLists/DueTypeList';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { MeasurementList } from '../../../../Common/EntityLists/MeasurementList';
import { IATACodeListService } from '../../../../Infrastructure/Services/StandardLists/IATACodeListService';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { DueTypeListService } from '../../../../Common/Services/StandardLists/DueTypeListService';
import { ChargesTypeListService } from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import { MeasurementListService } from '../../../../Common/Services/StandardLists/MeasurementListService';
import { ShipmentAWBPrintOnlyPM } from '../../../../Shipment/EntityPMs/ShipmentAWBPrintOnlyPM';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    selector: 'AWBWizardComponent',
    templateUrl: './AWBWizardComponent.html',
    providers: [EntityArgs, DocumentTypeListExtendedService, DocumentOutPMService, DocumentTypePMExtendedService]
})

export class AWBWizardComponent implements AfterViewInit{
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter < boolean>();
    @Output() SaveCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    public TenantPM: TenantPM;
    public EntityPM: ShipmentPM;
    public DataContext: AWBWizardComponent = this;
    public IsFHL: boolean = false;
    public IsFWB: boolean = false;
    public ObjectTableName: string;
    public ShipmentLevelCode: string;
    public IsNewEntity: boolean = false;
    public WindowArgs: AWBWizardArgs;
    private myPartnersDomainService: PartnersDomainService;
    public ValidationErrorsList: string[] = [];
    public ValidationWarningsList: string[] = [];
    public IsValidationSingleLine: boolean = false;
    public IsImportWizard: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public _documentTypeListExtendedService: DocumentTypeListExtendedService, public _documentOutPMService: DocumentOutPMService, public _documentTypePMService: DocumentTypePMExtendedService) {
        this.TenantPM = SessionLocator.TenantPM;
        this.myPartnersDomainService = new PartnersDomainService();

        ServiceLocator.SendTotangoUserActivity("AWBWizard", "View");

    }

    SetWindowArgs(windowArgs: AWBWizardArgs) {
        this.WindowArgs = windowArgs;
        this.InitializeWizard();
    }

    private isViewInited = false;
    ngAfterViewInit() {
        this.isViewInited = true;
        this.InitializeComponent();
    }

    public YellowIconHelpMessage: string;
    public SendAWBLabel: string = null;
    public IsSendVisible: boolean = false;
    public IsSendFHLsVisible: boolean = false;
    public IsTabVisible_OVE: boolean = false;
    public IsTabVisible_HAW: boolean = false;
    public IsTabVisible_RAD: boolean = false;
    public IsTabVisible_OTP: boolean = false;
    targetObjectTableId: string;
    public CCSTypeCode: string = "CHAMP";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private InitializeWizard() {
        if (this.WindowArgs != null) {

            this.IsNewEntity = this.WindowArgs.IsNewEntity;
            this.ShipmentLevelCode = this.WindowArgs.ShipmentLevelCode;
            this.ObjectTableName = this.ShipmentLevelCode == "C" ? "Master" : "Shipment";
           
            this.targetObjectTableId = window.ObjectTables.filter(x => x.Name === this.ObjectTableName && (x.Tenant == this.TenantPM.Id || x.Tenant == 0))[0].Id;

            this.IsFHL = this.ShipmentLevelCode == "H" ? true : false;
            this.IsFWB = !this.IsFHL;

            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "SENDAWB")) {
                this.IsSendVisible = true;
                this.SendAWBLabel = this.ShipmentLevelCode == "H" ? "Send FHL" : "Send FWB";
                this.IsSendFHLsVisible = this.ShipmentLevelCode == "C" ? true : false;

                if (!this.IsNewEntity) {
                    this.IsTabVisible_OVE = true;
                }
            }

            this.YellowIconHelpMessage = this.ShipmentLevelCode == "H" ? "Required fields for sending FHL" : "Required fields for sending FWB";

            this.IsTabVisible_HAW = this.ShipmentLevelCode == "C" ? true : false;
            this.IsTabVisible_OTP = this.IsFWB ? true : false;
            this.IsTabVisible_RAD = this.IsFWB && this.TenantPM.RegulatedAgentRegimeActivated ? true : false;

            if (this.WindowArgs.IsNewEntity) {
                this.CreateShipment();
            }

            else {
                this.EntityPM = this.WindowArgs.EntityPM;
            }

            this.entityArgs.EntityPM = this.EntityPM;

            if (this.EntityPM.DirectionId == "I") {
                this.IsImportWizard = true;
            }

            this.entityArgs.ObjectTableName = this.ObjectTableName;
            this.BuildPrintDocuments();
            this.SetCargonautDEXXVisibility();
            this.SetSelectedTab();
            this.SetMoreButtons();
        }
    }

    public PrintToggleButtonTop: string = "-170px";
    public PrintDocumentsList: DocumentTypeClass[] = [];
    documentTypeCode: string = "";
    documentTypeName: string;
    isCurrentDocsOutLoaded: boolean;
    selectedDocumentTypeClass: DocumentTypeClass;

    private BuildPrintDocuments() {
        this.PrintDocumentsList = [];
        switch (this.ShipmentLevelCode) {
            case "D": {
                this.PrintDocumentsList.push(new DocumentTypeClass("740", "Plain Paper AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("714", "Plain Paper HAWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740L", "AWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740HL", "HAWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740PP", "Neutral AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("714PP", "Neutral HAWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("785A", "Cargo Manifest"));
                this.PrintToggleButtonTop = "-170px";
                break;
            }

            case "H": {
                this.PrintDocumentsList.push(new DocumentTypeClass("714", "Plain Paper HAWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740HL", "HAWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("714PP", "Neutral HAWB"));
                this.PrintToggleButtonTop = "-80px";
                break;
            }

            case "C": {
                this.PrintDocumentsList.push(new DocumentTypeClass("740", "Plain Paper AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740L", "AWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740PP", "Neutral AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("785A", "Cargo Manifest"));
                this.PrintToggleButtonTop = "-100px";
                break;
            }
        }
    }

    public IsDEXXVisibile: boolean = false;
    public IsDEXXButtonVisibile: boolean = false;
    public IsDEXXToggleButtonVisibile: boolean = false;
    public IsCargonautVisibile: boolean = false;
    public IsCargonautButtonVisibile: boolean = false;
    public IsCargonautToggleButtonVisibile: boolean = false;
    public SetCargonautDEXXVisibility() {
        this.IsDEXXVisibile = false;
        this.IsDEXXButtonVisibile = false;
        this.IsDEXXToggleButtonVisibile = false;
        this.IsCargonautVisibile = false;
        this.IsCargonautButtonVisibile = false;
        this.IsCargonautToggleButtonVisibile = false;

        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "SENDAWB")) {
            switch (this.EntityPM.MainCarriageFromPortCode) {
                case "SPL":
                case "AMS":
                case "RTM":
                case "MST":
                    {
                        if (this.ShipmentLevelCode == "C") {
                            this.IsCargonautToggleButtonVisibile = true;
                        }

                        else {
                            this.IsCargonautButtonVisibile = true;
                        }

                        this.IsCargonautVisibile = true;

                        break;
                    }

                case "LGG":
                case "BRU":
                    {
                        if (this.ShipmentLevelCode == "C") {
                            this.IsDEXXToggleButtonVisibile = true;
                        }

                        else {
                            this.IsDEXXButtonVisibile = true;
                        }

                        this.IsDEXXVisibile = true;

                        break;
                    }
            }
        }
    }

    private InitializeComponent() {
        if (this.WindowArgs != null && this.isViewInited) {

            var airlineCode = this.EntityPM.MainCarriageCarrierCode;
            if (this.WindowArgs.IsCreatingHouseFromMaster) {
                airlineCode = this.WindowArgs.MasterPM.MainCarriageCarrierCode;
            }

            this.SelectionChanged();
            this.ValidateAllTabs();
            this.LoadAirlineRules(airlineCode);
            this.LoadAllowedAirline();
            this.LoadCurrencyRates();
            this.LoadAllStates();
        }
    }

    public AllStates: StateList[] = [];
    private myStateListService: StateListService;
    private LoadAllStates() {
        if (this.myStateListService == null) {
            this.myStateListService = new StateListService();
            
        }

        this.myStateListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.AllStates = myResponse.Result;
                    this.ValidateScreen_PAR();
                }
            }
        });
    }

    private CreateShipment() {
        var todayDate = DateTool.GetCurrentDateTimeAsUtc();

        this.EntityPM = new ShipmentPM();
        this.EntityPM.DirectionId = "E";
        this.EntityPM.TransportModeId = "A";
        this.EntityPM.ShipmentLevelCode = this.ShipmentLevelCode;
        this.EntityPM.FHLStatusCode = "NSEN";
        this.EntityPM.FWBStatusCode = "NSEN";
        this.EntityPM.ManifestStatusCode = "NSEN";
        this.EntityPM.FHLStatusName = "Not Sent";
        this.EntityPM.FWBStatusName = "Not Sent";
        this.EntityPM.IsOperationalClosed = false;
        this.EntityPM.CreateDateTime = todayDate;
        this.EntityPM.LastUpdateDate = todayDate;
        this.EntityPM.StatusDate = todayDate;
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.AWBCurrencyId = this.TenantPM.FreightCurrencyId;
        this.EntityPM.ProfitCurrencyId = this.TenantPM.ProfitCurrencyId;
        this.EntityPM.VolumeUnitCode = this.TenantPM.VolumeUnitCode;
        this.EntityPM.DimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        this.EntityPM.ChargeableWeightUnitCode = this.TenantPM.ChargeableWeightUnitCode;
        this.EntityPM.OtherPrepaidCollectId = this.TenantPM.ExportOtherPrepaidCollectId;
        this.EntityPM.CASSCode = this.TenantPM.CASSCode;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
        this.EntityPM.DepartmentId = SessionLocator.LoggedUserPM.DepartmentId;
        this.EntityPM.NewConcurrencyGUID = AppTool.GetNewGuid();
        this.EntityPM.Ratio = 6;
        this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
        this.EntityPM.AWBDeclaredValueForCarriage = "NVD";
        this.EntityPM.AWBDeclaredValueForCustoms = "NCV";
        this.EntityPM.AWBInsurrenceValue = "XXX";
        this.EntityPM.RateClassCode = "Q";

        this.EntityPM.FreightPrepaidCollectId = this.TenantPM.ExportFreightPrepaidCollectId;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.EntityPM.FreightPrepaidCollectId = this.TenantPM.MasterExportFreightPrepaidCollectId;
        }

        this.GetAWBSignature();

        if (!AppTool.IsNullOrEmpty(this.TenantPM.CASSCode)) {
            this.EntityPM.AWBChargesCodeCode = "PX";
        }

        else {
            ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
        }

        if (this.WindowArgs.IsCreatingHouseFromMaster) {
            var masterPM = this.WindowArgs.MasterPM;

            this.EntityPM.DirectionId = masterPM.DirectionId;
            this.EntityPM.TransportModeId = masterPM.TransportModeId;
            this.EntityPM.FromPortId = masterPM.MainCarriageFromPortId;
            this.EntityPM.MainCarriageFromPortId = masterPM.MainCarriageFromPortId;
            this.EntityPM.MainCarriageToPortId = masterPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.ToPortId = masterPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.FinalDistenationPortId = masterPM.FinalDistenationPortId;
            this.EntityPM.MainCarriageFinalDestinationPortId = masterPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.BranchId = masterPM.BranchId;
            this.EntityPM.DepartmentId = masterPM.DepartmentId;
            this.EntityPM.SalesmanUserId = masterPM.SalesmanUserId;
            this.EntityPM.FreightPrepaidCollectId = masterPM.FreightPrepaidCollectId;
            this.EntityPM.OtherPrepaidCollectId = masterPM.OtherPrepaidCollectId;
            this.EntityPM.MasterShipmentDataId = masterPM.Id;
            this.EntityPM.MainCarriageCarrierPrefix = masterPM.MainCarriageCarrierPrefix;
            this.EntityPM.TenantZeroAirlineId = masterPM.TenantZeroAirlineId;
            this.EntityPM.TenantZeroAirlineTTY = masterPM.TenantZeroAirlineTTY;
            this.EntityPM.TenantZeroAirlinePIMA = masterPM.TenantZeroAirlinePIMA;
            this.EntityPM.TenantZeroAirlineChampFWB = masterPM.TenantZeroAirlineChampFWB;
            this.EntityPM.TenantZeroAirlineChampFHL = masterPM.TenantZeroAirlineChampFHL;
            this.EntityPM.TenantZeroAirlineChampFSU = masterPM.TenantZeroAirlineChampFSU;
            this.EntityPM.TenantZeroAirlineChampFSRFSA = masterPM.TenantZeroAirlineChampFSRFSA;
            this.EntityPM.TenantZeroAirlineChampFVRFVA = masterPM.TenantZeroAirlineChampFVRFVA;
            this.EntityPM.CarrierIsChampRegistered = masterPM.CarrierIsChampRegistered;
            this.EntityPM.TenantZeroAirlineChampNeedsRegistration = masterPM.TenantZeroAirlineChampNeedsRegistration;
            this.EntityPM.TenantZeroAirlineGLSHKFWB = masterPM.TenantZeroAirlineGLSHKFWB;
            this.EntityPM.TenantZeroAirlineGLSHKFHL = masterPM.TenantZeroAirlineGLSHKFHL;
            this.EntityPM.TenantZeroAirlineGLSHKFSU = masterPM.TenantZeroAirlineGLSHKFSU;
            this.EntityPM.TenantZeroAirlineGLSHKFSRFSA = masterPM.TenantZeroAirlineGLSHKFSRFSA;
            this.EntityPM.TenantZeroAirlineGLSHKFVRFVA = masterPM.TenantZeroAirlineGLSHKFVRFVA;
            this.EntityPM.CarrierIsGLSHKRegistered = masterPM.CarrierIsGLSHKRegistered;
            this.EntityPM.TenantZeroAirlineGLSHKNeedsRegistration = masterPM.TenantZeroAirlineGLSHKNeedsRegistration;
            this.EntityPM.CarrierIsCheckDigit = masterPM.CarrierIsCheckDigit;
            this.EntityPM.CarrierIsLimitedLength = masterPM.CarrierIsLimitedLength;
            this.EntityPM.SCI = masterPM.SCI;
            this.GetAWBSignature();
        }

        if (this.WindowArgs.IsBuildFromBooking) {
            ShipmentTool.MapBookingShipment(this.EntityPM, this.WindowArgs.EntityPM);
        }

        if (this.WindowArgs.IsCopyFromShipment) {
            var oldShipment: ShipmentPM = this.WindowArgs.EntityPM;

            ShipmentTool.CopyShipment(this.EntityPM, oldShipment);
            ShipmentTool.CopyShipmentPackages(this.EntityPM, oldShipment, true);
            ShipmentTool.CopyFlights(this.EntityPM, oldShipment);

            // OCIs
            oldShipment.AWBOCIPMs.forEach(item => {
                var newItem: AWBOCIPM = new AWBOCIPM(this.EntityPM);
                newItem.Tenant = this.EntityPM.Tenant;
                newItem.ShipmentId = this.EntityPM.Id;
                newItem.CountryId = item.CountryId;
                newItem.AWBInformationCode = item.AWBInformationCode;
                newItem.AWBCustomsInformationCode = item.AWBCustomsInformationCode;
                newItem.SupplementaryCustomsInfo = item.SupplementaryCustomsInfo;
                this.EntityPM.AWBOCIPMs.push(newItem);
            });

            // Partners
            this.EntityPM.ShipperId = oldShipment.ShipperId;
            this.EntityPM.ShipperName = oldShipment.ShipperName;
            this.EntityPM.ShipperNote = oldShipment.ShipperNote;
            this.EntityPM.ShipperAddressId = oldShipment.ShipperAddressId;
            this.EntityPM.ShipperContactId = oldShipment.ShipperContactId;
            //this.EntityPM.ShipperReference1 = oldShipment.ShipperReference1;
            //this.EntityPM.ShipperReference2 = oldShipment.ShipperReference2;
            this.EntityPM.ShipperAddress1 = oldShipment.ShipperAddress1;
            this.EntityPM.ShipperAddress2 = oldShipment.ShipperAddress2;
            this.EntityPM.ShipperCity = oldShipment.ShipperCity;
            this.EntityPM.ShipperStateId = oldShipment.ShipperStateId;
            this.EntityPM.ShipperZipCode = oldShipment.ShipperZipCode;

            this.EntityPM.ConsigneeId = oldShipment.ConsigneeId;
            this.EntityPM.ConsigneeAddressId = oldShipment.ConsigneeAddressId;
            this.EntityPM.ConsigneeAddressId = oldShipment.ConsigneeAddressId;
            this.EntityPM.ConsigneeName = oldShipment.ConsigneeName;
            this.EntityPM.ConsigneeNote = oldShipment.ConsigneeNote;
            //this.EntityPM.ConsigneeReference1 = oldShipment.ConsigneeReference1;
            //this.EntityPM.ConsigneeReference2 = oldShipment.ConsigneeReference2;
            this.EntityPM.ConsigneeAddress1 = oldShipment.ConsigneeAddress1;
            this.EntityPM.ConsigneeAddress2 = oldShipment.ConsigneeAddress2;
            this.EntityPM.ConsigneeCity = oldShipment.ConsigneeCity;
            this.EntityPM.ConsigneeStateId = oldShipment.ConsigneeStateId;
            this.EntityPM.ConsigneeZipCode = oldShipment.ConsigneeZipCode;

            this.EntityPM.Notify1Id = oldShipment.Notify1Id;
            this.EntityPM.Notify1AddressId = oldShipment.Notify1AddressId;
            this.EntityPM.Notify1AddressId = oldShipment.Notify1AddressId;
            this.EntityPM.Notify1Name = oldShipment.Notify1Name;
            this.EntityPM.Notify1Note = oldShipment.Notify1Note;
            this.EntityPM.Notify1Address1 = oldShipment.Notify1Address1;
            this.EntityPM.Notify1Address2 = oldShipment.Notify1Address2;
            this.EntityPM.Notify1City = oldShipment.Notify1City;
            this.EntityPM.Notify1StateId = oldShipment.Notify1StateId;
            this.EntityPM.Notify1ZipCode = oldShipment.Notify1ZipCode;

            this.EntityPM.CustomerId = oldShipment.CustomerId;
            this.EntityPM.CustomerName = oldShipment.CustomerName;
            this.EntityPM.CustomerNote = oldShipment.CustomerNote;
            this.EntityPM.CustomerAddressId = oldShipment.CustomerAddressId;
            this.EntityPM.CustomerContactId = oldShipment.CustomerContactId;
            //this.EntityPM.CustomerReference1 = oldShipment.CustomerReference1;
            //this.EntityPM.CustomerReference2 = oldShipment.CustomerReference2;
            this.EntityPM.CustomerRankName = oldShipment.CustomerRankName;
            this.EntityPM.ShipmentCustomerTypeCode = oldShipment.ShipmentCustomerTypeCode;

            this.EntityPM.ViaColoader = oldShipment.ViaColoader;
            this.EntityPM.IssuingCarrierAgentId = oldShipment.IssuingCarrierAgentId;
            this.EntityPM.IssuingCarrierAddressId = oldShipment.IssuingCarrierAddressId;
            this.EntityPM.IssuingCarrierAgentName = oldShipment.IssuingCarrierAgentName;
            this.EntityPM.IssuingCarrierAgentNote = oldShipment.IssuingCarrierAgentNote;
            this.EntityPM.IssuingCarrierIATACode = oldShipment.IssuingCarrierIATACode;
            this.EntityPM.CASSCode = oldShipment.CASSCode;
            //this.EntityPM.IssuingCarrierReference1 = oldShipment.IssuingCarrierReference1;
            this.EntityPM.IssuingCarrierCity = oldShipment.IssuingCarrierCity;

            this.EntityPM.ConsolidatorId = oldShipment.ConsolidatorId;
            this.EntityPM.ConsolidatorName = oldShipment.ConsolidatorName;
            this.EntityPM.ConsolidatorNote = oldShipment.ConsolidatorNote;
            this.EntityPM.ConsolidatorAddressId = oldShipment.ConsolidatorAddressId;
            this.EntityPM.ConsolidatorContactId = oldShipment.ConsolidatorContactId;
            //this.EntityPM.ConsolidatorReference = oldShipment.ConsolidatorReference;
        }

        else {
            if (FeatureLocator.IsPackage_EAWB()) {
                this.GenerateShipmentAWBPrintOnlies();
            }
        }
    }

    private GenerateShipmentAWBPrintOnlies() {
        var iCurrencyListService: CurrencyListService = new CurrencyListService();
        var iChargesTypeListService: ChargesTypeListService = new ChargesTypeListService();
        var iMeasurementListService: MeasurementListService = new MeasurementListService();
        var iIATACodeListService: IATACodeListService = new IATACodeListService();

        iChargesTypeListService.getAll().subscribe((iResponseCharges: ServiceResponse) => {
            if (!iResponseCharges.HasError) {
                var AllChargesTypes: ChargesTypeList[] = iResponseCharges.Result;

                if (AllChargesTypes.length > 0) {
                    AllChargesTypes = AllChargesTypes.filter(d => d.IsAir == true && d.InActive == false && d.ChargesGroupCode != "FRT");

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        AllChargesTypes = AllChargesTypes.filter(d => d.IsAutoDisplayInConsolidation);
                    }

                    else {
                        AllChargesTypes = AllChargesTypes.filter(d => d.IsAutoDisplayInShipment == true);
                    }
                }

                if (AllChargesTypes.length > 0) {
                    iCurrencyListService.getAllFromCache().subscribe((iResponseCurrency: ServiceResponse) => {
                        if (!iResponseCurrency.HasError) {
                            var AllCurrencies: CurrencyList[] = iResponseCurrency.Result;

                            iMeasurementListService.getAllFromCache().subscribe((iResponseMeasurement: ServiceResponse) => {
                                if (!iResponseMeasurement.HasError) {
                                    var AllMeasurements: MeasurementList[] = iResponseMeasurement.Result;

                                    iIATACodeListService.getAllFromCache().subscribe((iResponseIATAs: ServiceResponse) => {
                                        if (!iResponseIATAs.HasError) {
                                            var AllIATACodes: IATACodeList[] = iResponseIATAs.Result;

                                            AllChargesTypes.sort((a, b) => { return a.ViewOrder - b.ViewOrder }).forEach((item) => {
                                                var itemPM: ShipmentAWBPrintOnlyPM = new ShipmentAWBPrintOnlyPM(this.EntityPM);
                                                itemPM.Tenant = this.EntityPM.Tenant;
                                                itemPM.ShipmentId = this.EntityPM.Id;
                                                itemPM.MeasurementId = item.MeasurementId;
                                                itemPM.DueTypeCode = item.DueTypeCode;
                                                itemPM.DueTypeName = item.DueTypeName;
                                                itemPM.IATACodeId = item.IATACodeId;
                                                itemPM.CurrencyId = this.EntityPM.AWBCurrencyId;
                                                itemPM.CurrencyCode = this.EntityPM.AWBCurrencyCode;
                                                itemPM.PrepaidCollectId = this.EntityPM.OtherPrepaidCollectId;

                                                if (itemPM.CurrencyId != null) {
                                                    var myCurrencyList: CurrencyList = AllCurrencies.filter(d => d.Id == itemPM.CurrencyId)[0];
                                                    if (myCurrencyList != null) {
                                                        itemPM.CurrencyCode = myCurrencyList.Code;
                                                    }

                                                    else {
                                                        iCurrencyListService.getSingle(itemPM.CurrencyId).subscribe((myResponse: ServiceResponse) => {
                                                            if (myResponse != null) {
                                                                if (!myResponse.HasError) {
                                                                    AllCurrencies.push(myResponse.Result);
                                                                    itemPM.CurrencyCode = myResponse.Result.Code;
                                                                }
                                                            }
                                                        });
                                                    }
                                                }
                                                if (itemPM.IATACodeId != null) {
                                                    var myIATACodeList: IATACodeList = AllIATACodes.filter(d => d.Id == itemPM.IATACodeId)[0];
                                                    if (myIATACodeList != null) {
                                                        itemPM.IATACodeName = myIATACodeList.Name;
                                                    }

                                                    else {
                                                        iIATACodeListService.getSingle(itemPM.IATACodeId).subscribe((myResponse: ServiceResponse) => {
                                                            if (myResponse != null) {
                                                                if (!myResponse.HasError) {
                                                                    AllIATACodes.push(myResponse.Result);
                                                                    itemPM.IATACodeName = myResponse.Result.Name;
                                                                }
                                                            }
                                                        });
                                                    }
                                                }
                                                if (itemPM.MeasurementId != null) {
                                                    var myMeasurementList: MeasurementList = AllMeasurements.filter(d => d.Id == itemPM.MeasurementId)[0];
                                                    if (myMeasurementList != null) {
                                                        itemPM.MeasurementCode = myMeasurementList.Code;

                                                        switch (itemPM.MeasurementCode) {
                                                            case "GRWT": { itemPM.Quantity = this.EntityPM.GrossWeight; break; }
                                                            case "CHWT": { itemPM.Quantity = this.EntityPM.ChargeableWeight; break; }
                                                            case "VOLU": { itemPM.Quantity = this.EntityPM.Volume; break; }
                                                            case "BTEU": { itemPM.Quantity = this.EntityPM.TEU; break; }
                                                            case "FIXD": { itemPM.Quantity = 1; break; }
                                                            case "PRVL": { itemPM.Quantity = this.EntityPM.ValueOfGoods; break; }
                                                            case "GWTN": { itemPM.Quantity = this.EntityPM.GrossWeightPerTon; break; }
                                                            case "QTY": { itemPM.Quantity = this.EntityPM.NumberOfPackages; break; }
                                                            default: { break; }
                                                        }
                                                    }

                                                    else {
                                                        iMeasurementListService.getSingle(itemPM.MeasurementId).subscribe((myResponse: ServiceResponse) => {
                                                            if (myResponse != null) {
                                                                if (!myResponse.HasError) {
                                                                    AllMeasurements.push(myResponse.Result);
                                                                    itemPM.MeasurementCode = myResponse.Result.Code;

                                                                    switch (itemPM.MeasurementCode) {
                                                                        case "GRWT": { itemPM.Quantity = this.EntityPM.GrossWeight; break; }
                                                                        case "CHWT": { itemPM.Quantity = this.EntityPM.ChargeableWeight; break; }
                                                                        case "VOLU": { itemPM.Quantity = this.EntityPM.Volume; break; }
                                                                        case "BTEU": { itemPM.Quantity = this.EntityPM.TEU; break; }
                                                                        case "FIXD": { itemPM.Quantity = 1; break; }
                                                                        case "PRVL": { itemPM.Quantity = this.EntityPM.ValueOfGoods; break; }
                                                                        case "GWTN": { itemPM.Quantity = this.EntityPM.GrossWeightPerTon; break; }
                                                                        case "QTY": { itemPM.Quantity = this.EntityPM.NumberOfPackages; break; }
                                                                        default: { break; }
                                                                    }
                                                                }
                                                            }
                                                        });
                                                    }
                                                }

                                                this.EntityPM.ShipmentAWBPrintOnlies.push(itemPM);
                                            });

                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            }
        });
    }

    private GetAWBSignature() {        

        if (this.EntityPM.BranchId) {

            var myResult: string = null;

            var myService = new BranchListService();
            myService.getSingleFromCache(this.EntityPM.BranchId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: BranchList = myResponse.Result;
                    if (list) {
                        myResult = list.Signature;
                    }

                    if (AppTool.IsNullOrEmpty(myResult)) {
                        myResult = SessionLocator.TenantPM.Signature;
                    }

                    this.EntityPM.AWBSignature = myResult;
                }
            });
        }

        else {
            this.EntityPM.AWBSignature = SessionLocator.TenantPM.Signature;
        }
    }

    // Selected Tab
    private SetSelectedTab() {

        if (this.IsTabVisible_OVE) {
            this.selectedTabCode = "OVE";
        }

        else {
            this.selectedTabCode = "PAR";
        }
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    private PageChild_OVE: any = null;
    private PageChild_PAR: any = null;
    private PageChild_ROU: any = null;
    private PageChild_HAW: any = null;
    private PageChild_PAC: any = null;
    private PageChild_FRE: any = null;
    private PageChild_OTC: any = null;
    private PageChild_RAD: any = null;
    private PageChild_GEN: any = null;
    private PageChild_OCI: any = null;
    private PageChild_OTP: any = null;
    SelectionChanged() {
        if (this.SelectedTabCode != null) {
            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response1=> {
                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
                if (myLocation != null) {

                    switch (this.SelectedTabCode) {
                        case "OVE": {
                            if (this.PageChild_OVE == null) {
                                this._entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(response=> {
                                    SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Overview/AWBOverviewTabComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.PageChild_OVE = cmpRef.instance;
                                            this.PageChild_OVE.InitTab(this.EntityPM, this);
                                            this.PageChild_OVE.PackagesResourcesReady = true;
                                        });
                                });
                            }

                            else {
                                this.PageChild_OVE.RefreshTab();
                            }

                            break;
                        }
                        case 'PAR': {
                            if (this.PageChild_PAR == null) {
                                this._entityResourceService.getEntityResourceByTableName("Card").subscribe(response=> {
                                    this._entityResourceService.getEntityResourceByTableName("Address").subscribe(response2=> {
                                        SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBPartnersTabComponent', myLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                this.PageChild_PAR = cmpRef.instance;
                                                this.PageChild_PAR.InitTab(this);
                                            });
                                    });
                                });
                            }

                            else {
                                this.PageChild_PAR.RefreshTab();
                            }

                            break;
                        }
                        case "ROU": {
                            if (this.PageChild_ROU == null) {
                                if (this.EntityPM.ShipmentLevelCode == "H") {
                                    SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Routings/AWBHouseRoutingsTabComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.PageChild_ROU = cmpRef.instance;
                                            this.PageChild_ROU.InitTab(this);
                                        });
                                }

                                else {
                                    SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Routings/AWBRoutingsTabComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.PageChild_ROU = cmpRef.instance;
                                            this.PageChild_ROU.InitTab(this);
                                        });
                                }
                            }

                            else {
                                this.PageChild_ROU.RefreshTab();
                            }

                            break;
                        }
                        case "HAW": {
                            if (this.PageChild_HAW == null) {
                                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/HAWB/HAWBTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_HAW = cmpRef.instance;
                                        this.PageChild_HAW.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_HAW.RefreshTab();
                            }

                            break;
                        }
                        case "PAC": {
                            if (this.PageChild_PAC == null) {
                                this._entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(response => {
                                    this._entityResourceService.getEntityResourceByTableName("ShipmentCommodity").subscribe(response2 => {
                                        SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBPackagesTabComponent', myLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                this.PageChild_PAC = cmpRef.instance;
                                                this.PageChild_PAC.InitTab(this);
                                            });
                                    });
                                });
                            }

                            else {
                                this.PageChild_PAC.RefreshTab();
                            }

                            break;
                        }
                        case "FRE": {
                            if (this.PageChild_FRE == null) {
                                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/FreightCharges/FreightChargesTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_FRE = cmpRef.instance;
                                        this.PageChild_FRE.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_FRE.RefreshTab();
                            }

                            break;
                        }
                        case "OTC": {
                            if (this.PageChild_OTC == null) {
                                this._entityResourceService.getEntityResourceByTableName("ShipmentAWBPrintOnly").subscribe(response=> {
                                    SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherCharges/OtherChargesTabComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.PageChild_OTC = cmpRef.instance;
                                            this.PageChild_OTC.InitTab(this);
                                        });
                                });
                            }

                            else {
                                this.PageChild_OTC.RefreshTab();
                            }

                            break;
                        }
                        case "RAD": {
                            if (this.PageChild_RAD == null) {
                                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/RADetails/RADetailsTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_RAD = cmpRef.instance;
                                        this.PageChild_RAD.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_RAD.RefreshTab();
                            }

                            break;
                        }
                        case "GEN": {
                            if (this.PageChild_GEN == null) {
                                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/GeneralDetails/GeneralDetailsTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_GEN = cmpRef.instance;
                                        this.PageChild_GEN.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_GEN.RefreshTab();
                            }

                            break;
                        }
                        case "OCI": {
                            if (this.PageChild_OCI == null) {
                                this._entityResourceService.getEntityResourceByTableName("AWBOCI", 0).subscribe((response:any) => {
                                    SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/OCI/OCITabComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.PageChild_OCI = cmpRef.instance;
                                            this.PageChild_OCI.InitTab(this);
                                        });
                                });
                            }

                            else {
                                this.PageChild_OCI.RefreshTab();
                            }

                            break;
                        }
                        case "OTP": {
                            if (this.PageChild_OTP == null) {
                                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherPartners/OtherPartnersTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_OTP = cmpRef.instance;
                                        this.PageChild_OTP.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_OTP.RefreshTab();
                            }

                            break;
                        }
                    }
                }
            });
        }

    }

    // Allowed Airline
    private LoadAllowedAirline() {
        if (SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    if (this.EntityPM.TransportModeId == "A") {
                        if (this.EntityPM.ShipmentLevelCode != "H") {

                            this.myPartnersDomainService.GetAllowedAirlineId().subscribe((myResponse: ServiceResponse) => {
                                if (myResponse != null) {
                                    if (myResponse.HasError) {
                                        this.ValidationErrorsList = myResponse.ErrorsArray;
                                    }

                                    else {
                                        var allowedAirlineId: string = myResponse.Result;
                                        if (!AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                            this.EntityPM.MainCarriageCarrierId = allowedAirlineId;
                                            this.GetMainCarriageCarrier();
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

    private myCardListService: CardListService;
    private myAirlineListService: AirlineListService;
    private GetMainCarriageCarrier() {
        if (this.EntityPM.MainCarriageCarrierId == null) {
            this.EntityPM.Master = null;
            this.EntityPM.LongMaster = null;
            this.EntityPM.AirlinePrefix = null;
            this.EntityPM.AccountNumber = null;
            this.EntityPM.MainCarriageCarrierPrefix = null;
            this.EntityPM.MainCarriageCarrierNumber = null;
            this.EntityPM.MainCarriageCarrierCode = null;
            this.EntityPM.MainCarriageCarrierName = null;
            this.EntityPM.CarrierIsChampRegistered = false;
            this.EntityPM.CarrierIsGLSHKRegistered = false;
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;

            ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
            this.OnLoadingAllowedAirlineCompleted();
        }

        else {
            if (this.myCardListService == null) {
                this.myCardListService = new CardListService();
                
            }

            if (this.myAirlineListService == null) {
                this.myAirlineListService = new AirlineListService();
                
            }

            this.myCardListService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: any = myResponse.Result;
                    if (list != null) {
                        this.EntityPM.MainCarriageCarrierPrefix = list.Code;
                        this.EntityPM.AccountNumber = list.AirlineAccountNumber;
                        this.EntityPM.MainCarriageCarrierCode = list.Code;
                        this.EntityPM.MainCarriageCarrierName = list.EnglishName;
                    }
                }

                this.OnLoadingAllowedAirlineCompleted();
            });

            this.myAirlineListService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: any = myResponse.Result;

                    if (list != null) {
                        var myPrefix: string = null;

                        if (!AppTool.IsNullOrEmpty(list.Prefix)) {
                            myPrefix = list.Prefix.toString().trim();
                            myPrefix = AppTool.PadLeft(myPrefix, 3, "0");
                        }

                        this.EntityPM.AirlinePrefix = myPrefix;
                        this.EntityPM.LongMaster = AppTool.GetLongMasterField(this.EntityPM.TransportModeId, myPrefix, this.EntityPM.Master);
                        this.EntityPM.CarrierIsChampRegistered = list.IsChampRegistered;
                        this.EntityPM.CarrierIsGLSHKRegistered = list.IsGLSHKRegistered;
                        this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                        this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;

                        this.myPartnersDomainService.GetAirlineByCode(list.Code, 0).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                ShipmentTool.MapTenantZeroAirline(this.EntityPM, myResponse.Result);
                            }

                            this.OnLoadingAllowedAirlineCompleted();
                        });
                    }
                }
            });
        }
    }

    private OnLoadingAllowedAirlineCompleted() {
        this.ValidateScreen_ROU();
    }

    // Airline Rules
    public AirlineRulesList: AirlineMessagingRuleList[] = [];
    public LoadAirlineRules(myAirlineCode: string) {

        if (AppTool.IsNullOrEmpty(myAirlineCode)) {
            this.AirlineRulesList = [];
            this.ValidateAllTabs();
            this.RefreshTab(this.SelectedTabCode);
            this.ValidateAllTabs();
        }

        else {
            var myMessageCode = this.IsFWB ? "FWB" : "FHL";

            this.myPartnersDomainService.GetAirlineRules(myAirlineCode, myMessageCode).subscribe((myResponse: ServiceResponse) => {
                if (myResponse == null) {
                    this.AirlineRulesList = [];
                    this.ValidateAllTabs();
                    this.RefreshTab(this.SelectedTabCode);
                }

                else {                    
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.AirlineRulesList = myResponse.Result;
                        this.ValidateAllTabs();
                        this.RefreshTab(this.SelectedTabCode);
                    }
                }

                this.ValidateAllTabs();
            });
        }
    }
    private RefreshTab(tabCode: string) {
        switch (tabCode) {

            case "OVE": {
                if (this.PageChild_OVE != null) {
                    this.PageChild_OVE.RefreshTab();
                }

                break;
            }

            case "PAR": {
                if (this.PageChild_PAR != null) {
                    this.PageChild_PAR.RefreshTab();
                }

                break;
            }

            case "ROU": {
                if (this.PageChild_ROU != null) {
                    this.PageChild_ROU.RefreshTab();
                }

                break;
            }

            case "HAW": {
                if (this.PageChild_HAW != null) {
                    this.PageChild_HAW.RefreshTab();
                }

                break;
            }

            case "PAC": {
                if (this.PageChild_PAC != null) {
                    this.PageChild_PAC.RefreshTab();
                }

                break;
            }

            case "FRE": {
                if (this.PageChild_FRE != null) {
                    this.PageChild_FRE.RefreshTab();
                }

                break;
            }

            case "OTC": {
                if (this.PageChild_OTC != null) {
                    this.PageChild_OTC.RefreshTab();
                }

                break;
            }

            case "RAD": {
                if (this.PageChild_RAD != null) {
                    this.PageChild_RAD.RefreshTab();
                }

                break;
            }

            case "GEN": {
                if (this.PageChild_GEN != null) {
                    this.PageChild_GEN.RefreshTab();
                }

                break;
            }

            case "OCI": {
                if (this.PageChild_OCI != null) {
                    this.PageChild_OCI.RefreshTab();
                }

                break;
            }

            case "OTP": {
                if (this.PageChild_OTP != null) {
                    this.PageChild_OTP.RefreshTab();
                }

                break;
            }
        }
    }

    // Currency Rates
    public AllCurrencyRates: LastRate[] = [];
    private LoadCurrencyRates() {
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        var myService: CurrencyRatesService = new CurrencyRatesService();

        myService.getAll(this.TenantPM.CurrencyId, todayDate).subscribe((myResponse: any) => {
            if (myResponse != null) {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.AllCurrencyRates = myResponse.Result;

                    if (this.IsNewEntity) {
                        var myProfitRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

                        if (this.EntityPM.ProfitExchangeRate == null) {
                            if (this.EntityPM.ProfitExchangeRate != myProfitRate) {
                                this.EntityPM.ProfitExchangeRate = myProfitRate;
                            }
                        }
                    }
                }
            }

        });
    }
    public GetCurrencyRate(myCurrencyId: string) {

        var myExchangeRate: number = null;

        if (!AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (this.AllCurrencyRates != null) {

                if (myCurrencyId == this.TenantPM.CurrencyId) {
                    myExchangeRate = 1;
                }

                else {

                    var lastRate = this.AllCurrencyRates.filter(d => d.ForeignCurrencyId == myCurrencyId)[0];
                    if (lastRate != null) {
                        myExchangeRate = lastRate.Rate;
                    }
                }
            }
        }

        return myExchangeRate;
    }

    // Validate Tabs
    public ValidationText: string;
    public TabErrors_PAR: string[] = [];
    public TabErrors_ROU: string[] = [];
    public TabErrors_PAC: string[] = [];
    public TabErrors_FRE: string[] = [];
    public TabErrors_OTC: string[] = [];
    public TabErrors_RAD: string[] = [];
    public TabErrors_GEN: string[] = [];
    public TabErrors_OCI: string[] = [];
    public TabErrors_OTP: string[] = [];
    public TabWarnings_PAR: string[] = [];
    public TabWarnings_ROU: string[] = [];
    public TabWarnings_PAC: string[] = [];
    public TabWarnings_FRE: string[] = [];
    public TabWarnings_OTC: string[] = [];
    public TabWarnings_RAD: string[] = [];
    public TabWarnings_GEN: string[] = [];
    public TabWarnings_OCI: string[] = [];
    public TabWarnings_OTP: string[] = [];
    private ValidateAllTabs() {
        this.ValidationText = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.ValidateScreen_PAR();
        this.ValidateScreen_ROU();
        this.ValidateScreen_PAC();
        this.ValidateScreen_FRE();
        this.ValidateScreen_OTC();
        this.ValidateScreen_RAD();
        this.ValidateScreen_GEN();
        this.ValidateScreen_OCI();
        this.ValidateScreen_OTP();
    }
    public ValidateScreen_PAR() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        this.ValidateScreen_PAR_Shipper(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_Consignee(screenErrors, screenWarnings);
        if (!this.IsImportWizard) {
            this.ValidateScreen_PAR_Notify1(screenErrors, screenWarnings);
            this.ValidateScreen_PAR_IssuingAgent(screenErrors, screenWarnings);
            this.ValidateScreen_PAR_AirlineRules(screenErrors, screenWarnings);
        }
        this.TabErrors_PAR = screenErrors;
        this.TabWarnings_PAR = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAR");
    }

    private ValidateScreen_PAR_Shipper(screenErrors: string[], screenWarnings: string[]) {

        if (this.EntityPM.ShipperId == null) {
            if (!this.IsImportWizard) {
                screenErrors.push(this.ValidationText.replace("%FieldName", "Shipper"));
            }
        }

        else {
            if (!this.IsImportWizard) {
            if (!FormatTool.IsTextFormatted(this.EntityPM.ShipperName)) {
                screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Shipper Name"));
            }

                if (AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address"));
                }

                else {
                    var myAddress1: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress1) ? null : this.EntityPM.ShipperAddress1.trim();
                    var myAddress2: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress2) ? null : this.EntityPM.ShipperAddress2.trim();
                    var myZipCode: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperZipCode) ? null : this.EntityPM.ShipperZipCode.trim();
                    var myCity: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperCity) ? null : this.EntityPM.ShipperCity.trim();
                    var myFaxNumber: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperFaxNumber) ? null : this.EntityPM.ShipperFaxNumber.trim();
                    var myPhoneNumber: string = AppTool.IsNullOrEmpty(this.EntityPM.ShipperPhoneNumber) ? null : this.EntityPM.ShipperPhoneNumber.trim();

                    if (!FormatTool.IsTextFormatted(myAddress1)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Shipper Address1"));
                    }

                    if (!FormatTool.IsTextFormatted(myAddress2)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Shipper Address2"));
                    }

                    if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address1 Or Address2"));
                    }

                    if (AppTool.IsNullOrEmpty(myZipCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Zip Code"));
                    }

                    else if (!FormatTool.IsTextFormatted(myZipCode)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Shipper Zip Code"));
                    }

                    if (AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper City"));
                    }

                    else if (!FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Shipper City"));
                    }

                    if (AppTool.IsNullOrEmpty(this.EntityPM.ShipperStateId)) {
                        if (this.AllStates.filter(d => d.CountryId == this.EntityPM.ShipperCountryId).length > 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper address state"));
                        }
                    }

                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (AppTool.IsNullOrEmpty(myFaxNumber) && AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Phone Or Fax"));
                        }
                    }
                }
            }
        }
    }
    private ValidateScreen_PAR_Consignee(screenErrors: string[], screenWarnings: string[]) {

        if (this.EntityPM.ConsigneeId == null) {
            if (!this.IsImportWizard) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee"));
            }
            else {
                screenErrors.push(this.ValidationText.replace("%FieldName", "Consignee"));
            }
        }

        else {
            if (!this.IsImportWizard) {
                if (!FormatTool.IsTextFormatted(this.EntityPM.ConsigneeName)) {
                    screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Consignee Name"));
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address"));
                }

                else {
                    var myAddress1: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress1) ? null : this.EntityPM.ConsigneeAddress1.trim();
                    var myAddress2: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress2) ? null : this.EntityPM.ConsigneeAddress2.trim();
                    var myZipCode: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeZipCode) ? null : this.EntityPM.ConsigneeZipCode.trim();
                    var myCity: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeCity) ? null : this.EntityPM.ConsigneeCity.trim();
                    var myFaxNumber: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeFaxNumber) ? null : this.EntityPM.ConsigneeFaxNumber.trim();
                    var myPhoneNumber: string = AppTool.IsNullOrEmpty(this.EntityPM.ConsigneePhoneNumber) ? null : this.EntityPM.ConsigneePhoneNumber.trim();

                    if (!FormatTool.IsTextFormatted(myAddress1)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Consignee Address1"));
                    }

                    if (!FormatTool.IsTextFormatted(myAddress2)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Consignee Address2"));
                    }

                    if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address1 Or Address2"));
                    }

                    if (AppTool.IsNullOrEmpty(myZipCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Zip Code"));
                    }

                    else if (!FormatTool.IsTextFormatted(myZipCode)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Consignee Zip Code"));
                    }

                    if (AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee City"));
                    }

                    else if (!FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Consignee City"));
                    }

                    if (AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeStateId)) {
                        if (this.AllStates.filter(d => d.CountryId == this.EntityPM.ConsigneeCountryId).length > 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee address state"));
                        }
                    }

                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (AppTool.IsNullOrEmpty(myFaxNumber) && AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Phone Or Fax"));
                        }
                    }
                }
            }
        }
    }
    private ValidateScreen_PAR_Notify1(screenErrors: string[], screenWarnings: string[]) {
        if (this.IsFWB) {
            if (this.EntityPM.Notify1Id != null) {
                if (!FormatTool.IsTextFormatted(this.EntityPM.Notify1Name)) {
                    screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Notify1 Name"));
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.Notify1AddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Address"));
                }

                else {
                    var myAddress1: string = AppTool.IsNullOrEmpty(this.EntityPM.Notify1Address1) ? null : this.EntityPM.Notify1Address1.trim();
                    var myAddress2: string = AppTool.IsNullOrEmpty(this.EntityPM.Notify1Address2) ? null : this.EntityPM.Notify1Address2.trim();
                    var myZipCode: string = AppTool.IsNullOrEmpty(this.EntityPM.Notify1ZipCode) ? null : this.EntityPM.Notify1ZipCode.trim();
                    var myCity: string = AppTool.IsNullOrEmpty(this.EntityPM.Notify1City) ? null : this.EntityPM.Notify1City.trim();
                    var myFaxNumber: string = AppTool.IsNullOrEmpty(this.EntityPM.Notify1FaxNumber) ? null : this.EntityPM.Notify1FaxNumber.trim();
                    var myPhoneNumber: string = AppTool.IsNullOrEmpty(this.EntityPM.Notify1PhoneNumber) ? null : this.EntityPM.Notify1PhoneNumber.trim();

                    if (!FormatTool.IsTextFormatted(myAddress1)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Notify1 Address1"));
                    }

                    if (!FormatTool.IsTextFormatted(myAddress2)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Notify1 Address2"));
                    }

                    if (AppTool.IsNullOrEmpty(myAddress1) && AppTool.IsNullOrEmpty(myAddress2)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Address1 Or Address2"));
                    }

                    if (AppTool.IsNullOrEmpty(myZipCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Zip Code"));
                    }

                    else if (!FormatTool.IsTextFormatted(myZipCode)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Notify1 Zip Code"));
                    }

                    if (AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 City"));
                    }

                    else if (!FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Notify1 City"));
                    }

                    if (AppTool.IsNullOrEmpty(this.EntityPM.Notify1StateId)) {
                        if (this.AllStates.filter(d => d.CountryId == this.EntityPM.Notify1CountryId).length > 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 address state"));
                        }
                    }

                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (AppTool.IsNullOrEmpty(myFaxNumber) && AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Phone Or Fax"));
                        }
                    }
                }
            }
        }
    }
    private ValidateScreen_PAR_IssuingAgent(screenErrors: string[], screenWarnings: string[]) {
        if (this.IsFWB) {
            if (this.EntityPM.IssuingCarrierAgentId == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent"));
            }

            else {
                if (!FormatTool.IsTextFormatted(this.EntityPM.IssuingCarrierAgentName)) {
                    screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Issuing Carrier Agent Name"));
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierIATACode)) {
                    if (!FormatTool.Validate_IATACode(this.EntityPM.IssuingCarrierIATACode)) {
                        var fieldName: string = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "IssuingCarrierIATACode");
                        screenWarnings.push(fieldName + " wrong format: must be 7 numeric digits max");
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.CASSCode)) {
                    if (!FormatTool.Validate_CASSCode(this.EntityPM.CASSCode)) {
                        var fieldName: string = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "CASSCode");
                        screenWarnings.push(fieldName + " wrong format: must be 4 numeric digits max");
                    }
                }
                
                if (AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierAddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent Address"));
                }

                else {

                    var myCity: string = AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierCity) ? null : this.EntityPM.IssuingCarrierCity.trim();

                    if (AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent City"));
                    }

                    else if (!FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage("Issuing Carrier Agent City"));
                    }
                }
            }

            if (this.CCSTypeCode == "GLSHK") {
                if (this.EntityPM.ViaColoader) {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierReference1)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Reference 1"));
                    }
                }
            }
        }
    }
    private ValidateScreen_PAR_AirlineRules(screenErrors: string[], screenWarnings: string[]) {
        this.ValidateAirlineRule("IssuingCarrierIATACode", this.EntityPM.IssuingCarrierIATACode, screenWarnings);
        this.ValidateAirlineRule("CASSCode", this.EntityPM.CASSCode, screenWarnings);
    }

    public ValidateScreen_ROU() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        if (this.EntityPM.MainCarriageFromPortId == null) {
            screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.Departure")));
        }

        if (this.EntityPM.MainCarriageToPortId == null) {
            screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.Destination")));
        }

        if (this.EntityPM.DirectionId.toUpperCase() == "D") {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
                if (this.EntityPM.FromCountryId != this.EntityPM.ToCountryId) {
                    if (this.EntityPM.FromCountryIsEC == false || this.EntityPM.ToCountryIsEC == false) {
                        if (this.EntityPM.TransportModeId == "I") {
                            screenErrors.push("Main Carriage Addresses must be in the same country since the direction is Domestic");
                        }

                        else {
                            screenErrors.push("Main Carriage Ports must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }

        if (this.IsFWB) {
            if (!this.IsImportWizard) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.Airline")));
                }

                else {
                    var codePrefix = AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierPrefix) ? this.EntityPM.MainCarriageCarrierPrefix : this.EntityPM.MainCarriageCarrierPrefix.trim();
                    if (AppTool.IsNullOrEmpty(codePrefix)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Carrier Prefix"));
                    }

                    else if (codePrefix.length != 2) {
                        screenWarnings.push("Main Carriage Carrier Prefix length must be 2");
                    }
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierNumber)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.FlightNo")));
                }

                else {
                    if (this.isSendButtonClicked || this.isPrintButtonClicked || this.isPreviewButtonClicked) {
                        if (!FormatTool.Validate_FlightNumber(this.EntityPM.MainCarriageCarrierNumber)) {
                            var fieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".O." + "Routings.FlightNo");
                            screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                        }
                    }
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
                var myMasterFieldError: string = AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);
                if (!AppTool.IsNullOrEmpty(myMasterFieldError)) {
                    screenErrors.push(myMasterFieldError);
                }
            }
            if (!this.IsImportWizard) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Master) && AppTool.IsNullOrEmpty(this.EntityPM.MAWBStackNumber)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.MAWB")));
                }

                if (this.EntityPM.MAWBOBLDate == null) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.MAWBDate")));
                }

                if (this.EntityPM.MainCarriageETD == null) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage ETD"));
                }


                if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1FromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1ToPortId)) {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 2 carrier"));
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2FromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2ToPortId)) {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 3 carrier"));
                    }
                }
            }
        }

        else {
            if (this.EntityPM.HasPreForwarding && AppTool.IsNullOrEmpty(this.EntityPM.PreForwardingFromPortId)) {
                screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.PreForwardingFromPortId")));
            }

            if (this.EntityPM.HasOnForwarding && AppTool.IsNullOrEmpty(this.EntityPM.OnForwardingToPortId)) {
                screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.OnForwardingToPortId")));
            }
            if (!this.IsImportWizard) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.House)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "House"));
                }
            }
        }

        this.TabErrors_ROU = screenErrors;
        this.TabWarnings_ROU = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "ROU");
    }

    public ValidateScreen_PAC() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        //if (!this.TenantPM.AllowEAWBMoreThanTenPackages) {
        //    var myError = ShipmentTool.ValidateAddedPackagesCount(this.EntityPM);

        //    if (!AppTool.IsNullOrEmpty(myError)) {
        //        screenErrors.push(myError);
        //    }
        //}
        
        if (AppTool.IsNullOrZero(this.EntityPM.GrossWeight)) {
            if (!this.IsImportWizard) {
                var msgField = TextCodeTranslator.Translate(this.ObjectTableName + ".F.GrossWeight");
                msgField = msgField.replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
                screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.SLAC)) {
            if (!FormatTool.Validate_SLAC(this.EntityPM.SLAC)) {
                var fieldName: string = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "SLAC");
                screenErrors.push(fieldName + " wrong format: must be 5 numeric digits max");
            }
        }

        if (this.IsFWB) {
            this.ValidateScreen_PAC_FWB(screenErrors, screenWarnings);
        }

        else {
            if (!this.IsImportWizard) {
                var myFieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".F.DescriptionOfGoods");

                if (AppTool.IsNullOrEmpty(this.EntityPM.DescriptionOfGoods)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", myFieldName));
                }
            }
        }

        this.ValidateScreen_PAC_AirlineRules(screenErrors, screenWarnings);

        this.TabErrors_PAC = screenErrors;
        this.TabWarnings_PAC = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAC");
    }
    private ValidateScreen_PAC_FWB(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            if (AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight)) {
                var msgField = TextCodeTranslator.Translate(this.ObjectTableName + ".F.ChargeableWeight");
                msgField = msgField.replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);

                screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
            }

            if (this.EntityPM.IsMultipleCommodities) {

                var BreakException = {};

                try {
                    this.EntityPM.ShipmentCommodities.forEach((item) => {

                        if (!AppTool.IsNullOrEmpty(item.CommodityNumber)) {
                            if (!FormatTool.Validate_CommodityNo(item.CommodityNumber)) {
                                var fieldName = TextCodeTranslator.Translate("ShipmentCommodity.F.CommodityNumber");
                                screenWarnings.push(fieldName + " must be 4-7 numeric");
                                throw BreakException;
                            }
                        }

                        if (AppTool.IsNullOrEmpty(item.RateClassCode)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("ShipmentCommodity.F.RateClassCode")));
                        }

                        if (!this.EntityPM.AsAgreedFreight) {
                            var rateClassGroupCode = ShipmentTool.GetRateClassGroupCode(item.RateClassCode);

                            if (rateClassGroupCode != "S") {
                                if (AppTool.IsNullOrZero(item.ChargeRate)) {
                                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("ShipmentCommodity.F.ChargeRate")));
                                }

                                if (AppTool.IsNullOrZero(item.ChargeAmount)) {
                                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("ShipmentCommodity.F.ChargeAmount")));
                                }
                            }
                        }
                    });
                }

                catch (e) {
                    if (e !== BreakException) throw e;
                }
            }

            else {

                if (this.EntityPM.ShipmentLevelCode == "C") {
                    if (this.EntityPM.MainCarriageCarrierCode == "AR") {
                        var myFieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".F.DescriptionOfGoods");

                        if (AppTool.IsNullOrEmpty(this.EntityPM.DescriptionOfGoods)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", myFieldName));
                        }
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBCommodityItemNumber)) {
                    if (!FormatTool.Validate_CommodityNo(this.EntityPM.AWBCommodityItemNumber)) {
                        var fieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBCommodityItemNumber");
                        screenWarnings.push(fieldName + " must be 4-7 numeric");
                    }
                }
            }
        }
    }
    private ValidateScreen_PAC_AirlineRules(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            if (!this.EntityPM.IsMultipleCommodities) {
                if (this.IsFWB) {
                    this.ValidateAirlineRule("AWBCommodityItemNumber", this.EntityPM.AWBCommodityItemNumber, screenWarnings);
                }

                this.ValidateAirlineRule("DescriptionOfGoods", this.EntityPM.DescriptionOfGoods, screenWarnings);
            }
        }
    }

    public ValidateScreen_FRE() {

        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];
        if (!this.IsImportWizard) {
            if (this.IsFWB) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.AWBCurrencyId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AWBCurrencyId")));
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.AWBChargesCodeCode)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AWBChargesCodeCode")));
                }

                if (!this.EntityPM.IsMultipleCommodities) {
                    if (AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight)) {
                        var msgField = TextCodeTranslator.Translate(this.ObjectTableName + ".F.ChargeableWeight");
                        msgField = msgField.replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);

                        screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
                    }

                    if (AppTool.IsNullOrEmpty(this.EntityPM.RateClassCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.RateClassCode")));
                    }

                    if (!this.EntityPM.AsAgreedFreight) {
                        var rateClassGroupCode = ShipmentTool.GetRateClassGroupCode(this.EntityPM.RateClassCode);

                        if (rateClassGroupCode != "S") {
                            if (this.EntityPM.AWBChargeRate == null || this.EntityPM.AWBChargeRate == 0) {
                                screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AWBChargeRate")));
                            }
                        }

                        if (this.EntityPM.AWBChargeAmount == null || this.EntityPM.AWBChargeAmount == 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AWBChargeAmount")));
                        }
                    }
                }
            }

            if (this.IsFHL) {
                this.ValidateAirlineRule("AWBChargeRate", this.EntityPM.AWBChargeRate, screenWarnings)
            }
        }

        this.TabErrors_FRE = screenErrors;
        this.TabWarnings_FRE = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "FRE");
    }
    public ValidateScreen_OTC() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        if (this.IsFWB) {
            this.EntityPM.ShipmentAWBPrintOnlies.forEach(item => {
                Validator.TryValidateObject(item, 'ShipmentAWBPrintOnly', screenErrors);
            });

            var myCount1 = this.EntityPM.ShipmentAWBPrintOnlies.length;
            var myCount2 = this.EntityPM.ShipmentPayables.filter(d => d.ChargesGroupCode != "FRT" && d.CurrencyId == this.EntityPM.AWBCurrencyId && d.AWBPrint == true).length;
            var myCount3 = this.EntityPM.ShipmentReceivables.filter(d => d.ChargesGroupCode != "FRT" && d.CurrencyId == this.EntityPM.AWBCurrencyId && d.AWBPrint == true).length;
            var myCount = myCount1 + myCount2 + myCount3;
            if (!this.IsImportWizard) {
                if (myCount > 9) {
                    screenWarnings.push("You have exceeded the allowable limit of 9 lines of other charges");
                }
            }
        }

        this.TabErrors_OTC = screenErrors;
        this.TabWarnings_OTC = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "OTC");
    }
    public ValidateScreen_RAD() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        this.TabErrors_RAD = screenErrors;
        this.TabWarnings_RAD = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "RAD");
    }

    public ValidateScreen_GEN() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        this.ValidateScreen_GEN_FWB(screenErrors, screenWarnings);
        this.ValidateScreen_GEN_Declared(screenErrors, screenWarnings);
        this.ValidateScreen_GEN_Dangerous(screenErrors, screenWarnings);
        this.ValidateScreen_GEN_AirlineRules(screenErrors, screenWarnings);

        this.TabErrors_GEN = screenErrors;
        this.TabWarnings_GEN = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "GEN");
    }
    private ValidateScreen_GEN_FWB(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            if (this.IsFWB) {

                if (AppTool.IsNullOrEmpty(this.EntityPM.AWBSignature)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AWBSignature")));
                }

                else if (!FormatTool.IsTextFormatted(this.EntityPM.AWBSignature)) {
                    screenWarnings.push(FormatTool.GetWrongTextFormatMessage(TextCodeTranslator.Translate("Shipment.F.AWBSignature")));
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.AWBPlace)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AWBPlace")));
                }

                else if (!FormatTool.IsTextFormatted(this.EntityPM.AWBPlace)) {
                    screenWarnings.push(FormatTool.GetWrongTextFormatMessage(TextCodeTranslator.Translate("Shipment.F.AWBPlace")));
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBAccountingInformation)) {
                    if (!FormatTool.IsTextFormatted(this.EntityPM.AWBAccountingInformation)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage(TextCodeTranslator.Translate("Shipment.F.AWBAccountingInformation")));
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBHandlingInformation)) {
                    if (!FormatTool.IsTextFormatted(this.EntityPM.AWBHandlingInformation)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage(TextCodeTranslator.Translate("Shipment.F.AWBHandlingInformation")));
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBComments)) {
                    if (!FormatTool.IsTextFormatted(this.EntityPM.AWBComments)) {
                        screenWarnings.push(FormatTool.GetWrongTextFormatMessage(TextCodeTranslator.Translate("Shipment.F.AWBComments")));
                    }
                }

                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainHarmonize)) {
                    var isValid = false;

                    if (this.EntityPM.MainHarmonize.length >= 6 && this.EntityPM.MainHarmonize.length <= 18) {
                        if (FormatTool.IsAlphaNumeric(this.EntityPM.MainHarmonize)) {
                            isValid = true;
                        }
                    }

                    if (!isValid) {
                        var fieldName = TextCodeTranslator.Translate("Shipment.F.MainHarmonize");
                        screenWarnings.push(fieldName + " must be 6-18 AlphaNumeric");
                    }
                }
            }
        }
    }
    private ValidateScreen_GEN_Declared(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            if (!FormatTool.Validate_DeclaredCarriage(this.EntityPM.AWBDeclaredValueForCarriage)) {
                var fieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBDeclaredValueForCarriage");
                screenWarnings.push(fieldName + " wrong format: must be numeric Or NVD");
            }

            if (!FormatTool.Validate_DeclaredCustoms(this.EntityPM.AWBDeclaredValueForCustoms)) {
                var fieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBDeclaredValueForCustoms");
                screenWarnings.push(fieldName + " wrong format: must be numeric Or NCV");
            }

            if (!FormatTool.Validate_DeclaredInsurrence(this.EntityPM.AWBInsurrenceValue)) {
                var fieldName = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBInsurrenceValue");
                screenWarnings.push(fieldName + " wrong format: must be numeric Or XXX");
            }
        }
    }
    private ValidateScreen_GEN_Dangerous(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            if (this.EntityPM.IsDangerous) {
                var isValidSpecialHandling = false;

                if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId1)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId2)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId3)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId4)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId5)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId6)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId7)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId8)) {
                    isValidSpecialHandling = true;
                }

                else if (!AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId9)) {
                    isValidSpecialHandling = true;
                }

                if (!isValidSpecialHandling) {
                    screenWarnings.push("Shipments with Dangerous packages at least one of the special handling codes is required");
                }
            }
        }
    }
    private ValidateScreen_GEN_AirlineRules(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            this.ValidateAirlineRule("SCI", this.EntityPM.SCI, screenWarnings);

            if (!ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                this.ValidateAirlineRule("AWBAccountingInformation", this.EntityPM.AWBAccountingInformation, screenWarnings);
            }

            this.ValidateAirlineRule("AWBHandlingInformation", this.EntityPM.AWBHandlingInformation, screenWarnings);
            this.ValidateAirlineRule("AWBSpecialHandlingCodeId1", this.EntityPM.AWBSpecialHandlingCodeId1, screenWarnings);
            this.ValidateAirlineRule("AWBSpecialHandlingCodeId2", this.EntityPM.AWBSpecialHandlingCodeId2, screenWarnings);

            if (this.IsFWB) {
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId3", this.EntityPM.AWBSpecialHandlingCodeId3, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId4", this.EntityPM.AWBSpecialHandlingCodeId4, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId5", this.EntityPM.AWBSpecialHandlingCodeId5, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId6", this.EntityPM.AWBSpecialHandlingCodeId6, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId7", this.EntityPM.AWBSpecialHandlingCodeId7, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId8", this.EntityPM.AWBSpecialHandlingCodeId8, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId9", this.EntityPM.AWBSpecialHandlingCodeId9, screenWarnings);

                this.ValidateAirlineRule("ReferenceNumber", this.EntityPM.ReferenceNumber, screenWarnings);
                this.ValidateAirlineRule("SupplementaryShipmentInformation1", this.EntityPM.SupplementaryShipmentInformation1, screenWarnings);
                this.ValidateAirlineRule("SupplementaryShipmentInformation2", this.EntityPM.SupplementaryShipmentInformation2, screenWarnings);
            }
        }
    }

    public ValidateScreen_OCI() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        this.EntityPM.AWBOCIPMs.forEach(item => {
            Validator.TryValidateObject(item, 'AWBOCI', screenErrors);

            if (AppTool.IsNullOrEmpty(item.CountryId) && AppTool.IsNullOrEmpty(item.AWBCustomsInformationCode) && AppTool.IsNullOrEmpty(item.AWBInformationCode)) {
                screenErrors.push("You must fill one of the fields (Country or Information or CustomsInformation)");
            }

            if (!AppTool.IsNullOrEmpty(item.SupplementaryCustomsInfo)) {
                if (!FormatTool.IsText(item.SupplementaryCustomsInfo)) {
                    screenErrors.push(FormatTool.GetWrongTextFormatMessage(TextCodeTranslator.Translate("AWBOCI.F.SupplementaryCustomsInfo")));
                }
            }
        });
        if (!this.IsImportWizard) {
            if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                if (this.EntityPM.AWBOCIPMs.length == 0) {
                    screenWarnings.push("Please add at least one line in the OCI tab");
                }
            }
        }

        this.TabErrors_OCI = screenErrors;
        this.TabWarnings_OCI = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "OCI");
    }

    public ValidateScreen_OTP() {
        var screenErrors: string[] = [];
        var screenWarnings: string[] = [];

        if (this.IsFWB) {
            this.ValidateScreen_OTP_Participant1(screenErrors, screenWarnings);
            this.ValidateScreen_OTP_Participant2(screenErrors, screenWarnings);
            this.ValidateScreen_OTP_Participant3(screenErrors, screenWarnings);
        }

        if (this.IsFWB) {
            this.ValidateScreen_OTP_AirlineRules(screenErrors, screenWarnings);
        }

        this.TabErrors_OTP = screenErrors;
        this.TabWarnings_OTP = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "OTP");
    }
    private ValidateScreen_OTP_Participant1(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
        var isParticipantFilled = ShipmentTool.IsParticipant1Filled(this.EntityPM);
            if (isParticipantFilled) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantIdCode1)) {
                    screenWarnings.push("Other Partners Participant1 Id field is required");
                }
                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode1)) {
                    screenWarnings.push("Other Partners Participant1 Id field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationCode1)) {
                    screenWarnings.push("Other Partners Participant1 Code field is required");
                }
                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode1)) {
                    screenWarnings.push("Other Partners Participant1 Code field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationPortCode1)) {
                    screenWarnings.push("Other Partners Participant1 Port/City field is required");
                }
                else if (!FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode1)) {
                    screenWarnings.push("Other Partners Participant1 Port/City field invalid format");
                }
                else if (this.EntityPM.OtherParticipantInformationPortCode1.length != 3) {
                    screenWarnings.push("Other Partners Participant1 Port/City field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationName1)) {
                    screenWarnings.push("Other Partners Participant1 Name field is required");
                }
                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationName1)) {
                    screenWarnings.push("Other Partners Participant1 Name field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationReference1)) {
                    screenWarnings.push("Other Partners Participant1 Reference field is required");
                }
                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference1)) {
                    screenWarnings.push("Other Partners Participant1 Reference field invalid format");
                }
            }
        }
    }
    private ValidateScreen_OTP_Participant2(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            var isParticipantFilled = ShipmentTool.IsParticipant2Filled(this.EntityPM);
            if (isParticipantFilled) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantIdCode2)) {
                    screenWarnings.push("Other Partners Participant2 Id field is required");
                }
                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode2)) {
                    screenWarnings.push("Other Partners Participant2 Id field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationCode2)) {
                    screenWarnings.push("Other Partners Participant2 Code field is required");
                }
                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode2)) {
                    screenWarnings.push("Other Partners Participant2 Code field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationPortCode2)) {
                    screenWarnings.push("Other Partners Participant2 Port/City field is required");
                }
                else if (!FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode2)) {
                    screenWarnings.push("Other Partners Participant2 Port/City field invalid format");
                }
                else if (this.EntityPM.OtherParticipantInformationPortCode2.length != 3) {
                    screenWarnings.push("Other Partners Participant2 Port/City field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationName2)) {
                    screenWarnings.push("Other Partners Participant2 Name field is required");
                }
                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationName2)) {
                    screenWarnings.push("Other Partners Participant2 Name field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationReference2)) {
                    screenWarnings.push("Other Partners Participant2 Reference field is required");
                }
                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference2)) {
                    screenWarnings.push("Other Partners Participant2 Reference field invalid format");
                }
            }
        }
    }
    private ValidateScreen_OTP_Participant3(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
        var isParticipantFilled = ShipmentTool.IsParticipant3Filled(this.EntityPM);
            if (isParticipantFilled) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantIdCode3)) {
                    screenWarnings.push("Other Partners Participant3 Id field is required");
                }
                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode3)) {
                    screenWarnings.push("Other Partners Participant3 Id field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationCode3)) {
                    screenWarnings.push("Other Partners Participant3 Code field is required");
                }
                else if (!FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode3)) {
                    screenWarnings.push("Other Partners Participant3 Code field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationPortCode3)) {
                    screenWarnings.push("Other Partners Participant3 Port/City field is required");
                }
                else if (!FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode3)) {
                    screenWarnings.push("Other Partners Participant3 Port/City field invalid format");
                }
                else if (this.EntityPM.OtherParticipantInformationPortCode3.length != 3) {
                    screenWarnings.push("Other Partners Participant3 Port/City field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationName3)) {
                    screenWarnings.push("Other Partners Participant3 Name field is required");
                }
                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationName3)) {
                    screenWarnings.push("Other Partners Participant3 Name field invalid format");
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationReference3)) {
                    screenWarnings.push("Other Partners Participant3 Reference field is required");
                }
                else if (!FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference3)) {
                    screenWarnings.push("Other Partners Participant3 Reference field invalid format");
                }
            }
        }
    }
    private ValidateScreen_OTP_AirlineRules(screenErrors: string[], screenWarnings: string[]) {
        if (!this.IsImportWizard) {
            this.ValidateAirlineRule("NominatedHandlingPartyId", this.EntityPM.NominatedHandlingPartyId, screenWarnings);
            this.ValidateAirlineRule("OtherParticipantIdCode1", this.EntityPM.OtherParticipantIdCode1, screenWarnings);
            this.ValidateAirlineRule("OtherParticipantIdCode2", this.EntityPM.OtherParticipantIdCode2, screenWarnings);
            this.ValidateAirlineRule("OtherParticipantIdCode3", this.EntityPM.OtherParticipantIdCode3, screenWarnings);
        }
    }

    ValidateAirlineRule(myFieldName: string, myFieldValue: any, validationList: string[]) {
        if (this.AirlineRulesList != null) {
            var myRule = this.AirlineRulesList.filter(d => d.RuleFieldName == myFieldName)[0];
            if (myRule != null) {

                var myFieldLabel = TextCodeTranslator.Translate(this.ObjectTableName + ".F." + myFieldName);

                if (myFieldValue == null || isNaN(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                    }
                }

                else if (typeof (myFieldValue) == "string") {
                    if (AppTool.IsNullOrEmpty(myFieldValue)) {
                        if (myRule.IsMandatoryForSending) {
                            validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                        }
                    }

                    else if (myRule.MaxSize > 0) {
                        if (myFieldValue.length > myRule.MaxSize) {
                            validationList.push(myFieldLabel + " exceeds max size (" + myRule.MaxSize + ")");
                        }
                    }
                }

                else if (typeof (myFieldValue) == "number") {
                    if (AppTool.IsNullOrZero(myFieldValue)) {
                        if (myRule.IsMandatoryForSending) {
                            validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                        }
                    }
                }
            }
        }
    }

    public Fill_PAR: string = null;
    public Fill_ROU: string = null;
    public Fill_PAC: string = null;
    public Fill_FRE: string = null;
    public Fill_OTC: string = null;
    public Fill_RAD: string = null;
    public Fill_GEN: string = null;
    public Fill_OCI: string = null;
    public Fill_OTP: string = null;
    private ApplyStyle(hasErrors: boolean, hasWarnings: boolean, screenCode: string) {
        if (hasErrors) {
            switch (screenCode) {
                case "PAR": { this.Fill_PAR = "#E45A26"; break; }
                case "OTP": { this.Fill_OTP = "#E45A26"; break; }
                case "ROU": { this.Fill_ROU = "#E45A26"; break; }
                case "PAC": { this.Fill_PAC = "#E45A26"; break; }
                case "FRE": { this.Fill_FRE = "#E45A26"; break; }
                case "OTC": { this.Fill_OTC = "#E45A26"; break; }
                case "GEN": { this.Fill_GEN = "#E45A26"; break; }
                case "OCI": { this.Fill_OCI = "#E45A26"; break; }
                default: { break; }
            }
        }

        else if (hasWarnings) {
            switch (screenCode) {
                case "PAR": { this.Fill_PAR = "#FFCB00"; break; }
                case "OTP": { this.Fill_OTP = "#FFCB00"; break; }
                case "ROU": { this.Fill_ROU = "#FFCB00"; break; }
                case "PAC": { this.Fill_PAC = "#FFCB00"; break; }
                case "FRE": { this.Fill_FRE = "#FFCB00"; break; }
                case "OTC": { this.Fill_OTC = "#FFCB00"; break; }
                case "GEN": { this.Fill_GEN = "#FFCB00"; break; }
                case "OCI": { this.Fill_OCI = "#FFCB00"; break; }
                default: { break; }
            }
        }

        else {
            switch (screenCode) {
                case "PAR": { this.Fill_PAR = null; break; }
                case "OTP": { this.Fill_OTP = null; break; }
                case "ROU": { this.Fill_ROU = null; break; }
                case "PAC": { this.Fill_PAC = null; break; }
                case "FRE": { this.Fill_FRE = null; break; }
                case "OTC": { this.Fill_OTC = null; break; }
                case "GEN": { this.Fill_GEN = null; break; }
                case "OCI": { this.Fill_OCI = null; break; }
                default: { break; }
            }
        }

        this.ValidateAWB();
    }

    ValidateAWB(): boolean {

        this.ValidationWarningsList = [];

        if (this.ValidationErrorsList.length == 0) {

            if (this.isSendButtonClicked) {
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAR);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_ROU);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAC);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_FRE);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_OTC);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_RAD);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_GEN);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_OCI);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_OTP);
            }
        }

        return this.ValidationWarningsList.length == 0 ? true : false;
    }
    ValidateFSR(): boolean {

        this.ValidateAllTabs();

        this.ValidationWarningsList = [];

        if (this.IsFSRRequestButtonClicked) {
            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            if (AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                this.ValidationWarningsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.Airline")));
            }

            if (AppTool.IsNullOrEmpty(this.EntityPM.Master) && AppTool.IsNullOrEmpty(this.EntityPM.MAWBStackNumber)) {
                this.ValidationWarningsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.O.Routings.MAWB")));
            }
        }

        return this.ValidationWarningsList.length == 0 ? true : false;
    }
    ValidateShipment(): boolean {

        this.ValidateAllTabs();

        this.ValidationErrorsList = [];
        this.IsValidationSingleLine = false;

        var errors: string[] = AWBHelper.ValidateShipment(this.EntityPM);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(errors);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAR);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_ROU);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAC);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_FRE);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_OTC);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_RAD);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_GEN);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_OCI);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_OTP);
        
        if (this.PageChild_ROU != null) {
            if (this.PageChild_ROU.MasterFieldValidityMessage != null) {
                var errors_ROU: string[] = [];
                errors_ROU.push(this.PageChild_ROU.MasterFieldValidityMessage);
                this.ValidationErrorsList = this.ValidationErrorsList.concat(errors_ROU);
            }            
        }

        return this.ValidationErrorsList.length == 0 ? true : false;
    }

    // Commands
    private isSendingFHLs: boolean = false;
    private isSendingDEXX: boolean = false;
    private isSendingCargonaut: boolean = false;
    private isSaveButtonClicked: boolean = false;
    private isSendButtonClicked: boolean = false;
    private isPrintButtonClicked: boolean = false;
    private isPreviewButtonClicked: boolean = false;
    private isConfirmCloseClicked: boolean = false;
    private isFullDetailsButtonClicked: boolean = false;
    private isCopyShipmentButtonClicked: boolean = false;
    private isCancelShipmentButtonClicked: boolean = false;
    private isReactivateShipmentButtonClicked: boolean = false;
    private isSendToAirlineTenantButtonClicked: boolean = false;
    public IsFSRRequestButtonClicked: boolean = false;
    private InitFlags() {
        this.isSendingFHLs = false;
        this.isSendingDEXX = false;
        this.isSendingCargonaut = false;
        this.isSaveButtonClicked = false;
        this.isSendButtonClicked = false;
        this.isPrintButtonClicked = false;
        this.isPreviewButtonClicked = false;
        this.isConfirmCloseClicked = false;
        this.IsFSRRequestButtonClicked = false;
        this.isFullDetailsButtonClicked = false;
        this.isCopyShipmentButtonClicked = false;
        this.isCancelShipmentButtonClicked = false;
        this.isReactivateShipmentButtonClicked = false;
        this.isSendToAirlineTenantButtonClicked = false;
    }

    public IsCancelButtonDisabled: boolean = false;
    public IsReactivateButtonDisabled: boolean = false;
    public IsSendToAirlineTenantVisible: boolean = false;
    private SetMoreButtons() {
        this.IsCancelButtonDisabled = false;
        this.IsReactivateButtonDisabled = false;

        if (this.EntityPM.IsAccountingClosed || this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || (this.EntityPM.MasterShipmentDataId != null && this.EntityPM.MasterShipmentDataId != this.EntityPM.Id)) {
            this.IsCancelButtonDisabled = true;
        }

        if (!this.EntityPM.IsCancelled) {
            this.IsReactivateButtonDisabled = true;
        }

        if (this.CurrentSession.CurrentWindow != null) {
            this.CurrentSession.CurrentWindow.ShowCancelControl(this.EntityPM.IsCancelled);
        }

        if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Action.SendToAirlineTenant")) {
            this.IsSendToAirlineTenantVisible = true;
        }
    }

    CopyShipmentClicked() {
        this.InitFlags();
        this.isCopyShipmentButtonClicked = true;

        var isValid: boolean = this.ValidateShipment();

        if (isValid) {
            this.Save();
        }
    }
    CancelShipmentClicked() {
        var confirmMsg: string;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Title = "Cancelling Shipment";
            messageWindow.Show("Can't cancel shipments that have a MAWB number, please remove it");
        }

        else if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
            confirmMsg = "Cancelling this shipment will disconnect it from the Booking , are you sure you want to cancel?";
            this.ConfirmCanceling(confirmMsg);
        }
        
        else {
            confirmMsg = "Are you sure you want to cancel this Shipment?";
            this.ConfirmCanceling(confirmMsg);
        }
    }
    ConfirmCanceling(confirmMsg: string) {
        var confirmWindow = new ConfirmWindow();

        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                var isValid: boolean = this.ValidateShipment();

                if (isValid) {
                    this.InitFlags();

                    this.isCancelShipmentButtonClicked = true;

                    this.EntityPM.IsCancelled = true;

                    this.isReloadingOnSave = true;

                    if (AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
                            this.EntityPM.MAWBReturnedToStack = true;
                            this.EntityPM.MAWBReturnedToStackWithCancel = true;
                            this.EntityPM.MAWBStackNumber = this.EntityPM.Master;
                        }
                    }

                    this.Save();
                }
            }
        });
    }

    ReactivateShipmentClicked() {
        this.InitFlags();
        this.isReactivateShipmentButtonClicked = true;

        var isValid: boolean = this.ValidateShipment();

        if (isValid) {
            this.Save();
        }
    }
    SendToAirlineTenantClicked() {
        this.InitFlags();
        this.isSendToAirlineTenantButtonClicked = true;

        var isValid: boolean = this.ValidateShipment();

        if (isValid) {
            this.Save();
        }
    }
    SendClicked(typeCode: string = null) {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        this.InitFlags();
        this.isSendButtonClicked = true;

        if (typeCode == "CARG") {
            this.isSendingCargonaut = true;
        }

        else if (typeCode == "DEXX") {
            this.isSendingDEXX = true;
        }

        var isValid: boolean = this.ValidateShipment();

        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }

        if (isValid) {
            this.Save();
        }

        else {
            this.StopBusyIndicator();
        }
    }
    SendFHLsClicked(typeCode: string = null) {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        this.InitFlags();
        this.isSendingFHLs = true;
        this.isSendButtonClicked = true;

        if (typeCode == "CARG") {
            this.isSendingCargonaut = true;
        }

        else if (typeCode == "DEXX") {
            this.isSendingDEXX = true;
        }

        var isValid: boolean = this.ValidateShipment();

        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }

        if (isValid) {
            this.Save();
        }

        else {
            this.StopBusyIndicator();
        }
    }
    SaveFSR() {
        this.InitFlags();
        this.IsFSRRequestButtonClicked = true;

        var isValid = this.ValidateShipment();

        if (isValid) {
            isValid = this.ValidateFSR();
        }

        if (isValid) {
            this.Save();
        }
    }
    CloseClicked() {

        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", this.ObjectTableName));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.isConfirmCloseClicked = true;

                    var isValid: boolean = this.ValidateShipment();

                    if (isValid) {
                        this.Save();
                    }
                }

                else if (confirmWindow.No) {
                    this.CloseWizardWindow();
                }
            });
        }

        else {
            this.CloseWizardWindow();
        }
    }
    SaveClicked() {
        this.InitFlags();
        this.isSaveButtonClicked = true;

        var isValid: boolean = this.ValidateShipment();

        if (isValid) {
            this.Save();
        }

        else {
            this.SaveCompleted.emit(false);
        }
    }

    private myService: ShipmentPMService;
    private Save() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        if (this.EntityPM.IsDirty) {
            if (this.EntityPM.Id == null) {
                this.isReloadingOnSave = true;
                this.SubmitCreatingShipment();
            }

            else {

                var isConfirmingPorts: boolean = false;
                if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                    if (this.EntityPM.OriginMainCarriageFromPortId != this.EntityPM.MainCarriageFromPortId) {
                        isConfirmingPorts = true;
                    }

                    else if (this.EntityPM.OriginFinalDestinationPortId != this.EntityPM.MainCarriageFinalDestinationPortId) {
                        isConfirmingPorts = true;
                    }
                }

                if (isConfirmingPorts) {

                    this.StopBusyIndicator();

                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Title = "Ports Changed";
                    confirmWindow.Show("Updating the Master shipment ports will update the house shipment accordingly");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {

                            this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
                            this.SubmitUpdatingShipment();                            
                        }
                    });
                }

                else {
                    this.SubmitUpdatingShipment();
                }
            }
        }

        else {
            this.StopBusyIndicator();
            this.OnSaveCompletedSuccessfully();
            this.SaveCompleted.emit(true);
        }
    }
    private SubmitCreatingShipment() {
        if (this.myService == null) {
            this.myService = new ShipmentPMService();
        }

        this.myService.insert(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
            if (!myRespone.HasError) {
                this.EntityPM = myRespone.Result;

                if (this.isReloadingOnSave) {
                    this.OnSaveCompletedSuccessfully();
                }

                else {
                    this.SaveCompleted.emit(true);
                    this.OnSaveCompletedSuccessfully();
                }
            }

            else {
                this.IsValidationSingleLine = true;
                this.ValidationErrorsList = myRespone.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
                this.SaveCompleted.emit(false);
            }
        });
    }
    private SubmitUpdatingShipment() {

        if (this.myService == null) {
            this.myService = new ShipmentPMService();
        }

        this.myService.update(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
            if (!myRespone.HasError) {
                this.EntityPM = myRespone.Result;

                if (this.isReloadingOnSave) {
                    this.OnSaveCompletedSuccessfully();
                }

                else {
                    this.SaveCompleted.emit(true);
                    this.OnSaveCompletedSuccessfully();
                }               
            }

            else {
                this.IsValidationSingleLine = true;
                this.ValidationErrorsList = myRespone.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
                this.SaveCompleted.emit(false);
            }
        });
    }
    private OnSaveCompletedSuccessfully() {
        if (this.isReloadingOnSave) {
            this.isReloadingOnSave = false;
            this.isExecutingMethod = true;
            this.ReloadShipment();
        }

        else {
            this.isExecutingMethod = true;
            this.ExecuteRequestedMethod();
        }
    }

    private isReloadingOnSave: boolean;
    private isExecutingMethod: boolean;
    private ExecuteRequestedMethod() {
        if (this.isExecutingMethod) {

            if (this.isSaveButtonClicked) {
                this.StopBusyIndicator();
            }

            else if (this.isFullDetailsButtonClicked) {
                //this.ViewFullDetails();
            }

            else if (this.isPrintButtonClicked) {
                this.Print();
            }

            else if (this.isSendButtonClicked) {
                this.ExecuteSend();
            }

            else if (this.isPreviewButtonClicked) {
                this.Preview();
            }

            else if (this.isCopyShipmentButtonClicked) {
                this.StopBusyIndicator();
                this.CopyShipment();
            }

            else if (this.isReactivateShipmentButtonClicked) {
                this.StopBusyIndicator();
                this.ReactivateShipment();
            }

            else if (this.isConfirmCloseClicked) {
                this.CloseWindow();
            }

            else if (this.isSendToAirlineTenantButtonClicked) {
                this.StopBusyIndicator();
                this.SendToAirlineTenant();
            }

            this.isSaveButtonClicked = false;
            this.isExecutingMethod = false;
            this.isReloadingOnSave = false;
        }
    }

    
    Print() {
        var isRunningPrintingManager: boolean = false;

        if (!this.IsImportWizard) {
            if (FeatureLocator.IsPackage_EAWB()) {
                if (SessionLocator.TenantManagementJS.IsAWBStockPrepaid) {
                    var isDemoTenant = false;

                    if (ObjectsLocator.IsDemoTenant(this.TenantPM.Id.toString()) || SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
                        isDemoTenant = true;
                    }

                    if (!isDemoTenant) {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            if (!AppTool.IsNullOrEmpty(this.EntityPM.TenantZeroAirlineTTY)) {
                                if (this.EntityPM.FWBStatusCode == "NSEN") {
                                    isRunningPrintingManager = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (isRunningPrintingManager) {
            this.RunPrintingManager(false);
        }

        else {
            this.ExecutePrinting();
        }
    }

    RunPrintingManager(isConfirmedByUser: boolean) {

        this.CurrentSession.StartBusyIndicatorLoading();
        var myService = new CCSWebService();

        myService.GetAWBPrintingStock(this.EntityPM.Id, this.isSendingCargonaut, this.isSendingDEXX, isConfirmedByUser).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (!myResponse.HasError) {
                var myResult: AWBPrintResult = myResponse.Result;
                if (myResult) {
                    if (myResult.IsConfirmedByUser) {
                        if (myResult.IsPrintingAllowed) {
                            this.ShowPrintingStockResult(myResult);
                        }

                        else {
                            var logitudeWindow = new LogitudeWindow();
                            logitudeWindow.Width = 370;
                            logitudeWindow.Height = 100;
                            logitudeWindow.IsShowCloseButton = true;
                            logitudeWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Others/PurchaseStockComponent');
                        }
                    }

                    else {
                        if (myResult.IsPrintingAllowed) {
                            this.ExecutePrinting();
                        }

                        else {
                            this.ConfirmPrintingStock();
                        }
                    }
                }
            }
        });
    }

    ConfirmPrintingStock() {
        var confirmMsg = "Please note that printing before sending will use one messaging stock. note that when sending the message it will be sent using the same stock";
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;       
        confirmWindow.ShowCancelButton = false;
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.RunPrintingManager(true);
            }
        });
    }

    ShowPrintingStockResult(myResult: AWBPrintResult) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show("Stock before the printing: " + myResult.StockRemainingBefore + ", Stock after the printing: " + myResult.StockRemainingAfter);
        messageWindow.WindowClosed.subscribe(($event: any) => {
            this.ExecutePrinting();
        });
    }


    ExecutePrinting() {
        this.isPrintButtonClicked = true;
        ServiceLocator.SendTotangoUserActivity("AWBWizard", "Print");

        var oldCode = this.documentTypeCode;
        this.documentTypeCode = this.selectedDocumentTypeClass.Code;
        this.documentTypeName = this.selectedDocumentTypeClass.Name;


        if (oldCode == this.selectedDocumentTypeClass.Code) {
            this.StartPrint();
        }

        else {
            this.GetDocstOut();
        }

    }
    StartPrint() {
        if (this.documentTypePM != null) {
            if (this.documentTypePM.DocumentTypeDefaultReportTemplateId) {
                this.LoadPrintControl();
            }

            else {
                var docType = this.documentTypeName;//(shipmentPM.ShipmentLevelCode == "H") ? "HAWB" : "AWB";
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show(docType + " document has no template!!");


                this.StopBusyIndicator();
            }
        }

        else {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Can't find document type!!");
            this.StopBusyIndicator();
        }

    }
    documentOutPmLists: DocumentOutPM[];
    isPrintWindowOpened: boolean;
    LoadPrintControl() {
       // this.StopBusyIndicator();

        if (!this.isPrintWindowOpened) {

            this.documentOutPmLists = new Array<DocumentOutPM>();

            this.isPrintWindowOpened = true;
            this.documentOutPM.NeedsRebuild = true;

            this.documentOutPmLists.push(this.documentOutPM);
            var SelectedInternalDocument = new DocsOutDataViewModel(this.documentTypePM, this.EntityPM.Id, this.documentOutPM.ChildEntityId, this.targetObjectTableId, "", this.documentOutPM.ChildEntityReference, this.documentOutPmLists, null, null, this.EntityPM);
            SelectedInternalDocument.IsNotFromDocsOutListOpenPrintControl = true;
            SelectedInternalDocument.EntityId = this.EntityPM.Id;
            SelectedInternalDocument.IsAWBWizard = true;
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 760;
            logitudeWindow.Height = 502;
            logitudeWindow.DataContext = SelectedInternalDocument;
            logitudeWindow.Title = "Print " + this.documentTypePM.Name;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent');

            logitudeWindow.WindowClosed.subscribe(($event: any) => {
                this.isPrintWindowOpened = false;
                this.StopBusyIndicator();
            });
        }
    }
    documentTypeId: string;
    documentOutPM: any;
    documentTypePM: DocumentTypePM;
    documentTypeList: DocumentTypeList;
    GetDocstOut() {
        this._documentTypeListExtendedService.getDocumentTypeListByCode(this.documentTypeCode, this.TenantPM.Id).subscribe((res:any) => {



            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentTypeList = myResult;

                    this.documentTypeId = this.documentTypeList.Id;
                    this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
                    this._documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.EntityPM.Id, this.TenantPM.Id, "", this.documentTypeId).subscribe((res:any) => {
               
                        this.StopBusyIndicator();

                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;

                            this.documentOutPM = myResult;

                            if (!this.documentOutPM) {

                                this._documentOutPMService.getCreateDocumentOut(this.documentTypeId, this.EntityPM.Id, null, null, this.targetObjectTableId, this.EntityPM.Tenant).subscribe((res:any) => {
                                        var pmResponse: ServiceResponse = res;
                                        if (!pmResponse.HasError) {
                                            var myResult = pmResponse.Result;
                                            if (myResult) {
                                                this.documentOutPM = myResult;
                                                this.LoadCreatedDocMethod();
                                            }
                                            else this.StopBusyIndicator();
                                        }

                                    });


                                }
                                else {
                                    this.LoadCreatedDocMethod();
                                }

                        }



                       

                    });

                }
                else {
                    this.documentTypePM = null;
                    this.StopBusyIndicator();
                }
            }

            else {
                this.documentTypePM = null;
                this.StopBusyIndicator();
            }
        });


    }
    LoadCreatedDocMethod() {

        this._documentOutPMService.getSingleDocumentOutPM(this.documentOutPM.Id, this.TenantPM.Id).subscribe((res:any) => {
     


            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentOutPM = myResult;
                    this.LoadDocumentTypeMethod();
                } else this.StopBusyIndicator();

            }


        });


    }
    LoadDocumentTypeMethod() {

        this._documentTypePMService.getSingleDocumentType(this.documentTypeId, this.documentOutPM.Id, this.TenantPM.Id).subscribe((res:any) => {


            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentTypePM = myResult;
                    this.isCurrentDocsOutLoaded = true;

                    if (this.isPrintButtonClicked) {
                        this.StartPrint();
                    }

                    else if (this.isPreviewButtonClicked) {
                        this.StartPreview();
                    }
                }
                else this.StopBusyIndicator();

            }



      
   


        });


    }
    StartPreview() {


        this.StopBusyIndicator();

        if (this.documentTypePM.DocumentTypeDefaultReportTemplateId) {
            this.LoadPreviewControl();
        }

        else {
            var docType = (this.EntityPM.ShipmentLevelCode == "H") ? "HAWB" : "AWB";
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show(docType + " document has no template!!");


        }

    }
    isPreviewWindowOpend: boolean;
    LoadPreviewControl() {
        if (!this.isPreviewWindowOpend) {
            this.isPreviewWindowOpend = true;

            var windowArgs: any = {};
            var logWindow = new LogitudeWindow();
            windowArgs.ModePage = "Preview";
            windowArgs.PageType = "EditDocument";
            windowArgs.WindowHeight = window.innerHeight - 100;
            windowArgs.WindowWidth = window.innerWidth - 100;
            windowArgs.CurrentDocument = this.documentOutPM;
            windowArgs.DocumentTypePM = this.documentTypePM;
            windowArgs.EntityId = this.EntityPM.Id;
            windowArgs.DataViewModel = this;
            
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = windowArgs.WindowWidth;
            logWindow.Height = windowArgs.WindowHeight;
            logWindow.Title = this.documentTypePM.Name + " Preview";
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/EditDocumentComponent');

            logWindow.WindowClosed.subscribe(($event: any) => {
                this.isPreviewWindowOpend = false;
                this.StopBusyIndicator();
            });


          
        }
    }
    PrintAWBMethod(item: DocumentTypeClass) {

        this.selectedDocumentTypeClass = item;

        this.InitFlags();
        this.isPrintButtonClicked = true;

        var isValid = this.ValidateShipment();

        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }

        if (isValid) {
            this.Save();
        }

    }
    PreviewAWBMethod() {
        this.InitFlags();
        this.isPreviewButtonClicked = true;

        var isValid = this.ValidateShipment();

        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }

        if (isValid) {
            this.Save();
        }


    }
    Preview() {
        if (this.EntityPM.ShipmentLevelCode == "H") {
            this.documentTypeCode = "714";
        }
        else {
            this.documentTypeCode = "740";
        }

        this.GetDocstOut();
    }

    private ExecuteSend() {

        var isValidForSending = true;
        var validationErrorMessage = "";

        var isCargonautEnabled = SessionLocator.TenantManagementJS.IsCargonautEnabled;
        var isDEXXConnectionEnabled = SessionLocator.TenantManagementJS.IsDEXXConnectionEnabled;
        var isDemoTenantManagement = SessionLocator.TenantManagementJS.IsEAWBOnlyDemo;

        var myCCSValidator: AWBCCSValidator = AWBHelper.ValidateAWBCCS(this.EntityPM);

        if (isValidForSending) {
            if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId == null) {
                isValidForSending = false;
                validationErrorMessage = " FHL can’t be sent, House isn't connected to a Master shipment";
            }
        }

        if (isValidForSending) {
            if (!isDemoTenantManagement) {
                if (myCCSValidator.TenantManagementFieldHasError) {
                    isValidForSending = false;
                    validationErrorMessage = myCCSValidator.TenantManagementFieldErrorMessage;
                }
            }
        }

        if (isValidForSending) {
            if (this.isSendingCargonaut && !isCargonautEnabled) {
                isValidForSending = false;
                validationErrorMessage = "Cargonaut is not connected. Please contact your account manager";
            }

            else if (this.isSendingDEXX && !isDEXXConnectionEnabled) {
                isValidForSending = false;
                validationErrorMessage = "DEXX is not connected. Please contact your account manager";
            }
        }

        if (!this.isSendingCargonaut && !this.isSendingDEXX) {
            if (isValidForSending) {
                if (myCCSValidator.AirlineFieldHasError) {
                    isValidForSending = false;
                    validationErrorMessage = myCCSValidator.AirlineFieldErrorMessage;
                }
            }

            if (isValidForSending) {
                if (this.EntityPM.ShipmentLevelCode == "H" || this.isSendingFHLs) {
                    if (!myCCSValidator.FHL) {
                        isValidForSending = false;
                        validationErrorMessage = "This airline will not receive FHL";
                    }
                }
            }

            if (isValidForSending) {
                if (!myCCSValidator.FWB) {
                    isValidForSending = false;
                    validationErrorMessage = "This airline will not receive FWB";
                }
            }

            if (isValidForSending) {
                if (!isDemoTenantManagement) {
                    if (myCCSValidator.AirlineRegistrationHasError) {
                        isValidForSending = false;
                        validationErrorMessage = myCCSValidator.AirlineRegistrationErrorMessage;
                    }
                }
            }
        }

        if (!isValidForSending) {
            this.StopBusyIndicator();
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show(validationErrorMessage);
        }

        else {
            var RateDescriptionMaxOccurs: number = 11;
            var DimensionsLinesMaxOccurs: number = RateDescriptionMaxOccurs - 1;
             
            if (this.IsFWB) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.MainHarmonize)) {
                    if (this.EntityPM.MainHarmonize.length >= 6 && this.EntityPM.MainHarmonize.length <= 18) {
                        if (FormatTool.IsAlphaNumeric(this.EntityPM.MainHarmonize)) {                            
                            DimensionsLinesMaxOccurs -= 1;
                        }
                    }
                }
            }

            if (this.EntityPM.ShipmentPackages.length > DimensionsLinesMaxOccurs) {

                this.StopBusyIndicator();

                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("Due to IATA limitation, please notice that not all the packages lines full details will be sent, some lines will be sent as a part of the total commodity.");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.ExecuteSendConfirmed();
                    }
                });
            }

            else {
                this.ExecuteSendConfirmed();
            }
        }
    }
    private ExecuteSendConfirmed() {
        var windowTitle = "";
        var totangoUserActivity = "";

        if (this.isSendingFHLs) {
            if (this.isSendingCargonaut) {
                windowTitle = "Send All Cargonaut FHL(s)";
                totangoUserActivity = "Sending All Cargonaut FHL(s)";
            }

            else if (this.isSendingDEXX) {
                windowTitle = "Send All DEXX FHL(s)";
                totangoUserActivity = "Sending All DEXX FHL(s)";
            }

            else {
                windowTitle = "Send All FHL(s)";
                totangoUserActivity = "Sending All FHL(s)";
            }
        }

        else if (this.EntityPM.ShipmentLevelCode == "H") {
            if (this.isSendingCargonaut) {
                windowTitle = "Send Cargonaut FHL";
                totangoUserActivity = "Sending Cargonaut FHL";
            }

            else if (this.isSendingDEXX) {
                windowTitle = "Send DEXX FHL";
                totangoUserActivity = "Sending DEXX FHL";
            }

            else {
                windowTitle = "Send FHL";
                totangoUserActivity = "Sending FHL";
            }
        }

        else {
            if (this.isSendingCargonaut) {
                windowTitle = "Send Cargonaut FWB";
                totangoUserActivity = "Sending Cargonaut FWB";
            }

            if (this.isSendingDEXX) {
                windowTitle = "Send DEXX FWB";
                totangoUserActivity = "Sending DEXX FWB";
            }

            else {
                windowTitle = "Send FWB";
                totangoUserActivity = "Sending FWB";
            }
        }

        ServiceLocator.SendTotangoUserActivity("AWBWizard", totangoUserActivity);
        this.RunSendWindow(windowTitle);
    }

    private isSendWindowOpen: boolean = false;
    private RunSendWindow(windowTitle: string) {

        this.StopBusyIndicator();

        if (!this.isSendWindowOpen) {

            this.isSendWindowOpen = true;

            var args = new SendAWBArgs();
            args.EnttiyPM = this.EntityPM;
            args.Wizard = this;
            args.IsSendingFHLs = this.isSendingFHLs;
            args.IsSendingDEXX = this.isSendingDEXX;
            args.IsSendingCargonaut = this.isSendingCargonaut;

            var logWindow = new LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = args;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/SendWindowComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.isSendWindowOpen = false;
                this.StopBusyIndicator();
            });
        }
    }

    private CloseWizardWindow() {
        this.CloseWindow();
    }
    private CloseWindow() {
        this.CurrentSession.CloseCurrentWindow();
    }
    private CopyShipment() {

        var windowArgs: AWBWizardArgs = new AWBWizardArgs();
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
        windowArgs.IsCopyFromShipment = true;
        windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Copy Shipment";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
    }
    private ReactivateShipment() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Title = "Reactivate Shipment";
        logWindow.WindowClosed.subscribe(($event: any) => this.OnShipmentReactivate($event));
        logWindow.Show('./Shipment/Components/Reactivate/ReactivateShipmentComponent');
    }
    private SendToAirlineTenant() {

        this.StartBusyIndicator("Sending...");

        var myService = new InfrastructureDomainService();
        myService.SendEntityToAirlineTenant(this.EntityPM.Id, "Shipment", this.EntityPM.MainCarriageCarrierCode).subscribe((myResponse: ServiceResponse) => {
            this.StopBusyIndicator();

            if (myResponse.HasError) {
                var messageWindow = new MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
            }
        });
    }

    OnShipmentReactivate($event: any) {
        if ($event == 'OK') {
            this.SetMoreButtons();
            this.SaveCompleted.emit(true);
        }
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private ReloadShipment() {
        var myService: ShipmentPMService = new ShipmentPMService();

        myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.LoadCompleted.emit(false);
                }

                else {
                    this.EntityPM = myResponse.Result;
                    this.LoadCompleted.emit(true);
                }
            }

            this.CurrentSession.StopBusyIndicator();

            this.SetMoreButtons();
            this.ExecuteRequestedMethod();
        });
    }
    public ReloadEntity() {

        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myService == null) {
            this.myService = new ShipmentPMService();
        }

        this.myService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                    this.LoadCompleted.emit(true);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.LoadCompleted.emit(false);
                }

                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}

class DocumentTypeClass {
    public Code: string = null;
    public Name: string = null;
    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }
}
