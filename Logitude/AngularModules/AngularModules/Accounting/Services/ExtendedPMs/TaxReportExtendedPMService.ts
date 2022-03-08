import {Injectable} from '@angular/core';

import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {TaxReportPM} from '../../EntityPMs/TaxReportPM';
import {TaxReportLinePM} from '../../EntityPMs/TaxReportLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'

@Injectable()

export class TaxReportExtendedPMService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {

        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxReportOp';
    }

    DownloadPNC874File(taxReportPM: TaxReportPM) {
        return this.httpClient.post(this._apiUrl + "/PostDownloadPNC874File", JSON.stringify(taxReportPM),  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    DownloadPNC874FileInBatch(taxReportPM: TaxReportPM) {
        var mappedEntity: TaxReportPM = this.MapJsonToEntityPM(taxReportPM, false);
        return this.httpClient.post(this._apiUrl + "/PostDownloadPNC874FileInBatch", JSON.stringify(mappedEntity),  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    CancelTaxReportInBatch(taxReportPM: TaxReportPM) {
        var mappedEntity: TaxReportPM = this.MapJsonToEntityPM(taxReportPM, false);
        return this.httpClient.post(this._apiUrl + "/CancelTaxReportInBatch", JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    PostCreateTaxReportInBatch(taxReportPM: TaxReportPM) {
        return this.httpClient.post(this._apiUrl + "/PostCreateTaxReportInBatch", JSON.stringify(taxReportPM),  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


      }

    GetReportLinesCounter(taxReportId: string) {
        return this.httpClient.get(this._apiUrl+'/GetLinesCounters?taxReportId='+taxReportId,  ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetTenantTransmittedTaxReports() {
        return this.httpClient.get(this._apiUrl + '/GetTenantTransmittedTaxReports', ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }
    GetReturnToDraftButtonStatus(createDate:Date) {
        return this.httpClient.get(this._apiUrl + '/GetReturnToDraftButtonStatus?createDate=' + createDate, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

  CreateNewTaxReportLine(taxReportPM: TaxReportPM) {
    return this.httpClient.put(this._apiUrl + "/PutCreateTaxReportLine", JSON.stringify(taxReportPM), ServiceHelper.GetHttpHeaders()).pipe(
      map(res => {
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        var result = res;
        serviceResponse.Result = result;

        return serviceResponse;
      }),
      catchError(ServiceHelper.HandleServiceError));
       }

    getErrorsCount(reportId: string) {
	    var callTime = new Date();

       return this.httpClient.get(this._apiUrl+'/GetErrorsCount/?'+'reportId=' + reportId,  ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {
            var result = response;

            return result;
        }),
        catchError(ServiceHelper.HandleServiceError));

    }


    CheckIfTaxReportCanHaveClosingJournal(taxReportId: string)
    {
        return this.httpClient.get(this._apiUrl + '/GetTaxReportClosingJournalAbility?taxReportId=' + taxReportId, ServiceHelper.GetHttpHeaders()).pipe(
            map(response =>
            {
                let serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
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
