import {Component, OnInit} from '@angular/core';
import {ActivityPM} from '../../../../CRM/EntityPMs/ActivityPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    selector: 'NewCallComponent',
    moduleId: module.id,
    templateUrl: './NewCallComponent.html',
})

export class NewCallComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Activity";
    public DataContext: NewCallComponent = this;
    public EntityPM: ActivityPM = new ActivityPM();

    constructor() {
        super();
    }

    ngOnInit() {


    }
}