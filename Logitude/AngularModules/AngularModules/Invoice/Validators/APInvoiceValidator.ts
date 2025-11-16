import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool, ArrayTool } from '../../Infrastructure/Tools';
import { Validator } from '../../Infrastructure/Validators/Validator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { VatTypeList } from '../../Common/EntityLists/VatTypeList';
import { APInvoicePM } from '../EntityPMs/APInvoicePM';
import { VatTypesValidator } from '../../Infrastructure/Validators/VatTypesValidator';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';

export class APInvoiceValidator {
    private Errors: string[] = [];
    private EntityPM: APInvoicePM;
    private message: string;
    private accountingActivated: boolean = false;
    constructor() {
        this.Errors = [];
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.accountingActivated = SessionLocator.TenantPM.AccountingActivated;

    }

    public Validate(entityPM: APInvoicePM) {
        this.Errors = [];
        this.EntityPM = entityPM;

        Validator.TryValidateObject(entityPM, "APInvoice", this.Errors);

        var allVatTypes: VatTypeList[] = VatTypesValidator.GetAllVatTypes();

        if (DateTool.GetDateParts(this.EntityPM.InvoiceDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation(SessionLocator.TenantPM.TimeZoneOffset).valueOf()) {
            this.Errors.push(TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ConfirmationNumber) && (this.EntityPM.ConfirmationNumber.length < 9 || this.EntityPM.ConfirmationNumber.length > 30)) {
            this.Errors.push(TextCodeTranslator.Translate("APInvoice.O.ConfirmationNumberLength"));
        }
        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(entityPM.VATNumber) && (this.EntityPM.VendorCountry === "IL"|| this.EntityPM.VendorCountry === null)) {
                this.Errors.push(this.message.replace("%FieldName", "Vat Number"));
            }
        }
        if(this.EntityPM.IsPrepaidExpenses && !this.EntityPM.HasExpenseAllocationSetting && this.EntityPM.SetApproved){
            this.Errors.push(TextCodeTranslator.Translate("APInvoice.O.ExpenseAllocationSettingIsRequiredForPrepaidExpenses"));
        }
        if(this.EntityPM.ExpenseAllocationStartDate != null && this.EntityPM.ExpenseAllocationStartDate < this.EntityPM.AccountingDate && this.EntityPM.IsPrepaidExpenses  && this.EntityPM.SetApproved){
            this.Errors.push(TextCodeTranslator.Translate("APInvoice.O.ExpenseAllocationSettingStartDateError"));
        }
        if (entityPM.IsMultipleEntities) {

        }

        else {
            if (entityPM.InvoiceLines.length == 0) {
                this.Errors.push(TextCodeTranslator.Translate("APInvoice.M.YouShouldHaveOneLineAtLeast"));
            }

            if (entityPM.InvoiceExpectedAmount == null) {
                this.Errors.push(this.message.replace("%FieldName", "Invoice Amount"));
            }


            if (this.accountingActivated && entityPM.AccountingDate == null) {
                this.Errors.push(this.message.replace("%FieldName", "Accounting Date"));
            }

            else {
                entityPM.InvoiceLines.forEach(item => {

                    Validator.TryValidateObject(item, "APInvoiceLine", this.Errors);

                    if (item.InvoiceCurrencyAmount == 0) {
                        this.Errors.push(TextCodeTranslator.Translate("APInvoice.M.InvoiceLineAmountNotZero"));
                    }

                    if (!entityPM.TotalVATOnly) {
                        if (item.VatTypeId == null) {
                            var field = TextCodeTranslator.Translate("APInvoiceLine.F.VatTypeId");
                            this.Errors.push(this.message.replace("%FieldName", field));
                        }

                        else {
                            if (item.VatPercentage == null) {
                                var vattType = allVatTypes.filter(d => d.Id == item.VatTypeId)[0];
                                if (vattType != null) {
                                    if (!vattType.IsMultiPercentage) {
                                        var field = TextCodeTranslator.Translate("APInvoiceLine.F.VatPercentage");
                                        this.Errors.push(this.message.replace("%FieldName", field));
                                    }
                                }
                            }
                        }
                    }
                });

                if (this.EntityPM.AmountInInvoiceCurrency != this.EntityPM.AmountInInvoiceCurrency_Summary) {
                    this.Errors.push(TextCodeTranslator.Translate("APInvoice.M.InvoiceAmountNotMatched"));
                }
                if(this.EntityPM.IsPrepaidExpenses){
                  
                   if (!this.EntityPM.InvoiceLines.some(line => line.IsPrepaidExpenses)) {
                      this.Errors.push(TextCodeTranslator.Translate("APInvoice.O.PrepaidExpensesLineRequired"));
                   }
                }
            }
        }
        this.CheckSpecialCharacters();

        return this.Errors;
    }
    CheckSpecialCharacters() {
        if (FeatureLocator.HasFeaturePermession("APInvoice", "INSC")) {
            var invoiceNumber_Check = /^[A-Za-z0-9]+$/i;
            if (!invoiceNumber_Check.test(this.EntityPM.InvoiceNumber)) {

                this.Errors.push(TextCodeTranslator.Translate("APInvoice.O.ValidateInvoiceNumber"));
            }
        }
    }
}
