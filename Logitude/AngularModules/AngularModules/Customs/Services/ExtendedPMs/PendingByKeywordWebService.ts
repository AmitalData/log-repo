import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
// import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";
import { Observable } from "rxjs";

@Injectable()
export class PendingByKeywordWebService {
    private apiUrl: string = ServiceHelper.GetLogitudeURL() + "api/PendingByKeywordExtended";
    private http: HttpClient = ServiceHelper.HttpClient;
    private header: { headers: HttpHeaders } = ServiceHelper.GetHttpHeaders()


    constructor(
       // private logtuideTableDataService: LogtuideTableDataService,
    ) { }


    delete(id: string): Promise<void> {
        const ajax: Observable<any> = this.http.delete(
            this.apiUrl + '/Delete',
            {
                params: { id: id },
                headers: this.header.headers,
            }
        )

        return null;// this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax);
    }
}