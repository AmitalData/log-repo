import { HttpClient, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { defer } from 'rxjs';
import { RootContext } from 'src/CargoTracking/Utilities/RootContext';
import { catchError, map } from 'rxjs/operators';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HomeComponent } from 'src/CargoTracking/Components/PublicSite/HomeComponent/HomeComponent';
import { Router } from '@angular/router';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
declare var window: any;



@Injectable()
export class DocumentDownloadService {
    private  _apiUrl: string;
    private  token: string;
    private headers: any;
    public authHeaders = ServiceHelper.GetHeadersWithToken();

    constructor(@Inject('BASE_URL') private baseUrl: string, private  _http: HttpClient, private router: Router) {

    }

    async ExternalDownloadAllDocuments(securityId: string, forwardingShipmentId: string, tenant: number) {

        var mylink = ServiceHelper.GetAppURL(this.baseUrl)
            + `api/CorrespondenceDownload/ValidateAndDownloadDocument?DA=1&securitykey=${securityId}::CS:${tenant}:${forwardingShipmentId ? forwardingShipmentId : ""}:cargo`;
        await this.downloadFile(mylink)
    }




    private async buildHeaders(){
            this.headers = ServiceHelper.GetHeadersWithToken();
            return this.headers;
    }


    private async downloadRequest(link: string): Promise<HttpResponse<Blob>> {

                //ServiceHelper.GetHeadersWithToken()
                return new Promise<any>((resolve, reject) => {
                    
                    this._http.get(link, {
                        ...this.authHeaders,
                        observe: 'response',
                        responseType: 'blob'
                    }).subscribe((res: HttpResponse<Blob>) =>{
                        resolve(res)      
                    })
                })
            
        
    }


    async ExternalDownloadPage(securityId: string, tenant: number, fileName: string ) {
        var mylink = ServiceHelper.GetAppURL(this.baseUrl) + `api/CorrespondenceDownload/ValidateAndDownloadDocument?Id=${securityId}~${tenant}~${null}~${fileName}`;
        await this.downloadFile(mylink)
    }

    async downloadFile(mylink: string ) {
        RootContext.StartBusyIndicatorLoading();

        try {
            const data = await this.downloadRequest(mylink);
            const contentDisposition = data.headers.get('Content-Disposition');

            const blob = new Blob([data.body])
            const downloadURL = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadURL;
            const filename2 = this.decodeBase64(contentDisposition);
            link.download = filename2;
            link.click();
            link.remove();
        } catch (error) {
            RootContext.StopBusyIndicator();
            this.OnSignoutClicked();
        }

        RootContext.StopBusyIndicator();
    }
    decodeBase64(str: string): string {
        // בדוק אם המחרוזת היא קידוד ב-Base64
        if (/^data:[^;]+;base64,/.test(str)) {
            return atob(str.split(',')[1]);
        }

        // במקרה של קידוד MIME
        const regex = /filename="=\?utf-8\?B\?(.+?)\?="/;
        const match = str.match(regex);
        if (match && match[1]) {
            const base64 = match[1];
            return this.decodeUTF8(atob(base64));
        }

        str = str
        .split(';')[1]
        .split('filename')[1]
        .split('=')[1]
        .trim()
        .match(/"([^"]+)"/)[1];
        return str; // אם אין קידוד, החזר את השם המקורי
    }
    decodeUTF8(str: string): string {
        // המרת תוים מקודדים ל-UTF-8
        try {
            return decodeURIComponent(escape(str));
        } catch (e) {
            console.error('Error decoding UTF-8', e);
            return str; // החזר את הקלט המקורי במקרה של שגיאה
        }
    }
    OnSignoutClicked() {

        CargoTrackingBrandingData.Tenant = +sessionStorage.getItem("LoggedUserTenant");

        sessionStorage.clear();

        if (CargoTrackingBrandingData.Tenant)




            this.router.navigate(["cargo-tracking/login"]);//,{ queryParams: {tenant: this.tenant}}

        else

            this.router.navigate(["cargo-tracking/login"]);    

    }



    public  DownloadPage(id: string, documentName: string) {
        var url: string = "id=" + id;
        url += documentName != null ? "*" + documentName : "";
        this.GetCurrenctUserValidity().subscribe((response:any) => {
            this.token = response.Result.DocumentDownloadToken;
            var link = ServiceHelper.GetAppURL(this.baseUrl) + "WebPages/DownloadPage.aspx?" + url + "&tempId=" + this.token + "&requestArea=CargoTracking";
            var win = window.open(link, '_blank');

            if (win) {
                win.focus();
            }
        });

    }

    DownloadAllPages(entityId: string, securityKey: string) {
        this.GetCurrenctUserValidity().subscribe((response: any) => {
            this.token = response.Result.DocumentDownloadToken;
            var link = ServiceHelper.GetAppURL(this.baseUrl) + "WebPages/SharedDownloadPage.aspx?id=" + SessionInfo.LoggedUserTenant + ":" + null + ":ship:" + entityId + ":CS:" + null + ":" + securityKey + ":securitykey:CargoTracking";
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

