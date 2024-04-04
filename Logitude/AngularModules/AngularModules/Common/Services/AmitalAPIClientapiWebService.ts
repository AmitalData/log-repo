import { Injectable } from '@angular/core';
import { AmitalAPIWebServiceBase } from './AmitalAPIWebServiceBase';

@Injectable()
export class AmitalAPIClientapiWebService extends AmitalAPIWebServiceBase<AmitalApiClientapi> {
    constructor() {
        super('AmitalAPIClientapi');
    }
}

export interface AmitalApiClientapi {
    Id: string;
    PartnerToken: string;
    PartnerId: string;
    PartnerName: string;
    ClientId: string;
    ClientName: string;
    SchemaId: string;
    SchemaName: string;
    ChunkSize: number;
    Priority: any;
    SaveAsXml: boolean;
    WebhookAddress: any;
    WebhookAuthType: any;
    WebhookHeaderParams: any;
    CreateDate: string;
    UpdateDate: string;
    Active: boolean;
    MoreParams: MoreParams;
}

export interface MoreParams {
    interface_type: string,
    host: string,
    user: string,
    password: string,
    directory: string
}