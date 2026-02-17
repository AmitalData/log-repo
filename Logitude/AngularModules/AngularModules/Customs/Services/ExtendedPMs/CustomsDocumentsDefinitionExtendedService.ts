import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsDocumentsTicketPM} from '../../EntityPMs/CustomsDocumentsTicketPM';
import {CustomsDocumentsDefinitionPM} from '../../EntityPMs/CustomsDocumentsDefinitionPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsDocumentsDefinitionExtendedService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsDocumentsDefinitionExtended';
    }

    delete(Id: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsDocumentsTicketPM;
            return this._http.delete(this._apiUrl + '/Delete/?' + 'Id=' + Id, { headers: authHeader }).map(response => {

                var pm = response.json();
                if (pm) {
                    var mappedResult: CustomsDocumentsTicketPM;
                    serviceResponse.Result = mappedResult;
                }

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    GetCustomsDocumentsDefinitionsForDeclaration(declarationId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsDocumentsDefinitionPM;
            return this._http.get(this._apiUrl + '/GetCustomsDocumentsDefinitionsForDeclaration/?' + 'declarationId=' + declarationId, { headers: authHeader }).map(response => {

                //var pmList = response.json();
                //if (pmList) {
                //    var mappedResult: Array<CustomsDocumentsDefinitionPM>;
                //    serviceResponse.Result = mappedResult;
                //}



                var allLists = response.json();
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

            }).catch(ServiceHelper.HandleServiceError);
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