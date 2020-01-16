import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {AppTool} from '../../Infrastructure/Tools';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {CardPM} from '../EntityPMs/CardPM';
import {AddressPM} from '../EntityPMs/AddressPM';
import {ContactPM} from '../EntityPMs/ContactPM';
import {AgentPM} from '../EntityPMs/AgentPM';
import {CustomerPM} from '../EntityPMs/CustomerPM';
import {CustomAgentPM} from '../EntityPMs/CustomAgentPM';
import {ShippingAgentPM} from '../EntityPMs/ShippingAgentPM';
import { VendorPM } from '../EntityPMs/VendorPM';
import { AccountingPartnerPM } from '../EntityPMs/AccountingPartnerPM';
import {WarehousePM} from '../EntityPMs/WarehousePM';
import {AirlinePM} from '../EntityPMs/AirlinePM';
import {TruckerPM} from '../EntityPMs/TruckerPM';
import {ShippingLinePM} from '../EntityPMs/ShippingLinePM';
import {TarrifHeaderPM} from '../EntityPMs/TarrifHeaderPM';
import {TarrifChargePM}  from '../EntityPMs/TarrifChargePM';
import {TarrifFromToPM}  from '../EntityPMs/TarrifFromToPM';
import {CardExternalAccountsByProductPM}  from '../EntityPMs/CardExternalAccountsByProductPM';
import {CardList} from '../EntityLists/CardList';
import {AirlineList} from '../EntityLists/AirlineList';
import {CustomerList} from '../EntityLists/CustomerList';
import {CustomerListService} from './StandardLists/CustomerListService';
import {AgentPMService} from './StandardPMs/AgentPMService';
import {CustomerPMService} from './StandardPMs/CustomerPMService';
import {CustomAgentPMService} from './StandardPMs/CustomAgentPMService';
import {ShippingAgentPMService} from './StandardPMs/ShippingAgentPMService';
import {VendorPMService} from './StandardPMs/VendorPMService';
import {WarehousePMService} from './StandardPMs/WarehousePMService';
import {AirlinePMService} from './StandardPMs/AirlinePMService';
import {TruckerPMService} from './StandardPMs/TruckerPMService';
import {ShippingLinePMService} from './StandardPMs/ShippingLinePMService';
import {CustomerSalesNotePM} from '../EntityPMs/CustomerSalesNotePM';
import {CardExternalAccountsByProductPMService} from './StandardPMs/CardExternalAccountsByProductPMService';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import { CardContactAdditionalServicePM } from '../EntityPMs/CardContactAdditionalServicePM';
import { CarrierAreaList } from '../EntityLists/CarrierAreaList';
import { CarrierAreaPM } from '../EntityPMs/CarrierAreaPM';
import { CarrierAreasPortPM } from '../EntityPMs/CarrierAreasPortPM';

@Injectable()

export class PartnersDomainService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PartnersDomain';
    }

    GetAllowedAirlineId() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllowedAirlineId';

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAirlineRules(myAirlineCode: string, myMessageCode: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        var url = this._apiUrl + '/GetMessagingRulesForAirline?myAirlineCode=' + myAirlineCode + '&myMessageCode=' + myMessageCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<AirlineMessagingRuleList> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: AirlineMessagingRuleList = this.MapAirlineMessagingRuleList(listJason[itemJeson]);
                    listMapped.push(itemMapped);

                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAllAddressesPMsbyCardId(myCardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllAddressesPMsbyCardId?myCardId=' + myCardId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<AddressPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: AddressPM = this.MapAddressPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCustomerCardListByTenantVatNumber(vatNumber: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCustomerCardListByTenantVatNumber?vatNumber=' + vatNumber;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();
                var listMapped: Array<CardList> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: CardList = this.MapCardList(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    
    GetCustomerActualData(customerId: string, year: number, month: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomerActualData?customerId=' + customerId + '&year=' + year + '&month=' + month;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();
                return listJason;
            }).catch(ServiceHelper.HandleServiceError);
        });        
    }

    GetCustomerSalesNotes(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCustomerSalesNotes?entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<CustomerSalesNotePM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: CustomerSalesNotePM = this.MapCustomerSalesNotePM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAllContactsPMsbyCardId(myCardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllContactsPMsbyCardId?myCardId=' + myCardId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<ContactPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: ContactPM = this.MapContactPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetTarrifHeadersByCardIdAndTypeCode(cardId: string, typeCode: string, getAll: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTarrifHeadersByCardIdAndTypeCode?cardId=' + cardId + '&typeCode=' + typeCode + '&getAll=' + getAll;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<TarrifHeaderPM> = [];

                for (var key in allLists) {
                    var entity: TarrifHeaderPM;
                    entity = this.MapTarrifHeaderPM(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = _mappedListsArray;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetContactsByEmail(email: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetContactsByEmail?email=' + email;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<ContactPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: ContactPM = this.MapContactPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCardContactsByContact(contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCardContactsByContact?contactId=' + contactId;

        return Observable.defer(() => {
            return this._http.get(url, {
                headers: authHeader
            }).map(response => {
                var listJason = response.json();
                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCustomerProducts(customerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCustomerProducts?customerId=' + customerId;

        return Observable.defer(() => {
            return this._http.get(url, {
                headers: authHeader
            }).map(response => {
                var listJason = response.json();
                return listJason;
            }).catch(ServiceHelper.HandleServiceError);
        });

    }
    GetCustomerProductHistoryActualData(customerId: string, productTypeCode: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCustomerProductHistoryActualData?customerId=' + customerId + '&productTypeCode=' + productTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, {
                headers: authHeader
            }).map(response => {

                var listJason = response.json();

                var myResponse = new ServiceResponse();
                myResponse.Result = listJason;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    PostPartnerAddress(entityPM: PartnerServicePM) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: PartnerServicePM = this.MapJsonToPartnerAddress(entityPM, false);

            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: PartnerServicePM = this.MapJsonToPartnerAddress(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    Put(entityPM: PartnerExternalAccountsServicePM) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var mappedEntity: PartnerExternalAccountsServicePM = this.MapJsonToPartnerExternalAccounts(entityPM, false);

            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();

                var mappedResult: PartnerExternalAccountsServicePM = this.MapJsonToPartnerExternalAccounts(myJsonResult, true, entityPM);

                var myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
        
    GetCarrierUpdate(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCarrierUpdate?entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCarrierCopyToCurrentTenant(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCarrierCopyToCurrentTenant?entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetIsCustomerConnectedToEntities(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetIsCustomerConnectedToEntities?entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCardsForContact(contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCardsForContact?contactId=' + contactId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<CardPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: CardPM = this.MapCardPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                return listMapped;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetInUseCarrier( type: string, code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetInUseCarrier?type=' + type + '&code=' + code;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAirlineByPrefix(Prefix: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlineByPrefix?Prefix=' + Prefix;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var mappedResult: AirlineList;

                if (myJsonResult) {
                    mappedResult = new AirlineList();

                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAirlineByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlineByCode?code=' + code + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();

                var mappedResult: AirlinePM;

                if (myJsonResult) {
                    mappedResult = new AirlinePM();

                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAirlineByICAO(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlineByICAO?code=' + code + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetShippingLineByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetShippingLineByCode?code=' + code + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetTruckerByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetTruckerByCode?code=' + code + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetWarehouseByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetWarehouseByCode?code=' + code + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetBillingAddressListByCardId(cardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetBillingAddressListByCardId?cardId=' + cardId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }   
    GetMainAddressListByCardId(cardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMainAddressListByCardId?cardId=' + cardId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetBillingOrMainAddressListByCardId(cardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetBillingOrMainAddressListByCardId?cardId=' + cardId;
        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetAddressByCardAndType(cardId: string, type: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAddressByCardAndType?cardId=' + cardId + '&type=' + type;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCustomerById(id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCustomerById?id=' + id;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var mappedResult: CustomerPM;

                if (myJsonResult) {
                    mappedResult = new CustomerPM();

                    var myCustomerPMService = new CustomerPMService();                   
                    mappedResult = myCustomerPMService.MapJsonToEntityPM(myJsonResult);                    
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetIsVATUniqueForCustomer(vatNumber: string, customerId: string, countryId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetIsVATUniqueForCustomer?vatNumber=' + vatNumber + '&customerId=' + customerId + '&countryId=' + countryId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCardExternalAccountsByProducts(myCardId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCardExternalAccountsByProducts?myCardId=' + myCardId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    GetCustomersQuickSearch(filters: ApiQueryFilters) {

        var urlparameters = '/GetCustomersQuickSearch?';
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
        var callUrl = this._apiUrl.concat(urlparameters);//

        return Observable.defer(() => {
            return this._http.get(callUrl, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<CustomerList> = [];

                var myService = new CustomerListService();

                for (var key in allLists) {
                    var entity: CustomerList;
                    entity = myService.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = _mappedListsArray;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: CardList;
        entityList = new CardList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    }
    MapCardPM(jsonList: any) {
        var entityPM: CardPM = new CardPM();

        if (jsonList) {
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }

        entityPM.IsDirty = false;
        return entityPM;
    }

    MapCardList(jsonList: any) {
        var entityPM: CardList = new CardList();

        if (jsonList) {
            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }

        return entityPM;
    }
    MapAddressPM(jsonList: any) {
        var entityPM: AddressPM = null;

        if (jsonList) {
            entityPM = new AddressPM();

            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];

                if (property === "UIProperties" || property === "entityParentPM") {
                    continue;
                }

                entityPM[property] = jsonList[property];
            }

            entityPM.IsDirty = false;
        }
        
        return entityPM;
    }
    MapContactPM(jsonList: any, mapParent: boolean = true) {
        var entityPM: ContactPM = null;

        if (jsonList) {
            entityPM = new ContactPM();

            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];

                if (property === "UIProperties" || property === "entityParentPM") {
                    continue;
                }

                entityPM[property] = jsonList[property];
            }

            if (!AppTool.IsNullOrEmpty(entityPM.CardId)) {
                var oldContactServices: CardContactAdditionalServicePM[] = [];
                if (entityPM.OldEntityPM && !mapParent) {
                    oldContactServices = entityPM.OldEntityPM.CardContactAdditionalServices;
                }

                entityPM.CardContactAdditionalServices = new Array<CardContactAdditionalServicePM>();
                for (var item in jsonList.CardContactAdditionalServices) {

                    var jItem = jsonList.CardContactAdditionalServices[item];
                    if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                        continue;
                    }
                    var newServicePM: CardContactAdditionalServicePM;
                    if (mapParent) {
                        newServicePM = new CardContactAdditionalServicePM(entityPM);
                    }
                    else {
                        newServicePM = new CardContactAdditionalServicePM(null);
                    }

                    var pmKeysArray = Object.keys(jItem);
                    for (var pmKey in pmKeysArray) {

                        if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                            continue;
                        }
                        var pmProperty = pmKeysArray[pmKey];
                        newServicePM[pmProperty] = jItem[pmProperty];
                    }

                    newServicePM.IsDirty = false;

                    if (mapParent) {
                        newServicePM.OldEntityPM = this.clone(newServicePM);
                        newServicePM.UniqueKey = Guid.newGuid();
                        newServicePM.ChangeSetOp = "None";
                        jItem.ChangeSetOp = "None";

                    }
                    else {

                        if (newServicePM.UniqueKey) {

                            if (jItem.IsDirty)
                                newServicePM.ChangeSetOp = "Update";
                        }
                        else {
                            newServicePM.ChangeSetOp = "Insert";
                        }

                        newServicePM.OldEntityPM = null;
                        newServicePM.EntityParentPM = null;
                    }


                    entityPM.CardContactAdditionalServices.push(newServicePM);
                }

                if (oldContactServices) {

                    for (var itemKey in oldContactServices) {
                        if (entityPM.CardContactAdditionalServices.filter(p => p.UniqueKey === oldContactServices[itemKey].UniqueKey).length === 0) {

                            if (oldContactServices[itemKey]) {
                                oldContactServices[itemKey].ChangeSetOp = "Delete";
                                entityPM.CardContactAdditionalServices.push(oldContactServices[itemKey]);
                            }
                        }
                    }
                }
            }
            
            entityPM.IsDirty = false;

            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
                entityPM.OldEntityPM.CardContactAdditionalServices = [];
                for (var m in entityPM.CardContactAdditionalServices) {
                    entityPM.OldEntityPM.CardContactAdditionalServices.push(this.clone(entityPM.CardContactAdditionalServices[m]));
                }
            }
            else {
                entityPM.OldEntityPM = null;
            }
        }
        
        return entityPM;
    }
    
    MapCustomerSalesNotePM(jsonList: any) {
        var entityPM: CustomerSalesNotePM = null;

        if (jsonList) {
            entityPM = new CustomerSalesNotePM(null);

            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];

                if (property === "UIProperties" || property === "entityParentPM") {
                    continue;
                }

                entityPM[property] = jsonList[property];
            }

            entityPM.IsDirty = false;
        }

        return entityPM;
    }
    
    MapTarrifHeaderPM(jsonPM: any, mapParent: boolean = true, entityPM: TarrifHeaderPM = null) {


        if (!entityPM) {

            entityPM = new TarrifHeaderPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldTarrifCharges: TarrifChargePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldTarrifCharges = entityPM.OldEntityPM.TarrifCharges;
        }


        entityPM.TarrifCharges = new Array<TarrifChargePM>();
        for (var item in jsonPM.TarrifCharges) {

            var jItem = jsonPM.TarrifCharges[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newTarrifChargePM: TarrifChargePM;
            if (mapParent) {
                newTarrifChargePM = new TarrifChargePM(entityPM);
            }
            else {
                newTarrifChargePM = new TarrifChargePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newTarrifChargePM[pmProperty] = jItem[pmProperty];
            }
            newTarrifChargePM.IsDirty = false;
            if (mapParent) {
                newTarrifChargePM.OldEntityPM = this.clone(newTarrifChargePM);
                newTarrifChargePM.UniqueKey = Guid.newGuid();
                newTarrifChargePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newTarrifChargePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newTarrifChargePM.ChangeSetOp = "Update";
                }
                else {
                    newTarrifChargePM.ChangeSetOp = "Insert";
                }

                newTarrifChargePM.OldEntityPM = null;
                newTarrifChargePM.EntityParentPM = null;
            }


            entityPM.TarrifCharges.push(newTarrifChargePM);
        }

        if (oldTarrifCharges) {

            for (var itemKey in oldTarrifCharges) {
                if (entityPM.TarrifCharges.filter(p => p.UniqueKey === oldTarrifCharges[itemKey].UniqueKey).length === 0) {

                    if (oldTarrifCharges[itemKey]) {
                        oldTarrifCharges[itemKey].ChangeSetOp = "Delete";
                        entityPM.TarrifCharges.push(oldTarrifCharges[itemKey]);
                    }
                }
            }
        }

        var oldTarrifFromToes: TarrifFromToPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldTarrifFromToes = entityPM.OldEntityPM.TarrifFromToes;
        }


        entityPM.TarrifFromToes = new Array<TarrifFromToPM>();
        for (var item in jsonPM.TarrifFromToes) {

            var jItem = jsonPM.TarrifFromToes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newTarrifFromToPM: TarrifFromToPM;
            if (mapParent) {
                newTarrifFromToPM = new TarrifFromToPM(entityPM);
            }
            else {
                newTarrifFromToPM = new TarrifFromToPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newTarrifFromToPM[pmProperty] = jItem[pmProperty];
            }
            newTarrifFromToPM.IsDirty = false;
            if (mapParent) {
                newTarrifFromToPM.OldEntityPM = this.clone(newTarrifFromToPM);
                newTarrifFromToPM.UniqueKey = Guid.newGuid();
                newTarrifFromToPM.ChangeOp = "None";
                jItem.ChangeOp = "None";

            }
            else {

                if (newTarrifFromToPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newTarrifFromToPM.ChangeOp = "Update";
                }
                else {
                    newTarrifFromToPM.ChangeOp = "Insert";
                }

                newTarrifFromToPM.OldEntityPM = null;
                newTarrifFromToPM.EntityParentPM = null;
            }


            entityPM.TarrifFromToes.push(newTarrifFromToPM);
        }

        if (oldTarrifFromToes) {

            for (var itemKey in oldTarrifFromToes) {
                if (entityPM.TarrifFromToes.filter(p => p.UniqueKey === oldTarrifFromToes[itemKey].UniqueKey).length === 0) {

                    if (oldTarrifFromToes[itemKey]) {
                        oldTarrifFromToes[itemKey].ChangeOp = "Delete";
                        entityPM.TarrifFromToes.push(oldTarrifFromToes[itemKey]);
                    }
                }
            }
        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.TarrifCharges = [];
            for (var m in entityPM.TarrifCharges) {
                entityPM.OldEntityPM.TarrifCharges.push(this.clone(entityPM.TarrifCharges[m]));
            }
            entityPM.OldEntityPM.TarrifFromToes = [];
            for (var m in entityPM.TarrifFromToes) {
                entityPM.OldEntityPM.TarrifFromToes.push(this.clone(entityPM.TarrifFromToes[m]));
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }
    MapAirlineMessagingRuleList(jsonList: any) {
        var entityList: AirlineMessagingRuleList;
        entityList = new AirlineMessagingRuleList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    private MapJsonToPartnerAddress(jsonPM: any, getCallMap: boolean = true, entity: PartnerServicePM = null) {
        if (!entity) {
            entity = new PartnerServicePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else if (property === "Agent") {
                if (jsonPM[property]) {
                    var myAgentPMService = new AgentPMService();
                    entity[property] = myAgentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "Customer") {
                if (jsonPM[property]) {
                    var myCustomerPMService = new CustomerPMService();
                    entity[property] = myCustomerPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "CustomAgent") {
                if (jsonPM[property]) {
                    var myCustomAgentPMService = new CustomAgentPMService();
                    entity[property] = myCustomAgentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "ShippingAgent") {
                if (jsonPM[property]) {
                    var myShippingAgentPMService = new ShippingAgentPMService();
                    entity[property] = myShippingAgentPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "Vendor") {
                if (jsonPM[property]) {
                    var myVendorPMService = new VendorPMService();
                    entity[property] = myVendorPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "Warehouse") {
                if (jsonPM[property]) {
                    var myWarehousePMService = new WarehousePMService();
                    entity[property] = myWarehousePMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "Airline") {
                if (jsonPM[property]) {
                    var myAirlinePMService = new AirlinePMService();
                    entity[property] = myAirlinePMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "Trucker") {
                if (jsonPM[property]) {
                    var myTruckerPMService = new TruckerPMService();
                    entity[property] = myTruckerPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "ShippingLine") {
                if (jsonPM[property]) {
                    var myShippingLinePMService = new ShippingLinePMService();
                    entity[property] = myShippingLinePMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
                }
            }

            else if (property === "Address") {
                if (jsonPM[property]) {
                    entity[property] = this.MapAddressPM(jsonPM[property]);
                }
            }

            else if (property === "Contact") {
                if (jsonPM[property]) {
                    entity[property] = this.MapContactPM(jsonPM[property], getCallMap);
                }
            }

            else {
                entity[property] = jsonPM[property];
            }
        }

        return entity;
    }
    private MapJsonToPartnerExternalAccounts(jsonPM: any, getCallMap: boolean = true, entityPM: PartnerExternalAccountsServicePM = null) {
        if (!entityPM) {
            entityPM = new PartnerExternalAccountsServicePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];

            if (property === "UIProperties") {
                continue;
            }

            else if (property === "Items") {
                var myPMService = new CardExternalAccountsByProductPMService();
                entityPM.Items = new Array<CardExternalAccountsByProductPM>();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];

                    var newItemPM: CardExternalAccountsByProductPM;
                    newItemPM = myPMService.MapJsonToEntityPM(jItem, getCallMap);
                    entityPM.Items.push(newItemPM);
                }
            }

            else {
                entityPM[property] = jsonPM[property];
            }
        }

        return entityPM;
    }
    private clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }
    public SetPartner(args: PartnerServicePM, myPartner: any, partnerTypeId: string = null) {
        if (myPartner) {

            if (partnerTypeId)
            {
                args.PartnerTypeId = partnerTypeId;
            }

            else {
                args.PartnerTypeId = myPartner.PartnerTypeId;
                args.IsPartnerDirty = myPartner.IsDirty;
            }

            if (AppTool.IsNullOrEmpty(args.PartnerTypeId)) {

                if (myPartner instanceof AirlinePM) {
                    args.PartnerTypeId = "AL";
                }

                else if (myPartner instanceof TruckerPM) {
                    args.PartnerTypeId = "TR";
                }

                else if (myPartner instanceof ShippingLinePM) {
                    args.PartnerTypeId = "SL";
                }                
            }          

            switch (args.PartnerTypeId) {
                case "CS":
                case "PO": {
                    args.Customer = myPartner;
                    break;
                }

                case "AG": {
                    args.Agent = myPartner;
                    break;
                }

                case "CG": {
                    args.CustomAgent = myPartner;
                    break;
                }

                case "SG": {
                    args.ShippingAgent = myPartner;
                    break;
                }

                case "VD": {
                    args.Vendor = myPartner;
                    break;
                }

                case "WH": {
                    args.Warehouse = myPartner;
                    break;
                }

                case "AL": {
                    args.Airline = myPartner;
                    break;
                }

                case "SL": {
                    args.ShippingLine = myPartner;
                    break;
                }

                case "TR": {
                    args.Trucker = myPartner;
                    break;
                }
            }
        }
    }

    GetRecentCustomers(ownerId: string, businessUnitId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRecentCustomers?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCustomersCounts(ownerId: string, businessUnitId: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersCounts?ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                var myResult = new CRMSummary();

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

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCustomersDecreasedShipments(dataTypeCode: string, startDate: Date, timeRange: string, ownerId: string, businessUnitId: string, RecordsTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCustomersDecreasedShipments?dataTypeCode=' + dataTypeCode + '&startDate=' + ServiceHelper.GetDateString(startDate) + '&timeRange=' + timeRange + '&ownerId=' + ownerId + '&businessUnitId=' + businessUnitId + '&RecordsTypeCode=' + RecordsTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var listJason = response.json();
                var listMapped: Array<CompareDataClass> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: CompareDataClass = this.MapCompareDataClass(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var myResponse: ServiceResponse;
                myResponse = new ServiceResponse();
                myResponse.Result = listMapped;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapCompareDataClass(jsonList: any) {
        var entityList: CompareDataClass;
        entityList = new CompareDataClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    GetAirlinesBySearchTextAndTenant(AWBMessagesCCSTypeCode: string, searchText: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAirlinesBySearchTextAndTenant?AWBMessagesCCSTypeCode=' + AWBMessagesCCSTypeCode + '&searchText=' + searchText + '&tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var allLists = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    AllowAirline(isAllowed: boolean, code: string, myTenantId: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllowAirline?isAllowed=' + isAllowed + "&code=" + code + "&myTenantId=" + myTenantId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetIsDirect(forwarderTenantId: number, airlineTenantId: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetIsDirect?forwarderTenantId=' + forwarderTenantId + "&airlineTenantId=" + airlineTenantId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    RegistrationRequested(isRequested: boolean, tenantAirlineId: string, zeroAirlineId: string, tenantManagmentId: number, AWBMessagesCCSTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRegistrationRequested?isRequested=' + isRequested + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    RegisteringAirline(isRegistered: boolean, tenantAirlineId: string, zeroAirlineId: string, tenantManagmentId: number, AWBMessagesCCSTypeCode: string, loggedContactName: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRegisteringAirline?isRegistered=' + isRegistered + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode + "&loggedContactName=" + loggedContactName;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    SetIsDirect(isDirect: boolean, tenantAirlineId: string, zeroAirlineId: string, tenantManagmentId: number, AWBMessagesCCSTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSetIsDirect?isDirect=' + isDirect + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    SetIsDeclined(isDeclined: boolean, declineNotes: string, tenantAirlineId: string, zeroAirlineId: string, tenantManagmentId: number, AWBMessagesCCSTypeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetSetIsDeclined?isDeclined=' + isDeclined + "&declineNotes=" + declineNotes + "&tenantAirlineId=" + tenantAirlineId + "&zeroAirlineId=" + zeroAirlineId + "&tenantManagmentId=" + tenantManagmentId + "&AWBMessagesCCSTypeCode=" + AWBMessagesCCSTypeCode;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAirlinesForRequestedTenant(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');

        var url = this._apiUrl + '/GetAirlinesForRequestedTenant?tenant=' + tenant;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<AirlineList> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: AirlineList;
                        entity = this.MapJsonToAirlineList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToAirlineList(jsonList: any) {
        var entityList: AirlineList;
        entityList = new AirlineList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }

    GetAirlinesByFiltersAndTenant(filters: ApiQueryFilters, tenant: number) {   
        var urlparameters = this._apiUrl + '/GetAirlinesByFiltersAndTenant?';
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

        urlparameters = urlparameters.concat("&tenant=" + tenant);

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(urlparameters, { headers: authHeader }).map(response => {
                var allLists = response.json();
                var _mappedListsArray: Array<AirlineList> = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity: AirlineList;
                        entity = this.MapJsonToAirlineList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }   

    GetCustomerCreditLimitActualAmount(myCustomerId: string, invoiceId: string = null) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var myapiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';

        var url = myapiUrl + '/GetCustomerCreditLimitActualAmount?myCustomerId=' + myCustomerId + "&invoiceId=" + invoiceId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var myResult = response.json();

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCardContactProducts(cardId: string, contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetCardContactProducts?cardId=' + cardId + "&contactId=" + contactId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAllCarrierAreasByCarrierId(carrierId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetAllCarrierAreasByCarrierId?carrierId=' + carrierId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var listJason = response.json();
                var listMapped: Array<CarrierAreaPM> = [];

                for (var itemJeson in listJason) {
                    var itemMapped: CarrierAreaPM = this.MapCarrierAreaPM(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    MapCarrierAreaPM(jsonList: any, mapParent: boolean = true) {
        var entityPM: CarrierAreaPM = null;

        if (jsonList) {
            entityPM = new CarrierAreaPM();

            var jsonListKeys = Object.keys(jsonList);

            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];

                if (property === "UIProperties" || property === "entityParentPM") {
                    continue;
                }

                entityPM[property] = jsonList[property];
            }

            var oldContactServices: CarrierAreasPortPM[] = [];
            if (entityPM.OldEntityPM && !mapParent) {
                oldContactServices = entityPM.OldEntityPM.CarrierAreasPorts;
            }

            entityPM.CarrierAreasPorts = new Array<CarrierAreasPortPM>();
            for (var item in jsonList.CarrierAreasPorts) {

                var jItem = jsonList.CarrierAreasPorts[item];
                if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                    continue;
                }
                var newServicePM: CarrierAreasPortPM;
                if (mapParent) {
                    newServicePM = new CarrierAreasPortPM(entityPM);
                }
                else {
                    newServicePM = new CarrierAreasPortPM(null);
                }

                var pmKeysArray = Object.keys(jItem);
                for (var pmKey in pmKeysArray) {

                    if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                        continue;
                    }
                    var pmProperty = pmKeysArray[pmKey];
                    newServicePM[pmProperty] = jItem[pmProperty];
                }

                newServicePM.IsDirty = false;

                if (mapParent) {
                    newServicePM.OldEntityPM = this.clone(newServicePM);
                    newServicePM.UniqueKey = Guid.newGuid();
                    newServicePM.ChangeSetOp = "None";
                    jItem.ChangeSetOp = "None";

                }
                else {

                    if (newServicePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newServicePM.ChangeSetOp = "Update";
                    }
                    else {
                        newServicePM.ChangeSetOp = "Insert";
                    }

                    newServicePM.OldEntityPM = null;
                    newServicePM.EntityParentPM = null;
                }


                entityPM.CarrierAreasPorts.push(newServicePM);
            }

            if (oldContactServices) {

                for (var itemKey in oldContactServices) {
                    if (entityPM.CarrierAreasPorts.filter(p => p.UniqueKey === oldContactServices[itemKey].UniqueKey).length === 0) {

                        if (oldContactServices[itemKey]) {
                            oldContactServices[itemKey].ChangeSetOp = "Delete";
                            entityPM.CarrierAreasPorts.push(oldContactServices[itemKey]);
                        }
                    }
                }
            }

            entityPM.IsDirty = false;

            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
                entityPM.OldEntityPM.CarrierAreasPorts = [];
                for (var m in entityPM.CarrierAreasPorts) {
                    entityPM.OldEntityPM.CarrierAreasPorts.push(this.clone(entityPM.CarrierAreasPorts[m]));
                }
            }
            else {
                entityPM.OldEntityPM = null;
            }
        }

        return entityPM;
    }

    RemoveAreaFromCarrier(areaId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetRemoveCarrierAreaFromCarrier?areaId=' + areaId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var done: string = response.json();

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = done;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}
export class AirlineMessagingRuleList {
    Id: string;
    Tenant: number;
    AirlineId: string;
    MessageTypeCode: string;
    RuleFieldId: string;
    RuleFieldCode: string;
    IsMandatoryForSending: boolean;
    MaxSize: number;
    InActive: boolean;
    RuleFieldName: string;
    AirlineCode: string;
}
export class PartnerServicePM {
    public Tenant: number;
    public AddressId: string = null;
    public ContactId: string = null;
    public PartnerId: string = null;
    public PartnerTypeId: string = null;
    public IsAddressDirty: boolean = false;
    public IsContactDirty: boolean = false;
    public IsPartnerDirty: boolean = false;
    public Address: AddressPM = null;
    public Contact: ContactPM = null;
    public Agent: AgentPM = null;
    public Customer: CustomerPM = null;
    public CustomAgent: CustomAgentPM = null;
    public ShippingAgent: ShippingAgentPM = null;
    public Vendor: VendorPM = null;
    public Warehouse: WarehousePM = null;
    public Airline: AirlinePM = null;
    public ShippingLine: ShippingLinePM = null;
    public Trucker: TruckerPM = null;
    public AccountingPartner: AccountingPartnerPM = null;

}
export class PartnerExternalAccountsServicePM {
    public Tenant: number;
    public CardId: string = null;
    public BusinessArea: string = null;
    public ObjectTableName: string = null;
    public ExternalId2: string = null;
    public Items: CardExternalAccountsByProductPM[] = [];
}
export class CRMSummary {
    public Id: number;
    public MyOpenDataCount: number;
    public MyOpenAsAccountManagerDataCount: number;
    public Customers_Waiting: number;
    public Customers_Potential: number;
    public Customers_Active: number;
    public Customers_Inactive: number;
}
export class CompareDataClass {
    public Id: string;
    public EntityName: string;
    public RankCode: string;
    public RankName: string;

    public TEU_New: number;
    public Revenue_New: number;
    public ChargeableWeight_New: number;
    public NumberOfShipments_New: number;

    public TEU_Old: number;
    public Revenue_Old: number;
    public ChargeableWeight_Old: number;
    public NumberOfShipments_Old: number;

    public TEU: number;
    public Revenue: number;
    public ChargeableWeight: number;
    public NumberOfShipments: number;
}
