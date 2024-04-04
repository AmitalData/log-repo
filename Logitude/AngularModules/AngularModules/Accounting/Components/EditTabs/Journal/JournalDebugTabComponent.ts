import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { JournalPM } from '../../../EntityPMs/JournalPM';
import { JournalLinePM } from '../../../EntityPMs/JournalLinePM';
import { JournalActionTypePM } from '../../../EntityPMs/JournalActionTypePM';
import { AccountingPeriodList } from '../../../EntityLists/AccountingPeriodList';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { AccountingPeriodExtendedListService } from '../../../Services/ExtendedLists/AccountingPeriodExtendedListService';
import { GLAccountExtendedListService } from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import { AccountingPeriodListService } from '../../../Services/StandardLists/AccountingPeriodListService';
import { RatesTableExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { GLAccountPM } from '../../../EntityPMs/GLAccountPM';
import { CurrencyPM } from '../../../../Common/EntityPMs/CurrencyPM';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { JournalValidator } from '../../../Validators/JournalValidator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { GLAccountListService } from '../../../Services/StandardLists/GLAccountListService'
import { APInvoicePMService } from '../../../../Invoice/Services/StandardPMs/APInvoicePMService';
import { APInvoicePM } from '../../../../Invoice/EntityPMs/APInvoicePM';
import { LedgerTransactionListService } from '../../../Services/StandardLists/LedgerTransactionListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { LedgerTransactionList } from '../../../EntityLists/LedgerTransactionList';
import { InterestTransactionListService } from '../../../Services/StandardLists/InterestTransactionListService';
import { InterestTransactionList } from '../../../EntityLists/InterestTransactionList';
import { JournalExtendedListService } from '../../../Services/ExtendedLists/JournalExtendedListService';
import { JournalMoreDataPM } from '../../../EntityPMs/JournalMoreDataPM';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { JournalAdditionalDataListService } from '../../../Services/StandardLists/JournalAdditionalDataListService';
import { JournalAdditionalDataList } from '../../../EntityLists/JournalAdditionalDataList';
import { LedgerTransactionPMService } from 'Accounting/Services/StandardPMs/LedgerTransactionPMService';
import { LedgerTransactionPM } from 'Accounting/EntityPMs/LedgerTransactionPM';


const BanksChartOfAccountsTypeCode = '5';
const amitalExternalSystemCode = 'amital';
@Component({

    templateUrl: './JournalDebugTabComponent.html',

})

export class JournalDebugTabComponent extends BaseComponent implements OnInit {
    public ActionId: any;

    public EntityPM: JournalPM = null;
    public ObjectTableName = "Journal";
    public DataContext = this;
    defaultCurrencyId: string = SessionLocator.TenantPM.CurrencyId;
    public TenantCurrency = SessionLocator.TenantPM.CurrencyCode;

    creditTotal: number = 0;
    debitTotal: number = 0;
    journalDisabled: boolean = false;
    forceFocus: boolean = false;
    PointerEvents: string = 'auto';
    Opacity: string = "1";
    referencesDivHeight: number;
    Approved: boolean = false;
    IsJournalEditableAfterApproval: boolean = false;
    APInvoice: APInvoicePM;
    Voided: boolean = false;

    _AccountingPeriodListService: AccountingPeriodListService = new AccountingPeriodListService();
    ratesTableExtendedListService: RatesTableExtendedListService = new RatesTableExtendedListService();
    private ledgerTransactionListService: LedgerTransactionListService = new LedgerTransactionListService();
    private interestTransactionListService: InterestTransactionListService = new InterestTransactionListService();
    JournalExtendedListService: JournalExtendedListService = new JournalExtendedListService();
    JournalAdditionalDataListService: JournalAdditionalDataListService=new JournalAdditionalDataListService();
    ledgerTransactionPMService: LedgerTransactionPMService=new LedgerTransactionPMService();
    Load: boolean = false;

    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    LedgerTransactionList: ObservableCollection = new ObservableCollection([]);
    IsCustomerCare: boolean;
    JournalReconciles: ObservableCollection = new ObservableCollection([]);
    JournalExternalReconciles: ObservableCollection = new ObservableCollection([]);
    InterestTransactionList: ObservableCollection = new ObservableCollection([]);
    JournalMoreDataList: ObservableCollection = new ObservableCollection([]);
    JournalAdditionalDataList: ObservableCollection = new ObservableCollection([]);
    constructor(
        private entityArgs: EntityArgs,
        private CD: ChangeDetectorRef,
        private EntityResourceService: EntityResourceService,

    ) {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.IsCustomerCare = SessionLocator.LoggedUserPM.IsCustomerCare;

        this.EntityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("JournalReconcile").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("JournalExternalReconcile").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("InterestTransaction").subscribe((response: any) => {
                        this.EntityResourceService.getEntityResourceByTableName("InterestReport").subscribe((response: any) => {

                            this.EntityResourceService.getEntityResourceByTableName("JournalMoreData").subscribe((response: any) => {
                                this.EntityResourceService.getEntityResourceByTableName("JournalAdditionalData").subscribe((response: any) => {
                                    this.EntityResourceService.getEntityResourceByTableName("Reconciliation").subscribe((response: any) => {
                                        this.EntityResourceService.getEntityResourceByTableName("ExternalReconciliation").subscribe((response: any) => {
                                            this.Load = true;
                                            this.EntityPM = this.entityArgs.EntityPM;
                                            this.ObjectTableName = this.entityArgs.ObjectTableName;





                                            // redraw


                                            this.Listen();
                                            this.FillGrid();
                                            this.SetUIProperties();
                                        })
                                    })
                                })
                            })
                        })
                    })
                })
            })
        });
    }

    public CurrentEditComponentId: string;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    Listen() {


        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                        this.FillGrid();
                        this.SetUIProperties();
                    }
                });
            }

            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                        this.FillGrid();
                        this.SetUIProperties();
                        console.log("Entity Reloaded");
                    }
                });
            }
        }

        this.EntityPM.PropertyChanged.subscribe(changes => {
            console.log("JournalPM changed", changes);

        });
    }

    SetUIProperties() {


        //this.UIProperties.SetVisibility("Reference2", "Journal", false);

    }





    FillGrid() {

        // if entity in edit mode
        if (this.EntityPM.Id != undefined || this.EntityPM.JournalLines.length > 0) {

            this.LedgerTransactionList.Clear();
            this.JournalReconciles.Clear();
            this.JournalExternalReconciles.Clear();
            this.InterestTransactionList.Clear();
            this.JournalMoreDataList.Clear();
            this.JournalAdditionalDataList.Clear();


            this.JournalExtendedListService.GetJournalAdditionalDataByJournalId(this.EntityPM.Id)
                .subscribe(r => {
                    let res: JournalAdditionalDataList[] = r.Result;
                    this.JournalAdditionalDataList.InsertCollection(res);
                });
            this.JournalExtendedListService.GetJournalMoreDatasByJournalId(this.EntityPM.Id)
                .subscribe(r => {
                    let res: JournalMoreDataPM[] = r.Result;
                    this.JournalMoreDataList.InsertCollection(res);
                });
            this.LedgerTransactiongetGetRows();
            this.InterestTransactionGetRows()
            //this.JournalAdditionalDataGetRows();            
            this.JournalReconciles.InsertCollection(this.EntityPM.JournalReconciles);
            this.JournalExternalReconciles.InsertCollection(this.EntityPM.JournalExternalReconciles);
        }

    }
    OpenJournal(id: string) {
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

    ngOnInit() {
        this.CurrentSession.LostFocusEvent.subscribe((res) => {
            if (this.CD) {
                var isDestroyed: boolean = this.CD['destroyed'];
                if (!isDestroyed) {
                    this.CD.detectChanges();
                    //console.log("AfterLostFocus");
                }
            }
        });


        //set focus on accounting date
        //var t = setTimeout(() => { this.forceFocus = true; }, 1);
    }

    //#region Properties

    //#endregion
    JournalAdditionalDataGetRows_notwork() {
        let filters = new ApiQueryFilters();


        filters.PageSize = 200;
        filters.PageIndex = 0;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "GLAccountId";
        //InterestEntityTypeCode

        filters.addAdditionalFilter("JournalId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");


        return this.JournalAdditionalDataListService.getByFilters(filters)
            .subscribe(r => {
                //this.LedgerTransactionList = new ObservableCollection([]);
                let res: JournalAdditionalDataList[] = r.Result;


                this.JournalAdditionalDataList.InsertCollection(res);
            });

    }

    InterestTransactionGetRows()//skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
    {
        let filters = new ApiQueryFilters();


        filters.PageSize = 200;
        filters.PageIndex = 0;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "GLAccountId";
        //InterestEntityTypeCode
        let journalEntityId = "3"
        /*
1	פקודת יומן	Journal
10	התאמה	Adjustment
11	העברת שנה	Year Transfer
12	התאמת בנק	Bank Adjustment
2	חשבונית לקוח	ARInvoice
3	קבלה לקוח	ARPayment
4	חשבונית ספק	APInvoice
5	תשלום לספק	APPayment
6	הפקדת המחאות	Cheque Deposit
7	הפקדת מזומן	Cash Deposit
8	שערוך	Revaluation
9	מערכת המחאות	Payment Cheque

>>>>

1	ARInvoice
2	ARPayment
3	Journal
4	Open Balance



         */
        let EntityPMId = this.EntityPM.Id;
        switch (this.EntityPM.AccountingEntityCode) {

            case "2": { journalEntityId = "1"; EntityPMId = this.EntityPM.AccountingEntityId } break;//	חשבונית ספקARInvoice
            case "3": { journalEntityId = "2"; EntityPMId = this.EntityPM.AccountingEntityId } break;//	תשלום לספקARPayment
            case "10": { journalEntityId = "5"; } break;
            default:
                { journalEntityId = "3"; } break;

        }
        //filters.addAdditionalFilter("JournalId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("InterestEntityTypeCode", journalEntityId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("EntityId", EntityPMId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        return this.interestTransactionListService.getByFilters(filters)
            .subscribe(r => {
                //this.LedgerTransactionList = new ObservableCollection([]);
                let res: InterestTransactionList[] = r.Result;
                res = res.filter(x => x.JournalNumber == this.EntityPM.JournalNumber);

                this.InterestTransactionList.InsertCollection(res);
            });

    }
    LedgerTransactiongetGetRows()//skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
    {

        let filters = new ApiQueryFilters();


        filters.PageSize = 200;
        filters.PageIndex = 0;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "JournalLineNumber";



        filters.addAdditionalFilter("JournalId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

        return this.ledgerTransactionListService.getByFilters(filters)
            .subscribe(r => {
                //this.LedgerTransactionList = new ObservableCollection([]);
                let res: LedgerTransactionList[] = r.Result;

                res.forEach(lt => {
                    let jl = this.EntityPM.JournalLines.filter(r => r.Line == lt.JournalLineNumber)[0];
                    if (jl.DebitAccountId == lt.AccountId) {
                        lt.AccountDisplayNumber = jl.DebitAccountNumber;
                    } else {
                        lt.AccountDisplayNumber = jl.CreditAccountNumber;
                    }

                });
                this.LedgerTransactionList.InsertCollection(r.Result);
            });

    }


    DetectChanges() {
        this.CD.detectChanges();
    }
    ShowRemark(generalData) {
        var myMessageWindow = new MessageWindow();
        myMessageWindow.Width = 800;
        myMessageWindow.Height = 600;
        //myMessageWindow.Title = "Query Doesn't Exist";
        //myMessageWindow.Message = "Query With the Id " + this.ID + " does not exist";

        myMessageWindow.Show(generalData);
    }

    ResetJournalClicked(clearIt: boolean) {

        this.JournalExtendedListService.GetResetJournalByJournalId(this.EntityPM.Id, clearIt)
            .subscribe(r => {
                if (!r.HasError) {
                    var myMessageWindow = new MessageWindow();

                    myMessageWindow.Show("Fix done !!\nRe Enter screen ");
                } else {
                    var myMessageWindow = new MessageWindow();
                    myMessageWindow.Title = "Error while reset journal";
                    myMessageWindow.Show(r.ErrorsArray[0]);
                }
            },
                (err) => {
                    var myMessageWindow = new MessageWindow();
                    myMessageWindow.Show(err);
                });

    }

    IsExternalReconcileChanged(transaction: LedgerTransactionList, checked)
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        transaction.IsExternalReconcile = checked;
        this.ledgerTransactionPMService.get(transaction.Id)
            .subscribe(response =>
            {
                this.CurrentSession.StopBusyIndicator();

                if (!response.HasError) {
                    var ledger: LedgerTransactionPM = response.Result;
                    this.UpdateLedgerTransaction(ledger, checked);

                } else {
                    var myMessageWindow = new MessageWindow();
                    myMessageWindow.Title = "Error";
                    myMessageWindow.Show(response.ErrorsArray[0]);
                }
            },
                (err) =>
                {
                    this.CurrentSession.StopBusyIndicator();

                    var myMessageWindow = new MessageWindow();
                    myMessageWindow.Show(err);
                });

    }


    private UpdateLedgerTransaction(ledger: LedgerTransactionPM, checked: any)
    {
        this.CurrentSession.StartBusyIndicatorSaving();

        ledger.IsExternalReconcile = checked;

        this.ledgerTransactionPMService.update(ledger)
            .subscribe(r =>
            {
                this.CurrentSession.StopBusyIndicator();

                if (!r.HasError) {
                    var myMessageWindow = new MessageWindow();

                    myMessageWindow.Show("Ledger Transaction Updated");
                } else {
                    var myMessageWindow = new MessageWindow();
                    myMessageWindow.Title = "Error while update";
                    myMessageWindow.Show(r.ErrorsArray[0]);

                }
            },
                (err) =>
                {
                    this.CurrentSession.StopBusyIndicator();

                    var myMessageWindow = new MessageWindow();
                    myMessageWindow.Show(err);
                });
    }


    IsExternalBankTransaction(ledger: LedgerTransactionList){
        const isFromAmital = this.EntityPM.ExternalSystem?.toLowerCase() == amitalExternalSystemCode;
        const journalLine = this.EntityPM.JournalLines.find(line=>line.Line == ledger.JournalLineNumber);
        const isConnectedToBankGLAccount = ((ledger.AccountId == journalLine.DebitAccountId && journalLine.DebitAccountCOACode == BanksChartOfAccountsTypeCode)
            || (ledger.AccountId == journalLine.CreditAccountId && journalLine.CreditAccountCOACode == BanksChartOfAccountsTypeCode));

        return ((isFromAmital || SessionLocator.LoggedUserPM.IsCustomerCare) && isConnectedToBankGLAccount);
    }
}

