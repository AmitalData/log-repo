import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {InvoiceTool} from "../Tools";
import {ARInvoicePM} from '../EntityPMs/ARInvoicePM';
//import {InvoiceTotalsClass} from '../Args';
import {VatTypeList} from '../../Common/EntityLists/VatTypeList';
import {VatTypesValidator} from '../../Infrastructure/Validators/VatTypesValidator';
import { EntityListService } from '../../Infrastructure/Services/EntityListService';
import { FullAccountingSettingPM } from '../../Accounting/EntityPMs/FullAccountingSettingPM';

export class ARInvoiceValidator {
    private Errors: string[] = [];
    private EntityPM: ARInvoicePM;
    private message: string;
    entityListService: EntityListService = new EntityListService();
    private FullAccountingSetting: FullAccountingSettingPM = new FullAccountingSettingPM();

    constructor() {
        this.Errors = [];
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    Validate(entity: ARInvoicePM) {
        this.Errors = [];
        this.EntityPM = entity;

        Validator.TryValidateObject(this.EntityPM, "ARInvoice", this.Errors);

        if (this.EntityPM.IsInvoiceNumberManuallySet && AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber)) {
            this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.YouShouldSetInvoiceNumber"));
        }

        if (this.EntityPM.IsInvoiceNumberFromStock && AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceStockId)) {
            this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.YouShouldSetInvoiceNumber"));
        }

        if (this.EntityPM.IsInvoiceNumberFromStock && AppTool.IsNullOrEmpty(this.EntityPM.InvoiceNumber) && this.EntityPM.IsAutoCredit) {
            this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.YouShouldSetInvoiceNumber"));
        }
        var date1 = new Date(this.EntityPM.InvoiceDate.toString());
        var date2 = new Date();
        date2.setHours(23);
        date2.setMinutes(59);
        
        if (date1.valueOf() > date2.valueOf()) {
            this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.CantIssueInvoiceWithFutureDate"));
        }
        if (SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAR) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                if (AppTool.IsNullOrEmpty(this.EntityPM.VatNumber)) {
                    this.Errors.push(this.message.replace("%FieldName", "Vat Number"));
                }
            }
        }

        if (!SessionLocator.AccountingSettingPM.AllowManualInvoiceNumber) {
            if (this.EntityPM.IsInvoiceNumberManuallySet) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                    this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.ManualInvoiceNumberNotAllowed"));
                }
            }
        }

        if (this.EntityPM.IsConsolidationInvoice) {
            this.ValidateConsolidationInvoice();
        }

        else {
            this.ValidateNormalInvoice()
        }        

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(this.EntityPM.SATPaymentMethodCode)) {
                this.Errors.push("Forma Pago Field is Required");
          }

          if (AppTool.IsNullOrEmpty(this.EntityPM.MetodoPagoCode)) {
            this.Errors.push("Metodo Pago Field is Required");
          }

          if (this.EntityPM.MetodoPagoCode == "PUE" && this.EntityPM.SATPaymentMethodCode == "99") {
            this.Errors.push("Since the metodo pago was set as PUE, you can't select Por definir (99). Please choose another value for the forma Pago.");
          }
        }

        return this.Errors;
    }
    private ValidateNormalInvoice() {
        if (this.EntityPM.InvoiceLines.length == 0) {
            this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast"));
        }

        else {

            var allVatTypes: VatTypeList[] = VatTypesValidator.GetAllVatTypes();

            this.EntityPM.InvoiceLines.forEach(item => {

                Validator.TryValidateObject(item, "ARInvoiceLine", this.Errors);

                if (item.VatTypeId == null) {
                    var field = TextCodeTranslator.Translate("ARInvoiceLine.F.VatTypeId");
                    this.Errors.push(this.message.replace("%FieldName", field));
                }

                else {
                    if (item.VatPercentage == null) {
                        var vattType = allVatTypes.filter(d => d.Id == item.VatTypeId)[0];
                        if (vattType != null) {
                            if (!vattType.IsMultiPercentage) {
                                var field = TextCodeTranslator.Translate("ARInvoiceLine.F.VatPercentage");
                                this.Errors.push(this.message.replace("%FieldName", field));
                            }
                        }
                    }
                }
            });


            var lineGrouped: LineCurrency[] = [];
            this.EntityPM.InvoiceLines.forEach(item => {
                var lineItem: LineCurrency = lineGrouped.filter(f => f.Id == item.ForiegnCurrencyId)[0];

                if (lineItem == null) {
                    lineGrouped.push(new LineCurrency(item.ForiegnCurrencyId, item.ForiegnCurrencyCode, item.ForiegnExchangeRate));
                }

                else if (lineItem.Rate != item.ForiegnExchangeRate) {
                    if (!lineItem.Validated) {
                        lineItem.Validated = true;
                        if (!SessionLocator.TenantPM.AccountingActivated) {
                            this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.InvoiceLinesHaveDifferentExchangeRates").replace("%CurrencyCode", lineItem.Code));

                        }

                    }
                }
            });

            //var listGrouped: InvoiceTotalsClass[] = [];
            //this.EntityPM.InvoiceLines.filter(f => f.VatTypeId != null).forEach(item => {

            //    var localAmount = item.VatPercentage * item.LocalCurrencyAmount / 100;
            //    var invoiceAmount = item.VatPercentage * item.InvoiceCurrencyAmount / 100;
            //    if (AppTool.IsNullOrEmpty(localAmount)) {
            //        localAmount = 0;
            //    }
            //    if (AppTool.IsNullOrEmpty(invoiceAmount)) {
            //        invoiceAmount = 0;
            //    }

            //    var itemGrouped: InvoiceTotalsClass = listGrouped.filter(f => f.VatTypeId == item.VatTypeId)[0];
            //    if (itemGrouped == null) {
            //        itemGrouped = new InvoiceTotalsClass();
            //        itemGrouped.VatTypeId = item.VatTypeId;
            //        itemGrouped.RowLabel = item.VatTypeName + " (" + item.VatPercentage + "%)";
            //        itemGrouped.LocalCurrencyAmount = localAmount;
            //        itemGrouped.InvoiceCurrencyAmount = invoiceAmount;
            //        listGrouped.push(itemGrouped);
            //    }

            //    else {
            //        itemGrouped.LocalCurrencyAmount += localAmount;
            //        itemGrouped.InvoiceCurrencyAmount += invoiceAmount;
            //    }
            //});

            if (this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                if (!this.EntityPM.IsAutoCredit) {
                    if (this.EntityPM.SubTotalInInvoiceCurrency > 0) {
                        this.Errors.push("Subtotal amount can't be positive");
                    }

                    if (this.EntityPM.AmountInInvoiceCurrency > 0) {
                        this.Errors.push("Invoice amount can't be positive");
                    }

                    //this.EntityPM.TotalVATs.filter(f => f.InvoiceCurrencyVATAmount > 0).forEach(item => {
                    //    this.Errors.push(item.VatTypeCell + " amount can't be positive");
                    //});

                    //listGrouped.filter(f => f.InvoiceCurrencyAmount > 0).forEach(item => {
                    //    this.Errors.push(item.RowLabel + " amount can't be positive");
                    //});
                }
            }
            else {
                if (this.EntityPM.SubTotalInInvoiceCurrency < 0) {
                    this.Errors.push("Subtotal amount can't be minus");
                }

                if (this.EntityPM.AmountInInvoiceCurrency < 0) {
                    this.Errors.push("Invoice amount can't be minus");
                }
            }

            this.ValidateSingleTaxPerInvoice();
        }
    }
    //private ValidateMultipleExchangeRates(lineItem:LineCurrency) {
    //    this.entityListService.getSingle(this.EntityPM.Tenant.toString(), "FullAccountingSetting").then((res: any) => {
    //        res.subscribe(myResponse => {
    //            if (myResponse != null) {
    //                var res = myResponse.Result;
    //                this.FullAccountingSetting = res;
    //                if (!this.FullAccountingSetting.AllowMultiRatesInInvoiceLines)
    //                    this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.InvoiceLinesHaveDifferentExchangeRates").replace("%CurrencyCode", lineItem.Code));
    //            }

    //        })
    //    });               



    //}
    private ValidateConsolidationInvoice() {

        if (!AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {

            //if (!this.EntityPM.IsBillToAllowConsolidation) {
            //    this.Errors.push(InvoiceTool.GetBillToNotAllowConsolidation());
            //}
        }

        if (this.EntityPM.StatusCode == "AC" || this.EntityPM.StatusCode == "AR") {
            if (this.EntityPM.InvoiceLines.length == 0) {
                this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast"));
            }
        }

        else if (this.EntityPM.ConstituentInvoices.length == 0) {
            this.Errors.push(TextCodeTranslator.Translate("ARInvoice.M.YouShouldHaveOneLineAtLeast"));
        }

        else {
            if (this.EntityPM.ARInvoiceTypeCode == "CD" || this.EntityPM.ARInvoiceTypeCode == "CC") {
                if (this.EntityPM.SubTotalInInvoiceCurrency > 0) {
                    this.Errors.push("Subtotal amount can't be positive");
                }

                if (this.EntityPM.AmountInInvoiceCurrency > 0) {
                    this.Errors.push("Invoice amount can't be positive");
                }
            }

            else {
                if (this.EntityPM.SubTotalInInvoiceCurrency < 0) {
                    this.Errors.push("Subtotal amount can't be minus");
                }

                if (this.EntityPM.AmountInInvoiceCurrency < 0) {
                    this.Errors.push("Invoice amount can't be minus");
                }
            }
        }
    }
    private ValidateSingleTaxPerInvoice() {

        var isValidating = false;
        if (SessionLocator.AccountingSettingPM.IsSingleTaxPerInvoice) {
            isValidating = true;
        }

        else if (SessionLocator.AccountingSystemPM) {
            if (SessionLocator.AccountingSystemPM.IsSingleTaxPerInvoice) {
                isValidating = true;
            }
        }

        if (isValidating) {
            var myVatTypeId: string = null;
            var myVatPercentage: number = 0;

            this.EntityPM.InvoiceLines.forEach(item => {
                if (item.VatPercentage != 0) {
                    if (myVatPercentage != 0 && item.VatPercentage != myVatPercentage) {
                        this.Errors.push("Only one vat percantage per invoice is allowed!");
                    }
                }

                else if (item.VatPercentage == 0) {
                    if (myVatTypeId != null && item.VatTypeId != myVatTypeId) {
                        this.Errors.push("Only one vat percantage per invoice is allowed!");
                    }

                    else {
                        myVatTypeId = item.VatTypeId;
                    }
                }

                myVatPercentage = item.VatPercentage;
            });
        }
    }
}
class LineCurrency {
    public Id: string;
    public Code: string;
    public Rate: number;
    public Validated: boolean = false;
    constructor(id: string, code: string, rate: number) {
        this.Id = id;
        this.Code = code;
        this.Rate = rate;
    }
}
