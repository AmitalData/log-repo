import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';

@Injectable()

export class FVRWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FVRWebService';
    }

    SendFVR(myAirlineId: string, myFromPortId: string, myToPortId: string, myETD: Date, myETA: Date, myVolume: number, myGrossWeight: number, myVolumeUnitCode: string, myGrossWeightUnitCode: string, myShipmentId: string, myBookingId: string, myRecipient: string) {
        return defer(() => {
            var args = new FVRServiceArgs();
            args.AirlineId = myAirlineId;
            args.ShipmentId = myShipmentId;
            args.BookingId = myBookingId;
            args.FromPortId = myFromPortId;
            args.ToPortId = myToPortId;
            args.ETD = myETD;
            args.ETA = myETA;
            args.Volume = myVolume;
            args.GrossWeight = myGrossWeight;
            args.VolumeUnitCode = myVolumeUnitCode;
            args.GrossWeightUnitCode = myGrossWeightUnitCode;
            args.Recipient = myRecipient;

            var mappedEntity: FVRServiceArgs = this.MapJsonToFVRServiceArgs(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var myJsonResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    SimulateXML(xmlString: string, myShipmentId: string, myBookingId, isFNA: boolean) {
        var url = this._apiUrl + '/GetSimulateXML?xmlString=' + xmlString + '&myShipmentId=' + myShipmentId + '&myBookingId=' + myBookingId + '&isFNA=' + isFNA;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: FVASimulatorResult = new FVASimulatorResult();

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

    GetCopyFlightsSchedulesPorts(myResponseIds: string) {
        var url = this._apiUrl + '/GetCopyFlightsSchedulesPorts?myResponseIds=' + myResponseIds;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var listMapped: Array<FlightSchedulePort> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: FlightSchedulePort = this.MapFlightSchedulePort(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    private MapJsonToFVRServiceArgs(jsonPM: any, getCallMap: boolean = true, entity: FVRServiceArgs = null) {
        if (!entity) {
            entity = new FVRServiceArgs();
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

    private MapFlightSchedulePort(jsonList: any) {
        var entityPM: FlightSchedulePort = new FlightSchedulePort();

        if (jsonList) {
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }
        
        return entityPM;
    }
}

export class FVRServiceArgs {
    public AirlineId: string;
    public ShipmentId: string;
    public BookingId: string;
    public FromPortId: string;
    public ToPortId: string;
    public ETD: Date;
    public ETA: Date;
    public Volume: number;
    public GrossWeight: number;
    public VolumeUnitCode: string;
    public GrossWeightUnitCode: string;
    public Recipient: string;
}

export class FVRResultClass {
    public Id: number;
    public Tenant: number;
    public IsValid: boolean;
    public RequestId: string;
    public Errors: string[];
}

export class FVASimulatorResult {
    public RequestId: string;
    public FromPortId: string;
    public ToPortId: string;
    public AirlineId: string;
    public ETD: Date;
    public IsValid: boolean;
    public IsValidXML: boolean;
    public Errors: string[];
}

export class FlightSchedulePort {
    public PortId: string;
    public PortCode: string;
    public PortName: string;
    public PortCountryCode: string;
    public PortCountryName: string;
}
