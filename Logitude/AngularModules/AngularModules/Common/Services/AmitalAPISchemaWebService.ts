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

    getRequestQuery(params: RequestQueryParams): Promise<RequestQuery[]> {
        return this.http.post(this.apiUrl + '/postRequestQuery', params, { headers: this.headers }).toPromise() as Promise<RequestQuery[]>
    }

    downloadRequest(id: string): Promise<any> {
        return this.http.get(this.apiUrl + '/getDownloadRequest', { 
            params: { id },
            headers: this.headers, 
            responseType: 'blob' 
        }).toPromise() as Promise<any>
    }

    requeue(ids: string[]): Promise<{success: boolean, message?: string}> {
        return this.http.post(this.apiUrl + '/postRequeue', { Ids: ids }, { headers: this.headers }).toPromise() as Promise<{success: boolean, message?: string}>
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

export interface RequestQuery {
    Id: string;
    CreateDate: Date;
    Tenant: string;
    PartnerName: string;
    ClientId: string;
    ClientName: string;
    ClientApi: string;
    StorageBlob: string;
    TaskName: string;
    TotalItems: number;
    Ref1: string | null;
    Ref2: string | null;
    Ref3: string | null;
    Ref4: string | null;
    IsParent: boolean;
    ParentId: string;
    ChunkIdx: number;
    OuterProess: string | null;
    TotalChunks: number;
    FileSize: number | null;
    HasError: boolean;
    ErrorMessage: string | null;
}

export interface RequestQueryParams {
    fromCreateDate: Date;
    reference: string | null;
    partner: string | null;
    clientApi: string | null;
    taskName: string | null;
    isParent: string | null;
    hasErrors: string | null;
    minItems: number | null;
    page: number | null;
    pageSize: number | null;
}