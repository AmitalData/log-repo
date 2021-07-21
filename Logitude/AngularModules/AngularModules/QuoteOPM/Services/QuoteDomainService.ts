import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {QuoteOPPM} from '../EntityPMs/QuoteOPPM';
import {QuoteOPSettingPM} from '../EntityPMs/QuoteOPSettingPM';
import {QuoteOPStageList} from '../EntityLists/QuoteOPStageList';
import { QuoteOPPMService } from './StandardPMs/QuoteOPPMService';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

export class QuoteDomainService {
    private _http: HttpClient;
    private _apiUrl: string;
    private _httpClient: HttpClient
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteDomain';
    }
    
    GetQuotesCounts(ownerId: string, businessUnitId: string, directionId: string, transportModeId: string, RecordsTypeCode: string) {

        var url = this._apiUrl + '/GetQuotesCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&directionId=' + directionId + '&transportModeId=' + transportModeId + '&RecordsTypeCode=' + RecordsTypeCode;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
                
                map(response => {
                    var myJsonResult = response;

                    var myResult = new CRMSummary();

                    if (myJsonResult) {
                        var jsonListKeys = Object.keys(myJsonResult);
                        for (var key in jsonListKeys) {
                            var property = jsonListKeys[key];
                            myResult[property] = myJsonResult[property];
                        }
                    }

                    var serviceResponse = new ServiceResponse();
                    serviceResponse.Result = myResult;
                    return serviceResponse;
                }),
                
                catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetQuotesByOpportunityId(oportunityId: string) {

        var url = this._apiUrl + '/GetQuotesByOpportunityId?oportunityId=' + oportunityId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });


    }
    ConnectQuotesToOpportunity(oportunityId: string, quotesIds:string) {
   
        var url = this._apiUrl + '/GetConnectQuotesToOpportunity?opportunityId=' + oportunityId + '&quotesIds=' + quotesIds;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });


    }
    GetRecentQuotes(ownerId: string, businessUnitId: string) {

        var url = this._apiUrl + '/GetRecentQuotes?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
                
                map(response => {
                    var allLists = response;

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = allLists;
                    return serviceResponse;
                }),
                
                catchError(ServiceHelper.HandleServiceError));
        });
    }  
    GetDataCountsForCRM(tenant: number, customerid: string) {
        var _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomersData';

        var url = _apiUrl + '/GetDataCountsForCRM?customerId=' + customerid + '&tenant=' + tenant;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetStageFunnelData(ownerId: string, businessUnitId: string, RecordsTypeCode: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetStageFunnelData?OwnerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse.Result;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }    
    GetCRMMoneyInformation(tenant: number, customerid: string) {
        var _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomersData';

        var url = _apiUrl + '/GetCRMMoneyInformation?CRMMoneyCustomerId=' + customerid + '&tenant=' + tenant;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSingleQuoteOPStageListByCode(code: string) {

        var url = this._apiUrl + '/GetSingleQuoteOPStageListByCode?code=' + code;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var mappedResult: QuoteOPStageList;

                if (myJsonResult) {
                    mappedResult = new QuoteOPStageList();

                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }    
    ComputeQuoteAutomaticSubject(entityPM: QuoteOPPM) {
        return defer(() => {

            var iService = new QuoteOPPMService();
            var mappedEntity: QuoteOPPM = iService.MapJsonToEntityPM(entityPM, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetActivitiesByQuoteId(entityId: string) {

        var url = this._apiUrl + '/GetActivitiesByQuoteId?entityId=' + entityId;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetIsQuoteConnectedToShipment(quoteId: string) {

        var url = this._apiUrl + '/GetIsQuoteConnectedToShipment?quoteId=' + quoteId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });


    }

    GetQuoteSettings() {

        var url = this._apiUrl + '/GetQuoteSettings';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var itemJSON = response;
                var itemMapped: QuoteOPSettingPM = this.MapQuoteSettings(itemJSON);

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    UpdateQuoteSettings(entityPM: QuoteOPSettingPM) {
        return defer(() => {

            var mappedEntity: QuoteOPSettingPM = this.MapQuoteSettings(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: QuoteOPSettingPM = this.MapQuoteSettings(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapQuoteSettings(jsonPM: any, getCallMap: boolean = true, entityPM: QuoteOPSettingPM = null) {

        if (!entityPM) {
            entityPM = new QuoteOPSettingPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else {
                entityPM[property] = jsonPM[property];
            }
        }

        return entityPM;
    }

    GetQuoteConnectedEntities(quoteId: string, opportunityId: string ) {

        var url = this._apiUrl + '/GetQuoteConnectedEntities?quoteId=' + quoteId + '&opportunityId=' + opportunityId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var listMapped: Array<QuoteConnectedEntity> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: QuoteConnectedEntity = this.MapQuoteConnectedEntity(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapQuoteConnectedEntity(jsonList: any) {
        var entityList: QuoteConnectedEntity;
        entityList = new QuoteConnectedEntity();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}

export class CRMSummary {
    public Id: number;
    public Quotes_All: number;
    public Quotes_My: number;
    public Quotes_InProgress: number;
    public Quotes_Created: number;
    public Quotes_Draft: number;
    public Quotes_Expired: number;
    public Quotes_Accepted: number;
    public Quotes_AcceptedNOShip: number;
    public Quotes_Cancelled: number;
    public Quotes_Sent: number;
    public Quotes_AllFollowups: number;
    public Quotes_MyFollowups: number;
}

export class QuoteConnectedEntity {
    public EntityId: string;
    public EntityNumber: string;
    public ObjectTable: string;    
    public EntityStatus: string;
    public OpenDate: Date;
    public ShipmentType: string;
    public Master: string;
    public House: string;
    public From: string;
    public To: string;
    public Customer: string;
    public GrossWeight: number;
    public VolumeInKG: number;
    public EntityOwner: string;
    public EntityClosingDate: Date;
}
