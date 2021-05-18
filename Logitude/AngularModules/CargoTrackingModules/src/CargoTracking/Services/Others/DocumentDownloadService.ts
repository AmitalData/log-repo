import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { defer } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
declare var window: any;


@Injectable()
export class DocumentDownloadService {
    private  _apiUrl: string;
    private  token: string;

    constructor(@Inject('BASE_URL') private baseUrl: string, private  _http: HttpClient) {
        
    }

    public  DownloadPage(id: string) {
        var url: string = "id=" + id;
        this.GetCurrenctUserValidity().subscribe((response:any) => {
            this.token = response.Result.DocumentDownloadToken;
            var link = ServiceHelper.GetAppURL(this.baseUrl) + "WebPages/DownloadPage.aspx?" + url + "&tempId=" + this.token;
            var win = window.open(link, '_blank');

            if (win) {
                win.focus();
            }
        });
     
    }

    DownloadAllPages(entityId: string) {
        this.GetCurrenctUserValidity().subscribe((response: any) => {
            this.token = response.Result.DocumentDownloadToken;
            var link = ServiceHelper.GetAppURL(this.baseUrl) + "WebPages/SharedDownloadPage.aspx?id=" + SessionInfo.LoggedUserTenant + ":" + null + ":ship:" + entityId + ":CS:" + this.token;
            var win = window.open(link, '_blank');

            if (win) {
                win.focus();
            }
        });

    }

    private  GetCurrenctUserValidity() {
        this._apiUrl = ServiceHelper.GetAppURL(this.baseUrl) + 'api/LogitudeApplication';

        var url = this._apiUrl + '/GetCurrenctUserValidity?clientEmail=' + SessionInfo.LoggedUserEmail
            + "&documentToken=" + SessionInfo.DocumentDownloadToken +
            '&tenant=' + SessionInfo.LoggedUserTenant;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHeadersWithToken()).pipe(map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                return serviceResponse;
            }), catchError(null))
    });
    }
}

