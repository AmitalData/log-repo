//file not found! child composition SharedLogisticContact
//file not found! child composition CustomerProductActualData
//file not found! child composition CustomerCompetitor
//file not found! child composition CustomerSalesmanByProduct
//file not found! child composition CustomerAccountManagerByProduct
//file not found! child composition CustomerCustomsAgentByProduct
//file not found! child composition CustomerForwarderByProduct
//file not found! child composition CustomerMediatorByProduct
//file not found! child composition CardExternalCodeByCurrency

import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

import {CustomerPM} from '../../EntityPMs/CustomerPM';

import {CustomerProductPM} from '../../EntityPMs/CustomerProductPM';

import {CustomerProductLocationPM} from '../../EntityPMs/CustomerProductLocationPM';
import {AddressPM} from '../../EntityPMs/AddressPM';
import {ContactPM} from '../../EntityPMs/ContactPM';
import {SharedLogisticContactPM} from '../../EntityPMs/SharedLogisticContactPM';
import {CustomerProductActualDataPM} from '../../EntityPMs/CustomerProductActualDataPM';
import {CustomerCompetitorPM} from '../../EntityPMs/CustomerCompetitorPM';
import {CustomerAdditionalServicePM} from '../../EntityPMs/CustomerAdditionalServicePM';
import {CustomerSalesNotePM} from '../../EntityPMs/CustomerSalesNotePM';
import {CustomerSalesmanByProductPM} from '../../EntityPMs/CustomerSalesmanByProductPM';
import {CustomerAccountManagerByProductPM} from '../../EntityPMs/CustomerAccountManagerByProductPM';
import {CustomerCustomsAgentByProductPM} from '../../EntityPMs/CustomerCustomsAgentByProductPM';
import {CustomerForwarderByProductPM} from '../../EntityPMs/CustomerForwarderByProductPM';
import {CustomerMediatorByProductPM} from '../../EntityPMs/CustomerMediatorByProductPM';
import {CardExternalCodeByCurrencyPM} from '../../EntityPMs/CardExternalCodeByCurrencyPM';
import { ProductItemPM } from '../../EntityPMs/ProductItemPM';
import { HTSCodePM } from '../../EntityPMs/HTSCodePM';

@Injectable()

export class CustomerPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/customers';
    }

    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;



                        var entity: CustomerPM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Customer", "GetSinglePM", 'id=' + id);

                        return serviceResponse;

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: CustomerPM) {

        var callTime = new Date();
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Customer", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: CustomerPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {

                            var pm = response.body;
                            if (pm) {
                                var mappedResult: CustomerPM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }


                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Customer", "SaveChanges", "");


                            return serviceResponse;

                        }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
        });
    }

    update(entityPM: CustomerPM) {

        var callTime = new Date();
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Customer", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: CustomerPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                const getCircularReplacer = () => {
                    const seen = new WeakSet();
                    return (key, value) => {
                        if (typeof value === "object" && value !== null) {
                            if (seen.has(value)) {
                                return;
                            }
                            seen.add(value);
                        }
                        return value;
                    };
                };

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity, getCircularReplacer()), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {


                            var pm = response.body;
                            if (pm) {
                                var mappedResult: CustomerPM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Customer", "SaveChanges", "");

                            return serviceResponse;

                        }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
        });

    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: CustomerPM = null) {


        if (!entityPM) {

            entityPM = new CustomerPM();
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }

        this.MapCustomerProducts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapAddresses(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapContacts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapSharedLogisticContacts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerProductActualDatas(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerCompetitors(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerAdditionalServices(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapSalesNotes(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerSalesmanByProducts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerAccountManagerByProducts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerCustomsAgentByProducts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerForwarderByProducts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomerMediatorByProducts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCardExternalCodeByCurrencies(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapProductItems(entityPM, jsonPM, mapParent);

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.CustomerProducts = [];
            for (var item in entityPM.CustomerProducts) {
                var myCustomerProductPM = entityPM.CustomerProducts[item];
                var newCustomerProductPM: CustomerProductPM = this.clone(myCustomerProductPM);

                newCustomerProductPM.ProductLocations = [];
                for (var k in myCustomerProductPM.ProductLocations) {
                    var myCustomerProductLocationPM = myCustomerProductPM.ProductLocations[k];
                    var newCustomerProductLocationPM = this.clone(myCustomerProductPM.ProductLocations[k]);
                    newCustomerProductPM.ProductLocations.push(newCustomerProductLocationPM);

                }

                entityPM.OldEntityPM.CustomerProducts.push(newCustomerProductPM);
            }

            entityPM.OldEntityPM.Addresses = [];
            for (var item in entityPM.Addresses) {
                var myAddressPM = entityPM.Addresses[item];
                var newAddressPM: AddressPM = this.clone(myAddressPM);


                entityPM.OldEntityPM.Addresses.push(newAddressPM);
            }

            entityPM.OldEntityPM.Contacts = [];
            for (var item in entityPM.Contacts) {
                var myContactPM = entityPM.Contacts[item];
                var newContactPM: ContactPM = this.clone(myContactPM);


                entityPM.OldEntityPM.Contacts.push(newContactPM);
            }

            entityPM.OldEntityPM.SharedLogisticContacts = [];
            for (var item in entityPM.SharedLogisticContacts) {
                var mySharedLogisticContactPM = entityPM.SharedLogisticContacts[item];
                var newSharedLogisticContactPM: SharedLogisticContactPM = this.clone(mySharedLogisticContactPM);


                entityPM.OldEntityPM.SharedLogisticContacts.push(newSharedLogisticContactPM);
            }

            entityPM.OldEntityPM.CustomerProductActualDatas = [];
            for (var item in entityPM.CustomerProductActualDatas) {
                var myCustomerProductActualDataPM = entityPM.CustomerProductActualDatas[item];
                var newCustomerProductActualDataPM: CustomerProductActualDataPM = this.clone(myCustomerProductActualDataPM);


                entityPM.OldEntityPM.CustomerProductActualDatas.push(newCustomerProductActualDataPM);
            }

            entityPM.OldEntityPM.CustomerCompetitors = [];
            for (var item in entityPM.CustomerCompetitors) {
                var myCustomerCompetitorPM = entityPM.CustomerCompetitors[item];
                var newCustomerCompetitorPM: CustomerCompetitorPM = this.clone(myCustomerCompetitorPM);


                entityPM.OldEntityPM.CustomerCompetitors.push(newCustomerCompetitorPM);
            }

            entityPM.OldEntityPM.CustomerAdditionalServices = [];
            for (var item in entityPM.CustomerAdditionalServices) {
                var myCustomerAdditionalServicePM = entityPM.CustomerAdditionalServices[item];
                var newCustomerAdditionalServicePM: CustomerAdditionalServicePM = this.clone(myCustomerAdditionalServicePM);


                entityPM.OldEntityPM.CustomerAdditionalServices.push(newCustomerAdditionalServicePM);
            }

            entityPM.OldEntityPM.SalesNotes = [];
            for (var item in entityPM.SalesNotes) {
                var myCustomerSalesNotePM = entityPM.SalesNotes[item];
                var newCustomerSalesNotePM: CustomerSalesNotePM = this.clone(myCustomerSalesNotePM);


                entityPM.OldEntityPM.SalesNotes.push(newCustomerSalesNotePM);
            }

            entityPM.OldEntityPM.CustomerSalesmanByProducts = [];
            for (var item in entityPM.CustomerSalesmanByProducts) {
                var myCustomerSalesmanByProductPM = entityPM.CustomerSalesmanByProducts[item];
                var newCustomerSalesmanByProductPM: CustomerSalesmanByProductPM = this.clone(myCustomerSalesmanByProductPM);


                entityPM.OldEntityPM.CustomerSalesmanByProducts.push(newCustomerSalesmanByProductPM);
            }

            entityPM.OldEntityPM.CustomerAccountManagerByProducts = [];
            for (var item in entityPM.CustomerAccountManagerByProducts) {
                var myCustomerAccountManagerByProductPM = entityPM.CustomerAccountManagerByProducts[item];
                var newCustomerAccountManagerByProductPM: CustomerAccountManagerByProductPM = this.clone(myCustomerAccountManagerByProductPM);


                entityPM.OldEntityPM.CustomerAccountManagerByProducts.push(newCustomerAccountManagerByProductPM);
            }

            entityPM.OldEntityPM.CustomerCustomsAgentByProducts = [];
            for (var item in entityPM.CustomerCustomsAgentByProducts) {
                var myCustomerCustomsAgentByProductPM = entityPM.CustomerCustomsAgentByProducts[item];
                var newCustomerCustomsAgentByProductPM: CustomerCustomsAgentByProductPM = this.clone(myCustomerCustomsAgentByProductPM);


                entityPM.OldEntityPM.CustomerCustomsAgentByProducts.push(newCustomerCustomsAgentByProductPM);
            }

            entityPM.OldEntityPM.CustomerForwarderByProducts = [];
            for (var item in entityPM.CustomerForwarderByProducts) {
                var myCustomerForwarderByProductPM = entityPM.CustomerForwarderByProducts[item];
                var newCustomerForwarderByProductPM: CustomerForwarderByProductPM = this.clone(myCustomerForwarderByProductPM);
                entityPM.OldEntityPM.CustomerForwarderByProducts.push(newCustomerForwarderByProductPM);
            }

            entityPM.OldEntityPM.CustomerMediatorByProducts = [];
            for (var item in entityPM.CustomerMediatorByProducts) {
                var myCustomerMediatorByProductPM = entityPM.CustomerMediatorByProducts[item];
                var newCustomerMediatorByProductPM: CustomerMediatorByProductPM = this.clone(myCustomerMediatorByProductPM);
                entityPM.OldEntityPM.CustomerMediatorByProducts.push(newCustomerMediatorByProductPM);
            }

            entityPM.OldEntityPM.CardExternalCodeByCurrencies = [];
            for (var item in entityPM.CardExternalCodeByCurrencies) {
                var myCardExternalCodeByCurrencyPM = entityPM.CardExternalCodeByCurrencies[item];
                var newCardExternalCodeByCurrencyPM: CardExternalCodeByCurrencyPM = this.clone(myCardExternalCodeByCurrencyPM);
                entityPM.OldEntityPM.CardExternalCodeByCurrencies.push(newCardExternalCodeByCurrencyPM);
            }

            entityPM.OldEntityPM.CustomerProductItems = [];
            for (var item in entityPM.CustomerProductItems) {
                var myProductItemPM = entityPM.CustomerProductItems[item];
                var newProductItemPM: ProductItemPM = this.clone(myProductItemPM);

                newProductItemPM.HTSCodes = [];
                for (var k in myProductItemPM.HTSCodes) {
                    var newHTSCodePM = this.clone(myProductItemPM.HTSCodes[k]);
                    newProductItemPM.HTSCodes.push(newHTSCodePM);
                }

                entityPM.OldEntityPM.CustomerProductItems.push(newProductItemPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapCustomerProducts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerProducts: CustomerProductPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerProducts = entityPM.OldEntityPM.CustomerProducts;
        }

        entityPM.CustomerProducts = new Array<CustomerProductPM>();
        for (var item in jsonPM.CustomerProducts) {
            var jItem = jsonPM.CustomerProducts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerProductPM: CustomerProductPM;

            if (mapParent) {
                newCustomerProductPM = new CustomerProductPM(entityPM);
            }
            else {
                newCustomerProductPM = new CustomerProductPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerProductPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerProductPM.IsDirty = false;

            if (mapParent) {
                newCustomerProductPM.UniqueKey = Guid.newGuid();
                newCustomerProductPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerProductPM.OldEntityPM = this.clone(newCustomerProductPM);


                this.MapProductLocations(newCustomerProductPM, jItem, mapParent);
                newCustomerProductPM.OldEntityPM.ProductLocations = [];
                for (var k in newCustomerProductPM.ProductLocations) {
                    var clonedInside = this.clone(newCustomerProductPM.ProductLocations[k]);
                    newCustomerProductPM.OldEntityPM.ProductLocations.push(clonedInside); // clone old ProductLocations//
                }


            }
            else {
                if (newCustomerProductPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerProductPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerProductPM.ChangeSetOp = "Insert";
                }


                this.MapProductLocations(newCustomerProductPM, jItem, mapParent);

                newCustomerProductPM.OldEntityPM = null;
                newCustomerProductPM.EntityParentPM = null;
            }

            newCustomerProductPM.IsDirty = false;
            entityPM.CustomerProducts.push(newCustomerProductPM);
        }
        if (oldCustomerProducts) {

            for (var itemKey in oldCustomerProducts) {
                if (entityPM.CustomerProducts.filter(p => p.UniqueKey === oldCustomerProducts[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerProducts[itemKey]) {
                        //oldCustomerProducts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerProducts.push(oldCustomerProducts[itemKey]);
                        var oldItemJson = oldCustomerProducts[itemKey];
                        var deletedPM: CustomerProductPM = new CustomerProductPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";



                        this.MapProductLocations(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerProducts.push(deletedPM);
                    }
                }
            }
        }
    }
    MapProductLocations(entityPM: CustomerProductPM, jsonPM: any, mapParent: boolean = true) {

        var oldProductLocations: CustomerProductLocationPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldProductLocations = entityPM.OldEntityPM.ProductLocations;
        }

        entityPM.ProductLocations = new Array<CustomerProductLocationPM>();
        for (var item in jsonPM.ProductLocations) {
            var jItem = jsonPM.ProductLocations[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerProductLocationPM: CustomerProductLocationPM;

            if (mapParent) {
                newCustomerProductLocationPM = new CustomerProductLocationPM(entityPM);
            }
            else {
                newCustomerProductLocationPM = new CustomerProductLocationPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerProductLocationPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerProductLocationPM.IsDirty = false;

            if (mapParent) {
                newCustomerProductLocationPM.UniqueKey = Guid.newGuid();
                newCustomerProductLocationPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerProductLocationPM.OldEntityPM = this.clone(newCustomerProductLocationPM);
                //file not found! child composition CustomerProductLocation


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newCustomerProductLocationPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newCustomerProductLocationPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newCustomerProductLocationPM.ChangeSetOp = "Update";
                    }
                    else {
                        newCustomerProductLocationPM.ChangeSetOp = "Insert";
                    }
                }
                //file not found! child composition CustomerProductLocation

                newCustomerProductLocationPM.OldEntityPM = null;
                newCustomerProductLocationPM.EntityParentPM = null;
            }

            newCustomerProductLocationPM.IsDirty = false;
            entityPM.ProductLocations.push(newCustomerProductLocationPM);
        }
        if (oldProductLocations) {

            for (var itemKey in oldProductLocations) {
                if (entityPM.ProductLocations.filter(p => p.UniqueKey === oldProductLocations[itemKey].UniqueKey).length === 0) {

                    if (oldProductLocations[itemKey]) {
                        //oldProductLocations[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ProductLocations.push(oldProductLocations[itemKey]);
                        var oldItemJson = oldProductLocations[itemKey];
                        var deletedPM: CustomerProductLocationPM = new CustomerProductLocationPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerProductLocation
                        deletedPM.OldEntityPM = null;
                        entityPM.ProductLocations.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerProductLocation

    MapAddresses(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        entityPM.Addresses = new Array<AddressPM>();
        for (var item in jsonPM.Addresses) {

            var jItem = jsonPM.Addresses[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newAddressPM: AddressPM;
            newAddressPM = new AddressPM();

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newAddressPM[pmProperty] = jItem[pmProperty];
            }
            newAddressPM.IsDirty = false;
            entityPM.Addresses.push(newAddressPM);
        }
    }
    MapContacts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        entityPM.Contacts = new Array<ContactPM>();
        for (var item in jsonPM.Contacts) {

            var jItem = jsonPM.Contacts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newContactPM: ContactPM;
            newContactPM = new ContactPM();

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newContactPM[pmProperty] = jItem[pmProperty];
            }
            newContactPM.IsDirty = false;
            entityPM.Contacts.push(newContactPM);
        }
    }
    MapSharedLogisticContacts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        entityPM.SharedLogisticContacts = new Array<SharedLogisticContactPM>();
        for (var item in jsonPM.SharedLogisticContacts) {

            var jItem = jsonPM.SharedLogisticContacts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSharedLogisticContactPM: SharedLogisticContactPM;
            newSharedLogisticContactPM = new SharedLogisticContactPM();

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSharedLogisticContactPM[pmProperty] = jItem[pmProperty];
            }
            newSharedLogisticContactPM.IsDirty = false;
            entityPM.SharedLogisticContacts.push(newSharedLogisticContactPM);
        }
    }
    MapCustomerProductActualDatas(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerProductActualDatas: CustomerProductActualDataPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerProductActualDatas = entityPM.OldEntityPM.CustomerProductActualDatas;
        }

        entityPM.CustomerProductActualDatas = new Array<CustomerProductActualDataPM>();
        for (var item in jsonPM.CustomerProductActualDatas) {
            var jItem = jsonPM.CustomerProductActualDatas[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerProductActualDataPM: CustomerProductActualDataPM;

            if (mapParent) {
                newCustomerProductActualDataPM = new CustomerProductActualDataPM(entityPM);
            }
            else {
                newCustomerProductActualDataPM = new CustomerProductActualDataPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerProductActualDataPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerProductActualDataPM.IsDirty = false;

            if (mapParent) {
                newCustomerProductActualDataPM.UniqueKey = Guid.newGuid();
                newCustomerProductActualDataPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerProductActualDataPM.OldEntityPM = this.clone(newCustomerProductActualDataPM);
                //file not found! child composition CustomerProductActualData


            }
            else {
                if (newCustomerProductActualDataPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerProductActualDataPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerProductActualDataPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CustomerProductActualData

                newCustomerProductActualDataPM.OldEntityPM = null;
                newCustomerProductActualDataPM.EntityParentPM = null;
            }

            newCustomerProductActualDataPM.IsDirty = false;
            entityPM.CustomerProductActualDatas.push(newCustomerProductActualDataPM);
        }
        if (oldCustomerProductActualDatas) {

            for (var itemKey in oldCustomerProductActualDatas) {
                if (entityPM.CustomerProductActualDatas.filter(p => p.UniqueKey === oldCustomerProductActualDatas[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerProductActualDatas[itemKey]) {
                        //oldCustomerProductActualDatas[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerProductActualDatas.push(oldCustomerProductActualDatas[itemKey]);
                        var oldItemJson = oldCustomerProductActualDatas[itemKey];
                        var deletedPM: CustomerProductActualDataPM = new CustomerProductActualDataPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerProductActualData
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerProductActualDatas.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerProductActualData
    MapCustomerCompetitors(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerCompetitors: CustomerCompetitorPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerCompetitors = entityPM.OldEntityPM.CustomerCompetitors;
        }

        entityPM.CustomerCompetitors = new Array<CustomerCompetitorPM>();
        for (var item in jsonPM.CustomerCompetitors) {
            var jItem = jsonPM.CustomerCompetitors[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerCompetitorPM: CustomerCompetitorPM;

            if (mapParent) {
                newCustomerCompetitorPM = new CustomerCompetitorPM(entityPM);
            }
            else {
                newCustomerCompetitorPM = new CustomerCompetitorPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerCompetitorPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerCompetitorPM.IsDirty = false;

            if (mapParent) {
                newCustomerCompetitorPM.UniqueKey = Guid.newGuid();
                newCustomerCompetitorPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerCompetitorPM.OldEntityPM = this.clone(newCustomerCompetitorPM);
                //file not found! child composition CustomerCompetitor


            }
            else {
                if (newCustomerCompetitorPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerCompetitorPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerCompetitorPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CustomerCompetitor

                newCustomerCompetitorPM.OldEntityPM = null;
                newCustomerCompetitorPM.EntityParentPM = null;
            }

            newCustomerCompetitorPM.IsDirty = false;
            entityPM.CustomerCompetitors.push(newCustomerCompetitorPM);
        }
        if (oldCustomerCompetitors) {

            for (var itemKey in oldCustomerCompetitors) {
                if (entityPM.CustomerCompetitors.filter(p => p.UniqueKey === oldCustomerCompetitors[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerCompetitors[itemKey]) {
                        //oldCustomerCompetitors[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerCompetitors.push(oldCustomerCompetitors[itemKey]);
                        var oldItemJson = oldCustomerCompetitors[itemKey];
                        var deletedPM: CustomerCompetitorPM = new CustomerCompetitorPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerCompetitor
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerCompetitors.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerCompetitor
    MapCustomerAdditionalServices(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerAdditionalServices: CustomerAdditionalServicePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerAdditionalServices = entityPM.OldEntityPM.CustomerAdditionalServices;
        }

        entityPM.CustomerAdditionalServices = new Array<CustomerAdditionalServicePM>();
        for (var item in jsonPM.CustomerAdditionalServices) {
            var jItem = jsonPM.CustomerAdditionalServices[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerAdditionalServicePM: CustomerAdditionalServicePM;

            if (mapParent) {
                newCustomerAdditionalServicePM = new CustomerAdditionalServicePM(entityPM);
            }
            else {
                newCustomerAdditionalServicePM = new CustomerAdditionalServicePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerAdditionalServicePM[pmProperty] = jItem[pmProperty];
            }
            newCustomerAdditionalServicePM.IsDirty = false;

            if (mapParent) {
                newCustomerAdditionalServicePM.UniqueKey = Guid.newGuid();
                newCustomerAdditionalServicePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerAdditionalServicePM.OldEntityPM = this.clone(newCustomerAdditionalServicePM);


            }
            else {
                if (newCustomerAdditionalServicePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerAdditionalServicePM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerAdditionalServicePM.ChangeSetOp = "Insert";
                }

                newCustomerAdditionalServicePM.OldEntityPM = null;
                newCustomerAdditionalServicePM.EntityParentPM = null;
            }

            newCustomerAdditionalServicePM.IsDirty = false;
            entityPM.CustomerAdditionalServices.push(newCustomerAdditionalServicePM);
        }
        if (oldCustomerAdditionalServices) {

            for (var itemKey in oldCustomerAdditionalServices) {
                if (entityPM.CustomerAdditionalServices.filter(p => p.UniqueKey === oldCustomerAdditionalServices[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerAdditionalServices[itemKey]) {
                        //oldCustomerAdditionalServices[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerAdditionalServices.push(oldCustomerAdditionalServices[itemKey]);
                        var oldItemJson = oldCustomerAdditionalServices[itemKey];
                        var deletedPM: CustomerAdditionalServicePM = new CustomerAdditionalServicePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerAdditionalServices.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSalesNotes(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldSalesNotes: CustomerSalesNotePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSalesNotes = entityPM.OldEntityPM.SalesNotes;
        }

        entityPM.SalesNotes = new Array<CustomerSalesNotePM>();
        for (var item in jsonPM.SalesNotes) {
            var jItem = jsonPM.SalesNotes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerSalesNotePM: CustomerSalesNotePM;

            if (mapParent) {
                newCustomerSalesNotePM = new CustomerSalesNotePM(entityPM);
            }
            else {
                newCustomerSalesNotePM = new CustomerSalesNotePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerSalesNotePM[pmProperty] = jItem[pmProperty];
            }
            newCustomerSalesNotePM.IsDirty = false;

            if (mapParent) {
                newCustomerSalesNotePM.UniqueKey = Guid.newGuid();
                newCustomerSalesNotePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerSalesNotePM.OldEntityPM = this.clone(newCustomerSalesNotePM);


            }
            else {
                if (newCustomerSalesNotePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerSalesNotePM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerSalesNotePM.ChangeSetOp = "Insert";
                }

                newCustomerSalesNotePM.OldEntityPM = null;
                newCustomerSalesNotePM.EntityParentPM = null;
            }

            newCustomerSalesNotePM.IsDirty = false;

            entityPM.SalesNotes.push(newCustomerSalesNotePM);
        }
        if (oldSalesNotes) {

            for (var itemKey in oldSalesNotes) {
                if (entityPM.SalesNotes.filter(p => p.UniqueKey === oldSalesNotes[itemKey].UniqueKey).length === 0) {

                    if (oldSalesNotes[itemKey]) {
                        //oldSalesNotes[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SalesNotes.push(oldSalesNotes[itemKey]);
                        var oldItemJson = oldSalesNotes[itemKey];
                        var deletedPM: CustomerSalesNotePM = new CustomerSalesNotePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.SalesNotes.push(deletedPM);
                    }
                }
            }
        }
    }
    MapCustomerSalesmanByProducts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerSalesmanByProducts: CustomerSalesmanByProductPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerSalesmanByProducts = entityPM.OldEntityPM.CustomerSalesmanByProducts;
        }

        entityPM.CustomerSalesmanByProducts = new Array<CustomerSalesmanByProductPM>();
        for (var item in jsonPM.CustomerSalesmanByProducts) {
            var jItem = jsonPM.CustomerSalesmanByProducts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerSalesmanByProductPM: CustomerSalesmanByProductPM;

            if (mapParent) {
                newCustomerSalesmanByProductPM = new CustomerSalesmanByProductPM(entityPM);
            }
            else {
                newCustomerSalesmanByProductPM = new CustomerSalesmanByProductPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerSalesmanByProductPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerSalesmanByProductPM.IsDirty = false;

            if (mapParent) {
                newCustomerSalesmanByProductPM.UniqueKey = Guid.newGuid();
                newCustomerSalesmanByProductPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerSalesmanByProductPM.OldEntityPM = this.clone(newCustomerSalesmanByProductPM);
                //file not found! child composition CustomerSalesmanByProduct


            }
            else {
                if (newCustomerSalesmanByProductPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerSalesmanByProductPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerSalesmanByProductPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CustomerSalesmanByProduct

                newCustomerSalesmanByProductPM.OldEntityPM = null;
                newCustomerSalesmanByProductPM.EntityParentPM = null;
            }

            newCustomerSalesmanByProductPM.IsDirty = false;
            entityPM.CustomerSalesmanByProducts.push(newCustomerSalesmanByProductPM);
        }
        if (oldCustomerSalesmanByProducts) {

            for (var itemKey in oldCustomerSalesmanByProducts) {
                if (entityPM.CustomerSalesmanByProducts.filter(p => p.UniqueKey === oldCustomerSalesmanByProducts[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerSalesmanByProducts[itemKey]) {
                        //oldCustomerSalesmanByProducts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerSalesmanByProducts.push(oldCustomerSalesmanByProducts[itemKey]);
                        var oldItemJson = oldCustomerSalesmanByProducts[itemKey];
                        var deletedPM: CustomerSalesmanByProductPM = new CustomerSalesmanByProductPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerSalesmanByProduct
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerSalesmanByProducts.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerSalesmanByProduct
    MapCustomerAccountManagerByProducts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerAccountManagerByProducts: CustomerAccountManagerByProductPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerAccountManagerByProducts = entityPM.OldEntityPM.CustomerAccountManagerByProducts;
        }

        entityPM.CustomerAccountManagerByProducts = new Array<CustomerAccountManagerByProductPM>();
        for (var item in jsonPM.CustomerAccountManagerByProducts) {
            var jItem = jsonPM.CustomerAccountManagerByProducts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerAccountManagerByProductPM: CustomerAccountManagerByProductPM;

            if (mapParent) {
                newCustomerAccountManagerByProductPM = new CustomerAccountManagerByProductPM(entityPM);
            }
            else {
                newCustomerAccountManagerByProductPM = new CustomerAccountManagerByProductPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerAccountManagerByProductPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerAccountManagerByProductPM.IsDirty = false;

            if (mapParent) {
                newCustomerAccountManagerByProductPM.UniqueKey = Guid.newGuid();
                newCustomerAccountManagerByProductPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerAccountManagerByProductPM.OldEntityPM = this.clone(newCustomerAccountManagerByProductPM);
                //file not found! child composition CustomerAccountManagerByProduct


            }
            else {
                if (newCustomerAccountManagerByProductPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerAccountManagerByProductPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerAccountManagerByProductPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CustomerAccountManagerByProduct

                newCustomerAccountManagerByProductPM.OldEntityPM = null;
                newCustomerAccountManagerByProductPM.EntityParentPM = null;
            }

            newCustomerAccountManagerByProductPM.IsDirty = false;
            entityPM.CustomerAccountManagerByProducts.push(newCustomerAccountManagerByProductPM);
        }
        if (oldCustomerAccountManagerByProducts) {

            for (var itemKey in oldCustomerAccountManagerByProducts) {
                if (entityPM.CustomerAccountManagerByProducts.filter(p => p.UniqueKey === oldCustomerAccountManagerByProducts[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerAccountManagerByProducts[itemKey]) {
                        //oldCustomerAccountManagerByProducts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerAccountManagerByProducts.push(oldCustomerAccountManagerByProducts[itemKey]);
                        var oldItemJson = oldCustomerAccountManagerByProducts[itemKey];
                        var deletedPM: CustomerAccountManagerByProductPM = new CustomerAccountManagerByProductPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerAccountManagerByProduct
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerAccountManagerByProducts.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerAccountManagerByProduct
    MapCustomerCustomsAgentByProducts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerCustomsAgentByProducts: CustomerCustomsAgentByProductPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerCustomsAgentByProducts = entityPM.OldEntityPM.CustomerCustomsAgentByProducts;
        }

        entityPM.CustomerCustomsAgentByProducts = new Array<CustomerCustomsAgentByProductPM>();
        for (var item in jsonPM.CustomerCustomsAgentByProducts) {
            var jItem = jsonPM.CustomerCustomsAgentByProducts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerCustomsAgentByProductPM: CustomerCustomsAgentByProductPM;

            if (mapParent) {
                newCustomerCustomsAgentByProductPM = new CustomerCustomsAgentByProductPM(entityPM);
            }
            else {
                newCustomerCustomsAgentByProductPM = new CustomerCustomsAgentByProductPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerCustomsAgentByProductPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerCustomsAgentByProductPM.IsDirty = false;

            if (mapParent) {
                newCustomerCustomsAgentByProductPM.UniqueKey = Guid.newGuid();
                newCustomerCustomsAgentByProductPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerCustomsAgentByProductPM.OldEntityPM = this.clone(newCustomerCustomsAgentByProductPM);
                //file not found! child composition CustomerCustomsAgentByProduct


            }
            else {
                if (newCustomerCustomsAgentByProductPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerCustomsAgentByProductPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerCustomsAgentByProductPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CustomerCustomsAgentByProduct

                newCustomerCustomsAgentByProductPM.OldEntityPM = null;
                newCustomerCustomsAgentByProductPM.EntityParentPM = null;
            }

            newCustomerCustomsAgentByProductPM.IsDirty = false;
            entityPM.CustomerCustomsAgentByProducts.push(newCustomerCustomsAgentByProductPM);
        }
        if (oldCustomerCustomsAgentByProducts) {

            for (var itemKey in oldCustomerCustomsAgentByProducts) {
                if (entityPM.CustomerCustomsAgentByProducts.filter(p => p.UniqueKey === oldCustomerCustomsAgentByProducts[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerCustomsAgentByProducts[itemKey]) {
                        //oldCustomerCustomsAgentByProducts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerCustomsAgentByProducts.push(oldCustomerCustomsAgentByProducts[itemKey]);
                        var oldItemJson = oldCustomerCustomsAgentByProducts[itemKey];
                        var deletedPM: CustomerCustomsAgentByProductPM = new CustomerCustomsAgentByProductPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerCustomsAgentByProduct
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerCustomsAgentByProducts.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerCustomsAgentByProduct
    MapCustomerForwarderByProducts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerForwarderByProducts: CustomerForwarderByProductPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerForwarderByProducts = entityPM.OldEntityPM.CustomerForwarderByProducts;
        }

        entityPM.CustomerForwarderByProducts = new Array<CustomerForwarderByProductPM>();
        for (var item in jsonPM.CustomerForwarderByProducts) {
            var jItem = jsonPM.CustomerForwarderByProducts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerForwarderByProductPM: CustomerForwarderByProductPM;

            if (mapParent) {
                newCustomerForwarderByProductPM = new CustomerForwarderByProductPM(entityPM);
            }
            else {
                newCustomerForwarderByProductPM = new CustomerForwarderByProductPM(null); 
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerForwarderByProductPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerForwarderByProductPM.IsDirty = false;

            if (mapParent) {
                newCustomerForwarderByProductPM.UniqueKey = Guid.newGuid();
                newCustomerForwarderByProductPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerForwarderByProductPM.OldEntityPM = this.clone(newCustomerForwarderByProductPM);
                //file not found! child composition CustomerForwarderByProduct


            }
            else {
                if (newCustomerForwarderByProductPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerForwarderByProductPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerForwarderByProductPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CustomerForwarderByProduct

                newCustomerForwarderByProductPM.OldEntityPM = null;
                newCustomerForwarderByProductPM.EntityParentPM = null;
            }

            newCustomerForwarderByProductPM.IsDirty = false;
            entityPM.CustomerForwarderByProducts.push(newCustomerForwarderByProductPM);
        }
        if (oldCustomerForwarderByProducts) {

            for (var itemKey in oldCustomerForwarderByProducts) {
                if (entityPM.CustomerForwarderByProducts.filter(p => p.UniqueKey === oldCustomerForwarderByProducts[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerForwarderByProducts[itemKey]) {
                        //oldCustomerForwarderByProducts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerForwarderByProducts.push(oldCustomerForwarderByProducts[itemKey]);
                        var oldItemJson = oldCustomerForwarderByProducts[itemKey];
                        var deletedPM: CustomerForwarderByProductPM = new CustomerForwarderByProductPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerForwarderByProduct
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerForwarderByProducts.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerForwarderByProduct
    MapCustomerMediatorByProducts(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomerMediatorByProducts: CustomerMediatorByProductPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomerMediatorByProducts = entityPM.OldEntityPM.CustomerMediatorByProducts;
        }

        entityPM.CustomerMediatorByProducts = new Array<CustomerMediatorByProductPM>();
        for (var item in jsonPM.CustomerMediatorByProducts) {
            var jItem = jsonPM.CustomerMediatorByProducts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerMediatorByProductPM: CustomerMediatorByProductPM;

            if (mapParent) {
                newCustomerMediatorByProductPM = new CustomerMediatorByProductPM(entityPM);
            }
            else {
                newCustomerMediatorByProductPM = new CustomerMediatorByProductPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerMediatorByProductPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerMediatorByProductPM.IsDirty = false;

            if (mapParent) {
                newCustomerMediatorByProductPM.UniqueKey = Guid.newGuid();
                newCustomerMediatorByProductPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerMediatorByProductPM.OldEntityPM = this.clone(newCustomerMediatorByProductPM);
                //file not found! child composition CustomerMediatorByProduct


            }
            else {
                if (newCustomerMediatorByProductPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerMediatorByProductPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerMediatorByProductPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CustomerMediatorByProduct

                newCustomerMediatorByProductPM.OldEntityPM = null;
                newCustomerMediatorByProductPM.EntityParentPM = null;
            }

            newCustomerMediatorByProductPM.IsDirty = false;
            entityPM.CustomerMediatorByProducts.push(newCustomerMediatorByProductPM);
        }
        if (oldCustomerMediatorByProducts) {

            for (var itemKey in oldCustomerMediatorByProducts) {
                if (entityPM.CustomerMediatorByProducts.filter(p => p.UniqueKey === oldCustomerMediatorByProducts[itemKey].UniqueKey).length === 0) {

                    if (oldCustomerMediatorByProducts[itemKey]) {
                        //oldCustomerMediatorByProducts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomerMediatorByProducts.push(oldCustomerMediatorByProducts[itemKey]);
                        var oldItemJson = oldCustomerMediatorByProducts[itemKey];
                        var deletedPM: CustomerMediatorByProductPM = new CustomerMediatorByProductPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CustomerMediatorByProduct
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerMediatorByProducts.push(deletedPM);
                    }
                }
            }
        }
    }
    //file not found! for child composition CustomerMediatorByProduct
    MapCardExternalCodeByCurrencies(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCardExternalCodeByCurrencies: CardExternalCodeByCurrencyPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCardExternalCodeByCurrencies = entityPM.OldEntityPM.CardExternalCodeByCurrencies;
        }

        entityPM.CardExternalCodeByCurrencies = new Array<CardExternalCodeByCurrencyPM>();
        for (var item in jsonPM.CardExternalCodeByCurrencies) {
            var jItem = jsonPM.CardExternalCodeByCurrencies[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCardExternalCodeByCurrencyPM: CardExternalCodeByCurrencyPM;

            if (mapParent) {
                newCardExternalCodeByCurrencyPM = new CardExternalCodeByCurrencyPM(entityPM);
            }
            else {
                newCardExternalCodeByCurrencyPM = new CardExternalCodeByCurrencyPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCardExternalCodeByCurrencyPM[pmProperty] = jItem[pmProperty];
            }
            newCardExternalCodeByCurrencyPM.IsDirty = false;

            if (mapParent) {
                newCardExternalCodeByCurrencyPM.UniqueKey = Guid.newGuid();
                newCardExternalCodeByCurrencyPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCardExternalCodeByCurrencyPM.OldEntityPM = this.clone(newCardExternalCodeByCurrencyPM);
                //file not found! child composition CardExternalCodeByCurrency


            }
            else {
                if (newCardExternalCodeByCurrencyPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCardExternalCodeByCurrencyPM.ChangeSetOp = "Update";
                }
                else {
                    newCardExternalCodeByCurrencyPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition CardExternalCodeByCurrency

                newCardExternalCodeByCurrencyPM.OldEntityPM = null;
                newCardExternalCodeByCurrencyPM.EntityParentPM = null;
            }

            newCardExternalCodeByCurrencyPM.IsDirty = false;
            entityPM.CardExternalCodeByCurrencies.push(newCardExternalCodeByCurrencyPM);
        }
        if (oldCardExternalCodeByCurrencies) {

            for (var itemKey in oldCardExternalCodeByCurrencies) {
                if (entityPM.CardExternalCodeByCurrencies.filter(p => p.UniqueKey === oldCardExternalCodeByCurrencies[itemKey].UniqueKey).length === 0) {

                    if (oldCardExternalCodeByCurrencies[itemKey]) {
                        //oldCardExternalCodeByCurrencies[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CardExternalCodeByCurrencies.push(oldCardExternalCodeByCurrencies[itemKey]);
                        var oldItemJson = oldCardExternalCodeByCurrencies[itemKey];
                        var deletedPM: CardExternalCodeByCurrencyPM = new CardExternalCodeByCurrencyPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        //file not found! child composition CardExternalCodeByCurrency
                        deletedPM.OldEntityPM = null;
                        entityPM.CardExternalCodeByCurrencies.push(deletedPM);
                    }
                }
            }
        }
    }

    MapProductItems(entityPM: CustomerPM, jsonPM: any, mapParent: boolean = true) {

        var oldProductItems: ProductItemPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldProductItems = entityPM.OldEntityPM.CustomerProductItems;
        }

        entityPM.CustomerProductItems = new Array<ProductItemPM>();
        for (var item in jsonPM.CustomerProductItems) {
            var jItem = jsonPM.CustomerProductItems[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomerProductItemPM: ProductItemPM;

            if (mapParent) {
                newCustomerProductItemPM = new ProductItemPM(entityPM);
            }
            else {
                newCustomerProductItemPM = new ProductItemPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomerProductItemPM[pmProperty] = jItem[pmProperty];
            }
            newCustomerProductItemPM.IsDirty = false;

            if (mapParent) {
                newCustomerProductItemPM.UniqueKey = Guid.newGuid();
                newCustomerProductItemPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomerProductItemPM.OldEntityPM = this.clone(newCustomerProductItemPM);

                this.MapHTSCodes(newCustomerProductItemPM, jItem, mapParent);
                newCustomerProductItemPM.OldEntityPM.HTSCodes = [];
                for (var k in newCustomerProductItemPM.HTSCodes) {
                    var clonedInside = this.clone(newCustomerProductItemPM.HTSCodes[k]);
                    newCustomerProductItemPM.OldEntityPM.HTSCodes.push(clonedInside);
                }
            }

            else {
                if (newCustomerProductItemPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomerProductItemPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomerProductItemPM.ChangeSetOp = "Insert";
                }

                this.MapHTSCodes(newCustomerProductItemPM, jItem, mapParent);

                newCustomerProductItemPM.OldEntityPM = null;
                newCustomerProductItemPM.EntityParentPM = null;
            }

            newCustomerProductItemPM.IsDirty = false;
            entityPM.CustomerProductItems.push(newCustomerProductItemPM);
        }
        if (oldProductItems) {

            for (var itemKey in oldProductItems) {
                if (entityPM.CustomerProductItems.filter(p => p.UniqueKey === oldProductItems[itemKey].UniqueKey).length === 0) {

                    if (oldProductItems[itemKey]) {
                        var oldItemJson = oldProductItems[itemKey];
                        var deletedPM: ProductItemPM = new ProductItemPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }

                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        this.MapHTSCodes(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomerProductItems.push(deletedPM);
                    }
                }
            }
        }
    }
    MapHTSCodes(entityPM: ProductItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldHTSCodes: HTSCodePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldHTSCodes = entityPM.OldEntityPM.HTSCodes;
        }

        entityPM.HTSCodes = new Array<HTSCodePM>();
        for (var item in jsonPM.HTSCodes) {
            var jItem = jsonPM.HTSCodes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newHTSCodePM: HTSCodePM;

            if (mapParent) {
                newHTSCodePM = new HTSCodePM(entityPM);
            }
            else {
                newHTSCodePM = new HTSCodePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newHTSCodePM[pmProperty] = jItem[pmProperty];
            }
            newHTSCodePM.IsDirty = false;

            if (mapParent) {
                newHTSCodePM.UniqueKey = Guid.newGuid();
                newHTSCodePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newHTSCodePM.OldEntityPM = this.clone(newHTSCodePM);
            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newHTSCodePM.ChangeSetOp = "Delete";
                }
                else {
                    if (newHTSCodePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newHTSCodePM.ChangeSetOp = "Update";
                    }
                    else {
                        newHTSCodePM.ChangeSetOp = "Insert";
                    }
                }

                newHTSCodePM.OldEntityPM = null;
                newHTSCodePM.EntityParentPM = null;
            }

            newHTSCodePM.IsDirty = false;
            entityPM.HTSCodes.push(newHTSCodePM);
        }
        if (oldHTSCodes) {

            for (var itemKey in oldHTSCodes) {
                if (entityPM.HTSCodes.filter(p => p.UniqueKey === oldHTSCodes[itemKey].UniqueKey).length === 0) {

                    if (oldHTSCodes[itemKey]) {
                        var oldItemJson = oldHTSCodes[itemKey];
                        var deletedPM: HTSCodePM = new HTSCodePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.HTSCodes.push(deletedPM);
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

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

    public GetNewEntityPM() {
        var entityPM: CustomerPM;
        entityPM = new CustomerPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }
}
