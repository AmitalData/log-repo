import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { SessionInfo } from './SessionInfo';
export var PasswordChangeService = (function () {
    function PasswordChangeService(_http) {
        this._http = _http;
        //this._http = ServiceHelper.Http;
        this._apiUrl = SessionInfo.GetLogitudeURL() + 'api/PasswordChange';
    }
    PasswordChangeService.decorators = [
        { type: Injectable },
    ];
    /** @nocollapse */
    PasswordChangeService.ctorParameters = [
        { type: Http, },
    ];
    return PasswordChangeService;
}());
//# sourceMappingURL=PasswordChangeService.js.map