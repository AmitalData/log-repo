import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { LogisticActionRequestRequestParams } from "Customs/DataContract/RequestParams/LogisticActionRequestRequestParams";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";
import { Observable } from "rxjs";


@Injectable()
export class LogisticActionRequestWebService {
    private _http: HttpClient;
    private _apiUrl: string;


    constructor(
        private logtuideTableDataService: LogtuideTableDataService,
    ) {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LogisticActionRequestWebService';
    }


    SendContainerization(genericRequestParams: LogisticActionRequestRequestParams) {
        const ajax: Observable<any> = this._http.post(
            this._apiUrl + "/SendCustomsMessage8410",
            { genericRequestParams: genericRequestParams },
            { headers: ServiceHelper.GetHttpHeaders().headers }
        );

        return this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax);
    }
}
