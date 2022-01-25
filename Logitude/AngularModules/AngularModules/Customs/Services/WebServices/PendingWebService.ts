import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";
import { Observable } from "rxjs";

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


    postBulkFeeding(listPending: string[], listPendingRemark: string[], declarationIdsList: string[], courierMasterId: string, checkboxAll: boolean) {
        const ajax: Observable<any> = this._http.post(
            this._apiUrl + "/BulkFeeding",
            {
                listPending: listPending,
                listPendingRemark: listPendingRemark,
                declarationIdsList: declarationIdsList,
                courierMasterId: courierMasterId,
                checkboxAll: checkboxAll
            },
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
            }
        );

        return this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax) as Promise<any>;
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
