import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {MessagingStockList} from '../EntityLists/MessagingStockList';
import {ShipmentPM} from '../EntityPMs/ShipmentPM';
import {ShipmentPMService} from './StandardPMs/ShipmentPMService';
import {ShipmentList} from '../EntityLists/ShipmentList';
import { MessagingStockUsageHistoryList } from '../EntityLists/MessagingStockUsageHistoryList';

@Injectable()

export class ShipmentDomainService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
    }

    GetShipmentsCounts(myDirectionId: string, myTransportModeId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentsCounts?myDirectionId=' + myDirectionId + '&myTransportModeId=' + myTransportModeId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var myResult = new ShipmentsSummary();

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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    CheckHousesOpenAmounts(masterId) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/CheckHousesOpenAmounts?masterId=' + masterId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetRecentShipments() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRecentShipments';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetDeparturesArrivals(myDirectionId: string, myTransportModeId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetDeparturesArrivals?myDirectionId=' + myDirectionId + '&myTransportModeId=' + myTransportModeId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var myResult: FlightSummary[] = [];

                for (var key in myJsonResult) {

                    var entity: FlightSummary;
                    entity = this.MapFlightSummary(myJsonResult[key]);
                    myResult.push(entity);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetShipmentCarrierStatuses(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentCarrierStatuses?entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<ShipmentCarrierStatusList> = [];

                for (var itemJeson in listJason) {

                    var itemMapped: ShipmentCarrierStatusList = this.MapShipmentCarrierStatus(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetLoggedTenantMessagingStockLists() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetLoggedTenantMessagingStockLists';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<MessagingStockList> = [];

                for (var itemJeson in listJason) {

                    var itemMapped: MessagingStockList = this.MapMessagingStockList(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetLoggedTenantMessagingStockUsageHistoryLists(stockId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetLoggedTenantMessagingStockUsageHistoryLists?stockId=' + stockId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<MessagingStockUsageHistoryList> = [];

                for (var itemJeson in listJason) {

                    var itemMapped: MessagingStockUsageHistoryList = this.MapMessagingStockUsageHistoryList(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    ValidateShipmentMasterFieldExistance(entityId: string, myBookingId: string, myMasterField: string, myAirlinePrefixField: string, myDirectionId: string, myTransportModeId: string, myShipmentLevelCode: string, isCancelled: boolean) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var args = new ValidateShipmentMasterArgs();
            args.ShipmentId = entityId;
            args.BookingId = myBookingId;
            args.Master = myMasterField;
            args.AirlinePrefix = myAirlinePrefixField;
            args.DirectionId = myDirectionId;
            args.TransportModeId = myTransportModeId;
            args.ShipmentLevelCode = myShipmentLevelCode;
            args.IsCancelled = isCancelled;

            var mappedEntity: ValidateShipmentMasterArgs = this.MapJsonToValidateShipmentMasterArgs(args, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                { headers: authHeader }).map((res) => {
                    var myJsonResult = res.json();
                    return myJsonResult;

                }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetMasterReceivables(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMasterReceivables?entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetInvoiceOpenAmountReceivables(invoiceTypeCode: string, entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetInvoiceOpenAmountReceivables?invoiceTypeCode=' + invoiceTypeCode + '&entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentsQuotesCount( ) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentsQuotesCount?';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentConsolidationPackages(masterId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentConsolidationPackages?masterId=' + masterId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentConnectedEntities(shipmentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentConnectedEntities?shipmentId=' + shipmentId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();
                var listMapped: Array<ShipmentConnectedEntity> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: ShipmentConnectedEntity = this.MapShipmentConnectedEntity(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentsQueriesCounts(tenant: number, transportModeId: string, directionId: string, SearchFilter: string, serviceContextUser: string, TypeCode: string = null) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentsQueriesCounts?tenant=' + tenant + '&transportModeId=' + transportModeId + '&directionId=' + directionId + '&SearchFilter=' + SearchFilter + '&serviceContextUser=' + serviceContextUser + '&TypeCode=' + TypeCode, {
                headers: authHeader
            }).map(response => {

                var myJsonResult = response.json();

                var myResult = new ImporterQueriesDataCounts();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                return myResult;
            });
        });
    }
    GetAllMasterHousesPayables(allHousesIdsString: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllMasterHousesPayables?allHousesIdsString=' + allHousesIdsString;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAllMasterHousesReceivables(allHousesIdsString: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllMasterHousesReceivables?allHousesIdsString=' + allHousesIdsString;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetConnectedShipmentsByMasterIdAndTenant(masterId: string,tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetConnectedShipmentsByMasterIdAndTenant?masterId=' + masterId + '&currentTenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<ShipmentPM> = [];

                var service: ShipmentPMService = new ShipmentPMService();
                for (var itemJeson in listJason) {

                    var itemMapped: ShipmentPM = service.MapJsonToEntityPM(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;


                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentsCountByQuoteId(quoteId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentsCountByQuoteId?quoteId=' + quoteId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentLevelCode(myShipmentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentLevelCode?myShipmentId=' + myShipmentId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult: string = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetInvoiceOpenAmountPayables(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetInvoiceOpenAmountPayables?entityId=' +  entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetPayableInvoices(PayableId: string, PayableParentId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetPayableInvoices?PayableId=' + PayableId + '&PayableParentId=' + PayableParentId ;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentsByQuoteId(quoteId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShipmentsByQuoteId?quoteId=' + quoteId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetSingleShipmentPMByNumber(shipmentNumber: string) {
        var authHeader = new Headers();
        authHeader.append('Token', sessionStorage.getItem("Token"));

        var url = this._apiUrl + '/GetSingleShipmentPMByNumber?shipmentNumber=' + shipmentNumber;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetSingleShipmentPMWithoutComposition(id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSingleShipmentPMWithoutComposition?id=' + id;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var service: ShipmentPMService = new ShipmentPMService();
                var shipment = service.MapJsonToEntityPM(myResult);
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = shipment;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    BlockNewARInvoice(shipmentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetBlockNewARInvoice?shipmentId=' + shipmentId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShipmentFullTextSearch(filters: ApiQueryFilters) {
        var urlparameters = this._apiUrl + '/GetShipmentFullTextSearch?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (!urlparameters.endsWith('?')) {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(urlparameters, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetMessagingStockListForTenantManagmentTab(tenantManagementId: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMessagingStockListForTenantManagmentTab?tenantManagementId=' + tenantManagementId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetShipmentCustomsTransmissionByShipmnetId(shipmentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetShipmentCustomsTransmissionByShipmnetId?shipmentId=' + shipmentId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    DisconnectQuote(shipmentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetDisconnectQuote?shipmentId=' + shipmentId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapFlightSummary(jsonList: any) {
        var entityList: FlightSummary;
        entityList = new FlightSummary();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    MapShipmentCarrierStatus(jsonList: any) {
        var entityList: ShipmentCarrierStatusList;
        entityList = new ShipmentCarrierStatusList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    MapMessagingStockList(jsonList: any) {
        var entityList: MessagingStockList;
        entityList = new MessagingStockList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    MapMessagingStockUsageHistoryList(jsonList: any) {
        var entityList: MessagingStockUsageHistoryList;
        entityList = new MessagingStockUsageHistoryList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    MapJsonToValidateShipmentMasterArgs(jsonPM: any, getCallMap: boolean = true, entity: ValidateShipmentMasterArgs = null) {
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
    MapJsonToEntityList(jsonList: any) {

        var entityList: ShipmentList;
        entityList = new ShipmentList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    SendToCustoms_AES(shipmentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSendToAESCustoms?shipmentId=' + shipmentId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetArtemusStatus(shipmentNumber:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetArtemusStatus?shipmentNumber=' + shipmentNumber;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    MapShipmentConnectedEntity(jsonList: any) {
        var entityList: ShipmentConnectedEntity;
        entityList = new ShipmentConnectedEntity();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    DownloadShipmentPackages(shipmentNumber: string, shipmentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetDownloadShipmentPackages?shipmentNumber=' + shipmentNumber + '&shipmentId=' + shipmentId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    CreateMissingMasters() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCreateMissingMasterData';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult: string = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    CheckIfConnectedEntryOrRelease(shipmentId) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetIfConnectedEntryOrRelease?shipmentId=' + shipmentId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    PostUploadExcelFile(filter: ExcelPackageFilter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadExcelFile", JSON.stringify(filter), {
                headers: authHeader,
            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }
}

export class ShipmentsSummary {
    public Id: number;
    public OperationalOpenCount_DH: number;
    public OperationalOpenCount_DC: number;
    public AccountingOpenCount_DH: number;
    public AccountingOpenCount_DC: number;
    public AllFollowUpsCount: number;
    public MyFollowUpsCount: number;
    public OperationalOpenCount_ETD: number;
    public OperationalOpenCount_LWU: number;
    public LastSentFSRCount: number;
    public ImportShipmentsCount: number;
    public CreditLimitBlockedCount: number;
    public ExpectedDeparturesNotTransmittedCount: number;
    public ShippingInstructionsLast7DaysCount: number;
    public ContainerStatusLast7DaysCount: number;
}
export class FlightSummary {
    public Id: string;
    public ShipmentId: string;
    public DirectionId: string;
    public TransportModeId: string;
    public CarrierId: string;
    public CarrierNumber: string;
    public CarrierCode: string;
    public CarrierName: string;
    public ExpectedDate: Date;
    public ActualDate: Date;
    public MainCarriageATA: Date;
    public ComputedStatusId: string;
}
export class ShipmentCarrierStatusList {
    public Id: string;
    public Tenant: number;
    public ShipmentId: string;
    public Status: string;
    public FromPortId: string;
    public ToPortId: string;
    public Details: string;
    public RecordHash: string;
    public Pieces: number;
    public Weight: number;
    public ReceivingDate: Date;
    public EventDate: Date;
    public Partial: boolean;
    public FlightNumber: string;
    public Location: string;
    public StatusName: string;
    public LocationCode: string;
    public LocationName: string;
    public AirlineName: string;
    public DepartureDate: Date;
    public ArrivalDate: Date;
    public TimeOfDepartureInfo: string;
    public TimeOfArrivalInfo: string;
}
export class ValidateShipmentMasterArgs {
    public ShipmentId: string;
    public BookingId: string;
    public Master: string;
    public AirlinePrefix: string;
    public DirectionId: string;
    public TransportModeId: string;
    public ShipmentLevelCode: string;
    public IsCancelled: boolean;
}
export class ImporterQueriesDataCounts {
    public OpenShipmentsCount: number;
    public MissingDocsCount: number;
    public RecentCount: number;
    public ArchivedShipmentsCount: number;
    public AllShipmentsCount: number;
    public AgentShipmentsCount: number;
    public ImporterShipmentsCount: number;
    public RequestedDocsCount: number;
    public RequiredActionsCount: number;
}
export class ShipmentConnectedEntity {
    public EntityId: string;
    public EntityType: string;
    public Reference: string;
    public ObjectTableName: string;
    public ExpectedDate: Date;
    public ActualDate: Date;
    public EntityStatus: string;
    public OpenDate: Date;
    public AcceptedDate: Date;
    public Salesman: string;    
}

export class ExcelPackageFilter {
    Tenant: number;
    FileData: string;
    ShipmentId: string;
}
export class ExcelPackage {
    ContainerTypeId: string;
    ContainerTypeCode: string;
    ContainerTypeName: string;
    ContainerNumber: string;
    Volume: number;
    GrossWeight: number;
    Step2Price: number;
    Tare: number;
    ShipperSeal: string;
    CarrierSeal: string;
    MarksAndNumbers: string;
    Description: string;
    IsRefrigerated: boolean;
    HasErrors: boolean;
}
