import { AccountingEntityHelper } from './../../../Utilities/AccountingEntityHelper';
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {LedgerTransactionListService} from '../../../Services/StandardLists/LedgerTransactionListService';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {ReconcileEventManager} from '../../../Utilities/ReconcileEventManager';
import {FullAccountingSettingPM} from '../../../EntityPMs/FullAccountingSettingPM';


//Entities
import {ExternalReconciliationPM} from '../../../EntityPMs/ExternalReconciliationPM';
import {ExternalReconciliationLinePM} from '../../../EntityPMs/ExternalReconciliationLinePM';
import {ExternalReconciliationList} from '../../../EntityLists/ExternalReconciliationList';
import {LedgerTransactionPM} from '../../../EntityPMs/LedgerTransactionPM';
import {LedgerTransactionList} from '../../../EntityLists/LedgerTransactionList';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {BankAccountPM} from '../../../EntityPMs/BankAccountPM';
import {ReconcileExternalPagePM} from '../../../EntityPMs/ReconcileExternalPagePM';
import {ReconcileExternalPageLinePM} from '../../../EntityPMs/ReconcileExternalPageLinePM';


//Services
import {ExternalReconciliationPMService} from '../../../Services/StandardPMs/ExternalReconciliationPMService';
import {ExternalReconciliationExtendedPMService} from '../../../Services/ExtendedPMs/ExternalReconciliationExtendedPMService';
import {LedgerTransactionExtendedListService} from '../../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import {ExternalReconciliationExtendedListService} from '../../../Services/ExtendedLists/ExternalReconciliationExtendedListService';
import {ReconcileExternalPageExtendedListService} from '../../../Services/ExtendedLists/ReconcileExternalPageExtendedListService';
import {CurrencyPMService} from '../../../../Common/Services/StandardPMs/CurrencyPMService';

@Component({
    
    templateUrl: './ExternalRecoDetailsTabComponent.html',
})

export class ExternalRecoDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: ExternalReconciliationPM = null;
    public ObjectTableName = "ExternalReconciliation";
    public DataContext = this;
    public TotalSum = 0;
    public NoRows: boolean = false;
    searchText: string = "";
    public isRTL: boolean = false;

    //public PagePM: ReconcileExternalPagePM;
    //public GLAccountPM: GLAccountPM;
    //public BankAccountPM: BankAccountPM;
    public ExternalRecoPM: ExternalReconciliationPM;
    openAmountCurrency: string = "";

    TransactionsLines: ObservableCollection;
    BankPageLines: ObservableCollection;

    entityListService: EntityListService = new EntityListService();
    ledgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    _ExternalReconciliationExtendedListService: ExternalReconciliationExtendedListService = new ExternalReconciliationExtendedListService();
    _ReconcileExternalPageExtendedListService: ReconcileExternalPageExtendedListService = new ReconcileExternalPageExtendedListService();
    externalReconciliationExtendedPMService: ExternalReconciliationExtendedPMService = new ExternalReconciliationExtendedPMService();
    externalReconciliationPMService: ExternalReconciliationPMService = new ExternalReconciliationPMService();
    _CurrencyPMService: CurrencyPMService = new CurrencyPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        this.ExternalRecoPM = entityArgs.EntityPM;

        this.TransactionsLines = new ObservableCollection([]);
        this.BankPageLines = new ObservableCollection([]);
    }

    ngOnInit() {
        this.GetDefaults();
        this.FillGrids();
    }

    GetDefaults() {

        var glAccountCurrencyId = this.ExternalRecoPM.AccountCurrencyId;
        //Get currency
        if (!glAccountCurrencyId || glAccountCurrencyId == "multi") {
            this.ledgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.GLAccountId).subscribe((serviceResponse: ServiceResponse) => {
                if (serviceResponse.Result) {
                    var result = serviceResponse.Result;
                    var transaction = result; // get the data
                    this.openAmountCurrency = transaction ? transaction.CurrencyCode : "";
                    this.ledgerAmountHeader += " (" + this.openAmountCurrency + ")";
                    this.bankAmountHeader += " (" + this.openAmountCurrency + ")";

                }
            });
        }
        else {
            this._CurrencyPMService.get(glAccountCurrencyId).subscribe((myResult:any) => {
                var currency = myResult.Result;
                this.openAmountCurrency = currency ? currency.Code : "";
                this.ledgerAmountHeader += " (" + this.openAmountCurrency + ")";
                this.bankAmountHeader += " (" + this.openAmountCurrency + ")";
            });
        }
    }

    //#region Data
    FillGrids() {

        //#region 1- prepare Ids list
        var transactionsLinesIds: string[] = [];
        var bankPageLinesIds: string[] = [];
        if (this.ExternalRecoPM) {
            for (var line of this.ExternalRecoPM.ExternalReconciliationLines) {
                if (line.LedgerTransactionId)
                    transactionsLinesIds.push(line.LedgerTransactionId);
                else if (line.ExternalPageLineId)
                    bankPageLinesIds.push(line.ExternalPageLineId)
                else
                    console.log("What type are you !!!!!!!", this.ExternalRecoPM, line);
            }

        } else {
            console.log("No ExternalRecoPM entity !!");
        }
        //#endregion

        // 2- get lines
        this.CurrentSession.StartBusyIndicatorLoading();
        if (bankPageLinesIds.length > 0) {
            this.GetBankLines(bankPageLinesIds, transactionsLinesIds); // then get ledger lines
        } else {
            console.log("No bank lines!");
            this.GetLedgerLines(transactionsLinesIds);
        }

    }
    GetBankLines(bankPageLinesIds, transactionsLinesIds) {
        this._ReconcileExternalPageExtendedListService.getBankPageLinesByIds(bankPageLinesIds).subscribe((myResult:ServiceResponse) => {
            var result = myResult.Result;
            var list = result.Result;

            // Incapsulate transactions
            var pageLines: BankLineModel[] = [];
            for (var line of list) {
                var pageLine = new BankLineModel(line, this, -1);
                pageLines.push(pageLine);
            }
            //

            // 3- fil group hash
            for (var recoLine of this.ExternalRecoPM.ExternalReconciliationLines) {
                if (recoLine.ExternalPageLineId) {
                    var item = pageLines.find(d => d.Id == recoLine.ExternalPageLineId);
                    item.GroupHash = recoLine.GroupNumber;
                }
            }

            // 4- sort
            pageLines.sort((a, b) => { return (a.PageLinePM.GroupHash === b.PageLinePM.GroupHash) ? 0 : (a.PageLinePM.GroupHash < b.PageLinePM.GroupHash) ? -1 : 1 });
            this.BankPageLines.InsertCollection(pageLines);

            this.GetLedgerLines(transactionsLinesIds);
        });
    }
    GetLedgerLines(transactionsLinesIds) {

        // 2- get ledger transactions lines
        if (transactionsLinesIds.length > 0) {
            this.ledgerTransactionExtendedListService.getLedgerTransactionsByIds(transactionsLinesIds).subscribe((myResult: ServiceResponse) => {
                var result = myResult.Result;
                var list = result;

                // Incapsulate transactions
                var transactionsItems: TransactionLineModel[] = [];
                for (var line of list) {
                    var item = new TransactionLineModel(line, this, -1);
                    transactionsItems.push(item);
                }
                //

                // 3- fil group hash
                for (var xxsxx of this.ExternalRecoPM.ExternalReconciliationLines) {
                    if (xxsxx.LedgerTransactionId) {
                        var vfvv = transactionsItems.find(d => d.Id == xxsxx.LedgerTransactionId);
                        vfvv.GroupHash = xxsxx.GroupNumber;
                    }
                }

                // 4- sort
                transactionsItems.sort((a, b) => { return (a.GroupHash === b.GroupHash) ? 0 : (a.GroupHash < b.GroupHash) ? -1 : 1 });
                this.TransactionsLines.InsertCollection(transactionsItems);

                this.FillGroupHash();

                this.CurrentSession.StopBusyIndicator();
            });
        } else {
            console.log("No ledger lines!");
            this.CurrentSession.StopBusyIndicator();
        }


    }
    FillGroupHash() {
        //// 3- fil group hash
        //for (var line of this.ExternalRecoPM.ExternalReconciliationLines) {
        //    if (line.LedgerTransactionId) {
        //        var item = this.TransactionsLines.Collection.find(d => d.Id == line.LedgerTransactionId);
        //        item.GroupHash = line.GroupNumber;
        //    }
        //    else if (line.ExternalPageLineId) {
        //        var item = this.BankPageLines.Collection.find(d => d.Id == line.ExternalPageLineId);
        //        item.GroupHash = line.GroupNumber;
        //    }
        //}
    }
    //#endregion

    OpenSource(id: string, type: string) {

        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = AccountingEntityHelper.getEntityObjectTableName(type);


        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName,
                    BackButtonLabel: 'GLAccount'
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

    bankAmountHeader = TextCodeTranslator.Translate('ReconcileExternalPageLine.F.Amount');
    ledgerAmountHeader = TextCodeTranslator.Translate('LedgerTransaction.F.OpenAmount');

}


class TransactionLineModel extends BaseComponent {
    public LedgerTransactionPM: LedgerTransactionPM = null;
    public ObjectTableName = "LedgerTransaction";
    public RowIndex: number;
    public DataContext = this;
    public isRTL: boolean = false;

    constructor(
        private ledgerTransaction: LedgerTransactionPM,
        private parent: ExternalRecoDetailsTabComponent,
        private myRowIndex: number
    ) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.LedgerTransactionPM = ledgerTransaction;
        //this.OriginalAmount = parent.CalculateOriginalAmount(this);
        //if (!this.AmountToReconcile)
        //    this.AmountToReconcile = this.ledgerTransaction.OpenAmount;
        this.RowIndex = myRowIndex;

        //this.OddEven = this.ColorMe();


        //#region Set Icons

        var iconTxt = AccountingEntityHelper.getEntityIcon(this.LedgerTransactionPM.SourceTypeCode);

        this.IconCode = iconTxt;

        //#endregion
    }


    get IsCredit(){
        return this.LedgerTransactionPM.ForeignAmountCredit != 0;
    }


    get GroupHash() { return this.LedgerTransactionPM.GroupHash };
    set GroupHash(value: number) { this.LedgerTransactionPM.GroupHash = value };


    // Properties
    public IconCode: string;

    //originalAmount: number;
    //get OriginalAmount() { return this.parent.CalculateOriginalAmount(this); }
    //set OriginalAmount(value: number) {
    //    if (this.originalAmount != value) {
    //        this.originalAmount = this.parent.CalculateOriginalAmount(this);
    //    }
    //}

    //get AmountToReconcile() { return this.LedgerTransactionPM.AmountToReconcile; }
    //set AmountToReconcile(value: number) {
    //    if (this.LedgerTransactionPM.AmountToReconcile != value) {
    //        this.LedgerTransactionPM.AmountToReconcile = value;

    //        //WI26522
    //        if (Math.abs(value) > Math.abs(this.OpenAmount)) {
    //            this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, false, TextCodeTranslator.Translate("Reconciliations.O.AmountMustBSmaller2OpenAmount"));
    //            this.parent.IsEntityValid = false;
    //        } else {
    //            this.UIProperties.SetValidity("AmountToReconcile", this.parent.ObjectTableName, true, "");
    //            this.parent.IsEntityValid = true;
    //        }

    //        this.parent.CalculateTotals();
    //    }

    //}

    OddEven: boolean;


    //#region Other Properties
    get Id() { return this.LedgerTransactionPM.Id; }
    get Tenant() { return this.LedgerTransactionPM.Tenant; }
    get AccountingDate() { return this.LedgerTransactionPM.AccountingDate; }
    get DocumentDate() { return this.LedgerTransactionPM.DocumentDate; }
    get JournalNumber() { return this.LedgerTransactionPM.JournalNumber; }
    get Source() { return this.LedgerTransactionPM.Source; }
    get SourceType() { return this.LedgerTransactionPM.SourceType; }
    get SourceId() { return this.LedgerTransactionPM.SourceId; }
    get DueDate() { return this.LedgerTransactionPM.DueDate; }
    get LocalAmountCredit() { return this.LedgerTransactionPM.LocalAmountCredit; }
    get LocalAmountDebit() { return this.LedgerTransactionPM.LocalAmountDebit; }
    get ForeignAmountCredit() { return this.LedgerTransactionPM.ForeignAmountCredit; }
    get ForeignAmountDebit() { return this.LedgerTransactionPM.ForeignAmountDebit; }
    get ForeignAmount() { return this.LedgerTransactionPM.ForeignAmount; }
    get OpenAmount() { return this.LedgerTransactionPM.OpenAmount; }
    get OpenAmountCurrencyCode() { return this.LedgerTransactionPM.OpenAmountCurrencyCode; }
    get OpenAmountCurrencySign() { return this.LedgerTransactionPM.OpenAmountCurrencySign; }
    get CurrencyId() { return this.LedgerTransactionPM.CurrencyId; }
    get Reference1() { return this.LedgerTransactionPM.Reference1; }
    get Reference2() { return this.LedgerTransactionPM.Reference2; }
    get Reference3() { return this.LedgerTransactionPM.Reference3; }
    get Notes() { return this.LedgerTransactionPM.Notes; }
    //get IsPartial() { return this.OpenAmount != this.AmountToReconcile; }
    get OpenAmountCurrencyId() { return this.LedgerTransactionPM.OpenAmountCurrencyId; }
    get SourceTypeCode() { return this.LedgerTransactionPM.SourceTypeCode; }
    get SourceNumber() { return this.LedgerTransactionPM.SourceNumber; }

    //#endregion


    //#region Row Coloring

    ColorMe() {
        //if (AppTool.IsNullOrEmpty(this.parent.lastGroupNumber))
        //    this.parent.lastGroupNumber = this.GroupHash;

        //if (this.parent.lastGroupNumber == this.GroupHash) {
        //    return this.parent.lastColorOperation == true;
        //} else {
        //    this.parent.lastGroupNumber = this.GroupHash;
        //    this.parent.lastColorOperation = !this.parent.lastColorOperation;
        //    return this.parent.lastColorOperation == true;
        //}
    }
    //#endregion

    CalculatOriginalCurruncy() {
        //
        // [i] copied from list template
        //
        let gLAccountReconcileMethodCode = ReconcileEventManager.GetGLAccountReconcileMethodCode();
        if (!AppTool.IsNullOrEmpty(gLAccountReconcileMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (gLAccountReconcileMethodCode == "0") { // 0-local currency

                // local
                return SessionLocator.TenantPM.CurrencySign;

            } else if (gLAccountReconcileMethodCode == "1") { // 1-foreign currency

                // foreign
                return this.ledgerTransaction.CurrencySign;

            }

        }
    }


}


class BankLineModel extends BaseComponent {
    public PageLinePM: ReconcileExternalPageLinePM = null;
    public ObjectTableName = "ReconcileExternalPageLine";
    public RowIndex: number;
    public DataContext = this;
    public isRTL: boolean = false;

    constructor(
        private pageLine: ReconcileExternalPageLinePM,
        private parent: ExternalRecoDetailsTabComponent,
        private myRowIndex: number
    ) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.PageLinePM = pageLine;
        this.RowIndex = myRowIndex;

    }



    get IsCredit(){
        return this.PageLinePM.CreditAmount != 0;
    }

    get GroupHash() { return this.pageLine.GroupHash };
    set GroupHash(value: number) { this.pageLine.GroupHash = value };

    //#region Properties
    get Id() { return this.PageLinePM.Id; }
    get Amount() { return this.PageLinePM.Amount; }
    get Reference() { return this.PageLinePM.Reference; }
    get ReferenceDate() { return this.PageLinePM.ReferenceDate; }
    get Notes() { return this.PageLinePM.Notes; }
    get LineNumber() { return this.PageLinePM.LineNumber; }

    //#endregion


}
