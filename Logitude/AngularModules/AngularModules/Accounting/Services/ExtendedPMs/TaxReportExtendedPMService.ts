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

            var mappedEntity: TaxReportPM;
            mappedEntity = this.MapJsonToEntityPM(taxReportPM, false);

            return this._http.post(this._apiUrl + "/PostDownloadPNC874File", JSON.stringify(mappedEntity), { headers: authHeader })
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

            var mappedEntity: TaxReportPM;
            mappedEntity = this.MapJsonToEntityPM(taxReportPM, false);

            return this._http.post(this._apiUrl + "/PostDownloadPNC874FileInBatch", JSON.stringify(mappedEntity), { headers: authHeader })
                .map((res) => {

                    var result = res.json();
                    serviceResponse.Result = result;

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });

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

        this.MapTaxReportLines(entityPM, jsonPM, mapParent); // Call composition tables map methods



        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.TaxReportLines = [];
            for (var item in entityPM.TaxReportLines) {
                var myTaxReportLinePM = entityPM.TaxReportLines[item];
                var newTaxReportLinePM: TaxReportLinePM = this.clone(myTaxReportLinePM);


                entityPM.OldEntityPM.TaxReportLines.push(newTaxReportLinePM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }

    MapTaxReportLines(entityPM: TaxReportPM, jsonPM: any, mapParent: boolean = true) {

        var oldTaxReportLines: TaxReportLinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldTaxReportLines = entityPM.OldEntityPM.TaxReportLines;
        }

        entityPM.TaxReportLines = new Array<TaxReportLinePM>();
        for (var item in jsonPM.TaxReportLines) {
            var jItem = jsonPM.TaxReportLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newTaxReportLinePM: TaxReportLinePM;

            if (mapParent) {
                newTaxReportLinePM = new TaxReportLinePM(entityPM);
            }
            else {
                newTaxReportLinePM = new TaxReportLinePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newTaxReportLinePM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newTaxReportLinePM.UniqueKey = Guid.newGuid();
                newTaxReportLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newTaxReportLinePM.OldEntityPM = this.clone(newTaxReportLinePM);


            }
            else {
                if (newTaxReportLinePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newTaxReportLinePM.ChangeSetOp = "Update";
                }
                else {
                    newTaxReportLinePM.ChangeSetOp = "Insert";
                }

                newTaxReportLinePM.OldEntityPM = null;
                newTaxReportLinePM.EntityParentPM = null;
            }

            newTaxReportLinePM.IsDirty = false;
            entityPM.TaxReportLines.push(newTaxReportLinePM);
        }
        if (oldTaxReportLines) {

            for (var itemKey in oldTaxReportLines) {
                if (entityPM.TaxReportLines.filter(p => p.UniqueKey === oldTaxReportLines[itemKey].UniqueKey).length === 0) {

                    if (oldTaxReportLines[itemKey]) {
                        //oldTaxReportLines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.TaxReportLines.push(oldTaxReportLines[itemKey]);
                        var oldItemJson = oldTaxReportLines[itemKey];
                        var deletedPM: TaxReportLinePM = new TaxReportLinePM(null);
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
                        entityPM.TaxReportLines.push(deletedPM);
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

    public GetNewEntityPM() {
        var entityPM: TaxReportPM;
        entityPM = new TaxReportPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }

    
} 
