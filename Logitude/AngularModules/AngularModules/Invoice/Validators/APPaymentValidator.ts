import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {APPaymentPM} from '../EntityPMs/APPaymentPM';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';

export class APPaymentValidator {
    public Validate(entityPm: APPaymentPM) {
        var validationResults = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        Validator.TryValidateObject(entityPm, null, validationResults);

        var isNegativeAmountEnabled: boolean = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetAPPayments && entityPm.PaymentMethodCode == "FS" ? true : false;

        if (entityPm.RegisterDate == null) {
            validationResults.push(msg.replace("%FieldName", "Register Date"));
        }

        else if (DateTool.GetDateParts(entityPm.RegisterDate).DateTicks > DateTool.GetCurrentDateAsUtc().valueOf()) {
            validationResults.push(TextCodeTranslator.Translate("APPayment.M.CantSetFutureDatePayment"));
        }

        if (entityPm.AmountInPaymentCurrency == 0) {
            var isAllowed = false;

            if (entityPm.PaymentMethodCode != null) {
                if (entityPm.PaymentMethodCode.toUpperCase() == "FS") {
                    isAllowed = true;
                }
            }

            if (!isAllowed) {
                validationResults.push(TextCodeTranslator.Translate("APPayment.M.CantSetZeroAmount"));
            }
        }

        if (entityPm.PaymentMethodCode == "CH" && !entityPm.AutomaticPaymentCheque) {
            if (AppTool.IsNullOrEmpty(entityPm.ChequeOrPaymentRef)) {
                validationResults.push(msg.replace("%FieldName", "Cheque Ref"));
            }
        }

        if ((entityPm.PaymentMethodCode == "CH" || entityPm.PaymentMethodCode == "BT" || entityPm.PaymentMethodCode == "CC") && entityPm.ValueDate == null) {
            validationResults.push(msg.replace("%FieldName", "Value Date"));

        }
        if (entityPm.HasInvoicesErrors) {
            validationResults.push(TextCodeTranslator.Translate("APPayment.M.PaymentInvoicesHaveErrors"));
        }

        var AmountPaid = 0;
        var ExternalAmount: number = 0;
        var isNoPaidAmount: boolean;
        entityPm.PaymentInvoices.forEach(item => {

            if (!AppTool.IsNullOrZero(item.PaymentAmount)) {
                AmountPaid += item.PaymentAmount;
            }

            if (AppTool.IsNullOrZero(item.ForeignAmount)) {
                isNoPaidAmount = true;
            }
        });

        AmountPaid = AppTool.Round(AmountPaid, 2);

        if (entityPm.ExternalPaymentAmount) {
            ExternalAmount = AppTool.Round(entityPm.ExternalPaymentAmount, 2);
        }

        if (isNoPaidAmount == true) {
            if (entityPm.PaymentMethodCode.toUpperCase() != "FS") {
                validationResults.push("Can't connect lines with zero Amount to Pay");
            }
        }

        if (isNegativeAmountEnabled == false) {
            if (entityPm.AmountInPaymentCurrency < 0) {
                validationResults.push(TextCodeTranslator.Translate("APPayment.M.CantSetMinusAmount"));
            }

            if (AmountPaid < 0) {
                validationResults.push(TextCodeTranslator.Translate("APPayment.M.PaymentAmountPaidCantBeMinus"));
            }
        }

        if ((AmountPaid + ExternalAmount) > entityPm.AmountInPaymentCurrency) {
            validationResults.push(TextCodeTranslator.Translate("APPayment.M.PaymentAmountPaidCantBeBigger"));
        }

        if (entityPm.PaymentMethodCode == "CC") {
            if (AppTool.IsNullOrEmpty(entityPm.CreditCardTypeId)) {
                validationResults.push("Credit Card Type Field is required");
            }
        }

        return validationResults;
    }
}
