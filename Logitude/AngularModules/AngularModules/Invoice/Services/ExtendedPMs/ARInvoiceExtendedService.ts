import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer } from 'rxjs';
import { ARInvoiceList } from 'Invoice/EntityLists/ARInvoiceList';

@Injectable()
export class ARInvoiceExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ARInvoiceExtended';
    }

    getInvoiceSequenceStatus(fromDate: Date, toDate: Date) {
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetInvoiceSequenceStatus/?' + '&fromDate=' + fromDate.toISOString() + '&toDate=' + toDate.toISOString(), ServiceHelper.GetHttpHeaders())
                .pipe(map((response: any) => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = response;
                    var _mappedListsArray: Array<ARInvoiceList> = [];
                    if (serviceResponse.Result) {
                        for (var key in serviceResponse.Result) {
    
                            var entity: ARInvoiceList;
                            entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                            _mappedListsArray.push(entity);
    
                        }
                    }
    
                    serviceResponse.Result = _mappedListsArray;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    UpdateIsApproveDoneInARInvocie(invoiceId: string, approvalInProgress: boolean) {
        return defer(() => {    
            return this._http.put(this._apiUrl + '/UpdateIsApproveDoneInARInvocie/?' + '&invoiceId=' + invoiceId + '&approvalInProgress=' + approvalInProgress, null,ServiceHelper.GetHttpHeaders())
                .pipe(map((response: any) => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: ARInvoiceList;
        entityList = new ARInvoiceList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    } 
}
 