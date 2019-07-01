import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import { TariffSettingPM } from '../EntityPMs/TariffSettingPM';
import { CustomFieldClass } from '../../Infrastructure/DataContracts/CustomFieldClass'

@Injectable()

export class TariffDomainService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TariffDomain';
    }

    GetTariffsCounts() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTariffsCounts';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var myResult = new TariffSummery();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAvailableAirlineFreightTariffs(FromPort: string, ToPort: string, BetweenDate: Date, Weight: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAvailableAirlineFreightTariffs?FromPort=' + FromPort + "&ToPort=" + ToPort + "&BetweenDate=" + ServiceHelper.GetDateString(BetweenDate) + "&Weight=" + Weight;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }    

    GenerateTariffs() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetGenerateTariffs';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GenerateTariffsFromExcel(filter: TariffFilterParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + "/PostGenerateTariffsFromExcel", JSON.stringify(filter), {
                headers: authHeader,
            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    
    GetTenantTariffSetting() {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTenantTariffSetting', { headers: authHeader }).map(response => {
                var pm = response.json();

                var entity: TariffSettingPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    DownloadTariff(tariffId: string, version: number, type: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetDownloadTariff?tariffId=' + tariffId + "&version=" + version + "&type=" + type;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    PostUploadExcelFile(filter: TariffFilterParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadExcelFile", JSON.stringify(filter), {
                headers: authHeader,
            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    ApproveVersion(tariffId: string, version: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetApproveVersion?tariffId=' + tariffId + "&version=" + version;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TariffSettingPM = null) {


        if (!entityPM) {

            entityPM = new TariffSettingPM();
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

    clone(jsonPM: any) {
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

    GetTariffVersionLines(tariffId: string, version: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTariffVersionLines?tariffId=' + tariffId + "&version=" + version

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    PostUpdateSurcharge(filter: UpdateSurchargeArgs) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + "/PostUpdateSurcharge", JSON.stringify(filter), {
                headers: authHeader,
            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }
}

export class TariffSummery {
    AirFreightCount: number;
    AirSurchargeCount: number;
}

export class TariffFilterParameter {
    Tenant: number;
    FileData: string;
    PriceSteps: string;
    TariffId: string;
    Version: number;
    TariffType: string;
    FileName: string;
}

export class TariffSearchSummary {
    Id: string;
    price: string;
    EffictiveDate: Date;
    Remarks: string;
    ImageId: string;
    Name: string;
    Currency: string;
}

export class ExcelTariffLines {
    FromPortId: string;
    FromPortCode: string;
    FromPortName: string;
    ToPortId: string;
    ToPortCode: string;
    ToPortName: string;
    MinPrice: number;
    Step1Price: number;
    Step2Price: number;
    Step3Price: number;
    Step4Price: number;
    Step5Price: number;
    Step6Price: number;
    Step7Price: number;
    Step8Price: number;

    FromPortText: string;
    ToPortText: string;
    MinPriceText: string;
    Step1PriceText: string;
    Step2PriceText: string;
    Step3PriceText: string;
    Step4PriceText: string;
    Step5PriceText: string;
    Step6PriceText: string;
    Step7PriceText: string;
    Step8PriceText: string;

    HasErrors: boolean;
    ErrorText: string;

    Surcharge1Price: number;
    Surcharge2Price: number;
    Surcharge3Price: number;
    Surcharge4Price: number;
    Surcharge5Price: number;
    Surcharge6Price: number;
    Surcharge7Price: number;
    Surcharge8Price: number;
    Surcharge9Price: number;
    Surcharge10Price: number;

    Surcharge1PriceText: string;
    Surcharge2PriceText: string;
    Surcharge3PriceText: string;
    Surcharge4PriceText: string;
    Surcharge5PriceText: string;
    Surcharge6PriceText: string;
    Surcharge7PriceText: string;
    Surcharge8PriceText: string;
    Surcharge9PriceText: string;
    Surcharge10PriceText: string;
    Index: number;
    Notes: string;
    IsUploaded: boolean;
    StartDate: Date;
    StartDateText: string;
}

export class UpdateSurchargeArgs {
    TariffId: string;
    VersionNumber: number;
    From: string[] = [];
    To: string[] = [];
    Surcharge: string[] = [];
    StartDate: Date;
}
