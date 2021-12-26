import { Component, OnInit} from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AutomationEvent } from 'Infrastructure/DataContracts/AutomationEvent';
import { EventTypeListService } from 'Infrastructure/Services/StandardLists/EventTypeListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EventTypeList } from 'Infrastructure/EntityLists/EventTypeList';
import { AppTool } from 'Infrastructure/Tools';
@Component({
    selector: 'EventCreationResultComponent',
    templateUrl: './EventCreationResultComponent.html',
    inputs: [''],

})

export class EventCreationResultComponent extends BaseComponent implements OnInit {
    DataContext: any;
    EventTypes: EventTypeList[];
    SelectedEventType: EventTypeList;

    constructor() {
        super();
        this.DataContext = this;
    }

    ngOnInit(): void {

    }

    GetEventTypes() {
        var myService = new EventTypeListService();
        myService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                return;
            }
            var lists: EventTypeList[] = myResponse.Result;
            this.EventTypes = lists.filter(f => f.ObjectTableId == this.AutomationEvent.ObjectTableId && f.InActive == false && f.IsManualEntry == true);

            if (!AppTool.IsNullOrEmpty(this.AutomationEvent.EventTypeId)) {
                this.SelectedEventType = this.EventTypes.filter(d => d.Id == this.AutomationEvent.EventTypeId)[0];
            }
        });
    }

    public AutomationEvent: AutomationEvent;
    public EventNote: string;
    Run(automationEvent: AutomationEvent) {
        this.AutomationEvent = automationEvent;
        this.EventNote = automationEvent.NoteValue;
        this.GetEventTypes();
    }

    EventNoteChanged(value) {
        this.EventNote = value;
        this.AutomationEvent.NoteValue = value;
    }

    EventTypeComboBoxChanged(value: any) {
        if (value) {
            this.AutomationEvent.EventTypeId = value.Id;
        }
    }


}