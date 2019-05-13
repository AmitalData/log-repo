import {Component, ViewChildren, QueryList} from '@angular/core';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
declare var window: any;

@Component({
    selector: 'FullAccountingComponent',
    moduleId: module.id,
    templateUrl: './AccountingWorkspaceComponent.html',
})

export class AccountingWorkspaceComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    public isRTL: boolean = false;
    public IsMainTabVisibile: boolean = false;
    public IsReceivablesTabVisibile: boolean = false;
    public IsPayablesTabVisibile: boolean = false;
    public IsBanksTabVisibile: boolean = false;
    public IsJournalTabVisibile: boolean = false;
    public IsGLAccountsTabVisibile: boolean = false;
    public IsMiscTabVisibile: boolean = false;

    constructor(private _entityResourceService: EntityResourceService) {
        this.RunComponent();
        this.GetResources();
        this.CheckFeatures();
    }

    private GetResources() {
        this._entityResourceService.getEntityResourceByTableName("BankDeposit").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("BankDepositLine").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ARPaymentCheque").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("APPayment").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("CashBook").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("CashBookLine").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPageLine").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("BankAccount").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("AccountingPeriod").subscribe((response: any) => { });
    }

    CheckFeatures() {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        var table = window.ObjectTables.filter(d => d.Name === 'General')[0];
        var mainTabFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCMAIN") && f.ObjectTableId == table.Id)[0];
        if (mainTabFeature) {
            this.IsMainTabVisibile = true;
        }
        var ReceivablesTabFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCReceivables") && f.ObjectTableId == table.Id)[0];
        if (ReceivablesTabFeature) {
            this.IsReceivablesTabVisibile = true;
        }
        var PayablesTabFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCPayables") && f.ObjectTableId == table.Id)[0];
        if (PayablesTabFeature) {
            this.IsPayablesTabVisibile = true;
        }
        var BanksTabFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCBanks") && f.ObjectTableId == table.Id)[0];
        if (BanksTabFeature) {
            this.IsBanksTabVisibile = true;
        }
        var journalTabFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCJORN") && f.ObjectTableId == table.Id)[0];
        if (journalTabFeature) {
            this.IsJournalTabVisibile = true;
        }
        var GLAccountsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCGLAccounts") && f.ObjectTableId == table.Id)[0];
        if (GLAccountsTabFeature) {
            this.IsGLAccountsTabVisibile = true;
        }
        var MiscTabFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCMisc") && f.ObjectTableId == table.Id)[0];
        if (MiscTabFeature) {
            this.IsMiscTabVisibile = true;
        }
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;

                this.InitSelectedTab();

            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    InitSelectedTab(){

        // if (this.IsMainTabVisibile) {
        //     this.SelectedItem = "Main";
        // }
        // else
        if (this.IsReceivablesTabVisibile) {
            this.SelectedItem = "RCV";

        }
        else if (this.IsPayablesTabVisibile) {
            this.SelectedItem = "PAY";

        }
        else if (this.IsBanksTabVisibile) {
            this.SelectedItem = "BNKS";

        }
        else if (this.IsJournalTabVisibile) {
            this.SelectedItem = "JORN";

        }
        else if (this.IsGLAccountsTabVisibile) {
            this.SelectedItem = "GLAccounts";

        }
        else if (this.IsMiscTabVisibile) {
            this.SelectedItem = "MISC";

        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }

    private Page_GLAccounts: any = null;
    private Page_Main: any = null;
    private Page_Journals: any = null;
    private Page_Receivable: any = null;
    private Page_Payable: any = null;
    private Page_Banks: any = null;
    private Page_Misc: any = null;
    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null) {

                    switch (this.SelectedItem) {

                        case "Main": {
                            if (this.Page_Main == null) {
                                this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe((response: any) => {
                                    SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Main/MainPageComponent", myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_Main = cmpRef.instance;
                                            this.Page_Main.InitComponent();
                                        });
                                });
                            }
                            break;
                        }
                        case "JORN": {
                            if (this.Page_Journals == null) {
                                SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Journal/JournalPageComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_Journals = cmpRef.instance;
                                        this.Page_Journals.InitComponent();
                                    });
                            }
                            break;
                        }
                        case "RCV": {
                            if (this.Page_Receivable == null) {
                                this._entityResourceService.getEntityResourceByTableName("ARInvoice", 0).subscribe((response: any) => {
                                    this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe((response: any) => {

                                        SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Receivable/ReceivablePageComponent", myLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                this.Page_Receivable = cmpRef.instance;
                                                this.Page_Receivable.InitComponent();
                                            });
                                    });
                                });
                            }
                            break;
                        }
                        case "PAY": {
                            if (this.Page_Payable == null) {
                                this._entityResourceService.getEntityResourceByTableName("APInvoice", 0).subscribe((response: any) => {
                                SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Payable/PayablePageComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_Payable = cmpRef.instance;
                                        this.Page_Payable.InitComponent();
                                        });
                                });
                            }
                            break;
                        }
                        case "BNKS": {
                            if (this.Page_Banks == null) {
                                SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Banks/BanksPageComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_Banks = cmpRef.instance;
                                        this.Page_Banks.InitComponent();
                                    });
                            }
                            break;
                        }
                        case "MISC": {
                            if (this.Page_Misc == null) {
                                SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/Misc/MiscPageComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_Misc = cmpRef.instance;
                                        this.Page_Misc.InitComponent();
                                    });
                            }
                            break;
                        }
                        case "GLAccounts": {
                            if (this.Page_GLAccounts == null) {
                                this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe((response: any) => {
                                    SessionLocator.DynamicLoader.Load("./Accounting/Components/Workspaces/GLAccounts/GLAccountsPageComponent", myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_GLAccounts = cmpRef.instance;
                                            this.Page_GLAccounts.InitComponent();
                                        });
                                });
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

}
