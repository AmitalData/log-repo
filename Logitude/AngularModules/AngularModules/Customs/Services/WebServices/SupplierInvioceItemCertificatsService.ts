import { catchError, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { defer, of } from 'rxjs';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CertificateErrorView } from '../../../CustomsModules/CustomsGeneralRequests/Components/ReceiptCertificateFromFileComponent';


@Injectable()

export class SupplierInvioceItemCertificatsService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SupplierInvioceItemCertificats';

    }
    PutSupplierInvioceItemCertificatFromFileRequest(fileUploadParamerter: any, tenant: number, clientId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + "/PutSupplierInvioceItemCertificatFromFileRequest?" + "tenant=" + tenant
                + "&clientId=" + clientId, JSON.stringify(fileUploadParamerter),ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {
                var result = response.json();
                var _mappedListsArray: Array<CertificateErrorView> = [];
                if (result) {
                    for (var key in result) {
                        var entity: CertificateErrorView;
                        entity = this.MapJsonToEntityPM(result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        
        });
        
    }
    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CertificateErrorView;
        entityPM = new CertificateErrorView();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}
