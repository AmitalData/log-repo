import { Injectable } from '@angular/core';
import { AmitalAPIWebServiceBase } from './AmitalAPIWebServiceBase';

@Injectable()
export class AmitalAPISchemaWebService extends AmitalAPIWebServiceBase<AmitalApiSchema> {    
    constructor() {
        super('AmitalAPISchema');
     }

    getSettings(): Promise<AmitalApiSettings> {
        return this.http.get(this.apiUrl + '/getSettings', { headers: this.headers }).toPromise() as Promise<AmitalApiSettings>
    }
}

export interface AmitalApiSchema {
    Id: string
    Name: string
    SchemaJson: string
    Ref1: string
    Ref2: string
    Ref3: string
    Ref4: string
    CreateDate: Date
    UpdateDate: Date
    Active: boolean
    Endpoint: string
    SchemaType: string
    SaveAsXml?: boolean
    ChunkSize: number
    Tenants: string[]
}

export interface AmitalApiSettings {
    baseAddress: string
    authAddress: string
    AuthScope: string
}
