import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {InterestTransactionPM} from '../../EntityPMs/InterestTransactionPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { InterestTransactionExtendedListService } from '../../Services/ExtendedLists/InterestTransactionExtendedListService';

@Component({
    selector: 'InterestTransactionNotesComponent',
    moduleId: './Accounting/Components/Others/',
    templateUrl: 'InterestTransactionNotesComponent.html',
})

export class InterestTransactionNotesComponent extends BaseComponent {

     public InterestTransactionService: InterestTransactionExtendedListService;

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


    constructor(private entityResourceService: EntityResourceService,private CD: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.InterestTransactionService = new InterestTransactionExtendedListService();
    }


    SetWindowArgs(args: any) {
        var loggedContact = SessionLocator.LoggedUserPM;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = args['interestTransaction'];
            this.Notes=this.EntityPM.Notes;
            this.EntityPM.CreateDateTime = new Date();
            this.EntityPM.UpdateDateTime = new Date();
            this.EntityPM.UpdatedByUserName = loggedContact.DontShowLocal ? loggedContact.EnglishName : (loggedContact.LocalName||loggedContact.EnglishName);
            this.IsResourcesReady = true;
        });

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.SetInterestTransactionNotes();

        this.CurrentSession.StartBusyIndicatorLoading();
        this.InterestTransactionService.PutInterestTransactionNotes(this.EntityPM,this.EntityPM.Notes).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.EntityPM = mm.Result;
                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            }
            this.CD.detectChanges();
            this.CurrentSession.CloseCurrentWindow();
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = false;
            
        });
    }

    private SetInterestTransactionNotes() {
        this.EntityPM.Notes = this.Notes;
        this.CurrentSession.CurrentEditComponent.EntityPM = this.EntityPM;
        this.CurrentSession.CurrentEditComponent.EntityId = this.EntityPM.Id;
        this.CurrentSession.CurrentEditComponent.ObjectTableName = this.ObjectTableName;
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
    }


}
