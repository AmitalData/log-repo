
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { TaxDeductionReportPM } from '../../EntityPMs/TaxDeductionReportPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'

@Injectable()


export class TaxDeductionReportExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TaxDeductionReportFile';
    }

 

    DownloadTaxDeduction856FileInBatch(taxDeductionReportPM: TaxDeductionReportPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: TaxDeductionReportPM;
            mappedEntity = this.MapJsonToEntityPM(taxDeductionReportPM, false);

            return this._http.post(this._apiUrl + "/PostDownloadTaxDeduction856FileInBatch", JSON.stringify(mappedEntity), { headers: authHeader })
                .map((res) => {

                    var result = res.json();
                    serviceResponse.Result = result;

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });

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
