import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
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
    public EntityVersionPM: TariffVersionPM;
    public EntityLinePM: TariffLinePM;
    public ValidationErrorsList: string[] = [];
    public TariffType: string;
    constructor() {
        super();

        this.EntityVersionPM = new TariffVersionPM(null);
        this.EntityLinePM= new  TariffLinePM(null);
    }

    SetWindowArgs(args: any) {
        this.EntityVersionPM = args['CurrentVersion'];
        this.EntityLinePM = args['CurrentLine'];
        this.TariffType = args['TariffType'];

        this.Clone();
    }

    get StartDate() {
        return this.EntityVersionPM.StartDate;
    }
    set StartDate(value: Date) {
        if (this.EntityVersionPM.StartDate != value) {
            this.EntityVersionPM.StartDate = value;
        }
    }

    get ExpirationDate() {
        return this.EntityVersionPM.ExpirationDate;
    }
    set ExpirationDate(value: Date) {
        if (this.EntityVersionPM.ExpirationDate != value) {
            this.EntityVersionPM.ExpirationDate = value;
        }
    }

    get LineExpirationDate() {
        return this.EntityLinePM.ExpirationDate;
    }
    set LineExpirationDate(value: Date) {
        if (this.EntityLinePM.ExpirationDate != value) {
            this.EntityLinePM.ExpirationDate = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.TariffType == "AFC") {
            if (this.StartDate == null) {
                this.ValidationErrorsList.push("Satrt date must be less than start date");
            }
            if (this.ExpirationDate == null) {
                this.ValidationErrorsList.push("Expiration date must be less than start date");
            }

            if (this.ExpirationDate != null && DateTool.GetDateParts(this.ExpirationDate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.ValidationErrorsList.push("Can't set Expiration date Field to past date");
            }
        }

        else if (this.TariffType == "ASC") {


        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);

        if (this.TariffType == "AFC") {
            this.myCloner.AddField('StartDate');
            this.myCloner.AddField('ExpirationDate');
            this.myCloner.AddEntity(this.EntityVersionPM);
        }

        else if (this.TariffType == "ASC") {
            this.myCloner.AddField('LineExpirationDate');
            this.myCloner.AddEntity(this.EntityLinePM);
        }
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
