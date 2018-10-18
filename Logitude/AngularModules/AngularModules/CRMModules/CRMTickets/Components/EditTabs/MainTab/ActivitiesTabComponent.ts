import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {TicketMainTabComponent, ActivityItemClass} from '../MainTab/TicketMainTabComponent';
import {ActivityList} from '../../../../../CRM/EntityLists/ActivityList';


@Component({
    selector: 'ActivitiesTabComponent',
    moduleId: module.id,
    templateUrl: './ActivitiesTabComponent.html',
})

export class ActivitiesTabComponent extends BaseComponent {

    public EntityPM: TicketPM;
    public Trigger: TicketMainTabComponent;
    public ObjectTableName: string;
    public DataContext: ActivitiesTabComponent = this;
    public ActivitiesList: ActivityItemClass[] = [];

    constructor(public entityArgs: EntityArgs) {
        super();
        this.ActivitiesList = [];
    }

    InitTab(trigger: TicketMainTabComponent) {
        this.Trigger = trigger;
        this.EntityPM = this.Trigger.EntityPM;
        this.ObjectTableName = this.Trigger.ObjectTableName;
        this.ActivitiesList = this.Trigger.ActivitiesList;
    }

    Updated(arg: boolean) {
        if (arg) {
            this.Trigger.LoadActivities();
        }
    }
}