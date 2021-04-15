

import {Injectable} from '@angular/core';
import { QueryFilterItem } from '../../Report/Components/Filters/QueryFilterItem';

@Injectable()

export class SchedulerDetails {

    public FTPDetails: FTPSchedulerDetails;
    public ReportDetails: ReportSchedulerDetails;
    public SendIfEmpty: boolean;

}

export class FTPSchedulerDetails {

    public Host: string;
    public Folder: string;
    public UserName: string;
    public Password: string;
    public From: string;
    public Subject: string;
    public Prefix: string;
    public Extension: string;
    public Suffix: string;
    public IsSFTP: boolean;

}

export class ReportSchedulerDetails {
    public CreatedByUserId: string;
    public Recepients: ReportSchedulerRecepients;
    public ReportTemplateId: string;
    public ReportFilterItems: Array<QueryFilterItem>;
    public MainCustomerFieldName: string;
}

export class ReportSchedulerRecepients {
    public To: string;
    public Cc: string;
    public Bcc: string;
}

