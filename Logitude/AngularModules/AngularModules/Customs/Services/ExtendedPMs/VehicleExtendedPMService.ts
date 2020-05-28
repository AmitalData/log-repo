import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { VehiclePM } from '../../EntityPMs/VehiclePM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class VehicleExtendedPMService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Vehicle';
    }

    GetVehicleByVehicleChassisNumberOrRichbitFileNumber( vehicleChassisNumber: string,  richbitFileNumber:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetVehicleByVehicleChassisNumberOrRichbitFileNumber?vehicleChassisNumber=' + vehicleChassisNumber + '&richbitFileNumber=' + richbitFileNumber, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
               // serviceResponse.Result = response;

                var pm = response;

                var entity: VehiclePM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    CheckIfVehicleExistByChassisNumber_old(vehicleChassisNumber: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');

        return defer(() => {
            return this._http.get(this._apiUrl + '/CheckIfVehicleExistByChassisNumber/?' + '&vehicleChassisNumber=' + vehicleChassisNumber, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                var res = response;
                serviceResponse.Result = res;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    CheckIfVehicleExistByChassisNumber(vehicleChassisNumber: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http
                .get(this._apiUrl + '/GetCheckIfVehicleExistByChassisNumber/?' + '&vehicleChassisNumber=' + vehicleChassisNumber,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDuplicatedVehInSameDeclaration(declarationId: string, vehiclesNumbers: string, chassissNumbersString: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDuplicatedVehInSameDeclaration?declarationId=' + declarationId + '&richbitNumbersString=' + vehiclesNumbers + '&chassissNumbersString=' + chassissNumbersString, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                // serviceResponse.Result = response;

                var pm = response;

                //var entity: VehiclePM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    GetCheckRichbitNumbersError(vehiclesNumbers: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckRichbitNumbersError?richbitNumbersString=' + vehiclesNumbers, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                // serviceResponse.Result = response;

                var pm = response;

                //var entity: VehiclePM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    GetVehiclesByRichbitFileNumbers(richbitNumbersString: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetVehiclesByRichbitFileNumbers/?' + '&richbitNumbersString=' + richbitNumbersString, ServiceHelper.GetHttpHeaders()).pipe(map((response: HttpResponse<any>) => {

                var allLists = response.body;
                var _mappedListsArray: Array<VehiclePM> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: VehiclePM;
                        entity = this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;

                var servertime = response.headers.get('ServerExecutionTime');

                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetIsVehicleAttachmentNumberIsMoreThenAllow(vehicleId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetIsVehicleAttachmentNumberIsMoreThenAllow/?' + '&vehicleId=' + vehicleId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                var pm = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: VehiclePM;
        entityPM = new VehiclePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }




}
