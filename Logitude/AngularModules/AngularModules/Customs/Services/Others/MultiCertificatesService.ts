import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CertificateConnectedItem} from '../../DataContract/CertificateConnectedItem';
import {CertificateTicket} from '../../DataContract/CertificateTicket';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class MultiCertificatesService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/MultiCertificates';
    }

    GetCertificateConnectedItems(declarationId: string, attachmentTypeCode: string, reqConfirmationTypeCode: string, CertificateExemptionTypeCode: string, CertificateNumber: string, ResConfirmationTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCertificateConnectedItems?' + 'declarationId=' + declarationId + '&attachmentTypeCode=' + attachmentTypeCode + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode + '&CertificateExemptionTypeCode=' + CertificateExemptionTypeCode + '&CertificateNumber=' + CertificateNumber + '&ResConfirmationTypeCode=' + ResConfirmationTypeCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<CertificateConnectedItem> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: CertificateConnectedItem;
                        entity = this.MapJsonToConnectedItem(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }
    

    PutCertificateTickets(certificateTicket: CertificateTicket )
        {



        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

         


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
          
            var mappedEntity: CertificateTicket;
            mappedEntity = this.MapJsonToCertificateTicket(certificateTicket, false);
            var jsonstr = JSON.stringify(mappedEntity);
            console.log(jsonstr);
            return this._http.post(this._apiUrl + '/PostCertificateTicket/', jsonstr,
                    ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                        var pm = res;
                        if (pm) {
                            var mappedResult: CertificateTicket;
                            mappedResult = this.MapJsonToCertificateTicket(pm, true, certificateTicket);
                            serviceResponse.Result = mappedResult;
                        }


                        return serviceResponse;

                    }),catchError(ServiceHelper.HandleServiceError));
       
        }

        );


    }

    UpdateCertificatesBySearchFields(
        declarationId: string,
        invoiceCounterKey: number,
        externalRequestTypeCode: string,
        approvalRequestNumber: string,
        reqConfirmationTypeCode: string,
        attachmentTypeCode: string,
        certificateNumber: string,
        certificateExemptionTypeCode: string,
        resConfirmationTypeCode: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetUpdateCertificatesBySearchFields?'
                + 'declarationId=' + declarationId
                + '&invoiceCounterKey=' + invoiceCounterKey
                + '&externalRequestTypeCode=' + externalRequestTypeCode
                + '&approvalRequestNumber=' + approvalRequestNumber
                + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode
                + '&attachmentTypeCode=' + attachmentTypeCode
                + '&certificateNumber=' + certificateNumber
                + '&certificateExemptionTypeCode=' + certificateExemptionTypeCode
                + '&resConfirmationTypeCode=' + resConfirmationTypeCode,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var result = response;
                
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    MapJsonToConnectedItem(json: any, mapParent: boolean = true, entity: CertificateConnectedItem = null) {


        if (!entity) {

            entity = new CertificateConnectedItem();
        }

        var jsonPMKeys = Object.keys(json);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }


        return entity;
    }

    MapJsonToCertificateTicket(json: any, mapParent: boolean = true, entity: CertificateTicket = null) {


        if (!entity) {

            entity = new CertificateTicket();
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

    getByFilters(filters: ApiQueryFilters, declarationId: string, attachmentTypeCode: string, reqConfirmationTypeCode: string, CertificateExemptionTypeCode: string, CertificateNumber: string, ResConfirmationTypeCode: string) {

        var urlparameters = '/GetByFilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//


        return defer(() => {
            callUrl = callUrl + '&declarationId=' + declarationId + '&attachmentTypeCode=' + attachmentTypeCode + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode + '&CertificateExemptionTypeCode=' + CertificateExemptionTypeCode + '&CertificateNumber=' + CertificateNumber + '&ResConfirmationTypeCode=' + ResConfirmationTypeCode;
            return this._http.get(callUrl , ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response;
                var _mappedListsArray: Array<CertificateConnectedItem> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CertificateConnectedItem;
                        entity = this.MapJsonToConnectedItem(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    getCountByFilters(filters: ApiQueryFilters, declarationId: string, attachmentTypeCode: string, reqConfirmationTypeCode: string, CertificateExemptionTypeCode: string, CertificateNumber: string, ResConfirmationTypeCode: string) {

        var urlparameters = '/getCountByFilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//


        return defer(() => {
            callUrl = callUrl + '&declarationId=' + declarationId + '&attachmentTypeCode=' + attachmentTypeCode + '&reqConfirmationTypeCode=' + reqConfirmationTypeCode + '&CertificateExemptionTypeCode=' + CertificateExemptionTypeCode + '&CertificateNumber=' + CertificateNumber + '&ResConfirmationTypeCode=' + ResConfirmationTypeCode;
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }



    getPromiseByFilters(filters: ApiQueryFilters, declarationId: string, attachmentTypeCode: string, reqConfirmationTypeCode: string, CertificateExemptionTypeCode: string, CertificateNumber: string, ResConfirmationTypeCode: string) {
       
        return new Promise((resolve, reject) => {
            
            resolve(this.getByFilters(filters, declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode));
            
        });
    }


    PutSupplierInvoiceItemCatalogNumber(certificateConnectedItem: CertificateConnectedItem) {



        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');




            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CertificateConnectedItem;
            mappedEntity = this.MapJsonToConnectedItem(certificateConnectedItem, false);

            return this._http.put(this._apiUrl + '/PutSupplierInvoiceItemCatalogNumber/', JSON.stringify(mappedEntity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var pm = res;
                    if (pm) {
                        var mappedResult: CertificateConnectedItem;
                        mappedResult = this.MapJsonToConnectedItem(pm, true, certificateConnectedItem);
                        serviceResponse.Result = mappedResult;
                    }


                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );


    }

    DeclarationHasInvoices(declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

    

        return defer(() => {
            var callURL = this._apiUrl + '/GetDeclarationHasInvoices?' + 'declarationId=' + declarationId;
         
            return this._http.get(callURL , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }









}