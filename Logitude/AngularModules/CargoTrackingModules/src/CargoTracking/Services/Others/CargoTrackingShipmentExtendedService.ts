import { Inject, Injectable } from '@angular/core';
import { HttpClient, JsonpClientBackend } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { DeclarationApprovalArgs } from 'src/CargoTracking/DataContracts/DeclarationApprovalArgs';
import { throwError } from 'rxjs';

@Injectable()

export class CargoTrackingShipmentExtendedService
{
    private _apiUrl: string;
    public authHeaders = ServiceHelper.GetHeadersWithToken();

    constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string)
    {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/CargoTrackingShipments';
    }
    GetCargoShipmentPMByEntityId(entityId: string)
    {
        return this._http.get(this._apiUrl + '/GetCargoShipmentPMByEntityId?' + 'entityId=' + entityId, this.authHeaders).pipe(
            map((response: ServiceResponse) =>
            {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetCargoShipmentPMBySecurityKey(securityKey: string, tenant: number,)
    {
        return this._http.get(this._apiUrl + '/GetCargoShipmentPMBySecurityKey?'
            + 'securityKey=' + securityKey
            + '&tenant=' + tenant).pipe(
                map((response: ServiceResponse) =>
                {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse = response;
                    return serviceResponse;
                }),
                catchError(null));
    }
    GetMainCargoShipmentPMBySecurityKey(securityKey: string, tenant: number)
    {
        return this._http.get(this._apiUrl + '/GetMainCargoShipmentPMBySecurityKey?'
            + 'securityKey=' + securityKey
            + '&tenant=' + tenant).pipe(
                map((response: ServiceResponse) =>
                {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse = response;
                    return serviceResponse;
                }),
                catchError(null));
    }
    PostDeclarationApprovalResponse(args: DeclarationApprovalArgs) {

        var url = '/PostDeclarationApprovalResponse';
        var callUrl = this._apiUrl.concat(url);
        return this._http.post(callUrl, args, { headers: ServiceHelper.GetHeaders() })
        .pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(err=> throwError(err?.error?.ErrorMessage)));
    }
}
