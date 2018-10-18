
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {DocumentsFilingPM} from '../../EntityPMs/DocumentsFilingPM';
import {DocumentTypeCustomFieldPM} from '../../EntityPMs/DocumentTypeCustomFieldPM';
import {FormCustomFieldPM} from '../../EntityPMs/FormCustomFieldPM';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 







@Injectable()
export class DocumentTypeCustomFieldService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeCustomField';
    }



    getFormCustomFieldsByDocument(tenant: number, documentTypeId: string, entityId: string, entityTypeId: string){

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&entityTypeId=' + entityTypeId, { headers: authHeader }).map(response => {
            var result = response.json();
                var entity: FormCustomFieldPM;
                var FromDocumentTypeCustomFieldLists: FormCustomFieldPM[];
                FromDocumentTypeCustomFieldLists = new Array<FormCustomFieldPM>();
                result.forEach((item) => {
                    entity = this.MapFormCustomFieldsByDocumentJsonToEntityPM(item);
                    FromDocumentTypeCustomFieldLists.push(entity);
                });


                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = FromDocumentTypeCustomFieldLists;
                return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }

    getDocumentTypeCustomFieldsByDocument(tenant: number, documentTypeId: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&documentTypeId=' + documentTypeId, { headers: authHeader }).map(response => {
            var result = response.json();
                var entity: DocumentTypeCustomFieldPM;
                var  DocumentTypeCustomFieldLists: DocumentTypeCustomFieldPM[];
                DocumentTypeCustomFieldLists = new Array<DocumentTypeCustomFieldPM>();


                result.forEach((item) => {
                    entity = this.MapJsonToEntityPM(item);
                    DocumentTypeCustomFieldLists.push(entity);
                });

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = DocumentTypeCustomFieldLists;
                return pmresponse;

        }).catch(ServiceHelper.HandleServiceError);
    }



    update(entityPM: DocumentTypeCustomFieldPM) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var errorsArray = [];

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DocumentTypeCustomFieldPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                return this._http.put(this._apiUrl + '/putdocumenttypecustomfield', JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: DocumentTypeCustomFieldPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return Observable.of(serviceResponse);

            }
        }

        );

    }


    Insert(entityPM: DocumentTypeCustomFieldPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');


            var errorsArray = [];


            var response: ServiceResponse;
            response = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DocumentTypeCustomFieldPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                return this._http.post(this._apiUrl + '/postdocumenttypecustomfield', JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: DocumentTypeCustomFieldPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            response.Result = mappedResult;
                        }

                        return response;

                    });
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return Observable.of(response);

            }
        }

        );
    }


    UpdateFormCustomField(entityPM: FormCustomFieldPM) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var errorsArray = [];

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: FormCustomFieldPM;
                mappedEntity = this.MapFormCustomFieldsByDocumentJsonToEntityPM(entityPM, false);
                return this._http.put(this._apiUrl + '/putFormCustomField', JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: FormCustomFieldPM;
                            mappedResult = this.MapFormCustomFieldsByDocumentJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return Observable.of(serviceResponse);

            }
        }

        );

    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: DocumentTypeCustomFieldPM = null) {


        if (!entityPM) {

            entityPM = new DocumentTypeCustomFieldPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
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


    
    MapFormCustomFieldsByDocumentJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: FormCustomFieldPM = null) {


        if (!entityPM) {

            entityPM = new FormCustomFieldPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }




        return entityPM;
    }




}

