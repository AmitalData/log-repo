import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {AppTool} from '../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ARPaymentPM} from '../EntityPMs/ARPaymentPM';
import {ARInvoicePM} from '../EntityPMs/ARInvoicePM';
import {ARInvoiceLinePM} from '../EntityPMs/ARInvoiceLinePM';
import {ARInvoiceEntityPM} from '../EntityPMs/ARInvoiceEntityPM';
import {ARInvoicePaymentPM} from '../EntityPMs/ARInvoicePaymentPM';
import {ARInvoiceTransferHistoryPM} from '../EntityPMs/ARInvoiceTransferHistoryPM';
import {ConstituentPM} from '../EntityPMs/ConstituentPM';
import {ARPaymentInvoicePM} from '../EntityPMs/ARPaymentInvoicePM';
import {APInvoiceMultipleShortPM} from '../EntityPMs/APInvoiceMultipleShortPM';
import {APInvoiceLinePM} from '../EntityPMs/APInvoiceLinePM';
import {Guid} from '../../Infrastructure/Utilities/Guid';

@Injectable()

export class InvoiceDomainService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
    }

    GetCardCurrenciesAccountingByCurrencyAndId(cardId: string, currencyId: string, isPayable: boolean) {
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCardCurrenciesAccountingByCurrencyAndId?cardId=' + cardId + '&currencyId=' + currencyId + '&isPayable=' + isPayable, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var cardCurrenciesAccounting = response;
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = cardCurrenciesAccounting;
                return myResponse;
            }));
        });
    }
    GetAccountingReceivablesSummary() {

        var url = this._apiUrl + '/GetAccountingReceivablesSummary';
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
                // map operator inside pipe
                map(response => {
                    var myJsonResult = response;
                    var myResult = new AccountReceivablesSummary();
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

    GetAccountPayablesSummary() {


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAccountPayablesSummary', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }));
        });
    }
    GetAccountingTransferSummary() {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAccountingTransferSummary', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }));
        });
    }
    GetMoneyStatusForTenant(months: number, days: number, tenant: number, index: number, currency: number) {


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetMoneyStatusForTenant?months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }));
        });
    }
    GetDebrotExposure(tenant: number, currency: number) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDebrotExposure?tenant=' + tenant + '&currency=' + currency, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }));
        });


    }
    GetDebrotExposureForGridControl(index: number) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDebrotExposureForGridControl?index=' + index, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;

                return myResponse;
            }));
        });

    }
    GetCreditorExposure(index: number) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCreditorExposure?index=' + index, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;

                return myResponse;
            }));
        });
    }
    GetAgingReportARInvioceData(index: number, customerId: any) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAgingReportARInvioceData?index=' + index + '&customerId=' + customerId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;

                return myResponse;
            }));
        });
    }
    GetAgingReportAPInvioceData(index: number) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAgingReportAPInvioceData?index=' + index, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;

                return myResponse;
            }));
        });
    }
    ValidateARPaymentFullAccounting(paymentMethod: string, currency: string, billTo: string, code: string, registergdate: Date, bankAccountId: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetARPaymentValidatingList?paymentMethod=' + paymentMethod + "&currency=" + currency + "&billTo=" + billTo + "&code=" + code + "&registergDateString=" + ServiceHelper.GetDateString(registergdate) + "&bankAccountId=" + bankAccountId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = result;

                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    ValidateAPInvoiceFullAccounting(currency: string, vendor: string, accountingdate: Date) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAPInvoiceValidatingList?currency=' + currency + "&vendor=" + vendor + "&accountingDateString=" + ServiceHelper.GetDateString(accountingdate), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    ValidateInvoiceDate(invoiceDate: Date) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetValidateInvoiceDate?invoiceDateString=' + ServiceHelper.GetDateString(invoiceDate), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });


    }
    ValidateInvoiceNumber(invoiceNumber: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetValidateInvoiceNumber?invoiceNumber=' + invoiceNumber, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });


    }

    PostARPaymentChequeAndCashBook(entityPM: ARPaymentPM) {
        return defer(() => {

            var mappedEntity: ARPaymentPM;
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            mappedEntity = this.MapARPaymentJsonToEntityPM(entityPM, false);

            return this._http.post(this._apiUrl + '/PostARPaymentChequeAndCashBook', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;
                var mappedResult: ARPaymentPM = this.MapARPaymentJsonToEntityPM(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    PostARInvoiceJournalAndJournalLines(entityPM: ARInvoicePM) {
        return defer(() => {

            var mappedEntity: ARInvoicePM;
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            mappedEntity = this.MapARInvoiceJsonToEntityPM(entityPM, false);

            return this._http.post(this._apiUrl + '/PostARInvoiceJournalAndJournalLines', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;
                var mappedResult: ARInvoicePM = this.MapARInvoiceJsonToEntityPM(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    ValidateARInvoiceFullAccounting(currency: string, billTo: string, accountingdate: Date) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetARInvoiceValidatingList?currency=' + currency + "&billTo=" + billTo + "&accountingDateString=" + ServiceHelper.GetDateString(accountingdate), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = result;

                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    IsARInvoiceNumberExists(InvoiceNumber: string) {

        var url = this._apiUrl + '/GetIsARInvoiceNumberExists?InvoiceNumber=' + InvoiceNumber;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var isExists: any = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = isExists;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    AutoCreditARInvoice(entityId: string, IsInvoiceNumberManuallySet: boolean, AutoCreditManualNumber: string, AutoCreditDate: Date) {


        var url = this._apiUrl + '/GetAutoCreditARInvoice?entityId=' + entityId + "&IsInvoiceNumberManuallySet=" + IsInvoiceNumberManuallySet + "&AutoCreditManualNumber=" + AutoCreditManualNumber + "&AutoCreditDateString=" + ServiceHelper.GetDateString(AutoCreditDate);

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var newInvoiceId: any = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = newInvoiceId;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    CheckVendor_NumberDuplication(vendorId: string, invoiceNumber: string, entityId: string) {

        var args = new APInvoiceNumberDuplicationCheckArgs();
        args.VendorId = vendorId;
        args.EntityId = entityId;
        args.InvoiceNumber = invoiceNumber;

        return defer(() => {

            return this._http.post(this._apiUrl, JSON.stringify(args), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {

                var newInvoiceId: any = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = newInvoiceId;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    CheckARPaymentCashBook(paymentMethod: string, currency: string, branch: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetARPaymentCashBook?paymentMethod=' + paymentMethod + "&currency=" + currency + "&branch=" + branch, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = result;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCustomerCreditLimitActualAmount(myCustomerId: string, invoiceId: string = null) {

        var url = this._apiUrl + '/GetCustomerCreditLimitActualAmount?myCustomerId=' + myCustomerId + "&invoiceId=" + invoiceId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSingleAPInvoiceShortPM(myInvoiceId: string, myShipmentId: string) {

        var url = this._apiUrl + '/GetSingleAPInvoiceShortPM?myInvoiceId=' + myInvoiceId + "&myShipmentId=" + myShipmentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;

                var entity: APInvoiceMultipleShortPM;
                if (pm) {
                    entity = this.MapJsonToAPInvoiceMultipleShortPM(pm);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    PutSingleAPInvoiceShortPM(entityPM: APInvoiceMultipleShortPM) {
        return defer(() => {

            var serviceResponse: ServiceResponse = new ServiceResponse();

            var mappedEntity: APInvoiceMultipleShortPM;
            mappedEntity = this.MapJsonToAPInvoiceMultipleShortPM(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var pm = res;
                    if (pm) {
                        var mappedResult: APInvoiceMultipleShortPM;
                        mappedResult = this.MapJsonToAPInvoiceMultipleShortPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }

                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    RebuildTransferFile(entityId: string) {

        var url = this._apiUrl + '/GetRebuildTransferFile?entityId=' + entityId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetNotReadyARInvoicesIds() {

        var url = this._apiUrl + '/GetNotReadyARInvoicesIds';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetNotReadyAPInvoicesIds() {

        var url = this._apiUrl + '/GetNotReadyAPInvoicesIds';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetNotReadyARPaymentsIds() {

        var url = this._apiUrl + '/GetNotReadyARPaymentIds';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetNotReadyAPPaymentsIds() {

        var url = this._apiUrl + '/GetNotReadyAPPaymentIds';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = allLists;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetRecalculateTransfer(ids: string[], entityCode: string) {

        var url = this._apiUrl + '/GetRecalculateTransfer?allIdsString=' + AppTool.GetIdsArrayText(ids) + "&entityCode=" + entityCode;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    BlockTransferEntities(ids: string[], entityCode: string) {

        var url = this._apiUrl + '/GetBlockForTransfer?allIdsString=' + AppTool.GetIdsArrayText(ids) + "&entityCode=" + entityCode;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    SetAccountingSettingStartDate(entityCode: string, myStartDate: Date) {

        var myStartDateString: string = ServiceHelper.GetDateString(myStartDate);
        var url = this._apiUrl + '/GetSetAccountingSettingStartDate?entityCode=' + entityCode + "&myStartDateString=" + myStartDateString;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    SendARPaymentSATXML(id: string) {

        var url = this._apiUrl + '/GetSendARPaymentSATXML?paymentId=' + id

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetARInvoiceSATStatus(id: string) {

        var url = this._apiUrl + '/GetARInvoiceSATStatus?paymentId=' + id

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetOnStartDateEntitiesIds(entityCode: string, myStartDate: Date) {

        var myStartDateString: string = ServiceHelper.GetDateString(myStartDate);
        var url = this._apiUrl + '/GetOnStartDateEntitiesIds?entityCode=' + entityCode + "&myStartDateString=" + myStartDateString;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSingleChargeTypeAccountingList(chargesTypeId: string, vatTypeId: string) {

        var url = this._apiUrl + '/GetSingleChargeTypeAccountingList?chargesTypeId=' + chargesTypeId + "&vatTypeId=" + vatTypeId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    PrintTaxData(startDate: Date, endDate: Date, email: string) {

        var startDateString: string = ServiceHelper.GetDateString(startDate);
        var endDateString: string = ServiceHelper.GetDateString(endDate);
        var url = this._apiUrl + '/GetTaxApprovalData?startDateString=' + startDateString + "&endDateString=" + endDateString + "&email=" + email;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetShipmentLevelCode(myShipmentId: string) {

        var url = this._apiUrl + '/GetShipmentLevelCode?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult: any = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetShipmentIsAccountingClosed(myShipmentId: string) {

        var url = this._apiUrl + '/GetShipmentIsAccountingClosed?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult: any = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetARPaymentSATCancellationStatus(paymentId: string) {

        var url = this._apiUrl + '/GetARPaymentSATCancellationStatus?paymentId=' + paymentId ;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetStatusOfARPaymentCheques(paymentId: string) {
 
        var url = this._apiUrl + '/GetStatusOfARPaymentCheques?paymentId=' + paymentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetARInvoiceSATCancellationStatus(invoiceId: string) {

        var url = this._apiUrl + '/GetARInvoiceSATCancellationStatus?invoiceId=' + invoiceId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    getConnectedARPayments(invoiceId: string) {

        var url = this._apiUrl + '/getConnectedARPayments?invoiceId=' + invoiceId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    getConnectedAPPayments(invoiceId: string) {

        var url = this._apiUrl + '/getConnectedAPPayments?invoiceId=' + invoiceId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetListOfARInvoiceStockPM() {

        var url = this._apiUrl + '/GetListOfARInvoiceStockPM?';
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }    

    MarkEntityAsBlocked(transferTypeCode: string, entityId: string) {

        var url = this._apiUrl + '/GetMarkEntityAsBlocked?transferTypeCode=' + transferTypeCode + "&entityId=" + entityId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetConnectedAPInvoicestoPayments(paymentId: string) {
        var url = this._apiUrl + '/GetConnectedAPInvoicestoPayments?paymentId=' + paymentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetShipmentsForMultipleAPInvoice(invoiceId: string, vendorId: string) {
        var url = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain' + '/GetShipmentsForMultipleAPInvoice?invoiceId=' + invoiceId + "&vendorId=" + vendorId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetInvoiceOpenAmountPayables(shipmentId: string) {
        var url = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain' + '/GetInvoiceOpenAmountPayables?entityId=' + shipmentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapARPaymentJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ARPaymentPM = null) {
        if (!entityPM) {
            entityPM = new ARPaymentPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldPaymentInvoices: ARPaymentInvoicePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldPaymentInvoices = entityPM.OldEntityPM.PaymentInvoices;
        }


        entityPM.PaymentInvoices = new Array<ARPaymentInvoicePM>();
        for (var item in jsonPM.PaymentInvoices) {

            var jItem = jsonPM.PaymentInvoices[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARPaymentInvoicePM: ARPaymentInvoicePM;
            if (mapParent) {
                newARPaymentInvoicePM = new ARPaymentInvoicePM(entityPM);
            }
            else {
                newARPaymentInvoicePM = new ARPaymentInvoicePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARPaymentInvoicePM[pmProperty] = jItem[pmProperty];
            }
            newARPaymentInvoicePM.IsDirty = false;
            if (mapParent) {
                newARPaymentInvoicePM.OldEntityPM = this.clone(newARPaymentInvoicePM);
                newARPaymentInvoicePM.UniqueKey = Guid.newGuid();
                newARPaymentInvoicePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newARPaymentInvoicePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newARPaymentInvoicePM.ChangeSetOp = "Update";
                }
                else {
                    newARPaymentInvoicePM.ChangeSetOp = "Insert";
                }

                newARPaymentInvoicePM.OldEntityPM = null;
                newARPaymentInvoicePM.EntityParentPM = null;
            }


            entityPM.PaymentInvoices.push(newARPaymentInvoicePM);
        }

        if (oldPaymentInvoices) {

            for (var itemKey in oldPaymentInvoices) {
                if (entityPM.PaymentInvoices.filter(p => p.UniqueKey === oldPaymentInvoices[itemKey].UniqueKey).length === 0) {

                    if (oldPaymentInvoices[itemKey]) {
                        oldPaymentInvoices[itemKey].ChangeSetOp = "Delete";
                        entityPM.PaymentInvoices.push(oldPaymentInvoices[itemKey]);
                    }
                }
            }
        }

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.PaymentInvoices = [];
            for (var m in entityPM.PaymentInvoices) {
                entityPM.OldEntityPM.PaymentInvoices.push(this.clone(entityPM.PaymentInvoices[m]));
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }
    MapARInvoiceJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ARInvoicePM = null) {


        if (!entityPM) {

            entityPM = new ARInvoicePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldInvoiceLines: ARInvoiceLinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoiceLines = entityPM.OldEntityPM.InvoiceLines;
        }


        entityPM.InvoiceLines = new Array<ARInvoiceLinePM>();
        for (var item in jsonPM.InvoiceLines) {

            var jItem = jsonPM.InvoiceLines[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoiceLinePM: ARInvoiceLinePM;
            if (mapParent) {
                newARInvoiceLinePM = new ARInvoiceLinePM(entityPM);
            }
            else {
                newARInvoiceLinePM = new ARInvoiceLinePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoiceLinePM[pmProperty] = jItem[pmProperty];
            }
            newARInvoiceLinePM.IsDirty = false;
            if (mapParent) {
                newARInvoiceLinePM.OldEntityPM = this.clone(newARInvoiceLinePM);
                newARInvoiceLinePM.UniqueKey = Guid.newGuid();
                newARInvoiceLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newARInvoiceLinePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newARInvoiceLinePM.ChangeSetOp = "Update";
                }
                else {
                    newARInvoiceLinePM.ChangeSetOp = "Insert";
                }

                newARInvoiceLinePM.OldEntityPM = null;
                newARInvoiceLinePM.EntityParentPM = null;
            }


            entityPM.InvoiceLines.push(newARInvoiceLinePM);
        }

        if (oldInvoiceLines) {

            for (var itemKey in oldInvoiceLines) {
                if (entityPM.InvoiceLines.filter(p => p.UniqueKey === oldInvoiceLines[itemKey].UniqueKey).length === 0) {

                    if (oldInvoiceLines[itemKey]) {
                        oldInvoiceLines[itemKey].ChangeSetOp = "Delete";
                        entityPM.InvoiceLines.push(oldInvoiceLines[itemKey]);
                    }
                }
            }
        }

        var oldInvoiceEntities: ARInvoiceEntityPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoiceEntities = entityPM.OldEntityPM.InvoiceEntities;
        }


        entityPM.InvoiceEntities = new Array<ARInvoiceEntityPM>();
        for (var item in jsonPM.InvoiceEntities) {

            var jItem = jsonPM.InvoiceEntities[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoiceEntityPM: ARInvoiceEntityPM;
            if (mapParent) {
                newARInvoiceEntityPM = new ARInvoiceEntityPM();
            }
            else {
                newARInvoiceEntityPM = new ARInvoiceEntityPM();
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoiceEntityPM[pmProperty] = jItem[pmProperty];
            }
            newARInvoiceEntityPM.IsDirty = false;
            if (mapParent) {
                newARInvoiceEntityPM.OldEntityPM = this.clone(newARInvoiceEntityPM);
                newARInvoiceEntityPM.UniqueKey = Guid.newGuid();
                newARInvoiceEntityPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newARInvoiceEntityPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newARInvoiceEntityPM.ChangeSetOp = "Update";
                }
                else {
                    newARInvoiceEntityPM.ChangeSetOp = "Insert";
                }

                newARInvoiceEntityPM.OldEntityPM = null;
                newARInvoiceEntityPM.EntityParentPM = null;
            }


            entityPM.InvoiceEntities.push(newARInvoiceEntityPM);
        }

        if (oldInvoiceEntities) {

            for (var itemKey in oldInvoiceEntities) {
                if (entityPM.InvoiceEntities.filter(p => p.UniqueKey === oldInvoiceEntities[itemKey].UniqueKey).length === 0) {

                    if (oldInvoiceEntities[itemKey]) {
                        oldInvoiceEntities[itemKey].ChangeSetOp = "Delete";
                        entityPM.InvoiceEntities.push(oldInvoiceEntities[itemKey]);
                    }
                }
            }
        }

        var oldInvoicePayments: ARInvoicePaymentPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoicePayments = entityPM.OldEntityPM.InvoicePayments;
        }


        entityPM.InvoicePayments = new Array<ARInvoicePaymentPM>();
        for (var item in jsonPM.InvoicePayments) {

            var jItem = jsonPM.InvoicePayments[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoicePaymentPM: ARInvoicePaymentPM;
            if (mapParent) {
                newARInvoicePaymentPM = new ARInvoicePaymentPM(entityPM);
            }
            else {
                newARInvoicePaymentPM = new ARInvoicePaymentPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoicePaymentPM[pmProperty] = jItem[pmProperty];
            }
            newARInvoicePaymentPM.IsDirty = false;
            if (mapParent) {
                newARInvoicePaymentPM.OldEntityPM = this.clone(newARInvoicePaymentPM);
                newARInvoicePaymentPM.UniqueKey = Guid.newGuid();
                newARInvoicePaymentPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newARInvoicePaymentPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newARInvoicePaymentPM.ChangeSetOp = "Update";
                }
                else {
                    newARInvoicePaymentPM.ChangeSetOp = "Insert";
                }

                newARInvoicePaymentPM.OldEntityPM = null;
                newARInvoicePaymentPM.EntityParentPM = null;
            }


            entityPM.InvoicePayments.push(newARInvoicePaymentPM);
        }

        if (oldInvoicePayments) {

            for (var itemKey in oldInvoicePayments) {
                if (entityPM.InvoicePayments.filter(p => p.UniqueKey === oldInvoicePayments[itemKey].UniqueKey).length === 0) {

                    if (oldInvoicePayments[itemKey]) {
                        oldInvoicePayments[itemKey].ChangeSetOp = "Delete";
                        entityPM.InvoicePayments.push(oldInvoicePayments[itemKey]);
                    }
                }
            }
        }



        entityPM.InvoiceTransfers = new Array<ARInvoiceTransferHistoryPM>();
        for (var item in jsonPM.InvoiceTransfers) {

            var jItem = jsonPM.InvoiceTransfers[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newARInvoiceTransferHistoryPM: ARInvoiceTransferHistoryPM;
            newARInvoiceTransferHistoryPM = new ARInvoiceTransferHistoryPM();

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newARInvoiceTransferHistoryPM[pmProperty] = jItem[pmProperty];
            }
            newARInvoiceTransferHistoryPM.IsDirty = false;


            entityPM.InvoiceTransfers.push(newARInvoiceTransferHistoryPM);
        }

        var oldConstituentInvoices: ConstituentPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldConstituentInvoices = entityPM.OldEntityPM.ConstituentInvoices;
        }


        entityPM.ConstituentInvoices = new Array<ConstituentPM>();
        for (var item in jsonPM.ConstituentInvoices) {

            var jItem = jsonPM.ConstituentInvoices[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newConstituentPM: ConstituentPM;
            if (mapParent) {
                newConstituentPM = new ConstituentPM(entityPM);
            }
            else {
                newConstituentPM = new ConstituentPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newConstituentPM[pmProperty] = jItem[pmProperty];
            }
            newConstituentPM.IsDirty = false;
            if (mapParent) {
                newConstituentPM.OldEntityPM = this.clone(newConstituentPM);
                newConstituentPM.UniqueKey = Guid.newGuid();
                newConstituentPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newConstituentPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newConstituentPM.ChangeSetOp = "Update";
                }
                else {
                    newConstituentPM.ChangeSetOp = "Insert";
                }

                newConstituentPM.OldEntityPM = null;
                newConstituentPM.EntityParentPM = null;
            }


            entityPM.ConstituentInvoices.push(newConstituentPM);
        }

        if (oldConstituentInvoices) {

            for (var itemKey in oldConstituentInvoices) {
                if (entityPM.ConstituentInvoices.filter(p => p.UniqueKey === oldConstituentInvoices[itemKey].UniqueKey).length === 0) {

                    if (oldConstituentInvoices[itemKey]) {
                        oldConstituentInvoices[itemKey].ChangeSetOp = "Delete";
                        entityPM.ConstituentInvoices.push(oldConstituentInvoices[itemKey]);
                    }
                }
            }
        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.InvoiceLines = [];
            for (var m in entityPM.InvoiceLines) {
                entityPM.OldEntityPM.InvoiceLines.push(this.clone(entityPM.InvoiceLines[m]));
            }
            entityPM.OldEntityPM.InvoiceEntities = [];
            for (var m in entityPM.InvoiceEntities) {
                entityPM.OldEntityPM.InvoiceEntities.push(this.clone(entityPM.InvoiceEntities[m]));
            }
            entityPM.OldEntityPM.InvoicePayments = [];
            for (var m in entityPM.InvoicePayments) {
                entityPM.OldEntityPM.InvoicePayments.push(this.clone(entityPM.InvoicePayments[m]));
            }
            entityPM.OldEntityPM.InvoiceTransfers = [];
            for (var m in entityPM.InvoiceTransfers) {
                entityPM.OldEntityPM.InvoiceTransfers.push(this.clone(entityPM.InvoiceTransfers[m]));
            }
            entityPM.OldEntityPM.ConstituentInvoices = [];
            for (var m in entityPM.ConstituentInvoices) {
                entityPM.OldEntityPM.ConstituentInvoices.push(this.clone(entityPM.ConstituentInvoices[m]));
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }
    MapJsonToAPInvoiceMultipleShortPM(jsonPM: any, mapParent: boolean = true, entityPM: APInvoiceMultipleShortPM = null) {
        if (!entityPM) {
            entityPM = new APInvoiceMultipleShortPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        this.MapShortAPInvoiceLines(entityPM, jsonPM, mapParent);

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.InvoiceLines = [];

            for (var item in entityPM.InvoiceLines) {
                var myInvoiceLinePM = entityPM.InvoiceLines[item];
                var newInvoiceLinePM: APInvoiceLinePM = this.clone(myInvoiceLinePM);
                entityPM.OldEntityPM.InvoiceLines.push(newInvoiceLinePM);
            }
        }

        else {
            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }
    MapShortAPInvoiceLines(entityPM: APInvoiceMultipleShortPM, jsonPM: any, mapParent: boolean = true) {

        var oldInvoiceLines: APInvoiceLinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInvoiceLines = entityPM.OldEntityPM.InvoiceLines;
        }

        entityPM.InvoiceLines = new Array<APInvoiceLinePM>();
        for (var item in jsonPM.InvoiceLines) {
            var jItem = jsonPM.InvoiceLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newAPInvoiceLinePM: APInvoiceLinePM;

            if (mapParent) {
                newAPInvoiceLinePM = new APInvoiceLinePM(entityPM);
            }
            else {
                newAPInvoiceLinePM = new APInvoiceLinePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newAPInvoiceLinePM[pmProperty] = jItem[pmProperty];
            }
            newAPInvoiceLinePM.IsDirty = false;

            if (mapParent) {
                newAPInvoiceLinePM.UniqueKey = Guid.newGuid();
                newAPInvoiceLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newAPInvoiceLinePM.OldEntityPM = this.clone(newAPInvoiceLinePM);


            }
            else {
                if (newAPInvoiceLinePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newAPInvoiceLinePM.ChangeSetOp = "Update";
                }
                else {
                    newAPInvoiceLinePM.ChangeSetOp = "Insert";
                }

                newAPInvoiceLinePM.OldEntityPM = null;
                newAPInvoiceLinePM.EntityParentPM = null;
            }


            entityPM.InvoiceLines.push(newAPInvoiceLinePM);
        }
        if (oldInvoiceLines) {

            for (var itemKey in oldInvoiceLines) {
                if (entityPM.InvoiceLines.filter(p => p.UniqueKey === oldInvoiceLines[itemKey].UniqueKey).length === 0) {

                    if (oldInvoiceLines[itemKey]) {
                        //oldInvoiceLines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.InvoiceLines.push(oldInvoiceLines[itemKey]);
                        var oldItemJson = oldInvoiceLines[itemKey];
                        var deletedPM: APInvoiceLinePM = new APInvoiceLinePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.InvoiceLines.push(deletedPM);
                    }
                }
            }
        }
    }
    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }
}

export class DebtorsClass {
    private static counter: number = 0;
    public DebtorsClass() {
        this.linePrimary = ++DebtorsClass.counter;
    }
    public linePrimary: number;
    public DebtorName: string;
    public Amount: number;
    public AmountLabel: string;
    public Outstanding: number;
    public Overdue: number;
    public DebtorId: string;
    public DebtorType: string;
}
export class CreditorsClass {
    private static counter: number = 0;
    public CreditorsClass() {
        this.linePrimary = ++CreditorsClass.counter;
    }

    public linePrimary: number;
    public CreditorName: string;
    public Amount: number;
    public Outstanding: number;
    public Overdue: number;
    public CreditorId: string;
    public CreditorType: string;
}
export class ARInvoiceSATStatus {

    public ARInvoiceId: string;
    public ARInvoiceNumber: string;
    public IsSATValid: boolean;
    public BillTo: string;
    public FNAReason: string;
    public SATStatusCode: string;
    public SATStatusName: string;
    public SATError: string;
}
class APInvoiceNumberDuplicationCheckArgs {
    public VendorId: string;
    public EntityId: string;
    public InvoiceNumber: string;
}
export class AccountReceivablesSummary {
    public  ARInvoicesDraftsCount: number;
    public  ARInvoicesUnpaidCount: number;
    public  ARInvoicesOpenConstituentCount: number;
    public  ARPaymentsDraftsCount: number;
    public  ARPaymentsOpenedCount: number;
    public  ARGeneralInvoiceDraftCount: number;
    public  ARPaymentsSATFailedCount: number;
    public  ARInvoicesSATFailedCount: number;
    public  ARInvoicesFailedCount: number;
    public  ARPaymentFailedCount: number;
}
