import {Injectable} from '@angular/core';

import {AutomationSetValue} from './AutomationSetValue';
import {AutomationCondition} from './AutomationCondition';
import {AutomationFollowUp} from './AutomationFollowUp';
import {AutomationQueuedTask} from './AutomationQueuedTask';

@Injectable()

export class AutomatedBackup {


    public CreateDate: Date;
    public UpdateDate: Date;
    public Name: string;
    public Version: number;
    public ResultCode: string;
    public Description: string;
    public Delaytime: number;
    public DelaytimeOp: string;
    public SelectedDelaytimeFieldCode: string;
    public DelaytimeIndicator: string;
    public Type: string;
    public IsAutomationResultEmailAllActiveUsers: boolean;
    AautomationConditionLists: AutomationCondition[];
    AutomationSetValueLists: AutomationSetValue[];
    DelayAautomationConditionLists: AutomationCondition[];
    AutomationFollowUp: AutomationFollowUp;
    AutomationQueuedTask: AutomationQueuedTask;
    AutomationSetSLAValue: AutomationSetSLAValue;
    AutomationSendInterface: AutomationSendInterface;


}

export class AutomationSetSLAValue {
    SLAId: string;
    ObjectFieldCode: string;
}



