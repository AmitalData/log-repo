import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ServiceArgs {
    public http: HttpClient;
    public objectTableName: string;
}
