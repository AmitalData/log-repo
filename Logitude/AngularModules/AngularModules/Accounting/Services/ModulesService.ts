import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class ModulesService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
    }

    GetAccountPayablesSummary() {

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';

       

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAccountPayablesSummary', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAccountingReceivablesSummary() {

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';



        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAccountingReceivablesSummary', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }


}
