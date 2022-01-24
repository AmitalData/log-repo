import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {AddAddressContactForClientRequestParams} from '../../DataContract/RequestParams/AddAddressContactForClientRequestParams';
import {ClientSearchRequestParams} from '../../DataContract/RequestParams/ClientSearchRequestParams';
import {CreateClientRequestParams} from '../../DataContract/RequestParams/CreateClientRequestParams';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import { ClientAddressPM } from 'Customs/EntityPMs/ClientAddressPM';
import { ClientPM } from 'Customs/EntityPMs/ClientPM';
import { ClientsAddressCommTypePM } from 'Customs/EntityPMs/ClientsAddressCommTypePM';

@Injectable()

export class ClientMessagesService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Client';

    }

    PostUpdateDeleteClientAddressContactRequest(entity: AddAddressContactForClientRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var hahahah = JSON.stringify(entity);
            return this._http.post(
                this._apiUrl + '/PostUpdateDeleteClientAddressContactRequest/',
                hahahah,
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    PostClientRequest(entity: ClientSearchRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var hahahahah = JSON.stringify(entity);
            //hahahahah = ({ "PBId": "69fbed82-8f73-45bf-ba9f-e09ee4347800", "IsAngularClient": true, "LoggingEnabled": true, "LoggingUserId": "1-2", "Tenant": 1, "LoggingEntityId": "fffff", "LoggingEntityReference": "321" }) + "";
            
            return this._http.post(
                this._apiUrl + '/PostClientRequest/',
                hahahahah,
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );
    }

    PostClientSearchByIDRequest(entity: ClientSearchRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostClientSearchByIDRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;
                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    CreateClientRequest(entity: CreateClientRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/CreateClientRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetSingleClientPMByCode(code: string, isIncludeAll: boolean = false) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        code = encodeURIComponent(code);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleClientPMByCode?' + 'code=' + code + "&isIncludeAll=" + isIncludeAll,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;


                var entity: ClientPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });
    }

    GetSingleClientPMByPassportNumberOrCountry(passportNumber: string, passportCountryCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        //code = encodeURIComponent(code);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleClientPMByPassportNumberOrCountry?' + 'passportNumber=' + passportNumber + "&passportCountryCode=" + passportCountryCode,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var pm = response;
                    var entity: ClientPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));

        });
    }

    PutRecallClientsForCutomsRequest(fileUploadParamerter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + '/PutRecallClientsForCutomsRequest', JSON.stringify(fileUploadParamerter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }
        );

    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ClientPM = null) {


        if (!entityPM) {
            entityPM = new ClientPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        this.MapClientAddresses(entityPM, jsonPM, mapParent); // Call composition tables map methods

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.ClientAddresses = [];
            for (var item in entityPM.ClientAddresses) {
                var myClientAddressPM = entityPM.ClientAddresses[item];
                var newClientAddressPM: ClientAddressPM = this.clone(myClientAddressPM);

                newClientAddressPM.ClientsAddressCommTypes = [];
                for (var k in myClientAddressPM.ClientsAddressCommTypes) {
                    var myClientsAddressCommTypePM = myClientAddressPM.ClientsAddressCommTypes[k];
                    var newClientsAddressCommTypePM = this.clone(myClientAddressPM.ClientsAddressCommTypes[k]);
                    newClientAddressPM.ClientsAddressCommTypes.push(newClientsAddressCommTypePM);

                }

                entityPM.OldEntityPM.ClientAddresses.push(newClientAddressPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapClientAddresses(entityPM: ClientPM, jsonPM: any, mapParent: boolean = true) {

        var oldClientAddresses: ClientAddressPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClientAddresses = entityPM.OldEntityPM.ClientAddresses;
        }

        entityPM.ClientAddresses = new Array<ClientAddressPM>();
        for (var item in jsonPM.ClientAddresses) {
            var jItem = jsonPM.ClientAddresses[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClientAddressPM: ClientAddressPM;

            if (mapParent) {
                newClientAddressPM = new ClientAddressPM(entityPM);
            }
            else {
                newClientAddressPM = new ClientAddressPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClientAddressPM[pmProperty] = jItem[pmProperty];
            }
            newClientAddressPM.IsDirty = false;

            if (mapParent) {
                newClientAddressPM.UniqueKey = Guid.newGuid();
                newClientAddressPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClientAddressPM.OldEntityPM = this.clone(newClientAddressPM);


                this.MapClientsAddressCommTypes(newClientAddressPM, jItem, mapParent);
                newClientAddressPM.OldEntityPM.ClientsAddressCommTypes = [];
                for (var k in newClientAddressPM.ClientsAddressCommTypes) {
                    var clonedInside = this.clone(newClientAddressPM.ClientsAddressCommTypes[k]);
                    newClientAddressPM.OldEntityPM.ClientsAddressCommTypes.push(clonedInside); // clone old ClientsAddressCommTypes//
                }


            }
            else {
                if (newClientAddressPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newClientAddressPM.ChangeSetOp = "Update";
                }
                else {
                    newClientAddressPM.ChangeSetOp = "Insert";
                }


                this.MapClientsAddressCommTypes(newClientAddressPM, jItem, mapParent);

                newClientAddressPM.OldEntityPM = null;
                newClientAddressPM.EntityParentPM = null;
            }


            entityPM.ClientAddresses.push(newClientAddressPM);
        }
        if (oldClientAddresses) {

            for (var itemKey in oldClientAddresses) {
                if (entityPM.ClientAddresses.filter(p => p.UniqueKey === oldClientAddresses[itemKey].UniqueKey).length === 0) {

                    if (oldClientAddresses[itemKey]) {
                        //oldClientAddresses[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClientAddresses.push(oldClientAddresses[itemKey]);
                        var oldItemJson = oldClientAddresses[itemKey];
                        var deletedPM: ClientAddressPM = new ClientAddressPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";



                        this.MapClientsAddressCommTypes(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.ClientAddresses.push(deletedPM);
                    }
                }
            }
        }
    }
    MapClientsAddressCommTypes(entityPM: ClientAddressPM, jsonPM: any, mapParent: boolean = true) {

        var oldClientsAddressCommTypes: ClientsAddressCommTypePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClientsAddressCommTypes = entityPM.OldEntityPM.ClientsAddressCommTypes;
        }

        entityPM.ClientsAddressCommTypes = new Array<ClientsAddressCommTypePM>();
        for (var item in jsonPM.ClientsAddressCommTypes) {
            var jItem = jsonPM.ClientsAddressCommTypes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClientsAddressCommTypePM: ClientsAddressCommTypePM;

            if (mapParent) {
                newClientsAddressCommTypePM = new ClientsAddressCommTypePM(entityPM);
            }
            else {
                newClientsAddressCommTypePM = new ClientsAddressCommTypePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClientsAddressCommTypePM[pmProperty] = jItem[pmProperty];
            }
            newClientsAddressCommTypePM.IsDirty = false;

            if (mapParent) {
                newClientsAddressCommTypePM.UniqueKey = Guid.newGuid();
                newClientsAddressCommTypePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClientsAddressCommTypePM.OldEntityPM = this.clone(newClientsAddressCommTypePM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClientsAddressCommTypePM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClientsAddressCommTypePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newClientsAddressCommTypePM.ChangeSetOp = "Update";
                    }
                    else {
                        newClientsAddressCommTypePM.ChangeSetOp = "Insert";
                    }
                }

                newClientsAddressCommTypePM.OldEntityPM = null;
                newClientsAddressCommTypePM.EntityParentPM = null;
            }


            entityPM.ClientsAddressCommTypes.push(newClientsAddressCommTypePM);
        }
        if (oldClientsAddressCommTypes) {

            for (var itemKey in oldClientsAddressCommTypes) {
                if (entityPM.ClientsAddressCommTypes.filter(p => p.UniqueKey === oldClientsAddressCommTypes[itemKey].UniqueKey).length === 0) {

                    if (oldClientsAddressCommTypes[itemKey]) {
                        //oldClientsAddressCommTypes[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClientsAddressCommTypes.push(oldClientsAddressCommTypes[itemKey]);
                        var oldItemJson = oldClientsAddressCommTypes[itemKey];
                        var deletedPM: ClientsAddressCommTypePM = new ClientsAddressCommTypePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.ClientsAddressCommTypes.push(deletedPM);
                    }
                }
            }
        }
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
