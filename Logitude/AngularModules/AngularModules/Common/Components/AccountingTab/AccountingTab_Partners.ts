import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './AccountingTab_Partners.html',
})

export class AccountingTab_Partners extends BaseComponent implements OnDestroy {
    public EntityPM: any = null;
    public ObjectTableName: string;
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;

        if (SessionLocator.AccountingSystemPM) {
            if (SessionLocator.AccountingSystemPM.Code == "GI" || SessionLocator.AccountingSystemPM.Code == "AI") {
                this.IsExternalByProductsVisible = true;
            }
        }

        if (this.ObjectTableName == "Customer") {
            this.UIProperties.SetVisibility("PayablesAccountingCard", this.ObjectTableName, false);
        }

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

                        if (this.isExternalByProductsRequestd) {
                            this.ApplyExternalByProducts();
                        }
                    }

                    this.isExternalByProductsRequestd = false;
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    get ReceivablesAccountingCard() { return this.EntityPM.ReceivablesAccountingCard; }
    set ReceivablesAccountingCard(value: string) {
        if (this.EntityPM.ReceivablesAccountingCard != value) {
            this.EntityPM.ReceivablesAccountingCard = value;
        }
    }

    get PayablesAccountingCard() { return this.EntityPM.PayablesAccountingCard; }
    set PayablesAccountingCard(value: string) {
        if (this.EntityPM.PayablesAccountingCard != value) {
            this.EntityPM.PayablesAccountingCard = value;
        }
    }

    //get ExternalId2() { return this.EntityPM.ExternalId2; }
    //set ExternalId2(value: string) {
    //    if (this.EntityPM.ExternalId2 != value) {
    //        this.EntityPM.ExternalId2 = value;
    //    }
    //}

    //get ExternalAccountingBusinessArea() { return this.EntityPM.ExternalAccountingBusinessArea; }
    //set ExternalAccountingBusinessArea(value: string) {
    //    if (this.EntityPM.ExternalAccountingBusinessArea != value) {
    //        this.EntityPM.ExternalAccountingBusinessArea = value;
    //    }
    //}

    public IsExternalByProductsVisible: boolean = false;
    private isExternalByProductsRequestd: boolean = false;
    ExternalByProductsClicked() {
        if (!this.isExternalByProductsRequestd) {
            this.isExternalByProductsRequestd = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
    ApplyExternalByProducts() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Accounting advanced";
        logWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName };
        logWindow.Show('./Common/Components/Partners/AddEdit/ExternalAccountsByProductsComponent');
    }
}
