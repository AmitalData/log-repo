import {Component, OnInit,ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { JournalLinePM } from '../../../EntityPMs/JournalLinePM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';


@Component({

    templateUrl: './UpdateJournalLineComponent.html',
})

export class UpdateJournalLineComponent extends BaseComponent{

    private CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;
    public EntityPM: JournalLinePM;
    public WarningErrorsList: string[] = [];
    public ValidationErrorsList: string[] = [];
    public ObjectTableName = "JournalLine";
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public Notes: string = "";
    public line: number;
    public journalLine: JournalLinePM;
    public Reference1: string;
    public Reference2: string;
    public Reference3: string;

    constructor(private entityResourceService: EntityResourceService) {
        super();
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
    }

    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = args['journalLine'];
            this.Notes = this.EntityPM.Notes;
            this.Reference1 = this.EntityPM.Reference1;
            this.Reference2 = this.EntityPM.Reference2;
            this.Reference3 = this.EntityPM.Reference3;

            this.IsResourcesReady = true;
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.SetJournalLineProperties();
        var SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            SaveCompletedEvent.unsubscribe();
               if (isSaveSuccess) {
                   
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.StopBusyIndicator();

                }
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.CloseCurrentWindow();
    }



    private SetJournalLineProperties() {
        this.EntityPM.Notes = this.Notes;
        this.EntityPM.Reference1 = this.Reference1;
        this.EntityPM.Reference2 = this.Reference2;
        this.EntityPM.Reference3 = this.Reference3;
    }
}
