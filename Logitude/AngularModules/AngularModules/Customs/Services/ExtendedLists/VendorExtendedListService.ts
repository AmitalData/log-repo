
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ImporterDespositionClass} from '../../DataContract/ImporterDespositionClass';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()


export class VendorExtendedListService {

    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/VendorExtended';
    }


    GetVendorsWithImporterDespositions(vendorId: string, importerId: string, ShowOnlyValid: boolean, useImporterFilter:boolean, searchText:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetVendorsWithImporterDespositions';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetVendorsWithImporterDespositions/?' + 'vendorId=' + vendorId + '&importerId=' + importerId + '&ShowOnlyValid=' + ShowOnlyValid + '&useImporterFilter=' + useImporterFilter+ '&searchText=' + searchText, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {


               
                var serviceResponse: ServiceResponse = response;
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
            }),catchError(ServiceHelper.HandleServiceError));
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
