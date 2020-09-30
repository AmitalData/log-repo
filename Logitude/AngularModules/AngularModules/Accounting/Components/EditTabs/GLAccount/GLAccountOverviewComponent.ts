import { DateTool } from './../../../../Infrastructure/Tools';
import { CardList } from './../../../../Common/EntityLists/CardList';
import { CardListService } from './../../../../Common/Services/StandardLists/CardListService';
import { CreditLimitSettingPM } from './../../../../Common/EntityPMs/CreditLimitSettingPM';
import { AccountingNotePMService } from './../../../Services/StandardPMs/AccountingNotePMService';
import { AccountingNotePM } from './../../../EntityPMs/AccountingNotePM';
import { MessageWindow } from './../../../../Controls/Windows/MessageWindow';
import { AccountingNoteExtendedListService } from './../../../Services/ExtendedLists/AccountingNoteExtendedListService';
import { AccountingNoteList } from './../../../EntityLists/AccountingNoteList';
import { RegionList } from './../../../../Common/EntityLists/RegionList';
import { AccountingEntityHelper } from './../../../Utilities/AccountingEntityHelper';
import { EntityResourceService } from './../../../../Infrastructure/Services/EntityResourceService';
import { LedgerTransactionList } from './../../../EntityLists/LedgerTransactionList';
import {Component, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import { GLAccountMoreDataList } from '../../../EntityLists/GLAccountMoreDataList';
import {GLAccountValidator} from '../../../Validators/GLAccountValidator';
import { GLAccountMoreDataListService } from '../../../Services/StandardLists/GLAccountMoreDataListService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { GLAccountListService } from '../../../Services/StandardLists/GLAccountListService';
import { GLAccountExtendedListService } from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import { LedgerTransactionExtendedListService } from '../../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import { ReconcileEventManager } from '../../../Utilities/ReconcileEventManager';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { GLAccountSummary } from '../../../DataContracts/AccountingSummery';
import { AgingReportParameters } from '../../../DataContracts/AgingReportParameters';
import { PeriodM } from '../../../DataContracts/PeriodM';
import { GLAccountList } from '../../../EntityLists/GLAccountList';
import { FullAccountingSettingList } from '../../../EntityLists/FullAccountingSettingList';
import { AccountingNoteListService } from '../../../Services/StandardLists/AccountingNoteListService';
import { GLAccountExtendedPMService } from 'Accounting/Services/ExtendedPMs/GLAccountExtendedPMService';
declare var makeAmBarChart;

@Component({

    templateUrl: './GLAccountOverviewComponent.html',
})

export class GLAccountOverviewComponent extends BaseComponent {

    // Const
    public DataContext = this;
    public ObjectTableName = "GLAccount";
    txtcode_Amount: string = TextCodeTranslator.Translate("Accounting.General.O.Amount");
    txtcode_AgingDetails: string = TextCodeTranslator.Translate("GLAccounts.O.AgingDetails");

    // Variables
    public AccountPM: GLAccountPM = null;
    public GLAccountMoreData: GLAccountMoreDataList = null;
    public isRTL: boolean = false;
    public showLocal: boolean = false;
    public isUsedOutside: boolean = false; // when view tab inside customer ..
    public OpenShipments:number=0;
    public CreditLimitAmount:number=0;
    //Services
    _EntityResourceService: EntityResourceService = new EntityResourceService();
    _GLAccountMoreDataListService: GLAccountMoreDataListService = new GLAccountMoreDataListService();
    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    _AccountingNoteExtendedListService: AccountingNoteExtendedListService = new AccountingNoteExtendedListService();
    _AccountingNotePMService: AccountingNotePMService = new AccountingNotePMService();
    _CardListService: CardListService = new CardListService();
    _GLAccountExtendedPMService: GLAccountExtendedPMService = new GLAccountExtendedPMService();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (SessionLocator.LoggedUserPM) this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;

        //Resources // Use Less
        //this._EntityResourceService.getEntityResourceByTableName("AccountingNote").subscribe((response: any) => { });
        //this._EntityResourceService.getEntityResourceByTableName("Reconciliation").subscribe((response: any) => { });
        //this._EntityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) => { });

        // Set Entity
        if(entityArgs && entityArgs.ObjectTableName == "GLAccount")
        {
            this.AccountPM = entityArgs.EntityPM;
            this.LoadAllData();
        }
        else
        {
            this.isUsedOutside = true;
        }

        this.chartId = "CustomerOverview_" + this.CurrentSession.GetChartId();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadAllData();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                        this.LoadAllData();
                    }
                });
            }


            //
            if (this.TabSelectedEvent == null) {
                this.TabSelectedEvent = this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (tabCode == "GAOV") {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadAllData();
                    }
                });
            }
        }
    }

    LoadAllData() {
        this.GetDefaultValues();
        this.GetLastTransactions();
        this.GetAccountingNotes();
        this.LoadChartData();
        this.SetUIProperties();
        this.LoadCreditDetailsData();

    }

    //#region Properties
    //get DisplayNumber() { return this.AccountPM.DisplayNumber; }
    //set DisplayNumber(value: string) {
    //    if (this.AccountPM.DisplayNumber != value) {
    //        this.AccountPM.DisplayNumber = value;
    //    }
    //}
    //#endregion
    TenantCurrency:string;
    accountCardlist: CardList[];
    accountCardnumberLists:string[]=[];
    GLaccountConnectedMoreOneCardText:string =TextCodeTranslator.Translate("GLAccount.O.GLaccountConnectedMoreOneCard"); 
    Connected10CardsText:string =TextCodeTranslator.Translate("GLAccount.O.Connected10Cards"); 

    private LoadExternalTransactionTotal()
    {
        this._GLAccountExtendedListService.GetGLAccountExternalTransactionsTotal(this.AccountPM.Id).subscribe((myResult: any) =>
        {
            console.log("GetAccountOpenTransactionsCount", myResult);
            var result: ServiceResponse = myResult;
            if (!result.HasError) {
                this.externalTransactionsTotal = result.Result || 0;
            }
            else {
            }
        });
    }

    GetDefaultValues() {

        // Get GLAccountMoreData
        this.CurrentSession.StartBusyIndicatorLoading();
        this._GLAccountMoreDataListService.getSingle(this.AccountPM.Id).subscribe((myResult:any) => {
            this.CurrentSession.StopBusyIndicator();
            console.log("_GLAccountMoreDataListService.getSingle", myResult);

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.GLAccountMoreData = mm.Result;
                this.LoadCreditDetailsData();

            }
            else {
            }
        });

        // Get GLAccount Open Transactions Count
        this._GLAccountExtendedListService.GetAccountOpenTransactionsCount(this.AccountPM.Id).subscribe((myResult:any) => {
            console.log("GetAccountOpenTransactionsCount", myResult);
            var result: ServiceResponse = myResult;
            if (!result.HasError)
            {
                this.GLAccountOpenTransactionsCount = result.Result;
            }
            else {
            }
        });


      
            this._GLAccountExtendedPMService.GetConnectedCardsForGLAccount(this.AccountPM.Id).subscribe((myResponse: ServiceResponse) => {
                var connectedCards = myResponse.Result;
                this.accountCardlist = connectedCards;
                if(this.accountCardlist != null && this.accountCardlist.length > 0){
                    var IsAllCardHasCriedtLimitNull:boolean=true;
                    var IsAllCardHasOpenShipmentNull:boolean=true;
                    this.CreditLimitAmount=0;
                    this.OpenShipments = 0;
                  
                    this.accountCardlist.forEach(s=>{
                        this.accountCardnumberLists.push(s.Code);
                        if(s.OpenShipments!=null){
                            this.OpenShipments+=s.OpenShipments;
                            IsAllCardHasOpenShipmentNull=false;
                        }
                        if(s.CreditLimitAmount!=null){
                            IsAllCardHasCriedtLimitNull=false;
                            this.CreditLimitAmount+=s.CreditLimitAmount;
                        }
                    });
                    if(this.accountCardlist.length >= 10){
                        this.GLaccountConnectedMoreOneCardText = this.Connected10CardsText;
                    }
                    else{
                        this.GLaccountConnectedMoreOneCardText+=" ";
                        this.GLaccountConnectedMoreOneCardText+= this.accountCardnumberLists.toString();
                    }
             
                    if(IsAllCardHasCriedtLimitNull){
                        this.CreditLimitAmount=null;
                    }
                    if(IsAllCardHasOpenShipmentNull){
                        this.OpenShipments=null;
                    }
                    this.LoadCreditDetailsData();
                }

            });
    
 

        // Get connect card
        // this._CardListService.getSingle(this.AccountPM.CardId).subscribe((myResult:any) => {
        //     console.log("_CardListService.getSingle", myResult);
        //     var result: ServiceResponse = myResult;
        //     if (!result.HasError)
        //     {
        //         this.accountCardlist = result.Result;
        //         this.LoadCreditDetailsData();

        //     }
        //     else {
        //         console.log("[!] cannot get glaccount card");

        //     }
        // });


        // Get tenant currency
        this.TenantCurrency = SessionLocator.TenantPM.CurrencyCode;

    }

    SetUIProperties() {
        //if (!this.AccountPM || this.DisableGLAccount)
        //    return;

        //if (this.AccountPM.IsMultiCurrency) {
        //    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        //}
        //if (this.AccountPM.ChartOfAccountsTypeCode) {
        //    this.ChartOfAccountsTypeCode = this.AccountPM.ChartOfAccountsTypeCode;
        //}
        //if (this.AccountPM.ChartOfAccountsId) {
        //    this.ChartOfAccountsId = this.AccountPM.ChartOfAccountsId;
        //    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "");
        //}

    }

    //#region Balance Section
    GLAccountOpenTransactionsCount: number = 0.0;

    private timerToken: any;
    private isMouseIn: boolean = false;
    OnMouseOver(HeadId:string,ElementId:string) {
        this.isMouseIn = true;
        var HeadClassString = "."+HeadId;
        var ElementClassString = "."+ElementId;
        var AllHeaditems :NodeListOf<HTMLElement> = document.querySelectorAll(HeadClassString);
        var MainHeadIndex:number=0;
        for(let i =0 ; i < AllHeaditems.length ; i++){
         if(AllHeaditems[i].clientLeft!=0 ||   AllHeaditems[i].clientTop!=0 || AllHeaditems[i].clientWidth!=0 || AllHeaditems[i].clientHeight!=0){
            MainHeadIndex = i ;
            break;
         }
        }
            this.timerToken = setTimeout(() => {
                if (AppTool.IsNullOrEmpty(AllHeaditems))
                    return;
                var itemRect = AllHeaditems[MainHeadIndex].getBoundingClientRect();
                if (this.isMouseIn) {
                    var AllSessionElements :NodeListOf<HTMLElement> = document.querySelectorAll(ElementClassString);
                    AllSessionElements.forEach(s=>{
                        s.style.position = "fixed";
                        s.style.top = (itemRect.top - 35) + 'px';
                        s.style.left = (itemRect.left + 145) + 'px';
                        s.style.visibility = "visible";
                        s. style.display = "initial";
                    });
                }

            }, 100);
    
    }
    OnMouseLeave(ElementId:string) {
          this.isMouseIn = false;
          var ElementClassString = "."+ElementId;
          var AllSessionElements :NodeListOf<HTMLElement> = document.querySelectorAll(ElementClassString);
             this.timerToken = setTimeout(() => {
                        AllSessionElements.forEach(s=>{
                            s.style.visibility = "hidden";
                            s.style.display = "none";
                        });

                    }, 500);

    }

    DisplayTransactionsLinkClicked() {

        if(this.isUsedOutside)
        {
            var editWindow = new LogitudeWindow();

            editWindow.ShowHeaderButtons = true;
            //editWindow.Title = windowTitle;
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.IsHideHeader  = true;
            editWindow.ShowEditComponent(this.AccountPM.Id, "GLAccount", "GATR");
            editWindow.WindowClosed.subscribe((res:any) => {
                this.LoadAllData();
            });
        }
        else
        {
            this.CurrentSession.CurrentEditComponent.SetSelectedTabByCode("GATR");
        }

    }
    ReconcileLinkClicked() {
        this.ReconcileButtonClicked();
    }
    ReconcileButtonClicked()
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();
        this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.AccountPM.Id).subscribe((serviceResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            if (serviceResponse.Result) {
                var transaction = serviceResponse.Result;
                var openAmountCurrency = transaction.OpenAmountCurrencySign;

                // original amount currency
                var originalAmountCurrency;
                if (this.AccountPM.ReconcileMethodCode == "0") originalAmountCurrency = SessionLocator.TenantPM.CurrencySign;
                else if (this.AccountPM.ReconcileMethodCode == "1") originalAmountCurrency = transaction.CurrencySign;


                var windowArgs: any = {};
                windowArgs.GLAccountPM = this.AccountPM;
                windowArgs.openAmountCurrency = openAmountCurrency;
                windowArgs.originalAmountCurrency = originalAmountCurrency;
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
                logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 800 ? 700 : screenHeight - 70) : screenHeight - 70;

                logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.Reconcile"); //"Reconcile";

                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Show('./Accounting/Components/Others/ReconcileComponent');
                logitudeWindow.WindowClosed.subscribe(($event: any) => {
                    // this.LoadAllData();
                    this.GetNonReconciledTransactionsCount();

                });

            }
        });
    }

    GetNonReconciledTransactionsCount() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._GLAccountExtendedListService.GetAccountReconcilesCount(this.AccountPM.Id).subscribe((myResult:any) => {

            if (!AppTool.IsNullOrEmpty(myResult)) {

                this.CurrentSession.CurrentEditComponent.EntityPM.ReconcilationCount = myResult;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(($event) => {
                    if ($event == true) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                    this.CurrentSession.StopBusyIndicator();

                });
            }

        });
    }
    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }
    getScreenWidth() {
        if (self.innerWidth) {
            return self.innerWidth;
        }

        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }

        if (document.body) {
            return document.body.clientWidth;
        }
    }
    //#endregion

    //#region Accounting Notes
    accountingNotesList: AccountingNoteList[] = [];
    isNotesLoading:boolean = false;

    GetAccountingNotes(){
        if(this.AccountPM.CardId){
            this.accountingNotesList = [];
            this.isNotesLoading = true;
            // setTimeout(() => {

            this._AccountingNoteExtendedListService.GetNotesByCard(this.AccountPM.CardId)
                .subscribe((res:ServiceResponse) =>
                {
                        this.isNotesLoading = false;


                    if(res.HasError){
                        var msg = new MessageWindow();
                        msg.Show("Get Accounting Note error: " + res.ErrorsArray[0]);
                    }else{
                        var notesList = res.Result;
                        this.accountingNotesList = notesList;
                    }
                });
            // }, 2000);

        }

    }
    OpenAccountingNote(notePM: AccountingNotePM){

        var windowArgs: any = {};
        windowArgs.AccountPM = this.AccountPM;
        windowArgs.AccountingNotePM = notePM;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 320;
        logitudeWindow.Title = notePM ? '' : TextCodeTranslator.Translate("Accounting.O.NewAccountingNote");

        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/AccountingNoteComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.GetAccountingNotes();
        });

    }
    ItemEditButton(_noteList: AccountingNoteList){

        this.CurrentSession.StartBusyIndicatorLoading();

        this._AccountingNotePMService.get(_noteList.Id)
            .subscribe((myResult:any) => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var _notePM = mm.Result;
                    this.OpenAccountingNote(_notePM);
                    this.CurrentSession.StopBusyIndicator();


                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                }
            });


    }
    ItemDeleteButton(item: AccountingNoteList){
        this._AccountingNoteExtendedListService.DeleteNote(item.Id)
            .subscribe((myResult:any) => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var res = mm.Result;
                    this.CurrentSession.StopBusyIndicator();
                    this.GetAccountingNotes();

                }
                else {
                    var msg = new MessageWindow();
                    msg.Show(mm.ErrorsArray[0]);
                    this.CurrentSession.StopBusyIndicator();
                }
            });
    }
    txt_updatedBy: string = TextCodeTranslator.Translate("AccountingNote.F.UpdatedByUserName");
    GetNoteTitle(note:AccountingNoteList){
        var result = "";
        if(note){
            var myFormats = DateTool.GetDateFormats(note.UpdateDate);
            var formatedDate = myFormats.DateString + " " + myFormats.ShortTimeString;
            result = this.txt_updatedBy + ' ' + note.UpdatedByUserName + ' (' + formatedDate + ') ';
        }
        return result;
    }
    //#endregion

    //#region Last 10 Transactions
    lastTransactionsList: LedgerTransactionList[];
    GetAmountLabel() {
        var msg = this.txtcode_Amount + " (" + SessionLocator.TenantPM.CurrencyCode + ")";
        return msg;
    }
    GetTransAmount(transaction: LedgerTransactionList) {
        return transaction.LocalAmountDebit ? transaction.LocalAmountDebit : transaction.LocalAmountCredit;
    }
    GetIconText(line: LedgerTransactionList) {

        var iconTxt = AccountingEntityHelper.getEntityIcon(line.SourceTypeCode);
        return iconTxt;
    }
    GetReferencesText(transaction: LedgerTransactionList) {
        var reference="";

        reference = this.pushText(reference, transaction.Reference1);
        reference = this.pushText(reference, transaction.Reference2);
        reference = this.pushText(reference, transaction.Reference3);

        return reference;
    }
    pushText(txt: string, target: string) {
        return (target ? (!target.includes(txt) ? (target += ' / ' + txt) : target) : txt);
    }

    GetLastTransactions() {
        this._LedgerTransactionExtendedListService.getLast10TransactionsForAccount(this.AccountPM.Id).subscribe((myResult: ServiceResponse) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError)
            {
                this.lastTransactionsList = mm.Result;
            }
            else
            {
            }
        });

    }
    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }
    OpenSource(transaction: LedgerTransactionList, id: string) {

        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = AccountingEntityHelper.getEntityObjectTableName(transaction.SourceTypeCode);


        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName
                });
            });

    }

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    CalculateOriginalAmount(transaction) {
        if (!AppTool.IsNullOrEmpty(ReconcileEventManager.GLAccountReconcileMethodCode)) {

            if (ReconcileEventManager.GLAccountReconcileMethodCode == "0") { // 0-local currency

                if (transaction['LocalAmountCredit'] == 0) {
                    return transaction['LocalAmountDebit'];
                } else {
                    return -1 * transaction['LocalAmountCredit'];
                }

            } else if (ReconcileEventManager.GLAccountReconcileMethodCode == "1") { // 1-foreign currency

                if (transaction['ForeignAmountCredit'] == 0) {
                    return transaction['ForeignAmountDebit'];
                } else {
                    return -1 * transaction['ForeignAmountCredit'];
                }

            }

        }
    }
    GetIndicatorText(transaction)
    {
        var showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        if(transaction['OpenAmount'] != this.CalculateOriginalAmount(transaction))
            return showLocal ? 'סכום פתוח חלקית' : 'Partial transaction';
        else
            return showLocal ? 'סכום פתוח ' : 'Open transaction';
    }


    //#endregion

    //#region Credit limit
    creditPercentage:number = 0;
    accountTotal: number = 0;
    creditStatusAmount: number = 0;
    externalTransactionsTotal: number = 0;
    LoadCreditDetailsData(){


        this.LoadExternalTransactionTotal();

        console.log("LoadCreditDetailsData");

        // Calculate credit percentage
        var percentage = 0;
        if (this.GLAccountMoreData && this.accountCardlist)
        {
            percentage =
                (this.GLAccountMoreData.BalanceInLocalCurrency ? this.GLAccountMoreData.BalanceInLocalCurrency : 0)
            +   (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
            +   (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0)
            + (this.OpenShipments?this.OpenShipments:0 );

            this.accountTotal = percentage;

            if(this.CreditLimitAmount && this.CreditLimitAmount != 0)
                percentage = percentage / (this.CreditLimitAmount ? this.CreditLimitAmount : 0);
            else
                percentage = 0;

            this.creditStatusAmount = (this.CreditLimitAmount ? this.CreditLimitAmount : 0) - this.accountTotal;

        }

        if (!percentage) percentage = 0;
        if (percentage < 0) percentage = 0;

        percentage = percentage * 100;

        this.creditPercentage = percentage;


    }

    DisplayChequelistClicked(){

    }
    CardIndexClicked(){
        this.DisplayTransactionsLinkClicked();
    }
    IsOverCredit(){
        var total =
        (this.GLAccountMoreData.BalanceInLocalCurrency ? this.GLAccountMoreData.BalanceInLocalCurrency : 0)
    +   (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
    +   (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0)
        + (this.OpenShipments ? this.OpenShipments : 0);


        return (total > this.CreditLimitAmount);
    }
    IsCreditNotDefined(){
        return this.CreditLimitAmount == null;
    }
    //

    //#endregion

    //#region Aging Details
    chartId: string = "";

    GetAgingHeader() {
        var txt = this.txtcode_AgingDetails;
        txt += " (" + SessionLocator.TenantPM.CurrencyCode + ")";
        return txt;
    }

    // Filter Methods
    public FilterSelectedValue: string = '3mo';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.FilterLines();
        }
    }
    FilterLines() {
        this.LoadChartData();
    }

    // Filter Methods
    public DateFilterSelectedValue: string = 'filter_Collecting';
    DateFilterItemClicked(itemValue: string) {
        if (this.DateFilterSelectedValue != itemValue) {
            this.DateFilterSelectedValue = itemValue;
            this.FilterLines();
        }
    }

    //Chart Code
    public barChartLabels: string[] = [];
    public barChartData: any[] = [{ data: [], label: '', scaleShowVerticalLines: false, }];
    LoadChartData() {
        //if (AppTool.IsNullOrEmpty(this.accSettings)) return;
        var numberOfmonthsbackwards = 3;
        switch (this.FilterSelectedValue) {
            case '3mo': {
                numberOfmonthsbackwards = 3;
                break;
            }
            case '6mo': {
                numberOfmonthsbackwards = 6;
                break;
            }
            case '9mo': {
                numberOfmonthsbackwards = 9;
                break;
            }
            case '12mo': {
                numberOfmonthsbackwards = 12;
                break;
            }
        }

        var args = new AgingReportParameters();

        args.Tenant = SessionLocator.Tenant,
            args.AgingForDate = new Date();
        args.NumberOfmonthsbackwards = numberOfmonthsbackwards == null ? 3 : numberOfmonthsbackwards;
        //args.VendorCustomerId = AppTool.IsNullOrEmpty(this.accSettings) ? "" : this.accSettings.CustomerControlAccountId;
        args.VendorCustomerId = this.AccountPM.Id;
        //args.Category1Id = "";
        //args.Category2Id = "";
        //args.Category3Id = "";
        //args.Category4Id = "";
        //args.Category5Id = "";
        //args.CollectorId = SessionLocator.LoggedUserId;
        //args.SalesmanId = "";
        args.IsCustomer = true;
        args.GroupByDate = this.DateFilterSelectedValue == "filter_Accounting" ? "AccountingDate" : "DueDate";

        this.CurrentSession.StartBusyIndicatorLoading();
        this._GLAccountExtendedListService.GetAgingReport(args).subscribe((myResponse: ServiceResponse) => {
            console.log("GetAgingReport: ", periods);
            this.CurrentSession.StopBusyIndicator();

            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var periods: PeriodM[];
                    periods = res;
                    this.LoadChart(periods);
                }
            }
        });

    }
    OrganizeData(data: GLAccountList[]) {

        //var oData: any[];
        //var takeNumber = 5; // Number of columns to show in the graph , execlude "Others" column

        //if (!AppTool.IsNullOrEmpty(data)) {

        //    if (data.length > takeNumber) {

        //         1-Take first section
        //        oData = data.slice(0, takeNumber);

        //         2-Calculate "Others" Column
        //        var othersColumn = new GLAccountList();
        //        othersColumn.TotalAmount = 0;
        //        othersColumn.GLAccountTypeCode = "-1"; // manual entry "Others"
        //        for (var i = takeNumber; i < data.length; i++) {
        //            var amount = data[i].TotalAmount;
        //            amount = this.ConvertToLocal(amount, data[i].CurrencyId);
        //            othersColumn.TotalAmount += ((AppTool.IsNullOrEmpty(amount)) ? 0 : amount);
        //        }
        //        oData.push(othersColumn);
        //    } else {
        //        return data
        //    }
        //}

        // 3-Return data
        //return oData;
    }
    LoadChart(data: PeriodM[]) {

        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
            data: [], label: '', scaleShowVerticalLines: false,
        }];

        //Graph Properties
        var Graphs = Graphs = [{
            "balloonText": "Amount: <br>[[value]]",
            "fillAlphas": 1,
            "id": "AmGraph-10" + i,
            "title": "Aging",
            "type": "column",
            "valueField": "dataCol",
            "fillColors": ["#BADFE8", "#7AC2D4", "#73BFD2", "#7AC2D4", "#BADFE8",],
            "gradientOrientation": "horizontal",
            "borderAlpha": 0,
            "lineColor": "#fff",
            "fixedColumnWidth": this.FilterSelectedValue == "9mo" ? 30 : 40,


        }]
        //#endregion

        data.forEach(element => {
            //if (element.Total <= 0) return;
            this.barChartData[0].label = "Amount";

            // Amount
            var value = element.Total;// + (Math.floor((Math.random() * 2500) + 1));
            this.barChartData[0].data[i] = value.toString();

            // Labels
            var label = element.PeriodName.replace("b4", this.showLocal ? "עד" : "Before"); // replace 'b4' with 'Before'
            label = label.replace("/20", "/"); // minimize year in 'Before' Column
            this.barChartLabels[i] = label;

            // Data
            DataProvider[i] = { "category": this.barChartLabels[i], "dataCol": this.barChartData[0].data[i] };

            if (value > max)
                max = value;


            i++;
        });

        var poisition = this.isRTL == true ? "right" : "left";

        makeAmBarChart(this.chartId, Graphs, DataProvider, max, null, null, null, null, poisition);

    }
    //

    //#endregion


}
