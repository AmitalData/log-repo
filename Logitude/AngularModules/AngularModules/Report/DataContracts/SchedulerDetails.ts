

import {Injectable} from '@angular/core';

@Injectable()

export class SchedulerDetails {

    public ReportDetails: ReportSchedulerDetails;

}

export class ReportSchedulerDetails {

    public Id: string;
    public Tenant: number;
    public Name: string;
    public Description: string;
    public ReportId: string;
    public CreateDate: Date;
    public CreatedbyUserID: string;
    public UpdateDate: Date;
    public UpdatedbyUserID: string;
    public FiltersXML: string;
    public Inactive: boolean;
    public Frequency: string;
    public Recipients: string;

}


