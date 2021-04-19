import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {ARPaymentPM} from '../EntityPMs/ARPaymentPM';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class ARPaymentValidator {
  public Validate(entityPm: ARPaymentPM) {

    var validationResults = [];

    var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    
    Validator.TryValidateObject(entityPm, null, validationResults);



    var isNegativeAmountEnabled: boolean = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments && entityPm.AccountingPaymentMethodCode == "FS" ? true : false;

    if (entityPm.RegisterDate == null) {
      validationResults.push(msg.replace("%FieldName", "Register Date"));
    }

    else if (DateTool.GetDateParts(entityPm.RegisterDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation().valueOf()) {
      validationResults.push(TextCodeTranslator.Translate("ARPayment.M.CantSetFutureDatePayment"));
    }

    if (entityPm.AmountInPaymentCurrency == 0) {
      var isAllowed = false;

      if (entityPm.AccountingPaymentMethodCode != null) {
        if (entityPm.AccountingPaymentMethodCode.toUpperCase() == "FS") {

          isAllowed = true;

          //if (entityPm.PaymentInvoices.length == 0) {
          //    validationResults.push("You should have 1 Invoice line at least");
          //}

          //else {
          //    isAllowed = true;
          //}
        }
      }

      if (!isAllowed) {
        validationResults.push(TextCodeTranslator.Translate("ARPayment.M.CantSetZeroAmount"));
      }
    }

      if (entityPm.AccountingPaymentMethodCode == "CH") {
          this.ValidatePaymentChequeFields(entityPm, validationResults,msg);
      
    }



    if (entityPm.HasInvoicesErrors) {
      validationResults.push(TextCodeTranslator.Translate("ARPayment.M.PaymentInvoicesHaveErrors"));
    }

    var myLinesPaidAmount = 0;
    var isNoPaidAmount: boolean;
    entityPm.PaymentInvoices.forEach(item => {
      myLinesPaidAmount += item.PaymentAmount;

      if (AppTool.IsNullOrZero(item.ForeignAmount)) {
        isNoPaidAmount = true;
      }
    });

    if (isNoPaidAmount == true) {
      validationResults.push("Can't connect lines with zero Amount to Pay");
    }

    var myLinesPaidAmountRounded = AppTool.Round(myLinesPaidAmount, 2);

    if (isNegativeAmountEnabled == false) {
      if (entityPm.AmountInPaymentCurrency < 0) {
        validationResults.push(TextCodeTranslator.Translate("ARPayment.M.CantSetMinusAmount"));
      }

      if (myLinesPaidAmountRounded < 0) {
        validationResults.push(TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeMinus"));
      }
    }

    if (myLinesPaidAmountRounded > entityPm.AmountInPaymentCurrency) {
      validationResults.push(TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeBigger"));
    }

    if (entityPm.AccountingPaymentMethodCode == "CC") {
      if (AppTool.IsNullOrEmpty(entityPm.CreditCardTypeId)) {
        validationResults.push("Credit Card Type Field is required");
      }
    }

    if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
      if (AppTool.IsNullOrEmpty(entityPm.MetodoPagoCode)) {
        validationResults.push("Metodo Pago Field is Required");
      }

      if (AppTool.IsNullOrEmpty(entityPm.SATPaymentMethodCode)) {
        validationResults.push("Forma Pago Field is Required");
      }

      if (entityPm.SATPaymentMethodCode == "99") {
        validationResults.push("Forma Pago value can't be 'Por Definir'.Please choose another value.");
      }


      if (!AppTool.IsNullOrEmpty(entityPm.TipoCadenaPago) && entityPm.TipoCadenaPago == "01" && entityPm.SATPaymentMethodCode == "03") {
        if (AppTool.IsNullOrEmpty(entityPm.CertPago))
          validationResults.push(msg.replace("%FieldName", "Cert Pago"));

        if (AppTool.IsNullOrEmpty(entityPm.CadPago))
          validationResults.push(msg.replace("%FieldName", "Cad Pago"));

        if (AppTool.IsNullOrEmpty(entityPm.SelloPago))
          validationResults.push(msg.replace("%FieldName", "Sello Pago"));
      }

    }

    if (!SessionLocator.AccountingSettingPM.AllowManualARPaymentNumber) {
      if (entityPm.IsPaymentNumberManuallySet) {
        if (AppTool.IsNullOrEmpty(entityPm.StatusCode) || entityPm.StatusCode == "DR") {
          validationResults.push("Accounting Settings don't allow manual payment number");
        }
      }
    }


    if (!SessionLocator.TenantPM.AccountingActivated && entityPm.AccountingPaymentMethodCode != "CA" && entityPm.AccountingPaymentMethodCode != "FS" && entityPm.ValueDate == null) {
      validationResults.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.ValueDate")));
    }

    return validationResults;
  }

    private  ValidatePaymentChequeFields(entityPm: ARPaymentPM, validationResults: any[], msg:string) {
        if (AppTool.IsNullOrEmpty(entityPm.ChequeOrPaymentRef)) {
            validationResults.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef")));
        }
        if (AppTool.IsNullOrEmpty(entityPm.Bank)) {
            validationResults.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.Bank")));
        }
        if (AppTool.IsNullOrEmpty(entityPm.Account)) {
            validationResults.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.Account")));
        }
        if (AppTool.IsNullOrEmpty(entityPm.BankBranch)) {
            validationResults.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.BankBranch")));
        }
    }
    public static ValidateCurrenctEntity(entityPm: ARPaymentPM) {
        var errors = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        Validator.TryValidateObject(entityPm, null, errors);

        var isNegativeAmountEnabled: boolean = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments && entityPm.AccountingPaymentMethodCode == "FS" ? true : false;

        if (entityPm.RegisterDate == null) {
            errors.push(msg.replace("%FieldName", "Register Date"));
        }

        else if (DateTool.GetDateParts(entityPm.RegisterDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation().valueOf()) {
            errors.push(TextCodeTranslator.Translate("ARPayment.M.CantSetFutureDatePayment"));
        }

        if (entityPm.AmountInPaymentCurrency == 0) {
            var isAllowed = false;

            if (entityPm.AccountingPaymentMethodCode != null) {
                if (entityPm.AccountingPaymentMethodCode.toUpperCase() == "FS") {

                    isAllowed = true;

                    //if (entityPm.PaymentInvoices.length == 0) {
                    //    errors.push("You should have 1 Invoice line at least");
                    //}

                    //else {
                    //    isAllowed = true;
                    //}
                }
            }

            if (!isAllowed) {
                errors.push(TextCodeTranslator.Translate("ARPayment.M.CantSetZeroAmount"));
            }
        }

        if (entityPm.AccountingPaymentMethodCode == "CH") {
            if (AppTool.IsNullOrEmpty(entityPm.ChequeOrPaymentRef)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef")));
            }
        }

        if (entityPm.HasInvoicesErrors) {
            errors.push(TextCodeTranslator.Translate("ARPayment.M.PaymentInvoicesHaveErrors"));
        }

        var result = 0;
        var isNoPaidAmount: boolean;
        entityPm.PaymentInvoices.forEach(item => {
            result += item.PaymentAmount;

            if (AppTool.IsNullOrZero(item.ForeignAmount)) {
                isNoPaidAmount = true;
            }
        });

        if (isNoPaidAmount == true) {
            errors.push("Can't connect lines with zero Amount to Pay");
        }

        var paymentAmountPaid = AppTool.Round(result, 2);

        if (isNegativeAmountEnabled == false) {
            if (entityPm.AmountInPaymentCurrency < 0) {
                errors.push(TextCodeTranslator.Translate("ARPayment.M.CantSetMinusAmount"));
            }

            if (paymentAmountPaid < 0) {
                errors.push(TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeMinus"));
            }
        }

        if (paymentAmountPaid > entityPm.AmountInPaymentCurrency) {
            errors.push(TextCodeTranslator.Translate("ARPayment.M.PaymentAmountPaidCantBeBigger"));
        }

        if (entityPm.AccountingPaymentMethodCode == "CC") {
            if (AppTool.IsNullOrEmpty(entityPm.CreditCardTypeId)) {
                errors.push("Credit Card Type Field is required");
            }
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(entityPm.MetodoPagoCode)) {
                errors.push("Metodo Pago Field is Required");
            }

            if (AppTool.IsNullOrEmpty(entityPm.SATPaymentMethodCode)) {
                errors.push("Forma Pago Field is Required");
            }

            if (entityPm.SATPaymentMethodCode == "99") {
                errors.push("Forma Pago value can't be 'Por Definir'.Please choose another value.");
            }


            if (!AppTool.IsNullOrEmpty(entityPm.TipoCadenaPago) && entityPm.TipoCadenaPago == "01" && entityPm.SATPaymentMethodCode == "03") {
                if (AppTool.IsNullOrEmpty(entityPm.CertPago))
                    errors.push(msg.replace("%FieldName", "Cert Pago"));

                if (AppTool.IsNullOrEmpty(entityPm.CadPago))
                    errors.push(msg.replace("%FieldName", "Cad Pago"));

                if (AppTool.IsNullOrEmpty(entityPm.SelloPago))
                    errors.push(msg.replace("%FieldName", "Sello Pago"));
            }

        }

        if (!SessionLocator.AccountingSettingPM.AllowManualARPaymentNumber) {
            if (entityPm.IsPaymentNumberManuallySet) {
                if (AppTool.IsNullOrEmpty(entityPm.StatusCode) || entityPm.StatusCode == "DR") {
                    errors.push("Accounting Settings don't allow manual payment number");
                }
            }
        }

        var isValid = true;
        if (errors != null && errors.length > 0) {
            isValid = false;
        }

        return errors;
    }
}
