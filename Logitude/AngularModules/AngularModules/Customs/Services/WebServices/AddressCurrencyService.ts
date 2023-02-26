import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {VendorInsertUpdateDeleteMessageRequestParams} from '../../DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import {VendorAddCommunicationDeviceRequestParams} from '../../DataContract/RequestParams/VendorAddCommunicationDeviceRequestParams';
import {VendorSearchByCustomsAgentRequestParams} from '../../DataContract/RequestParams/VendorSearchByCustomsAgentRequestParams';
import { List } from 'Infrastructure/DataContracts/Dashboard/List';
import { AddressCurrencyPM } from 'Customs/EntityPMs/AddressCurrencyPM';



@Injectable()

export class AddressCurrencyService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AddressCurrency';

    }

    UpadateListCurrencyByAddress(addressCurrencyPMs: AddressCurrencyPM[]) {

       var mappedList: AddressCurrencyPM[] = [];
       for (var k in addressCurrencyPMs) {
           var field = addressCurrencyPMs[k];
           var mappedEntity: any;
           mappedEntity = this.MapJsonToEntityPM(field, false);
           mappedList.push(mappedEntity.AddressCurrencyPM);
       }
        return this._http.post(this._apiUrl + '/UpadateListCurrencyByAddress/', JSON.stringify(mappedList), ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

   


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AddressCurrencyPM = null) {


        if (!entityPM) {

            entityPM = new AddressCurrencyPM();
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

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }


    public clone(jsonPM: any) {
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
