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
import { CountryCurrencyPM } from 'Customs/EntityPMs/CountryCurrencyPM';



@Injectable()

export class CountryCurrencyService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CountryCurrency';

    }

    UpadateListCurrencyByCountry(countryCurrencyPMs: CountryCurrencyPM[],CountryId:string) {

       var mappedList: CountryCurrencyPM[] = [];
       var countryCurrencyPM= new CountryCurrencyPM();    
       countryCurrencyPM.CountryId=CountryId
       countryCurrencyPM.CurrencyTypeName="-1"
       mappedList.push(countryCurrencyPM);
       for (var k in countryCurrencyPMs) {
           var field = countryCurrencyPMs[k];
           var mappedEntity: any;
           mappedEntity = this.MapJsonToEntityPM(field, false);
           mappedList.push(mappedEntity.CountryCurrencyPM);
       }
        return this._http.post(this._apiUrl + '/UpadateListCurrencyByCountry/', JSON.stringify(mappedList), ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

   


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: CountryCurrencyPM = null) {


        if (!entityPM) {

            entityPM = new CountryCurrencyPM();
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
