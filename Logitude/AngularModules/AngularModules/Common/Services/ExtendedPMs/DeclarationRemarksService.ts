import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import{ ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { DeclarationRemarks, Remarks } from '../../../Customs/EntityPMs/Extended/DeclarationRemarks';
 
@Injectable()

export class DeclarationRemarksService {
  private _http: HttpClient;
  private _apiUrl: string;
  constructor() {
    this._http = ServiceHelper.HttpClient;
    this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Urouter';
  }

  GetSVCOrSRVStatusList(tenant: number, customFileNo: string) {

    return this._http.get(
      this._apiUrl + '/GetMSVGStatusList' + '?tenant=' + tenant + '&customFileNo=' + customFileNo, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
        var entity: Remarks;
        entity = this.MapJsonToEntityPM(response);
        let remarkList: DeclarationRemarks[] = [];
        for (let item of entity.StatusItemlist) {
          remarkList.push(item);
        }
        var pmresponse: ServiceResponse;
        pmresponse = new ServiceResponse();
        pmresponse.Result = remarkList;
        return pmresponse;
      }), catchError(ServiceHelper.HandleServiceError));
  }
  GetINCorINAtatusList(tenant: number, customFileNo: string) {

    return this._http.get(
      this._apiUrl + '/GetMVKRStatusList' + '?tenant=' + tenant + '&customFileNo=' + customFileNo, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
        var entity: Remarks;
        entity = this.MapJsonToEntityPM(response);
        let remarkList: DeclarationRemarks[] = [];
        for (let item of entity.StatusItemlist) {
          remarkList.push(item);
        }
        var pmresponse: ServiceResponse;
        pmresponse = new ServiceResponse();
        pmresponse.Result = remarkList;
        return pmresponse;
      }), catchError(ServiceHelper.HandleServiceError));
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
