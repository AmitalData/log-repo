
import {Component, EventEmitter, Output} from '@angular/core';
import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {SupplierInvoiceExtendedListService} from '../../../../../Customs/Services/ExtendedLists/SupplierInvoiceExtendedListService';
import {SupplierInvoiceItemList} from '../../../../../Customs/EntityLists/Extended/SupplierInvoiceItemList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {CustomsDocumentsTicketPM} from '../../../../../Customs/EntityPMs/CustomsDocumentsTicketPM';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './PointersFromInvoicesSelectionComponent.html',
})

export class PointersFromInvoicesSelectionComponent {
    public DeclarationPM: DeclarationPM;
    SupplierInvoicesList: ObservableCollection;
    public Invoices: SupplierInvoicePM[];
    InvoiceItemsList: ObservableCollection;
    public SelectedInvoices: ObservableCollection;
    public SelectedInvoiceItems: ObservableCollection;
    public StaticSelectedInvoiceItems: ObservableCollection;
    entityResourceService: EntityResourceService = new EntityResourceService();
    supplierInvoiceExtendedListService: SupplierInvoiceExtendedListService = new SupplierInvoiceExtendedListService
    CustomsDocumentsTicket: CustomsDocumentsTicketPM;
    public ConnectedInvoiceItems: string;
    public ConnectedInvoices: string;
    public IsTicketChanged: boolean;
    SelectInvoicesOnly: boolean;
    IsDisplayOnly: boolean;
    DisplayOnlyMessage: string;
    IsEntityDisplayOnly: boolean;
    deletedInvItems: string = "";
    initialSelectedItems: string = "";
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

    isExportDeclaration: boolean = false;
    IsVisibile: boolean = false;
    existInvoices:number[];
    SetWindowArgs(args: any) {
        if(args.CertificateOfOriginItem){
            this.isForCertificateOfOriginGeneralTabScreen = true;
            this.DeclarationPM = args.DeclarationPM;
            if (this.DeclarationPM != null && this.DeclarationPM.Direction == "E") {
                this.isExportDeclaration = true;
            }
            if(args.existInvoices){
                let selectedInvoices = args.existInvoices.split(',');
                this.existInvoices = selectedInvoices.map(str => Number(str));
            }
            this.BuildInvoicesListForCerficate();

            this.SelectInvoicesOnly = args.selectInvoicesOnly;
            var connectedCounterKeys = "";
            var connectedLineNumbers = "";   
            this.IsEntityDisplayOnly = args.IsEntityDisplayOnly;
            this.IsDisplayOnly = false;
            this.DisplayOnlyMessage = "";
        }
        else {

            this.DeclarationPM = args.DeclarationPM;
            if (this.DeclarationPM != null && this.DeclarationPM.Direction == "E") {
                this.isExportDeclaration = true;
            }
            this.CustomsDocumentsTicket = args.CustomsDocumentsTicket;
            this.SelectInvoicesOnly = args.selectInvoicesOnly;
            //this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
            this.initialSelectedItems = this.CustomsDocumentsTicket.ConnectedInvoiceItemsSequences;
    
            var connectedCounterKeys = "";
            var connectedLineNumbers = "";
            this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((pointer) => {
                if (pointer.Child1EntityId != null && pointer.Child2EntityId) {
                connectedCounterKeys = connectedCounterKeys + "," + pointer.Child1EntityId;
                  if (this.CustomsDocumentsTicket.ConnectedInvoiceItemsSequences.indexOf(pointer.Child2EntityId) > -1) {
                    connectedLineNumbers = connectedLineNumbers + "," + pointer.Child2EntityId;
                  }
                }
            });
    
            if (!AppTool.IsNullOrEmpty(connectedCounterKeys) && !AppTool.IsNullOrEmpty(connectedCounterKeys)) {
                this.supplierInvoiceExtendedListService.GetSelectedSupplierInvoiceItemLists(this.DeclarationPM.Id, connectedCounterKeys, connectedLineNumbers).subscribe((response:any) => {
                    var resp = response.Result;
                    resp.forEach((item) => {
                        this.StaticSelectedInvoiceItems.Insert(item);
                    });
    
                    this.BuildInvoicesList();
                });
            }
            else {
                this.BuildInvoicesList();
            }
            //this.BuildInvoicesList();
            //this.SelectedItemChangedEvt = this.SelectedInvoiceItems.Changed.subscribe((response) => {
            //    if (response.Operation == "insert") {
            //        //this.SelectedInvoiceItems.Collection.forEach((item) => {
            //        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == response.Item.rowData.CounterKey && d.LineNumber == response.Item.rowData.LineNumber)[0];
            //        if (!exists) {
            //            this.StaticSelectedInvoiceItems.Insert(response.Item.rowData);
            //        }
            //        // });
            //    }
            //    else if (response.Operation == "remove") {
            //        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == response.Item.rowData.CounterKey && d.LineNumber == response.Item.rowData.LineNumber)[0];
            //        this.StaticSelectedInvoiceItems.Remove(exists);
            //    }
    
    
            //    //var previousSelected = this.StaticSelectedInvoiceItems;
            //    //previousSelected.Collection.forEach((item) => {
            //    //    var exists = this.SelectedInvoiceItems.Collection.filter(d => d.rowData.CounterKey == item.CounterKey && d.rowData.LineNumber == item.LineNumber)[0];
            //    //    if (!exists) {
            //    //        this.StaticSelectedInvoiceItems.Remove(item);
            //    //    }
            //    //});
            //});
    
            //});
            this.IsEntityDisplayOnly = args.IsEntityDisplayOnly;
            if (this.CustomsDocumentsTicket.RequestedCustomsDocId || this.IsEntityDisplayOnly) {
                this.IsDisplayOnly = true;
                this.DisplayOnlyMessage = "מסמך לתצוגה בלבד";
            }
            else {
                this.IsDisplayOnly = false;
                this.DisplayOnlyMessage = "";
            }
        } 
    }

    BuildInvoicesList() {


        this.SupplierInvoicesList.Clear();
      this.Invoices = this.DeclarationPM.SupplierInvoices;
      var temp: SupplierInvoiceLine[] = [];
        this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((pointer) => {
            var invoice: SupplierInvoicePM = this.Invoices.filter(d => d.InvoiceCounterKey + "" == pointer.Child1EntityId)[0];
            if (invoice) {
                var exists: SupplierInvoicePM = this.SelectedInvoices.Collection.filter(d => d.InvoiceCounterKey == invoice.InvoiceCounterKey)[0];
                if (!exists) {
                  var line: SupplierInvoiceLine = new SupplierInvoiceLine(invoice, this);
                  var exist = temp.filter(d => d.InvoiceCounterKey == line.InvoiceCounterKey)[0];
                  if (!exist) {
                      this.SupplierInvoicesList.Insert(line); 
                      //this.SelectedInvoices.Collection.push(line);
                      temp.push(line); 
                    }
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
        //this.LoadSupplierInvoiceItems(this.SelectedInvoices.Collection);

    }

    isForCertificateOfOriginGeneralTabScreen = false;
    BuildInvoicesListForCerficate() {
        this.SupplierInvoicesList.Clear();
        this.Invoices = this.DeclarationPM.SupplierInvoices;
        this.SelectedInvoices = new ObservableCollection([]);
        this.Invoices.forEach(invoice=>{
            var line: SupplierInvoiceLine = new SupplierInvoiceLine(invoice, this);
            this.SupplierInvoicesList.Insert(line); 
            if(this.existInvoices?.filter(i => i == invoice.InvoiceCounterKey)[0]) {
                this.SelectedInvoices.Insert(line);
            } 
        });
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
        //this.LoadSupplierInvoiceItems(items);
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }
    OnRowUnselected(item: any) {
        //console.log(item);
        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == item.CounterKey && d.LineNumber == item.LineNumber)[0];
        this.StaticSelectedInvoiceItems.Remove(exists);
    }
    OnItemRowSelected(item: any) {

        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == item.rowData.CounterKey && d.LineNumber == item.rowData.LineNumber)[0];
        if (!exists) {
            this.StaticSelectedInvoiceItems.Insert(item.rowData);
        }

        //var exists = this.SelectedInvoiceItems.Collection.filter(d => d.rowData.CounterKey == item.rowData.CounterKey && d.rowData.LineNumber==item.rowData.LineNumber)[0];
        //if (!exists) {
        //    this.SelectedInvoiceItems.Insert(item);
        //    //this.StaticSelectedInvoiceItems.Insert(item);

        //}
        //this.SelectedInvoiceItems.Collection.forEach((invoiceItemLine) => {
        //    var item = items.filter(d => d.CounterKey == invoiceItemLine.CounterKey && d.LineNumber == invoiceItemLine.LineNumber)[0];
        //    if (!item) {
        //        this.SelectedInvoiceItems.Remove(invoiceItemLine);
        //    }
        //});
    }

    OnDataLoaded(items: any[]) {
        var temp = [];
        //this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((pointer) => {
        //    var invItem: any = items.filter(d => d.rowData.CounterKey + "" == pointer.Child1EntityId && d.rowData.LineNumber + "" == pointer.Child2EntityId)[0];
        //    if (invItem) {
        //        temp.push(invItem);
        //    }

        //});


        this.StaticSelectedInvoiceItems.Collection.forEach((item) => {
          //var exists = this.SelectedInvoiceItems.Collection.filter(d => d.rowData.CounterKey == item.CounterKey && d.rowData.LineNumber == item.LineNumber)[0];
          this.SelectedInvoiceItems.Clear();
            var invItem: any = items.filter(d => d.rowData.CounterKey + "" == item.CounterKey && d.rowData.LineNumber + "" == item.LineNumber)[0];
            //if (this.SelectedInvoiceItems.Collection.indexOf(item) == -1) {exists == null &&
            if (invItem!=null) {
                this.SelectedInvoiceItems.Insert(invItem);
            }
        });

        //if (!this.SelectedInvoiceItems.Collection.includes(invItem)) {
        //this.SelectedInvoiceItems.AppendCollection(temp);//.push(invItem);
        //}
        //temp.forEach((invoiceItemLine) => {
        //    this.SelectedInvoiceItems.Insert(invoiceItemLine);
        //});
    }

    LoadSupplierInvoiceItems(invoices: SupplierInvoiceLine[]) {
        return;
        //this.InvoiceItemsList.Clear();

        //var keys = "";
        //invoices.forEach((invoice) => {
        //    keys = keys + "," + invoice.InvoiceCounterKey;
        //});
        //keys = keys.substr(1, keys.length - 1);
        //if (keys.length > 0) {
        //    this.supplierInvoiceExtendedListService.GetSupplierInvoiceItemsForInvoices(this.DeclarationPM.Id, keys, null, null).subscribe((response: any) => {
        //        if (response) {
        //            var invoiceItems: SupplierInvoiceItemList[] = response.Result;
        //            if (invoiceItems) {

        //                for (let item of invoiceItems) {
        //                    var invoiceItem: SupplierInvoiceItemLine = new SupplierInvoiceItemLine(item, this);
        //                    this.InvoiceItemsList.Insert(invoiceItem);
        //                }
        //                var temp = [];
        //                this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((pointer) => {
        //                    var invItem: SupplierInvoiceItemLine = this.InvoiceItemsList.Collection.filter(d => d.CounterKey + "" == pointer.Child1EntityId && d.LineNumber + "" == pointer.Child2EntityId)[0];
        //                    if (invItem) {
        //                        temp.push(invItem);
        //                    }
        //                    //if (!this.SelectedInvoiceItems.Collection.includes(invItem)) {
        //                    this.SelectedInvoiceItems.Collection = temp;//.push(invItem);
        //                    //}
        //                });


        //            }

        //        }

        //    });
        //}
    }

    CancelButtonClicked() {
        this.Dispose();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        //this.parent.SelectedInvoiceItems = this.SelectedInvoiceItems;
        //this.parent.SelectedInvoices = this.SelectedInvoices;
        //this.CustomsDocumentsTicket.ConnectedInvoicesSequences

        this.ConnectedInvoiceItems = "";
        //var initallySelected: string[] = this.initialSelectedItems ? this.initialSelectedItems.split(',') : [];
        //var deleted: string[] = this.deletedInvItems.substr(1, this.deletedInvItems.length - 1).split(',');
        //for (var i = 0; i < initallySelected.length; i++) {
        //    if (deleted.indexOf(initallySelected[i]) == -1) {
        //        this.ConnectedInvoiceItems = this.ConnectedInvoiceItems + "," + initallySelected[i];
        //    }
        //}


        this.StaticSelectedInvoiceItems.Collection.forEach((item) => {
            //if (initallySelected.indexOf(item.rowData.SequenceNumeric.toString()) == -1) {
                this.ConnectedInvoiceItems = this.ConnectedInvoiceItems + "," + item.SequenceNumeric;
           // }
        });
        this.ConnectedInvoiceItems = this.ConnectedInvoiceItems.substr(1, this.ConnectedInvoiceItems.length - 1);

        this.ConnectedInvoices = "";
        this.SelectedInvoices.Collection.forEach((item) => {
            this.ConnectedInvoices = this.ConnectedInvoices + "," + item.SequenceNumeric;
        });
        this.ConnectedInvoices = this.ConnectedInvoices.substr(1, this.ConnectedInvoices.length - 1);

        this.Dispose();
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

    ViewInitCompleted($event) {
        //this.SelectedRow = this.ItemsSource.Collection[0];
        //this.OnRowSelected(this.SelectedRow);
        this.SelectedRows = this.SelectedInvoices.Collection;
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        //this.SelectedInvoiceItems.Changed.subscribe((isCollection) => {

        //});
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



        //if (filters == null) {
        //    filters = new ApiQueryFilters();
        //}

        //filters.PageSize = take;
        //filters.PageIndex = skip;
        //filters.GetAll = false;
        //filters.GetCount = true;
        //filters.SortBy = "SequenceNumeric";
        //filters.SortDirection = "Ascending";
        //if (this.SelectedRow) {
        //    filters.addAdditionalFilter("DeclarationId", this.SelectedRow.DeclarationId, null, null, "Equals", false, false, false, "string");
        //    filters.addAdditionalFilter("CounterKey", this.SelectedRow.InvoiceCounterKey, null, null, "Equals", false, false, false, "number");

        //}
        //else {
        //    filters.addAdditionalFilter("DeclarationId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        //    //          filters.addAdditionalFilter("CounterKey", null, null, null, "Equals", false, false, false, "number");

        //}
        //return this._entityListService.getExtendedByFilters("Customs.SupplierInvoiceItem", filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);

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
    Parent: PointersFromInvoicesSelectionComponent;
    constructor(invoice: SupplierInvoicePM, parent: PointersFromInvoicesSelectionComponent) {
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
    Parent: PointersFromInvoicesSelectionComponent;
    constructor(item: SupplierInvoiceItemList, parent: PointersFromInvoicesSelectionComponent) {
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
