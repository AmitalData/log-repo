

import {Injectable} from '@angular/core';

@Injectable()

export class SchedulerDetails {

    public FTPDetails: FTPSchedulerDetails;

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

}


