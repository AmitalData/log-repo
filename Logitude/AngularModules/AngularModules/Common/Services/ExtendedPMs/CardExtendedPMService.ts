
import { Injectable } from '@angular/core';

import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { CardPM } from '../../EntityPMs/CardPM';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { CardList } from 'Common/EntityLists/CardList';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';
import { CardListService } from '../StandardLists/CardListService';


@Injectable()

export class CardExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CardExtended';
    }


    DisconnectGLAccountFromCard(id: string, partnerTypeId: string, eventTypeCode:string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDisconnectGLAccountFromCard?' + 'id=' + id + '&partnerTypeId=' + partnerTypeId + '&eventTypeCode=' + eventTypeCode,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;
                

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;



                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }

    GetAllConnectedPartnersByGLAccountId(glAccountId: string) {

        var url = this._apiUrl + '/GetAllConnectedPartnersByGLAccountId?glAccountId=' + glAccountId;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }
    getByCompactFilters(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
        

        return new Promise((resolve, reject) => {
                resolve(this.getByCompactFiltersShort(filters));
           
        });
    }
    getByCompactFiltersShort(filters: ApiQueryFilters) {

		var callTime = new Date();        
		var urlparameters = '/GetByCompactFiltersShort?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;

        for (var i in mykeys) {
			var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters" || propName == "TreeFilters"  || propName == "ParentEntity");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }

            if (!ignoreFilter) {
				propValue = encodeURIComponent(propValue);
				urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
			}

			if (propName == "TreeFilters" && propValue && propValue.length > 0) {
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }
			
			if (propName == "ParentEntity" && propValue) {
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0) {
                addtionalFiltersValues = JSON.stringify(propValue);
			}
        }

        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var callUrl = this._apiUrl.concat(urlparameters);
        		
		return defer(() => {
			return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders())
				.pipe(
					map((response: HttpResponse<any>) => {

						var serviceResponse: ServiceResponse = response.body;
						var _mappedListsArray: Array<CardList> = [];
                        var cardListService:CardListService=new CardListService()

						if (serviceResponse.Result) {
							for (var key in serviceResponse.Result) {				
								var entity: CardList = cardListService.MapJsonToEntityList(serviceResponse.Result[key]);
								_mappedListsArray.push(entity);
							}
						}

						serviceResponse.Result = _mappedListsArray; 
						serviceResponse.CallTime = callTime;

						var servertime = response.headers.get('ServerExecutionTime');
						PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Card", "GetByCompactFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll); 
                 
						return serviceResponse;
					}),
				
					catchError(ServiceHelper.HandleServiceError));
		});        
	}

    GetAllCardsByVatNumber(vatNumber: string) {

        var url = this._apiUrl + '/GetAllCardsByVatNumber?vatNumber=' + vatNumber  ;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

}
