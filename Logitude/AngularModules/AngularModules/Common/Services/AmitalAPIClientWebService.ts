import { Injectable } from '@angular/core';
import { AmitalAPIWebServiceBase } from './AmitalAPIWebServiceBase';

@Injectable()
export class AmitalAPIClientWebService extends AmitalAPIWebServiceBase<AmitalApiClient> {    
    constructor() {
        super('AmitalAPIClient');
    }
}

export interface AmitalApiClient {
    Id: string;
    Name: string;
    Tenant: string;
    AzureApiRegisterName: string;
    AzureClientId: string;
    Token: string;
    AzureManagedApplObjId: string;
    CreateDate: string;
    UpdateDate: string;
    SecretExpired: string;
    SecretValue: string;
    Active: boolean;
}