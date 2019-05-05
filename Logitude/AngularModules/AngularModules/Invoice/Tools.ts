import {AppTool, DateTool, FormatTool} from '../Infrastructure/Tools';
import {ShipmentPM} from '../Shipment/EntityPMs/ShipmentPM';
import {ARInvoicePM} from './EntityPMs/ARInvoicePM';
import {APInvoicePM} from './EntityPMs/APInvoicePM';
import {ARPaymentPM} from './EntityPMs/ARPaymentPM';
import {APPaymentPM} from './EntityPMs/APPaymentPM';
import {PaymentTermList} from '../Common/EntityLists/PaymentTermList';
import {PaymentTermListService} from '../Common/Services/StandardLists/PaymentTermListService';
import {ServiceResponse} from '../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../Infrastructure/Utilities/FeatureLocator';
import {ObjectsLocator} from '../Infrastructure/Locators/ObjectsLocator';

export class InvoiceTool {
    public static IsEditingARInvoiceEnabled(entityPM: ARInvoicePM) {
        var myResult = false;

        if (entityPM != null) {
            if (AppTool.IsNullOrEmpty(entityPM.Id)) {
                myResult = true;
            }

            else if (AppTool.IsNullOrEmpty(entityPM.StatusCode)) {
                myResult = true;
            }

            else if (entityPM.StatusCode == "DR") {
                myResult = true;
            }

            else if (entityPM.IsConstituentInvoice) {
                if (AppTool.IsNullOrEmpty(entityPM.ConsolidationInvoiceId) && entityPM.StatusCode == "NT") {
                    myResult = true;
                }
            }
        }

        return myResult;
    }
    public static IsEditingAPInvoiceEnabled(entityPM: APInvoicePM) {
        var myResult = false;

        if (entityPM != null) {
            if (AppTool.IsNullOrEmpty(entityPM.Id)) {
                myResult = true;
            }

            else if (AppTool.IsNullOrEmpty(entityPM.StatusCode)) {
                myResult = true;
            }

            else if (entityPM.StatusCode == "WA") {
                myResult = true;
            }
        }

        return myResult;
    }
    public static IsEditingARPaymentEnabled(entityPM: ARPaymentPM) {
        var myResult = false;

        if (entityPM != null) {
            if (AppTool.IsNullOrEmpty(entityPM.StatusCode) || entityPM.StatusCode == "DR") {
                myResult = true;
            }            
        }

        return myResult;
    }
    public static IsEditingAPPaymentEnabled(entityPM: APPaymentPM) {
        var myResult = false;

        if (entityPM != null) {
            if (AppTool.IsNullOrEmpty(entityPM.StatusCode) || entityPM.StatusCode == "DR") {
                myResult = true;
            }
        }

        return myResult;
    }
    public static GetARInvoicePartners(shipmentPM: ShipmentPM) {
        var invoicePartners: InvoicePartnerType[] = [];

        if (shipmentPM != null) {
            if (!AppTool.IsNullOrEmpty(shipmentPM.CustomerId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "CUS", "Customer"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.ShipperId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "SHI", "Shipper"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.ConsigneeId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "CON", "Consignee"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.AgentId)) {
                invoicePartners.push(new InvoicePartnerType("AG", "AGE", "Agent"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.CustomAgentExportId)) {
                invoicePartners.push(new InvoicePartnerType("CG", "CGE", "Custom Agent Export"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.CustomAgentImportId)) {
                invoicePartners.push(new InvoicePartnerType("CG", "CGI", "Custom Agent Import"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.Notify1Id)) {
                invoicePartners.push(new InvoicePartnerType("CS", "NOT1", "Notify1"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.Notify2Id)) {
                invoicePartners.push(new InvoicePartnerType("CS", "NOT2", "Notify2"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.ShipperNotExporterId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "SNE", "Shipper Not Exporter"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.ConsigneeNotImporterId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "CNI", "Consignee Not Importer"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.FreightForwarderId)) {
                invoicePartners.push(new InvoicePartnerType("AG", "FFW", "Freight Forwarder"));
            }

            if (!AppTool.IsNullOrEmpty(shipmentPM.ConsolidatorId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "DTR", "Consolidator"));
            }

            invoicePartners.push(new InvoicePartnerType("CS,CG,AG", "OTH", "Other Partners"));

            if (shipmentPM.TransportModeId == "A") {
                invoicePartners.push(new InvoicePartnerType("AL", "AL", "Airline"));
            }

            if (shipmentPM.TransportModeId == "O") {
                invoicePartners.push(new InvoicePartnerType("SL", "SL", "Shipping Line"));
            }

            if (shipmentPM.TransportModeId == "I") {
                invoicePartners.push(new InvoicePartnerType("TR", "TR", "Trucker"));
            }
        }

        else {
            invoicePartners.push(new InvoicePartnerType("CS", "CS", "Customer", null, true));
            invoicePartners.push(new InvoicePartnerType("CS", "CS", "Shipper & Consignee"));
            invoicePartners.push(new InvoicePartnerType("AG", "AG", "Agent"));
            invoicePartners.push(new InvoicePartnerType("CG", "CG", "Custom agent"));
            invoicePartners.push(new InvoicePartnerType("SG", "SG", "Shipping agent"));
            invoicePartners.push(new InvoicePartnerType("AL", "AL", "Airline"));
            invoicePartners.push(new InvoicePartnerType("SL", "SL", "Shipping line"));
            invoicePartners.push(new InvoicePartnerType("TR", "TR", "Trucker"));
            invoicePartners.push(new InvoicePartnerType("VD", "VD", "Vendor"));
        }

        return invoicePartners;
    }
    public static GetBillToPartnerTypes() {
        return "CS,AG,AL,CG,SG,SL,TR,VD,WH";
    }
    public static GetVendorPartnerTypes() {
        return "AG,AL,CG,SG,SL,TR,VD,WH";
    }
    public static GetOperationalDate(shipmentPM: ShipmentPM) {
        var myResult: Date = null;

        if (shipmentPM != null) {
            if (shipmentPM.DirectionId == "I") {
                myResult = shipmentPM.MainCarriageFinalDestinationATA;

                if (myResult == null) {
                    myResult = shipmentPM.MainCarriageFinalDestinationETA;
                }
            }

            else {
                myResult = shipmentPM.MainCarriageATD;

                if (myResult == null) {
                    myResult = shipmentPM.MainCarriageETD;
                }
            }

            if (myResult == null) {
                myResult = shipmentPM.CreateDateTime;
            }
        }

        return myResult;
    }
    public static ComputeARInvoiceDueDate(entityPM: ARInvoicePM) {
        if (entityPM != null) {
            if (AppTool.IsNullOrEmpty(entityPM.PaymentTermId)) {
                entityPM.DueDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
            }

            else {
                var myService = new PaymentTermListService();
                myService.getSingleFromCache(entityPM.PaymentTermId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PaymentTermList = myResponse.Result;
                        if (list != null) {
                            if (list.IsManuallySet) {
                                entityPM.DueDate = null;
                            }

                            else if (AppTool.IsNullOrZero(list.Days)) {

                                var myComparativeDate: Date = null;

                                if (entityPM.IsConsolidationInvoice) {
                                    myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }

                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.OperationalDate).DateObject;

                                        if (myComparativeDate == null) {
                                            myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }

                                    else {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                    }
                                }

                                if (entityPM.DueDate != myComparativeDate) {
                                    entityPM.DueDate = myComparativeDate;
                                }
                            }

                            else {
                                var myComparativeDate: Date = null;

                                if (entityPM.IsConsolidationInvoice) {
                                    myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }

                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.OperationalDate).DateObject;

                                        if (myComparativeDate == null) {
                                            myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }

                                    else {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                    }
                                }

                                if (myComparativeDate != null) {
                                    var dateYear = myComparativeDate.getUTCFullYear();
                                    var dateMonth = myComparativeDate.getUTCMonth() + 1;
                                    var dateDay = myComparativeDate.getUTCDate();

                                    if (list.CurrentMonth) {
                                        dateMonth += 1;
                                        dateDay = 1;
                                    }

                                    var myDate = new Date();
                                    myDate.setUTCFullYear(dateYear);
                                    myDate.setUTCMonth(dateMonth - 1);
                                    myDate.setUTCDate(dateDay);
                                    myDate.setUTCHours(0);
                                    myDate.setUTCMinutes(0);
                                    myDate.setUTCSeconds(0);
                                    myDate.setUTCMilliseconds(0);

                                    myComparativeDate = myDate;
                                    myComparativeDate.setUTCDate(myComparativeDate.getUTCDate() + list.Days);

                                    if (entityPM.DueDate != myComparativeDate) {
                                        entityPM.DueDate = myComparativeDate;
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    }
    public static ComputeAPInvoiceDueDate(entityPM: APInvoicePM) {
        if (entityPM != null) {
            if (AppTool.IsNullOrEmpty(entityPM.PaymentTermId)) {
                entityPM.DueDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
            }

            else {
                var myService = new PaymentTermListService();
                myService.getSingleFromCache(entityPM.PaymentTermId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PaymentTermList = myResponse.Result;
                        if (list != null) {
                            if (list.IsManuallySet) {
                                entityPM.DueDate = null;
                            }

                            else if (AppTool.IsNullOrZero(list.Days)) {

                                var myComparativeDate: Date = null;

                                if (entityPM.IsMultipleEntities) {
                                    myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }

                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.OperationalDate).DateObject;

                                        if (myComparativeDate == null) {
                                            myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }

                                    else {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                    }
                                }

                                if (entityPM.DueDate != myComparativeDate) {
                                    entityPM.DueDate = myComparativeDate;
                                }
                            }

                            else {
                                var myComparativeDate: Date = null;

                                if (entityPM.IsMultipleEntities) {
                                    myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }

                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.OperationalDate).DateObject;

                                        if (myComparativeDate == null) {
                                            myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }

                                    else {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                    }
                                }

                                if (myComparativeDate != null) {
                                    var dateYear = myComparativeDate.getUTCFullYear();
                                    var dateMonth = myComparativeDate.getUTCMonth() + 1;
                                    var dateDay = myComparativeDate.getUTCDate();

                                    if (list.CurrentMonth) {
                                        dateMonth += 1;
                                        dateDay = 1;
                                    }

                                    var myDate = new Date();
                                    myDate.setUTCFullYear(dateYear);
                                    myDate.setUTCMonth(dateMonth - 1);
                                    myDate.setUTCDate(dateDay);
                                    myDate.setUTCHours(0);
                                    myDate.setUTCMinutes(0);
                                    myDate.setUTCSeconds(0);
                                    myDate.setUTCMilliseconds(0);

                                    myComparativeDate = myDate;
                                    myComparativeDate.setUTCDate(myComparativeDate.getUTCDate() + list.Days);

                                    if (entityPM.DueDate != myComparativeDate) {
                                        entityPM.DueDate = myComparativeDate;
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    }
    public static ComputeFullAccountingAPInvoiceDueDate(entityPM: APInvoicePM) {
        if (entityPM != null) {
            if (AppTool.IsNullOrEmpty(entityPM.PaymentTermId)) {
                entityPM.DueDate = DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
            }

            else {
                var myService = new PaymentTermListService();
                myService.getSingleFromCache(entityPM.PaymentTermId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PaymentTermList = myResponse.Result;
                        if (list != null) {
                            if (list.IsManuallySet) {
                                entityPM.DueDate = null;
                            }

                            else {
                                var myComparativeDate: Date = null;

                                if (entityPM.IsMultipleEntities) {
                                    myComparativeDate = DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
                                }

                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.OperationalDate).DateObject;

                                        if (myComparativeDate == null) {
                                            myComparativeDate = DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
                                        }
                                    }

                                    else {
                                        myComparativeDate = DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
                                    }
                                }

                                if (myComparativeDate != null) {
                                    var dateYear = myComparativeDate.getUTCFullYear();
                                    var dateMonth = myComparativeDate.getUTCMonth() + 1;
                                    var dateDay = myComparativeDate.getUTCDate();

                                    if (list.CurrentMonth) {
                                        dateMonth += 1;
                                        dateDay = 1;
                                    }

                                    var myDate = new Date();
                                    myDate.setUTCFullYear(dateYear);
                                    myDate.setUTCMonth(dateMonth - 1);
                                    myDate.setUTCDate(dateDay);
                                    myDate.setUTCHours(0);
                                    myDate.setUTCMinutes(0);
                                    myDate.setUTCSeconds(0);
                                    myDate.setUTCMilliseconds(0);

                                    myComparativeDate = myDate;
                                    myComparativeDate.setUTCDate(myComparativeDate.getUTCDate() + list.Days);

                                    if (entityPM.DueDate != myComparativeDate) {
                                        entityPM.DueDate = myComparativeDate;
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    }
    public static ComputeARInvoicePaymentTerm(entityPM: ARInvoicePM) {
        if (entityPM != null) {
            if (entityPM.DueDate != null && entityPM.InvoiceDate != null) {
                var myPaymentTermId: string = null;

                var days = DateTool.GetDaysBetweenDates(entityPM.DueDate, entityPM.InvoiceDate);

                var myService = new PaymentTermListService();
                myService.getAll().subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var all: PaymentTermList[] = myResponse.Result;
                        var list = all.filter(f => f.Days == days)[0];

                        if (list != null) {
                            myPaymentTermId = list.Id;
                        }

                        else {
                            var list = all.filter(f => f.Days == 0 && f.IsManuallySet == true)[0];
                            if (list != null) {
                                myPaymentTermId = list.Id;
                            }
                        }

                        entityPM.PaymentTermId = myPaymentTermId;
                    }
                });                
            }
        }
    }
    public static ComputeAPInvoicePaymentTerm(entityPM: APInvoicePM) {
        if (entityPM != null) {
            if (entityPM.DueDate != null && entityPM.InvoiceDate != null) {
                var myPaymentTermId: string = null;

                var days = DateTool.GetDaysBetweenDates(entityPM.DueDate, entityPM.InvoiceDate);

                var myService = new PaymentTermListService();
                myService.getAll().subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var all: PaymentTermList[] = myResponse.Result;
                        var list = all.filter(f => f.Days == days)[0];

                        if (list != null) {
                            myPaymentTermId = list.Id;
                        }

                        else {
                            var list = all.filter(f => f.Days == 0 && f.IsManuallySet == true)[0];
                            if (list != null) {
                                myPaymentTermId = list.Id;
                            }
                        }

                        entityPM.PaymentTermId = myPaymentTermId;
                    }
                });
            }
        }
    }

    public static GetBillToNotAllowConsolidation() {
        return "Bill to is not allowed for consolidation invoices";
    }
}
export class CreditLimitHelper {
    private EntityPM: ARInvoicePM
    public HasCreditLimitFeature: boolean = false;
    private HasCreditOverrideFeature: boolean = false;
    public IsCreditLimitActivated: boolean = false;
    private IsCreditLimitHasAction: boolean = false;
    public IsBlockingShipment: boolean = false;
    public IsActivated: boolean = false;
    public IsValid: boolean = false;
    public Errors: string[] = [];
    public Warnings: string[] = [];
    constructor(entityPM: ARInvoicePM) {
        this.EntityPM = entityPM;
        this.HasCreditLimitFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        this.HasCreditOverrideFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
        this.EntityPM.HasCreditLimitOverrideFeature = this.HasCreditOverrideFeature;

        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            this.IsCreditLimitHasAction = (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock == true || ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning == true) ? true : false;
        }

        if (this.HasCreditLimitFeature && this.IsCreditLimitActivated && this.IsCreditLimitHasAction && this.EntityPM.BillToIsCreditLimitEnabled) {
            this.IsActivated = true;
        }
    }

    public Run(loadedAmount: number) {
        this.Errors = [];
        this.Warnings = [];
        this.IsValid = false;
        this.IsBlockingShipment = false;

        this.EntityPM.BillToCreditLimitActualAmount = loadedAmount;
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
                this.Errors.push(LimitError);
            }

            else if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                this.Warnings.push(LimitWarning);
            }
        }

        else if (isBillToHasWarningPercentage && ActualBalance > (WarningPercentage * LimitAmount / 100)) {
            if (ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {

                var RemainingLimit = FormatTool.FormatNumber(LimitAmount - ActualBalance);
                var PercentageWarning: string = "The remaining credit limit for this customer is (" + RemainingLimit + ")";
                this.Warnings.push(PercentageWarning);
            }
        }

        if (this.Errors.length == 0 && this.Warnings.length == 0) {
            this.IsValid = true;
        }

        if (this.Errors.length > 0) {
            this.IsBlockingShipment = true;
        }
    }
}

export class InvoicePartnerType {
    public PartnerTypeId: string;
    public Code: string;
    public Name: string;
    public BillToId: string;
    public IsCustomer: boolean;
    constructor(myPartnerTypeId: string, myCode: string, myName: string, myBillToId: string = null, isCustomer: boolean = false) {
        this.PartnerTypeId = myPartnerTypeId;
        this.Code = myCode;
        this.Name = myName;
        this.BillToId = myBillToId;
        this.IsCustomer = isCustomer;
    }
}
