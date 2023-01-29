import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { catchError, map } from 'rxjs/operators';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { InterestTransactionPM } from '../../EntityPMs/InterestTransactionPM';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass';
import { InterestReportLinesByDatePM } from '../../EntityPMs/InterestReportLinesByDatePM';
import { Guid } from '../../../Infrastructure/Utilities/Guid';

 

@Injectable()
export class InterestTransactionExtendedListService {
     private httpClient: HttpClient;
     private _apiUrl: string;

    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InterestTransactionViews';
    }


    GetAllInterestTransactionByDate(ReportId: string, InterestCalculationDate: Date) {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        var url = this._apiUrl + "/GetAllInterestTransactionByDate?ReportId=" + ReportId + "&InterestCalculationDate=" + InterestCalculationDate;
        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                serviceResponse.Result = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

    PutConfirmCreateInvoice(entityPM: InterestReportPM) {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        var url = this._apiUrl + "/PutConfirmCreateInvoice";
        var validator: ClassLevelValidator;
        validator = new ClassLevelValidator();
        var errorsArray = validator.Validate("InterestReport", entityPM);
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        if (errorsArray.length == 0) {
            var mappedEntity: InterestReportPM;
            mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return this.httpClient.put(url, JSON.stringify(mappedEntity),ServiceHelper.GetHttpHeaders()).pipe(
                map(response => {
                    var pm = response;
                    if (pm) {
                        var mappedResult: InterestReportPM;
                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
        }
    }

    PutInterestTransactionNotes(entityPM: InterestTransactionPM,Notes: string){
        var serviceResponse: ServiceResponse = new ServiceResponse();
        var url = this._apiUrl + "/PutInterestTransactionNotes";
        var validator: ClassLevelValidator;
        validator = new ClassLevelValidator();
        var errorsArray = validator.Validate("InterestTransaction", entityPM);
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();
        if (errorsArray.length == 0) {
            var mappedEntity: InterestTransactionPM;
            mappedEntity = this.MapPM(entityPM, false);

            return this.httpClient.put(url, JSON.stringify(mappedEntity),ServiceHelper.GetHttpHeaders()).pipe(
                map(response => {
                    var pm = response;
                    if (pm) {
                        var mappedResult: InterestTransactionPM;
                        mappedResult = this.MapPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }
                    serviceResponse.Result = response;
                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
        }

    }

    MapPM(jsonPM: any, getCallMap: boolean = true, entityPM: InterestTransactionPM = null) {
        if (!entityPM) {
            entityPM = new InterestTransactionPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        return entityPM;
    }

    GetSingle(id: string) {
        var serviceResponse: ServiceResponse = new ServiceResponse();
        return this.httpClient.get(this._apiUrl + '/GetSingle?id=' + id  ,  ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
             }),
            catchError(ServiceHelper.HandleServiceError));
    
    }

    GetCheckRecentReports(interestDate: Date,  customerId: string  ) {
        
        var serviceResponse: ServiceResponse = new ServiceResponse();
        var url = this._apiUrl + "/GetCheckRecentReports?interestDate=" + interestDate + "&customerId=" + customerId;
        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                serviceResponse.Result = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: InterestReportPM = null) {


        if (!entityPM) {

            entityPM = new InterestReportPM();
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

        this.MapInterestReportLinesByDates(entityPM, jsonPM, mapParent); // Call composition tables map methods



        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.InterestReportLinesByDates = [];
            for (var item in entityPM.InterestReportLinesByDates) {
                var myInterestReportLinesByDatePM = entityPM.InterestReportLinesByDates[item];
                var newInterestReportLinesByDatePM: InterestReportLinesByDatePM = this.clone(myInterestReportLinesByDatePM);


                entityPM.OldEntityPM.InterestReportLinesByDates.push(newInterestReportLinesByDatePM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }

    MapInterestReportLinesByDates(entityPM: InterestReportPM, jsonPM: any, mapParent: boolean = true) {

        var oldInterestReportLinesByDates: InterestReportLinesByDatePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldInterestReportLinesByDates = entityPM.OldEntityPM.InterestReportLinesByDates;
        }

        entityPM.InterestReportLinesByDates = new Array<InterestReportLinesByDatePM>();
        for (var item in jsonPM.InterestReportLinesByDates) {
            var jItem = jsonPM.InterestReportLinesByDates[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newInterestReportLinesByDatePM: InterestReportLinesByDatePM;

            if (mapParent) {
                newInterestReportLinesByDatePM = new InterestReportLinesByDatePM(entityPM);
            }
            else {
                newInterestReportLinesByDatePM = new InterestReportLinesByDatePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newInterestReportLinesByDatePM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newInterestReportLinesByDatePM.UniqueKey = Guid.newGuid();
                newInterestReportLinesByDatePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newInterestReportLinesByDatePM.OldEntityPM = this.clone(newInterestReportLinesByDatePM);


            }
            else {
                if (newInterestReportLinesByDatePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newInterestReportLinesByDatePM.ChangeSetOp = "Update";
                }
                else {
                    newInterestReportLinesByDatePM.ChangeSetOp = "Insert";
                }

                newInterestReportLinesByDatePM.OldEntityPM = null;
                newInterestReportLinesByDatePM.EntityParentPM = null;
            }

            newInterestReportLinesByDatePM.IsDirty = false;
            entityPM.InterestReportLinesByDates.push(newInterestReportLinesByDatePM);
        }
        if (oldInterestReportLinesByDates) {

            for (var itemKey in oldInterestReportLinesByDates) {
                if (entityPM.InterestReportLinesByDates.filter(p => p.UniqueKey === oldInterestReportLinesByDates[itemKey].UniqueKey).length === 0) {

                    if (oldInterestReportLinesByDates[itemKey]) {
                        //oldInterestReportLinesByDates[itemKey].ChangeSetOp = "Delete";
                        //entityPM.InterestReportLinesByDates.push(oldInterestReportLinesByDates[itemKey]);
                        var oldItemJson = oldInterestReportLinesByDates[itemKey];
                        var deletedPM: InterestReportLinesByDatePM = new InterestReportLinesByDatePM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.InterestReportLinesByDates.push(deletedPM);
                    }
                }
            }
        }
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
