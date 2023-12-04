import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {APInvoiceLineItem} from './APInvoiceDetailsTabGeneral';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {VatTypesValidator} from '../../../../Infrastructure/Validators/VatTypesValidator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ColumnsWidths } from 'Infrastructure/Components/LogitudeComponents/LogLovV2Component';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
@Component({
    
    templateUrl: './AddEditAPGeneralInvoiceLineComponent.html',
})

export class AddEditAPGeneralInvoiceLineComponent {
    public EntityPM: APInvoiceLinePM = null;
    public ObjectTableName = "APInvoiceLine";
    public DataContext: APInvoiceLineItem;
    public ValidationErrorsList: string[] = [];
    public EnableMultiRateAPInvoices: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    ColumnsWidths: ColumnsWidths[] = [];
    public GLAccountsFilterItems: ApiQueryFilters;

    constructor() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       

        if (SessionLocator.AccountingSettingPM) {
            this.EnableMultiRateAPInvoices = SessionLocator.AccountingSettingPM.EnableMultiRateAPInvoices;
        }

        if (SessionLocator.TenantPM.AccountingActivated) {
            this.FillChargesTypesCustomLOVColumnsWidths();
        }
        this.InitLOVFilters();
    }
    InitLOVFilters() {
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("GLAccountId", "null", null, null, "NotEqual", false, false, false, "string");
    }
    FillChargesTypesCustomLOVColumnsWidths()
    {
        this.ColumnsWidths = [
            { ColumnName: 'Code', Width: 80 },
            { ColumnName: 'EnglishName', Width: 180 },
            { ColumnName: 'LocalName', Width: 200 },
            { ColumnName: 'MeasurementShortName', Width: 80 },
            { ColumnName: 'ChargesGroupName', Width: 80 },
            { ColumnName: 'VatTypeName', Width: 80 }
        ];
    }



    SetDataContext(dataContext: APInvoiceLineItem) {
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        this.EntityPM = dataContext.invoiceLinePM;
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.EntityPM.VatTypeId)) {
            var field = TextCodeTranslator.Translate("APInvoiceLine.F.VatTypeId");
            errors.push(msg.replace("%FieldName", field));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.VatPercentage)) {
            if (!this.EntityPM.VatIsMultiPercentage) {
                var field = TextCodeTranslator.Translate("APInvoiceLine.F.VatPercentage");
                errors.push(msg.replace("%FieldName", field));
            }
        }

        if (this.EntityPM.VatIsMultiPercentage) {
            if (!SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                errors.push(VatTypesValidator.GetError());
            }
        }

        if (this.DataContext.chargesTypeList != null && AppTool.IsNullOrEmpty(this.DataContext.chargesTypeList.PayableDebitGLAcountId)) {
            errors.push(TextCodeTranslator.Translate("APInvoice.M.NoGLAccount"));
        }

        if (this.DataContext.Glaccount != null && this.DataContext.Glaccount.IsMultiCurrency == false) {
            if (this.DataContext.Glaccount.CurrencyId != this.DataContext.InvoiceCurrencyId) {
                errors.push("Line currency is " + this.DataContext.InvoiceCurrencyCode + " but the GLAccount of the charge type is " + this.DataContext.Glaccount.CurrencyCode);
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.DataContext.AddNewLineMode) {
                this.DataContext.AddNewLineMode = false;
                if (this.DataContext.fatherComponent.ItemsSource.Collection.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                }
                if (this.DataContext.fatherComponent.EntityPM.InvoiceLines.indexOf(this.EntityPM) == -1) {
                    this.DataContext.fatherComponent.EntityPM.AddAPInvoiceLinePM(this.EntityPM);
                }
                this.DataContext.Exists = true;
            }

            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('LocalDescription');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
        this.myCloner.AddField('VendorId');
        this.myCloner.AddField('ForiegnCurrencyId');
        this.myCloner.AddField('ExpectedAmount');
        this.myCloner.AddField('OtherInvoicesAmounts');
        this.myCloner.AddField('InvoiceCurrencyAmount');
        this.myCloner.AddField('OpenAmount');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
