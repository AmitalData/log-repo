import {Component, OnInit} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ARInvoiceLinePM} from '../../../../Invoice/EntityPMs/ARInvoiceLinePM';
import {ARInvoiceLineItem} from './ARInvoiceDetailsTabGeneral';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {VatTypesValidator} from '../../../../Infrastructure/Validators/VatTypesValidator';
import { ColumnsWidths } from 'Infrastructure/Components/LogitudeComponents/LogLovV2Component';

@Component({

    templateUrl: './AddEditARGeneralInvoiceLineComponent.html',
})

export class AddEditARGeneralInvoiceLineComponent implements OnInit{
    public EntityPM: ARInvoiceLinePM = null;
    public ObjectTableName = "ARInvoiceLine";
    public DataContext: ARInvoiceLineItem;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    ColumnsWidths: ColumnsWidths[] = [];
    IsAccountingActivated: boolean = false;

    constructor() {
        if (SessionLocator.TenantPM.AccountingActivated) {
            this.FillChargesTypesCustomLOVColumnsWidths();
        }
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
    }
    ngOnInit(): void {
        this.SetDefaultValues(); 
    }

    SetDefaultValues() {
        this.Quantity = this.EntityPM.Quantity != null ? this.EntityPM.Quantity : 1;
            this.ForiegnCurrencyId = this.EntityPM.ForiegnCurrencyId?.length != 0 ? this.EntityPM.ForiegnCurrencyId : 
            ( this.IsAccountingActivated ? this.EntityPM.InvoiceCurrencyId : SessionLocator.TenantPM.CurrencyId );

    }

    FillChargesTypesCustomLOVColumnsWidths()
    {
        this.ColumnsWidths = [
            { ColumnName: 'Code', Width: 80 },
            { ColumnName: 'EnglishName', Width: 200 },
            { ColumnName: 'LocalName', Width: 200 },
            { ColumnName: 'MeasurementShortName', Width: 80 },
            { ColumnName: 'ChargesGroupName', Width: 80 },
            { ColumnName: 'VatTypeName', Width: 80 }
        ];
    }

    SetDataContext(dataContext: ARInvoiceLineItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.SetLabels();
        this.Clone();
        this.BuildQueryFilters();
    }

    public ChargeTypesQueryFilters: ApiQueryFilters;
    private BuildQueryFilters() {
        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsReceivable", true, null, null, "Equals", false, false, false, "boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("ReceivableCreditGLAccountId", true, null, null, "IsNotNull", false, false, false, "Text");
    }

  //  public AmountForiegnLabel: string = null;
    public AmountLocalLabel: string = null;
    public AmountInvoiceLabel: string = null;
    SetLabels() {
     //   this.AmountForiegnLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.ForiegnCurrencyAmount").replace("%ForiegnCurrencyCode", this.DataContext.ForiegnCurrencyCode);
        this.AmountLocalLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.LocalCurrencyAmount").replace("%LocalCurrencyCode", SessionLocator.LocalCurrencyCode);
        this.AmountInvoiceLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.InvoiceCurrencyAmount").replace("%InvoiceCurrencyCode", this.DataContext.InvoiceCurrencyCode);
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = newValue;
        }
    }
    get ForiegnCurrencyId() { return this.EntityPM.ForiegnCurrencyId; }
    set ForiegnCurrencyId(newValue: string) {
        if (this.EntityPM.ForiegnCurrencyId != newValue) {
            this.EntityPM.ForiegnCurrencyId = newValue;
            this.SetLabels();
        }
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.DataContext.UnitPrice == 0) {
            errors.push("Unit price field is required");
        }

        else {
            if (this.DataContext.fatherComponent.EntityPM.ARInvoiceTypeCode == "CD") {
                if (this.DataContext.UnitPrice > 0) {
                    if (!SessionLocator.AccountingSettingPM.AllowPositiveAmountsCreditNote) {
                        errors.push(TextCodeTranslator.Translate("ARInvoice.M.NoPositivePrice"));
                    }
                }
            }
            else {
                if (this.DataContext.UnitPrice < 0) {
                    if (!SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                        errors.push(TextCodeTranslator.Translate("ARInvoice.M.NoMinusPrice"));
                    }
                }
            }
        }

        if (this.DataContext.chargesTypeList != null && AppTool.IsNullOrEmpty(this.DataContext.chargesTypeList.ReceivableCreditGLAccountId)) {
            errors.push(TextCodeTranslator.Translate("ARInvoice.M.NoGLAccount"));
        }

        if (this.DataContext.fatherComponent.glaccount != null && this.DataContext.fatherComponent.glaccount.IsVATExempt == true && this.DataContext.VatPercentage > 0) {
            errors.push("The partner is VAT exempt");
        }

        if (errors.length == 0) {
            if (this.DataContext.AddNewLineMode) {
                this.DataContext.AddNewLineMode = false;

                this.DataContext.fatherComponent.EntityPM.AddARInvoiceLinePM(this.EntityPM);
                //this.DataContext.Exists = true;
                this.DataContext.fatherComponent.SetUIProperties();
                this.DataContext.fatherComponent.BuildScreenData();
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }

        else {
            var errors_new = [];
            errors.forEach(item => {

                if (item.indexOf("%ForiegnCurrencyCode") > -1) {
                    errors_new.push(item.replace("%ForiegnCurrencyCode", this.DataContext.ForiegnCurrencyCode));
                }
                else if (item.indexOf("%LocalCurrencyCode") > -1) {
                    errors_new.push(item.replace("%LocalCurrencyCode", this.DataContext.LocalCurrencyCode));
                }

                else if (item.indexOf("%InvoiceCurrencyCode") > -1) {
                    errors_new.push(item.replace("%InvoiceCurrencyCode", this.DataContext.InvoiceCurrencyCode));
                }
                else {
                    errors_new.push(item);
                }

            });

            errors = errors_new;
        }
        this.ValidationErrorsList = errors;

    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('LocalDescription');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
        this.myCloner.AddField('ForiegnCurrencyId');
        this.myCloner.AddField('ForiegnExchangeRate');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('ForiegnCurrencyAmount');
        this.myCloner.AddField('LocalCurrencyAmount');
        this.myCloner.AddField('InvoiceCurrencyAmount');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
