import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';


@Injectable()

export class PhysicalCheckExtendedPMService {
  private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
      this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PhysicalCheck';
    }

    GetPhysicalCheckRequest(mainInterfaceCode: string, communicationLogId: string, tenant: number, stepFilter: Array<number>, suppressHugeData?: boolean) {
        var $stepFilter = "";
        for (let a in stepFilter) {
            if ($stepFilter) {
                $stepFilter += ",";
            }
            $stepFilter += stepFilter[a];
        }
        var suppressHugeDataValue: boolean = false;
        if (suppressHugeData) {
            suppressHugeDataValue = true;
      }

        return this._http.get(
            //GetCommunicationLogStepsDocumentDataBystringStepFilter(string mainInterfaceCode, string communicationLogId, int tenant, string stringStepFilter)
            this._apiUrl + '/GetPhysicalCheckRequest/' + '?mainInterfaceCode=' + mainInterfaceCode + '&communicationLogId=' + communicationLogId + '&tenant=' + tenant + '&stringStepFilter=' + $stepFilter + "&suppressHugeData=" + suppressHugeDataValue,
          ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
          }), catchError(ServiceHelper.HandleServiceError));
    }
    GetClosedPhysicalCheck(mainInterfaceCode: string, communicationLogId: string, tenant: number, stepFilter: Array<number>, suppressHugeData?: boolean) {
        var authHeader = new Headers();
        var $stepFilter = "";
        for (let a in stepFilter) {
            if ($stepFilter) {
                $stepFilter += ",";
            }
            $stepFilter += stepFilter[a];
        }
        var suppressHugeDataValue: boolean = false;
        if (suppressHugeData) {
            suppressHugeDataValue = true;
        }
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(
            // edit API 
            this._apiUrl + '/GetClosedPhysicalCheck/' + '?mainInterfaceCode=' + mainInterfaceCode + '&communicationLogId=' + communicationLogId + '&tenant=' + tenant + '&stringStepFilter=' + $stepFilter + "&suppressHugeData=" + suppressHugeDataValue,

          ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
          }), catchError(ServiceHelper.HandleServiceError));
    }
}
