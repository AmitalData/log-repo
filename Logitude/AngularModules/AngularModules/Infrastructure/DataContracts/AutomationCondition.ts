

import {Injectable} from '@angular/core';

@Injectable()

export class AutomationCondition {


    public CreateDate: Date;
    public UpdateDate: Date;

    public Tenant: number;

    //public ObjectFieldId: string;
    public OperatorCode: string;
    public Value: string;
    public ConditionType: string;
    public CreatedByUserId: string;

    public UpdatedByUserId: string;
    public ObjectFieldType: string;
    //public PartnerObjectFieldId: string;
    public PartnerObjectFieldCode: string;
    public ObjectFieldCode: string;
    public IsValid;
}

