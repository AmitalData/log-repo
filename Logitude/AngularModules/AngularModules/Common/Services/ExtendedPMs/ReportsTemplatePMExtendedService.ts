
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

import { ReportsTemplatePM } from '../../EntityPMs/ReportsTemplatePM';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { ReportFliter } from 'Report/Components/Filters/ReportFliter';


@Injectable()
export class ReportsTemplatePMExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsTemplateExtended';
    }


    GetReportsTemplatePMsByReportId(reportId: string, reportType: string = "") {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetReportsTemplatePMsByReportId" + '?reportId=' + reportId + "&reportType=" + reportType, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result: any = response;
            var entity: ReportsTemplatePM;
            var ReportsTemplatePMLists: ReportsTemplatePM[];
            ReportsTemplatePMLists = new Array<ReportsTemplatePM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                ReportsTemplatePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = ReportsTemplatePMLists;
            return pmresponse;




        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetCopyReportsTemplate(reportsTemplateId: string, userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetCopyReportsTemplateByReportsTemplateId" + '?reportsTemplateId=' + reportsTemplateId + "&userId=" + userId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var entity: ReportsTemplatePM;
            var result: any = response;
            entity = this.MapJsonToEntityPM(result);


            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;




        }), catchError(ServiceHelper.HandleServiceError));
    }
    PostReportTemplateEditorHtmlData(reportParams:ReportTemplateEditorHtmlDataParams) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(reportParams);
            return this._http.post(
                this._apiUrl + '/PostReportTemplateEditorHtmlData/',
                JSON.stringify(params),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    serviceResponse.Result = res;
                    return serviceResponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });
    }



    CreateReportTemplate(reportsTemplatePM: ReportsTemplatePM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');


            var errorsArray = [];


            var response: ServiceResponse;
            response = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ReportsTemplatePM;
                mappedEntity = this.MapJsonToEntityPM(reportsTemplatePM, false);
                return this._http.post(this._apiUrl + '/PostReportTemplate', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var pm = res;
                    if (pm) {
                        var mappedResult: ReportsTemplatePM;
                        mappedResult = this.MapJsonToEntityPM(pm, true, reportsTemplatePM);
                        response.Result = mappedResult;
                    }

                    return response;

                }));
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return of(response);

            }
        }

        );
    }


    GetMessageReportsTemplateBodyByReportTemplateIdAndVersion(reportsTemplateId: string, version: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetMessageReportsTemplateBodyByReportTemplateIdAndVersion" + '?reportsTemplateId=' + reportsTemplateId + "&version=" + version, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result: any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;




        }), catchError(ServiceHelper.HandleServiceError));
    }

    SaveReportTemplateMessageBody(reportsTemplatePM: ReportsTemplatePM) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var errorsArray = [];

            var response: ServiceResponse;
            response = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ReportsTemplatePM;
                mappedEntity = this.MapJsonToEntityPM(reportsTemplatePM, false);
                return this._http.put(this._apiUrl + '/PutSaveReportTemplateMessageBody', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var pm = res;
                    if (pm) {
                        var mappedResult: ReportsTemplatePM;
                        mappedResult = this.MapJsonToEntityPM(pm, true, reportsTemplatePM);
                        response.Result = mappedResult;
                    }

                    return response;

                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errorsArray;

                return of(response);

            }
        }

        );
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ReportsTemplatePM = null) {


        if (!entityPM) {

            entityPM = new ReportsTemplatePM();
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



}

export class ReportTemplateEditorHtmlDataParams{
    public ReportsTemplateId: string;
    public Version: number;
    public UserId: string;
    public Subject: string;
    public From: string;
    public ReplyTo: string;
    public Cc: string;
    public ReportFilter:ReportFliter
}
