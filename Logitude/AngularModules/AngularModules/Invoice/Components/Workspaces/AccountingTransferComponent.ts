import {Component} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {InvoiceDomainService} from '../../Services/InvoiceDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'AccountingTransferComponent',
    moduleId: module.id,
    templateUrl: './AccountingTransferComponent.html',
})

export class AccountingTransferComponent   { 
    public IsResourcesReady: boolean = false;
    private invoiceDomainService: InvoiceDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.invoiceDomainService = new InvoiceDomainService();
        entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe(res1 => {
            entityResourceService.getEntityResourceByTableName("APInvoice").subscribe(res2 => {
                entityResourceService.getEntityResourceByTableName("ARPayment").subscribe(res3 => {
                    entityResourceService.getEntityResourceByTableName("APPayment").subscribe(res4 => {
                    entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe(res5 => {
                        entityResourceService.getEntityResourceByTableName("AccountingTransferHeader").subscribe(res6 => {
                            this.InitializeComponent();
                            this.IsResourcesReady = true;
                            this.LoadDataCount();
                            this.Listen();
                        });
                    });
                });
                });
            });
        });
    }

    private Listen() {
        this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "Accounting_T") {
                this.LoadDataCount();
            }

            if (s == "RefreshTransferComponent") {
                this.InitializeComponent();
            }
        });
    }
    
    public AccountingSystemCode: string = null;
    public AccountingSystemName: string = null;
    public HasTransferFeature: boolean = false;
    public IsARInvoicesEnabled: boolean = false;
    public IsAPInvoicesEnabled: boolean = false;
    public IsARPaymentsEnabled: boolean = false;
    public IsAPPaymentsEnabled: boolean = false;
    public IsAccountingSettingEnabled: boolean = false;    
    InitializeComponent() {
        this.HasTransferFeature = false;
        this.IsARInvoicesEnabled = false;
        this.IsAPInvoicesEnabled = false;
        this.IsARPaymentsEnabled = false;
        this.IsAPPaymentsEnabled = false;
        this.IsAccountingSettingEnabled = false;

        this.AccountingSystemCode = SessionLocator.AccountingSettingPM.AccountingSystemCode;

        if (FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGTRANSFER")) {
            this.HasTransferFeature = true;
        }

        if (SessionLocator.AccountingSystemPM) {
            this.AccountingSystemName = SessionLocator.AccountingSystemPM.Name;
        }

        if (this.AccountingSystemCode != "NO") {
            if (!AppTool.IsNullOrEmpty(SessionLocator.AccountingSettingPM.ReceivableVATableTempCard)
                && !AppTool.IsNullOrEmpty(SessionLocator.AccountingSettingPM.ReceivableVATExemptTempCard)
                && !AppTool.IsNullOrEmpty(SessionLocator.AccountingSettingPM.PayableVATExemptTempCard)
                && !AppTool.IsNullOrEmpty(SessionLocator.AccountingSettingPM.PayableVATExemptTempCard)) {
                this.IsAccountingSettingEnabled = true;
            }

            else if (this.AccountingSystemCode == "QB" || this.AccountingSystemCode == "GI" || this.AccountingSystemCode == "AI") {
                this.IsAccountingSettingEnabled = true;
            }
        }
        
        if (this.IsAccountingSettingEnabled) {
            if (FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGTRANSFER")) {
                if (SessionLocator.AccountingSystemPM) {
                    if (SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer) {
                        if (SessionLocator.AccountingSettingPM.IsARInvoicesTransferEnabled) {
                            this.IsARInvoicesEnabled = true;
                        }
                    }

                    if (SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer) {
                        if (SessionLocator.AccountingSettingPM.IsAPInvoicesTransferEnabled) {
                            this.IsAPInvoicesEnabled = true;
                        }
                    }

                    if (SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer) {
                        if (SessionLocator.AccountingSettingPM.IsARPaymentsTransferEnabled) {
                            this.IsARPaymentsEnabled = true;
                        }
                    } 

                    if (SessionLocator.AccountingSystemPM.AllowAPPaymentsTransfer) {
                        if (SessionLocator.AccountingSettingPM.IsAPPaymentsTransferEnabled) {
                            this.IsAPPaymentsEnabled = true;
                        }
                    } 
                }
            }
        }

        this.SetQueriesUIProperties();
    }

    public IsVisible_ARInvoice: boolean = false;
    public IsVisible_NewTransfer: boolean = false;
    public IsVisible_ARInvoice_NotReady: boolean = false;
    public IsVisible_ARInvoice_MarkedAsBlocked: boolean = false;
    public IsVisible_ARInvoice_ErrorInTransfer: boolean = false;
    public IsVisible_ARInvoice_TransferHistory: boolean = false;
    public IsVisible_ARInvoice_RecalculateExternals: boolean = false;
    //-----
    public IsVisible_APInvoice: boolean = false;
    public IsVisible_APInvoice_NotReady: boolean = false;
    public IsVisible_APInvoice_MarkedAsBlocked: boolean = false;
    public IsVisible_APInvoice_ErrorInTransfer: boolean = false;
    public IsVisible_APInvoice_TransferHistory: boolean = false;
    public IsVisible_APInvoice_RecalculateExternals: boolean = false;
    //-----
    public IsVisible_ARPayment: boolean = false;
    public IsVisible_ARPayment_NotReady: boolean = false;
    public IsVisible_ARPayment_MarkedAsBlocked: boolean = false;
    public IsVisible_ARPayment_TransferHistory: boolean = false;
    public IsVisible_ARPayment_RecalculateExternals: boolean = false;
    //-----
    public IsVisible_APPayment: boolean = false;
    public IsVisible_APPayment_NotReady: boolean = false;
    public IsVisible_APPayment_MarkedAsBlocked: boolean = false;
    public IsVisible_APPayment_TransferHistory: boolean = false;
    public IsVisible_APPayment_RecalculateExternals: boolean = false;
    //------
    public IsTaxesVisible: boolean = false;
    SetQueriesUIProperties() {
        this.IsVisible_ARInvoice = false;
        this.IsVisible_APInvoice = false;
        this.IsVisible_ARPayment = false;
        this.IsVisible_APPayment = false;
        this.IsVisible_NewTransfer = false;
        this.IsVisible_ARInvoice_TransferHistory = false;
        this.IsVisible_APInvoice_TransferHistory = false;
        this.IsVisible_ARPayment_TransferHistory = false;
        this.IsVisible_APPayment_TransferHistory = false;
        this.IsTaxesVisible = false;

        if (this.AccountingSystemCode != "QB") {

            if (FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "NEW")) {
                this.IsVisible_NewTransfer = true;
                this.IsVisible_ARInvoice = true;
                this.IsVisible_APInvoice = true;
                this.IsVisible_ARPayment = true;
                this.IsVisible_APPayment = true;
            }

            if (FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "ARInvoiceTransferHistory")) {
                this.IsVisible_ARInvoice_TransferHistory = true;
                this.IsVisible_ARInvoice = true;
            }

            if (FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "APInvoiceTransferHistory")) {
                this.IsVisible_APInvoice_TransferHistory = true;
                this.IsVisible_APInvoice = true;
            }

            if (FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "ARPaymentTransferHistory")) {
                this.IsVisible_ARPayment_TransferHistory = true;
                this.IsVisible_ARPayment = true;
            }

            if (FeatureLocator.HasFeaturePermession("AccountingTransferHeader", "APPaymentTransferHistory")) {
                this.IsVisible_APPayment_TransferHistory = true;
                this.IsVisible_APPayment = true;
            }
        }
        
        // ARInvoice
        this.IsVisible_ARInvoice_NotReady = false;
        this.IsVisible_ARInvoice_MarkedAsBlocked = false;
        this.IsVisible_ARInvoice_ErrorInTransfer = false;
        this.IsVisible_ARInvoice_RecalculateExternals = false;
        if (FeatureLocator.HasFeaturePermession("ARInvoice", "NOTREADYINVOICES")) {
            this.IsVisible_ARInvoice_NotReady = true;
            this.IsVisible_ARInvoice = true;
        }
        if (FeatureLocator.HasFeaturePermession("ARInvoice", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_ARInvoice_MarkedAsBlocked = true;
            this.IsVisible_ARInvoice = true;
        }
        if (FeatureLocator.HasFeaturePermession("ARInvoice", "ERRORINTRANSFERINVOICES")) {
            this.IsVisible_ARInvoice_ErrorInTransfer = true;
            this.IsVisible_ARInvoice = true;
        }
        if (FeatureLocator.HasFeaturePermession("ARInvoice", "RecalculateExternals")) {
            this.IsVisible_ARInvoice_RecalculateExternals = true;
            this.IsVisible_ARInvoice = true;
        }

        // APInvoice
        this.IsVisible_APInvoice_NotReady = false;
        this.IsVisible_APInvoice_MarkedAsBlocked = false;
        this.IsVisible_APInvoice_ErrorInTransfer = false;
        this.IsVisible_APInvoice_RecalculateExternals = false;
        if (FeatureLocator.HasFeaturePermession("APInvoice", "NOTREADYINVOICES")) {
            this.IsVisible_APInvoice_NotReady = true;
            this.IsVisible_APInvoice = true;
        }
        if (FeatureLocator.HasFeaturePermession("APInvoice", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_APInvoice_MarkedAsBlocked = true;
            this.IsVisible_APInvoice = true;
        }
        if (FeatureLocator.HasFeaturePermession("APInvoice", "ERRORINTRANSFERINVOICES")) {
            this.IsVisible_APInvoice_ErrorInTransfer = true;
            this.IsVisible_APInvoice = true;
        }
        if (FeatureLocator.HasFeaturePermession("APInvoice", "RecalculateExternals")) {
            this.IsVisible_APInvoice_RecalculateExternals = true;
            this.IsVisible_APInvoice = true;
        }

        // ARPayment
        this.IsVisible_ARPayment_NotReady = false;
        this.IsVisible_ARPayment_MarkedAsBlocked = false;
        this.IsVisible_ARPayment_RecalculateExternals = false;
        if (FeatureLocator.HasFeaturePermession("ARPayment", "NOTREADYPAYMENTS")) {
            this.IsVisible_ARPayment_NotReady = true;
            this.IsVisible_ARPayment = true;
        }
        if (FeatureLocator.HasFeaturePermession("ARPayment", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_ARPayment_MarkedAsBlocked = true;
            this.IsVisible_ARPayment = true;
        }
        if (FeatureLocator.HasFeaturePermession("ARPayment", "RecalculateExternals")) {
            this.IsVisible_ARPayment_RecalculateExternals = true;
            this.IsVisible_ARPayment = true;
        }

        // APPayment
        this.IsVisible_APPayment_NotReady = false;
        this.IsVisible_APPayment_MarkedAsBlocked = false;
        this.IsVisible_APPayment_RecalculateExternals = false;
        if (FeatureLocator.HasFeaturePermession("APPayment", "NOTREADYPAYMENTS")) {
            this.IsVisible_APPayment_NotReady = true;
            this.IsVisible_APPayment = true;
        }
        if (FeatureLocator.HasFeaturePermession("APPayment", "MARKEDASBLOCKEDFORTRANSFER")) {
            this.IsVisible_APPayment_MarkedAsBlocked = true;
            this.IsVisible_APPayment = true;
        }
        if (FeatureLocator.HasFeaturePermession("APPayment", "RecalculateExternals")) {
            this.IsVisible_APPayment_RecalculateExternals = true;
            this.IsVisible_APPayment = true;
        }

        //Open format
        if (SessionLocator.TenantPM.CountryCode == "IL") {
            this.IsTaxesVisible = true;
        }
    }

    public ARInvoicesNotReadyCount: string = "0";
    public ARInvoicesDontTransferCount: string = "0";
    public ARInvoicesErrorInTransferCount: string = "0";
    //-----
    public APInvoicesNotReadyCount: string = "0";
    public APInvoicesDontTransferCount: string = "0";
    public APInvoicesErrorInTransferCount: string = "0";
    //-----
    public ARPaymentsNotReadyCount: string = "0";
    public ARPaymentsDontTransferCount: string = "0";
    public ARPaymentsErrorInTransferCount: string = "0";
    //-----
    public APPaymentsNotReadyCount: string = "0";
    public APPaymentsDontTransferCount: string = "0";
    public APPaymentsErrorInTransferCount: string = "0";
    LoadDataCount() {
        if (this.IsAccountingSettingEnabled) {
            this.invoiceDomainService.GetAccountingTransferSummary().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        this.ARInvoicesNotReadyCount = myResponse.Result.ARInvoicesNotReadyCount > 1000 ? "1000+" : myResponse.Result.ARInvoicesNotReadyCount + "";
                        this.ARInvoicesDontTransferCount = myResponse.Result.ARInvoicesDontTransferCount > 1000 ? "1000+" : myResponse.Result.ARInvoicesDontTransferCount + "";
                        this.ARInvoicesErrorInTransferCount = myResponse.Result.ARInvoicesErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.ARInvoicesErrorInTransferCount + "";
                        //-----
                        this.APInvoicesNotReadyCount = myResponse.Result.APInvoicesNotReadyCount > 1000 ? "1000+" : myResponse.Result.APInvoicesNotReadyCount + "";
                        this.APInvoicesDontTransferCount = myResponse.Result.APInvoicesDontTransferCount > 1000 ? "1000+" : myResponse.Result.APInvoicesDontTransferCount + "";
                        this.APInvoicesErrorInTransferCount = myResponse.Result.APInvoicesErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.APInvoicesErrorInTransferCount + "";
                        //-----
                        this.ARPaymentsNotReadyCount = myResponse.Result.ARPaymentsNotReadyCount > 1000 ? "1000+" : myResponse.Result.ARPaymentsNotReadyCount + "";
                        this.ARPaymentsDontTransferCount = myResponse.Result.ARPaymentsDontTransferCount > 1000 ? "1000+" : myResponse.Result.ARPaymentsDontTransferCount + "";
                        this.ARPaymentsErrorInTransferCount = myResponse.Result.ARPaymentsErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.ARPaymentsErrorInTransferCount + "";
                        //-----
                        this.APPaymentsNotReadyCount = myResponse.Result.APPaymentsNotReadyCount > 1000 ? "1000+" : myResponse.Result.APPaymentsNotReadyCount + "";
                        this.APPaymentsDontTransferCount = myResponse.Result.APPaymentsDontTransferCount > 1000 ? "1000+" : myResponse.Result.APPaymentsDontTransferCount + "";
                        this.APPaymentsErrorInTransferCount = myResponse.Result.APPaymentsErrorInTransferCount > 1000 ? "1000+" : myResponse.Result.APPaymentsErrorInTransferCount + "";
                    }
                }
            });
        }
    }

    NewTransferClicked(args: string) {
        if (args) {
            var logWindowTitle: string = null;
            var transferTypeCode: string = null;

            switch (args) {
                case "ARInvoice": {
                    transferTypeCode = "ARIN";
                    logWindowTitle = "New AR Invoice Accounting Transfer";
                    break;
                }
                case "APInvoice": {
                    transferTypeCode = "APIN";
                    logWindowTitle = "New AP Invoice Accounting Transfer";
                    break;
                }
                case "ARPayment": {
                    transferTypeCode = "ARPA";
                    logWindowTitle = "New AR Payment Accounting Transfer";
                    break;
                }
                case "APPayment": {
                    transferTypeCode = "APPA";
                    logWindowTitle = "New AP Payment Accounting Transfer";
                    break;
                }
            }

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;            
            logWindow.Title = logWindowTitle;
            logWindow.WindowArgs = transferTypeCode;
            logWindow.Show('./Invoice/Components/Workspaces/Windows/NewTransferComponent');
        }
    }
    RecalculateExternalClicked(args: string) {
        if (args) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 250;
            logWindow.WindowArgs = args;
            logWindow.Title = "Recalculate " + args + " Invoices external IDs";
            logWindow.Show('./Invoice/Components/Workspaces/Windows/RecalculateExternalsComponent');            
        }
    }
    ViewQueryClicked(args: string) {
        if (args) {
            var argsSplit: string[] = args.split(':');
            var objectTableName = argsSplit[0];
            var myQueryCode = argsSplit[1];

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("General.MH.Accounting");

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    this.CurrentSession.AddMenuReference(cmpRef);

                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);

                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.LoadDataCount();
                    });
                });
        }
    }   
    OpenAccountingSystem() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Accounting Transfer Settings";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/TransferSettingsComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.InitializeComponent();
            }
        });
    }

    OpenFormatReportClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Dates";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/PrintTaxComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                var msg = new MessageWindow();
                msg.Show("An e-mail that includes two attachments will be sent to you in a few minutes");
            }
        });
    }
}
