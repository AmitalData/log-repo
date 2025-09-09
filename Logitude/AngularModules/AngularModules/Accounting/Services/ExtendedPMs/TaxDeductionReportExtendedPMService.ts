
import { Injectable } from '@angular/core';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { TaxDeductionReportPM} from '../../EntityPMs/TaxDeductionReportPM';
import { TaxDeductionReportData } from '../../DataContracts/TaxDeductionReportData';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { Observable } from 'rxjs/internal/Observable';
 

@Injectable()


export class TaxDeductionReportExtendedPMService {

   
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
     
        this.httpClient = ServiceHelper.HttpClient;
         this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxDeductionReportFile';
    }

 

    DownloadTaxDeduction856FileInBatch(taxDeductionReportPM: TaxDeductionReportPM) {

        var mappedEntity: TaxDeductionReportPM;
        mappedEntity = this.MapJsonToEntityPM(taxDeductionReportPM, false);

        return this.httpClient.post(this._apiUrl + "/PostDownloadTaxDeduction856FileInBatch", JSON.stringify(mappedEntity),  ServiceHelper.GetHttpHeaders()).pipe(
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



    GetTaxDeductionReportData(reportId: string): Observable<ServiceResponse> {
        const url = `${this._apiUrl}/GetTaxDeductionReportData?reportId=${encodeURIComponent(reportId)}`;
        return this.httpClient.get<any[]>(url, ServiceHelper.GetHttpHeaders()).pipe(
        map(response => {
            const returnedval = response as any;
            console.log("Returned value:", returnedval);
            let mappedList;
            if (Array.isArray(response)) {
                mappedList = response.map(json => this.MapJsonToEntityData(json));
            } else {
                mappedList = this.MapJsonToEntityData(response);
            }
            const serviceResponse = new ServiceResponse();
            serviceResponse.Result = mappedList;
            return serviceResponse;
        }),
        catchError(ServiceHelper.HandleServiceError)
        );
    }



    MapJsonToEntityData(jsonData: any, mapParent: boolean = true, entityData: TaxDeductionReportData = new TaxDeductionReportData()): TaxDeductionReportData {
        const customFields = Array.from({ length: 10 }, (_, i) => `Field${i + 1}`);


        for (const property of Object.keys(jsonData)) {
            if (property === "UIProperties" || property === "PropertyChanged") {
                continue;
            }

            if (customFields.includes(property) && jsonData[property]) {
                entityData[property] = new CustomFieldClass(
                    jsonData[property].Value,
                    jsonData[property].FieldName,
                    jsonData[property].TableName
                );
            } else {
                entityData[property] = jsonData[property];
            }
        }

        return entityData;    
    }



}
