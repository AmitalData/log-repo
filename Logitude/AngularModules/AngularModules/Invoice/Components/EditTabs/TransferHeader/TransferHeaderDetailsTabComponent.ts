import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AccountingTransferHeaderPM} from '../../../EntityPMs/AccountingTransferHeaderPM';
import {AccountingTransferLinePM} from '../../../EntityPMs/AccountingTransferLinePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {InvoiceDomainService} from '../../../Services/InvoiceDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './TransferHeaderDetailsTabComponent.html',
})

export class TransferHeaderDetailsTabComponent extends BaseComponent {
    public EntityPM: AccountingTransferHeaderPM = null;
    public ObjectTableName = "AccountingTransferHeader";
    public DataContext = this;
    public ItemsSource: AccountingTransferLinePM[] = [];
    public SelectedItem: AccountingTransferLinePM = null;
    private invoiceDomainService: InvoiceDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ItemsSource = this.EntityPM.TransferLines;
        this.invoiceDomainService = new InvoiceDomainService();
        this.SetColumnHeaders();
    }

    public ColumnHeader_Date: string = null;
    public ColumnHeader_Number: string = null;
    public ColumnHeader_Partner: string = null;
    public ColumnHeader_Status: string = null;
    public ColumnHeader_Amount: string = null;
    SetColumnHeaders() {
        switch (this.EntityPM.AccountingTransferTypeCode) {
            case "ARIN": {
                this.ColumnHeader_Date = TextCodeTranslator.Translate("ARInvoice.CH.InvoiceDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator.Translate("ARInvoice.CH.InvoiceNumberListLable");
                this.ColumnHeader_Partner = TextCodeTranslator.Translate("ARInvoice.CH.BillToNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator.Translate("ARInvoice.CH.StatusNameRateListLable");
                this.ColumnHeader_Amount = TextCodeTranslator.Translate("ARInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }

            case "APIN": {
                this.ColumnHeader_Date = TextCodeTranslator.Translate("APInvoice.CH.InvoiceDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator.Translate("APInvoice.CH.InvoiceNumberListLable");
                this.ColumnHeader_Partner = TextCodeTranslator.Translate("APInvoice.CH.VendorNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator.Translate("APInvoice.CH.StatusNameListLable");
                this.ColumnHeader_Amount = TextCodeTranslator.Translate("APInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }

            case "ARPA": {
                this.ColumnHeader_Date = TextCodeTranslator.Translate("ARPayment.CH.RegisterDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator.Translate("ARPayment.CH.PaymentNoListLable");
                this.ColumnHeader_Partner = TextCodeTranslator.Translate("ARPayment.CH.BillToNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator.Translate("ARPayment.CH.StatusNameListLable");
                this.ColumnHeader_Amount = TextCodeTranslator.Translate("ARPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }

            case "APPA": {
                this.ColumnHeader_Date = TextCodeTranslator.Translate("APPayment.CH.RegisterDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator.Translate("APPayment.CH.PaymentNoListLable");
                this.ColumnHeader_Partner = TextCodeTranslator.Translate("APPayment.CH.VendorNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator.Translate("APPayment.CH.StatusNameListLable");
                this.ColumnHeader_Amount = TextCodeTranslator.Translate("APPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }
        }
    }

    RebuildClicked() {
        this.CurrentSession.StartBusyIndicator("Rebuilding ...");

        this.invoiceDomainService.RebuildTransferFile(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
        });
    }

    DownloadClicked() {
        DownloadManager.DownloadTransferHeaderFile(this.EntityPM.FileName);
    }
}
