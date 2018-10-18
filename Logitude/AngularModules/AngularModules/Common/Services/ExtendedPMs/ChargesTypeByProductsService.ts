import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ChargesExternalAccountsByProductPM}  from '../../EntityPMs/ChargesExternalAccountsByProductPM';
import {ChargesExternalAccountsByProductPMService} from '../StandardPMs/ChargesExternalAccountsByProductPMService';

@Injectable()

export class ChargesTypeByProductsService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ChargesTypeByProducts';
    }

    GetChargesTypeExternalAccountsByProducts(myChargesTypeId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetChargesTypeExternalAccountsByProducts?myChargesTypeId=' + myChargesTypeId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    Put(entityPM: ChargesTypeByProductsControllerHelper) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: ChargesTypeByProductsControllerHelper = this.MapJsonToChargesTypeByProductsControllerHelper(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: ChargesTypeByProductsControllerHelper = this.MapJsonToChargesTypeByProductsControllerHelper(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
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