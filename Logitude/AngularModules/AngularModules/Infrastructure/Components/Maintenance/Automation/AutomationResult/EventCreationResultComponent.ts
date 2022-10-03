import { Component, OnInit} from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AutomationEvent } from 'Infrastructure/DataContracts/AutomationEvent';
import { EventTypeList } from 'Infrastructure/EntityLists/EventTypeList';

@Component({
    selector: 'EventCreationResultComponent',
    templateUrl: './EventCreationResultComponent.html',
    inputs: [''],

})

export class EventCreationResultComponent extends BaseComponent implements OnInit {
    DataContext: any;
    SelectedEventType: EventTypeList;
    public AutomationEvent: AutomationEvent;
    public EventNote: string;

    constructor() {
        super();
        this.DataContext = this;
    }

    ngOnInit(): void {

    }


    Run(automationEvent: AutomationEvent) {
        this.AutomationEvent = automationEvent;
        this.EventNote = automationEvent.NoteValue;
    }

    EventNoteChanged(value) {
        this.EventNote = value;
        this.AutomationEvent.NoteValue = value;
    }

}