import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { defer } from "cypress/types/lodash";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { SessionInfo } from "Infrastructure/Utilities/SessionInfo";
import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";

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

    getDeclarationsforBulkFeed(goodsDescription: string, weightFrom: string, weightTo: string, incotermCode: string, searchFilter: string, totalInvoice: string, fastIndividualProcess: string, skip: number = null, take: number =  null): Promise<DeclarationsforBulkFeed[]> {
        const ajax: Observable<any> = this._http.get(
            this._apiUrl + "/DeclarationsforBulkFeed",
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
                params: {
                    goodsDescription: goodsDescription,
                    weightFrom: weightFrom,
                    weightTo: weightTo,
                    incotermCode: incotermCode,
                    searchFilter: searchFilter,
                    totalInvoice: totalInvoice,
                    fastIndividualProcess: fastIndividualProcess,
                    skip: '' + skip,
                    take: '' + take,
                }
            }
        );

        return this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax);
    }

}


export interface DeclarationsforBulkFeed {
    $id: string;
    Cargodescription: string;
    Casualimporteraddress1: string;
    Casualimporteraddress2?: any;
    Casualimportercity: string;
    Code?: any;
    Id: string;
    Importername: string;
    IncotermCode: string;
    PackageMeasureQualifierCode: number;
    Totalinvoiceamountinus: number;
}