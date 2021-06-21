declare var window: any;
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../Infrastructure/Validators/Validator';
import { LuhnAlgorithm } from '../../Customs/Utilities/LuhnAlgorithm';
import {DeclarationPM}          from '../EntityPMs/DeclarationPM';
import {SupplierInvoiceItemPM}  from '../EntityPMs/SupplierInvoiceItemPM';
import { DecDangersContactPM } from '../EntityPMs/DecDangersContactPM';
import { ConsignmentPackDangerPM } from '../EntityPMs/ConsignmentPackDangerPM';

export class DeclarationValidator {
    private _DeclarationPM: DeclarationPM;
    private FIELD_IS_REQUIERD: string;
    public ValidationErrorMessageCodes: string[];

    constructor() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public SetEntityPM(declarationPM: DeclarationPM) {
        this._DeclarationPM = declarationPM;
    }

    public validatePackagesDanger(decDangersContactPM: DecDangersContactPM, consignmentPackDangerPM: ConsignmentPackDangerPM) {
        var errors = [];

        Validator.TryValidateObject(decDangersContactPM, "Customs.DecDangersContact", errors);
        Validator.TryValidateObject(consignmentPackDangerPM, "Customs.ConsignmentPackDanger", errors);

        return errors;

    }

    public ValidateSupplierInvoiceItem(supplierInvoiceItemPM: SupplierInvoiceItemPM) {
        var errors = [];

        Validator.TryValidateObject(supplierInvoiceItemPM, "Customs.SupplierInvoiceItem", errors);

        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemsMods) {
            Validator.TryValidateObject(item, "Customs.SupplierInvoiceItemsMod", errors);
        }

        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars) {
            Validator.TryValidateObject(item, "Customs.SupplierInvoiceItemsConDeclar", errors);
        }

        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemLevies) {
            Validator.TryValidateObject(item, "Customs.SupplierInvoiceItemsLevy", errors);
        }

        //
        // Grid Empty Fields check
        //
        // ProcesType
        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes) {
            if (AppTool.IsNullOrEmpty(item.ProcessTypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemProcesType.F.ProcessTypeCode"));
            }
        }

        // ConDeclars
        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars) {
            if (AppTool.IsNullOrEmpty(item.DeclarationTypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.DeclarationTypeCode"));
            }

            if (AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.DeclarationNumber"));
            }

            if (AppTool.IsNullOrEmpty(item.InvoiceNumber)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.InvoiceNumber"));
            }

            if (AppTool.IsNullOrEmpty(item.ItemSequence)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.ItemSequence"));
            }

            if (AppTool.IsNullOrEmpty(item.Quantity)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsConDeclar.F.Quantity"));
            }
        }

        // SerialNums
        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums) {
            if (AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsSerialNum.F.TypeCode"));
            }

            if (AppTool.IsNullOrEmpty(item.SerialNumber)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsSerialNum.F.SerialNumber"));
            }


        }

        // Descripts
        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemsDescripts) {
            if (AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsDescript.F.TypeCode"));
            }

            if (AppTool.IsNullOrEmpty(item.Description)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsDescript.F.Description"));
            }


        }

        // ProdIdents
        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents) {
            if (AppTool.IsNullOrEmpty(item.TypeCode)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsProdIdent.F.TypeCode"));
            }

            if (AppTool.IsNullOrEmpty(item.Identification)) {
                errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItemsProdIdent.F.Identification"));
            }


        }

        // Levies
        for (let item of supplierInvoiceItemPM.SupplierInvoiceItemLevies) {
            if (AppTool.IsNullOrEmpty(item.TradeLevyExamptCode) && AppTool.IsNullOrEmpty(item.TradeLevyNumber)) {
                errors.push("יש למלם קוד פטור או זיהוי");
                break;
            }
            //else if (!AppTool.IsNullOrEmpty(item.TradeLevyExamptCode) && !AppTool.IsNullOrEmpty(item.TradeLevyNumber)) {//task 36728 --mohammad
            //    errors.push("יש למלם קוד פטור םו זיהוי ולם גם וגם");
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
        if (!AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPrice) && AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.WholeSaleItemPriceCurrencyCode"));
        }
        else if (AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPrice) && !AppTool.IsNullOrEmpty(supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.WholeSaleItemPrice"));
        }

        // NonCustomsItemPriceCurCode
        if (!AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPrice) && AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPriceCurCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.NonCustomsItemPriceCurCode"));

        }
        else if (AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPrice) && !AppTool.IsNullOrEmpty(supplierInvoiceItemPM.NonCustomsItemPriceCurCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.NonCustomsItemPrice"));

        }

        // AdditionalQuantityType
        if (!AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantity) && AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.AdditionalQuantityType"));

        }
        else if (AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantity) && !AppTool.IsNullOrEmpty(supplierInvoiceItemPM.AdditionalQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.AdditionalQuantity"));

        }

        // StatisticQuantityType
        if (!AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantity) && AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.StatisticQuantityType"));

        }
        else if (AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantity) && !AppTool.IsNullOrEmpty(supplierInvoiceItemPM.StatisticQuantityType)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvoiceItem.F.StatisticQuantity"));

        }

        return errors;
    }

    //Check if declaration was amendment  with status !=2 
    public IsAmendment() {
        var errorMessage: string = "";

        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.IsAmendment && this._DeclarationPM.AmendmentStatus != "2" && (!AppTool.IsNullOrEmpty(this._DeclarationPM.AmendmentStatus))) {
                errorMessage = "Customs.Declaration.O.IsAmendment";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                    return ' - ' +  this._DeclarationPM.AmendmentStatusName;
                }
            }
        }

        return "";
    }

    //Check if declaration was already paid
    public PaymentDateCheck() {
        var errorMessage: string = "";

        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.PaymentDate) {
                errorMessage = "Customs.General.O.NoPaymentDate";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    }

    //Check if Importer exists
    public ImporterCheck() {
        if (this._DeclarationPM != null) {
            if (AppTool.IsNullOrEmpty(this._DeclarationPM.ImporterId)) {
                var errorMessage = "";
                if (!AppTool.IsNullOrEmpty(this._DeclarationPM.ImporterCode)) {
                    errorMessage = "Customs.General.O.ImporterCodeNoId";
                }
                else {
                    errorMessage = "Customs.General.O.NoImporterId";
                }

                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    }

    public ImportersCheck(importerField: string, importerCode: string, importerId: string, importerType: string,
        importerName: string, importerAddress: string, importerPassportNumber: string, importerPassCountryCode: string) {
        var errorMessage = "";
        if (this._DeclarationPM != null) {
            switch (importerType) {
                case "1":
                    if (AppTool.IsNullOrEmpty(importerId)) {
                        if (!AppTool.IsNullOrEmpty(importerCode)) {
                            //ALLREADY DONE INRAMALLA - (TASK 16825)
                            //search the code in Clients table 
                            //if (false)
                            //{
                            //    errorMessage = "יש לשלוף יבוםן מהמכס לפני שליחה";
                            //}
                        }
                        else {
                            //Check if ImporterName & ImporterAddrress has value
                            if (AppTool.IsNullOrEmpty(importerName) && AppTool.IsNullOrEmpty(importerAddress)) {
                                errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                            }
                        }
                    }
                    break;
                case "2":
                case "3":
                    {

                        //Check That Both PassportCountry & PassportNumber has values
                        if (AppTool.IsNullOrEmpty(importerPassportNumber) || AppTool.IsNullOrEmpty(importerPassCountryCode)) {
                            errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                        }
                        break;
                    }
                default:
                    {
                        if (AppTool.IsNullOrEmpty(importerId)) {
                            if (!AppTool.IsNullOrEmpty(importerCode)) {
                                //errorMessage = "Customs.General.O.ImporterCodeNoId";
                                errorMessage = "יש לשלוף לקוח מהמכס עבור יבואן " + importerField + " לפני שליחה";
                            }
                            else {
                                //errorMessage = "Customs.General.O.NoImporterId";
                                errorMessage = "מספר יבואן " + importerField + " הום שדה חובה";
                            }
                        }
                        break;

                    }
            }
        }
        return (errorMessage);
    }

    //Check constraints in progress
    public ConstraintsInProgressCheck() {
        //Check if Declaration Paid and there waiting for constraint approval
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.DeclarationStatusTypeCode == "11") {
                var errorMessage = "Customs.General.O.ConstraintsInProgress";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
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
    }

    //Check if future payment was done
    public FuturePaymentDoneCheck() {
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.DeclarationStatusTypeCode == "10") {
                var errorMessage = "Customs.General.O.FuturePaymentDone";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    }

    //An indication to send declaration again- in this case only Declaration Payment is allowed // Mirit 25/06/15 Task 14330
    public SubmitDeclarationAgainDoneCheck() {
        if (this._DeclarationPM.DeclarationStatusTypeCode == "14") {
            var errorMessage = "Customs.General.O.SubmitDeclarationAgain";
            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    }

    //Check if future payment was done
    public TaxationDateTimeCheck() {
        var errorMessage = "";
        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.IsAmendment == true) return "";
            if (!this._DeclarationPM.TaxationDateTime) {
                errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            }
            else {
                var taxationDateTime: Date = DateTool.GetDateParts(this._DeclarationPM.TaxationDateTime).DateObject;// date 00:00:00
                var todayDate: Date = DateTool.GetCurrentDateAsUtc();
                if ((taxationDateTime.getFullYear() != todayDate.getUTCFullYear()) ||
                    (taxationDateTime.getMonth() != todayDate.getUTCMonth()) ||
                    (taxationDateTime.getDate() != todayDate.getUTCDate())) {
                    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                }
            }
            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
        return errorMessage;
    }

    //Check if there is at least one Supplier Invoice AND each Supplier Invoice has at least one Item
    public CheckSupplierInvoice() {
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


        if (!AppTool.IsNullOrEmpty(errorMessage)) {
            this.ValidationErrorMessageCodes.push(errorMessage);
        }
    }

    //Check if there is at least one Package for each Consignment
    private CheckConsignmentPackages() {
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

            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    }

    //Validation Checks before sending Declaration to customs
    public PreDeclarationSendChecks() {
        var errorMessage: string = "";
        //CheckSupplierInvoice();
        this.PaymentDateCheck();
        //ImporterCheck();
        //Importer
        errorMessage = this.ImportersCheck("", this._DeclarationPM.ImporterCode, this._DeclarationPM.ImporterId, this._DeclarationPM.ImporterTypeCode, this._DeclarationPM.ImporterName, this._DeclarationPM.ImporterAddress, this._DeclarationPM.ImporterPassportNumber, this._DeclarationPM.ImporterPassCountryCode);
        if (!AppTool.IsNullOrEmpty(errorMessage)) {
            this.ValidationErrorMessageCodes.push(errorMessage);
        }
        //Transfer Importer
        //errorMessage = ImportersCheck("מעביר", _DeclarationPM.TransferImporterCode, _DeclarationPM.TransferImporterId, _DeclarationPM.TransferImporterTypeCode, _DeclarationPM.TransferImporterName, _DeclarationPM.TransferImporterAddress, _DeclarationPM.TransferPassportNumber, _DeclarationPM.TransferImporterCountryCode);
        //if (!string.IsNullOrWhiteSpace(errorMessage))
        //{
        //    ErrorCode.Add(errorMessage);
        //}
        //Transfer Importer
        //errorMessage = ImportersCheck("זכםי", _DeclarationPM.EntitleImporterCode, _DeclarationPM.EntitleImporterId, _DeclarationPM.EntitleImporterTypeCode, _DeclarationPM.EntitleImporterName, _DeclarationPM.EntitleImporterAddress, _DeclarationPM.EntitlePassportNumber, _DeclarationPM.EntitleImporterCountryCode);
        //if (!string.IsNullOrWhiteSpace(errorMessage))
        //{
        //    ErrorCode.Add(errorMessage);
        //}

        //allready done by displayt only - pls check ConstraintsInProgressCheck();
        //allready done by displayt only - pls check FuturePaymentDoneCheck();
        this.CheckConsignmentPackages();
    }

    //Check if it's a converted declaration (IsConvertedDeclaration=True)  // Mirit 02/12/15 Task 18508
    public CheckIsConvertedDeclaration() {

        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.IsConvertedDeclaration == true) {
                var errorMessage = "Customs.General.O.IsConvertedDeclaration";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    }

    //Check if declaration is close
    public CheckIsCloseDeclaration() {
        var errorMessage: string = "";

        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.IsClose) {
                errorMessage = "Customs.Declaration.O.Closed";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
    }

    //<--- Yuval Chalup 18.11.2014 TASK-4240
    //Checks for opening Declaration view as 'Display Only'
    public DeclarationViewDisplayOnlyChecks() {
        //var error = "";
        //error= this.IsAmendment();
        this.PaymentDateCheck();
        this.ConstraintsInProgressCheck();
        this.FuturePaymentDoneCheck();
        //SubmitDeclarationAgainDoneCheck(); // Mirit 25/06/15 Task 14330 + Remarked by Yuval Chalup 02.08.2015 TASK-15145
        this.CheckIsConvertedDeclaration(); // Mirit 02/12/15 Task 18508
        this.CheckIsCloseDeclaration();
        this.CheckIfAutomaticPayment();
    }
    CheckIfAutomaticPayment() {

        var errorMessage: string = "";

        if (this._DeclarationPM != null) {
            if (this._DeclarationPM.AutomaticPayment) {
                //"הצהרה בתהליך תשלום םוטומטי - לתצוגה בלבד"
                errorMessage = "Customs.General.O.InAutomaticPayment";
                if (!AppTool.IsNullOrEmpty(errorMessage)) {
                    this.ValidationErrorMessageCodes.push(errorMessage);
                }
            }
        }
 

    }
    //Yuval Chalup 18.11.2014 TASK-4240 --->



    //Check if there is an empty consignment package
    public EmptyConsignmentPackageCheck() {
        var errorMessage: string = "";

        if (this._DeclarationPM != null) {
            for (let item of this._DeclarationPM.Consignments) {
                for (let line of item.ConsignmentPackages) {
                    if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                        errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                         if (!AppTool.IsNullOrEmpty(errorMessage)) {
                        this.ValidationErrorMessageCodes.push(errorMessage);

                    }
                }
              
                }
            }
        }
    }

    //Check if ImporterCode Valid
    public CheckIsImporterCodeValid() {

        if (this._DeclarationPM != null && !AppTool.IsNullOrEmpty(this._DeclarationPM.ImporterCode)) {

            this._DeclarationPM.ImporterTypeCode
            if (///this._DeclarationPM.ImporterCode[0] == "P" || this._DeclarationPM.ImporterCode[0] == "F") {
                this._DeclarationPM.ImporterTypeCode == "2" /*"P"*/ ||
                this._DeclarationPM.ImporterTypeCode == "3" /*"F"*/) {
                if (!AppTool.IsNullOrEmpty(this._DeclarationPM.ImporterPassportNumber)) {
                    if (this._DeclarationPM.ImporterPassportNumber.length > 15) {
                        this.ValidationErrorMessageCodes.push(TextCodeTranslator.Translate("Customs.Declaration.O.TooLongCode"));
                    }
                }
                return;
            }


            if (this._DeclarationPM.ImporterCode.length < 9) {
                this.ValidationErrorMessageCodes.push("מספר יבואן קצר מידיי");
            }
            else if (this._DeclarationPM.ImporterCode.length > 9) {
                this.ValidationErrorMessageCodes.push(TextCodeTranslator.Translate("Customs.Declaration.O.TooLongCode"));
            }
            else {
                var digit: string = this._DeclarationPM.ImporterCode.toString().substring(8);
                var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(this._DeclarationPM.ImporterCode.substring(0, 8));

                if (digit != checkDigit.toString()) {
                    this.ValidationErrorMessageCodes.push(TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
                }
            }
        
        }
    }
  
    public Validate(entityPM: DeclarationPM) {
   
        var result = [];
        this._DeclarationPM = entityPM;
        this.EmptyConsignmentPackageCheck();
        this.CheckIsImporterCodeValid();

        return this.ValidationErrorMessageCodes;
    }


    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }
}
