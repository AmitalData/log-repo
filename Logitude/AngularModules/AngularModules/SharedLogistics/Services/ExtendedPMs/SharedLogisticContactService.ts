import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SharedLogisticContactPM} from '../../../Common/EntityPMs/SharedLogisticContactPM';


@Injectable()
export class SharedLogisticContactService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SharedLogisticContact';
    }
   // GetSharedLogisticContactsbyCardId(string cardId, int tenant)
    getSharedLogisticContactsbyCardId(cardId: string, tenant: number) {


        return this._http.get(this._apiUrl + '/getsharedlogisticcontactsbycardid/?' + 'cardId=' + cardId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result:any = response;
            var entity: SharedLogisticContactPM;
            var SharedLogisticContactPMLists: SharedLogisticContactPM[];
            SharedLogisticContactPMLists = new Array<SharedLogisticContactPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                SharedLogisticContactPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = SharedLogisticContactPMLists;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }
    

    ContactInternetAccessInvitation(entityPM: SharedLogisticContactPM) {

        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/putcontactinternetaccessinvitation', JSON.stringify(entityPM), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;
                var entity: SharedLogisticContactPM;
                entity = this.MapJsonToEntityPM(pm);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );

    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: SharedLogisticContactPM;
        entityPM = new SharedLogisticContactPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

}

