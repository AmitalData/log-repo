import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';

@Injectable()
export abstract class AmitalAPIWebServiceBase<T> {
    public http: HttpClient = ServiceHelper.HttpClient;
    public apiUrl: string = '';
    public headers: HttpHeaders = ServiceHelper.GetHttpHeaders().headers;

    constructor(controller: string) {
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/' + controller;
    }

    get(id: string | number): Promise<T> {
        return this.http.get(this.apiUrl + '/get/' + id, { headers: this.headers }).toPromise() as Promise<T>;
    }

    getAll(): Promise<T[]> {
        return this.http.get(this.apiUrl + '/getAll', { headers: this.headers }).toPromise() as Promise<T[]>
    }


    delete(id: string): Promise<boolean> {
        return this.http.delete(this.apiUrl + '/delete/' + id, { headers: this.headers }).toPromise() as Promise<boolean>;
    }

    update(data: T): Promise<boolean> {
        return this.http.put(this.apiUrl + '/put/' + data['Id'], data, { headers: this.headers }).toPromise() as Promise<boolean>;
    }

    add(data: T): Promise<T> {
        return this.http.post(this.apiUrl + '/post', data, { headers: this.headers }).toPromise() as Promise<T>;
    }
}