

import {Injectable} from '@angular/core';
import { QueryFilterItem } from '../../Report/Components/Filters/QueryFilterItem';

@Injectable()

export class SchedulerDetails {

    public FTPDetails: FTPSchedulerDetails;
    public ReportDetails: ReportSchedulerDetails;

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
    public Recepients: string;
    public ReportTemplateId: string;
    public ReportFilterItems: Array<QueryFilterItem>;
}

