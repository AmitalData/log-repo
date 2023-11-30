import { Inject, Injectable } from '@angular/core';
import { HttpClient, JsonpClientBackend} from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { DeclarationApprovalArgs } from 'src/CargoTracking/DataContracts/DeclarationApprovalArgs';


@Injectable()

export class CargoTrackingShipmentService {
    private _apiUrl: string;
    public authHeaders = ServiceHelper.GetHeadersWithToken();

    constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/ShipmentCargoTracking';
    }

    get(id: string) {
        return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, this.authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetShipmentPackages(id: string) {
        return this._http.get(this._apiUrl + '/GetShipmentPackages?' + 'id=' + id, this.authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetPartnersAddresses(partnersIds: string[]) {
        var IdsParameterString = "";
        if (partnersIds && partnersIds.length > 0) {
            partnersIds.forEach(el => {
                IdsParameterString += 'partnersIds=' + el + '&';
            });
        }


        return this._http.get(this._apiUrl + '/GetPartnersAddresses?' + IdsParameterString, this.authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetShipmentCustomsData(shipmentId: string) {
        return this._http.get(`${this._apiUrl}/GetShipmentCustomsData?shipmentId=${shipmentId}`, this.authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetDocumentsFilingsConnectedToShipment(id: string) {
        return this._http.get(this._apiUrl + '/GetDocumentsFilingsConnectedToShipment?' + 'id=' + id, this.authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }





    BuildURLParameters(object: any)
    {
        var urlparameters = '?';
        var mykeys = Object.keys(object);

        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = object[propName];

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }

            propValue = encodeURIComponent(propValue);
            urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
        }
        return urlparameters;
    }
}



