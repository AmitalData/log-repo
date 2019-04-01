import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BusinessHourHolidayArgs} from './NewBusinessHourAndHolidaysComponent';
import {BusinessHoursHolidayPM} from '../../../../Infrastructure/EntityPMs/BusinessHoursHolidayPM';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditBusinessHourHolidayComponent.html',
})

export class AddEditBusinessHourHolidayComponent extends BaseComponent {

    public ObjectTableName: string ="BusinessHoursHoliday";
    public DataContext: BusinessHourHolidayArgs;
    public ValidationErrorsList: string[] = [];
    public EntityPM: BusinessHoursHolidayPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: BusinessHourHolidayArgs) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.entityPM;
        this.Clone();
    }

    // Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.DataContext.CreateDatePicker == null) {
            if (this.DataContext.IsRecurring) {
                if (this.DataContext.Day == 0 && this.DataContext.Month == 0) {
                    errors.push("Holiday Date field is required");
                }
            }
            else {
                errors.push("Holiday Date field is required");
            }
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.isNew) {
                this.DataContext.isNew = false;
                if (this.DataContext.trigger.entityPM.BusinessHoursHolidays.indexOf(this.EntityPM) == -1) {
                    this.DataContext.trigger.entityPM.AddBusinessHoursHolidayPM(this.EntityPM);
                }
                if (this.DataContext.trigger.HolidaysDataList.indexOf(this.DataContext) == -1) {
                    this.DataContext.trigger.HolidaysDataList.push(this.DataContext);
                }
            }
            this.DataContext.trigger.fillHolidays();
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('HolidayName');
        this.myCloner.AddField('IsRecurring');
        this.myCloner.AddField('CreateDatePicker');
        this.myCloner.AddField('Day');
        this.myCloner.AddField('Height');
        this.myCloner.AddField('Month');
        this.myCloner.AddField('Inactive');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.entityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
