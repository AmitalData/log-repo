import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../Utilities/SessionInfo';
@Injectable()

export class CCSWebService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CCSWebService';
    }

    Send(myShipmentId: string, myRecipient: string, isSendingCargonaut: boolean, isSendingDEXX: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMessageResult?myShipmentId=' + myShipmentId + '&myRecipient=' + myRecipient + '&isSendingCargonaut=' + isSendingCargonaut + '&isSendingDEXX=' + isSendingDEXX;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetFHLsValidation(myMasterId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetFHLsValidation?myMasterId=' + myMasterId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<FHLShipmentValidator> = [];

                for (var key in allLists) {
                    var entity: FHLShipmentValidator;
                    entity = this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetSendingValidations(myShipmentId: string, myRecipient: string, isSendingFHLs: boolean, isSendingCargonaut: boolean, isSendingDEXX: boolean, mainCarriageCarrierId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSendingValidations?myShipmentId=' + myShipmentId + '&myRecipient=' + myRecipient + '&isSendingFHLs=' + isSendingFHLs + '&isSendingCargonaut=' + isSendingCargonaut + '&isSendingDEXX=' + isSendingDEXX + '&mainCarriageCarrierId=' + mainCarriageCarrierId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAWBPrintingStock(myShipmentId: string, isCargonautSending: boolean, isDEXXSending: boolean, isConfirmedByUser: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetAWBPrintingStock?myShipmentId=' + myShipmentId + '&isCargonautSending=' + isCargonautSending + '&isDEXXSending=' + isDEXXSending + '&isConfirmedByUser=' + isConfirmedByUser;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

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
            }).catch(ServiceHelper.HandleServiceError);
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

