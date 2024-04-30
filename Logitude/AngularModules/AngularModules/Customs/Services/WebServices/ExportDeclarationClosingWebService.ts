import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { EntityListService } from "Infrastructure/Services/EntityListService";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { LogtuideTableDataService } from "Infrastructure/Services/logtuide-table-data.service";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
// import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";
import { Observable } from "rxjs";


@Injectable()
export class ExportDeclarationClosingWebService {
    private _http: HttpClient;
    private _apiUrl: string;
   private logtuideTableDataService: LogtuideTableDataService = new LogtuideTableDataService(new EntityListService(), new EntityResourceService());


    constructor(
    ) {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExportDeclarationClosingWebService';
    }


    getUnifreightData(exportfile: string): Promise<ExportData> {
        const ajax: Observable<any> = this._http.get(
            this._apiUrl + "/UnifreightData",
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
                params: { exportfile: exportfile }
            }
        );

        return this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax);
    }
}


export type ExportData = {
    loadingSite?: string;
    flightDate?: any;
    MAWB?: any;
    HAWB?: any;
}