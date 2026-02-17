import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../Utilities/ServiceHelper';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {SessionInfo} from '../Utilities/SessionInfo';

@Injectable()

export class BusinessProcessDomainService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BusinessProcessDomain';
    }

    GetQueuesWithCounts(myFilter: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetQueuesWithCounts?myFilter=' + myFilter;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists: QueueData[] = response.json();
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

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetTeamsForLoggedUser(loggedUserId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetTeamsForLoggedUser?loggedUserId=' + loggedUserId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
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