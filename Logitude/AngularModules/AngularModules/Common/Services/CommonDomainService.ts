import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ApiQueryFilters, FilterItem} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {VatTypePercentagePM} from '../EntityPMs/VatTypePercentagePM';
import {VATTypesGroupPM} from '../EntityPMs/VATTypesGroupPM';
import {VatTypePM} from '../EntityPMs/VatTypePM';
import {CurrencyList} from '../EntityLists/CurrencyList';
import {CurrencyListService} from '../Services/StandardLists/CurrencyListService';
import {UserLicensePM} from '../EntityPMs/UserLicensePM';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {ComputingPartnerList} from '../EntityLists/ComputingPartnerList';
import {PerformanceLogger} from '../../Infrastructure/Utilities/PerformanceLogger';
import {VatTypePMService} from '../Services/StandardPMs/VatTypePMService';
import {FilingInboxPM} from '../EntityPMs/FilingInboxPM'; 
import { AccountingSettingPM } from '../EntityPMs/AccountingSettingPM';
import { CustomFieldClass } from '../../Infrastructure/DataContracts/CustomFieldClass'

@Injectable()

export class CommonDomainService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommonDomain';
    }

    DownloadUploadPartnersTemplate() {
        var url = this._apiUrl + '/GetDownloadUploadPartnersTemplate';
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    PostUploadPartnersExcelFile(filter: PartnersUploadExcelParameter) {
        return defer(() => {
            return this._http.post(this._apiUrl + "/PostUploadPartnersExcelFile", JSON.stringify(filter), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    InvokeUpdateAutoDisplay(chargeTypeId: string, propertyTypeCode: string, isAutoDisplay: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUpdateAutoDisplay?myChargeTypeId=' + chargeTypeId + '&myPropertyTypeCode=' + propertyTypeCode + '&isAutoDisplay=' + isAutoDisplay;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                return response;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    InsertNewCurrency() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    }

    GetBlueSnapToken(VaultedShopperId:string,countryName:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetBlueSnapToken?VaultedShopperId=' + VaultedShopperId + '&countryname=' + countryName,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetBlueSnapSecretToken(VaultedShopperId: string,countryName:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetBlueSnapSecretToken?VaultedShopperId=' + VaultedShopperId + '&countryname=' + countryName,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(CustomerId: string, CustomerTenantAccessId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId?CustomerId=' + CustomerId + '&CustomerTenantAccessId=' + CustomerTenantAccessId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });

    }
    GetTranslationHeadersByTenant(Id: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTranslationHeadersByTenant?Id=' + Id;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<TranslationHeader> = [];

                for (var key in allLists) {

                    var entity: TranslationHeader;
                    entity = this.MapTranslationHeaders(allLists[key]);
                    _mappedListsArray.push(entity);

                }

                return _mappedListsArray;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    CopyCurrencyToTenant(CurrencyId: string, CurrencyRate: number, RateDate: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCopyCurrencyToTenant?CurrencyId=' + CurrencyId + '&CurrencyRate=' + CurrencyRate + '&RateDate=' + RateDate;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {                

                var list = response;
                var service = new CurrencyListService();

                var entity: CurrencyList;
                if (list) {
                    entity = service.MapJsonToEntityList(list);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity; 
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetPortCopyToCurrentTenant(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetPortCopyToCurrentTenant?entityId=' + entityId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                return response;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCopyCommodityToTenant(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCopyCommodityToTenant?entityId=' + entityId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                return response;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetQuickSearch(ObjectTableName: string, SearchFields: string) {
        var callTime = new Date();

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetQuickSearch?ObjectTableName=' + ObjectTableName + '&SearchFields=' + SearchFields;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.CallTime = callTime;
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetVatTypePercentagePMByDate(date: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetVatTypePercentagePMByDate?dateString=' + ServiceHelper.GetDateString(date);

        return defer(() => {

            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<VatTypePercentagePM> = [];

                for (var key in allLists) {
                    var entity: VatTypePercentagePM;
                    entity = this.MapJsonToVatTypePercentagePM(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetAllVatTypesGroups() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllVatTypesGroups';

        return defer(() => {

            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                var _mappedListsArray: Array<VATTypesGroupPM> = [];

                for (var key in allLists) {
                    var entity: VATTypesGroupPM;
                    entity = this.MapJsonToVATTypesGroupPM(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetSingleVatTypeByCode(Code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSingleVatTypeByCode?Code=' + Code;

        return defer(() => {

            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
   
                var entity: VatTypePM;
                var myResponse = response;

                if (myResponse) {
                    var myService = new VatTypePMService();
                    entity = myService.MapJsonToEntityPM(myResponse);
                    
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToVatTypePercentagePM(jsonList: any) {
        var entityList: VatTypePercentagePM;
        entityList = new VatTypePercentagePM(null);
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    MapJsonToVATTypesGroupPM(jsonList: any) {
        var entityList: VATTypesGroupPM;
        entityList = new VATTypesGroupPM(null);
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    MapTranslationHeaders(jsonList: any) {
        var entityList: TranslationHeader;
        entityList = new TranslationHeader();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    GetContactsCounts() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetContactsCounts';

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myJsonResult = response;
                var myResult = new ContactSummary();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetGettingStartedData() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetGettingStartedData';

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateUserData(displayGettingStarted: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetUserGettingStartedData?displayGettingStarted=' + displayGettingStarted,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDropBoxAuthURI(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDropBoxAuthURI?tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDropBoxComLog(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDropBoxComLog?tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDropBoxComLogTestFile(tenant: number, FileName: string, FolderName: string, FullText: string,ObjectTableId :string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDropBoxComLogTestFile?tenant=' + tenant + '&FileName=' + FileName + '&FolderName=' + FolderName + '&FullText=' + FullText + '&ObjectTableId=' + ObjectTableId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDropBoxAccessTocken(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDropBoxAccessTocken?tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                serviceResponse = new ServiceResponse();
                var myResult = response;
                if (myResult) {
                    var serviceResponse: ServiceResponse;
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = null;
                }
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetRedOfDropBoxAccessTocken(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetRedOfDropBoxAccessTocken?tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                var myResult = response;
                if (myResult) {
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = null;
                }
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDropBoxConnectionTest(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDropBoxConnectionTest?tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                var myResult = response;
                if (myResult == "invalid_access_token") {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.Result = myResult;
                }
                return serviceResponse;
            }));
        });
    }

    GetUpdateCustomerActualData(entityId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetUpdateCustomerActualData?entityId=' + entityId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetContactsByEmails(emails: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetContactsByEmails?emails=' + emails,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetUsersByEmails(emails: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetUsersByEmails?emails=' + emails,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSingleCustomerTenantAccess(CustomerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleCustomerTenantAccess?CustomerId=' + CustomerId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetUserListsByidsString(ids: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetUserListsByidsString?ids=' + ids,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetComputingPartnerTranslationsByPartnerAndTableId(ComputingPartnerId: string, ObjectTableId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetComputingPartnerTranslationsByPartnerAndTableId?ComputingPartnerId=' + ComputingPartnerId + '&ObjectTableId=' + ObjectTableId + '&tenant=' + tenant;
        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetCustomerTenantAccessCard(CustomerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerTenantAccessCard?CustomerId=' + CustomerId;
        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetHybridTenantThresholdByIdTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetHybridTenantThresholdByIdTenant',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                serviceResponse = new ServiceResponse();
                var myResult = response;
                if (myResult) {
                    var serviceResponse: ServiceResponse;
                    serviceResponse.Result = myResult;
                }
                else {
                    serviceResponse.HasError = true;
                    serviceResponse.Result = null;
                }
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetCustomsInterfaceListByTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomsInterfaceListByTenant?',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    UpdateUserLicense(entity: UserLicenseUpdateHelper) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: UserLicenseUpdateHelper = this.MapJsonToUserLicenseUpdateHelper(entity, false);

            return this._http.put(this._apiUrl + "/PutUserLicenses", JSON.stringify(mappedEntity),ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;

                var mappedResult: UserLicenseUpdateHelper = this.MapJsonToUserLicenseUpdateHelper(myJsonResult, true, entity);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    getNoneZeroTenantTranslation(computingPartnerId:string,ObjectTableId:string,Code:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetNoneZeroTenantTranslation?ComputingPartnerId=' + computingPartnerId + '&ObjectTableId=' + ObjectTableId + '&Code=' + Code;
        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });


    }

    getByFilters(filters: CustomApiQueryFilters) {

        var callTime = new Date();

        var urlparameters = '/GetByFiltersGrouping?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ComputingPartnerViewsExtended';
        var callUrl = _apiUrl.concat(urlparameters);//
        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {

                var serviceResponse: ServiceResponse = response.body;
                //var _mappedListsArray: Array<ComputingPartnerList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result) {

                //        var entity: ComputingPartnerList;
                //        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                //        _mappedListsArray.push(entity);

                //    }
                //}

                serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ComputingPartner", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    getByFiltersSingle(filters: CustomApiQueryFilters) {

        var callTime = new Date();

        var urlparameters = '/GetByFiltersGrouping?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);


        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ComputingPartnerViewsExtended';
        var callUrl = _apiUrl.concat(urlparameters);//
        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {

                var serviceResponse: ServiceResponse = response.body;
                //var _mappedListsArray: Array<ComputingPartnerList> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result) {

                //        var entity: ComputingPartnerList;
                //        entity = this.MapJsonToEntityList(serviceResponse.Result[key]);
                //        _mappedListsArray.push(entity);

                //    }
                //}

                serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ComputingPartner", "GetByFilters", "PageIndex:" + filters.PageIndex + ", PageSize:" + filters.PageSize + ", GetAll:" + filters.GetAll);

                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDeafaultMyWarehouse() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDeafaultMyWarehouse',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSignRequestReceived() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSignRequestReceived?',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {

        var entityList: ComputingPartnerList;
        entityList = new ComputingPartnerList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

    MapJsonToUserLicenseUpdateHelper(jsonPM: any, getCallMap: boolean = true, entityPM: UserLicenseUpdateHelper = null) {
        if (!entityPM) {
            entityPM = new UserLicenseUpdateHelper();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else if (property === "Items") {

                entityPM.Items = new Array<UserLicensePM>();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];

                    var newItemPM: UserLicensePM;
                    newItemPM = this.MapUserLicensePM(jItem);
                    entityPM.Items.push(newItemPM);
                }
            }

            else {
                entityPM[property] = jsonPM[property];
            }
        }

        return entityPM;
    }

    MapUserLicensePM(jsonPM: any, mapParent: boolean = true, entityPM: UserLicensePM = null) {
        if (!entityPM) {
            entityPM = new UserLicensePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }

        else {
            entityPM.OldEntityPM = null;
        }

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

    MapTranslationItem(jsonPM: any, mapParent: boolean = true, entityPM: TranslationItem = null) {
        if (!entityPM) {
            entityPM = new TranslationItem();
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

    GetOnCreatingMexicanTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetOnCreatingMexicanTenant',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetOnCreatingUSTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetOnCreatingUSTenant',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    OnCreatingMoroccoTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetOnCreatingMoroccoTenant',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    OnCreatingIsraelTenant() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetOnCreatingIsraelTenant',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var entity: AccountingSettingPM;
                if (myResult) {
                    entity = this.MapJsonToAccountingSettingPM(myResult);
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    // FilingInbox
    GetFilingInboxes(filters: ApiQueryFilters, userId: string, isShowDeleted: boolean) {
        //var authHeader = new Headers();
        //authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        //return defer(() => {
        //    return this._http.get(this._apiUrl + '/GetFilingInboxes?userId=' + userId + '&isShowDeleted=' + isShowDeleted, {
        //        headers: authHeader
        //    }).map(response => {
        //        var myResult = response;
        //        var serviceResponse: ServiceResponse;
        //        serviceResponse = new ServiceResponse();
        //        serviceResponse.Result = myResult;
        //        return serviceResponse;
        //    }),catchError(ServiceHelper.HandleServiceError));
        //});

        var urlparameters = this._apiUrl + '/GetFilingInboxes?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];

            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (!urlparameters.endsWith('?')) {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));

            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }

        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }

        urlparameters = urlparameters.concat("&userId=" + userId + '&isShowDeleted=' + isShowDeleted);

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(urlparameters, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {
                var serviceResponse = new ServiceResponse();
                serviceResponse = response.body;
                //var _mappedListsArray: Array<FilingInboxPM> = [];
                //if (serviceResponse.Result) {
                //    for (var key in serviceResponse.Result) {
                //        var entity: FilingInboxPM;
                //        entity = this.MapJsonToFilingInboxPM(serviceResponse.Result[key]);
                //        _mappedListsArray.push(entity);
                //    }
                //}
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapJsonToFilingInboxPM(jsonList: any) {
        var entityList: FilingInboxPM;
        entityList = new FilingInboxPM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    GetFilingAttachPdfReport(documentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetFilingAttachPdfReport?documentId=' + documentId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    PutFilingInboxLogs(summary: FilingInboxSummary) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            return this._http.put(this._apiUrl + '/PutFilingInboxLogs', JSON.stringify(summary),ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;
                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetProductTypesByTenant(currentTenant: number) {
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain'

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetProductTypesByTenant?tenant=' + currentTenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myList: any = response;

                return myList;
            }));
        });

    }

    GetTenantLogoUri(Id: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTenantLogoUri?tenant=' + Id;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeadersWithoutToken()).pipe(map(response => {
                 
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
                 
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTenantEcommerceSupportEmail(id: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTenantEcommerceSupportEmail?' + 'id=' + id, ServiceHelper.GetHttpHeadersWithoutToken()).pipe(map(response => {
                var pm = response;


                //var entity: TenantPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTenantLogoUriByShipmentSecurityKey(Id: number, securityKey: string) {
        
        let url = this._apiUrl + '/GetTenantLogoUriByShipmentSecurityKey?tenant=' + Id + '&securityKey=' + securityKey;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeadersWithoutToken()).pipe(map(response => {

                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTenantEcommerceSupportEmailByShipmentSecurityKey(id: number, securityKey: string) {

        let url = this._apiUrl + '/GetTenantEcommerceSupportEmailByShipmentSecurityKey?' + 'id=' + id + '&securityKey=' + securityKey;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeadersWithoutToken()).pipe(map(response => {
                var pm = response;

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapJsonToAccountingSettingPM(jsonPM: any, mapParent: boolean = true, entityPM: AccountingSettingPM = null) {
        if (!entityPM) {

            entityPM = new AccountingSettingPM();
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

    GetCarrierAreas(carrierId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCarrierAreas?carrierId=' + carrierId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetCardOccasions(cardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCardOccasions?cardId=' + cardId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetContactOccasions(contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetContactOccasions?contactId=' + contactId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetMeasurementIdByCode(code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMeasurementIdByCode?code=' + code

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetChargesTypeByCode(code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/GetChargesTypeByCode" + '?code=' + code, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetHTSCodesForProductItemsIds(productItemIds: string, toCountryId: string) {
        var callTime = new Date();

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetHTSCodesForProductItemsIds?productItemIds=' + productItemIds + '&toCountryId=' + toCountryId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.CallTime = callTime;
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class TranslationHeader {
    Code: string;
    Description: string;
}

export class ContactSummary {
    public Id: number;
    public UpcomingEventsCount: number;
    public WithoutRemindersCount: number;
    public AllContactsCount: number;
}

export class HelpResource {
    public Code: string;
    public Name: string;
    public CreateDate: Date;
    public UpdateDate: Date;
    public Language: string;
    public Type: string;
    public Category: string;
    public VideoURL: string;
    public Duration: string;
    public FileName: string;
    public SearchFields: string;
    public IsNew: boolean;
    public FeatureCode: string;
}

export class UserLicenseUpdateHelper {
    public Tenant: number;
    public Items: UserLicensePM[] = [];
}

export class CustomApiQueryFilters {

    constructor(getAll: boolean = false) {
        this.GetAll = getAll;
    }
    public PageIndex: number;
    public PageSize: number;
    public SortBy: string;
    public SortDirection: string;
    public GetCount: boolean;
    public Tenant: number;
    public myTableDataList: any;
    public computingPartnerId: string;
    public objectTableId: string;
    public objectTableName: string;
    public ClinetName: string;
    public tenant2: number;
    public SearchingFields: string;
    public ComputingPartnerName: string;
    public AdditionalFilters: FilterItem[] = [];
    public ForceCacheRefresh: boolean = false;
    public IsClosedTable: boolean = false;
    public ParentObjectTableName: string;
    public ParentObjectTableId: string;
    addAdditionalFilter(
        FieldName: string,
        FieldValue: any,
        FieldValue2: any,
        FieldValue3: any,
        Operator: string,
        IsCustom: boolean,
        DisplayInList: boolean,
        IsCustomField: boolean,
        FieldDataType: string,
        IgnoreFilter: boolean = false,
        IsCacheOnClient: boolean = false) {

        if (!IsCacheOnClient) {
            if (typeof (FieldValue) === "string") {
                //var temp = encodeURIComponent("\"");
                FieldValue = encodeURIComponent(FieldValue)
                //FieldValue = FieldValue.replace("%22", "\%22");
            }
            if (typeof (FieldValue2) === "string") {
                FieldValue2 = encodeURIComponent(FieldValue2)
                //FieldValue = FieldValue.replace("%20", " ");
            }
            if (typeof (FieldValue3) === "string") {
                FieldValue3 = encodeURIComponent(FieldValue3)
                //FieldValue = FieldValue.replace("%20", " ");
            }
        }
        var existedItem = this.AdditionalFilters.find(d => d.FieldName == FieldName);
        if (!existedItem) {
            var item = new FilterItem(FieldName, FieldValue, FieldValue2, FieldValue3, Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType, IgnoreFilter, IsCacheOnClient);
            this.AdditionalFilters.push(item);
        }
    }

    removeAdditionalFilter(FieldName: string) {
        var item = this.AdditionalFilters.filter(d => d.FieldName == FieldName)[0];
        if (item) {
            var index = this.AdditionalFilters.indexOf(item);
            this.AdditionalFilters.splice(index, 1);
        }
    }

    public queryId: string;
    public queryCode: string;
    public uniqueCode: string;

    //public tenant: number;
    public userid: string;
    public ObjectTableName: string;

    public Filter1Name: string;
    public Filter1Value: Object;
    public Filter1Operator: string;
    public Filter1Value2: Object;

    public Filter2Name: string;
    public Filter2Value: Object;
    public Filter2Operator: string;
    public Filter2Value2: Object;

    public Filter3Name: string;
    public Filter3Value: Object;
    public Filter3Operator: string;
    public Filter3Value2: Object;

    public Filter4Name: string;
    public Filter4Value: Object;
    public Filter4Operator: string;
    public Filter4Value2: Object;

    public Filter5Name: string;
    public Filter5Value: Object;
    public Filter5Operator: string;
    public Filter5Value2: Object;

    public Filter6Name: string;
    public Filter6Value: Object;
    public Filter6Operator: string;
    public Filter6Value2: Object;

    public Filter7Name: string;
    public Filter7Value: Object;
    public Filter7Operator: string;
    public Filter7Value2: Object;

    public Filter8Name: string;
    public Filter8Value: Object;
    public Filter8Operator: string;
    public Filter8Value2: Object;

    public Filter9Name: string;
    public Filter9Value: Object;
    public Filter9Operator: string;
    public Filter9Value2: Object;

    public Filter10Name: string;
    public Filter10Value: Object;
    public Filter10Operator: string;
    public Filter10Value2: Object;

    public GetAll: boolean;


}

export class TranslationItem {
    public Id: string;
    public DefaultId: string;
    public OurCode: string;
    public PartnerCode: string;
    public DefaultTranslationCode: string;
    public DefaultTranslationPartnerCode: string;
    public Tenant: number;
    public ComputingPartnerId: string;
    public ComputingPartnerName: string;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public Name: string;
    public CreateDate: Date;
    public UpdateDate: Date; 
    public CreatedByUserId: string; 
    public UpdatedByUserId: string;
    public CreatedByUserName: string;
    public UpdatedByUserName: string;
    public CreatedDateDefault: Date;
    public UpdatedDateDefault: Date;
    public CreatedByUserNameDefault: string;
    public UpdatedByUserNameDefault: string;
    public SearchFields: string;
}

export class FilingInboxSummary {
    public EntityId: string;
    public EntityNumber: string;
    public FilingId: string;
    public DocumentTypeId: string;
    public ObjectTableName: string;
    public UserId: string;
    public IsDeleted: boolean;
    public Attaches: FilingInboxAttachItem[] = [];
}
export class FilingInboxAttachItem {
    public EntityId: string;
    public DocumentType: string;
    public House: string;
    public HouseNumber: string;
    public DocumentId: string;
    public FileName: string;
    public Description: string;
    public IsSharedWithAgent: boolean;
    public IsDigitallySign: boolean;
}
export class PartnersUploadExcelParameter {
    Tenant: number;
    FileData: string;
    FileName: string;
    IsConfirmationByUser: boolean;
    DocumentId: string;
}
