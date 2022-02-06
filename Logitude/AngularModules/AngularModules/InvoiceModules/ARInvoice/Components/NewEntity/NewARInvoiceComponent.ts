import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoiceLinePM} from '../../../../Invoice/EntityPMs/ARInvoiceLinePM';
import {ARInvoiceTotalVATPM} from '../../../../Invoice/EntityPMs/ARInvoiceTotalVATPM';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentReceivablePM} from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {ARInvoicePMService} from '../../../../Invoice/Services/StandardPMs/ARInvoicePMService';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateTool, AppTool, FormatTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InvoiceTool, InvoicePartnerType} from '../../../../Invoice/Tools';
import {InvoiceTotalsClass} from '../../../../Invoice/Args';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {VatTypeList} from '../../../../Common/EntityLists/VatTypeList';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {VatTypeListService} from '../../../../Common/Services/StandardLists/VatTypeListService';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {InvoiceDomainService} from '../../../../Invoice/Services/InvoiceDomainService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './NewARInvoiceComponent.html',
})

export class NewARInvoiceComponent extends BaseComponent {
    public EntityPM: ARInvoicePM;
    public ObjectTableName: string = "ARInvoice";
    public DataContext = this;
    public InvoicePartners: InvoicePartnerType[] = [];
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");          

        this.InitializeServices();

       

        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "Intercompany")) {
            this.IsIntercompanyVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }

        this.SetRegionalTaxVisibility();
    }

    public DisplaySATSettings: boolean = false;
    public IsIntercompanyVisible: boolean = false;
    public AllVatTypes: VatTypeList[] = [];
    private myCardListService: CardListService;
    private myCurrencyListService: CurrencyListService;
    private myPaymentTermListService: PaymentTermListService;
    private myVatTypeListService: VatTypeListService;
    private myChargesTypeListService: ChargesTypeListService;
    private myCommonDomainService: CommonDomainService;
    private myInvoiceDomainService: InvoiceDomainService;
    private myEntityPMService: ARInvoicePMService;
    InitializeServices() {
        this.myCardListService = new CardListService();
        this.myCurrencyListService = new CurrencyListService();
        this.myPaymentTermListService = new PaymentTermListService();
        this.myVatTypeListService = new VatTypeListService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.myCommonDomainService = new CommonDomainService();
        this.myInvoiceDomainService = new InvoiceDomainService();
        this.myEntityPMService = new ARInvoicePMService();

        this.myVatTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatTypes = myResponse.Result;
            }
        });
    }

    private shipmentPM: ShipmentPM;
    private InvoiceTypeCode: string = null;
    private EntityLevelCode: string = null;
    private EntityTableName: string = null;
    private EntityReceivables: any[] = [];
    private EntitySalesmanUserId: string = null;

    SetWindowArgs(myarguments: any) {
        this.shipmentPM = myarguments["Shipment"];
        this.InvoiceTypeCode = myarguments["InvoiceTypeCode"];
        this.EntityLevelCode = myarguments["EntityLevelCode"];
        this.EntityTableName = myarguments["EntityTableName"];
        this.EntityReceivables = myarguments["EntityReceivables"];

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = this.myEntityPMService.GetNewEntityPM();
            this.EntityPM.ARInvoiceTypeCode = this.InvoiceTypeCode;
            this.EntityPM.PrepaidCollectId = this.InvoiceTypeCode == "MN" ? "C" : "B";
            this.EntityPM.IsCustomsChargesOnly = (this.InvoiceTypeCode == "CI" || this.InvoiceTypeCode == "CC") ? true : false;
            this.EntityPM.MainEntityId = this.shipmentPM.Id;
            this.EntityPM.MainEntityReference = this.shipmentPM.ShipmentNumber;
            this.EntityPM.ShipmentsNumbers = this.shipmentPM.ShipmentNumber;
            this.EntityPM.HouseNumber = this.shipmentPM.House;
            this.EntityPM.MasterNumber = this.shipmentPM.LongMaster;
            this.EntityPM.ProfitCurrencyId = this.shipmentPM.ProfitCurrencyId;
            this.EntityPM.OperationalDate = InvoiceTool.GetOperationalDate(this.shipmentPM);
            this.EntityPM.BranchId = this.shipmentPM.BranchId;
            this.EntityPM.SalesmanUserId = this.shipmentPM.SalesmanUserId;
            this.EntityPM.SalesmanUserName = this.shipmentPM.SalesmanUserName;
            this.EntitySalesmanUserId = this.shipmentPM.SalesmanUserId;

            var myDescription: string = null;
            switch (this.shipmentPM.DirectionId) {
                case "E": { myDescription = "Export to " + this.shipmentPM.MainCarriageFinalDestinationPortCode; break; }
                case "I": { myDescription = "Import from " + this.shipmentPM.MainCarriageFromPortCode; break; }
                case "D": { myDescription = "Ship to " + this.shipmentPM.ToPartnerCity; break; }
            }
            this.EntityPM.Description = myDescription;

            if (SessionLocator.TenantPM.AccountingActivated) {
                this.EntityPM.IsFullAccounting = true;
            }
            if (this.InvoiceTypeCode == "MN" || this.shipmentPM.ShipmentLevelCode == "C") {
                this.EntityTableName = "Master";
            }

            else {
                this.EntityTableName = "Shipment";
            }

            this.IsResourcesReady = true;

            this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
            this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;

            this.SetUIProperties();
            this.BuildPartnersTypes();
           this.LoadData();

            if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40") {
            this.DisplaySATSettings = true;
            this.MetodoPagoCode = SessionLocator.SATInterfaceSettings.MetodoPagoCode;

            if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
              this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, true);
            }
          }
        });
    }

    // SetUIProperties
    public RateIsEnabled: boolean = false;
    public PaymentTermDisplayInLOV: boolean = true;
    public IsConstituentInvoiceVisible: boolean = true;
    private SetUIProperties() {
        this.SetUIProperties_BillTo();
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_ExchangeRate();
        this.SetUIProperties_Constituent(false);
        this.SetUIProperties_DueDate();
        this.SetUIProperties_Payment();
    }
    SetUIProperties_BillTo() {
        var isBillToEnabled = false;
        var isBillToAddressEnabled = false;

        if (this.SelectedPartnerType) {
            if (!AppTool.IsNullOrEmpty(this.BillToPartnerTypeId)) {
                if (this.SelectedPartnerType.Code == "OTH") {
                    isBillToEnabled = true;
                }
            }
        }

        if (!AppTool.IsNullOrEmpty(this.BillToId)) {
            isBillToAddressEnabled = true;
        }

        this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PartnerId", this.ObjectTableName, isBillToEnabled);
        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, isBillToAddressEnabled);
    }

    SetUIProperties_VatNumber() {
        var isFieldRequired = false;

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (AppTool.IsNullOrEmpty(this.VatNumber)) {
                isFieldRequired = true;
            }

            this.UIProperties.SetRequired("VatNumber", this.ObjectTableName, isFieldRequired);
        }
    }
    SetUIProperties_ExchangeRate() {
        var isFieldtEnabled = false;

        if (FeatureLocator.HasFeaturePermession("ARInvoice", "ARInvoiceEditExchangeRate")) {
            if (this.InvoiceCurrencyId) {
                if (this.InvoiceCurrencyId != SessionLocator.TenantPM.CurrencyId) {
                    isFieldtEnabled = true;
                }
            }
        }

        this.RateIsEnabled = isFieldtEnabled;
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, isFieldtEnabled);
    }
    SetUIProperties_Constituent(isBillToHasConstituentEnabled: boolean) {
        var isFieldVisible = false;
        var isFieldtEnabled = false;

        if (FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent")) {

            isFieldVisible = true;
            isFieldtEnabled = true;

            if (this.EntityPM.ARInvoiceTypeCode == "CI" || this.EntityPM.ARInvoiceTypeCode == "CC") {                
                isFieldVisible = false;
            }

            //isFieldtEnabled = isBillToHasConstituentEnabled;

            if (AppTool.IsNullOrEmpty(this.BillToId)) {
                isFieldtEnabled = false;
            }
        }

        this.IsConstituentInvoiceVisible = isFieldVisible;
        this.UIProperties.SetEnabled("IsConstituentInvoice", this.ObjectTableName, isFieldtEnabled);
        //this.UIProperties.SetVisibility("IsConstituentInvoice", this.ObjectTableName, isFieldVisible);
    }
    SetUIProperties_DueDate() {
        var AllowManuallyDueDate: boolean = false;

        if (SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }

        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);

        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
    }
    SetUIProperties_Payment() {
      this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, false);
      this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, false);

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40") {
        if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
          this.UIProperties.SetRequired("SATPaymentMethodCode", this.ObjectTableName, true);
        }

        if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
          this.UIProperties.SetRequired("MetodoPagoCode", this.ObjectTableName, true);
        }
      }
    }

    // BillTo
    public BillToDependencyValue1: string = null;
    public BillToDependencyValue1IsList: boolean = true;
    BuildPartnersTypes() {

        this.BillToDependencyValue1 = InvoiceTool.GetBillToPartnerTypes();
        this.InvoicePartners = InvoiceTool.GetARInvoicePartners(this.shipmentPM);       

        if (this.EntityPM.Id == null) {
            if (this.EntityTableName == "Shipment") {
                this.PartnersTypeSelectionMethod(this.InvoicePartners.filter(d => d.Code == "CUS")[0]);
            }

            else if (this.EntityTableName == "Master") {
                this.PartnersTypeSelectionMethod(this.InvoicePartners.filter(d => d.Code == "AGE")[0]);
            }
        }
    }

    public SelectedPartnerType: InvoicePartnerType = null;
    PartnersTypeSelectionMethod(selected: InvoicePartnerType) {
        if (this.SelectedPartnerType != selected) {
            this.SelectedPartnerType = selected

            this.PartnerId = null;
            this.BillToId = null;
            this.BillToAddressId = null;
            this.BillToPartnerTypeId = null;

            var myReference: string = null;
            if (selected) {
                this.BillToPartnerTypeId = selected.PartnerTypeId;

                switch (selected.Code) {
                    case "CUS":
                        {
                            this.PartnerId = this.shipmentPM.CustomerId;

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.CustomerReference1)) {
                                myReference = this.shipmentPM.CustomerReference1;
                            }

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.CustomerReference2)) {
                                myReference = AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.CustomerReference2 : myReference + "," + this.shipmentPM.CustomerReference2;
                            }

                            break;
                        }

                    case "SHI":
                        {
                            this.PartnerId = this.shipmentPM.ShipperId;

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.ShipperReference1)) {
                                myReference = this.shipmentPM.ShipperReference1;
                            }

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.ShipperReference2)) {
                                myReference = AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.ShipperReference2 : myReference + "," + this.shipmentPM.ShipperReference2;
                            }

                            break;
                        }

                    case "CON":
                        {
                            this.PartnerId = this.shipmentPM.ConsigneeId;

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.ConsigneeReference1)) {
                                myReference = this.shipmentPM.ConsigneeReference1;
                            }

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.ConsigneeReference2)) {
                                myReference = AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.ConsigneeReference2 : myReference + "," + this.shipmentPM.ConsigneeReference2;
                            }

                            break;
                        }

                    case "AGE":
                        {
                            this.PartnerId = this.shipmentPM.AgentId;

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.AgentReference1)) {
                                myReference = this.shipmentPM.AgentReference1;
                            }

                            if (!AppTool.IsNullOrEmpty(this.shipmentPM.AgentReference2)) {
                                myReference = AppTool.IsNullOrEmpty(myReference) ? this.shipmentPM.AgentReference2 : myReference + "," + this.shipmentPM.AgentReference2;
                            }

                            break;
                        }

                    case "CGE": { this.PartnerId = this.shipmentPM.CustomAgentExportId; myReference = this.shipmentPM.CustomAgentExportReference; break; }
                    case "CGI": { this.PartnerId = this.shipmentPM.CustomAgentImportId; myReference = this.shipmentPM.CustomAgentImportReference; break; }
                    case "NOT1": { this.PartnerId = this.shipmentPM.Notify1Id; break; }
                    case "NOT2": { this.PartnerId = this.shipmentPM.Notify2Id; break; }
                    case "SNE": { this.PartnerId = this.shipmentPM.ShipperNotExporterId; myReference = this.shipmentPM.ShipperReference1; break; }
                    case "CNI": { this.PartnerId = this.shipmentPM.ConsigneeNotImporterId; myReference = this.shipmentPM.ConsigneeReference1; break; }
                    case "FFW": { this.PartnerId = this.shipmentPM.FreightForwarderId; myReference = this.shipmentPM.FreightForwarderReference; break; }
                    case "DTR": { this.PartnerId = this.shipmentPM.ConsolidatorId; myReference = this.shipmentPM.ConsolidatorReference; break; }
                    case "OTH": { this.PartnerId = null; break; }
                    case "AL": { this.PartnerId = this.shipmentPM.MainCarriageCarrierId; break; }
                    case "SL": { this.PartnerId = this.shipmentPM.MainCarriageCarrierId; break; }
                    case "TR": { this.PartnerId = this.shipmentPM.MainCarriageCarrierId; break; }
                    default: { break; }
                }
            }

            this.CustomerRef = myReference;
            this.SetUIProperties();
        }
    }

    get BillToPartnerTypeId() { return this.EntityPM.BillToPartnerTypeId; }
    set BillToPartnerTypeId(newValue: string) {
        if (this.EntityPM.BillToPartnerTypeId != newValue) {
            this.EntityPM.BillToPartnerTypeId = newValue;
        }
    }

    get PartnerId() { return this.EntityPM.PartnerId; }
    set PartnerId(newValue: string) {
        if (this.EntityPM.PartnerId != newValue) {
            this.EntityPM.PartnerId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.BillToId = null;
            }
            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list != null) {
                            this.BillToId = list.BillToId;
                            if (AppTool.IsNullOrEmpty(this.BillToId)) {
                                this.BillToId = newValue;
                            }
                        }
                    }
                });
            }
        }
    }

    private billToIsCustomer: boolean = false;
    private billToCorePartnerTypeId: string = null;
    get BillToId() { return this.EntityPM.BillToId; }
    set BillToId(newValue: string) {
        if (this.EntityPM.BillToId != newValue) {
            this.EntityPM.BillToId = newValue;
            this.SetUIProperties_BillTo();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.VatTypeId = null;
                this.VatNumber = null;
                this.BillToName = null;
                this.BillToAddressId = null;
                this.SATPaymentMethodCode = null;
                this.UsoCFDICode = null;
                this.billToIsCustomer = false;
                this.billToCorePartnerTypeId = null;
                this.InvoiceCurrencyId = SessionLocator.TenantPM.CurrencyId;
                this.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;        
                this.IsConstituentInvoice = false;      
                this.SetUIProperties_Constituent(this.IsConstituentInvoice);

                this.EntityPM.BillToIsCreditLimitEnabled = false;
                this.EntityPM.BillToCreditLimitAmount = null;
                this.EntityPM.BillToCreditLimitOpenBalance = null;
                this.EntityPM.BillToCreditLimitWarningPercentage = null;
                this.EntityPM.BillToBlockNewInvoiceCreation = false;

                if (SessionLocator.SATInterfaceSettings) {
                    this.MetodoPagoCode = SessionLocator.SATInterfaceSettings.MetodoPagoCode;
                }
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;

                        if (list != null) {
                            this.VatTypeId = list.VatTypeId;
                            this.VatNumber = list.VatNumber;
                            this.BillToName = list.EnglishName;
                            this.billToIsCustomer = list.IsCustomer;
                            this.billToCorePartnerTypeId = list.PartnerTypeId;

                            if (this.EntityPM.ARInvoiceTypeCode != "CI" && this.EntityPM.ARInvoiceTypeCode != "CC") {
                                this.IsConstituentInvoice = list.EnableConsolidationInvoices;
                            }

                            this.EntityPM.BillToIsCreditLimitEnabled = list.IsCreditLimitEnabled;
                            this.EntityPM.BillToCreditLimitAmount = list.CreditLimitAmount;
                            this.EntityPM.BillToCreditLimitOpenBalance = list.CreditLimitOpenBalance;
                            this.EntityPM.BillToCreditLimitWarningPercentage = list.CreditLimitWarningPercentage;
                            this.EntityPM.BillToBlockNewInvoiceCreation = list.BlockNewInvoiceCreation;                            

                            if (!this.EntitySalesmanUserId) {
                                this.EntityPM.SalesmanUserId = list.SalesmanUserId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
                                this.SATPaymentMethodCode = list.SATPaymentMethodCode;
                            }
                      
                            if (!AppTool.IsNullOrEmpty(list.MetodoPagoCode)) {
                                this.MetodoPagoCode = list.MetodoPagoCode;
                            }
                            else if (SessionLocator.SATInterfaceSettings != null && !AppTool.IsNullOrEmpty(SessionLocator.SATInterfaceSettings.MetodoPagoCode)) {
                                this.MetodoPagoCode = SessionLocator.SATInterfaceSettings.MetodoPagoCode;
                            }

                            //if (!AppTool.IsNullOrEmpty(list.UsoCFDICode)) {
                                this.UsoCFDICode = list.UsoCFDICode; 
                            //}
                           

                            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                this.InvoiceCurrencyId = list.InvoiceCurrencyId;
                            }

                            if (!AppTool.IsNullOrEmpty(list.PaymentTermId)) {
                                this.PaymentTermId = list.PaymentTermId;
                            }
                           
                            if (!AppTool.IsNullOrEmpty(list.BillingAddressId)) {
                                this.BillToAddressId = list.BillingAddressId;
                            }

                            else if (!AppTool.IsNullOrEmpty(list.MainAddressId)) {
                                this.BillToAddressId = list.MainAddressId;
                            }

                            else {
                                this.BillToAddressId = null;
                            }

                            this.SetUIProperties_Constituent(this.IsConstituentInvoice);
                        }
                    }
                });
            }
        }
    }
    
    get BillToName() { return this.EntityPM.BillToName; }
    set BillToName(newValue: string) {
        if (this.EntityPM.BillToName != newValue) {
            this.EntityPM.BillToName = newValue;
        }
    }

    get BillToAddressId() { return this.EntityPM.BillToAddressId; }
    set BillToAddressId(newValue: string) {
        if (this.EntityPM.BillToAddressId != newValue) {
            this.EntityPM.BillToAddressId = newValue;
        }
    }

    // Currency
    get InvoiceCurrencyId() { return this.EntityPM.InvoiceCurrencyId; }
    set InvoiceCurrencyId(newValue: string) {
        if (this.EntityPM.InvoiceCurrencyId != newValue) {
            this.EntityPM.InvoiceCurrencyId = newValue;
            this.SetCurrencyRateData();
            this.SetUIProperties_ExchangeRate();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.InvoiceCurrencyCode = null;
            }

            else {
                this.myCurrencyListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CurrencyList = myResponse.Result;
                        if (list != null) {
                            this.InvoiceCurrencyCode = list.Code;
                        }
                    }
                });
            }
        }
    }    

    get InvoiceCurrencyCode() { return this.EntityPM.InvoiceCurrencyCode; }
    set InvoiceCurrencyCode(value: string) {
        if (this.EntityPM.InvoiceCurrencyCode != value) {
            this.EntityPM.InvoiceCurrencyCode = value;
        }
    }

    get InvoiceCurrencyExchangeRate() { return this.EntityPM.InvoiceCurrencyExchangeRate; }
    set InvoiceCurrencyExchangeRate(value: number) {
        var setValue: number = AppTool.Round(value, 5);
        if (this.EntityPM.InvoiceCurrencyExchangeRate != setValue) {
            this.EntityPM.InvoiceCurrencyExchangeRate = setValue;
        }
    }

    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(value: Date) {
        if (this.EntityPM.ExchangeRateDate != value) {
            this.EntityPM.ExchangeRateDate = value;
            this.ComputeRelativeRateDate();
        }
    }

    private myRelativeRateDate: string = null;
    get RelativeRateDate() { return this.myRelativeRateDate; }
    set RelativeRateDate(value: string) {
        if (this.myRelativeRateDate != value) {
            this.myRelativeRateDate = value;
            this.SetUIProperties_BillTo();
        }
    }
    ComputeRelativeRateDate() {
        this.RelativeRateDate = DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old");
    }

    // Properties
    private vatTypeId: string;
    get VatTypeId() { return this.vatTypeId; }
    set VatTypeId(newValue: string) {
        if (this.vatTypeId != newValue) {
            this.vatTypeId = newValue;
        }
    }

    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.EntityPM.VatNumber != newValue) {
            this.EntityPM.VatNumber = newValue;
            this.SetUIProperties_VatNumber();
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(newValue: string) {
        if (this.EntityPM.PaymentTermId != newValue) {
            this.EntityPM.PaymentTermId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.PaymentTermName = null;                
            }

            else {
                this.myPaymentTermListService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PaymentTermList = myResponse.Result;
                        if (list != null) {
                            this.PaymentTermName = list.EnglishName;
                        }
                    }
                });
            }

            InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
        }
    }

    get PaymentTermName() { return this.EntityPM.PaymentTermName; }
    set PaymentTermName(newValue: string) {
        if (this.EntityPM.PaymentTermName != newValue) {
            this.EntityPM.PaymentTermName = newValue;
        }
    }

    get InvoiceDate() { return this.EntityPM.InvoiceDate; }
    set InvoiceDate(newValue: Date) {
        if (this.EntityPM.InvoiceDate != newValue) {
            this.EntityPM.InvoiceDate = newValue;

            InvoiceTool.ComputeARInvoiceDueDate(this.EntityPM);
            this.ComputeRelativeRateDate();
            this.LoadData();
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(newValue: Date) {
        if (this.EntityPM.DueDate != newValue) {
            this.EntityPM.DueDate = newValue;
            InvoiceTool.ComputeARInvoicePaymentTerm(this.EntityPM);
        }
    }

    get CustomerRef() { return this.EntityPM.CustomerRef; }
    set CustomerRef(newValue: string) {
        if (this.EntityPM.CustomerRef != newValue) {
            this.EntityPM.CustomerRef = newValue;
        }
    }

    get IsConstituentInvoice() { return this.EntityPM.IsConstituentInvoice; }
    set IsConstituentInvoice(newValue: boolean) {
        if (this.EntityPM.IsConstituentInvoice != newValue) {
            this.EntityPM.IsConstituentInvoice = newValue;
        }
    }

    get Intercompany() { return this.EntityPM.Intercompany; }
    set Intercompany(newValue: boolean) {
        if (this.EntityPM.Intercompany != newValue) {
            this.EntityPM.Intercompany = newValue;
        }
    }

    get SATPaymentMethodCode() { return this.EntityPM.SATPaymentMethodCode; }
    set SATPaymentMethodCode(newValue: string) {
        if (this.EntityPM.SATPaymentMethodCode != newValue) {
            this.EntityPM.SATPaymentMethodCode = newValue;
            this.SetUIProperties_Payment();
        }
    }

    get MetodoPagoCode() { return this.EntityPM.MetodoPagoCode; }
    set MetodoPagoCode(newValue: string) {
        if (this.EntityPM) {
            if (this.EntityPM.MetodoPagoCode != newValue) {
                this.EntityPM.MetodoPagoCode = newValue;
                this.SetUIProperties_Payment();
            }
        }
    }
    get UsoCFDICode() { return this.EntityPM.UsoCFDICode; }
    set UsoCFDICode(newValue: string) {
        if (this.EntityPM) {
            if (this.EntityPM.UsoCFDICode != newValue) {
                this.EntityPM.UsoCFDICode = newValue;
                //this.SetUIProperties_Payment();
            }
        }
    }

    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;
        }
    }

    // Load Date 
    private LastRatesList: LastRate[] = [];
    private VatTypePercentagesList: VatTypePercentagePM[] = [];
    private myCurrencyRatesService: CurrencyRatesService;
    LoadData() {

        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService();
        }

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.LocalCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;
                this.SetCurrencyRateData();

                this.myCommonDomainService.GetVatTypePercentagePMByDate(loadingDate).subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.VatTypePercentagesList = myResponse2.Result;
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    SetCurrencyRateData() {
        var myRate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            if (this.InvoiceCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.InvoiceCurrencyId)[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this.InvoiceCurrencyExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
    }
    GetCurrencyRate(currencyId: string) {
        var myResult: number = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }

        return myResult;
    }
    GetCurrencyRateDate(currencyId: string) {
        var myResult: Date = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                myResult = null;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                if (lastRate != null) {
                    myResult = lastRate.ValueDate;
                }
            }
        }

        return myResult;
    }
    GetVatTypePercentage(vatTypeId: string) {
        var myResult: number = null;

        var vatTypePercentagePM = this.VatTypePercentagesList.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }

        return myResult;
    }
    UpdateCurrencyRateClicked() {

        var loadingDate = this.EntityPM.InvoiceDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.InvoiceCurrencyId, CurrencyCode: this.InvoiceCurrencyCode, Rate: this.InvoiceCurrencyExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LastRatesList = comp.RatesList;
                    this.InvoiceCurrencyExchangeRate = comp.Rate;
                    this.ExchangeRateDate = comp.RateDate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    }

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }    
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.BillToPartnerTypeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.PartnerType")));
        }

        if (AppTool.IsNullOrEmpty(this.BillToId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.BillToId")));
        }

        //if (AppTool.IsNullOrEmpty(this.BillToAddressId)) {
        //    errors.push(msg.replace("%FieldName", "Address"));
        //}

        if (AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceCurrencyId")));
        }

        var date1 = new Date(this.InvoiceDate.toString());
        var date2 = new Date();
        date2.setHours(23);
        date2.setMinutes(59);

        
        
        if (this.InvoiceDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate")));
        }

        else if (date1.valueOf() > date2.valueOf()) {
            errors.push(TextCodeTranslator.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate"));
        }

        if (this.DueDate == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.DueDate")));
        }

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (AppTool.IsNullOrEmpty(this.VatNumber)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.VatNumber")));
            }
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40") {
            if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.SATPaymentMethodCode")));
            }
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40") {
            //if (AppTool.IsNullOrEmpty(this.SATPaymentMethodCode)) {
            //  errors.push(msg.replace("%FieldName", "Forma Pago"));
            //}

            if (AppTool.IsNullOrEmpty(this.MetodoPagoCode)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.MetodoPagoCode")));
            }

            if (this.MetodoPagoCode == "PUE" && this.SATPaymentMethodCode == "99") {
                errors.push("Since the metodo pago was set as PUE, you can't select Por definir (99). Please choose another value for the forma Pago.");
            }
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.BranchId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.BranchId")));
        }

        if (errors.length == 0) {
            this.ValidateCreditLimitPartnersRestrictions(errors);
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.UpdateCreditLimitFlags();

            if (SessionLocator.TenantPM.AccountingActivated) {
                this.ValidateFullAccounting();
            }

            else if (this.HasCreditLimitFeature && this.IsCreditLimitActivated && this.IsCreditLimitHasAction && this.EntityPM.BillToIsCreditLimitEnabled) {
                this.ValidateCreditLimit();
            }

            else {
                this.OnEntityValid();
            }
        }
    }

    ValidateCreditLimitPartnersRestrictions(errors: string[]) {
        var errorText_Blocking: string = "Credit limit setting is blocking invoice for ";

        if (ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled) {
            switch (this.billToCorePartnerTypeId) {
                case "CS":
                    {
                        if (this.billToIsCustomer) {
                            if (ObjectsLocator.CreditLimitSettingPM.CustomersInvoicesBlock) {
                                errors.push(errorText_Blocking + "Customers");
                            }
                        }

                        else {
                            if (ObjectsLocator.CreditLimitSettingPM.ShipperConsigneeInvoiceBlock) {
                                errors.push(errorText_Blocking + "Shippers and Consignees");
                            }
                        }

                        break;
                    }

                case "AG":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.AgentsInvoicesBlock) {
                            errors.push(errorText_Blocking + "Agents");
                        }

                        break;
                    }

                case "CG":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.CustomsAgentsInvoicesBlock) {
                            errors.push(errorText_Blocking + "Customs Agents");
                        }

                        break;
                    }

                case "SG":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.ShippingAgentsInvoicesBlock) {
                            errors.push(errorText_Blocking + "Shipping Agents");
                        }

                        break;
                    }

                case "AL":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.AirlinesInvoicesBlock) {
                            errors.push(errorText_Blocking + "Airlines");
                        }

                        break;
                    }

                case "SL":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.ShippingLinesInvoicesBlock) {
                            errors.push(errorText_Blocking + "Shipping Lines");
                        }

                        break;
                    }

                case "TR":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.TruckersInvoicesBlock) {
                            errors.push(errorText_Blocking + "Truckers");
                        }

                        break;
                    }

                case "VD":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.VendorsInvoicesBlock) {
                            errors.push(errorText_Blocking + "Vendors");
                        }

                        break;
                    }

                case "WH":
                    {
                        if (ObjectsLocator.CreditLimitSettingPM.WarehousesInvoicesBlock) {
                            errors.push(errorText_Blocking + "Warehouses");
                        }

                        break;
                    }
            }
        }
    }

    ValidateFullAccounting() {
        this.CurrentSession.StartBusyIndicator("Checking ...");

        this.myInvoiceDomainService.ValidateARInvoiceFullAccounting(this.EntityPM.InvoiceCurrencyId, this.EntityPM.BillToId, this.EntityPM.InvoiceDate).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {

                if (!myResponse.HasError) {
                    if (this.HasCreditLimitFeature && this.IsCreditLimitActivated && this.IsCreditLimitHasAction) {
                        this.ValidateCreditLimit();
                    }

                    else {
                        this.OnEntityValid();
                    }
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            }
        });
    }
    ValidateCreditLimit() {
        if (this.EntityPM.BillToBlockNewInvoiceCreation) {

            var errorText_Blocking = "Credit limit setting is blocking invoice for bill to: " + this.EntityPM.BillToName;

            var errors: string[] = [];
            var warnings: string[] = [];

            if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                errors.push(errorText_Blocking);
            }

            else if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                warnings.push(errorText_Blocking);
            }

            if (errors.length > 0 || warnings.length > 0) {

                var logWindow = new LogitudeWindow();
                logWindow.Width = 450;
                logWindow.Height = 200;
                logWindow.Title = "Credit limit";
                logWindow.WindowArgs = { Errors: errors, Warnings: warnings };

                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.OnEntityValid();
                    }
                });

                logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
            }

            else {
                this.OnEntityValid();
            }
        }

        else {
            this.CurrentSession.StartBusyIndicatorLoading();

            this.myInvoiceDomainService.GetCustomerCreditLimitActualAmount(this.BillToId).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    var errors: string[] = [];
                    var warnings: string[] = [];

                    this.EntityPM.BillToCreditLimitActualAmount = myResponse.Result;
                    this.EntityPM.BillToCreditLimitActualBalance = AppTool.AddAmounts(this.EntityPM.BillToCreditLimitOpenBalance, this.EntityPM.BillToCreditLimitActualAmount);

                    var LimitAmount = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitAmount) ? 0 : this.EntityPM.BillToCreditLimitAmount;
                    var WarningPercentage = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitWarningPercentage) ? 0 : this.EntityPM.BillToCreditLimitWarningPercentage;
                    var ActualBalance = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitActualBalance) ? 0 : this.EntityPM.BillToCreditLimitActualBalance;

                    var isBillToHasLimitAmount = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitAmount) ? false : true;
                    var isBillToHasWarningPercentage = AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitWarningPercentage) ? false : true;

                    if (isBillToHasLimitAmount && ActualBalance > LimitAmount) {
                        var LimitError = "";
                        var LimitWarning = "The customer exceeded the credit limit available.";
                        LimitError += "Bill To exceeded its credit limit of " + FormatTool.FormatNumber(LimitAmount) + " (" + this.EntityPM.LocalCurrencyCode + ")."
                        LimitError += " ";
                        LimitError += "The current balance stands on " + FormatTool.FormatNumber(ActualBalance) + " (" + this.EntityPM.LocalCurrencyCode + ").";

                        if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                            errors.push(LimitError);
                        }

                        else if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                            warnings.push(LimitWarning);
                        }
                    }

                    else if (isBillToHasWarningPercentage && ActualBalance > (WarningPercentage * LimitAmount / 100)) {
                        if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {

                            var RemainingLimit = FormatTool.FormatNumber(LimitAmount - ActualBalance);
                            var PercentageWarning: string = "The remaining credit limit for this customer is " + RemainingLimit;
                            warnings.push(PercentageWarning);
                        }
                    }

                    if (errors.length > 0 || warnings.length > 0) {

                        var isBlockingShipment = false;
                        if (errors.length > 0) {
                            if (!this.shipmentPM.IsNewARInvoiceBlocked) {
                                this.shipmentPM.IsNewARInvoiceBlocked = true;
                                this.shipmentPM.IsDirty = false;
                                isBlockingShipment = true;
                            }
                        }

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 200;
                        logWindow.Title = "Credit limit";
                        logWindow.WindowArgs = { Errors: errors, Warnings: warnings, IsBlockingShipment: isBlockingShipment, ShipmentId: this.shipmentPM.Id };

                        logWindow.WindowClosed.subscribe(s => {
                            if (s) {
                                this.OnEntityValid();
                            }
                        });

                        logWindow.Show('./Invoice/Components/NewEntity/CreditLimitPopupComponent');
                    }

                    else {
                        this.OnEntityValid();
                    }
                }
            });
        }
    }
    OnEntityValid() {
        if (!FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent")) {
            this.IsConstituentInvoice = false;
        }

        this.CurrentSession.StartBusyIndicatorLoading();

        this.InitializeComponent();   
    }

    InitializeComponent() {

        this.myCurrencyListService.getSingleFromCache(this.EntityPM.ProfitCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: CurrencyList = myResponse.Result;
                if (list != null) {
                    this.EntityPM.ProfitCurrencyCode = list.Code;
                }
            }
        });

        this.EntityPM.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);

        if (this.IsConstituentInvoice) {
            this.EntityPM.StatusCode = "NT";
            this.EntityPM.StatusName = "Not Connected";
        }

        else {
            this.EntityPM.StatusCode = "DR";
            this.EntityPM.StatusName = "Draft";
        }

        this.BuildOpenAmounts();
    }
    BuildOpenAmounts() {
        var filteredReceivables: ShipmentReceivablePM[] = this.EntityReceivables.filter(d => d.ShipmentReceivableParentId == null);

        if (this.EntityPM.ARInvoiceTypeCode != "MN") {
            filteredReceivables = filteredReceivables.filter(f => f.ShipmentReceivableLineStatusCode == "OAMT"
                && ((f.MeasurementCode == "STFE" && f.ChargesTypeCode == "ISTOR") ||(  f.Quantity != null && f.UnitPrice != null))
                && f.ARInvoiceId == null && f.ARInvoiceLineId == null);

            switch (this.EntityPM.ARInvoiceTypeCode) {
                case "IN":
                case "CI":
                    {
                        if (!SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                            filteredReceivables = filteredReceivables.filter(f => f.UnitPrice > 0 || (f.ChargesTypeCode == "ISTOR" && f.MeasurementCode == "STFE" && f.TotalAmount > 0));
                        }

                        break;
                    }

                case "CD":
                case "CC":
                    {
                    if (!SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                        filteredReceivables = filteredReceivables.filter(f => f.UnitPrice < 0 || (f.MeasurementCode == "STFE" && f.ChargesTypeCode == "ISTOR" && f.TotalAmount < 0));
                    }

                    break;
                }
            }

            if (this.EntityPM.PrepaidCollectId != null && this.EntityPM.PrepaidCollectId != "B") {
                filteredReceivables = filteredReceivables.filter(f => f.PrepaidCollectId == this.EntityPM.PrepaidCollectId);
            }
        }

        this.BuildInvoiceLines(filteredReceivables);
    }
    BuildInvoiceLines(filteredReceivables: ShipmentReceivablePM[]) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            filteredReceivables.forEach(receivable => {
                var invoiceLine: ARInvoiceLinePM = new ARInvoiceLinePM(this.EntityPM);
                invoiceLine.Tenant = receivable.Tenant;
                invoiceLine.ChargesTypeId = receivable.ChargesTypeId;
                invoiceLine.ForiegnCurrencyId = receivable.CurrencyId;
                invoiceLine.ForiegnCurrencyCode = receivable.CurrencyCode;
                invoiceLine.ReceivableId = receivable.Id;
                invoiceLine.MeasurementId = receivable.MeasurementId;
                invoiceLine.MeasurementCode = receivable.MeasurementCode;
                invoiceLine.PrepaidCollectId = receivable.PrepaidCollectId;
                invoiceLine.IsExchangeRateFixed = receivable.IsExchangeRateFixed;
                invoiceLine.EntityId = receivable.ShipmentId;
                invoiceLine.EntityReference = receivable.ShipmentNumber;
                invoiceLine.Quantity = AppTool.Round(receivable.Quantity, 3);
                invoiceLine.UnitPrice = AppTool.Round(receivable.UnitPrice, 3);
                invoiceLine.ForiegnCurrencyAmount = AppTool.Round(receivable.TotalAmount, 2);
                //invoiceLine.IsBackToBack = receivable.IsBackToBack;
                invoiceLine.IsExpense = receivable.IsExpense;
                invoiceLine.Notes = receivable.Notes;

                var itemExchangeRate: number = null;

                if (invoiceLine.IsExchangeRateFixed) {
                    itemExchangeRate = receivable.Rate;
                }

                else if (invoiceLine.ForiegnCurrencyId == this.InvoiceCurrencyId) {
                    itemExchangeRate = this.InvoiceCurrencyExchangeRate;
                }

                else {
                    itemExchangeRate = this.GetCurrencyRate(invoiceLine.ForiegnCurrencyId);
                }

                var itemExchangeRateRounded: number = AppTool.Round(itemExchangeRate, 5);
                invoiceLine.ForiegnExchangeRate = itemExchangeRateRounded;
                invoiceLine.ExchangeRateDate = this.GetCurrencyRateDate(invoiceLine.ForiegnCurrencyId);

                // Local Amount
                if (SessionLocator.LocalCurrencyId == invoiceLine.ForiegnCurrencyId) {
                    invoiceLine.LocalCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                }
                else {
                    invoiceLine.LocalCurrencyAmount = AppTool.Round((invoiceLine.ForiegnCurrencyAmount * invoiceLine.ForiegnExchangeRate), 2);
                }

                // Profit Amount
                if (this.EntityPM.ProfitCurrencyId == invoiceLine.ForiegnCurrencyId) {
                    invoiceLine.ProfitCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                }
                else if (this.EntityPM.ProfitCurrencyId == SessionLocator.LocalCurrencyId) {
                    invoiceLine.ProfitCurrencyAmount = invoiceLine.LocalCurrencyAmount;
                }
                else {
                    invoiceLine.ProfitCurrencyAmount = AppTool.Round((invoiceLine.LocalCurrencyAmount / this.EntityPM.ProfitCurrencyExchangeRate), 2);
                }

                // Invoice Amount
                if (this.EntityPM.InvoiceCurrencyId == invoiceLine.ForiegnCurrencyId) {
                    invoiceLine.InvoiceCurrencyAmount = invoiceLine.ForiegnCurrencyAmount;
                }
                else if (this.EntityPM.InvoiceCurrencyId == SessionLocator.LocalCurrencyId) {
                    invoiceLine.InvoiceCurrencyAmount = invoiceLine.LocalCurrencyAmount;
                }
                else if (this.EntityPM.InvoiceCurrencyId == this.EntityPM.ProfitCurrencyId) {
                    invoiceLine.InvoiceCurrencyAmount = invoiceLine.ProfitCurrencyAmount;
                }
                else {
                    invoiceLine.InvoiceCurrencyAmount = AppTool.Round((invoiceLine.LocalCurrencyAmount / this.EntityPM.InvoiceCurrencyExchangeRate), 2)
                }
               
                this.myChargesTypeListService.getSingleFromCache(invoiceLine.ChargesTypeId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {

                        var list: ChargesTypeList = myResponse.Result;

                        this.SetInvoiceLineVatType(list, receivable, invoiceLine);

                        if (list != null) {
                            invoiceLine.Description = list.EnglishName;
                            invoiceLine.LocalDescription = list.LocalName;
                            invoiceLine.IsCustomsCharge = list.IsCustoms;

                            if (this.RegionalTaxId) {
                                if (invoiceLine.VatIsMultiPercentage == false) {
                                    invoiceLine.IsRegionalTax = list.ApplyRegionalTax;
                                }
                            }
                        }
                    }
                });

                this.EntityPM.AddARInvoiceLinePM(invoiceLine);
            });
        }

        this.BuildTotalVATs();
        this.ComputeTotals();
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }

    SetInvoiceLineVatType(list: ChargesTypeList, myReceivable: ShipmentReceivablePM, invoiceLine: ARInvoiceLinePM) {

        if (!AppTool.IsNullOrEmpty(this.VatTypeId)) {
            invoiceLine.VatTypeId = this.VatTypeId;
        }

        else if (myReceivable.IsFromQuote && !AppTool.IsNullOrEmpty(myReceivable.VatTypeId)) {
            invoiceLine.VatTypeId = myReceivable.VatTypeId;
        }

        else if (list) {
            invoiceLine.VatTypeId = list.VatTypeId;
        }
 
        if (!AppTool.IsNullOrEmpty(invoiceLine.VatTypeId)) {
            var list_VAT: VatTypeList = this.AllVatTypes.filter(f => f.Id == invoiceLine.VatTypeId)[0];
            if (list_VAT) {
                invoiceLine.VatTypeName = list_VAT.EnglishName;
                invoiceLine.VatIsMultiPercentage = list_VAT.IsMultiPercentage;

                if (!list_VAT.IsMultiPercentage) {
                    invoiceLine.VatPercentage = this.GetVatTypePercentage(invoiceLine.VatTypeId);
                }
            }
        }
    }

    BuildTotalVATs() {
        this.EntityPM.TotalVATs = [];

        var myDataLines: ARInvoiceLinePM[] = this.EntityPM.InvoiceLines.filter(f => f.VatTypeId != null);
        if (myDataLines.length > 0) {

            // Build Totals Class
            var group_Source: InvoiceTotalsClass[] = [];
            myDataLines.forEach(item => {
                var lineVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                if (lineVatType) {

                    if (AppTool.IsNullOrEmpty(item.LocalCurrencyAmount)) {
                        item.LocalCurrencyAmount = 0;
                    }

                    if (AppTool.IsNullOrEmpty(item.InvoiceCurrencyAmount)) {
                        item.InvoiceCurrencyAmount = 0;
                    }

                    if (AppTool.IsNullOrEmpty(item.ProfitCurrencyAmount)) {
                        item.ProfitCurrencyAmount = 0;
                    }

                    if (!lineVatType.IsMultiPercentage) {
                        var myQroupItem = new InvoiceTotalsClass();
                        myQroupItem.Id = lineVatType.Id;
                        myQroupItem.VatTypeId = lineVatType.Id;
                        myQroupItem.VatTypePercentage = item.VatPercentage;
                        myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                        myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                        myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                        myQroupItem.ExternalVatCard = SessionLocator.AccountingSettingPM.ReceivableVATCard;
                        myQroupItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;

                        if (item.IsRegionalTax) {

                            myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount + item.LocalCurrencyAmount * (this.RegionalTaxPercentage / 100);
                            myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount + item.InvoiceCurrencyAmount * (this.RegionalTaxPercentage / 100);
                            myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount + item.ProfitCurrencyAmount * (this.RegionalTaxPercentage / 100);

                            var regionalTaxItem = new InvoiceTotalsClass();
                            regionalTaxItem.Id = this.RegionalTaxId;
                            regionalTaxItem.VatTypeId = this.RegionalTaxId;
                            regionalTaxItem.VatTypePercentage = this.RegionalTaxPercentage;
                            regionalTaxItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                            regionalTaxItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                            regionalTaxItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                            regionalTaxItem.ExternalVatCard = SessionLocator.AccountingSettingPM.ReceivableVATCard;
                            regionalTaxItem.ExternalTAXItemId = lineVatType.ExternalTAXItemId;
                            group_Source.push(regionalTaxItem);
                        }

                        group_Source.push(myQroupItem);
                    }

                    else {
                        var myVatGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == item.VatTypeId);

                        myVatGroups.forEach(itemGroup => {
                            var myQroupItem = new InvoiceTotalsClass();
                            myQroupItem.Id = itemGroup.SingleVATTypeId;
                            myQroupItem.VatTypeId = itemGroup.SingleVATTypeId;
                            myQroupItem.LocalCurrencyAmount = item.LocalCurrencyAmount;
                            myQroupItem.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                            myQroupItem.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                            myQroupItem.ExternalVatCard = SessionLocator.AccountingSettingPM.ReceivableVATCard;

                            var vatType = this.AllVatTypes.filter(f => f.Id == itemGroup.SingleVATTypeId)[0];
                            if (vatType) {
                                myQroupItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                myQroupItem.VatTypePercentage = this.GetVatTypePercentage(vatType.Id);
                            }

                            group_Source.push(myQroupItem);
                        });
                    }
                }
            });

            // Group Totals Class
            var group_data: InvoiceTotalsClass[] = [];
            group_Source.forEach(item => {
                var record: InvoiceTotalsClass = group_data.filter(f => f.VatTypeId == item.VatTypeId && f.VatTypePercentage == item.VatTypePercentage && f.ExternalVatCard == item.ExternalVatCard && f.ExternalTAXItemId == item.ExternalTAXItemId)[0];
                if (record) {
                    record.LocalCurrencyAmount += item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount += item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount += item.ProfitCurrencyAmount;
                }

                else {
                    record = new InvoiceTotalsClass();
                    record.Id = item.Id;
                    record.VatTypeId = item.VatTypeId;
                    record.VatTypePercentage = item.VatTypePercentage;
                    record.ExternalVatCard = item.ExternalVatCard;
                    record.ExternalTAXItemId = item.ExternalTAXItemId;
                    record.LocalCurrencyAmount = item.LocalCurrencyAmount;
                    record.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                    record.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                    group_data.push(record);
                }
            });

            // Build Invoice Total VATs
            group_data.forEach(item => {

                var itemVatType = this.AllVatTypes.filter(f => f.Id == item.VatTypeId)[0];

                var itemTotalVAT = new ARInvoiceTotalVATPM(null);                
                itemTotalVAT.Tenant = SessionLocator.Tenant;
                itemTotalVAT.ARInvoiceId = this.EntityPM.Id;
                itemTotalVAT.VatTypeId = item.VatTypeId;
                itemTotalVAT.VatTypeName = itemVatType.EnglishName;
                itemTotalVAT.ExternalVATCard = item.ExternalVatCard;
                itemTotalVAT.ExternalTAXItemId = item.ExternalTAXItemId;
                itemTotalVAT.VATPercent = AppTool.Round(item.VatTypePercentage, 2);
                itemTotalVAT.LocalVatableAmount = AppTool.Round(item.LocalCurrencyAmount, 2);
                itemTotalVAT.InvoiceCurrencyVatableAmount = AppTool.Round(item.InvoiceCurrencyAmount, 2);
                itemTotalVAT.ProfitVatableAmount = AppTool.Round(item.ProfitCurrencyAmount, 2);
                itemTotalVAT.LocalVATAmount = AppTool.Round((itemTotalVAT.LocalVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.InvoiceCurrencyVATAmount = AppTool.Round((itemTotalVAT.InvoiceCurrencyVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.ProfitCurrencyVATAmount = AppTool.Round((itemTotalVAT.ProfitVatableAmount * itemTotalVAT.VATPercent / 100), 2);
                itemTotalVAT.VatTypeCell = itemTotalVAT.VatTypeName + " (" + itemTotalVAT.VATPercent + "%)";
                this.EntityPM.AddARInvoiceTotalVATPM(itemTotalVAT);
            });
        }
    }
    ComputeTotals() {      
        this.EntityPM.SubTotalInLocalCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "LocalCurrencyAmount"),2);
        this.EntityPM.SubTotalInInvoiceCurrency = AppTool.Round(ArrayTool.Sum(this.EntityPM.InvoiceLines, "InvoiceCurrencyAmount"), 2);
        this.EntityPM.AmountInLocalCurrency = AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalVATAmount"), 2);
        this.EntityPM.AmountInInvoiceCurrency = AppTool.Round(this.EntityPM.SubTotalInLocalCurrency + ArrayTool.Sum(this.EntityPM.TotalVATs, "InvoiceCurrencyVATAmount"), 2);

        if (this.EntityPM.ProfitCurrencyId == SessionLocator.LocalCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInLocalCurrency;
        }

        else if (this.EntityPM.ProfitCurrencyId == this.EntityPM.InvoiceCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.AmountInInvoiceCurrency;
        }

        else {
            if (this.EntityPM.ProfitCurrencyExchangeRate != 0) {
                this.EntityPM.AmountInProfitCurrency = AppTool.Round(this.EntityPM.AmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate,2);
            }
        }
    }

    // CreditLimit
    public HasCreditLimitFeature: boolean = false;
    public HasCreditOverrideFeature: boolean = false;
    public IsCreditLimitActivated: boolean = false;
    public IsCreditLimitHasAction: boolean = false;
    UpdateCreditLimitFlags() {
        this.HasCreditLimitFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        this.HasCreditOverrideFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
        this.EntityPM.HasCreditLimitOverrideFeature = this.HasCreditOverrideFeature;

        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            this.IsCreditLimitHasAction = (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock == true || ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning == true) ? true : false;
        }
    }


    // RegionalTaxId
    public IsRegionalTaxVisible: boolean = false;
    private SetRegionalTaxVisibility() {
        var isVisible: boolean = false;

        if (FeatureLocator.HasFeaturePermession("General", "REGIONALTAX")) {
            if (SessionLocator.AccountingSettingPM.AllowRegionalTaxManagement) {
                isVisible = true;
            }
        }

        this.IsRegionalTaxVisible = isVisible;
    }

    get RegionalTaxId() { return this.EntityPM.RegionalTaxId; }
    set RegionalTaxId(newValue: string) {
        if (this.EntityPM.RegionalTaxId != newValue) {
            this.EntityPM.RegionalTaxId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.RegionalTaxPercentage = null;
            }

            else {
                this.RegionalTaxPercentage = this.GetVatTypePercentage(newValue);
            }
        }
    }

    get RegionalTaxPercentage() {

        var output: number = 0;

        if (this.EntityPM.RegionalTaxPercentage) {
            output = this.EntityPM.RegionalTaxPercentage;
        }

        return output;
    }
    set RegionalTaxPercentage(newValue: number) {
        if (this.EntityPM.RegionalTaxPercentage != newValue) {
            this.EntityPM.RegionalTaxPercentage = AppTool.Round(newValue, 2);
            this.ComputeTotals();
        }
    }
}
