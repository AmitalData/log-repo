import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AccountingPaymentMethodPM} from '../../EntityPMs/AccountingPaymentMethodPM';

@Component({
    moduleId: module.id,
    templateUrl: './AccountingTab_AccountingPaymentMethod.html',
})

export class AccountingTab_AccountingPaymentMethod extends BaseComponent implements OnDestroy {
    public EntityPM: AccountingPaymentMethodPM = null;
    public ObjectTableName: string;
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
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
                    }
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

    get ARExternalId() { return this.EntityPM.ARExternalId; }
    set ARExternalId(value: string) {
        if (this.EntityPM.ARExternalId != value) {
            this.EntityPM.ARExternalId = value;
        }
    }


    get APExternalId() { return this.EntityPM.APExternalId; }
    set APExternalId(value: string) {
        if (this.EntityPM.APExternalId != value) {
            this.EntityPM.APExternalId = value;
        }
    }
}
