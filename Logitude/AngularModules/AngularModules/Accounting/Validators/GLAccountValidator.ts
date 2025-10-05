import { GLAccountPM } from '../EntityPMs/GLAccountPM';
import { AppTool } from '../../Infrastructure/Tools';
import {GLAccountExtendedListService} from '../Services/ExtendedLists/GLAccountExtendedListService';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';

export class GLAccountValidator {
    private static CurrentSession = SessionLocator.SelectedSession;
    public static ValidateGLAccount(entityPM: any) {


        return [];
    }

    

    public static ValidateWithHoldingTaxLines(entityPM: GLAccountPM) {
        var FIELD_IS_REQUIERD: string = null;
        FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        for (var line of entityPM.GLAccountWithholdingTaxes) {
            if (line.FromDate == undefined) {
                var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccountWithholdingTax.F.FromDate"));
                errors.push(s);
            }
            if (line.ToDate == undefined) {
                var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccountWithholdingTax.F.ToDate"));
                errors.push(s);
            }
            if (line.Percentage == undefined) {
                var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccountWithholdingTax.F.Percentage"));
                errors.push(s);
            }
        }

            // this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            return errors;

    }
    public static ValidateCurrency(entityPM: GLAccountPM, oldCurrency: string) {

        //
        // [!] THE VALIDATION MOVED TO SERVER
        //


        //var errors = [];

        //var _GLAccountExtendedListService = new GLAccountExtendedListService();

        //if (entityPM.AccountTypeCode == "2") { // Customer

        //    if (oldCurrency != entityPM.CurrencyId) {

        //        // Customer: LedgerTransactions check
        //        _GLAccountExtendedListService.CheckIfHasLedgerTransactions(entityPM.Id).subscribe((exist) => {
        //            if (!AppTool.IsNullOrEmpty(exist)) {
        //                if (exist) {
        //                    errors.push(TextCodeTranslator.Translate("Accounting.General.O.ThereTransactions4GLAwithexistingCurrency"));
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = false;
        //                    return errors;

        //                } else {
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //                }
        //            }
        //        });

        //    } else {
        //        this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //    }
        //} else {
        //    this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //}

    }


    public Validate(entityPM: GLAccountPM) {
        var errors = [];
        var result = [];
        if (entityPM.Id != undefined) {
            errors = GLAccountValidator.ValidateWithHoldingTaxLines(entityPM);
        }


        return errors;
    }

}

