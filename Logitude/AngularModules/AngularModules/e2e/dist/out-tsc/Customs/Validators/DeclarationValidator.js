"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var DeclarationValidator = /** @class */ (function () {
    function DeclarationValidator() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    DeclarationValidator.prototype.SetEntityPM = function (declarationPM) {
        this._DeclarationPM = declarationPM;
    };
    DeclarationValidator.prototype.ValidateSupplierInvoiceItem = function (supplierInvoiceItemPM) {
        var errors = [];
        Validator_1.Validator.TryValidateObject(supplierInvoiceItemPM, "Customs.SupplierInvoiceItem", errors);
        for (var _i = 0, _a = supplierInvoiceItemPM.SupplierInvoiceItemsMods; _i < _a.length; _i++) {
            var item = _a[_i];
            Validator_1.Validator.TryValidateObject(item, "Customs.SupplierInvoiceItemsMod", errors);
        }
        for (var _b = 0, _c = supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars; _b < _c.length; _b++) {
            var item = _c[_b];
            Validator_1.Validator.TryValidateObject(item, "Customs.SupplierInvoiceItemsConDeclar", errors);
        }
        for (var _d = 0, _e = supplierInvoiceItemPM.SupplierInvoiceItemLevies; _d < _e.length; _d++) {
            var item = _e[_d];
            Validator_1.Validator.TryValidateObject(item, "Customs.SupplierInvoiceItemsLevy", errors);
        }
        //
        // Grid Empty Fields check
        //
        // ProcesType
        for (var _f = 0, _g = supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes; _f < _g.length; _f++) {
            var item = _g[_f];
            if (Tools_1.AppTool.IsNullOrEmpty(item.ProcessTypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemProcesType.F.ProcessTypeCode"));
            }
        }
        // ConDeclars
        for (var _h = 0, _j = supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars; _h < _j.length; _h++) {
            var item = _j[_h];
            if (Tools_1.AppTool.IsNullOrEmpty(item.DeclarationTypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.DeclarationTypeCode"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.DeclarationNumber"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.InvoiceNumber)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.InvoiceNumber"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.ItemSequence)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.ItemSequence"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.Quantity"));
            }
        }
        // SerialNums
        for (var _k = 0, _l = supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums; _k < _l.length; _k++) {
            var item = _l[_k];
            if (Tools_1.AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsSerialNum.F.TypeCode"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.SerialNumber)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsSerialNum.F.SerialNumber"));
            }
        }
        // Descripts
        for (var _m = 0, _o = supplierInvoiceItemPM.SupplierInvoiceItemsDescripts; _m < _o.length; _m++) {
            var item = _o[_m];
            if (Tools_1.AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsDescript.F.TypeCode"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.Description)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsDescript.F.Description"));
            }
        }
        // ProdIdents
        for (var _p = 0, _q = supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents; _p < _q.length; _p++) {
            var item = _q[_p];
            if (Tools_1.AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsProdIdent.F.TypeCode"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.Identification)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsProdIdent.F.Identification"));
            }
        }
        // Levies
        for (var _r = 0, _s = supplierInvoiceItemPM.SupplierInvoiceItemLevies; _r < _s.length; _r++) {
            var item = _s[_r];
            if (Tools_1.AppTool.IsNullOrEmpty(item.TradeLevyExamptCode) && Tools_1.AppTool.IsNullOrEmpty(item.TradeLevyNumber)) {
                errors.push("יש למלא קוד פטור או זיהוי");
                break;
            }
            //else if (!AppTool.IsNullOrEmpty(item.TradeLevyExamptCode) && !AppTool.IsNullOrEmpty(item.TradeLevyNumber)) {//task 36728 --mohammad
            //    errors.push("יש למלא קוד פטור או זיהוי ולא גם וגם");
            //}
            else {
                // do nothing one of them is filled.
            }
            //if (AppTool.IsNullOrEmpty(item.TradeLevyNumber)) {
            //    errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsLevy.F.TradeLevyNumber"));
            //}
        }
        //
        // Fields with two DDL (currency and amount)
        //
        // WholeSaleItemPriceCurrencyCode
        if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPrice) && Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.WholeSaleItemPriceCurrencyCode"));
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPrice) && !Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.WholeSaleItemPrice"));
        }
        // NonCustomsItemPriceCurCode
        if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPrice) && Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPriceCurCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.NonCustomsItemPriceCurCode"));
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPrice) && !Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPriceCurCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.NonCustomsItemPrice"));
        }
        // AdditionalQuantityType
        if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantity) && Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.AdditionalQuantityType"));
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantity) && !Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.AdditionalQuantity"));
        }
        // StatisticQuantityType
        if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantity) && Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.StatisticQuantityType"));
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantity) && !Tools_1.AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.StatisticQuantity"));
        }
        return errors;
    };
    //Check if declaration was already paid
    DeclarationValidator.prototype.PaymentDateCheck = function () {
        var errorMessage = "";
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.PaymentDate) {
                errorMessage = "Customs.General.O.NoPaymentDate";
                if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    };
    //Check if Importer exists
    DeclarationValidator.prototype.ImporterCheck = function () {
        if (this._DeclarationPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(this._DeclarationPM.ImporterId)) {
                var errorMessage = "";
                if (!Tools_1.AppTool.IsNullOrEmpty(this._DeclarationPM.ImporterCode)) {
                    errorMessage = "Customs.General.O.ImporterCodeNoId";
                }
                else {
                    errorMessage = "Customs.General.O.NoImporterId";
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    };
    DeclarationValidator.prototype.ImportersCheck = function (importerField, importerCode, importerId, importerType, importerName, importerAddress, importerPassportNumber, importerPassCountryCode) {
        var errorMessage = "";
        if (this._DeclarationPM != null) {
            switch (importerType) {
                case "1":
                    if (Tools_1.AppTool.IsNullOrEmpty(importerId)) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(importerCode)) {
                            //ALLREADY DONE INRAMALLA - (TASK 16825)
                            //search the code in Clients table 
                            //if (false)
                            //{
                            //    errorMessage = "יש לשלוף יבואן מהמכס לפני שליחה";
                            //}
                        }
                        else {
                            //Check if ImporterName & ImporterAddrress has value
                            if (Tools_1.AppTool.IsNullOrEmpty(importerName) && Tools_1.AppTool.IsNullOrEmpty(importerAddress)) {
                                errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                            }
                        }
                    }
                    break;
                case "2":
                case "3":
                    {
                        //Check That Both PassportCountry & PassportNumber has values
                        if (Tools_1.AppTool.IsNullOrEmpty(importerPassportNumber) || Tools_1.AppTool.IsNullOrEmpty(importerPassCountryCode)) {
                            errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                        }
                        break;
                    }
                default:
                    {
                        if (Tools_1.AppTool.IsNullOrEmpty(importerId)) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(importerCode)) {
                                //errorMessage = "Customs.General.O.ImporterCodeNoId";
                                errorMessage = "יש לשלוף לקוח מהמכס עבור יבואן " + importerField + " לפני שליחה";
                            }
                            else {
                                //errorMessage = "Customs.General.O.NoImporterId";
                                errorMessage = "מספר יבואן " + importerField + " הוא שדה חובה";
                            }
                        }
                        break;
                    }
            }
        }
        return (errorMessage);
    };
    //Check constraints in progress
    DeclarationValidator.prototype.ConstraintsInProgressCheck = function () {
        //Check if Declaration Paid and there waiting for constraint approval
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.DeclarationStatusTypeCode == "11") {
                var errorMessage = "Customs.General.O.ConstraintsInProgress";
                if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
            //Check if there are constraints in progress
            //if (false)//yaron
            //{
            //    if (this._DeclarationPM.DeclarationStatusTypeCode == "13") {
            //        var errorMessage = "";
            //        if (this._DeclarationPM.DeclarationConstraints != null) {
            //            if (this._DeclarationPM.DeclarationConstraints.length > 0) {
            //                errorMessage = "Customs.General.O.ConstraintsInProgress2";
            //            }
            //        }
            //        if (!AppTool.IsNullOrEmpty(errorMessage)) {
            //            this.ValidationErrorMessageCodes.push(errorMessage);
            //        }
            //    }
            //}
        }
    };
    //Check if future payment was done
    DeclarationValidator.prototype.FuturePaymentDoneCheck = function () {
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.DeclarationStatusTypeCode == "10") {
                var errorMessage = "Customs.General.O.FuturePaymentDone";
                if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    };
    //An indication to send declaration again- in this case only Declaration Payment is allowed // Mirit 25/06/15 Task 14330
    DeclarationValidator.prototype.SubmitDeclarationAgainDoneCheck = function () {
        if (this._DeclarationPM.DeclarationStatusTypeCode == "14") {
            var errorMessage = "Customs.General.O.SubmitDeclarationAgain";
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    };
    //Check if future payment was done
    DeclarationValidator.prototype.TaxationDateTimeCheck = function () {
        var errorMessage = "";
        if (this._DeclarationPM != null) {
            if (!this._DeclarationPM.TaxationDateTime) {
                errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            }
            else {
                var taxationDateTime = Tools_1.DateTool.GetDateParts(this._DeclarationPM.TaxationDateTime).DateObject; // date 00:00:00
                var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                if ((taxationDateTime.getFullYear() != todayDate.getUTCFullYear()) ||
                    (taxationDateTime.getMonth() != todayDate.getUTCMonth()) ||
                    (taxationDateTime.getDate() != todayDate.getUTCDate())) {
                    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
        return errorMessage;
    };
    //Check if there is at least one Supplier Invoice AND each Supplier Invoice has at least one Item
    DeclarationValidator.prototype.CheckSupplierInvoice = function () {
        var errorMessage = "";
        //Check if there is at least one Supplier Invoice
        if (this._DeclarationPM.SupplierInvoices == null) {
            errorMessage = "Customs.General.O.NoSupplierInvoiceForDeclaration";
        }
        else {
            if (this._DeclarationPM.SupplierInvoices.length == 0) {
                errorMessage = "Customs.General.O.NoSupplierInvoiceForDeclaration";
            }
            //If there is at least one Supplier Invoice
            else {
                //Check that each Supplier Invoice has at least one Item
                for (var i = 0; i < this._DeclarationPM.SupplierInvoices.length; i++) {
                    if (this._DeclarationPM.SupplierInvoices[i].SupplierInvoiceItems == null) {
                        errorMessage = "Customs.General.O.NoSupplierInvoiceItemForInvoice";
                    }
                    else {
                        if (this._DeclarationPM.SupplierInvoices[i].SupplierInvoiceItems.length == 0) {
                            errorMessage = "Customs.General.O.NoSupplierInvoiceItemForInvoice";
                        }
                    }
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
            this.ValidationErrorMessageCodes.push(errorMessage);
        }
    };
    //Check if there is at least one Package for each Consignment
    DeclarationValidator.prototype.CheckConsignmentPackages = function () {
        var errorMessage = "";
        if (this._DeclarationPM.Consignments == null) {
            return;
        }
        for (var i = 0; i < this._DeclarationPM.Consignments.length; i++) {
            if (this._DeclarationPM.Consignments[i].ConsignmentPackages == null) {
                errorMessage = "Customs.General.O.NoConsignmentPackages";
            }
            else {
                if (this._DeclarationPM.Consignments[i].ConsignmentPackages.length == 0) {
                    errorMessage = "Customs.General.O.NoConsignmentPackages";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    };
    //Validation Checks before sending Declaration to customs
    DeclarationValidator.prototype.PreDeclarationSendChecks = function () {
        var errorMessage = "";
        //CheckSupplierInvoice();
        this.PaymentDateCheck();
        //ImporterCheck();
        //Importer
        errorMessage = this.ImportersCheck("", this._DeclarationPM.ImporterCode, this._DeclarationPM.ImporterId, this._DeclarationPM.ImporterTypeCode, this._DeclarationPM.ImporterName, this._DeclarationPM.ImporterAddress, this._DeclarationPM.ImporterPassportNumber, this._DeclarationPM.ImporterPassCountryCode);
        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
            this.ValidationErrorMessageCodes.push(errorMessage);
        }
        //Transfer Importer
        //errorMessage = ImportersCheck("מעביר", _DeclarationPM.TransferImporterCode, _DeclarationPM.TransferImporterId, _DeclarationPM.TransferImporterTypeCode, _DeclarationPM.TransferImporterName, _DeclarationPM.TransferImporterAddress, _DeclarationPM.TransferPassportNumber, _DeclarationPM.TransferImporterCountryCode);
        //if (!string.IsNullOrWhiteSpace(errorMessage))
        //{
        //    ErrorCode.Add(errorMessage);
        //}
        //Transfer Importer
        //errorMessage = ImportersCheck("זכאי", _DeclarationPM.EntitleImporterCode, _DeclarationPM.EntitleImporterId, _DeclarationPM.EntitleImporterTypeCode, _DeclarationPM.EntitleImporterName, _DeclarationPM.EntitleImporterAddress, _DeclarationPM.EntitlePassportNumber, _DeclarationPM.EntitleImporterCountryCode);
        //if (!string.IsNullOrWhiteSpace(errorMessage))
        //{
        //    ErrorCode.Add(errorMessage);
        //}
        //allready done by displayt only - pls check ConstraintsInProgressCheck();
        //allready done by displayt only - pls check FuturePaymentDoneCheck();
        this.CheckConsignmentPackages();
    };
    //Check if it's a converted declaration (IsConvertedDeclaration=True)  // Mirit 02/12/15 Task 18508
    DeclarationValidator.prototype.CheckIsCoverteedDeclaration = function () {
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.IsConvertedDeclaration == true) {
                var errorMessage = "Customs.General.O.IsConvertedDeclaration";
                if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    };
    //Check if declaration is close
    DeclarationValidator.prototype.CheckIsCloseDeclaration = function () {
        var errorMessage = "";
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.IsClose) {
                errorMessage = "Customs.Declaration.O.Closed";
                if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    };
    //<--- Yuval Chalup 18.11.2014 TASK-4240
    //Checks for opening Declaration view as 'Display Only'
    DeclarationValidator.prototype.DeclarationViewDisplayOnlyChecks = function () {
        this.PaymentDateCheck();
        this.ConstraintsInProgressCheck();
        this.FuturePaymentDoneCheck();
        //SubmitDeclarationAgainDoneCheck(); // Mirit 25/06/15 Task 14330 + Remarked by Yuval Chalup 02.08.2015 TASK-15145
        this.CheckIsCoverteedDeclaration(); // Mirit 02/12/15 Task 18508
        this.CheckIsCloseDeclaration();
    };
    //Yuval Chalup 18.11.2014 TASK-4240 --->
    //Check if there is an empty consignment package
    DeclarationValidator.prototype.EmptyConsignmentPackageCheck = function () {
        var errorMessage = "";
        if (this._DeclarationPM != null) {
            for (var _i = 0, _a = this._DeclarationPM.Consignments; _i < _a.length; _i++) {
                var item = _a[_i];
                for (var _b = 0, _c = item.ConsignmentPackages; _b < _c.length; _b++) {
                    var line = _c[_b];
                    if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                        errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                        if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
        }
    };
    DeclarationValidator.prototype.Validate = function (entityPM) {
        var result = [];
        this._DeclarationPM = entityPM;
        this.EmptyConsignmentPackageCheck();
        return this.ValidationErrorMessageCodes;
    };
    DeclarationValidator.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    return DeclarationValidator;
}());
exports.DeclarationValidator = DeclarationValidator;
//# sourceMappingURL=DeclarationValidator.js.map