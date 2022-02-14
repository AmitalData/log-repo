import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import { TariffSettingPM } from '../EntityPMs/TariffSettingPM';
import { CustomFieldClass } from '../../Infrastructure/DataContracts/CustomFieldClass'
import { ShipmentPackagePM } from '../../Shipment/EntityPMs/ShipmentPackagePM';

@Injectable()

export class TariffDomainService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TariffDomain';
    }

    GetTariffsCounts() {

        var url = this._apiUrl + '/GetTariffsCounts';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

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
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAvailableAirlineFreightTariffs(args: TariffSearchArgs) {

        return defer(() => {
            return this._http.post(this._apiUrl + "/PostAvailableAirlineFreightTariffs", JSON.stringify(args), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetCheckDatesValidty(FromPort: string, ToPort: string, ToDate: Date, TariffId:string) {

        var url = this._apiUrl + '/GetCheckDatesValidty?FromPort=' + FromPort + "&ToPort=" + ToPort +  "&ToDate=" + ServiceHelper.GetDateString(ToDate)  + "&TariffId=" + TariffId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }    

    GenerateTariffs() {

        var url = this._apiUrl + '/GetGenerateTariffs';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTariffsLogsByTariffId( tariffId: string, version: number) {

        var url = this._apiUrl + '/GetTariffsLogsByTariffId?tariffId=' + tariffId + "&version=" + version;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var listJason = response;
                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GenerateTariffsFromExcel(filter: TariffFilterParameter) {

        return defer(() => {
            return this._http.post(this._apiUrl + "/PostGenerateTariffsFromExcel", JSON.stringify(filter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }
        
    GetTenantTariffSetting() {


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTenantTariffSetting', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;

                var entity: TariffSettingPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    DownloadTariff(tariffId: string, version: number, type: string) {

        var url = this._apiUrl + '/GetDownloadTariff?tariffId=' + tariffId + "&version=" + version + "&type=" + type;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    PostUploadExcelFile(filter: TariffFilterParameter) {

        return defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadExcelFile", JSON.stringify(filter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    ApproveVersion(tariffId: string, version: number) {

        var url = this._apiUrl + '/GetApproveVersion?tariffId=' + tariffId + "&version=" + version;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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

        var url = this._apiUrl + '/GetTariffVersionLines?tariffId=' + tariffId + "&version=" + version

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAllVersionsWithLinesForTariff(tariffId: string) {

        var url = this._apiUrl + '/GetAllVersionsWithLinesForTariff?tariffId=' + tariffId  
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    PostUpdateSurcharge(filter: UpdateSurchargeArgs) {

        return defer(() => {
            return this._http.post(this._apiUrl + "/PostUpdateSurcharge", JSON.stringify(filter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    GetRecentTariffs() {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetRecentTariffs', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                return allLists;
            }));
        });
    }

    GetTariffLineContainerPrices(tariffId:string, version: number, fromPortId: string, toPortId: string) {

        var url = this._apiUrl + "/GetTariffLineContainerPrices?tariffId=" + tariffId + "&version=" + version + "&fromPortId=" + fromPortId + "&toPortId=" + toPortId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    RefreshPortsFromTranslations(tariffId: string, version: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRefreshPortsFromTranslations?tariffId=' + tariffId + "&version=" + version;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTariffsPricesConnectedToPayables(freightTariffId: string, shipmentId: string, tariffType: string) {
        var url = this._apiUrl + '/GetTariffsPricesConnectedToPayables?freightTariffId=' + freightTariffId + "&shipmentId=" + shipmentId + "&tariffType=" + tariffType
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetUploadedExcelByTariffAndVersion(tariffId: string, version: number) {

        var url = this._apiUrl + '/GetUploadedExcelByTariffAndVersion?tariffId=' + tariffId + "&version=" + version;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var allLists = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAvailableCustomsChargesTariffs(args: CustomsChargesTariffSearchArgs) {
        return defer(() => {
            return this._http.post(this._apiUrl + "/PostAvailableCustomsChargesTariffs", JSON.stringify(args), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class TariffSummery {
    AirFreightCount: number;
    AirSurchargeCount: number;
    OceanSurchargeCount: number;
    OceanLCLFreightCount: number;
    OceanFCLFreightCount: number;
    OceanFCLSurchargesCount: number;
    ImportCustomsChargesCount: number;
    ExportCustomsChargesCount: number;
}

export class TariffFilterParameter {
    Tenant: number;
    FileData: string;
    PriceSteps: string;
    TariffId: string;
    Version: number;
    TariffType: string;
    FileName: string;
    FileExtension: string;
}

export class TariffSearchSummary {
    TariffId: string;
    TariffNumber: string;
    ChargeTypeId: string;
    Price: string;
    SurchargesPrice: string;
    ActualPrice: number;
    EffictiveDate: Date;
    Remarks: string;
    ImageId: string;
    SellerName: string;
    ViaPortId: string;
    ViaPortCode: string;
    ViaPortName: string;
    ViaPortCountryCode: string;
    ViaPortCountryName: string;
    CurrencyCode: string;
    CurrencyId: string;
    VersionId: string;
    TotalSurcharge: string;
    WholePrice: string;
    WholePriceWithoutAllIn: string;
    SurchargesWithoutAllIn: Array<SurchargeSummary>;
    AllInSurcharges: Array<SurchargeSummary>;
    AllIn: string;
    AllInIds: string;
    IsShown: boolean = false;
    UnitOfMesurmentCode: string;
    UnitOfMesurmentId: string;
    SellerId: string;
    MinPrice: number;
    IsMinIconVisible: boolean;
    LineId: string;
    ContainersPrices: Array<ContainersPrice>;
    TransitTime: string;
    LastUsedDate: Date;
    UpdateDate: Date;
    ValidityDate: string;
    CurrencySign: string;
    NoteMissingContainers: string;
    MoreLessDetailsLabel: string = "More Details";
    ActualMinPrice: number;
    IsDifferentCurrency: boolean;
}

export class ContainersPrice {
    ContainerId: string;
    TariffId: string;
    Price: number;
    Quantity: number;
    Price_WithoutQuantity: number;
}

export class SurchargeSummary {
    Code: string;
    Name: string;
    Price: number;
    ActualPrice: number;
    ChargeTypeId: string;
    TariffId: string;
    TariffNumber: string;
    CurrencyId: string;
    UnitOfMesurmentCode: string;
    UnitOfMesurmentId: string;
    VersionId: string;
    SellerId: string;
    SellerName: string;
    MinPrice: number;
    IsMinIconVisible: boolean;
    LineId: string;
    IsAllIn: boolean;
    ContainersPrices: Array<ContainersPrice>;
    CurrencySign: string;
}

export class ExcelTariffLines {
    FromPortId: string;
    FromPortCode: string;
    FromPortCombinedCode: string;
    FromPortName: string;
    ToPortId: string;
    ToPortCode: string;
    ToPortCombinedCode: string;
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
    StartDate: Date;
    StartDateText: string;
    TransitTime: string;
}

export class UpdateSurchargeArgs {
    TariffId: string;
    VersionNumber: number;
    From: string[] = [];
    To: string[] = [];
    Surcharge: string[] = [];
    StartDate: Date;
}

export class TariffSearchArgs {
    OriginPortId: string;
    DestinationPortId: string;
    ViaPortId: string;    
    Date: string;
    Weight : number;
    WeightCode: string;
    GrossWeight :number;
    GrossWeightCode: string;
    Volume : number;
    VolumeUnitCode: string;
    CurrencyId: string;
    TariffType: string;
    ContainerType1Id: string;
    ContainerType2Id: string;
    ContainerType3Id: string;
    ContainerType4Id: string;
    ContainerType5Id: string;
    Quantity1: number;
    Quantity2: number;
    Quantity3: number;
    Quantity4: number;
    Quantity5: number;
    ProductId: string;
}

export class CustomsChargesTariffSearchArgs {
    FromCountryId: string;
    ToCountryId: string;
    MainCarriageATD: Date;
    MainCarriageETD: Date;    
    FriehgtAmount: number;    
    ForiegnChargesAmount: number;
    LocalCurrencyId: string;    
    ShipmentId: string;
    CustomsChargesPayables: CustomsChargesPayable[] = [];
}

export class CustomsChargesPayable {
    ChargeTypeId: string;
    ChargeTypeCode: string;
    ChargeTypeName: string;
    UnitOfMesurmentId: string;
    UnitOfMesurmentCode: string;
    TariffId: string;
    TariffNumber: string;
    VersionId: number;
    TariffLineId: string;
    CurrencyId: string;
    CurrencyCode: string;
    ExpectedAmount: number;
    LocalExpectedAmount: number;
    ProfitExpectedAmount: number;
    MinAmount: number;
    Quantity: number;
    Price: number;
    IsDifferentCurrency: boolean;
    Notes: string;
    Rate: number;
    CustomsBrokerId: string;
    CustomsBrokerName: string;
}
