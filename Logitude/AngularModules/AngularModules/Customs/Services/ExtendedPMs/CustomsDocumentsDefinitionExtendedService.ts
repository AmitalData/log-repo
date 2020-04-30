import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsDocumentsTicketPM} from '../../EntityPMs/CustomsDocumentsTicketPM';
import {CustomsDocumentsDefinitionPM} from '../../EntityPMs/CustomsDocumentsDefinitionPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsDocumentsDefinitionExtendedService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsDocumentsDefinitionExtended';
    }

    delete(Id: string) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsDocumentsTicketPM;
            return this._http.delete(this._apiUrl + '/Delete/?' + 'Id=' + Id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;
                if (pm) {
                    var mappedResult: CustomsDocumentsTicketPM;
                    serviceResponse.Result = mappedResult;
                }

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    GetCustomsDocumentsDefinitionsForDeclaration(declarationId: string) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsDocumentsDefinitionPM;
            return this._http.get(this._apiUrl + '/GetCustomsDocumentsDefinitionsForDeclaration/?' + 'declarationId=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                //var pmList = response;
                //if (pmList) {
                //    var mappedResult: Array<CustomsDocumentsDefinitionPM>;
                //    serviceResponse.Result = mappedResult;
                //}



                var allLists = response;
                var _mappedListsArray: Array<CustomsDocumentsDefinitionPM> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: CustomsDocumentsDefinitionPM;
                        entity = this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;



                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }
    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomsDocumentsDefinitionPM;
        entityPM = new CustomsDocumentsDefinitionPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }



}