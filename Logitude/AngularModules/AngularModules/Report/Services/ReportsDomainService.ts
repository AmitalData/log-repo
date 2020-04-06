import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import { ParticipantList } from '../EntityLists/ParticipantList';

@Injectable()

export class ReportsDomainService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'
    }

    GetActivityStatus(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'


        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetIQueryableEntityList?tenant=' + currentTenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: Array<ParticipantList> = new Array<ParticipantList>();
                for (var key in allLists) {
                    var entity: ParticipantList;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    myList.push(entity);
                }
                return myList;
            }));
        });

    }
    GetBusinessUnitLists(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetBusinessUnitLists?tenant=' + currentTenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myList:any=response;
                
                return myList;
            }));
        });

    }
    GetAdditionalServicesByTenant(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'


        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAdditionalServicesByTenant?tenant=' + currentTenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myList: any = response;

                return myList;
            }));
        });

    }
    GetProductTypesByTenant(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetProductTypesByTenant?tenant=' + currentTenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myList: any = response;

                return myList;
            }));
        });

    }
    GetLeadSourceLists(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetLeadSourceLists?tenant=' + currentTenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myList: any = response;

                return myList;
            }));
        });

    }
    UploadStaticFile(fileUploadParamerter: any) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/putuploadstaticfile', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
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
