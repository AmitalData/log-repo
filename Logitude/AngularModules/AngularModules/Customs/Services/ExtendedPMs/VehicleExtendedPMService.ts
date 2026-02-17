import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { VehiclePM } from '../../EntityPMs/VehiclePM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class VehicleExtendedPMService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Vehicle';
    }

    GetVehicleByVehicleChassisNumberOrRichbitFileNumber( vehicleChassisNumber: string,  richbitFileNumber:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetVehicleByVehicleChassisNumberOrRichbitFileNumber?vehicleChassisNumber=' + vehicleChassisNumber + '&richbitFileNumber=' + richbitFileNumber, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
               // serviceResponse.Result = response.json();

                var pm = response.json();

                var entity: VehiclePM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    CheckIfVehicleExistByChassisNumber_old(vehicleChassisNumber: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/CheckIfVehicleExistByChassisNumber/?' + '&vehicleChassisNumber=' + vehicleChassisNumber, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    CheckIfVehicleExistByChassisNumber(vehicleChassisNumber: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http
                .get(this._apiUrl + '/GetCheckIfVehicleExistByChassisNumber/?' + '&vehicleChassisNumber=' + vehicleChassisNumber,
                { headers: authHeader }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response.json();

                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetDuplicatedVehInSameDeclaration(declarationId: string, vehiclesNumbers: string, chassissNumbersString: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDuplicatedVehInSameDeclaration?declarationId=' + declarationId + '&richbitNumbersString=' + vehiclesNumbers + '&chassissNumbersString=' + chassissNumbersString, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                // serviceResponse.Result = response.json();

                var pm = response.json();

                //var entity: VehiclePM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    GetCheckRichbitNumbersError(vehiclesNumbers: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckRichbitNumbersError?richbitNumbersString=' + vehiclesNumbers, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                // serviceResponse.Result = response.json();

                var pm = response.json();

                //var entity: VehiclePM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    GetVehiclesByRichbitFileNumbers(richbitNumbersString: string) {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetVehiclesByRichbitFileNumbers/?' + '&richbitNumbersString=' + richbitNumbersString, { headers: authHeader }).map(response => {

                var allLists = response.json();
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
            }).catch(ServiceHelper.HandleServiceError);
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