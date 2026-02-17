import {Component} from '@angular/core';
import {ActivityNoteItem} from './ActivityGeneralTabComponent';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {ActivityNotePM} from '../../../../CRM/EntityPMs/ActivityNotePM'; 
import {ActivityPM} from '../../../../CRM/EntityPMs/ActivityPM'; 
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools'; 

@Component({
    moduleId: module.id,
    templateUrl: './AddEditActivityNotesComponent.html',
})

export class AddEditActivityNotesComponent {
    private activityPM: ActivityPM = null;
    private entityPM: ActivityNotePM = null;
    private isNew: boolean;
    public DataContext: ActivityNoteItem;
    public ObjectTableName: string = "ActivityNote";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(args: ActivityNoteItem) {
        this.activityPM = args.activityPM;
        this.entityPM = args.entityPM;
        this.isNew = args.isNew;
        this.DataContext = args;
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.entityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            if (AppTool.IsNullOrEmpty(this.DataContext.entityPM.Notes)) {
                errors.push("Notes is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.isNew) {
                if (this.DataContext.activityPM.ActivityNotes.indexOf(this.DataContext.entityPM) == -1) {
                    this.DataContext.activityPM.AddActivityNote(this.DataContext.entityPM);
                }
                this.isNew = false;
            }
            else if (this.DataContext.isDataEdited) {
                var maxDate: Date = new Date();
                var nowData = DateTool.GetCurrentDateTimeAsUtc();

                if (this.DataContext.activityPM.ActivityNotes.length > 0) {
                    this.DataContext.activityPM.ActivityNotes.forEach(item => {
                        if (item.UpdateDate.valueOf > maxDate.valueOf) {
                            maxDate = item.UpdateDate;
                        }
                    });
                }

                if (maxDate == null) {
                    maxDate = nowData;
                }
                else {
                    maxDate.setUTCHours(maxDate.getUTCHours() + 1);
                }

                this.DataContext.entityPM.UpdateDate = maxDate;
            }
            this.DataContext.father.BuildNotes();
        }
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.entityPM);
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('PostToFollowers');
        this.myCloner.AddField('CreateDate');
        this.myCloner.AddField('CreatedByUserName');
        this.myCloner.AddField('UpdateDate');
        this.myCloner.AddField('UpdatedByUserName');
        this.myCloner.AddEntity(this.entityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
