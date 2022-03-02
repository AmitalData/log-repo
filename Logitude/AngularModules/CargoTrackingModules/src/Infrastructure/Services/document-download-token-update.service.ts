import { Injectable } from '@angular/core';
import { SessionInfo } from '../Utilities/SessionInfo';
import { LoginExtendedService } from './Extended/LoginExtendedService';

@Injectable({
    providedIn: 'root'
})
export class DocumentDownloadTokenUpdateService {
    public tryEvry = 1000 * 60 * 1 ;
    public downloadTokenTimer;
    constructor(
        private loginExtendedService: LoginExtendedService,
    ) {
    }
    startUpdateDocumentDownloadToken() {
        this.updateDocumentDownloadToken();
        if (this.downloadTokenTimer)
            clearInterval(this.downloadTokenTimer);
        this.downloadTokenTimer = setInterval(() => this.updateDocumentDownloadToken(), this.tryEvry);

    }
    updateDocumentDownloadToken() {
        var token = sessionStorage.getItem("Token")
        if (token && token.length > 0)
            this.loginExtendedService.GetDocumentDownloadToken().subscribe((myResult: any) => {
                if (myResult) {
                    SessionInfo.DocumentDownloadToken = myResult;
                }
            });
    }
}
