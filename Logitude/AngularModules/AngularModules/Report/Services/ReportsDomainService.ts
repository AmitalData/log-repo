import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ParticipantList} from '../EntityLists/ParticipantList';
@Injectable()

export class ReportsDomainService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'
    }

    GetActivityStatus(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetIQueryableEntityList?tenant=' + currentTenant, {
                headers: authHeader
            }).map(response => {

                var allLists: ParticipantList[] = response.json();
                var myList: Array<ParticipantList> = new Array<ParticipantList>();
                for (var key in allLists) {
                    var entity: ParticipantList;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.push(entity);
                }
                return myList;
            });
        });

    }
    GetBusinessUnitLists(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetBusinessUnitLists?tenant=' + currentTenant, {
                headers: authHeader
            }).map(response => {

                var myList:any=response.json();
                
                return myList;
            });
        });

    }
    GetAdditionalServicesByTenant(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAdditionalServicesByTenant?tenant=' + currentTenant, {
                headers: authHeader
            }).map(response => {

                var myList: any = response.json();

                return myList;
            });
        });

    }
    GetProductTypesByTenant(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetProductTypesByTenant?tenant=' + currentTenant, {
                headers: authHeader
            }).map(response => {

                var myList: any = response.json();

                return myList;
            });
        });

    }
    GetLeadSourceLists(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetLeadSourceLists?tenant=' + currentTenant, {
                headers: authHeader
            }).map(response => {

                var myList: any = response.json();

                return myList;
            });
        });

    }
    UploadStaticFile(fileUploadParamerter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/putuploadstaticfile', JSON.stringify(fileUploadParamerter),{
                headers: authHeader,

            }).map(response => {
                var result = response.json();

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }
    MapJsonToEntityList(jsonList: any) {

        var entityList: ParticipantList;
        entityList = new ParticipantList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }
}