import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { LogtuideTableDataService } from "Infrastructure/Services/logtuide-table-data.service";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";
import { Observable } from "rxjs";

@Injectable()

export class AzureSearchWebService {
    private _http: HttpClient = ServiceHelper.HttpClient;
    private _apiUrl: string = ServiceHelper.GetLogitudeURL() + 'api/AzureSearch';
    private logtuideTableDataService: LogtuideTableDataService = LogtuideTableDataService.createInstance();
    
    constructor() {}

    fastSearch(filters: ApiQueryFilters, searchText: string, index: string): Promise<FastSearchResult[]> {
        const ajax: Observable<any> = this._http.get(
            this._apiUrl + '/GetFastSearch?' + this.logtuideTableDataService.apiQueryFilterToQueryString(filters),
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
                params: { searchText, index }
            }
        );

        const res: Observable<ServiceResponse> = this.logtuideTableDataService.standartSendAjax(ajax);
        return this.logtuideTableDataService.getDataFromService(res)
    }

    GetSettings(index: string): Promise<FastSearchSettings> {
        const ajax: Observable<any> = this._http.get(
            this._apiUrl + '/GetSettings',
            {
                headers: ServiceHelper.GetHttpHeaders().headers,
                params: { index }
            }
        );

        const res: Observable<ServiceResponse> = this.logtuideTableDataService.standartSendAjax(ajax);
        return this.logtuideTableDataService.getDataFromService(res)
    }
}


export interface FastSearchResult {
    $id: string;
    id: string;
    customFileNo: string;
    tenant: number;
    customerId: string;
    importerId: string | null;
    searchFields: string;
    declarationNumber: string | null;
    externalDeclarationNumber: string;
    createDateTime: Date;
    updateDateTime: Date;
    isCancelled: boolean;
    signerPesonalId: string | null;
    casualSupplierName: string | null;
    courierHAWB: string | null;
    courierSearchFields: string;
    amendmentRequestNumber: string | null;
    exportFile: string | null;
    cargoDescription: string;
    exportCloseAmendRequestNumber: string | null;
    direction: string;
    amendmentDontDisplayInList: boolean;
    customerName: string;
    transportModeId: string;
  }

 export interface FastSearchSettings {
    maxResults: number;
    idleSearchTimeMs: number;
    showTopResults: number;
    showRecent: boolean;
    showRecentObject: string;
    ddlHtmlLine: string;
    recentLineHeader: string;
    recentShowTopResults: number;
    recentEditScreen: string;
    recentEditScreenParam: string;
    addAsteriskToNumberSearch: string;
}