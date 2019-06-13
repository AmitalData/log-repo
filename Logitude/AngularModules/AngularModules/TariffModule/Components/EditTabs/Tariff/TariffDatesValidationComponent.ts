import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { DateTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'TariffDatesValidationComponent',
    moduleId: module.id,
    templateUrl: './TariffDatesValidationComponent.html',
})

export class TariffDatesValidationComponent extends BaseComponent {

    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityPM: TariffVersionPM;
    public ValidationErrorsList: string[] = [];

    constructor() {
        super();
    }

    SetWindowArgs(arg: TariffVersionPM) {
        this.EntityPM = arg;
        this.Clone();
    }

    get StartDate() {
        return this.EntityPM.StartDate;
    }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;
        }
    }

    get ExpirationDate() {
        return this.EntityPM.ExpirationDate;
    }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }


    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.StartDate  == null) {
            this.ValidationErrorsList.push("Satrt date must be less than start date");
        }
        if (this.ExpirationDate == null) {
            this.ValidationErrorsList.push("Expiration date must be less than start date");
        }

        if (this.ExpirationDate != null && DateTool.GetDateParts(this.ExpirationDate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
            this.ValidationErrorsList.push("Can't set Expiration date Field to past date");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('ExpirationDate');
        this.myCloner.AddEntity(this.EntityPM);

    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
