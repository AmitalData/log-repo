import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';

@Injectable()

export class ShipmentContainersWebService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentContainersWebService';
    }

    GetContainerStatusResult(shipmentId: string, containerId: string, isContainer: boolean) {

        var url = this._apiUrl + '/GetContainerStatusRequest?shipmentId=' + shipmentId + '&containerId=' + containerId+ '&isContainer=' + isContainer;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        //mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    Simulate(entity: ShipmentContainerSimulator) {
        return defer(() => {

            var mappedEntity: ShipmentContainerSimulator = this.MapJsonToShipmentContainerSimulator(entity, false);

            return this._httpClient.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: ShipmentContainerSimulator = this.MapJsonToShipmentContainerSimulator(myJsonResult, true, entity);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToShipmentContainerSimulator(jsonPM: any, getCallMap: boolean = true, entity: ShipmentContainerSimulator = null) {
        if (!entity) {
            entity = new ShipmentContainerSimulator();
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

export class ShipmentContainerSimulator {
    public XmlString: string;
    public AnalyzeQueueId: string;
    public FilesCount: number;
    public Success: boolean;
    public Errors: string[] = [];
    public IsFromContainer: boolean;
    public ShipmentId: string;
    public ContainerNumber: string;
}
