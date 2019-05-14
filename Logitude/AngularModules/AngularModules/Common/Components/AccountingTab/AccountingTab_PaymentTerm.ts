import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {PaymentTermPM} from '../../EntityPMs/PaymentTermPM';

@Component({
    moduleId: module.id,
    templateUrl: './AccountingTab_PaymentTerm.html',
})

export class AccountingTab_PaymentTerm extends BaseComponent implements OnDestroy {
    public EntityPM: PaymentTermPM = null;
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

    get ExternalId() { return this.EntityPM.ExternalId; }
    set ExternalId(value: string) {
        if (this.EntityPM.ExternalId != value) {
            this.EntityPM.ExternalId = value;
        }
    }
}
