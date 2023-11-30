import { Injectable } from '@angular/core';
import { SessionInfo } from '../Utilities/SessionInfo';
import { LoginExtendedService } from './Extended/LoginExtendedService';

@Injectable({
    providedIn: 'root'
})
export class DocumentDownloadTokenUpdateService {
    public tryEvry = 1000 * 60 * 1 ;
    public downloadTokenInterval;
    constructor(
        private loginExtendedService: LoginExtendedService,
    ) {
    }
    startIntervalUpdate() {
        this.updateToken();
        if (this.downloadTokenInterval)
            clearInterval(this.downloadTokenInterval);
        this.downloadTokenInterval = setInterval(() => this.updateToken(), this.tryEvry);

    }
    updateToken() {
        var token = sessionStorage.getItem("Token")
        if (token && token.length > 0)
            this.loginExtendedService.GetDocumentDownloadToken().subscribe((myResult: any) => {
                if (myResult) {
                    SessionInfo.DocumentDownloadToken = myResult;
                }
            });
    }
}
