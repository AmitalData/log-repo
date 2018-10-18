import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SharedLogisticContactPM} from '../../../Common/EntityPMs/SharedLogisticContactPM';


@Injectable()
export class SharedLogisticContactService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SharedLogisticContact';
    }
   // GetSharedLogisticContactsbyCardId(string cardId, int tenant)
    getSharedLogisticContactsbyCardId(cardId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getsharedlogisticcontactsbycardid/?' + 'cardId=' + cardId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
    }
    

    ContactInternetAccessInvitation(entityPM: SharedLogisticContactPM) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/putcontactinternetaccessinvitation', JSON.stringify(entityPM), {
                headers: authHeader,

            }).map(response => {
                var pm = response.json();
                var entity: SharedLogisticContactPM;
                entity = this.MapJsonToEntityPM(pm);
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = entity;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
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

