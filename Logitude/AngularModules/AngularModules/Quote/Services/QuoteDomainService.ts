import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {QuotePM} from '../EntityPMs/QuotePM';
import {QuoteSettingPM} from '../EntityPMs/QuoteSettingPM';
import {QuoteStageList} from '../EntityLists/QuoteStageList';

export class QuoteDomainService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteDomain';
    }

    GetQuotesCounts(ownerId: string, businessUnitId: string, directionId: string, transportModeId: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetQuotesCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&directionId=' + directionId + '&transportModeId=' + transportModeId + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new CRMSummary();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetQuotesByOpportunityId(oportunityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetQuotesByOpportunityId?oportunityId=' + oportunityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });


    }
    ConnectQuotesToOpportunity(oportunityId: string, quotesIds:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetConnectQuotesToOpportunity?opportunityId=' + oportunityId + '&quotesIds=' + quotesIds;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });


    }
    GetRecentQuotes(ownerId: string, businessUnitId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRecentQuotes?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }  
    GetDataCountsForCRM(tenant: number, customerid: string) {
        var _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomersData';
        var authHeader = new Headers();

        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = _apiUrl + '/GetDataCountsForCRM?customerId=' + customerid + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetStageFunnelData(ownerId: string, businessUnitId: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetStageFunnelData?OwnerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse.Result;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }    
    GetCRMMoneyInformation(tenant: number, customerid: string) {
        var _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomersData';
        var authHeader = new Headers();

        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = _apiUrl + '/GetCRMMoneyInformation?CRMMoneyCustomerId=' + customerid + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetSingleQuoteStageListByCode(code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSingleQuoteStageListByCode?code=' + code;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var mappedResult: QuoteStageList;

                if (myJsonResult) {
                    mappedResult = new QuoteStageList();

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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }    
    ComputeQuoteAutomaticSubject(entityPM: QuotePM) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            
            var args = new QuoteSubjectArgs();
            args.EntityId = entityPM.Id;
            args.DirectionId = entityPM.DirectionId;
            args.TransportModeId = entityPM.TransportModeId;
            args.IncotermId = entityPM.IncotermId;
            args.FromPartnerAddressId = entityPM.FromPartnerAddressId;
            args.ToPartnerAddressId = entityPM.ToPartnerAddressId;
            args.IncludePickUp = entityPM.IncludePickUp;
            args.PickUpAddressId = entityPM.PickUpAddressId;
            args.FromAddressCity = entityPM.FromAddressCity;
            args.FromAddressZipCode = entityPM.FromAddressZipCode;
            args.FromPortId = entityPM.FromPortId;
            args.IncludeDelivery = entityPM.IncludeDelivery;
            args.DeliveryAddressId = entityPM.DeliveryAddressId;
            args.ToAddressCity = entityPM.ToAddressCity;
            args.ToAddressZipCode = entityPM.ToAddressZipCode;
            args.ToPortId = entityPM.ToPortId;
           
            var mappedEntity: QuoteSubjectArgs = this.MapJsonToQuoteSubjectArgs(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: QuoteSubjectArgs = this.MapJsonToQuoteSubjectArgs(myJsonResult, true, args);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetActivitiesByQuoteId(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetActivitiesByQuoteId?entityId=' + entityId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetIsQuoteConnectedToShipment(quoteId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetIsQuoteConnectedToShipment?quoteId=' + quoteId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var result = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });


    }

    GetQuoteSettings() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetQuoteSettings';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var itemJSON = response.json();
                var itemMapped: QuoteSettingPM = this.MapQuoteSettings(itemJSON);

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    UpdateQuoteSettings(entityPM: QuoteSettingPM) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: QuoteSettingPM = this.MapQuoteSettings(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: QuoteSettingPM = this.MapQuoteSettings(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    MapQuoteSettings(jsonPM: any, getCallMap: boolean = true, entityPM: QuoteSettingPM = null) {

        if (!entityPM) {
            entityPM = new QuoteSettingPM();
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

    MapJsonToQuoteSubjectArgs(jsonPM: any, getCallMap: boolean = true, entity: QuoteSubjectArgs = null) {
        if (!entity) {
            entity = new QuoteSubjectArgs();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }

    GetQuoteConnectedEntities(quoteId: string, opportunityId: string ) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetQuoteConnectedEntities?quoteId=' + quoteId + '&opportunityId=' + opportunityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();
                var listMapped: Array<QuoteConnectedEntity> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: QuoteConnectedEntity = this.MapQuoteConnectedEntity(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
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
export class QuoteSubjectArgs {
    public EntityId: string;
    public DirectionId: string;
    public TransportModeId: string;
    public IncotermId: string;
    public FromPartnerAddressId: string;
    public ToPartnerAddressId: string;
    public IncludePickUp: boolean;
    public PickUpAddressId: string;
    public FromAddressCity: string;
    public FromAddressZipCode: string;
    public FromPortId: string;
    public IncludeDelivery: boolean;
    public DeliveryAddressId: string;
    public ToAddressCity: string;
    public ToAddressZipCode: string;
    public ToPortId: string;
    public Subject: string;
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
