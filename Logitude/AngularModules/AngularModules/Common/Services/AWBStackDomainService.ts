import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {MAWBStackPM} from '../EntityPMs/MAWBStackPM';

@Injectable()

export class AWBStackDomainService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AWBStackDomain';
    }

    GetMAWBStackPMsByAirlineId(myCardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMsByAirlineId?myCardId=' + myCardId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<MAWBStackPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: MAWBStackPM = this.MapStackPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetMAWBStackPMByNumber(number: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMByNumber?number=' + number;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var itemJason = response;
                var itemMapped: MAWBStackPM;

                if (itemJason) {
                    itemMapped = this.MapStackPM(itemJason);
                }

                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = itemMapped;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetMAWBStackPMsCountByAirlineIdAndShipperId(airlineId: string, customerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMsCountByAirlineIdAndShipperId?airlineId=' + airlineId + '&customerId=' + customerId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var itemJason = response;
                var itemMapped: number = +itemJason;

                return itemMapped;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCardHasAssignedMawbStacks(airlineId: string, customerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCardHasAssignedMawbStacks?airlineId=' + airlineId + '&customerId=' + customerId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {
      
                var itemMapped: Boolean = response.body;   //== "true" ? true : false;
                return itemMapped;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetMAWBStackPMsByAirlineIdAndShipperId(airlineId: string, customerId: string, pageSize: number, pageIndex: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMAWBStackPMsByAirlineIdAndShipperId?airlineId=' + airlineId + '&customerId=' + customerId + '&pageSize=' + pageSize + '&pageIndex=' + pageIndex;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<MAWBStackPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: MAWBStackPM = this.MapStackPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                return listMapped;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCustomerStockSeries(myCustomerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCustomerStockSeries?myCustomerId=' + myCustomerId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetAllAvailableStockSeries() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllAvailableStockSeries';

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetAirlineAvailableStockSeries(airlineId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlineAvailableStockSeries?airlineId=' + airlineId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    AssignStockSeriesToCustomer(start: number, end: number, airlineId: string, customerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAssignStockSeriesToCustomer?start=' + start + '&end=' + end + '&airlineId=' + airlineId + '&customerId=' + customerId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    UnAssignStockSeriesToUser(start: number, end: number, airlineId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUnAssignStockSeriesToUser?start=' + start + '&end=' + end + '&airlineId=' + airlineId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    DeleteMAWBStacksOperation(myStackId: string, myAirlineId: string, isDeletingSeries: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetDeleteMAWBStacksOperation?myStackId=' + myStackId + '&myAirlineId=' + myAirlineId + '&isDeletingSeries=' + isDeletingSeries;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    CreateMAWBStacksOperation(myAirlineId: string, myStartNumber: number, myEndNumber: number, assignedToId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCreateMAWBStacksOperation?myAirlineId=' + myAirlineId + '&myStartNumber=' + myStartNumber + '&myEndNumber=' + myEndNumber + '&assignedToId=' + assignedToId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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
