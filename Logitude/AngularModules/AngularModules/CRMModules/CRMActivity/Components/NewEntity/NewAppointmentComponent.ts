import {Component, OnInit} from '@angular/core';
import {ActivityPM} from '../../../../CRM/EntityPMs/ActivityPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    selector: 'NewAppointmentComponent',
    moduleId: module.id,
    templateUrl: './NewAppointmentComponent.html',
})

export class NewAppointmentComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Activity";
    public DataContext: NewAppointmentComponent = this;
    public EntityPM: ActivityPM = new ActivityPM();

    constructor() {
        super();
    }

    ngOnInit() {


    }
}