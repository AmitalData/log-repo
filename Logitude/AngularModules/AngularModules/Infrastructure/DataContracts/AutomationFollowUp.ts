

import {DocumentTypeList} from '../../Common/EntityLists/DocumentTypeList';

import {Injectable} from '@angular/core';

@Injectable()

export class AutomationFollowUp {
    public EventTypeId : string;
    public DateValue: any;
    public NoteValue: string;
    public OwnerValue: string;
    public LegType: string;
    public DocumentTypeLists: FollowUpDocumentTypeList[];
    public OwnerFieldType: string;
    public ObjectTableName: string;
    public DateEscalationActionTimeIndicatorCode: string;
    public DateEscalationTime: number;

}


export class FollowUpDocumentTypeList {
    public Id: string;
    public Name: string;
    public Area: string;
}
