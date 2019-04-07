import {Component, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {QuotePM} from '../../EntityPMs/QuotePM';
import {QuoteTool} from '../../Tools';
import {AppTool} from '../../../Infrastructure/Tools';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    moduleId: module.id,
    templateUrl: './QuoteHelperComponent.html',
})

export class QuoteHelperComponent implements OnDestroy {
    public EntityPM: QuotePM;
    public IsFollowupsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {

        this.IsFollowupsVisible = FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups");

        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {
            this.Listen();
            this.BuildComponent();
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;                        
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
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

    private BuildComponent() {
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
            ServiceLocator.SendTotangoUserActivity("Quote", "Notes update");
        }
    }
}
