
import { Component } from '@angular/core';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { SupplierInvoicePM } from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { SupplierInvoiceExtendedListService } from '../../../../../Customs/Services/ExtendedLists/SupplierInvoiceExtendedListService';
import { SupplierInvoiceItemList } from '../../../../../Customs/EntityLists/Extended/SupplierInvoiceItemList';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationPaymentComponent } from './DeclarationPaymentComponent';
import { DeclarationPaymentProtestPM } from '../../../../../Customs/EntityPMs/DeclarationPaymentProtestPM';
import { AppTool } from '../../../../../Infrastructure/Tools';

@Component({    
    templateUrl: './SupplierInvoiceSelectionComponent.html',
})

export class SupplierInvoiceSelectionComponent {
    public DeclarationPM: DeclarationPM;
    SupplierInvoicesList: ObservableCollection;
    public Invoices: SupplierInvoicePM[];
    InvoiceItemsList: ObservableCollection;
    SelectedInvoices: ObservableCollection;
    SelectedInvoiceItems: ObservableCollection;
    parent: DeclarationPaymentComponent;
    paymentProtest: DeclarationPaymentProtestPM;
    supplierInvoiceItemList: SupplierInvoiceItemList[];
    entityResourceService: EntityResourceService = new EntityResourceService();
    supplierInvoiceExtendedListService: SupplierInvoiceExtendedListService = new SupplierInvoiceExtendedListService
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.SupplierInvoicesList = new ObservableCollection([SupplierInvoiceLine]);
        this.InvoiceItemsList = new ObservableCollection([]);
        this.SelectedInvoiceItems = new ObservableCollection([]);
        this.SelectedInvoices = new ObservableCollection([]);
        this.supplierInvoiceItemList = [];
    }


    IsVisibile: boolean = false;
    SetWindowArgs(args: any) {
        this.DeclarationPM = args.DeclarationPM;
        this.parent = args.Parent;
        this.paymentProtest = args.Protest;
        //this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
        this.BuildInvoicesList();

        //});
    }

    BuildInvoicesList() {


        this.SupplierInvoicesList.Clear();
        this.Invoices = this.DeclarationPM.SupplierInvoices;
        let invoice: SupplierInvoicePM = null;
        //for (let protest of this.parent.paymentPM.DeclarationPaymentProtests) {

        if (!AppTool.IsNullOrEmpty(this.paymentProtest)) {
            invoice = this.Invoices.filter(a => a.InvoiceNumber == this.paymentProtest.InvoiceNumber && a.DeclarationId == this.paymentProtest.DeclarationId && a.InvoiceCounterKey == this.paymentProtest.InvoiceCounterKey)[0];
        }
        if (invoice != null && invoice.InvoiceNumber != null) {
            if (!this.SelectedInvoices.Collection.includes(invoice)) {


                this.SelectedInvoices.Collection.push(invoice);

            }

        }




        for (var item of this.Invoices) {

            var line: SupplierInvoiceLine = new SupplierInvoiceLine(item, this);

            this.SupplierInvoicesList.Insert(line);
            if (this.SelectedInvoices.Collection.includes(item)) {
                line.IsSelected = true;

                //this.LoadSupplierInvoiceItems(line);
            }
        }

    }


    SelectedRow: SupplierInvoiceLine;
    preventSelect: boolean;
    OnRowSelected(item: SupplierInvoiceLine) {
        if (!this.preventSelect) {
            if (this.SelectedRow != item) {
                this.SelectedRow = item;
                item.IsSelected = true;
                //if (!item.IsSelected) {
                this.LoadSupplierInvoiceItems(item);

                //}
            }


        }
        this.preventSelect = false;

    }

    LoadSupplierInvoiceItems(invoice: SupplierInvoiceLine) {


        //for (let item of this.InvoiceItemsList.Collection) {
        //    if (!item.IsSelected) {
        //        this.InvoiceItemsList.Remove(item);

        //    }
        //}

        this.InvoiceItemsList.Clear();

        this.supplierInvoiceExtendedListService.GetSupplierInvoiceItemsForInvoice(this.DeclarationPM.Id, invoice.InvoiceCounterKey).subscribe((response: any) => {
            if (response) {
                var TempInvoiceItemsList = [];
                for (let item of response.Result) {
                    let selectedItem: SupplierInvoiceItemList = null;
                    if (this.paymentProtest) {//&& a.ClassificationCode == this.paymentProtest.InvoiceItemClassificationCode && a.DeclarationId == this.paymentProtest.DeclarationId
                        selectedItem = response.Result.filter(a => a.SequenceNumeric == this.paymentProtest.GoodsItemLineNumber)[0];

                    }

                    if (selectedItem != null) {
                        if (invoice.IsAccumalated) {
                            if (selectedItem.IsParent) {

                                if (!this.SelectedInvoiceItems.Collection.includes(selectedItem)) {
                                    this.SelectedInvoiceItems.Collection.push(selectedItem);

                                }
                            }
                        }

                        else {
                            if (!this.SelectedInvoiceItems.Collection.includes(selectedItem)) {
                                this.SelectedInvoiceItems.Collection.push(selectedItem);
                            }

                        }
                    }

                    var invoiceItem: SupplierInvoiceItemLine = new SupplierInvoiceItemLine(item, this);
                    //var x: SupplierInvoiceItemLine = this.InvoiceItemsList.Collection.filter(d => d.LineNumer == item.LineNumber && d.CounterKey == item.CounterKey)[0];
                    //  this.supplierInvoiceItemList.push(item);
                    //if (x == null) {
                    if (invoice.IsAccumalated) {
                        if (item.IsParent) {

                            TempInvoiceItemsList.push(invoiceItem);
                        }
                    }
                    else {
                        TempInvoiceItemsList.push(invoiceItem);
                    }



                }
                    this.InvoiceItemsList.InsertCollection(TempInvoiceItemsList);

                if (this.SelectedInvoiceItems.Collection != null && this.SelectedInvoiceItems.Collection.length > 0) {
                    var line = this.SelectedInvoiceItems.Collection[0].LineNumber;
                    for (var i = 0; i < this.InvoiceItemsList.Collection.length; i++) {
                         if (line == this.InvoiceItemsList.Collection[i].entity.LineNumber)
                            this.InvoiceItemsList.Collection[i].IsSelected = true;
                    }

                }

            }

        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    OkButtonClicked() {
        this.parent.SelectedInvoiceItems = this.SelectedInvoiceItems;
        this.parent.SelectedInvoices = this.SelectedInvoices;
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

    SelectedItem: SupplierInvoiceItemLine;
    preventSelectItem: boolean = false;
    OnItemSelected(item: SupplierInvoiceItemLine) {
        //if (this.SelectedItem == item) {
        //    this.SelectedItem = null;
        //    item.IsSelected = false;
        //}
        //else {
        //    this.SelectedItem = item;
        //    item.IsSelected = true;
        //}

        if (!this.preventSelectItem) {
            if (this.SelectedItem != item) {
                this.SelectedItem = item;
                item.IsSelected = true;

            }
            //else {
            //    this.SelectedItem = null;
            //   item.IsSelected = false;
            //}



        }
        this.preventSelectItem = false;
    }

}

export class SupplierInvoiceLine {

    entity: SupplierInvoicePM;
    Parent: SupplierInvoiceSelectionComponent;
    constructor(invoice: SupplierInvoicePM, parent: SupplierInvoiceSelectionComponent) {
        this.entity = invoice;
        this.Parent = parent;
    }

    public get SequenceNumeric() { return this.entity.SequenceNumeric; }
    public get InvoiceNumber() { return this.entity.InvoiceNumber; }
    public get IssueDate() { return this.entity.IssueDate; }
    public get IncotermCode() { return this.entity.IncotermCode; }
    public get InvoiceAmount() { return this.entity.InvoiceAmount; }
    public get IssueCountryName() { return this.entity.IssueCountryName; }
    public get VendorName() { return this.entity.VendorName; }
    public get PreferenceDocumentTypeName() { return this.entity.PreferenceDocumentTypeName; }
    public get InsruanceCurrencyTypeCode() { return this.entity.InsruanceCurrencyTypeCode; }
    public get InsuranceAmount() { return this.entity.InsuranceAmount; }
    public get InvoiceCounterKey() { return this.entity.InvoiceCounterKey; }
    public get InvoiceCurrencyTypeCode() { return this.entity.InvoiceCurrencyTypeCode; }
    public get TotalFreightInInvoiceCurrencyText() { return this.entity.TotalFreightInFreightCurrency; }
    public get IsAccumalated() { return this.entity.IsAccumalated; }

    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        if (this.isSelected) {
            for (let item of this.Parent.SupplierInvoicesList.Collection) {
                if (item != this && item.IsSelected) {
                    item.IsSelected = false;
                }
                this.Parent.OnRowSelected(this);
            }
            if (!this.Parent.SelectedInvoices.Collection.includes(this.entity)) {
                this.Parent.SelectedInvoices.Insert(this.entity);
            }
        }
        else {
            if (this.Parent.SelectedInvoices.Collection.includes(this.entity)) {
                this.Parent.SelectedInvoices.Remove(this.entity);
            }
        }

    }

    PreventSelect() {
        this.Parent.preventSelect = true;
    }

}

export class SupplierInvoiceItemLine {
    entity: SupplierInvoiceItemList;
    Parent: SupplierInvoiceSelectionComponent;
    constructor(item: SupplierInvoiceItemList, parent: SupplierInvoiceSelectionComponent) {
        this.entity = item;
        this.Parent = parent;
    }

    public get SequenceNumeric() { return this.entity.SequenceNumeric; }
    public get ItemCode() { return this.entity.ItemCode; }
    public get ClassificationCode() { return this.entity.ClassificationCode; }
    public get TradeAgreementName() { return this.entity.TradeAgreementName; }
    public get StatisticQuantity() { return this.entity.StatisticQuantity; }
    public get InvoiceQuantity() { return this.entity.InvoiceQuantity; }
    public get CounterKey() { return this.entity.CounterKey; }
    public get ActualInvoiceLines() { return this.entity.ActualInvoiceLines; }
    public get ItemPrice() { return this.entity.ItemPrice; }
    public get OriginCountryName() { return this.entity.OriginCountryName; }
    public get LineNumber() { return this.entity.LineNumber; }
    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        if (this.isSelected) {
            this.Parent.preventSelect = false;
            for (let item of this.Parent.InvoiceItemsList.Collection) {
                if (item != this && item.IsSelected) {
                    item.IsSelected = false;
                }
                this.Parent.OnItemSelected(this);
            }
            if (!this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                this.Parent.SelectedInvoiceItems.Insert(this.entity);
            }
        }
        else {
            //this.Parent.SelectedItem = null;
            //this.Parent.preventSelectItem = true;
            if (this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                this.Parent.SelectedInvoiceItems.Remove(this.entity);
            }
        }

    }


    PreventSelect() {
        this.Parent.preventSelectItem = true;
    }


}
