import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AddressList} from '../../EntityLists/AddressList';

@Injectable()
export class AddressService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + "api/ngAddress";
    }
    
    GetMainAddressByCardId(cardId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAddressListByCardId?cardId=' + cardId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var itemJason = response;
                var itemMapped: AddressList;

                if (itemJason) {
                    itemMapped = this.MapAddressList(itemJason);
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = itemMapped;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
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
