import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
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

export class AccountingTab_Full extends BaseComponent implements OnDestroy {
    public EntityPM: any = null;
    public ObjectTableName: string;
    public DataContext = this;
    public GLAccountId: string = null;
    public FullAccountingLabel: string = "Accounting Activation";
    public CardList: CardList = null;
    private myCardListService: CardListService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

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
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadCardList();
                        
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadCardList();
                    }
                });
            }
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
                if (this.CardList) {
                    this.GLAccountId = this.CardList.GLAccountId;
                    if (!AppTool.IsNullOrEmpty(this.GLAccountId)) {
                        this.FullAccountingLabel = this.ObjectTableName + " GLAccount";
                    }

                    if (this.isFullAccountingClicked) {
                        this.isFullAccountingClicked = false;
                        if (!AppTool.IsNullOrEmpty(this.GLAccountId)) {
                            this.EditGLAccount();
                        }
                        else {
                            this.RunNewGLAccount();
                        }
                    }
                }
            }
        });
    }
    private isFullAccountingClicked = false; 
    FullAccountingClicked() {
        this.isFullAccountingClicked = true;
        this.entityArgs.EditComponent.SaveChanges();
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
        else {
            args.AccountType = "3";
            args.ChartOfAccountType = "4";
        }
        args.RevenueExpenseType = "3";
        args.CardId = this.CardList.Id;
        args.DisplayNo = this.CardList.Code;
        args.LocalName = this.CardList.LocalName;
        args.EnglishName = this.CardList.EnglishName;
        logWindow.WindowArgs = args;
        logWindow.Show('./Accounting/Components/NewEntity/NewGLAccountComponent');

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.LoadCardList();
            }
        });
    }
    EditGLAccount() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.GLAccountId, ObjectTableName: 'GLAccount' });
            });
    }
}