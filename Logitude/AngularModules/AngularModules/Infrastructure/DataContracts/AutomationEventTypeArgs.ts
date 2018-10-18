

import {Injectable} from '@angular/core';

@Injectable()

export class AutomationEventTypeArgs {

    public EntityId: string;
    public Tenant: number;
    public EventTypeCodeList: string[];
    public ObjectTableId: string;
    public LoggedContactId: string;

}

