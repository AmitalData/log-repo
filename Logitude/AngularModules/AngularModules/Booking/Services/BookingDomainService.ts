import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {BookingAnswerPM} from '../EntityPMs/BookingAnswerPM';
import {ChartingDataClass} from '../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';

export class BookingDomainService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BookingDomain';
    }

    GetBookingsCounts() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetBookingsCounts';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new BookingsDataCounts();

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

    GetRecentBookings() {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetRecentBookings';

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

    GetBookingAnswerPMs(bookingId: string) {  
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetBookingAnswerPMs?bookingId=' + bookingId;
        
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



    GetBookingsDashBoard(Tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetBookingsDashBoard?tenant=' + Tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists: ChartingDataClass[] = response.json();
                var myList: Array<ChartingDataClass> = new Array<ChartingDataClass>();
                for (var key in allLists) {
                    var entity: ChartingDataClass;
                    entity = this.MapJsonToEntityListChartingDataClass(allLists[key]);
                    myList.push(entity);
                }


                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myList;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }



    ValidateBookingForSending(bookingId: string, isCancellationSent: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var url = this._apiUrl + '/GetValidateBookingForSending?bookingId=' + bookingId + "&isCancellationSent=" + isCancellationSent;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new BookingValidatorResultClass();

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

    ValidateBookingMasterFieldExistance(entityId: string, myMasterField: string, myAirlinePrefixField: string, myDirectionId: string, myTransportModeId: string, isCancelled: boolean, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var args = new ValidateShipmentMasterArgs();
            args.BookingId = entityId;
            args.Master = myMasterField;
            args.AirlinePrefix = myAirlinePrefixField;
            args.DirectionId = myDirectionId;
            args.TransportModeId = myTransportModeId;
            args.IsCancelled = isCancelled;

            var mappedEntity: ValidateShipmentMasterArgs = this.MapJsonToValidateShipmentMasterArgs(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                { headers: authHeader }).map((res) => {
                    var myJsonResult = res.json();

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = myJsonResult;
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    private MapJsonToValidateShipmentMasterArgs(jsonPM: any, getCallMap: boolean = true, entity: ValidateShipmentMasterArgs = null) {
        if (!entity) {
            entity = new ValidateShipmentMasterArgs();
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

    MapJsonToEntityListChartingDataClass(jsonList: any) {

        var entityList: ChartingDataClass;
        entityList = new ChartingDataClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}

export class BookingsDataCounts {
    public Id: number;
    public CreatedBookingsCount: number;
    public WaitingForResponseCount: number;
    public ConfirmedBookingsCount: number;
    public RejectedBookingsCount: number;
    public InProgressBookingsCount: number;
    public AllBookingsCount: number;
    public CancelledBookingsCount: number;
}
export class ValidateShipmentMasterArgs {
    public BookingId: string;
    public Master: string;
    public AirlinePrefix: string;
    public DirectionId: string;
    public TransportModeId: string;
    public IsCancelled: boolean;
}
export class BookingValidatorResultClass {
    public Id: number;
    public Tenant: number;
    public TTY: string;
    public PIMA: string;
    public BookingId: string;
    public IsDemoTenant: boolean;
    public IsValid: boolean;
    public HasMainErrors: boolean;
    public HasStockErrors: boolean;
    public IsEAWBOnlyDemot: boolean;
    public AWBMessagesCCSTypeCode: string;
    public ErrorsList: string[] = [];
}
