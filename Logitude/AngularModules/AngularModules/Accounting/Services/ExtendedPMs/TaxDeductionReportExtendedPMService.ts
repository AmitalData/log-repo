
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { TaxDeductionReportPM} from '../../EntityPMs/TaxDeductionReportPM';
import { TaxDeductionReportData, TotalForCompany, DBVendorsList, ByMonthList, ByVendorList  } from '../../DataContracts/TaxDeductionReportData';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
 

@Injectable()


export class TaxDeductionReportExtendedPMService {

   
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
     
        this.httpClient = ServiceHelper.HttpClient;
         this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxDeductionReportFile';
    }

 

    DownloadTaxDeduction856FileInBatch(taxDeductionReportPM: TaxDeductionReportPM) {
        return this.httpClient.post(this._apiUrl + "/PostDownloadTaxDeduction856FileInBatch" ,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: TaxDeductionReportPM;
            mappedEntity = this.MapJsonToEntityPM(taxDeductionReportPM, false);
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }



    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TaxDeductionReportPM = null) {


        if (!entityPM) {

            entityPM = new TaxDeductionReportPM();
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

        entityPM.IsDirty = false;
        return entityPM;
    }



    GetTaxDeductionReportData(reportId: string) {
        return this.httpClient.get(this._apiUrl + "/GetTaxDeductionReportData?reportId=" + reportId ,  ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;

                var _mappedListsArray: Array<TaxDeductionReportData> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: TaxDeductionReportData;
                        entity = this.MapJsonToEntityData(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError));
       
        

    }


    MapJsonToEntityData(jsonData: any, mapParent: boolean = true, entityData: TaxDeductionReportData = null) {


        if (!entityData) {

            entityData = new TaxDeductionReportData();
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonDataKeys = Object.keys(jsonData);

        for (var key in jsonDataKeys) {
            if (jsonDataKeys[key] === "UIProperties" || jsonDataKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonDataKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonData[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonData[property].Value, jsonData[property].FieldName, jsonData[property].TableName);
                    entityData[property] = customFieldClass;
                }
            }
            else {
                entityData[property] = jsonData[property];
            }

        }

        return entityData;
    }







}
