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



export class AutomationSendInterface {


    constructor() {
        this.FTPDetails = new FTPAutomationDetails();
    }


   public InterfaceName: string;
    public SendVia: string;
    public Format: string;
    public ComputingPartnerId: string;
    public  FTBFolderId: string;
    public FTPDetails: FTPAutomationDetails;
    public  IsChanged: boolean;

}



export class FTPAutomationDetails {
    public Host :string;
    public Folder :string;
    public  UserName :string;
    public   Password :string;
    //public    From :string;
    //public   Subject :string;
    //public   Prefix :string;
    //public Suffix :string;
    //public   Extension :string;

    }
