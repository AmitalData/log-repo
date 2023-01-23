import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {InterestTransactionPM} from '../../EntityPMs/InterestTransactionPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';


@Component({
    selector: 'InterestTransactionNotesComponent',
    moduleId: './Accounting/Components/Others/',
    templateUrl: 'InterestTransactionNotesComponent.html',
})

export class InterestTransactionNotesComponent extends BaseComponent {

    private CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;
    public EntityPM: InterestTransactionPM;
    public WarningErrorsList: string[] = [];
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "InterestTransaction";
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public Notes: string = "";
    public interestTransactionPM: InterestTransactionPM;


    constructor(private entityResourceService: EntityResourceService) {
        super();
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
    }


    SetWindowArgs(args: any) {
        var loggedContact = SessionLocator.LoggedUserPM;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = args['interestTransaction'];
            this.EntityPM.CreateDateTime = new Date();
            this.EntityPM.UpdateDateTime = new Date();
            this.EntityPM.CreatedByUserId = loggedContact.Id;
            this.EntityPM.UpdatedByUserId = loggedContact.Id;
            this.IsResourcesReady = true;
        });

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //#endregion

    //#region Buttons Handlers
    OkButtonClicked() {
        this.SetInterestTransactionNotes();

        this.CurrentSession.CurrentEditComponent.SaveChanges();

        const SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            SaveCompletedEvent.unsubscribe();
            if (isSaveSuccess) {

                this.CurrentSession.StopBusyIndicator();

            }
        });

        this.CurrentSession.CloseCurrentWindow();

    }

    private SetInterestTransactionNotes() {
        this.EntityPM.Notes = this.Notes;
        this.CurrentSession.CurrentEditComponent.EntityPM = this.EntityPM;
        this.CurrentSession.CurrentEditComponent.EntityId = this.EntityPM.Id;
        this.CurrentSession.CurrentEditComponent.ObjectTableName = this.ObjectTableName;
    }

    //#endregion

}
