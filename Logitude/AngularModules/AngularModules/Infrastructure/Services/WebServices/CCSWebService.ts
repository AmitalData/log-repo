import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class CCSWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CCSWebService';
    }

    Send(myShipmentId: string, myRecipient: string, isSendingCargonaut: boolean, isSendingDEXX: boolean) {
        var url = this._apiUrl + '/GetMessageResult?myShipmentId=' + myShipmentId + '&myRecipient=' + myRecipient + '&isSendingCargonaut=' + isSendingCargonaut + '&isSendingDEXX=' + isSendingDEXX;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: CCSResult = new CCSResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetFHLsValidation(myMasterId: string) {
        var url = this._apiUrl + '/GetFHLsValidation?myMasterId=' + myMasterId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<FHLShipmentValidator> = [];

                for (var key in allLists) {
                    var entity: FHLShipmentValidator;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSendingValidations(myShipmentId: string, myRecipient: string, isSendingFHLs: boolean, isSendingCargonaut: boolean, isSendingDEXX: boolean, mainCarriageCarrierId: string) {
        var url = this._apiUrl + '/GetSendingValidations?myShipmentId=' + myShipmentId + '&myRecipient=' + myRecipient + '&isSendingFHLs=' + isSendingFHLs + '&isSendingCargonaut=' + isSendingCargonaut + '&isSendingDEXX=' + isSendingDEXX + '&mainCarriageCarrierId=' + mainCarriageCarrierId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: AWBResultClass = new AWBResultClass();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAWBPrintingStock(myShipmentId: string, isCargonautSending: boolean, isDEXXSending: boolean, isConfirmedByUser: boolean) {
        var url = this._apiUrl + '/GetAWBPrintingStock?myShipmentId=' + myShipmentId + '&isCargonautSending=' + isCargonautSending + '&isDEXXSending=' + isDEXXSending + '&isConfirmedByUser=' + isConfirmedByUser;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var mappedResult: AWBPrintResult = new AWBPrintResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: FHLShipmentValidator;
        entityList = new FHLShipmentValidator();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}

export class CCSResult {
    public Id: string;
    public IsValid: boolean;
    public IsMasterFieldMissing: boolean;
    public HasStockError: boolean;
    public IsFNAValidationLong: boolean;
    public SendingCount: number;
    public StockRemainingBefore: number;
    public StockRemainingAfter: number;
    public IsDemoTenant: boolean;
}

export class AWBResultClass {
    public Id: string;
    public Tenant: number;
    public TTY: string;
    public PIMA: string;
    public ShipmentId: string;
    public Recipient: string;
    public StockFHLCode: string;
    public StockFWBCode: string;
    public SendingCount: number;
    public IsSendingFHLs: boolean;
    public AllHousesCount: number;
    public ValidHousesCount: number;
    public IsAWBStockPrepaid: boolean;
    public StockRemainingBefore: number;
    public StockRemainingAfter: number;
    public IsCargonautEnabled: boolean;
    public IsCargonautSending: boolean;
    public IsDEXXEnabled: boolean;
    public IsDEXXSending: boolean;
    public IsDemoTenant: boolean;
    public IsValid: boolean;
    public HasMainErrors: boolean;
    public HasStockErrors: boolean;
    public IsEAWBOnlyDemo: boolean;
    public AWBMessagesCCSTypeCode: string;
    public ErrorsList: string[] = [];
    public ValidFHLsDataStringList: string[] = [];
}

export class FHLShipmentValidator {
    public Id: string;
    public IsFHLValid: boolean;
    public ShipmentId: string;
    public ShipmentNumber: string;
    public Shipper: string;
    public FNAReason: string;
    public FHLStatusCode: string;
    public FHLStatusName: string;
    public CargonautFHLStatusCode: string;
    public CargonautFHLStatusName: string;
    public FHLErrors: string[] = [];
}

export class AWBPrintResult {
    Id: string;
    Tenant: number;
    ShipmentId: string;
    IsDEXXSending: boolean;
    IsCargonautSending: boolean;
    LoggedContactId: string;
    StockFHLCode: string;
    StockFWBCode: string;
    StockRemainingBefore: number;
    StockRemainingAfter: number;
    IsDemoTenant: boolean;
    IsAWBStockPrepaid: boolean;
    IsConfirmedByUser: boolean;
    IsPrintingAllowed: boolean;
    IsStockAlreadyTaken: boolean;
    IsNoRemainingStocks: boolean;
}
