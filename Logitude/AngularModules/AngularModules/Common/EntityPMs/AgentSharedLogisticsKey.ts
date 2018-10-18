



import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';

export class AgentSharedLogisticsKey {
    public SharedKey: string;
    public Agent1Tenant: number;
    public Agent2Tenant: number;
    public CreateDate: Date;
    public CreatedByUserEmail: string;
    public ApprovedByUserEmail: string;
    public ApproveDate: Date;
    public InactiveDate: Date;  
    public InactiveByUserEmail: string;
    public StatusCode: string;
}