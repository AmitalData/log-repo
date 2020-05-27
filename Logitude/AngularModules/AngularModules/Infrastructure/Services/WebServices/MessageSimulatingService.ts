import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class MessageSimulatingService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/MessageSimulating';
    }

    Simulate(args: SimulatorArgs) {
        return defer(() => {
            var mappedEntity: SimulatorArgs = this.MapSimulatorArgs(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var myJsonResult = response;

                var mappedResult: SimulatorResult = new SimulatorResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    private MapSimulatorArgs(jsonPM: any, getCallMap: boolean = true, entity: SimulatorArgs = null) {
        if (!entity) {
            entity = new SimulatorArgs();
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

export class SimulatorArgs {
    public Id: number;
    public Tenant: number;
    public EntityId: string;
    public EntityName: string;
    public MessageIdentifier: string;
    public AirlineId: string;
    public House: string;
    public Master: string;
    public AirlinePrefix: string;
    public ShipmentNumber: string;
    public ShipmentLevelCode: string;
    public IsViaColoader: boolean;
    public ColoaderReference: string;
    public AnalyzeQueueId: string;
    public XmlText: string;
    public IsGLSHKISAC: boolean;
    public ISAC_Sender: string;
    public ReasonForRejection: string;
    public ReasonForAcknowledgement: string;
    public FSA: any;
    public FFA: any;
    public FVA: SimulatorFVA;
    public IsLocalAnalyze: boolean;
    public IsChampSimulator: boolean;
}

export class SimulatorFVA {
    public FromPortId: string;
    public ToPortId: string;
    public ETD: Date;
    public ETA: Date;
    public Volume: number;
    public GrossWeight: number;
    public VolumeUnitCode: string;
    public GrossWeightUnitCode: string;
    public Recipient: string;
    public AnswerOSI: string;
    public AnswerReasonForNoReply: string;
    public ScheduleInformations: FVASimulatorScheduleInformation[] = [];
}

export class FVASimulatorScheduleInformation {
    public Id: number;
    public FVAClassId: number;
    public AirlineId: string;
    public FlightNumber: string;
    public FromPortId: string;
    public ToPortId: string;
    public ETD: Date;
    public ETA: Date;
}

export class SimulatorResult {
    public Id: number;
    public IsValid: boolean;
    public Errors: string[];
}
