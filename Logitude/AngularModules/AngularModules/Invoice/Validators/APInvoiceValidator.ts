import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool, ArrayTool} from '../../Infrastructure/Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {InvoiceTool} from "../Tools";
import {CurrencyList} from '../../Common/EntityLists/CurrencyList';
import {VatTypeList} from '../../Common/EntityLists/VatTypeList';
import {AccountingSystemList} from '../../Common/EntityLists/AccountingSystemList';
import {ARInvoiceLinePM} from '../EntityPMs/ARInvoiceLinePM';
import {InvoiceTotalsClass} from '../Args';
import {AccountingSystemListService} from '../../Common/Services/StandardLists/AccountingSystemListService';
import {CurrencyListService} from '../../Common/Services/StandardLists/CurrencyListService';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {GroupByPipe} from '../../Infrastructure/Pipes/GroupByPipe';
import {APInvoicePM} from '../EntityPMs/APInvoicePM';
import {VATTypesGroupPM} from '../../Common/EntityPMs/VATTypesGroupPM';
import {VatTypesValidator} from '../../Infrastructure/Validators/VatTypesValidator';
import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';

export class APInvoiceValidator {
    private Errors: string[] = [];
    private EntityPM: APInvoicePM;
    private message: string;
    constructor() {
        this.Errors = [];
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public Validate(entityPM: APInvoicePM) {
        this.Errors = [];
        this.EntityPM = entityPM;

        Validator.TryValidateObject(entityPM, "APInvoice", this.Errors);

        var allVatTypes: VatTypeList[] = VatTypesValidator.GetAllVatTypes();

        if (DateTool.GetDateParts(this.EntityPM.InvoiceDate).DateTicks > DateTool.GetCurrentDateAsUtcForAccountingValidation().valueOf()) {
            this.Errors.push(TextCodeTranslator.Translate("APInvoice.M.CantReceiveFutureDateInvoice"));
        }   

        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (AppTool.IsNullOrEmpty(entityPM.VATNumber)) {
                this.Errors.push(this.message.replace("%FieldName", "Vat Number"));
            }
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
