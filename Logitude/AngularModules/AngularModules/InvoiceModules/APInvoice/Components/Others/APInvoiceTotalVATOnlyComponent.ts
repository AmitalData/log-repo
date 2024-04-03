import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { APInvoicePM } from '../../../../Invoice/EntityPMs/APInvoicePM';
import { APInvoiceTotalVATPM } from '../../../../Invoice/EntityPMs/APInvoiceTotalVATPM';
import { VatTypePercentagePM } from '../../../../Common/EntityPMs/VatTypePercentagePM';
import { VATTypesGroupPM } from '../../../../Common/EntityPMs/VATTypesGroupPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { InvoiceTool } from '../../../../Invoice/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { VatTypeList } from '../../../../Common/EntityLists/VatTypeList';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { APInvoiceDetailsTabNormal } from '../EditTabs/APInvoiceDetailsTabNormal';

@Component({
    templateUrl: './APInvoiceTotalVATOnlyComponent.html',
})

export class APInvoiceTotalVATOnlyComponent extends BaseComponent {
    public EntityPM: APInvoicePM = null;
    public DataContext = this;
    public ObjectTableName: string = "APInvoice";
    public ValidationErrorsList: string[] = [];
    public TotalVATsList: ObservableCollection;
    public IsEditingEnabled: boolean = false;
    DetailsTabComponent: APInvoiceDetailsTabNormal;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsUsingVirtuallization: boolean = false;
    constructor() {
        super();
        this.TotalVATsList = new ObservableCollection([]);
    }

    SetIsUsingVirtuallization() {
        var hasGridVirtuallizationToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EVG")[0]
        if (hasGridVirtuallizationToggleFeature) {
            this.IsUsingVirtuallization = true;
        }
    }

    private TotalVATOnlyOrigin: boolean = false;
    SetWindowArgs(tabComponent: APInvoiceDetailsTabNormal) {
        this.SetIsUsingVirtuallization();
        this.DetailsTabComponent = tabComponent;
        this.EntityPM = this.DetailsTabComponent.EntityPM;

        this.IsEditingEnabled = InvoiceTool.IsEditingAPInvoiceEnabled(this.EntityPM);

        this.UIProperties.SetEnabled("TotalVATOnly", this.ObjectTableName, this.IsEditingEnabled);

        this.TotalVATOnlyOrigin = this.TotalVATOnly;

        if (this.TotalVATOnly) {
            this.BuildItems();
        }

        this.Clone();
    }

    get IsEditingVATEnabled() {
        var output: boolean = false;

        if (this.IsEditingEnabled) {
            if (this.TotalVATOnly) {
                output = true;
            }
        }

        return output;
    }

    get TotalVATOnly() { return this.EntityPM.TotalVATOnly; }
    set TotalVATOnly(value: boolean) {
        if (this.EntityPM.TotalVATOnly != value) {
            this.EntityPM.TotalVATOnly = value;
        }
    }

    BuildItems() {
        this.TotalVATsList.Clear();

        var itemsCollection: TotalVATItem[] = [];

        this.EntityPM.TotalVATs.forEach(item => {
            itemsCollection.push(new TotalVATItem(item, this, false));
        });

        this.TotalVATsList.InsertCollection(itemsCollection);
    }

    AddTotalVATItemClicked() {
        var item: APInvoiceTotalVATPM = new APInvoiceTotalVATPM(null);
        item.Tenant = SessionLocator.Tenant;
        item.APInvoiceId = this.EntityPM.Id;
        this.TotalVATsList.Insert(new TotalVATItem(item, this, true));
    }

    OkButtonClicked() {
        this.CheckIfHasChanged();

    // wronge
    // he can open this window
    // old = vat only
    // didnt change the check
    // but he added and removed lines

        //if (this.TotalVATOnly == this.TotalVATOnlyOrigin) {
        //    this.CurrentSession.CloseCurrentWindow();
        //}

        if (this.TotalVATOnly == false) {
            this.DetailsTabComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            var errors: string[] = [];

            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            if (this.TotalVATsList.Length == 0) {
                errors.push("You should have at least 1 invoice total VAT");
            }

            else {
                this.TotalVATsList.Collection.forEach((item: TotalVATItem) => {
                    // No need for validator, it will display alot of required fields
                    // Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);

                    if (AppTool.IsNullOrEmpty(item.VatTypeId)) {
                        errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoiceTotalVAT.F.VatTypeId")));
                    }

                    if (AppTool.IsNullOrEmpty(item.VatPercent)) {
                        if (!item.IsMultiPercentage) {
                            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APInvoiceTotalVAT.F.VatPercent")));
                        }
                    }

                    if (AppTool.IsNullOrEmpty(item.InvoiceCurrencyVATAmount)) {
                        errors.push(msg.replace("%FieldName", "Amount"));
                    }
                });
            }

            this.ValidationErrorsList = errors;

            if (errors.length == 0) {

                this.EntityPM.TotalVATs = [];

                this.TotalVATsList.Collection.forEach((item: TotalVATItem) => {

                    this.EntityPM.AddAPInvoiceTotalVATPM(item.EntityPM);

                    if (item.IsNewEntity) {
                    // HasChanges .. use such flag to decide calling the build totals / summary or just close screen

                    }
                });

                this.DetailsTabComponent.ComputeTotals();
                this.CurrentSession.CloseCurrentWindowEmit("Ok");
            }
        }
    }

    CheckIfHasChanged() {
        if (!this.EntityPM.Id) {

        }

        else if (this.TotalVATOnly != this.TotalVATOnlyOrigin) {

        }

        else if (this.TotalVATOnly) {

        } 
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('TotalVATOnly');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {

        this.TotalVATsList.Collection.forEach((item: TotalVATItem) => {
            item.RejectChanges();
        });

        this.myCloner.RejectChanges();
    }

    RemoveLine(item) {
        if (item) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Delete this item ?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    var index = this.TotalVATsList.Collection.indexOf(item);
                    if (index > -1) {
                        this.TotalVATsList.Remove(item);
                    }
                }
            });
        }
    }
}

export class TotalVATItem extends BaseComponent {
    public EntityPM: APInvoiceTotalVATPM;
    public IsNewEntity: boolean = false;
    public ObjectTableName: string = "APInvoiceTotalVAT";
    public IsMultiPercentage: boolean = false;
    public VatTypesGroups: VATTypesGroupPM[] = [];
    private VatTypeId_Origin: string;
    private VatTypeName_Origin: string;
    private VatPercent_Origin: number;
    private Amount_Origin: number;
    constructor(entity: APInvoiceTotalVATPM, public father: APInvoiceTotalVATOnlyComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;

        this.VatTypeId_Origin = this.VatTypeId;
        this.VatTypeName_Origin = this.VatTypeName;
        this.VatPercent_Origin = this.VatPercent;
        this.Amount_Origin = this.InvoiceCurrencyVATAmount;

        var vat: VatTypeList = father.DetailsTabComponent.AllVatTypes.filter(f => f.Id == entity.VatTypeId)[0];
        if (vat) {
            if (vat.IsMultiPercentage) {
                this.IsMultiPercentage = true;
            }
        }
    }

    private vatTypeList: VatTypeList;
    get VatTypeList() { return this.vatTypeList; }
    set VatTypeList(value: VatTypeList) {
        if (this.vatTypeList != value) {
            this.vatTypeList = value;

            this.VatPercent = null;
            this.VatTypeName = null;
            this.VatTypesGroups = [];
            this.IsMultiPercentage = false;
            this.EntityPM.ExternalVATCard = null;
            this.EntityPM.ExternalTAXItemId = null;

            if (value) {
                this.VatTypeName = value.EnglishName;
                this.IsMultiPercentage = value.IsMultiPercentage;
                this.EntityPM.ExternalVATCard = SessionLocator.AccountingSettingPM.PayableVATCard;
                this.EntityPM.ExternalTAXItemId = value.ExternalTAXItemId;

                if (value.IsMultiPercentage) {
                    this.VatTypesGroups = SessionLocator.AllVatTypesGroups.filter(f => f.GroupVATTypeId == this.VatTypeId);
                }

                else {
                    this.VatPercent = this.father.DetailsTabComponent.GetVatTypePercentage(value.Id);
                }
            }

            this.ComputeAllAmounts();
        }
    }

    get VatTypeId() { return this.EntityPM.VatTypeId; }
    set VatTypeId(value: string) {
        if (this.EntityPM.VatTypeId != value) {
            this.EntityPM.VatTypeId = value;
        }
    }

    get VatTypeName() { return this.EntityPM.VatTypeName; }
    set VatTypeName(value: string) {
        if (this.EntityPM.VatTypeName != value) {
            this.EntityPM.VatTypeName = value;
        }
    }

    get VatPercent() { return this.EntityPM.VatPercent; }
    set VatPercent(value: number) {
        if (this.EntityPM.VatPercent != value) {
            this.EntityPM.VatPercent = value;
        }
    }

    get InvoiceCurrencyVATAmount() { return this.EntityPM.InvoiceCurrencyVATAmount; }
    set InvoiceCurrencyVATAmount(value: number) {
        if (this.EntityPM.InvoiceCurrencyVATAmount != value) {
            this.EntityPM.InvoiceCurrencyVATAmount = value;
            this.ComputeAllAmounts();
        }
    }

    ComputeAllAmounts() {

        this.EntityPM.InvoiceCurrencyVatableAmount = 0;
        this.EntityPM.LocalVatableAmount = 0;
        this.EntityPM.ProfitVatableAmount = 0;

        this.EntityPM.InvoiceCurrencyVATAmount = AppTool.Round(this.EntityPM.InvoiceCurrencyVATAmount, 2);

        //this.EntityPM.InvoiceCurrencyVatableAmount = AppTool.Round(this.EntityPM.InvoiceCurrencyVatableAmount, 2);
        //this.EntityPM.LocalVatableAmount = AppTool.Round(this.EntityPM.InvoiceCurrencyVatableAmount * this.father.EntityPM.InvoiceCurrencyExchangeRate, 2);
        //this.EntityPM.ProfitVatableAmount = AppTool.Round(this.EntityPM.LocalVatableAmount / this.father.EntityPM.ProfitCurrencyExchangeRate, 2);

        //var vatAmount: number = 0;

        //if (this.IsMultiPercentage) {
        //    this.VatTypesGroups.forEach((item: VATTypesGroupPM) => {

        //        var percentage: number = item.SingleVATTypePercentage;

        //        if (!percentage) {
        //            percentage = this.father.DetailsTabComponent.GetVatTypePercentage(item.SingleVATTypeId);
        //        }

        //        vatAmount += this.EntityPM.InvoiceCurrencyVatableAmount * percentage / 100;
        //    });
        //}

        //else {
        //    vatAmount = AppTool.Round((this.EntityPM.InvoiceCurrencyVatableAmount * this.EntityPM.VatPercent / 100), 2);
        //}

        //this.EntityPM.InvoiceCurrencyVATAmount = AppTool.Round(vatAmount, 2);
        this.EntityPM.LocalVATAmount = AppTool.Round(this.EntityPM.InvoiceCurrencyVATAmount * this.father.EntityPM.InvoiceCurrencyExchangeRate, 2);
        this.EntityPM.ProfitCurrencyVATAmount = AppTool.Round(this.EntityPM.LocalVATAmount / this.father.EntityPM.ProfitCurrencyExchangeRate, 2);
    }

    RejectChanges() {
        this.VatTypeId = this.VatTypeId_Origin;
        this.VatTypeName = this.VatTypeName_Origin;
        this.VatPercent = this.VatPercent_Origin;
        this.InvoiceCurrencyVATAmount = this.Amount_Origin;
    }
}
