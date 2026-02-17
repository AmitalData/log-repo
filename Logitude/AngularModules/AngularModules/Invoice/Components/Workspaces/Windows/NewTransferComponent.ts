import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AccountingTransferHeaderPM} from '../../../EntityPMs/AccountingTransferHeaderPM';
import {AccountingTransferLinePM} from '../../../EntityPMs/AccountingTransferLinePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ARInvoiceList} from '../../../EntityLists/ARInvoiceList';
import {APInvoiceList} from '../../../EntityLists/APInvoiceList';
import {ARPaymentList} from '../../../EntityLists/ARPaymentList';
import {APPaymentList} from '../../../EntityLists/APPaymentList';
import {ARInvoiceListService} from '../../../Services/StandardLists/ARInvoiceListService';
import {APInvoiceListService} from '../../../Services/StandardLists/APInvoiceListService';
import {ARPaymentListService} from '../../../Services/StandardLists/ARPaymentListService';
import {APPaymentListService} from '../../../Services/StandardLists/APPaymentListService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { InvoiceDomainService } from '../../../Services/InvoiceDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './NewTransferComponent.html',
})

export class NewTransferComponent extends BaseComponent {
    public EntityPM: AccountingTransferHeaderPM = null;
    public ObjectTableName: string = "AccountingTransferHeader";
    public DataContext = this;
    public TransferTypeCode: string = null;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: NewTransferLine[] = [];
    public SelectedItem: NewTransferLine = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new AccountingTransferHeaderPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.UserId = SessionLocator.LoggedUserId;
        this.EntityPM.TransferDate = DateTool.GetCurrentDateTimeAsUtc();
        this.Listen();
    }

    private Listen() {
        this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "TransferExportFirstTime") {
                this.IsFirstTimeLoading = true;
                this.LoadData()
            }
        });
    }

    SetWindowArgs(transferTypeCode: string) {
        this.TransferTypeCode = transferTypeCode;
        this.EntityPM.AccountingTransferTypeCode = transferTypeCode;
        this.SetLabels();
        this.LoadData();
    }

    public HeaderLabel_Date: string = null;
    public HeaderLabel_Number: string = null;
    public HeaderLabel_Partner: string = null;
    public HeaderLabel_Status: string = null;
    public HeaderLabel_Amount: string = null;
    SetLabels() {
        switch (this.TransferTypeCode) {
            case "ARIN": {
                this.HeaderLabel_Date = TextCodeTranslator.Translate("ARInvoice.CH.InvoiceDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator.Translate("ARInvoice.CH.InvoiceNumberListLable");
                this.HeaderLabel_Partner = TextCodeTranslator.Translate("ARInvoice.CH.BillToNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator.Translate("ARInvoice.CH.StatusNameRateListLable");
                this.HeaderLabel_Amount = TextCodeTranslator.Translate("ARInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }

            case "APIN": {
                this.HeaderLabel_Date = TextCodeTranslator.Translate("APInvoice.CH.InvoiceDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator.Translate("APInvoice.CH.InvoiceNumberListLable");
                this.HeaderLabel_Partner = TextCodeTranslator.Translate("APInvoice.CH.VendorNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator.Translate("APInvoice.CH.StatusNameListLable");
                this.HeaderLabel_Amount = TextCodeTranslator.Translate("APInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }

            case "ARPA": {
                this.HeaderLabel_Date = TextCodeTranslator.Translate("ARPayment.CH.RegisterDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator.Translate("ARPayment.CH.PaymentNoListLable");
                this.HeaderLabel_Partner = TextCodeTranslator.Translate("ARPayment.CH.BillToNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator.Translate("ARPayment.CH.StatusNameListLable");
                this.HeaderLabel_Amount = TextCodeTranslator.Translate("ARPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }

            case "APPA": {
                this.HeaderLabel_Date = TextCodeTranslator.Translate("APPayment.CH.RegisterDateListLable");
                this.HeaderLabel_Number = TextCodeTranslator.Translate("APPayment.CH.PaymentNoListLable");
                this.HeaderLabel_Partner = TextCodeTranslator.Translate("APPayment.CH.VendorNameListLable");
                this.HeaderLabel_Status = TextCodeTranslator.Translate("APPayment.CH.StatusNameListLable");
                this.HeaderLabel_Amount = TextCodeTranslator.Translate("APPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    // Filters
    private isAllChecked: boolean = true;
    get IsAllChecked() { return this.isAllChecked; }
    set IsAllChecked(value: boolean) {
        if (this.isAllChecked != value) {
            this.isAllChecked = value;

            this.ItemsSource.forEach(item => {
                item.IsChecked = value;
            });

            this.OnLinesSelected();
        }
    }

    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.LoadData();
        }
    }

    private toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.LoadData();
        }
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadData();
        }
    }

    private entityListService: any = null;
    LoadData() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;

        switch (this.TransferTypeCode) {
            case "ARIN": {
                filters.addAdditionalFilter("StatusCode", "AD,PP,PD,AC,AR", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");
                filters.addAdditionalFilter("IsConstituentInvoice", false, null, null, "Equals", false, false, false, "Boolean");

                this.AppendDateFilter(filters, "InvoiceDate");

                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyInvoices", this.SearchText, null, null, "Equals", true, false, false, "string");
                }

                if (this.entityListService == null) {
                    this.entityListService = new ARInvoiceListService();
                }

                this.entityListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                    this.BuildItemsSource(myResponse);
                });

                break;
            }

            case "APIN": {
                filters.addAdditionalFilter("StatusCode", "AD,PP,PD", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");

                this.AppendDateFilter(filters, "InvoiceDate");

                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyInvoices", this.SearchText, null, null, "Equals", true, false, false, "string");
                }

                if (this.entityListService == null) {
                    this.entityListService = new APInvoiceListService();
                }

                this.entityListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                    this.BuildItemsSource(myResponse);
                });

                break;
            }

            case "ARPA": {
                filters.addAdditionalFilter("StatusCode", "AD,CL", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");

                this.AppendDateFilter(filters, "RegisterDate");

                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyPayments", this.SearchText, null, null, "Equals", true, false, false, "string");
                }

                if (this.entityListService == null) {
                    this.entityListService = new ARPaymentListService();
                }

                this.entityListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                    this.BuildItemsSource(myResponse);
                });

                break;
            }

            case "APPA": {
                filters.addAdditionalFilter("StatusCode", "AD,CL", null, null, "InList", false, true, false, "string");
                filters.addAdditionalFilter("TransferStatusCode", "RD", null, null, "Equals", false, true, false, "string");

                this.AppendDateFilter(filters, "RegisterDate");

                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("SearchReadyPayments", this.SearchText, null, null, "Equals", true, false, false, "string");
                }

                if (this.entityListService == null) {
                    this.entityListService = new APPaymentListService();
                }

                this.entityListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                    this.BuildItemsSource(myResponse);
                });

                break;
            }
        }
    }
    AppendDateFilter(filters: ApiQueryFilters, myFilterField: string) {
        if (!AppTool.IsNullOrEmpty(this.FromDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
            filters.addAdditionalFilter(myFilterField, this.FromDate, this.ToDate, null, "Between", false, true, false, "date");
        }

        else if (!AppTool.IsNullOrEmpty(this.FromDate)) {
            filters.addAdditionalFilter(myFilterField, this.FromDate, null, null, "GreaterThanOrEqual", false, true, false, "date");
        }

        else if (!AppTool.IsNullOrEmpty(this.ToDate)) {
            filters.addAdditionalFilter(myFilterField, this.ToDate, null, null, "LessThanOrEqual", false, true, false, "date");
        }
    }
    BuildItemsSource(myResponse: ServiceResponse) {
        this.ItemsSource = [];
        this.SelectedItem = null;
        var myResultList: NewTransferLine[] = [];

        if (!myResponse.HasError) {
            if (myResponse.Result.length > 0) {
                switch (this.TransferTypeCode) {
                    case "ARIN": {

                        myResponse.Result.forEach((item: ARInvoiceList) => {
                            var myResultItem = new NewTransferLine(this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.InvoiceDate;
                            myResultItem.DateTicks = DateTool.GetDateParts(item.InvoiceDate).DateTicks;
                            myResultItem.Reference = item.InvoiceNumber
                            myResultItem.Partner = item.BillToName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInInvoiceCurrency;
                            myResultItem.CurrencyCode = item.InvoiceCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });

                        break;
                    }

                    case "APIN": {

                        myResponse.Result.forEach((item: APInvoiceList) => {
                            var myResultItem = new NewTransferLine(this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.InvoiceDate;
                            myResultItem.DateTicks = DateTool.GetDateParts(item.InvoiceDate).DateTicks;
                            myResultItem.Reference = item.InvoiceNumber
                            myResultItem.Partner = item.VendorName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInInvoiceCurrency;
                            myResultItem.CurrencyCode = item.InvoiceCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });

                        break;
                    }

                    case "ARPA": {

                        myResponse.Result.forEach((item: ARPaymentList) => {
                            var myResultItem = new NewTransferLine(this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.RegisterDate;
                            myResultItem.DateTicks = DateTool.GetDateParts(item.RegisterDate).DateTicks;
                            myResultItem.Reference = item.PaymentNo
                            myResultItem.Partner = item.BillToName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInPaymentCurrency;
                            myResultItem.CurrencyCode = item.PaymentCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });

                        break;
                    }

                    case "APPA": {

                        myResponse.Result.forEach((item: APPaymentList) => {
                            var myResultItem = new NewTransferLine(this);
                            myResultItem.Id = item.Id;
                            myResultItem.Date = item.RegisterDate;
                            myResultItem.DateTicks = DateTool.GetDateParts(item.RegisterDate).DateTicks;
                            myResultItem.Reference = item.PaymentNo
                            myResultItem.Partner = item.VendorName;
                            myResultItem.Status = item.StatusName;
                            myResultItem.Amount = item.AmountInPaymentCurrency;
                            myResultItem.CurrencyCode = item.PaymentCurrencyCode;
                            myResultItem.TransferError = item.TransferError;
                            myResultItem.ReadyForTransfer = item.TransferStatusCode == "RD" ? true : false;
                            myResultList.push(myResultItem);
                        });

                        break;
                    }
                }
            }
        }

        this.ItemsSource = myResultList.sort(function (a, b) { return a.DateTicks == b.DateTicks ? 0 : a.DateTicks < b.DateTicks ? -1 : 1; });

        if (this.CheckAllItemsAgain) {
            this.ItemsSource.forEach(item => {
                item.IsChecked = true;
            });

            this.CheckAllItemsAgain = false;
        }

        this.IsFirstTimeLoading = false;
        this.OnLinesSelected();
    }

    public SelectedCount: number = 0;
    public ExportButtonIsEnabled: boolean = false;
    public IsFirstTimeLoading: boolean = true;
    OnLinesSelected() {
        this.SelectedCount = this.ItemsSource.filter(f => f.IsChecked == true).length;
        this.ExportButtonIsEnabled = this.SelectedCount > 0 ? true : false;
    }

    ExportButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.SelectedCount = this.ItemsSource.filter(f => f.IsChecked == true).length;

        if (this.SelectedCount == 0) {
            errors.push("You must select 1 line at least");
        }

        else if (this.SelectedCount > 100) {
            errors.push("You must select 100 line max");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var logWindowTitle: string = null;
            switch (this.TransferTypeCode) {
                case "ARIN": { logWindowTitle = "Exporting AR Invoice Transfer"; break; }
                case "APIN": { logWindowTitle = "Exporting AP Invoice Transfer"; break; }
                case "ARPA": { logWindowTitle = "Exporting AR Payment Transfer"; break; }
                case "APPA": { logWindowTitle = "Exporting AP Payment Transfer"; break; }
            }
            
            var logWindow = new LogitudeWindow();
            logWindow.Title = logWindowTitle;
            logWindow.Width = 500;
            logWindow.Height = 200;
            logWindow.Show('./Invoice/Components/Workspaces/Windows/ExportTransferComponent');
            logWindow.ComponentLoaded.subscribe(comp => {

                this.ItemsSource.filter(f => f.IsChecked == true).forEach(item => {
                    var TransferLinePM = new AccountingTransferLinePM(null);
                    TransferLinePM.Tenant = SessionLocator.Tenant;
                    TransferLinePM.EntityId = item.Id;
                    TransferLinePM.EntityReference = item.Reference;
                    this.EntityPM.AddAccountingTransferLinePM(TransferLinePM);
                });

                comp.Export(this.EntityPM);
            });            
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private CheckAllItemsAgain: boolean = false;
    MarkAsBlockedClicked(itemId: string) {
        this.CurrentSession.StartBusyIndicatorSaving();

        var invoiceService: InvoiceDomainService = new InvoiceDomainService();
        invoiceService.MarkEntityAsBlocked(this.TransferTypeCode, itemId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LoadData();
                this.CheckAllItemsAgain = true;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
}

export class NewTransferLine {
    constructor(private fatherComponent: NewTransferComponent) {
        if (fatherComponent.IsFirstTimeLoading) {
            this.isChecked = true;
        }
    }

    public Id: string;
    public Date: Date;
    public DateTicks: number;
    public Reference: string;
    public Partner: string;
    public Status: string;
    public Amount: number;
    public CurrencyCode: string;
    public TransferError: string;
    public ReadyForTransfer: boolean;

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            this.fatherComponent.OnLinesSelected();            
        }
    }
}
