import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";
import { defer, Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { ServiceResponse } from "../../../Infrastructure/DataContracts/ServiceResponse";
import { SessionInfo } from "../../../Infrastructure/Utilities/SessionInfo";
import { SendMultiUpdateRequestParams } from "../../DataContract/RequestParams/SendMultiUpdateRequestParams";

@Injectable()
export class PendingWebService {
    private _http: HttpClient;
    private _apiUrl: string;


    constructor(
        private logtuideTableDataService: LogtuideTableDataService,
    ) {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PendingWebService';
    }

    getDeclarationsforBulkFeed(courierMasterId: string, goodsDescription: string, weightFrom: string, weightTo: string, incotermCode: string, searchFilter: string, totalInvoice: string, fastIndividualProcess: string, skip: number = null, take: number = null, sortingCol: string = '', sortingDir: string = '') {
        const ajax: Observable<any> = this._http.get(
            this._apiUrl + "/DeclarationsforBulkFeed",
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
                params: {
                    courierMasterId: courierMasterId,
                    goodsDescription: goodsDescription,
                    weightFrom: weightFrom,
                    weightTo: weightTo,
                    incotermCode: incotermCode,
                    searchFilter: searchFilter,
                    totalInvoice: totalInvoice,
                    fastIndividualProcess: fastIndividualProcess,
                    skip: '' + skip,
                    take: '' + take,
                    sortingCol: sortingCol,
                    sortingDir: sortingDir,
                }
            }
        );

        // return this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax);
        return this.logtuideTableDataService.standartSendAjax(ajax);
    }


    postBulkFeeding(
        listPending: string[],
        listPendingRemark: string[],
        declarationIdsList: string[],
        courierMasterId: string,
        checkboxAll: boolean,
        allWithoutdeclarationIdsList: string[],
        customFilter: ApiQueryFilters,
        isCreateInvoiceDocument: boolean,
        IsWorkSheetFromExcel:boolean=false) {

        const ajax: Observable<any> = this._http.post(
            this._apiUrl + "/BulkFeeding?" + this.logtuideTableDataService.apiQueryFilterToQueryString(customFilter),
            {
                listPending: listPending,
                listPendingRemark: listPendingRemark,
                declarationIdsList: declarationIdsList,
                allWithoutdeclarationIdsList: allWithoutdeclarationIdsList,
                courierMasterId: courierMasterId,
                checkboxAll: !!checkboxAll,
                isCreateInvoiceDocument: !!isCreateInvoiceDocument,
                IsWorkSheetFromExcel:IsWorkSheetFromExcel,
            },
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
            }
        );

        return this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax) as Promise<any>;
    }

    PostSendMultiUpdate(
        requestParams: SendMultiUpdateRequestParams,
        customFilter: ApiQueryFilters) {

        //const ajax: Observable<any> = this._http.post(
        //    this._apiUrl + "/PostSendMultiUpdate?" + this.logtuideTableDataService.apiQueryFilterToQueryString(customFilter),
        //    {
        //        requestParamsData: requestParams
        //    },
        //    {
        //        headers: ServiceHelper.GetHttpHeaders().headers,
        //    }
        //);

        //return this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax) as Promise<any>;

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + "/PostSendMultiUpdate?" + this.logtuideTableDataService.apiQueryFilterToQueryString(customFilter),
                JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;

                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }
}


export interface DeclarationsforBulkFeed {
    $id: string;
    Cargodescription: string;
    Casualimporteraddress: string;
    Casualimportercity: string;
    Code?: any;
    CourierHAWB: string;
    DeclarationId: string;
    Importername: string;
    IncotermCode: string;
    PackageMeasureQualifierCode: number;
    Totalinvoiceamountinus: number;
}
