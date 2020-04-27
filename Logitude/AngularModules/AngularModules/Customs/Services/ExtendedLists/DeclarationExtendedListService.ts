import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

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



        return defer(() => {
          return this._http.get(this._apiUrl + '/GetSingleDeclarationByCustomFileNo/?' + 'customFileNo=' + customFileNo, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


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



        return defer(() => {
          return this._http.get(this._apiUrl + '/GetSupplierInvoiceItemCount/?' + 'declarationId=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


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

        return defer(() => {
            return this._http
              .get(this._apiUrl + '/GetSingleDeclarationByNumber/?' + 'declarationByNumber=' + declarationByNumber + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


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

        return defer(() => {
          return this._http.get(this._apiUrl + '/GetConsignmentListPMByCustomFileNo/?' + 'customFileNo=' + customFileNo, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


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

        return defer(() => {
          return this._http.get(this._apiUrl + '/GetCurrenciesCodesForDeclaration/?' + 'declarationId=' + declarationId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


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

        return defer(() => {
          return this._http.get(this._apiUrl + '/GetSingleDeclarationPMByCargoIdentifiers/?' + 'cargoTypeCode=' + cargoTypeCode + '&manifestNumber=' + manifestNumber + '&secondCargoID=' + secondCargoID + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    PutCopyDeclaration(fromDeclarationId: string, toDeclarationId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

   

        return defer(() => {
          return this._http.put(this._apiUrl + '/PutCopyDeclaration/?' + 'fromDeclarationId=' + fromDeclarationId + '&toDeclarationId=' + toDeclarationId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDeclarationByCustomFileNoAndCCU(customFileNo: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
          return this._http.get(this._apiUrl + '/GetDeclarationByCustomFileNoAndCCU/?' + 'customFileNo=' + customFileNo, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

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



}
