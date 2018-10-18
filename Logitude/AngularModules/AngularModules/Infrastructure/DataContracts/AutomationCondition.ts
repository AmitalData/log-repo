

import {Injectable} from '@angular/core';

@Injectable()

export class AutomationCondition {

    public Id: string;
    public CreateDate: Date;
    public UpdateDate: Date;
    public AutomationsId: string;
    public Tenant: number;

    public ObjectFieldId: string;
    public OperatorCode: string;
    public Value: string;
    public ConditionType: string;
    public CreatedByUserId: string;

    public UpdatedByUserId: string;
    public ObjectFieldType: string;


}

