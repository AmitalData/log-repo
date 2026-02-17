import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {TaxReportPM} from '../../EntityPMs/TaxReportPM';
import {TaxReportLinePM} from '../../EntityPMs/TaxReportLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'

@Injectable()

export class TaxReportExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
      this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxReportOp';
    }

    DownloadPNC874File(taxReportPM: TaxReportPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: TaxReportPM;
            //mappedEntity = this.MapJsonToEntityPM(taxReportPM, false);

            return this._http.post(this._apiUrl + "/PostDownloadPNC874File", JSON.stringify(taxReportPM), { headers: authHeader })
                .map((res) => {

                    var result = res.json();
                    serviceResponse.Result = result;

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });

    }

    DownloadPNC874FileInBatch(taxReportPM: TaxReportPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: TaxReportPM;
            //mappedEntity = this.MapJsonToEntityPM(taxReportPM, false);

            return this._http.post(this._apiUrl + "/PostDownloadPNC874FileInBatch", JSON.stringify(taxReportPM), { headers: authHeader })
                .map((res) => {

                    var result = res.json();
                    serviceResponse.Result = result;

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });

    }

    PostCreateTaxReportInBatch(taxReportPM: TaxReportPM) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: TaxReportPM;
            //mappedEntity = this.MapJsonToEntityPM(taxReportPM, false);

            return this._http.post(this._apiUrl + "/PostCreateTaxReportInBatch", JSON.stringify(taxReportPM), { headers: authHeader })
                .map((res) => {

                    var result = res.json();
                    serviceResponse.Result = result;

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });
      }

    GetReportLinesCounter(taxReportId: string) {

            return Observable.defer(() => {

                var authHeader = new Headers();
                authHeader.append('Token', SessionInfo.Token);
                authHeader.append('Content-Type', 'application/json');

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();

                return this._http.get(this._apiUrl+'/GetLinesCounters?taxReportId='+taxReportId, { headers: authHeader }).map(response => {
                    var allLists = response.json();

                    var serviceResponse = new ServiceResponse();
                    serviceResponse.Result = allLists;
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
            });

    }


    getErrorsCount(reportId: string) {
	    var callTime = new Date();
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl+'/GetErrorsCount/?'+'reportId=' + reportId, {
                headers: authHeader
            }).map(response => {
                var result = response.json();

                return result;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TaxReportPM = null) {


        if (!entityPM) {

            entityPM = new TaxReportPM();
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }




        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);



        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
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

    public GetNewEntityPM() {
        var entityPM: TaxReportPM;
        entityPM = new TaxReportPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }


}
