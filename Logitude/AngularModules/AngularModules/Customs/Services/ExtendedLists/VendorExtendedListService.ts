
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ImporterDespositionClass} from '../../DataContract/ImporterDespositionClass';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()


export class VendorExtendedListService {

    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/VendorExtended';
    }


    GetVendorsWithImporterDespositions(vendorId: string, importerId: string, ShowOnlyValid: boolean, useImporterFilter:boolean, searchText:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetVendorsWithImporterDespositions';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetVendorsWithImporterDespositions/?' + 'vendorId=' + vendorId + '&importerId=' + importerId + '&ShowOnlyValid=' + ShowOnlyValid + '&useImporterFilter=' + useImporterFilter+ '&searchText=' + searchText, { headers: authHeader }).map(response => {


               
                var serviceResponse: ServiceResponse = response.json();
                var _mappedListsArray: Array<ImporterDespositionClass> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: ImporterDespositionClass;
                        entity = this.MapJsonToImporterDesposition(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToImporterDesposition(json: any, mapParent: boolean = true, entity: ImporterDespositionClass = null) {


        if (!entity) {

            entity = new ImporterDespositionClass();
        }

        var jsonPMKeys = Object.keys(json);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }


        //  entity.IsDirty = false;



        return entity;
    }
}