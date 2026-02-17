import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentPM} from '../EntityPMs/ShipmentPM';
import {ShipmentPMService} from '../Services/StandardPMs/ShipmentPMService';

@Injectable()

export class SplitShipmentService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SplitShipment';
    }

    Split(entityPM: SplitShipmentHelper) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: SplitShipmentHelper = this.MapSplitShipmentHelper(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: SplitShipmentHelper = this.MapSplitShipmentHelper(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    private MapSplitShipmentHelper(jsonPM: any, getCallMap: boolean = true, entityPM: SplitShipmentHelper = null) {
        if (!entityPM) {
            entityPM = new SplitShipmentHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else if (property === "Shipment") {
                if (jsonPM[property]) {
                    var myShipmentPMService = new ShipmentPMService();
                    entityPM[property] = myShipmentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "SplitPackages") {

                entityPM.SplitPackages = new Array<SplitPackage>();

                for (var item in jsonPM.SplitPackages) {
                    var jItem = jsonPM.SplitPackages[item];

                    var newItemPM: SplitPackage = this.MapSplitPackage(jItem);

                    entityPM.SplitPackages.push(newItemPM);
                }
            }

            else {
                entityPM[property] = jsonPM[property];
            }
        }

        return entityPM;
    }
    private MapSplitPackage(jsonItem: any) {
        var entity: SplitPackage = new SplitPackage();
        var jsonItemKeys = Object.keys(jsonItem);

        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entity[property] = jsonItem[property];
        }

        return entity;
    }

}

export class SplitShipmentHelper {
    public OldShipmentId: string;
    public NewShipmentId: string;
    public Shipment: ShipmentPM;
    public SplitPackages: SplitPackage[] = [];
}

export class SplitPackage {
    public Id: string;
    public IsSplit: boolean;
    public IsPartialSplit: boolean;
    public ParentId: string;
    public Quantity: number;
    public Weight: number;
    public Volume: number;
}