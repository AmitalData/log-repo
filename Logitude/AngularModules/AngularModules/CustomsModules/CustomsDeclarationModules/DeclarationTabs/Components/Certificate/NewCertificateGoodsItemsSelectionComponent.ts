
import {Component, EventEmitter, Output} from '@angular/core';
import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {SupplierInvoiceExtendedListService} from '../../../../../Customs/Services/ExtendedLists/SupplierInvoiceExtendedListService';
import {SupplierInvoiceItemList} from '../../../../../Customs/EntityLists/Extended/SupplierInvoiceItemList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../../Infrastructure/Tools';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { MultiCertificatesService } from '../../../../../Customs/Services/Others/MultiCertificatesService';

@Component({
    
    templateUrl: './NewCertificateGoodsItemsSelectionComponent.html',
})

export class NewCertificateGoodsItemsSelectionComponent {
    public DeclarationPM: DeclarationPM;
    public AttachmentTypeCode: string;
    public ReqConfirmationTypeCode: string;
    public ResConfirmationTypeCode: string;
    public CertificateNumber: string;
    public CertificateExemptionTypeCode: string;
    SupplierInvoicesList: ObservableCollection;
    public Invoices: SupplierInvoicePM[];
    InvoiceItemsList: ObservableCollection;
    public SelectedInvoices: ObservableCollection;
    public SelectedInvoiceItems: ObservableCollection;
    public StaticSelectedInvoiceItems: ObservableCollection;
    entityResourceService: EntityResourceService = new EntityResourceService();
    supplierInvoiceExtendedListService: SupplierInvoiceExtendedListService = new SupplierInvoiceExtendedListService
    multiCertificatesService: MultiCertificatesService = new MultiCertificatesService();
    public ConnectedInvoiceItems: string;
    public ConnectedInvoices: string;
    public IsTicketChanged: boolean;
    IsEntityDisplayOnly: boolean;
    deletedInvItems: string = "";
    @Output() MenuHeaderchangeevent = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    SelectedItemChangedEvt: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.SupplierInvoicesList = new ObservableCollection([SupplierInvoiceLine]);
        this.InvoiceItemsList = new ObservableCollection([]);
        this.SelectedInvoiceItems = new ObservableCollection([]);
        this.StaticSelectedInvoiceItems = new ObservableCollection([]);
        this.SelectedInvoices = new ObservableCollection([]);
        this.BuildColumns();
    }

    IsVisibile: boolean = false;
    SetWindowArgs(args: any) {
        this.DeclarationPM = args.DeclarationPM;
        this.AttachmentTypeCode = args.AttachmentTypeCode;
        this.ReqConfirmationTypeCode = args.ReqConfirmationTypeCode;
        this.ResConfirmationTypeCode = args.ResConfirmationTypeCode;
        this.CertificateNumber = args.CertificateNumber;
        this.CertificateExemptionTypeCode = args.CertificateExemptionTypeCode;
        this.BuildInvoicesList();
    }

    BuildInvoicesList() {

      this.SupplierInvoicesList.Clear();
      this.Invoices = this.DeclarationPM.SupplierInvoices;
      var temp: SupplierInvoiceLine[] = [];
        this.Invoices.forEach((invoice) => {
                var exists: SupplierInvoicePM = this.SelectedInvoices.Collection.filter(d => d.InvoiceCounterKey == invoice.InvoiceCounterKey)[0];
                if (!exists) {
                  var line: SupplierInvoiceLine = new SupplierInvoiceLine(invoice, this);
                  var exist = temp.filter(d => d.InvoiceCounterKey == line.InvoiceCounterKey)[0];
                  if (!exist) {
                      this.SupplierInvoicesList.Insert(line); 
                      temp.push(line); 
                    }
                }
      });
      this.SelectedInvoices.Collection = temp;
      this.SelectedRows = this.SelectedInvoices.Collection;
        for (var item of this.Invoices) {
            
            var invoice: SupplierInvoicePM = this.SupplierInvoicesList.Collection.filter(d => d.InvoiceCounterKey == item.InvoiceCounterKey)[0];
            if (!invoice) {
                var line: SupplierInvoiceLine = new SupplierInvoiceLine(item, this);
                if (!this.SupplierInvoicesList.Collection.includes(line)) {
                    this.SupplierInvoicesList.Insert(line);
                }
            }
        }
      
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    SelectedRow: SupplierInvoiceLine;
    SelectedRows: SupplierInvoiceLine[];
    preventSelect: boolean;
    OnRowSelected(items: SupplierInvoiceLine[]) {
        this.SelectedRows = items;
        this.SelectedInvoices.Collection.forEach((invoiceLine) => {
            var item = items.filter(d => d.InvoiceCounterKey == invoiceLine.InvoiceCounterKey)[0];
            if (!item) {
                this.SelectedInvoices.Remove(invoiceLine);
                var tempItems: any[] = [];

                this.SelectedInvoiceItems.Collection.forEach((invoiceItem) => {
                    tempItems.push(invoiceItem);

                });

                tempItems.forEach((invoiceItemtemp) => {
                    if (invoiceItemtemp.rowData.CounterKey == invoiceLine.InvoiceCounterKey) {
                        this.SelectedInvoiceItems.Remove(invoiceItemtemp);
                        this.deletedInvItems += "," + invoiceItemtemp.rowData.SequenceNumeric;
                    }
                });
            }
        });
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }
    OnRowUnselected(item: any) {
        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == item.CounterKey && d.LineNumber == item.LineNumber)[0];
        this.StaticSelectedInvoiceItems.Remove(exists);
    }
    OnItemRowSelected(item: any) {

        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == item.rowData.CounterKey && d.LineNumber == item.rowData.LineNumber)[0];
        if (!exists) {
            this.StaticSelectedInvoiceItems.Insert(item.rowData);
        }
    }

    OnDataLoaded(items: any[]) {
        var temp = [];
        this.StaticSelectedInvoiceItems.Collection.forEach((item) => {
          this.SelectedInvoiceItems.Clear();
            var invItem: any = items.filter(d => d.rowData.CounterKey + "" == item.CounterKey && d.rowData.LineNumber + "" == item.LineNumber)[0];
            if (invItem!=null) {
                this.SelectedInvoiceItems.Insert(invItem);
            }
        });
    }

    CancelButtonClicked() {
        this.Dispose();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ConnectedInvoiceItems = "";
        this.StaticSelectedInvoiceItems.Collection.forEach((item) => {
            this.ConnectedInvoiceItems = this.ConnectedInvoiceItems + "," + (item.CounterKey + " " + item.LineNumber);
        });
        this.ConnectedInvoiceItems = this.ConnectedInvoiceItems.substr(1, this.ConnectedInvoiceItems.length - 1);

        this.ConnectedInvoices = "";
        this.SelectedInvoices.Collection.forEach((item) => {
            this.ConnectedInvoices = this.ConnectedInvoices + "," + item.SequenceNumeric;
        });
        this.ConnectedInvoices = this.ConnectedInvoices.substr(1, this.ConnectedInvoices.length - 1);

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
        this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
        this.multiCertificatesService.CreateCertificateForInvoiceItems(this.DeclarationPM.Id, this.DeclarationPM.CustomFileNo, this.AttachmentTypeCode, this.ReqConfirmationTypeCode, this.ResConfirmationTypeCode, this.CertificateNumber, this.CertificateExemptionTypeCode, this.ConnectedInvoiceItems).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (response.HasError) {
                let messageWindow = new MessageWindow();
                messageWindow.Show(response.ErrorsArray[0]);
            }
            else {
                let messageWindow = new MessageWindow();
                messageWindow.RTL = true;
                let message = response.Result;
                messageWindow.Show(message.Message);
            }
            this.Dispose();
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        });
    }

    ViewInitCompleted($event) {
        this.SelectedRows = this.SelectedInvoices.Collection;
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    DataSource = {

        pageSize: 10,
        rowCount: null,
        sortingCol: "SequenceNumeric",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var keys = "";
        this.SelectedRows.forEach((invoice) => {
            keys = keys + "," + invoice.InvoiceCounterKey;
        });
        keys = keys.substr(1, keys.length - 1);
        return new Promise((resolve, reject) => {
            resolve(this.supplierInvoiceExtendedListService.GetSupplierInvoiceItemsForInvoices(this.DeclarationPM.Id, keys, skip, take, getCount));
        });

    }
    public columns: any[];
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'InvoiceNumber',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoice.F.InvoiceNumber"),
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'SequenceNumeric',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.SequenceNumeric"),
            Styles: { width: '50px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'TradeAgreementName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.TradeAgreementName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'InvoiceQuantity',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.InvoiceQuantity"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'DeclarationSupplierInvoiceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationSupplierInvoiceListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemPrice',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemPrice"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'DeclarationSupplierInvoiceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationSupplierInvoiceListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'OriginCountryName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.OriginCountryName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });


    }

    Dispose() {
        //this.SelectedItemChangedEvt.unsubscribe();
        //this.SelectedItemChangedEvt = null;
    }
}

export class SupplierInvoiceLine {

    entity: SupplierInvoicePM;
    Parent: NewCertificateGoodsItemsSelectionComponent;
    constructor(invoice: SupplierInvoicePM, parent: NewCertificateGoodsItemsSelectionComponent) {
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
    public get ExportFreightAmount() { return this.entity.ExportFreightAmount; }
    public get ExportInsuranceAmount() {  return this.entity.ExportInsuranceAmount; }
    public get DeclarationId() { return this.entity.DeclarationId; }
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
    Parent: NewCertificateGoodsItemsSelectionComponent;
    constructor(item: SupplierInvoiceItemList, parent: NewCertificateGoodsItemsSelectionComponent) {
        this.entity = item;
        this.Parent = parent;
    }

    public get SequenceNumeric() { return this.entity.SequenceNumeric; }
    public get ItemCode() { return this.entity.ItemCode; }
    public get ClassificationCode() { return this.entity.ClassificationCode; }
    public get TradeAgreementName() { return this.entity.TradeAgreementName; }
    public get StatisticQuantity() { return this.entity.StatisticQuantity; }
    public get InvoiceQuantity() { return this.entity.InvoiceQuantity; }
    public get OriginCountryName() { return this.entity.OriginCountryName; }
    public get ActualInvoiceLines() { return this.entity.ActualInvoiceLines; }
    public get ItemPrice() { return this.entity.ItemPrice; }
    public get CounterKey() { return this.entity.CounterKey; }
    public get LineNumber() { return this.entity.LineNumber; }

    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        if (this.isSelected) {
            for (let item of this.Parent.InvoiceItemsList.Collection) {
                if (item != this && item.IsSelected) {
                    item.IsSelected = false;
                }
            }
            if (!this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                this.Parent.SelectedInvoiceItems.Insert(this.entity);
            }
        }
        else {
            if (this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                this.Parent.SelectedInvoiceItems.Remove(this.entity);
            }
        }

    }

}
