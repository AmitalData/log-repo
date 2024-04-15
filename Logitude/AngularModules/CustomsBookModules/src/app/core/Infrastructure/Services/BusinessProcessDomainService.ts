import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class BusinessProcessDomainService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BusinessProcessDomain';
    }

    GetQueuesWithCounts(myFilter: string) {
        var url = this._apiUrl + '/GetQueuesWithCounts?myFilter=' + myFilter;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: Array<QueueData> = new Array<QueueData>();
                for (var key in allLists) {
                    var entity: QueueData;
                    entity = this.MapJsonToEntityListQueueData(allLists[key]);
                    myList.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myList;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTeamsForLoggedUser(loggedUserId: string) {
        var url = this._apiUrl + '/GetTeamsForLoggedUser?loggedUserId=' + loggedUserId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityListQueueData(jsonList: any) {
        var entityList: QueueData;
        entityList = new QueueData();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}

export class QueueData {    
    public Count: number;
    public QueueId: string;
    public QueueName: string;
}
