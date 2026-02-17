import {Input, Output, Component, EventEmitter, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties, UIPropertyArgs} from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {LogCalendarComponent, DayOfMonth} from '../../../../Infrastructure/Components/LogitudeComponents/LogCalendarComponent'
import {ControlsIdCounter} from '../../../../Infrastructure/Utilities/ControlsIdCounter';
import {TimeSelectComponent} from '../../../../Infrastructure/Components/LogitudeComponents/TimeSelectComponent'
import {ShipmentPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'OnCarriageDate',
    moduleId: module.id,
    templateUrl: './OnCarriageDateComponent.html',
    inputs: ['EntityPM', 'State'],
})

export class OnCarriageDateComponent extends BaseComponent implements OnInit {
    public IsOpened: boolean = false;
    public ObjectTableName: string = null;
    public State: string = "Departure";
    public DataContext = this;
    public EntityPM: ShipmentPackagePM;
    public ValidationErrorsList: string[] = [];
    @Output() PopupClosed = new EventEmitter<any>();
    constructor() {
        super();
    }

    ngOnInit() {
        this.Clone();
    }
    
    public get ExpectedDate() {
        if (this.State == "Departure") {
            return this.EntityPM.OnCarriageETD;
        }

        else {
            return this.EntityPM.OnCarriageETA;
        }
    }
    public set ExpectedDate(newValue: Date) {
        if (this.State == "Departure") {
            this.EntityPM.OnCarriageETD = newValue;
        }

        else {
            this.EntityPM.OnCarriageETA = newValue;
        }
    }
    
    public get ActualDate() {
        if (this.State == "Departure") {
            return this.EntityPM.OnCarriageATD;
        }

        else {
            return this.EntityPM.OnCarriageATA;
        }
    }
    public set ActualDate(newValue: Date) {
        if (this.State == "Departure") {
            this.EntityPM.OnCarriageATD = newValue;
        }

        else {
            this.EntityPM.OnCarriageATA = newValue;
        }
    }
    
    SetActualDateClicked() {
        this.ActualDate = DateTool.GetDateParts(this.ExpectedDate).DateObject;
    }

    OnSelectedDate_ActualChanged(date) {
        this.ActualDate = date.SelectedDate;
    }

    OnSelectedDate_ExpectedChanged(date) {
        this.ExpectedDate = date.SelectedDate;
    }
    
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (!DateTool.IsActualDateValid(this.ActualDate)) {
            this.ValidationErrorsList.push("Can't set actual date to future date");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.Clone();
            this.IsOpened = false;
            this.PopupClosed.emit(this);
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.ValidationErrorsList = [];
        this.IsOpened = false;
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this);
        this.myCloner.AddField('ExpectedDate');
        this.myCloner.AddField('ActualDate');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {        
        this.myCloner.RejectChanges();
    }
}