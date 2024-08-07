import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {APInvoiceLineShortItem} from './EditMultipleShipmentComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {VatTypesValidator} from '../../../../Infrastructure/Validators/VatTypesValidator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    
    templateUrl: './AddEditMultipleAPInvoiceLineComponent.html',
})

export class AddEditMultipleAPInvoiceLineComponent {
    public EntityPM: APInvoiceLinePM = null;
    public ObjectTableName = "APInvoiceLine";
    public DataContext: APInvoiceLineShortItem;
    public ValidationErrorsList: string[] = [];
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public GLAccountsFilterItems: ApiQueryFilters;

    constructor() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
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
    SetDataContext(dataContext: APInvoiceLineShortItem) {
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.Clone();
      //  this.BuildQueryFilters();
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

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.DataContext.AddNewLineMode) {
                this.DataContext.AddNewLineMode = false;

                if (this.DataContext.fatherComponent.ItemsSource.Collection.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                }  

                this.DataContext.IsChecked = true;
            }

            this.CurrentSession.CloseCurrentWindowEmit("OK");
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
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
