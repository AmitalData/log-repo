import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {InfraGenericFilter} from '../../Utilities/InfraGenericFilter';
import {CachedDataManager} from '../../Utilities/CachedDataManager';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {RatesTableList} from '../../EntityLists/RatesTableList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
@Injectable()

export class RatesTableExtendedListService {
	private _http: Http;
    private _apiUrl: string;   
	public static CachedData: Array<RatesTableList> = [];
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ratestableviews';  
    }

    getClosestRate(baseCurrenyId: string, foreignCurrencyId: string) {
	   
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetClosestRate/?' + 'baseCurrenyId=' + baseCurrenyId + '&foreignCurrencyId=' + foreignCurrencyId, { headers: authHeader }).map(response => {
                var list = response.json();
                    
                var entity: RatesTableList;
				if(list)
				{
                   entity = this.MapJsonToEntityList(list);
                }   

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse(); 
                serviceResponse.Result = entity;  
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
	MapJsonToEntityList(jsonList: any) {
       
            var entityList: RatesTableList;
            entityList = new RatesTableList();
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityList[property] = jsonList[property];
            }
			

        return entityList;
    }

}

