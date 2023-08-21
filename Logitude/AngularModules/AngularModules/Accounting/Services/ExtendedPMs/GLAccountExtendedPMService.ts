import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
 

@Injectable()

export class GLAccountExtendedPMService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {

        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GLAccountViews';
    }

    GetSplittedByCurrencyGLAccounts(accountId: string) {

     return this.httpClient.get(this._apiUrl + '/GetSplittedByCurrencyGLAccounts?accountId=' + accountId,  ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                var _mappedListsArray: Array<GLAccountPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: GLAccountPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));
       
    }

    ConnectCardToGLAccount(accountId: string, cardId: string, skipConnectedCardsValidation: boolean = false)
    {
    
        
        var api = ServiceHelper.GetLogitudeURL() + 'api/GLAccounts';
        return this.httpClient.get(api + '/GetConnectCardToGLAccount?accountId=' + accountId
        + '&cardId=' + cardId
        + '&skipConnectedCardsValidation=' + skipConnectedCardsValidation,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
       

    }

    GetConnectedCardsForGLAccount(accountId: string)
    {
     
     var api = ServiceHelper.GetLogitudeURL() + 'api/GLAccounts';
     return this.httpClient.get(api + '/GetConnectedCardsForGLAccount?accountId=' + accountId,  ServiceHelper.GetHttpHeaders()).pipe(
         map(response => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

            serviceResponse.Result = response;

            return serviceResponse;
         }),
         catchError(ServiceHelper.HandleServiceError));
        

    }

    GetByGLAccountsDisplayNumber(displayNumber: string, tenant: number)
    {
     
     var api = ServiceHelper.GetLogitudeURL() + 'api/GLAccounts';
     return this.httpClient.get(api + '/GetByGLAccountsDisplayNumber?displayNumber=' + displayNumber + '&tenant=' + tenant,  ServiceHelper.GetHttpHeaders()).pipe(
         map(response => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

            serviceResponse.Result = response;

            return serviceResponse;
         }),
         catchError(ServiceHelper.HandleServiceError));
    }

    GetGLAReconcilationCount(accountId: string)
    {
     
     var api = ServiceHelper.GetLogitudeURL() + 'api/GLAccounts';
     return this.httpClient.get(api + '/GetGLAReconcilationCount?accountId=' + accountId,  ServiceHelper.GetHttpHeaders()).pipe(
         map(response => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

            serviceResponse.Result = response;

            return serviceResponse;
         }),
         catchError(ServiceHelper.HandleServiceError));
    }



    GetARPyamentChequesListAsLedgerTransactions(accountId: string) {

        var api = ServiceHelper.GetLogitudeURL() + 'api/GLAccounts';
        return this.httpClient.get(this._apiUrl  + '/GetARPyamentChequesListAsLedgerTransactions?accountId=' + accountId, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }



    MapJsonToEntityPM(jsonPM: any) {

        var entityList: GLAccountPM;
        entityList = new GLAccountPM();
        var jsonListKeys = Object.keys(jsonPM);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonPM[property];
        }


        return entityList;
    }

}
