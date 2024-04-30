import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, Observable, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';
// import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';


@Injectable()
export class DeclarationsBulkFeedWebService {
    private _http: HttpClient = ServiceHelper.HttpClient;
    private _apiUrl: string = ServiceHelper.GetLogitudeURL() + 'api/DeclarationsBulkFeedWebService/';
     private logtuideTableDataService: LogtuideTableDataService = LogtuideTableDataService.createInstance();

    constructor() {}

    checkDeclarationsInDisplayOnly(declarationIdsList: string [], allWithoutdeclarationIdsList: string[], checkboxAll: boolean, filter: ApiQueryFilters) : Promise<any[]> {
        const ajax: Observable<any> = this._http.post(
            this._apiUrl + "checkDeclarationsInDisplayOnly?" + this.logtuideTableDataService.apiQueryFilterToQueryString(filter),
            {
                declarationIdsList: declarationIdsList, 
                allWithoutdeclarationIdsList: allWithoutdeclarationIdsList, 
                checkboxAll: checkboxAll, 
            },
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
            }
        );

        return   this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax) as Promise<any[]>;
    }
}
