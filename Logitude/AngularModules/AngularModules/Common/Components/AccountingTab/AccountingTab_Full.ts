import { TextCodeTranslator } from './../../../Infrastructure/Utilities/TextCodeTranslator';
import { GLAccountPM } from './../../../Accounting/EntityPMs/GLAccountPM';
import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import { GLAccountPMService } from './../../../Accounting/Services/StandardPMs/GLAccountPMService';
import {Component, OnDestroy, ViewContainerRef, ViewChild, OnInit} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CardList} from '../../EntityLists/CardList';
import {CardListService} from '../../Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {NewGLAccountArgs} from '../../Args';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './AccountingTab_Full.html',
})

export class AccountingTab_Full extends BaseComponent implements OnDestroy, OnInit {
    @ViewChild("TabPlaceholder", { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    public EntityPM: any = null;
    public ObjectTableName: string;
    public DataContext = this;
    public GLAccountId: string = null;
    public FullAccountingLabel: string = "Accounting Activation";
    public CardList: CardList = null;
    private myCardListService: CardListService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _GLAccountPMService: GLAccountPMService = new GLAccountPMService();
    ShowMessage: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;


    public get ShowConnectToCardButton() : boolean {
        //if(this.CardList)
        //    return this.CardList.PartnerTypeId == 'CS' || this.CardList.PartnerTypeId == 'VD' || this.CardList.PartnerTypeId == 'AC';

        return true;
    }

    constructor(private entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { });
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.myCardListService = new CardListService();
        this.LoadCardList();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadCardList();

                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadCardList();
                    }
                });
            }
        }
    }
    ngOnInit()
    {
    }
    LoadOverviewTab() {

        this.ShowMessage = this.GLAccountId == null;

        if (this.GLAccountId) {

            // 1- Get the GLAccount
            this.CurrentSession.StartBusyIndicatorLoading();

            this._GLAccountPMService.get(this.GLAccountId).subscribe(myResult => {

                var response: ServiceResponse = myResult;
                if (!response.HasError) {
                    var entity = response.Result;

                    var myComponentPath = "./Accounting/Components/EditTabs/GLAccount/GLAccountOverviewComponent";
                    SessionLocator.DynamicLoader.Load(myComponentPath, this.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.AccountPM = entity;
                            cmpRef.instance.LoadAllData();
                        });
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                }
            });




        }

    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    LoadCardList() {
        this.myCardListService.getSingle(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CardList = myResponse.Result;
                if (this.CardList)
                {
                    this.GLAccountId = this.CardList.GLAccountId;
                    if (!AppTool.IsNullOrEmpty(this.GLAccountId)) {
                        this.GetGLAccount();
                        this.FullAccountingLabel = this.ObjectTableName + " GLAccount";
                    }

                    this.LoadOverviewTab();


                }
            }
        });
    }
    private isFullAccountingClicked = false;
    FullAccountingClicked()
    {
        if(!this.GLAccountId)
            this.RunNewGLAccount();
        else
            this.EditGLAccount(); // will not be hit!
    }
    RunNewGLAccount() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Account";

        var args = new NewGLAccountArgs();
        if (this.CardList.PartnerTypeId == "CS" || this.CardList.PartnerTypeId == "PO") {
            args.AccountType = "2";
            args.ChartOfAccountType = "3";
        }
        else if (this.CardList.PartnerTypeId == "AC") {
            args.AccountType = null;
            args.ChartOfAccountType = null;
           
        }
        else {
            args.AccountType = "3";
            args.ChartOfAccountType = "4";
        }
        args.RevenueExpenseType = "3";
        args.CardId = this.CardList.Id;
        args.DisplayNo = this.CardList.Code;
        args.LocalName = this.CardList.LocalName;

        args.EnglishName = this.CardList.EnglishName;
        args.PartnerType = "AC";
        logWindow.WindowArgs = args;

        logWindow.Show('./Accounting/Components/NewEntity/NewGLAccountComponent');

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
    }
    EditGLAccount() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef =>
            {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.GLAccountId, ObjectTableName: 'GLAccount' });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                });
            });
    }

    Connect2ExistCard() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 650;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator.Translate("Accounting.O.GLAccounts");

        var args: any = {};

        var chartOfAccountTypeCode

        if (this.CardList.PartnerTypeId == 'CS' || this.CardList.PartnerTypeId == 'CC' || this.CardList.PartnerTypeId == 'CG' || this.CardList.PartnerTypeId == 'CH' || this.CardList.PartnerTypeId == 'CO')
            chartOfAccountTypeCode = '3';
        else 
            chartOfAccountTypeCode = '4';

        args.AccountTypeCode = chartOfAccountTypeCode;
        args.CardId = this.CardList.Id;

        logWindow.WindowArgs = args;
        logWindow.Show('./Common/Components/AccountingTab/GLAccountSelectWindow/GLAccountSelectComponent');
        logWindow.WindowClosed.subscribe(glaccountId => {
            if (glaccountId) {
                console.log('GLAccountSelectComponent',glaccountId);
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

            }
        });
    }

    glaccount: GLAccountPM;
    GetGLAccount(){
        return new Promise(resolve =>
            {
                this.CurrentSession.StartBusyIndicatorLoading();
                this._GLAccountPMService.get(this.GLAccountId).subscribe(myResult => {

                    var response: ServiceResponse = myResult;
                    if (!response.HasError) {
                        var entity = response.Result;
                        this.glaccount = entity;
                        resolve(entity);
                    }
                    else {
                        this.CurrentSession.StopBusyIndicator();
                        resolve(null);
                    }
                });
            });


    }
}
