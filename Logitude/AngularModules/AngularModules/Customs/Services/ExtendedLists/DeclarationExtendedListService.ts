import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
 import { SendCollateralRequestParams } from '../../DataContract/RequestParams/SendCollateralRequestParams';
 
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
 
@Injectable()

export class DeclarationExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Declarartion';
    }


    GetSingleDeclarationByCustomFileNo(customFileNo: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleDeclarationByCustomFileNo/?' + 'customFileNo=' + customFileNo,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    var declarationList: DeclarationList;
                    if (serviceResponse.Result) {


                        var entity: DeclarationList;
                        declarationList = entity = this.MapJsonToEntityList(serviceResponse.Result);



                    }

                    serviceResponse.Result = declarationList;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSupplierInvoiceItemsCount(declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSupplierInvoiceItemCount/?' + 'declarationId=' + declarationId,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSingleDeclarationByNumber(declarationByNumber: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetSingleDeclarationByCustomFileNo';

        return Observable.defer(() => {
            return this._http
                .get(this._apiUrl + '/GetSingleDeclarationByNumber/?' + 'declarationByNumber=' + declarationByNumber + '&tenant=' + tenant,
                    ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;
                        var declarationList: DeclarationList;
                        if (serviceResponse.Result) {


                            var entity: DeclarationList;
                            declarationList = entity = this.MapJsonToEntityList(serviceResponse.Result);



                        }

                        serviceResponse.Result = declarationList;
                        return serviceResponse;
                    }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetConsignmentListPMByCustomFileNo(customFileNo: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetConsignmentListPMByCustomFileNo';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetConsignmentListPMByCustomFileNo/?' + 'customFileNo=' + customFileNo,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDeclarationPendingListPMByDeclarationId(declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetDeclarationPendingListPMByDeclarationId';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDeclarationPendingListPMByDeclarationId/?' + 'declarationId=' + declarationId,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetCurrenciesCodesForDeclaration(declarationId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        //var url = this._apiUrl + '/GetConsignmentListPMByCustomFileNo';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCurrenciesCodesForDeclaration/?' + 'declarationId=' + declarationId + '&tenant=' + tenant,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSingleDeclarationPMByCargoIdentifiers(cargoTypeCode: string, manifestNumber: string, secondCargoID: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        //var url = this._apiUrl + '/GetConsignmentListPMByCustomFileNo';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleDeclarationPMByCargoIdentifiers/?' + 'cargoTypeCode=' + cargoTypeCode + '&manifestNumber=' + manifestNumber + '&secondCargoID=' + secondCargoID + '&tenant=' + tenant,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    PostSendCollateral8212(requestParams: SendCollateralRequestParams) {

        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendCollateral8212/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;


                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;


                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }

    PutCopyDeclaration_test(fromDeclarationId: string , tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutCopyDeclaration_test/?' + 'fromDeclarationId=' + fromDeclarationId + '&tenant=' + tenant,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    PutCopyDeclaration(fromDeclarationId: string, toDeclarationId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);



        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutCopyDeclaration/?' + 'fromDeclarationId=' + fromDeclarationId + '&toDeclarationId=' + toDeclarationId + '&tenant=' + tenant,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDeclarationByCustomFileNoAndCCU(customFileNo: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDeclarationByCustomFileNoAndCCU/?' + 'customFileNo=' + customFileNo,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    var declarationList: DeclarationList;
                    if (serviceResponse.Result) {
                        var entity: DeclarationList;
                        declarationList = entity = this.MapJsonToEntityList(serviceResponse.Result);
                    }

                    serviceResponse.Result = declarationList;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }



    GetDeclarationAmendmentsById(id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetDeclarationAmendmentsById/?' + 'id=' + id,
                ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var list:any = response;

                    var _mappedListsArray: Array<DeclarationList> = [];
                    if (list) {
                        for (var key in list.Result) {
                            var entity: DeclarationList;
                            entity = this.MapJsonToEntityList(list.Result[key]);
                            _mappedListsArray.push(entity);
                        }
                    }
 
                    serviceResponse.Result = _mappedListsArray;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });


    }
    MapJsonToEntityList(jsonList: any) {

        var entityList: DeclarationList;
        entityList = new DeclarationList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }



    getByFilters(filters: ApiQueryFilters) {

        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var propValue;
        for (var i in mykeys) {
            var propName = mykeys[i];
            propValue = filters[propName];
        }

        var urlparameters = '/GetDeclarationAmendmentsById/?' + 'id=' + filters.AdditionalFilters[0].FieldValue;
 

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callUrl = this._apiUrl.concat(urlparameters);//


        return Observable.defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {

                var serviceResponse: ServiceResponse;
                serviceResponse = response;
                var _mappedListsArray: Array<DeclarationList> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: DeclarationList;
                        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


}
