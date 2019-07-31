import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ARPaymentInvoiceArgs} from './ARPaymentDetailsTabComponent';
import {ARInvoicePaymentPM} from '../../../../Invoice/EntityPMs/ARInvoicePaymentPM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './EditMultiCurrency.html',
})

export class EditMultiCurrency extends BaseComponent {
    public EntityPM: ARInvoicePaymentPM = null;
    public LocalCurrencyId: string;
    public DataContext = this;
    public ObjectTableName: string = "ARInvoicePayment";
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public PaymentCurrencyId: string = null;
    public PaymentCurrencyCode: string = null;
    public PaymentExchangeRate: number = null;
    public InvoiceCurrencyId: string = null;
    public InvoiceCurrencyCode: string = null;
    public InvoiceExchangeRate: number = null;
    private lineComponent: ARPaymentInvoiceArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.LocalCurrencyId = SessionLocator.LocalCurrencyId;
    }

    SetWindowArgs(myParam: ARPaymentInvoiceArgs) {
        this.lineComponent = myParam;
        this.PaymentCurrencyId = this.lineComponent.PaymentPM.PaymentCurrencyId;
        this.PaymentCurrencyCode = this.lineComponent.PaymentPM.PaymentCurrencyCode;
        this.PaymentExchangeRate = this.lineComponent.PaymentPM.PaymentCurrencyExchangeRate;
        this.InvoiceCurrencyId = this.lineComponent.Invoice.InvoiceCurrencyId;
        this.InvoiceCurrencyCode = this.lineComponent.Invoice.InvoiceCurrencyCode;
        this.InvoiceExchangeRate = this.lineComponent.Invoice.InvoiceCurrencyExchangeRate;

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {

            var myForeignAmount: number = null;
            var myPaymentAmount: number = null;
            var myExchangeRate: number = null;

            if (this.lineComponent.EntityPM) {
                myForeignAmount = this.lineComponent.EntityPM.ForeignAmount;
                myPaymentAmount = this.lineComponent.EntityPM.PaymentAmount;
                myExchangeRate = this.lineComponent.EntityPM.ExchangeRate;
            }

            else {
                myForeignAmount = this.lineComponent.AmountDue;
                myPaymentAmount = this.lineComponent.GetConnectedAmount_PAY(myForeignAmount);
                myExchangeRate = this.lineComponent.ExchangeRate;
            }

            if (!AppTool.IsNullOrZero(this.lineComponent.AmountPaid)) {
                if (myForeignAmount != this.lineComponent.AmountPaid) {
                    myForeignAmount = this.lineComponent.AmountPaid;
                    myPaymentAmount = this.lineComponent.GetConnectedAmount_PAY(myForeignAmount);

                    if (this.lineComponent.IsConnected) {
                        myPaymentAmount = this.lineComponent.ConnectedAmount_PAY;
                    }
                }
            }

            this.EntityPM = new ARInvoicePaymentPM(null);
            this.EntityPM.ForeignAmount = myForeignAmount;
            this.EntityPM.PaymentAmount = myPaymentAmount;
            this.EntityPM.ExchangeRate = myExchangeRate;

            this.IsResourcesReady = true;
        });
    }

    get PaymentAmount() { return this.EntityPM.PaymentAmount; }
    set PaymentAmount(value: number) {
        if (this.EntityPM.PaymentAmount != value) {
            this.EntityPM.PaymentAmount = AppTool.Round(value, 2);
            this.CalculateForeignAmount();
        }
    }

    get ForeignAmount() { return this.EntityPM.ForeignAmount; }
    set ForeignAmount(value: number) {
        if (this.EntityPM.ForeignAmount != value) {
            this.EntityPM.ForeignAmount = AppTool.Round(value, 2);

            if (AppTool.IsNullOrZero(value)) {
                this.EntityPM.PaymentAmount = value;
            }

            else {
                this.EntityPM.PaymentAmount = this.GetConnectedAmount_PAY(value);
            }

            this.ValidateForeignAmount();
        }
    }

    get ExchangeRate() { return this.EntityPM.ExchangeRate; }
    set ExchangeRate(value: number) {
        if (this.EntityPM.ExchangeRate != value) {
            this.EntityPM.ExchangeRate = AppTool.Round(value, 5);
            this.CalculateForeignAmount();

            this.UIProperties.SetRequired("ExchangeRate", this.ObjectTableName, AppTool.IsNullOrZero(value) ? true : false);
        }
    }

    CalculateForeignAmount() {
        var myResult = null;

        if (this.PaymentAmount == 0) {
            myResult = 0;
        }

        else if (AppTool.IsNullOrEmpty(this.PaymentAmount) || AppTool.IsNullOrEmpty(this.ExchangeRate)) {
            myResult = null;
        }

        else if (this.InvoiceCurrencyId == this.LocalCurrencyId) {
            myResult = this.PaymentAmount * this.ExchangeRate;
        }

        else {
            myResult = this.PaymentAmount / this.ExchangeRate;
        }

        this.EntityPM.ForeignAmount = AppTool.Round(myResult, 2);
        this.ValidateForeignAmount();
    }

    GetConnectedAmount_PAY(invoiceAmount: number) {
        var myResult: number = 0;

        if (!AppTool.IsNullOrZero(invoiceAmount)) {
            if (this.InvoiceCurrencyId != this.PaymentCurrencyId) {
                if (this.InvoiceCurrencyId == this.LocalCurrencyId) {
                    myResult = invoiceAmount / this.ExchangeRate;
                }

                else {
                    myResult = invoiceAmount * this.ExchangeRate;
                }
            }

            else {
                myResult = invoiceAmount;
            }
        }

        return myResult;
    }

    ForeignAmountIsValid: boolean = true;
    ForeignAmountValidation: string = null;
    ValidateForeignAmount() {
        var isValid: boolean = true;
        var isValidMessage: string = null;

        var inputEntry = this.ForeignAmount == null ? 0 : AppTool.Round(this.ForeignAmount, 2);
        var allowedAmount = AppTool.Round(this.lineComponent.InvoiceAmount - this.lineComponent.OtherPaymentsAmount, 2);

        if (this.lineComponent.Invoice.ARInvoiceTypeCode == "CD" || this.lineComponent.Invoice.ARInvoiceTypeCode == "CC") {
            if (inputEntry > 0) {
                isValid = false;
                isValidMessage = TextCodeTranslator.Translate("ARPayment.M.OnlyMinusValue");
            }

            else if (inputEntry < allowedAmount) {
                isValid = false;
                isValidMessage = TextCodeTranslator.Translate("ARPayment.M.AmountPaidNotLess") + " " + allowedAmount + " " + this.InvoiceCurrencyCode;
            }
        }

        else {
            if (inputEntry < 0) {
                isValid = false;
                isValidMessage = TextCodeTranslator.Translate("ARPayment.M.CantPayMinusValue");
            }

            else if (inputEntry > allowedAmount) {
                isValid = false;
                isValidMessage = TextCodeTranslator.Translate("ARPayment.M.AmountPaidLessOrEqual") + " " + allowedAmount + " " + this.InvoiceCurrencyCode;
            }
        }

        this.ForeignAmountIsValid = isValid;
        this.ForeignAmountValidation = isValidMessage;
        this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, isValid, isValidMessage);
    }

    CancelButtonClicked() {        
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        this.ValidateForeignAmount();
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrZero(this.ExchangeRate)) {
            var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.ExchangeRate");
            errors.push(msg.replace("%FieldName", field));
        }

        if (!this.ForeignAmountIsValid) {
            errors.push(this.ForeignAmountValidation);
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    }
}
