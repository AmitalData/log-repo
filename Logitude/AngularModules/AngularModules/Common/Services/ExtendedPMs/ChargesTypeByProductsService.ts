import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ChargesExternalAccountsByProductPM}  from '../../EntityPMs/ChargesExternalAccountsByProductPM';
import {ChargesExternalAccountsByProductPMService} from '../StandardPMs/ChargesExternalAccountsByProductPMService';

@Injectable()

export class ChargesTypeByProductsService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ChargesTypeByProducts';
    }

    GetChargesTypeExternalAccountsByProducts(myChargesTypeId: string) {
        var url = this._apiUrl + '/GetChargesTypeExternalAccountsByProducts?myChargesTypeId=' + myChargesTypeId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    Put(entityPM: ChargesTypeByProductsControllerHelper) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: ChargesTypeByProductsControllerHelper = this.MapJsonToChargesTypeByProductsControllerHelper(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: ChargesTypeByProductsControllerHelper = this.MapJsonToChargesTypeByProductsControllerHelper(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    private MapJsonToChargesTypeByProductsControllerHelper(jsonPM: any, getCallMap: boolean = true, entityPM: ChargesTypeByProductsControllerHelper = null) {
        if (!entityPM) {
            entityPM = new ChargesTypeByProductsControllerHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else if (property === "Items") {
                var myPMService = new ChargesExternalAccountsByProductPMService();
                entityPM.Items = new Array<ChargesExternalAccountsByProductPM>();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];

                    var newItemPM: ChargesExternalAccountsByProductPM;
                    newItemPM = myPMService.MapJsonToEntityPM(jItem, getCallMap);
                    entityPM.Items.push(newItemPM);
                }
            }

            else {
                entityPM[property] = jsonPM[property];
            }
        }

        return entityPM;
    }

}

export class ChargesTypeByProductsControllerHelper {
    public Tenant: number;
    public ChargesTypeId: string = null;
    public Items: ChargesExternalAccountsByProductPM[] = [];

}
