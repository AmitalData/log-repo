import { Component, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {QuoteOPPM} from '../../EntityPMs/QuoteOPPM';
import {QuoteTool} from '../../Tools';
import {AppTool} from '../../../Infrastructure/Tools';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomerPMService } from '../../../Common/Services/StandardPMs/CustomerPMService';
import { CustomerPM } from '../../../Common/EntityPMs/CustomerPM';

@Component({
    
    templateUrl: './QuoteHelperComponent.html',
})

export class QuoteHelperComponent implements OnDestroy {
    public EntityPM: QuoteOPPM;
    public CustomerPM: CustomerPM;
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
                    
                        this.GetSingleCustomer();
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
        this.GetSingleCustomer();
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
            ServiceLocator.SendTotangoUserActivity("Quote", "Notes update");
        }
    }

    private GetSingleCustomer() {
        var customerService = new CustomerPMService();
        customerService.get(this.EntityPM.CustomerId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.CustomerPM = myResponse.Result;
                    this.CurrentSession.FireEvent("CustomerSalesNotesChanged");
                }
            }
        });
    }
}
