import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: "./CashBookShortTitleComponent.html",
})
export class CashBookShortTitleComponent {
    public EntityPM: InterestReportPM;
    public isRTL: boolean = false;
    public IsVisibile: boolean;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this._entityResourceService.getEntityResourceByTableName("CashBook", 0).subscribe((response: any) => {
            this.IsVisibile = true;
        });


        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }


        }
    }


}
