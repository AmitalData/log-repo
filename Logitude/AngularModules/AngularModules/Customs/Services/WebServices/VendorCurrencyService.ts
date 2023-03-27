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
import { VendorCurrencyPM } from 'Customs/EntityPMs/VendorCurrencyPM';
import { List } from 'Infrastructure/DataContracts/Dashboard/List';



@Injectable()

export class VendorCurrencyService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/VendorCurrency';

    }

    UpadateListCurrencyByVendor(vendorCurrencyPMs: VendorCurrencyPM[],VendorId: string) {

       var mappedList: VendorCurrencyPM[] = [];
       var vendorCurrencyPM= new VendorCurrencyPM();    
       vendorCurrencyPM.VendorId=VendorId
       vendorCurrencyPM.CurrencyTypeName="-1"
       mappedList.push(vendorCurrencyPM);
       
       for (var k in vendorCurrencyPMs) {
           var field = vendorCurrencyPMs[k];
           var mappedEntity: any;
           mappedEntity = this.MapJsonToEntityPM(field, false);
           mappedList.push(mappedEntity.VendorCurrencyPM);
       }
       
        return this._http.post(this._apiUrl + '/UpadateListCurrencyByVendor/', JSON.stringify(mappedList), ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    GenListVendorCurrencyByVendorId(vendorId:string){

       
            return this._http.get(this._apiUrl + '/GetVendorCurrencyByVendorId?VendorId='+vendorId, ServiceHelper.GetHttpHeaders()).pipe(
                map(res => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = res;
                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
       
      
      

      
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: VendorCurrencyPM = null) {


        if (!entityPM) {

            entityPM = new VendorCurrencyPM();
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
