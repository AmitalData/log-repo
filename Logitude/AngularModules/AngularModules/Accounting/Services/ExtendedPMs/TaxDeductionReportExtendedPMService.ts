
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { TaxDeductionReportPM } from '../../EntityPMs/TaxDeductionReportPM';
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


}
