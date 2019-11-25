import { Component } from '@angular/core';
import { InterestBasesPeriodItem } from '../InterestBasesTypeDetailsTabComponent';
import { InterestBasesPeriodPM } from '../../../../../EntityPMs/InterestBasesPeriodPM';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditInterestBasesPeriodComponent.html',
})

export class AddEditInterestBasesPeriodComponent extends BaseComponent {
    public ObjectTableName: string = "InterestBasesPeriod";
    public DataContext: InterestBasesPeriodItem;
    public EntityPM: InterestBasesPeriodPM;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: InterestBasesPeriodItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity)
                if (this.DataContext.InterestBasesTypePM.InterestBasesPeriods.indexOf(this.EntityPM) == -1) {
                    this.DataContext.InterestBasesTypePM.AddInterestBasesPeriod(this.EntityPM);
                    this.DataContext.fatherComponent.InterestBasesPeriodsList.Insert(this.DataContext);
                this.DataContext.fatherComponent.BuildData();
                }
                this.CurrentSession.CloseCurrentWindow();
        }
    }

}
