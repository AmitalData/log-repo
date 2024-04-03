import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {APInvoiceLineItem} from './APInvoiceDetailsTabNormal';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {VatTypesValidator} from '../../../../Infrastructure/Validators/VatTypesValidator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    
    templateUrl: './AddEditAPInvoiceLineComponent.html',
})

export class AddEditAPInvoiceLineComponent {
    public EntityPM: APInvoiceLinePM = null;
    public ObjectTableName = "APInvoiceLine";
    public DataContext: APInvoiceLineItem;
    public ValidationErrorsList: string[] = [];
    public EnableMultiRateAPInvoices: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public GLAccountsFilterItems: ApiQueryFilters;

    constructor() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       
        if (SessionLocator.AccountingSettingPM) {
            this.EnableMultiRateAPInvoices = SessionLocator.AccountingSettingPM.EnableMultiRateAPInvoices;
        }
        this.InitLOVFilters();
    }
    InitLOVFilters() {
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("GLAccountId", "null", null, null, "NotEqual", false, false, false, "string");
    }
    
    public ChargeTypesQueryFilters: ApiQueryFilters;
    private BuildQueryFilters() {
        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsPayable", true, null, null, "Equals", false, false, false, "boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("PayableDebitGLAcountId", true, null, null, "IsNotNull", false, false, false, "Text");
    }
    public TotalVATOnly: boolean = false;
    SetDataContext(dataContext: APInvoiceLineItem) {
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        this.EntityPM = dataContext.invoiceLinePM;
        this.TotalVATOnly = this.DataContext.fatherComponent.EntityPM.TotalVATOnly;
        this.Clone();
        this.BuildQueryFilters();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (!this.TotalVATOnly) {
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
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.DataContext.AddNewLineMode) {
                this.DataContext.AddNewLineMode = false;

                //this.DataContext.fatherComponent.entityPM.AddAPInvoiceLinePM(this.EntityPM);
                if (this.DataContext.fatherComponent.ItemsSource.Collection.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                }
                if (this.DataContext.fatherComponent.EntityPM.InvoiceLines.indexOf(this.EntityPM) == -1) {
                    this.DataContext.fatherComponent.EntityPM.AddAPInvoiceLinePM(this.EntityPM);
                }      

                this.DataContext.Exists = true;
                //this.DataContext.fatherComponent.BuildInvoiceLines();
            }

            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
        this.myCloner.AddField('VendorId');
        this.myCloner.AddField('ForiegnCurrencyId');
        this.myCloner.AddField('ExpectedAmount');
        this.myCloner.AddField('OtherInvoicesAmounts');
        this.myCloner.AddField('InvoiceCurrencyAmount');
        this.myCloner.AddField('OpenAmount');
        this.myCloner.AddField("ForiegnCurrencyAmount");
        this.myCloner.AddField("ForiegnExchangeRate");
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
