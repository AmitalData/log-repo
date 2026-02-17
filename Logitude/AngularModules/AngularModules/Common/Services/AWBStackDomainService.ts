import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {MAWBStackPM} from '../EntityPMs/MAWBStackPM';

@Injectable()

export class AWBStackDomainService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AWBStackDomain';
    }

    GetMAWBStackPMsByAirlineId(myCardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMsByAirlineId?myCardId=' + myCardId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<MAWBStackPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: MAWBStackPM = this.MapStackPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetMAWBStackPMByNumber(number: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMByNumber?number=' + number;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var itemJason = response.json();
                var itemMapped: MAWBStackPM;

                if (itemJason) {
                    itemMapped = this.MapStackPM(itemJason);
                }

                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = itemMapped;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetMAWBStackPMsCountByAirlineIdAndShipperId(airlineId: string, customerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMsCountByAirlineIdAndShipperId?airlineId=' + airlineId + '&customerId=' + customerId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var itemJason = response.json();
                var itemMapped: number = +itemJason;

                return itemMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCardHasAssignedMawbStacks(airlineId: string, customerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCardHasAssignedMawbStacks?airlineId=' + airlineId + '&customerId=' + customerId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var itemJason = response.json();
                var itemMapped: Boolean = itemJason; //== "true" ? true : false;
                return itemMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetMAWBStackPMsByAirlineIdAndShipperId(airlineId: string, customerId: string, pageSize: number, pageIndex: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMsByAirlineIdAndShipperId?airlineId=' + airlineId + '&customerId=' + customerId + '&pageSize=' + pageSize + '&pageIndex=' + pageIndex;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<MAWBStackPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: MAWBStackPM = this.MapStackPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCustomerStockSeries(myCustomerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCustomerStockSeries?myCustomerId=' + myCustomerId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAllAvailableStockSeries() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllAvailableStockSeries';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAirlineAvailableStockSeries(airlineId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlineAvailableStockSeries?airlineId=' + airlineId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    AssignStockSeriesToCustomer(start: number, end: number, airlineId: string, customerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAssignStockSeriesToCustomer?start=' + start + '&end=' + end + '&airlineId=' + airlineId + '&customerId=' + customerId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    UnAssignStockSeriesToUser(start: number, end: number, airlineId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUnAssignStockSeriesToUser?start=' + start + '&end=' + end + '&airlineId=' + airlineId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    DeleteMAWBStacksOperation(myStackId: string, myAirlineId: string, isDeletingSeries: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetDeleteMAWBStacksOperation?myStackId=' + myStackId + '&myAirlineId=' + myAirlineId + '&isDeletingSeries=' + isDeletingSeries;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    CreateMAWBStacksOperation(myAirlineId: string, myStartNumber: number, myEndNumber: number, assignedToId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCreateMAWBStacksOperation?myAirlineId=' + myAirlineId + '&myStartNumber=' + myStartNumber + '&myEndNumber=' + myEndNumber + '&assignedToId=' + assignedToId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    MapStackPM(jsonList: any) {
        var entityPM: MAWBStackPM = null;

        if (jsonList) {
            entityPM = new MAWBStackPM();

            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }

            entityPM.IsDirty = false;
        }

        return entityPM;
    }
}

export class StockSeries {
    public Tenant: number;
    public From: number;
    public To: number;
    public Total: number;
    public CustomerId: string;
    public AirlineId: string;
    public AirlineName: string;
}

export class StockSeriesListClass {
    public StockSeriesList: StockSeries[] = [];
}