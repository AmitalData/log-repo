"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Infrastructure/Tools");
var PaymentTermListService_1 = require("../Common/Services/StandardLists/PaymentTermListService");
var FeatureLocator_1 = require("../Infrastructure/Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../Infrastructure/Locators/ObjectsLocator");
var InvoiceTool = /** @class */ (function () {
    function InvoiceTool() {
    }
    InvoiceTool.IsEditingARInvoiceEnabled = function (entityPM) {
        var myResult = false;
        if (entityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.Id) && !entityPM.IsAutoCredit) {
                myResult = true;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.StatusCode)) {
                myResult = true;
            }
            else if (entityPM.StatusCode == "DR") {
                myResult = true;
            }
            else if (entityPM.IsConstituentInvoice) {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.ConsolidationInvoiceId) && entityPM.StatusCode == "NT") {
                    myResult = true;
                }
            }
        }
        return myResult;
    };
    InvoiceTool.IsEditingAPInvoiceEnabled = function (entityPM) {
        var myResult = false;
        if (entityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.Id)) {
                myResult = true;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(entityPM.StatusCode)) {
                myResult = true;
            }
            else if (entityPM.StatusCode == "WA") {
                myResult = true;
            }
        }
        return myResult;
    };
    InvoiceTool.IsEditingARPaymentEnabled = function (entityPM) {
        var myResult = false;
        if (entityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.StatusCode) || entityPM.StatusCode == "DR") {
                myResult = true;
            }
        }
        return myResult;
    };
    InvoiceTool.IsEditingAPPaymentEnabled = function (entityPM) {
        var myResult = false;
        if (entityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.StatusCode) || entityPM.StatusCode == "DR") {
                myResult = true;
            }
        }
        return myResult;
    };
    InvoiceTool.GetARInvoicePartners = function (shipmentPM) {
        var invoicePartners = [];
        if (shipmentPM != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.CustomerId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "CUS", "Customer"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ShipperId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "SHI", "Shipper"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ConsigneeId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "CON", "Consignee"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.AgentId)) {
                invoicePartners.push(new InvoicePartnerType("AG", "AGE", "Agent"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.CustomAgentExportId)) {
                invoicePartners.push(new InvoicePartnerType("CG", "CGE", "Custom Agent Export"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.CustomAgentImportId)) {
                invoicePartners.push(new InvoicePartnerType("CG", "CGI", "Custom Agent Import"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.Notify1Id)) {
                invoicePartners.push(new InvoicePartnerType("CS", "NOT1", "Notify1"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.Notify2Id)) {
                invoicePartners.push(new InvoicePartnerType("CS", "NOT2", "Notify2"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ShipperNotExporterId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "SNE", "Shipper Not Exporter"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ConsigneeNotImporterId)) {
                invoicePartners.push(new InvoicePartnerType("CS", "CNI", "Consignee Not Importer"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.FreightForwarderId)) {
                invoicePartners.push(new InvoicePartnerType("AG", "FFW", "Freight Forwarder"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(shipmentPM.ConsolidatorId)) {
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
    };
    InvoiceTool.GetBillToPartnerTypes = function () {
        return "CS,AG,AL,CG,SG,SL,TR,VD,WH";
    };
    InvoiceTool.GetVendorPartnerTypes = function () {
        return "AG,AL,CG,SG,SL,TR,VD,WH";
    };
    InvoiceTool.GetOperationalDate = function (shipmentPM) {
        var myResult = null;
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
    };
    InvoiceTool.ComputeARInvoiceDueDate = function (entityPM) {
        if (entityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.PaymentTermId)) {
                entityPM.DueDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
            }
            else {
                var myService = new PaymentTermListService_1.PaymentTermListService();
                myService.getSingleFromCache(entityPM.PaymentTermId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list != null) {
                            if (list.IsManuallySet) {
                                entityPM.DueDate = null;
                            }
                            else if (Tools_1.AppTool.IsNullOrZero(list.Days)) {
                                var myComparativeDate = null;
                                if (entityPM.IsConsolidationInvoice) {
                                    myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }
                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.OperationalDate).DateObject;
                                        if (myComparativeDate == null) {
                                            myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }
                                    else {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                    }
                                }
                                if (entityPM.DueDate != myComparativeDate) {
                                    entityPM.DueDate = myComparativeDate;
                                }
                            }
                            else {
                                var myComparativeDate = null;
                                if (entityPM.IsConsolidationInvoice) {
                                    myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }
                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.OperationalDate).DateObject;
                                        if (myComparativeDate == null) {
                                            myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }
                                    else {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
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
    };
    InvoiceTool.ComputeAPInvoiceDueDate = function (entityPM) {
        if (entityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.PaymentTermId)) {
                entityPM.DueDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
            }
            else {
                var myService = new PaymentTermListService_1.PaymentTermListService();
                myService.getSingleFromCache(entityPM.PaymentTermId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list != null) {
                            if (list.IsManuallySet) {
                                entityPM.DueDate = null;
                            }
                            else if (Tools_1.AppTool.IsNullOrZero(list.Days)) {
                                var myComparativeDate = null;
                                if (entityPM.IsMultipleEntities) {
                                    myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }
                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.OperationalDate).DateObject;
                                        if (myComparativeDate == null) {
                                            myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }
                                    else {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                    }
                                }
                                if (entityPM.DueDate != myComparativeDate) {
                                    entityPM.DueDate = myComparativeDate;
                                }
                            }
                            else {
                                var myComparativeDate = null;
                                if (entityPM.IsMultipleEntities) {
                                    myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                }
                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.OperationalDate).DateObject;
                                        if (myComparativeDate == null) {
                                            myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
                                        }
                                    }
                                    else {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.InvoiceDate).DateObject;
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
    };
    InvoiceTool.ComputeFullAccountingAPInvoiceDueDate = function (entityPM) {
        if (entityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.PaymentTermId)) {
                entityPM.DueDate = Tools_1.DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
            }
            else {
                var myService = new PaymentTermListService_1.PaymentTermListService();
                myService.getSingleFromCache(entityPM.PaymentTermId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list != null) {
                            if (list.IsManuallySet) {
                                entityPM.DueDate = null;
                            }
                            else {
                                var myComparativeDate = null;
                                if (entityPM.IsMultipleEntities) {
                                    myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
                                }
                                else {
                                    if (list.FromDateTypeCode == "SHI") {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.OperationalDate).DateObject;
                                        if (myComparativeDate == null) {
                                            myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
                                        }
                                    }
                                    else {
                                        myComparativeDate = Tools_1.DateTool.GetDateParts(entityPM.AccountingDate).DateObject;
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
    };
    InvoiceTool.ComputeARInvoicePaymentTerm = function (entityPM) {
        if (entityPM != null) {
            if (entityPM.DueDate != null && entityPM.InvoiceDate != null) {
                var myPaymentTermId = null;
                var days = Tools_1.DateTool.GetDaysBetweenDates(entityPM.DueDate, entityPM.InvoiceDate);
                var myService = new PaymentTermListService_1.PaymentTermListService();
                myService.getAll().subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var all = myResponse.Result;
                        var list = all.filter(function (f) { return f.Days == days; })[0];
                        if (list != null) {
                            myPaymentTermId = list.Id;
                        }
                        else {
                            var list = all.filter(function (f) { return f.Days == 0 && f.IsManuallySet == true; })[0];
                            if (list != null) {
                                myPaymentTermId = list.Id;
                            }
                        }
                        entityPM.PaymentTermId = myPaymentTermId;
                    }
                });
            }
        }
    };
    InvoiceTool.ComputeAPInvoicePaymentTerm = function (entityPM) {
        if (entityPM != null) {
            if (entityPM.DueDate != null && entityPM.InvoiceDate != null) {
                var myPaymentTermId = null;
                var days = Tools_1.DateTool.GetDaysBetweenDates(entityPM.DueDate, entityPM.InvoiceDate);
                var myService = new PaymentTermListService_1.PaymentTermListService();
                myService.getAll().subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var all = myResponse.Result;
                        var list = all.filter(function (f) { return f.Days == days; })[0];
                        if (list != null) {
                            myPaymentTermId = list.Id;
                        }
                        else {
                            var list = all.filter(function (f) { return f.Days == 0 && f.IsManuallySet == true; })[0];
                            if (list != null) {
                                myPaymentTermId = list.Id;
                            }
                        }
                        entityPM.PaymentTermId = myPaymentTermId;
                    }
                });
            }
        }
    };
    InvoiceTool.GetBillToNotAllowConsolidation = function () {
        return "Bill to is not allowed for consolidation invoices";
    };
    return InvoiceTool;
}());
exports.InvoiceTool = InvoiceTool;
var CreditLimitHelper = /** @class */ (function () {
    function CreditLimitHelper(entityPM) {
        this.HasCreditLimitFeature = false;
        this.HasCreditOverrideFeature = false;
        this.IsCreditLimitActivated = false;
        this.IsCreditLimitHasAction = false;
        this.IsBlockingShipment = false;
        this.IsActivated = false;
        this.IsValid = false;
        this.Errors = [];
        this.Warnings = [];
        this.EntityPM = entityPM;
        this.HasCreditLimitFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        this.HasCreditOverrideFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
        this.EntityPM.HasCreditLimitOverrideFeature = this.HasCreditOverrideFeature;
        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            this.IsCreditLimitHasAction = (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock == true || ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning == true) ? true : false;
        }
        if (this.HasCreditLimitFeature && this.IsCreditLimitActivated && this.IsCreditLimitHasAction && this.EntityPM.BillToIsCreditLimitEnabled) {
            this.IsActivated = true;
        }
    }
    CreditLimitHelper.prototype.Run = function (loadedAmount) {
        this.Errors = [];
        this.Warnings = [];
        this.IsValid = false;
        this.IsBlockingShipment = false;
        this.EntityPM.BillToCreditLimitActualAmount = loadedAmount;
        this.EntityPM.BillToCreditLimitActualBalance = Tools_1.AppTool.AddAmounts(this.EntityPM.BillToCreditLimitOpenBalance, this.EntityPM.BillToCreditLimitActualAmount);
        var LimitAmount = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitAmount) ? 0 : this.EntityPM.BillToCreditLimitAmount;
        var WarningPercentage = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitWarningPercentage) ? 0 : this.EntityPM.BillToCreditLimitWarningPercentage;
        var ActualBalance = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitActualBalance) ? 0 : this.EntityPM.BillToCreditLimitActualBalance;
        var isBillToHasLimitAmount = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitAmount) ? false : true;
        var isBillToHasWarningPercentage = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToCreditLimitWarningPercentage) ? false : true;
        if (isBillToHasLimitAmount && ActualBalance > LimitAmount) {
            var LimitError = "";
            var LimitWarning = "The customer exceeded the credit limit available.";
            LimitError += "Bill To exceeded its credit limit of " + Tools_1.FormatTool.FormatNumber(LimitAmount) + " (" + this.EntityPM.LocalCurrencyCode + ").";
            LimitError += " ";
            LimitError += "The current balance stands on " + Tools_1.FormatTool.FormatNumber(ActualBalance) + " (" + this.EntityPM.LocalCurrencyCode + ").";
            if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationBlock) {
                this.Errors.push(LimitError);
            }
            else if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                this.Warnings.push(LimitWarning);
            }
        }
        else if (isBillToHasWarningPercentage && ActualBalance > (WarningPercentage * LimitAmount / 100)) {
            if (ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.InvoiceCreationWarning) {
                var RemainingLimit = Tools_1.FormatTool.FormatNumber(LimitAmount - ActualBalance);
                var PercentageWarning = "The remaining credit limit for this customer is (" + RemainingLimit + ")";
                this.Warnings.push(PercentageWarning);
            }
        }
        if (this.Errors.length == 0 && this.Warnings.length == 0) {
            this.IsValid = true;
        }
        if (this.Errors.length > 0) {
            this.IsBlockingShipment = true;
        }
    };
    return CreditLimitHelper;
}());
exports.CreditLimitHelper = CreditLimitHelper;
var InvoicePartnerType = /** @class */ (function () {
    function InvoicePartnerType(myPartnerTypeId, myCode, myName, myBillToId, isCustomer) {
        if (myBillToId === void 0) { myBillToId = null; }
        if (isCustomer === void 0) { isCustomer = false; }
        this.PartnerTypeId = myPartnerTypeId;
        this.Code = myCode;
        this.Name = myName;
        this.BillToId = myBillToId;
        this.IsCustomer = isCustomer;
    }
    return InvoicePartnerType;
}());
exports.InvoicePartnerType = InvoicePartnerType;
//# sourceMappingURL=Tools.js.map