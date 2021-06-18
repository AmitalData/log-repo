import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
;
import { defer, of } from 'rxjs';
import { TenantManagementList } from '../../Infrastructure/EntityLists/TenantManagementList';
import { BatchServicesDefinitionPM } from '../../Infrastructure/EntityPMs/BatchServicesDefinitionPM';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import { TenantManagementPM } from '../../Infrastructure/EntityPMs/TenantManagementPM';
import { TenantManagementJS } from '../../Infrastructure/DataContracts/TenantManagementJS';
import { ObjectsUpdater } from '../../Infrastructure/Locators/ObjectsUpdater';

@Injectable()

export class GlobalDomainService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GlobalDomain';
    }

    GetMessagingStockTenantsList(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMessagingStockTenantsList?tenant=' + tenant;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<TenantManagementList> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: TenantManagementList = this.MapTenantManagementList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    GetQuickBooksOnlineVatTypesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {
         


            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineVatTypesById?Id=' + Id ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetQuickBooksOnlineReceivableChargesTypesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineReceivableChargesTypesById?Id=' + Id,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetQuickBooksOnlinePayablesChargesTypesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlinePayablesChargesTypesById?Id=' + Id,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetQuickBooksOnlinePaymentMethodsById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlinePaymentMethodsById?Id=' + Id,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    GetQuickBooksOnlinePaymentTermsById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlinePaymentTermsById?Id=' + Id,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetQuickBooksOnlineCurrenciesById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineCurrenciesById?Id=' + Id,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    GetQuickBooksOnlineCustomerById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineCustomersById?Id=' + Id,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    GetQuickBooksOnlineVendorById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';


        return defer(() => {



            return this._http.get(this._apiUrl + '/GetQuickBooksOnlineVendorById?Id=' + Id,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    GetAccountingSystem(AccountingSystemCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAccountingSystem?AccountingSystemCode=' + AccountingSystemCode;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResultJason = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResultJason;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    InvoiceToQuickBooks(invoiceId: string, id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';        
 

        return defer(() => {

            return this._http.get(this._apiUrl + '/GetInvoiceToQuickBooks?Customerid=' + id + '&invoiceId=' + invoiceId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {     
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetQuickBooksQueries(args: any,SearchText:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuickbooksDomain';
        return defer(() => {

            return this._http.get(this._apiUrl + '/GetQuickBooksQueries?CardName=' + args.CardName + '&SearchField=' + args.SearchField + '&SearchText=' + SearchText + '&ReceivableCard=' + args.ReceivableCard + '&PayableCard=' + args.PayableCard + '&LogitudeCardName=' + args.LogitudeCardName,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    MapTenantManagementList(jsonList: any) {
        var entityList: TenantManagementList;
        entityList = new TenantManagementList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    
    GetAllHelpResources() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAllHelpResources?',ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAirlineTenantExistsForAirline(code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlineTenantExistsForAirline?code=' + code;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResultJason = response;

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResultJason;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAllBatchServicesDefinitionsPMs(filterByDateCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllBatchServicesDefinitionsPMs?filterByDateCode=' + filterByDateCode;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<BatchServicesDefinitionPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: BatchServicesDefinitionPM = this.MapBatchServicesDefinitionPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapBatchServicesDefinitionPM(jsonList: any) {
        var entityList: BatchServicesDefinitionPM;
        entityList = new BatchServicesDefinitionPM();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    UpdateTenantZeroService(Message:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUpdateTenantZeroService?Message=' + Message;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {
                var itemJason: Boolean = response.body;
         
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = itemJason;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetParentTenants() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetParentTenants?';

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var listJason = response;
                var listMapped: Array<TenantManagementList> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: TenantManagementList = this.MapTenantManagementList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetTenantManagementJS(LoggedUserId:string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTenantManagementJS?loggeduserid=' + LoggedUserId;

        return defer(() => {
            return this._http.get(url,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var iResultJson = response;
                var iResultMapped: TenantManagementJS;

                if (iResultJson) {
                    iResultMapped = this.MapTenantManagementJS(iResultJson);
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = iResultMapped;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapTenantManagementJS(jsonList: any) {

        var entityList: TenantManagementJS = new TenantManagementJS();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
    UpdateTenantManagementJS(entityPM: TenantManagementPM) {
        var myResult: TenantManagementJS = new TenantManagementJS();
        myResult.Id = entityPM.Id;
        myResult.Name = entityPM.Name;
        myResult.PackageCode = entityPM.PackageCode;
        myResult.PackageName = entityPM.PackageName;
        myResult.TTY = entityPM.TTY;
        myResult.PIMA = entityPM.PIMA;
        myResult.AWBMessagesCCSTypeCode = entityPM.AWBMessagesCCSTypeCode;
        myResult.IsAWBStockPrepaid = entityPM.IsAWBStockPrepaid;
        myResult.TrialStartDate = entityPM.TrialStartDate;
        myResult.TrialEndDate = entityPM.TrialEndDate;
        myResult.PaidUntilDate = entityPM.PaidUntilDate;
        myResult.PrivateLabelId = entityPM.PrivateLabelId;
        myResult.PaymentFailure = entityPM.PaymentFailure;
        myResult.SuspendDate = entityPM.SuspendDate;
        myResult.IsTrial = entityPM.IsTrial;
        myResult.IsRecurring = entityPM.IsRecurring;
        myResult.IsEAWBOnlyDemo = entityPM.IsEAWBOnlyDemo;
        myResult.IsRestrictedByAirline = entityPM.IsRestrictedByAirline;
        myResult.IsCargonautEnabled = entityPM.IsCargonautEnabled;
        myResult.IsDEXXConnectionEnabled = entityPM.IsDEXXConnectionEnabled;
        myResult.ManageLicencesPerUser = entityPM.ManageLicencesPerUser;
        myResult.ChangeHeaderColor = entityPM.ChangeHeaderColor;
        myResult.TrailDaysLeft = entityPM.TrailDaysLeft;
        myResult.PaidDaysLeft = entityPM.PaidDaysLeft;
        myResult.SuspendDaysLeft = entityPM.SuspendDaysLeft;
        myResult.NumberOfUsers = entityPM.NumberOfUsers;
        myResult.BluesnapContractId = entityPM.BluesnapContractId;
        myResult.BluesnapAccount = entityPM.BluesnapAccount;
        myResult.ManagesRegisteredAgent = entityPM.ManagesRegisteredAgent;
        myResult.IsINTTRAOnlyDemo = entityPM.IsINTTRAOnlyDemo;
        myResult.IsMultiPackage = entityPM.IsMultiPackage;
        myResult.TemporalPackageCode = entityPM.TemporalPackageCode;
        myResult.PackagesCodes_PK = entityPM.PackagesCodes_PK;
        myResult.PackagesCodes_BS = entityPM.PackagesCodes_BS;
        myResult.TenantManagementLicenses = entityPM.TenantManagementLicenses;
        ObjectsUpdater.UpdateTenantManagementJS(myResult);
    }

    GetOceanInsightGlobalSetting() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetOceanInsightGlobalSetting';
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var iResultJson = response;
                var iResultMapped: OceanInsightGlobalSetting;
                if (iResultJson) {
                    iResultMapped = this.MapOceanInsightGlobalSetting(iResultJson);
                }
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = iResultMapped;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapOceanInsightGlobalSetting(jsonList: any) {
        var entityList: OceanInsightGlobalSetting = new OceanInsightGlobalSetting();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    }

    UpdateOceanInsightGlobalSetting(oceanInsightGlobalSetting: OceanInsightGlobalSetting) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetUpdateOceanInsightGlobalSetting?oceanInsightGlobalSetting=' + oceanInsightGlobalSetting;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpResponse<any>) => {
                var itemJason: Boolean = response.body;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = itemJason;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class UploadFileArgs {
    Tenant: number;
    FileData: string;    
    FileName: string;
}

export class OceanInsightGlobalSetting {
    OITenantNumber: number;
    AmitalCloudEnvironmentURL: string;
    AmitalCloudLogitudeTenantPrimaryKey: string;
}
