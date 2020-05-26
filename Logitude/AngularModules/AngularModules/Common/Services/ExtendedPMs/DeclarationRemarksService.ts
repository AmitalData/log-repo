import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { PortValidator } from '../../Validators/PortValidator';
import { DeclarationRemarks, Remarks } from '../../../Customs/EntityPMs/Extended/DeclarationRemarks';
 
@Injectable()

export class DeclarationRemarksService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Urouter';
    }
    GetSVCOrSRVStatusList(tenant: number, customFileNo: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(
            this._apiUrl + '/GetMSVGStatusList' + '?tenant=' + tenant + '&customFileNo=' + customFileNo,
            { headers: authHeader }
        ).map(response => {
            var entity: Remarks;
            entity = this.MapJsonToEntityPM(response.json());
            let remarkList: DeclarationRemarks[]=[];
            for (let item of entity.StatusItemlist) {
                remarkList.push(item);
            }
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = remarkList;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    GetINCorINAtatusList(tenant: number, customFileNo: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(
            this._apiUrl + '/GetMVKRStatusList' + '?tenant=' + tenant + '&customFileNo=' + customFileNo,
            { headers: authHeader }
        ).map(response => {
            var entity: Remarks;
            entity = this.MapJsonToEntityPM(response.json());
            let remarkList: DeclarationRemarks[] = [];
            for (let item of entity.StatusItemlist) {
                remarkList.push(item);
            }
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = remarkList;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    MapJsonToEntityPM(jsonPM: any) {
        var entityPM: Remarks;
        entityPM = new Remarks();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}
