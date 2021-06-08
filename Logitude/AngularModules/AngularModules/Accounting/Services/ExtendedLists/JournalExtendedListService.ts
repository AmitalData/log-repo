import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {JournalList} from '../../EntityLists/JournalList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 
@Injectable()

export class JournalExtendedListService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
     
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/journalviews';
    }

    GetRecentJournals() {
      

        var url = this._apiUrl + '/GetRecentJournals';
        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 
       
    }

    GetJournalsByAccountingEntityId(entityId:string, entityCode:string) {
    

       
        var url = this._apiUrl + '/GetJournalsByAccountingEntityId?EntityId=' + entityId + '&entityCode=' + entityCode;
       
        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
             
                var allLists = response ;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 

    }
    
    GetResetJournalByJournalId(entityId: string) {


        var url = this._apiUrl + '/GetResetJournalByJournalId?JournalId=' + entityId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map((response,indx) => {

                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }
    GetJournalMoreDatasByJournalId(entityId: string) {


        var url = this._apiUrl + '/GetJournalMoreDatasByJournalId?JournalId=' + entityId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {

                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetJournalLinesByJournalId(entityId: string) {
  

        var url = this._apiUrl + '/GetJournalLinesByJournalId?JournalId=' + entityId;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
             
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 
        
    
    }

    GetByJournalNumber(journalNumber) {

    

        var url = this._apiUrl + '/GetByJournalNumber?journalNumber=' + journalNumber;

        return this.httpClient.get(url,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
             
                var list = response ;

                var entity: JournalList;
                if (list) {
                    entity = this.MapJsonToEntityList(list);
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError)); 

     

    }
    
    GetJournalsSummary() {
      

        return this.httpClient.get(this._apiUrl + '/GetJournalsSummary?',  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;
                return allLists;
            }),
            catchError(ServiceHelper.HandleServiceError)); 


      
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: JournalList;
        entityList = new JournalList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}
