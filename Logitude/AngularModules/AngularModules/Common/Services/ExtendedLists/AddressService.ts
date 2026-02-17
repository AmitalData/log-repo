import {Injectable} from '@angular/core';
import {Http, Headers, ConnectionBackend, BaseRequestOptions} from '@angular/http';
import 'rxjs/add/operator/map';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AddressList} from '../../EntityLists/AddressList';

@Injectable()
export class AddressService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + "api/ngAddress";
    }
    
    GetMainAddressByCardId(cardId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAddressListByCardId?cardId=' + cardId + '&tenant=' + tenant, {headers: authHeader}).map(response => {

                var itemJason = response.json();
                var itemMapped: AddressList;

                if (itemJason) {
                    itemMapped = this.MapAddressList(itemJason);
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = itemMapped;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapAddressList(jsonList: any) {
        var entityPM: AddressList = null;

        if (jsonList) {
            entityPM = new AddressList();

            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }

        return entityPM;
    }
}
