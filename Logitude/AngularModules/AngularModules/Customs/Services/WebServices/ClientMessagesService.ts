import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {DeclarationList} from '../../EntityLists/DeclarationList';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {AddAddressContactForClientRequestParams} from '../../DataContract/RequestParams/AddAddressContactForClientRequestParams';
import {ClientSearchRequestParams} from '../../DataContract/RequestParams/ClientSearchRequestParams';
import {CreateClientRequestParams} from '../../DataContract/RequestParams/CreateClientRequestParams';
import {ClientPM} from '../../EntityPMs/ClientPM';
import {ClientAddressPM} from '../../EntityPMs/ClientAddressPM';
import {ClientsAddressCommTypePM} from '../../EntityPMs/ClientsAddressCommTypePM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Injectable()

export class ClientMessagesService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Client';

    }

    PostUpdateDeleteClientAddressContactRequest(entity: AddAddressContactForClientRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var hahahah = JSON.stringify(entity);
            return this._http.post(
                this._apiUrl + '/PostUpdateDeleteClientAddressContactRequest/',
                hahahah,
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    PostClientRequest(entity: ClientSearchRequestParams) {

        return Observable.defer(() => {

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
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    PostClientSearchByIDRequest(entity: ClientSearchRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostClientSearchByIDRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    CreateClientRequest(entity: CreateClientRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/CreateClientRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    GetSingleClientPMByCode(code: string, isIncludeAll: boolean = false) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        code = encodeURIComponent(code);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleClientPMByCode?' + 'code=' + code + "&isIncludeAll=" + isIncludeAll,
                { headers: authHeader }).map(response => {

                var pm = response.json();


                var entity: ClientPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        });
    }

    GetSingleClientPMByPassportNumberOrCountry(passportNumber: string, passportCountryCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        //code = encodeURIComponent(code);
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleClientPMByPassportNumberOrCountry?' + 'passportNumber=' + passportNumber + "&passportCountryCode=" + passportCountryCode,
                { headers: authHeader }).map(response => {
                    var pm = response.json();
                    var entity: ClientPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        });
    }

    PutRecallClientsForCutomsRequest(fileUploadParamerter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutRecallClientsForCutomsRequest', JSON.stringify(fileUploadParamerter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }).catch(ServiceHelper.HandleServiceError);
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
