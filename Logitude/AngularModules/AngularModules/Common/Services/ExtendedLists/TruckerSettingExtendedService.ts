import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';
;

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class TruckerSettingExtendedService {


    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TruckerSettingExtended';
    }

    getTruckerSettingByAddressId(addressId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetTruckerSettingByAddressId" + '?addressId=' + addressId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }

    getTruckerSettingForDefaultShipmentDelivery(fromAddressCityId: string, toAddressCityId: string, shipmentType: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        const fields = 'fromAddressCityId=' + (fromAddressCityId || "") + '&toAddressCityId=' + (toAddressCityId || "") + '&shipmentType=' + (shipmentType || "");

        return this._http.get(this._apiUrl + "/GetTruckerSettingForDefaultShipmentDelivery?" + fields, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }
}

