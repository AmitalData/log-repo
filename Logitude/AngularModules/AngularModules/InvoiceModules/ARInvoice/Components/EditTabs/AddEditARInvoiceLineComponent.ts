import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ARInvoiceLinePM} from '../../../../Invoice/EntityPMs/ARInvoiceLinePM';
import {ARInvoiceLineItem} from './ARInvoiceDetailsTabNormal';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {VatTypesValidator} from '../../../../Infrastructure/Validators/VatTypesValidator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditARInvoiceLineComponent.html',
})

export class AddEditARInvoiceLineComponent {
    public EntityPM: ARInvoiceLinePM;
    public ObjectTableName = "ARInvoiceLine";
    public DataContext: ARInvoiceLineItem;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    SetDataContext(dataContext: ARInvoiceLineItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        //this.DataContext.SetUIProperties();
        this.SetLabels();
        this.Clone();
    }

    public AmountForiegnLabel: string = null;
    public AmountLocalLabel: string = null;
    public AmountInvoiceLabel: string = null;
    SetLabels() {
        this.AmountForiegnLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.ForiegnCurrencyAmount").replace("%ForiegnCurrencyCode", this.DataContext.ForiegnCurrencyCode);
        this.AmountLocalLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.LocalCurrencyAmount").replace("%LocalCurrencyCode", SessionLocator.LocalCurrencyCode);
        this.AmountInvoiceLabel = TextCodeTranslator.Translate("ARInvoiceLine.F.InvoiceCurrencyAmount").replace("%InvoiceCurrencyCode", this.DataContext.fatherComponent.InvoiceCurrencyCode);
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
            var field = TextCodeTranslator.Translate("ARInvoiceLine.F.VatTypeId");
            errors.push(msg.replace("%FieldName", field));
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.VatPercentage)) {
            if (!this.EntityPM.VatIsMultiPercentage) {
                var field = TextCodeTranslator.Translate("ARInvoiceLine.F.VatPercentage");
                errors.push(msg.replace("%FieldName", field));
            }
        }

        if (this.EntityPM.VatIsMultiPercentage) {
            if (!SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                errors.push(VatTypesValidator.GetError());
            }
        }

        if (this.DataContext.UnitPrice == 0) {
            var field = TextCodeTranslator.Translate("ARInvoiceLine.F.UnitPrice");
            errors.push(msg.replace("%FieldName", field));
        }

        else {
            if (this.DataContext.fatherComponent.EntityPM.ARInvoiceTypeCode == "CD" || this.DataContext.fatherComponent.EntityPM.ARInvoiceTypeCode == "CC") {
                if (this.DataContext.UnitPrice > 0) {
                    if (!SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
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

        this.ValidationErrorsList = [];

        errors.forEach(error => {

            if (error.indexOf("%ForiegnCurrencyCode") > -1) {
                error = error.replace("%ForiegnCurrencyCode", this.DataContext.ForiegnCurrencyCode);
            }

            this.ValidationErrorsList.push(error);
        });

        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('LocalDescription');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
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
