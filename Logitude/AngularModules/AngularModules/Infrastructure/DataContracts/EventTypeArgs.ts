

import {Injectable} from '@angular/core';

@Injectable()

export class EventTypeArgs {

    public EntityId: string;
    public Tenant: number;
    public EventTypeCodeList: string[];
    public EventTypeList: EventTypeClass[];
    public ObjectTableId: string;
    public LoggedContactId: string;

}


export class EventTypeClass{

    Code: string;
    Date: Date;
    constructor(code: string, date: Date) {
        this.Code = code;
        this.Date = date;
    }

}

