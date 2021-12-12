import {Injectable} from '@angular/core';

@Injectable()
export class AutomationEvent {
    public EventTypeId : string;
    public NoteValue: string;
    public ObjectTableName: string;
    public ObjectTableId: string;
}