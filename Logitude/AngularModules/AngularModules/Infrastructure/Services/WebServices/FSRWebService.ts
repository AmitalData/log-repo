import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ShipmentPM} from '../../../Shipment/EntityPMs/ShipmentPM';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class FSRWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FSRWebService';
    }

    SendFSR(shipmentId: string, objectTableId: string, myRecipient: string) {
        var url = this._apiUrl + '/GetSendFSR?shipmentId=' + shipmentId + '&objectTableId=' + objectTableId + '&myRecipient=' + myRecipient;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: FSRResultClass = new FSRResultClass();

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
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    SendFSRShipment(entityPM: ShipmentPM) {
        var mappedEntity: ShipmentPM;
        mappedEntity = this.MapJsonToEntityPM(entityPM, false);

        return defer(() => {
            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: FSRResultClass = new FSRResultClass();

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
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    SendBookingFSR(bookingId: string, objectTableId: string, myRecipient: string) {
        var url = this._apiUrl + '/GetSendBookingFSR?bookingId=' + bookingId + '&objectTableId=' + objectTableId + '&myRecipient=' + myRecipient;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: FSRResultClass = new FSRResultClass();

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
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    private MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: ShipmentPM = null) {
        if (!entityPM) {
            entityPM = new ShipmentPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        entityPM.IsDirty = false;

        if (getCallMap) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    private clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        return entityPM;
    }
}

export class FSRResultClass {
    public Id: number;
    public CreateDate: Date;
    public IsDemoTenant: boolean;
}
