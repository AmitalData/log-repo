declare var window: any;
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../Infrastructure/Validators/Validator';

import { ClaimPM } from '../EntityPMs/ClaimPM';
import { SupplierInvoiceItemPM } from '../EntityPMs/SupplierInvoiceItemPM';

export class ClaimValidator {
    private _ClaimPM: ClaimPM;
    private FIELD_IS_REQUIERD: string;
    public ValidationErrorMessageCodes: string[];

    constructor() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public SetEntityPM(claimPM: ClaimPM) {
        this._ClaimPM = claimPM;
    }

    //Check if there is an empty Importer Declarations
    public ClaimImporterDeclarsPage3Check() {
        var errorMessage: string = "";

        if (this._ClaimPM != null) {
            for (let item of this._ClaimPM.ClaimImporterDeclarsPage3) {
                if (AppTool.IsNullOrEmpty(item.ImporterLoiDeclarationTypeCode) || item.ClaimImporterDeclarsP3Loi == null
                    || (item.ClaimImporterDeclarsP3Loi != null && item.ClaimImporterDeclarsP3Loi.length == 0)
                    || (item.ClaimImporterDeclarsP3Loi != null && item.ClaimImporterDeclarsP3Loi.length > 0 && AppTool.IsNullOrEmpty(item.ClaimImporterDeclarsP3Loi[0].DeclarationNumber))) {
                    errorMessage = "לא ניתן להוסיף הצהרת יבואן בלי תצהיר ו/או הצהרה";
                    if (!AppTool.IsNullOrEmpty(errorMessage)) {
                        this.ValidationErrorMessageCodes.push(errorMessage);
                    }
                }
            }
        }
    }

    //Check if there is an empty Importer Declarations Page 3A
    public ClaimImporterDeclarsPage3ACheck() {
        var errorMessage: string = "";

        if (this._ClaimPM != null) {
            if (this._ClaimPM.ClaimImporterDeclarsPage3A != null && this._ClaimPM.ClaimImporterDeclarsPage3A.length > 0) {
                for (let item of this._ClaimPM.ClaimImporterDeclarsPage3A) {
                    if (AppTool.IsNullOrEmpty(item.CommercialSaleTypeCode)) {
                        errorMessage = "לא ניתן להוסיף פרטי מישור מסחרי ריק";
                        if (!AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
        }
    }

    //Check if there is an empty Importer Declarations Page 3B
    public ClaimImporterDeclarsPage3BCheck() {
        var errorMessage: string = "";

        if (this._ClaimPM != null) {
            if (this._ClaimPM.ClaimImporterDeclarsPage3B != null && this._ClaimPM.ClaimImporterDeclarsPage3B.length > 0) {
                for (let item of this._ClaimPM.ClaimImporterDeclarsPage3B) {
                    if (AppTool.IsNullOrEmpty(item.DescriptionOfGoods)) {
                        errorMessage = "לא ניתן להוסיף הצהרת מכירה ריקה";
                        if (!AppTool.IsNullOrEmpty(errorMessage)) {
                            this.ValidationErrorMessageCodes.push(errorMessage);
                        }
                    }
                }
            }
        }
    }

    public SubmitDateTimeCheck() {
        var errorMessage: string = "";

        if (this._ClaimPM != null) {
            if (this._ClaimPM.SubmitDate == null) {
                errorMessage = errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            }
            else{
                //if ((this._ClaimPM.SubmitDate.getFullYear != Date.) ||
                //    (this._ClaimPM.SubmitDate.Value.Date.Month != DateTime.Now.Date.Month) ||
                //    (this._ClaimPM.SubmitDate.Value.Date.Day != DateTime.Now.Date.Day)) {
                //    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                //}
            }

            if (!AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    }

    public Validate(entityPM: ClaimPM) {

        var result = [];
        this._ClaimPM = entityPM;
        this.ClaimImporterDeclarsPage3Check();  //Page3
        this.ClaimImporterDeclarsPage3ACheck(); //Page3-A
        this.ClaimImporterDeclarsPage3BCheck(); //Page3-B

        return this.ValidationErrorMessageCodes;
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }
}
