import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

import { PortPM } from '../../EntityPMs/PortPM';

import { PortValidator } from '../../Validators/PortValidator';
 
@Injectable()

export class PhysicalCheckExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PhysicalCheck';
    }

    GetPhysicalCheckRequest(mainInterfaceCode: string, communicationLogId: string, tenant: number, stepFilter: Array<number>, suppressHugeData?: boolean) {
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
            //GetCommunicationLogStepsDocumentDataBystringStepFilter(string mainInterfaceCode, string communicationLogId, int tenant, string stringStepFilter)
            this._apiUrl + '/GetPhysicalCheckRequest/' + '?mainInterfaceCode=' + mainInterfaceCode + '&communicationLogId=' + communicationLogId + '&tenant=' + tenant + '&stringStepFilter=' + $stepFilter + "&suppressHugeData=" + suppressHugeDataValue,

            { headers: authHeader }
        ).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
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

            { headers: authHeader }
        ).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
}
