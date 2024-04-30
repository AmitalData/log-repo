import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CustomsCollateralPM } from "Customs/EntityPMs/CustomsCollateralPM";
import { CustomsCollateralsAnswerPM } from "Customs/EntityPMs/CustomsCollateralsAnswerPM";
import { LogtuideTableDataService } from "Infrastructure/Services/logtuide-table-data.service";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
//import { LogtuideTableDataService } from "QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service";
import { Observable } from "rxjs";

@Injectable()
export class CustomsCollateralWebService {
    private _http: HttpClient;
    private _apiUrl: string;
    private logtuideTableDataService: LogtuideTableDataService = LogtuideTableDataService.createInstance();

    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsCollateralWebService';
    }


    updateMulti(ids: string[], declarationId: string, selectAll: boolean, customsCollateralsAnswerPM: CustomsCollateralsAnswerPM): Promise<CustomsCollateralPM[]> {
        const ajax: Observable<any> = this._http.put(
            this._apiUrl + "/Multi?",
            {
                ids: ids,
                declarationId: declarationId,
                selectAll: '' + selectAll,
                customsCollateralsAnswerPM: customsCollateralsAnswerPM
            },
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
            }
        );

        return  this.logtuideTableDataService.sendAjaxAndGetDataStandart(ajax) as Promise<CustomsCollateralPM[]>;
    }
}

