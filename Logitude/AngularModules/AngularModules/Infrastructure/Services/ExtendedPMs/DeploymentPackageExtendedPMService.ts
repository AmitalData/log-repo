import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { of, defer } from 'rxjs';
import { DeploymentPackagePM } from '../../EntityPMs/DeploymentPackagePM';

@Injectable()
export class DeploymentPackageExtendedPMService {
    private httpClient: HttpClient;
    private apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeploymentPackageExtended';
    }

    ValidateDependiencies(entityPM: DeploymentPackagePM) {
        return defer(() => {
            var url = this.apiUrl + '/ValidateDependiencies';
            return this.httpClient.put(url, JSON.stringify(this.clone(entityPM)), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;

                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetDeploymentPackageDetailsListByDocumentId(documentId: string) {
        return defer(() => {
            return this.httpClient.get(this.apiUrl + '/GetDeploymentPackageDetailsListByDocumentId?' + 'documentId=' + documentId, ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        let serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;

                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError));
        });
    }
    DeleteImportedDocumentById(documentId: string) {
        return defer(() => {
            return this.httpClient.get(this.apiUrl + '/DeleteImportedDocumentById?' + 'documentId=' + documentId, ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        let serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;

                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError));
        });
    }
    ValidateDeploymentPackageCode(code: string) {
        return defer(() => {
            return this.httpClient.get(this.apiUrl + '/ValidateDeploymentPackageCode?' + 'code=' + code, ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        let serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;

                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}
