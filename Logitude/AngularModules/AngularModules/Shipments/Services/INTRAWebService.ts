import {Injectable} from '@angular/core';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';

@Injectable()

export class INTRAWebService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/INTRAWebService';
    }

    Send(myShipmentId: string) {

        var url = this._apiUrl + '/GetMessageResult?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var mappedResult: INTRAResult = new INTRAResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    Validate(myShipmentId: string) {

        var url = this._apiUrl + '/GetMessageResultValidate?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var mappedResult: INTRAResult = new INTRAResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetContainerStatuses(ShipmentId: string, ContainerId:string) {

        var url = this._apiUrl + '/GetContainerStatuses?ShipmentId=' + ShipmentId + '&ContainerId=' + ContainerId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    ReadFTP() {

        var url = this._apiUrl + '/GetReadFTPFolder';

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var entity: INTTRASimulator;
                if (myJsonResult) {
                    entity = this.MapJsonToINTTRASimulator(myJsonResult, true, entity);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    Simulate(entity: INTTRASimulator) {
        return defer(() => {

            var mappedEntity: INTTRASimulator = this.MapJsonToINTTRASimulator(entity, false);

            return this._httpClient.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: INTTRASimulator = this.MapJsonToINTTRASimulator(myJsonResult, true, entity);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    SendEBooking(myShipmentId: string) {

        var url = this._apiUrl + '/GetSendEBooking?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var mappedResult: INTRAResult = new INTRAResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    ValidateBooking(myShipmentId: string) {

        var url = this._apiUrl + '/GetBookingMessageResultValidate?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var mappedResult: INTRAResult = new INTRAResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToINTTRASimulator(jsonPM: any, getCallMap: boolean = true, entity: INTTRASimulator = null) {
        if (!entity) {
            entity = new INTTRASimulator();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }
}
export class INTRAResult {
    public Id: string;
    public IsValid: boolean;
    public IsLimited: boolean;
    public Errors: string[] = [];
    public IsCarrierRegisteredToINTTRA: boolean;
    public IsCarrierRegisteredToBranch: boolean;
    public IsDemoTenant: boolean;
    public HasStockError: boolean;
    public IsStockPrepaid: boolean;
}
export class INTTRASimulator {
    public XmlString: string;
    public AnalyzeQueueId: string;
    public FilesCount: number;
    public Success: boolean;
    public Errors: string[] = [];
}
