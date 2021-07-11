import {Injectable} from '@angular/core';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {MessagingStockList} from '../EntityLists/MessagingStockList';
import {ShipmentPM} from '../EntityPMs/ShipmentPM';
import {ShipmentPMService} from './StandardPMs/ShipmentPMService';
import {ShipmentList} from '../EntityLists/ShipmentList';
import { MessagingStockUsageHistoryList } from '../EntityLists/MessagingStockUsageHistoryList';
import { AppTool } from '../../Infrastructure/Tools';
import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { CustomsTransferHeaderPM } from '../EntityPMs/CustomsTransferHeaderPM';
import { CustomsTransferHeaderPMService } from './StandardPMs/CustomsTransferHeaderPMService';
import { ShipmentTool } from '../Tools';
import { BaseService } from '../../Abstractions/Services/BaseService';
import { Observable } from 'rxjs';

@Injectable()

export class ShipmentDomainService extends BaseService  {

    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        super();
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        this.ApiURL = this.BaseURL + 'api/ShipmentDomain';
    }

    GetCustomerCreditLimitDetails(customerId: string, quoteId: string, isBuildFromQuote: boolean) {
        var url = this._apiUrl + '/GetCustomerCreditLimitDetails?customerId=' + customerId + '&quoteId=' + quoteId + '&isBuildFromQuote=' + isBuildFromQuote;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
                map(response => {
                    var serviceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),

                // catchErrro operator inside pipe
                catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetShipmentsCounts(myDirectionId: string, myTransportModeId: string) {

        var url = this._apiUrl + '/GetShipmentsCounts?myDirectionId=' + myDirectionId + '&myTransportModeId=' + myTransportModeId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(

                map(response => {
                    var myJsonResult = response;

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
                }),

                // catchErrro operator inside pipe
                catchError(ServiceHelper.HandleServiceError));
        });
    }

    CheckHousesOpenAmounts(masterId) {

        var url = this._apiUrl + '/CheckHousesOpenAmounts?masterId=' + masterId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetRecentShipments1() {

        var url = this.ApiURL + '/GetRecentShipments';

        return defer(() => {
            return this.HttpClient.get(url, this.HttpHeaders).pipe(
                map(response => {
                    return this.GetServiceResponse(response);
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetRecentShipments() {
        return Observable.create(observer => {
            this.Get(this.ApiURL + '/GetRecentShipments').subscribe((response: any) => {
                return observer.next(this.GetServiceResponse(response));                
            });
        });
    }

    GetDeparturesArrivals(myDirectionId: string, myTransportModeId: string) {

        var url = this._apiUrl + '/GetDeparturesArrivals?myDirectionId=' + myDirectionId + '&myTransportModeId=' + myTransportModeId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var myResult: FlightSummary[] = [];

                for (var key in myJsonResult) {

                    var entity: FlightSummary;
                    entity = this.MapFlightSummary(myJsonResult[key]);
                    myResult.push(entity);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetShipmentCarrierStatuses(entityId: string) {

        var url = this._apiUrl + '/GetShipmentCarrierStatuses?entityId=' + entityId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<ShipmentCarrierStatusList> = [];

                for (var itemJeson in listJason) {

                    var itemMapped: ShipmentCarrierStatusList = this.MapShipmentCarrierStatus(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetLoggedTenantMessagingStockLists() {

        var url = this._apiUrl + '/GetLoggedTenantMessagingStockLists';

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<MessagingStockList> = [];

                for (var itemJeson in listJason) {

                    var itemMapped: MessagingStockList = this.MapMessagingStockList(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }

                return listMapped;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetLoggedTenantMessagingStockUsageHistoryLists(stockId: string) {

        var url = this._apiUrl + '/GetLoggedTenantMessagingStockUsageHistoryLists?stockId=' + stockId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<MessagingStockUsageHistoryList> = [];

                for (var itemJeson in listJason) {

                    var itemMapped: MessagingStockUsageHistoryList = this.MapMessagingStockUsageHistoryList(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }

                return listMapped;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    ValidateShipmentMasterFieldExistance(entityPM: ShipmentPM) {
        return defer(() => {

            var isValidating = ShipmentTool.IsValidatingShipmentMasterFieldExistance(entityPM);

            if (!isValidating) {
                return null;
            }

            else {
                var args = new ValidateShipmentMasterArgs();
                args.ShipmentId = entityPM.Id;
                args.BookingId = entityPM.BookingId;
                args.Master = entityPM.Master;
                args.AirlinePrefix = entityPM.AirlinePrefix;
                args.DirectionId = entityPM.DirectionId;
                args.TransportModeId = entityPM.TransportModeId;
                args.ShipmentLevelCode = entityPM.ShipmentLevelCode;
                args.IsCancelled = entityPM.IsCancelled;
                args.OperationalDate = ShipmentTool.CalculateShipmentOperationalDate(entityPM);

                var mappedEntity: ValidateShipmentMasterArgs = this.MapJsonToValidateShipmentMasterArgs(args, false);

                return this.HttpClient.post(this.ApiURL + "/PostValidateShipmentMasterArgs", JSON.stringify(mappedEntity), this.HttpHeaders).pipe(
                    map((res) => {
                        var myJsonResult = res;
                        return myJsonResult;

                    }), catchError(ServiceHelper.HandleServiceError));
            }
        });
    }


    GetMasterReceivables(entityId: string) {

        var url = this._apiUrl + '/GetMasterReceivables?entityId=' + entityId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
                map(response => {

                    var myJsonResult = response;

                    var serviceResponse = new ServiceResponse();
                    serviceResponse.Result = myJsonResult;
                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetInvoiceOpenAmountReceivables(invoiceTypeCode: string, entityId: string) {

        var url = this._apiUrl + '/GetInvoiceOpenAmountReceivables?invoiceTypeCode=' + invoiceTypeCode + '&entityId=' + entityId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentsQuotesCount() {

        var url = this._apiUrl + '/GetShipmentsQuotesCount?';

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentConsolidationPackages(masterId: string) {

        var url = this._apiUrl + '/GetShipmentConsolidationPackages?masterId=' + masterId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentConnectedEntities(shipmentId: string) {

        var url = this._apiUrl + '/GetShipmentConnectedEntities?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var listMapped: Array<ShipmentConnectedEntity> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: ShipmentConnectedEntity = this.MapShipmentConnectedEntity(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentsQueriesCounts(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) {
        let urlparameters = '/GetShipmentsQueriesCounts?';
        const mykeys = Object.keys(shipmentsQueriesCountsArgs);
        for (var i in mykeys) {
            let propName = mykeys[i];
            let propValue = shipmentsQueriesCountsArgs[propName];

            if (urlparameters != "/GetShipmentsQueriesCounts?") {
                urlparameters = urlparameters.concat('&');
            }

            propValue = encodeURIComponent(propValue);
            urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
        }

        const callUrl = this._apiUrl.concat(urlparameters);

        return defer(() => {
            return this._httpClient.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var myResult = new ImporterQueriesDataCounts();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                return myResult;
            }));
        });
    }
    GetAllMasterHousesPayables(allHousesIdsString: string) {

        var url = this._apiUrl + '/GetAllMasterHousesPayables?allHousesIdsString=' + allHousesIdsString;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetAllMasterHousesReceivables(allHousesIdsString: string) {

        var url = this._apiUrl + '/GetAllMasterHousesReceivables?allHousesIdsString=' + allHousesIdsString;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetConnectedShipmentsByMasterIdAndTenant(masterId: string, tenant: number) {

        var url = this._apiUrl + '/GetConnectedShipmentsByMasterIdAndTenant?masterId=' + masterId + '&currentTenant=' + tenant;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<ShipmentPM> = [];

                var service: ShipmentPMService = new ShipmentPMService();
                for (var itemJeson in listJason) {

                    var itemMapped: ShipmentPM = service.MapJsonToEntityPM(listJason[itemJeson]);

                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;


                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentsCountByQuoteId(quoteId: string) {

        var url = this._apiUrl + '/GetShipmentsCountByQuoteId?quoteId=' + quoteId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentLevelCode(myShipmentId: string) {

        var url = this._apiUrl + '/GetShipmentLevelCode?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult: any = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetInvoiceOpenAmountPayables(entityId: string) {

        var url = this._apiUrl + '/GetInvoiceOpenAmountPayables?entityId=' + entityId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetPayableInvoices(PayableId: string, PayableParentId: string) {

        var url = this._apiUrl + '/GetPayableInvoices?PayableId=' + PayableId + '&PayableParentId=' + PayableParentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentsByQuoteId(quoteId: string) {

        var url = this._apiUrl + '/GetShipmentsByQuoteId?quoteId=' + quoteId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSingleShipmentPMByNumber(shipmentNumber: string) {

        var url = this._apiUrl + '/GetSingleShipmentPMByNumber?shipmentNumber=' + shipmentNumber;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSingleShipmentPMWithoutComposition(id: string) {

        var url = this._apiUrl + '/GetSingleShipmentPMWithoutComposition?id=' + id;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var service: ShipmentPMService = new ShipmentPMService();
                var shipment = service.MapJsonToEntityPM(myResult);
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = shipment;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    BlockNewARInvoice(shipmentId: string) {

        var url = this._apiUrl + '/GetBlockNewARInvoice?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
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

        return defer(() => {
            return this._httpClient.get(urlparameters, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetMessagingStockListForTenantManagmentTab(tenantManagementId: number) {

        var url = this._apiUrl + '/GetMessagingStockListForTenantManagmentTab?tenantManagementId=' + tenantManagementId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetShipmentCustomsTransmissionByShipmnetId(shipmentId: string) {

        var url = this._apiUrl + '/GetShipmentCustomsTransmissionByShipmnetId?shipmentId=' + shipmentId;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    DisconnectQuote(shipmentId: string) {

        var url = this._apiUrl + '/GetDisconnectQuote?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    DisconnectStandaloneShipment(shipmentId: string) {
        var url = this._apiUrl + '/GetDisconnectStandaloneShipment?shipmentId=' + shipmentId;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
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

        var url = this._apiUrl + '/GetSendToAESCustoms?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetArtemusStatus(shipmentNumber: string) {

        var url = this._apiUrl + '/GetArtemusStatus?shipmentNumber=' + shipmentNumber;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
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

        var url = this._apiUrl + '/GetDownloadShipmentPackages?shipmentNumber=' + shipmentNumber + '&shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    CreateMissingMasters() {

        var url = this._apiUrl + '/GetCreateMissingMasterData';

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult: any = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    CheckIfConnectedEntryOrRelease(shipmentId) {

        var url = this._apiUrl + '/GetIfConnectedEntryOrRelease?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    PostUploadExcelFile(filter: ExcelPackageFilter) {

        return defer(() => {
            return this._httpClient.post(this._apiUrl + "/PostUploadExcelFile", JSON.stringify(filter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    CheckIfHouseConnectedToMaster(houseId: string) {

        var url = this._apiUrl + '/GetIfHouseConnectedToMaster?houseId=' + houseId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    SetAMANACStartDate(entityCode: string, myStartDate: Date) {

        var myStartDateString: string = ServiceHelper.GetDateString(myStartDate);
        var url = this._apiUrl + '/GetSetAMANACStartDate?entityCode=' + entityCode + "&myStartDateString=" + myStartDateString;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetOnStartDateEntitiesIds(entityCode: string, myStartDate: Date) {

        var myStartDateString: string = ServiceHelper.GetDateString(myStartDate);
        var url = this._apiUrl + '/GetOnStartDateEntitiesIds?entityCode=' + entityCode + "&myStartDateString=" + myStartDateString;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    BlockTransferEntities(ids: string[], entityCode: string) {

        var url = this._apiUrl + '/GetBlockForTransfer?allIdsString=' + AppTool.GetIdsArrayText(ids) + "&entityCode=" + entityCode;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    SendToAMANAC(shipmentId: string) {

        var url = this._apiUrl + '/GetSendToAMANAC?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MarkShipmentAsBlocked(shipmentId: string) {

        var url = this._apiUrl + '/GetMarkShipmentAsBlocked?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    UnblockedShipment(shipmentId: string) {

        var url = this._apiUrl + '/GetUnblockedShipment?shipmentId=' + shipmentId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetShipmentsTransferSummary() {

        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetShipmentsTransferSummary', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }));
        });
    }

    RebuildTransferFile(entityId: string) {

        var url = this._apiUrl + '/GetRebuildTransferFile?entityId=' + entityId;

        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    ValidateAMANACShipmentsBeforeExporting(entityPM: CustomsTransferHeaderPM) {
        return defer(() => {
            var serviceResponse: ServiceResponse = new ServiceResponse();
            var customsHeaderService: CustomsTransferHeaderPMService = new CustomsTransferHeaderPMService();

            var mappedEntity: CustomsTransferHeaderPM = customsHeaderService.MapJsonToEntityPM(entityPM, false);

            return this._httpClient.post(this._apiUrl + "/PostValidateAMANACShipmentsBeforeExporting", JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {

                        var pm = response.body;
                        if (pm) {
                            var mappedResult: CustomsTransferHeaderPM = customsHeaderService.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }

                        return serviceResponse;
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetNumberOfShipmentPackages(shipmentId: string) {

        var url = this._apiUrl + '/GetNumberOfShipmentPackages?shipmentId=' + shipmentId;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var result = response;
                  
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetIfShipmentPackageConnectedToPickUpDeliveryPackage(containerId: string) {

        var url = this._apiUrl + '/GetIfShipmentPackageConnectedToPickUpDeliveryPackage?containerId=' + containerId;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var result = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetIfShipmentPackagesConnectedToStandAloneShipmentPackage(shipmentId: string) {

        var url = this._apiUrl + '/GetIfShipmentPackagesConnectedToStandAloneShipmentPackage?shipmentId=' + shipmentId;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var result = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetPickupDeliveryValidForInlandDomestic(pickupDeliveryId: string) {
        var url = this._apiUrl + '/GetPickupDeliveryValidForInlandDomestic?pickupDeliveryId=' + pickupDeliveryId;
        return defer(() => {
            return this._httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
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
    public EBookingInProgressCount: number; 
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
    public ActualDateCode: string;
    public ExpectedDateCode: string;
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
    public OperationalDate: Date;
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

export class ShipmentsQueriesCountsArgs {
    Tenant: number;
    TransportModeId: string;
    DirectionId: string;
    SearchFilter: string;
    ServiceContextUser: string;
    TypeCode: string = null;
    ForwarderPartnerId: string;
}
