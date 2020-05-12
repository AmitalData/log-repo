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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TotalVATsList = new ObservableCollection([]);
    }

    //private TabComponent:
    private isTotalVATOnlyOrigin: boolean = false;
    private PercentagesList: VatTypePercentagePM[] = [];
    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.PercentagesList = args.PercentagesList;

        this.IsEditingEnabled = InvoiceTool.IsEditingAPInvoiceEnabled(this.EntityPM);

        this.UIProperties.SetEnabled("TotalVATOnly", this.ObjectTableName, this.IsEditingEnabled);

        this.isTotalVATOnlyOrigin = this.TotalVATOnly;

        if (this.TotalVATOnly) {
            this.BuildPackageItems();
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

    BuildPackageItems() {
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

        //item.VatTypeId = item.VatTypeId;
        //item.VatTypeName = itemVatType.EnglishName;
        //item.ExternalVATCard = item.ExternalVatCard;
        //item.ExternalTAXItemId = item.ExternalTAXItemId;
        //item.VatPercent = AppTool.Round(item.VatTypePercentage, 3);
        //item.LocalVatableAmount = AppTool.Round(item.LocalCurrencyAmount, 2);
        //item.InvoiceCurrencyVatableAmount = AppTool.Round(item.InvoiceCurrencyAmount, 2);
        //item.ProfitVatableAmount = AppTool.Round(item.ProfitCurrencyAmount, 2);
        //item.LocalVATAmount = AppTool.Round((itemTotalVAT.LocalVatableAmount * itemTotalVAT.VatPercent / 100), 2);
        //item.InvoiceCurrencyVATAmount = AppTool.Round((itemTotalVAT.InvoiceCurrencyVatableAmount * itemTotalVAT.VatPercent / 100), 2);
        //item.ProfitCurrencyVATAmount = AppTool.Round((itemTotalVAT.ProfitVatableAmount * itemTotalVAT.VatPercent / 100), 2);
        //item.VatTypeCell = itemTotalVAT.VatTypeName + " (" + pipe.transform(itemTotalVAT.VatPercent, "N3") + "%)"; //pipe.transform(item.InvoiceCurrencyVATAmount, "N2")
        //this.EntityPM.AddAPInvoiceTotalVATPM(itemTotalVAT);

        this.TotalVATsList.Insert(new TotalVATItem(item, this, true));
    }

    OnRowEnded($event) {
        //var errors = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        //if (($event) == this.TotalVATsList.Length) {
        //    this.AddTotalVATItemClicked();
        //}        
    }

    GetVatTypePercentage(vatTypeId: string) {
        var output: number = null;

        var vatTypePercentagePM = this.PercentagesList.filter(d => d.VatTypeId == vatTypeId)[0];
        if (vatTypePercentagePM != null) {
            output = vatTypePercentagePM.Percentage;
        }

        return output;
    }

    OkButtonClicked() {
        var errors: string[] = [];

        this.TotalVATsList.Collection.forEach((item: TotalVATItem) => {
            Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);
        });

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
 
            this.CurrentSession.CloseCurrentWindow();
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
        this.myCloner.RejectChanges();
    }
}

export class TotalVATItem extends BaseComponent {
    public EntityPM: APInvoiceTotalVATPM;
    public ObjectTableName: string = "APInvoiceTotalVAT";
    public IsNewEntity: boolean = false;
    public IsMultiPercentage: boolean = false;
    constructor(entity: APInvoiceTotalVATPM, public father: APInvoiceTotalVATOnlyComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
    }

    private vatTypeList: VatTypeList;
    get VatTypeList() { return this.vatTypeList; }
    set VatTypeList(value: VatTypeList) {
        if (this.vatTypeList != value) {
            this.vatTypeList = value;

            if (value) {
                this.VatTypeName = value.EnglishName;
                this.IsMultiPercentage = value.IsMultiPercentage;

                if (this.IsMultiPercentage) {
                    this.VatPercent = null;
                }

                else {
                    this.VatPercent = this.father.GetVatTypePercentage(value.Id);
                }
            }

            else {
                this.VatTypeName = null;
                this.IsMultiPercentage = false;
                this.VatPercent = null;
            }
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

    get InvoiceCurrencyVatableAmount() { return this.EntityPM.InvoiceCurrencyVatableAmount; }
    set InvoiceCurrencyVatableAmount(value: number) {
        if (this.EntityPM.InvoiceCurrencyVatableAmount != value) {
            this.EntityPM.InvoiceCurrencyVatableAmount = value;
        }
    }

    RemoveLine(item) {
        //var confirmWindow = new ConfirmWindow();
        //confirmWindow.Show("Delete this item ?");
        //confirmWindow.WindowClosed.subscribe((event: any) => {
        //    if (confirmWindow.Yes) {
        //        if (this.fatherComponent.EntityPM.ShipmentPackageItems.indexOf(this.EntityPM) != -1) {
        //            this.fatherComponent.EntityPM.RemoveShipmentPackageItemPM(this.EntityPM);
        //        }

        //        if (this.fatherComponent.PackageItemsList.Collection.indexOf(this) != -1) {
        //            this.fatherComponent.PackageItemsList.Remove(this);
        //        }
        //    }
        //});
    }
}
