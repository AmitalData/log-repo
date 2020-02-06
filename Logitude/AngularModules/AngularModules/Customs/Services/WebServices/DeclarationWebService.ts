/*
* Contains a functions used in customs answers like:
*  GetDeclarationErrors, constraints , ....
*/
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {DeclarationList} from '../../EntityLists/DeclarationList';
import {DeclarationErrorView} from '../../EntityPMs/Extended/DeclarationErrorView';
import {DeclarationCorrectionView} from '../../EntityPMs/Extended/DeclarationCorrectionView';
import {SupplierInvoicePM} from '../../EntityPMs/SupplierInvoicePM';
import {CertificateTicket} from '../../DataContract/CertificateTicket';
import {DeclarationPaymentPM} from '../../EntityPMs/DeclarationPaymentPM';
import {SupplierInvoiceItemPM} from '../../EntityPMs/SupplierInvoiceItemPM';
import {SupplierInvioceItemCertificatPM} from '../../EntityPMs/SupplierInvioceItemCertificatPM';
import {SupplierInvoiceModificationPM} from '../../EntityPMs/SupplierInvoiceModificationPM';
import {SupplierInvoiceFreightAmountPM} from '../../EntityPMs/SupplierInvoiceFreightAmountPM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {GenericRequestParams} from '../../DataContract/RequestParams/GenericRequestParams';
import { CargoSealsRequestParams } from '../../DataContract/RequestParams/CargoSealsRequestParams';
import {SupplierInvoicePMService} from '../../Services/StandardPMs/SupplierInvoicePMService';
import {DeclarationPaymentPMService} from '../../Services/StandardPMs/DeclarationPaymentPMService';

import {CustomsCollateralPM} from '../../EntityPMs/CustomsCollateralPM';
import {CollateralsRequestFileCondPM} from '../../EntityPMs/CollateralsRequestFileCondPM';
import {CustomsCollateralsAnswerPM} from '../../EntityPMs/CustomsCollateralsAnswerPM';
import {CustomsCollateralsConditionPM} from '../../EntityPMs/CustomsCollateralsConditionPM';

@Injectable()

export class DeclarationWebService {
    private _http: Http
    private _apiUrl: string;

    _SupplierInvoicePMService: SupplierInvoicePMService = new SupplierInvoicePMService();
    _DeclarationPaymentPMService: DeclarationPaymentPMService = new DeclarationPaymentPMService();
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationWebService';



    }

    //customs answers
    GetDeclarationConstraintsByDeclrationId(declarationId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationConstraintsByDeclrationId/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }
    GetDeclarationErrors(declarationId: string, listVersionId: string,courierFilter:string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationErrors/?declarationId=" + declarationId
                + "&listVersionId=" + listVersionId + "&courierFilter=" + courierFilter, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<DeclarationErrorView> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: DeclarationErrorView;
                        entity = this.MapJsonToEntity(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }
    GetSingleCustomsCollateral(id: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSingleCustomsCollateral/?id=" + id, {
                    headers: authHeader
                }).map(response => {

                    var entity: CustomsCollateralPM;
                    var pm = response.json();
                    if (pm) {
                        entity = this.MapJsonToCustomsCollateralPM(pm);
                    }

                    
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }
    CheckIfDocumentPointerExistsForConstraint(constraintNumber: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCheckIfDocumentPointerExistsForConstraint/?constraintNumber=" + constraintNumber, {
                headers: authHeader
            }).map(response => {

                var res = response.json();
               
                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    //customs ansewers -> supplier invoices
    GetSupplierInvoiceBySequenceNumber(declarationId: string, invoiceSequence: number, skip: number, take: number) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSupplierInvoiceBySequenceNumber/?declarationId=" + declarationId
                + "&invoiceSequence=" + invoiceSequence
                + "&skip=" + skip
                + "&take=" + take
                , {
                headers: authHeader
            }).map(response => {

                var res = response.json();
                if (res) {
                    res = this._SupplierInvoicePMService.MapJsonToEntityPM(res, true);
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }
    GetSupplierInvoiceWithItemBySequenceNumber(declarationId: string, invoiceSequence: number, itemSequence: number, skip: number, take: number, type:string = null) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSupplierInvoiceWithItemBySequenceNumber/?declarationId=" + declarationId
                + "&invoiceSequence=" + invoiceSequence
                + "&itemSequence=" + itemSequence
                + "&skip=" + skip
                + "&take=" + take
                + "&type=" + type
                , {
                    headers: authHeader
                }).map(response => {

                    var res = response.json();
                    if(res)
                        res = this._SupplierInvoicePMService.MapJsonToEntityPM(res, true);

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = res;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

  


    GetSupplierInvoiceWithSpecificItemByCounterKey(declarationId: string, counterKey: number, itemSequence: number) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSupplierInvoiceWithSpecificItemByCounterKey/?declarationId=" + declarationId
                + "&counterKey=" + counterKey
                + "&itemSequence=" + itemSequence
               
                , {
                    headers: authHeader
                }).map(response => {

                    var res = response.json();
                    //res = this.MapJsonToEntityPM(res, true);
                    if(res)
                        res = this._SupplierInvoicePMService.MapJsonToEntityPM(res, true);

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = res;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    //Certificates
    GetCertificateTickets(declarationId: string, reqConfirmationType: string, invoiceNumber: string, invoiceCounterKey: number, demandState: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCertificateTickets/?declarationId=" + declarationId
                + "&reqConfirmationType=" + reqConfirmationType
                + "&invoiceNumber=" + invoiceNumber
                + "&invoiceCounterKey=" + invoiceCounterKey
                + "&demandState=" + demandState
                , {
                    headers: authHeader
            }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<CertificateTicket> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: CertificateTicket;
                        entity = this.MapJsonToCertificateTicket(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }
    GetDeclarationInvoicesNumbers(declarationId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationInvoicesNumbers/?declarationId=" + declarationId
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    //supplier invoice -> ItemCode double click logic
    GetCustomsPartnersItemsForSelection(vendorId: string, CustomerId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCustomsPartnersItemsForSelection/?vendorId=" + vendorId
                + "&CustomerId=" + CustomerId
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }
    GetGTBITEMPartnersItemList(vendorId: string, customerCode: string, search: string, top: number) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetGTBITEMPartnersItemList/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&search=" + search
                + "&top=" + top
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }
    GetGITITEMPartnersItemList(vendorId: string, customerCode: string, search: string, top: number, isSearchNULLVendor: boolean) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetGITITEMPartnersItemList/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&search=" + search
                + "&top=" + top
                + "&searchNULLVendor=" + isSearchNULLVendor
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetGITITEMPartnersItemListByItemCode(vendorId: string, customerCode: string, itemCode: string, top: number, isSearchNULLVendor: boolean) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetGITITEMPartnersItemListByItemCode/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&itemCode=" + itemCode
                + "&top=" + top
                + "&searchNULLVendor=" + isSearchNULLVendor
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetGITITEMPartnersItemListByName(vendorId: string, customerCode: string, name: string, top: number) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetGITITEMPartnersItemListByName/?vendorId=" + vendorId
                + "&customerCode=" + customerCode
                + "&name=" + name
                + "&top=" + top
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    //Send declaration
    PostSendDeclaration(genericRequestParams: GenericRequestParams) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return this._http.post(
                this._apiUrl + '/PostSendDeclaration/',
                JSON.stringify(genericRequestParams),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }


    PostSendDeclarationAmendment(genericRequestParams: GenericRequestParams) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return this._http.post(
                this._apiUrl + '/PostSendDeclarationAmendment/',
                JSON.stringify(genericRequestParams),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    GetNewAmendmentDeclaration(genericRequestParams: GenericRequestParams) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return this._http.post(
                this._apiUrl + '/PostNewAmendmentDeclaration/',
                JSON.stringify(genericRequestParams),
                { headers: authHeader }).map((res) => {
                   
                    serviceResponse.Result = this.MapJsonToEntityPM(res.json(), true);
               

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }
    PostSendManifest(genericRequestParams: GenericRequestParams) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(genericRequestParams);
            return this._http.post(
                this._apiUrl + '/PostSendManifest/',
                JSON.stringify(genericRequestParams),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    PostSendDeclarationChecksAndPrecalculations(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSendDeclarationChecksAndPrecalculations/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        }

        );
    }

    GetRequiredFieldsForDeclaration(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetRequiredFieldsForDeclaration/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        }

        );
    }

    GetRequiredFieldsForCourierDeclaration(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetRequiredFieldsForCourierDeclaration/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        }

        );
    }

    CheckCertificateStatus(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCheckCertificateStatus/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        }

        );
    }

    GetMAWBCourierMasterByDeclaration(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetMAWBCourierMasterByDeclaration/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    GetDocumentDeclarationId(declarationId: string) {
    return Observable.defer(() => {

        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');

        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        return this._http.get(this._apiUrl + "/GetDocumentDeclarationId/?DeclarationId=" + declarationId
            
            , {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
    });
}
    
    GetDeclarationDocumentList(parentEntityId: string, parentEntityCode: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationDocumentList/?parentEntityId=" + parentEntityId
                + "&parentEntityCode=" + parentEntityCode
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetDeclarationMandatoryTicketList(parentEntityId: string, parentEntityCode: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationMandatoryTicketList/?parentEntityId=" + parentEntityId
                + "&parentEntityCode=" + parentEntityCode
                , {
                    headers: authHeader
                }).map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = response.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    CheckFreightAmountsByIncoterm(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCheckFreightAmountsByIncoterm/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        }

        );
    }

    CheckFreightAmountsByIncotermWithDefault(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCheckFreightAmountsByIncotermWithDefault/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        });
    }

    DeclarationClosureMethod(declarationId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationClosureMethod/?declarationId=" + declarationId + '&tenant=' + tenant, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    CancelDeclarationClosureMethod(declarationId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCancelDeclarationClosureMethod/?declarationId=" + declarationId + '&tenant=' + tenant, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    //Payment Orders
    GetSingleDeclarationPaymentPMandDefaultExplain(id: string, CustomerCode: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetSingleDeclarationPaymentPMandDefaultExplain/?id=" + id
                + "&CustomerCode=" + CustomerCode
                , {
                    headers: authHeader
                }).map(response => {

                    var res = response.json();

                    var entity: DeclarationPaymentPM;

                    if (res) {
                        entity = this._DeclarationPaymentPMService.MapJsonToEntityPM(res, true);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    //serviceResponse = response;
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    GetCustomBanksForCard(cardId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCustomBanksForCard/?cardId=" + cardId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                ////serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    GetAllRequiredFieldsForDeclarationPayment(declarationId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetAllRequiredFieldsForDeclarationPayment/?declarationId=" + declarationId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse = response;
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    // Amendments
    GetDeclarationCorrection(declarationId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationCorrection/?declarationId=" + declarationId, {
                    headers: authHeader
                }).map(response => {

                    var mappedEntity;
                    var allLists = response.json();
                    if (allLists)
                        mappedEntity = this.MapJsonToCorrectionView(allLists);

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = mappedEntity;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);

        }

        );
    }

    // Tapag
    GetDeclarationByTapagConnectionConnection(tapagId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationByTapagConnectionConnection/?tapagId=" + tapagId, {
                headers: authHeader
            }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetDeclarationCollateralsList(declarationId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationCollateralsList/?declarationId=" + declarationId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(response => {

                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetAcceptDeclarationAmendment(declarationId: string) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetAcceptDeclarationAmendment/?declarationId=" + declarationId , {
                headers: authHeader
            }).map(response => {

                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetDeclarationCargoSplitByDeclarationIdList(declarationId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationCargoSplitByDeclarationIdList/?declarationId=" + declarationId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(response => {

                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    GetDeclarationMamanSpecialAction(declarationId: string, tenant: number, actionCode: string, mamanSpecialActionCode: string) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetDeclarationMamanSpecialAction/?declarationId=" + declarationId + "&tenant=" + tenant + "&actionCode=" + actionCode + "&mamanSpecialActionCode=" + mamanSpecialActionCode, {
                headers: authHeader
            }).map(response => {

                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    PostSendCargoSealsRequest(requestParams: CargoSealsRequestParams) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            var params = JSON.stringify(requestParams);
            return this._http.post(
                this._apiUrl + '/PostSendCargoSealsRequest/',
                JSON.stringify(requestParams),
                { headers: authHeader }).map((res) => {
                    serviceResponse.Result = res.json();
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        });
    }

    // ---------------------------------------- MAPING --------------------------------------------------
    MapJsonToEntity(jsonPM: any, mapParent: boolean = true, entityPM: DeclarationErrorView = null) {


        if (!entityPM) {

            entityPM = new DeclarationErrorView();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        //entityPM.IsDirty = false;

        //if (mapParent) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entityPM;
    }
    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: SupplierInvoicePM = null) {


        if (!entityPM) {

            entityPM = new SupplierInvoicePM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        this.MapSupplierInvoiceItems(entityPM, jsonPM, mapParent); // Call composition tables map methods
     

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.SupplierInvoiceItems = [];
            for (var item in entityPM.SupplierInvoiceItems) {
                var mySupplierInvoiceItemPM = entityPM.SupplierInvoiceItems[item];
                var newSupplierInvoiceItemPM: SupplierInvoiceItemPM = this.clone(mySupplierInvoiceItemPM);

              

                entityPM.OldEntityPM.SupplierInvoiceItems.push(newSupplierInvoiceItemPM);
            }

            entityPM.OldEntityPM.SupplierInvoiceModifications = [];
            for (var item in entityPM.SupplierInvoiceModifications) {
                var mySupplierInvoiceModificationPM = entityPM.SupplierInvoiceModifications[item];
                var newSupplierInvoiceModificationPM: SupplierInvoiceModificationPM = this.clone(mySupplierInvoiceModificationPM);


                entityPM.OldEntityPM.SupplierInvoiceModifications.push(newSupplierInvoiceModificationPM);
            }

            entityPM.OldEntityPM.SupplierInvoiceFreightAmounts = [];
            for (var item in entityPM.SupplierInvoiceFreightAmounts) {
                var mySupplierInvoiceFreightAmountPM = entityPM.SupplierInvoiceFreightAmounts[item];
                var newSupplierInvoiceFreightAmountPM: SupplierInvoiceFreightAmountPM = this.clone(mySupplierInvoiceFreightAmountPM);


                entityPM.OldEntityPM.SupplierInvoiceFreightAmounts.push(newSupplierInvoiceFreightAmountPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }
    MapJsonToCertificateTicket(json: any, mapParent: boolean = true, entity: CertificateTicket = null) {


        if (!entity) {

            entity = new CertificateTicket();
        }

        var jsonPMKeys = Object.keys(json);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entity[property] = json[property];
        }


      //  entity.IsDirty = false;

     

        return entity;
    }
    MapSupplierInvoiceItems(entityPM: SupplierInvoicePM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItems: SupplierInvoiceItemPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItems = entityPM.OldEntityPM.SupplierInvoiceItems;
        }

        entityPM.SupplierInvoiceItems = new Array<SupplierInvoiceItemPM>();
        for (var item in jsonPM.SupplierInvoiceItems) {
            var jItem = jsonPM.SupplierInvoiceItems[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemPM: SupplierInvoiceItemPM;

            if (mapParent) {
                newSupplierInvoiceItemPM = new SupplierInvoiceItemPM(entityPM);
            }
            else {
                newSupplierInvoiceItemPM = new SupplierInvoiceItemPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemPM.OldEntityPM = this.clone(newSupplierInvoiceItemPM);

            }
            else {
                if (newSupplierInvoiceItemPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSupplierInvoiceItemPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoiceItemPM.ChangeSetOp = "Insert";
                }
                
                newSupplierInvoiceItemPM.OldEntityPM = null;
                newSupplierInvoiceItemPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItems.push(newSupplierInvoiceItemPM);
        }
        if (oldSupplierInvoiceItems) {

            for (var itemKey in oldSupplierInvoiceItems) {
                if (entityPM.SupplierInvoiceItems.filter(p => p.UniqueKey === oldSupplierInvoiceItems[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItems[itemKey]) {
                        //oldSupplierInvoiceItems[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItems.push(oldSupplierInvoiceItems[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItems[itemKey];
                        var deletedPM: SupplierInvoiceItemPM = new SupplierInvoiceItemPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        
                        deletedPM.OldEntityPM = null;
                        entityPM.SupplierInvoiceItems.push(deletedPM);
                    }
                }
            }
        }
    }

    MapJsonToCustomsCollateralPM(jsonPM: any, mapParent: boolean = true, entityPM: CustomsCollateralPM = null) {


        if (!entityPM) {

            entityPM = new CustomsCollateralPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        this.MapCustomsCollateralsConditions(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCustomsCollateralsAnswers(entityPM, jsonPM, mapParent); // Call composition tables map methods

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.CustomsCollateralsConditions = [];
            for (var item in entityPM.CustomsCollateralsConditions) {
                var myCustomsCollateralsConditionPM = entityPM.CustomsCollateralsConditions[item];
                var newCustomsCollateralsConditionPM: CustomsCollateralsConditionPM = this.clone(myCustomsCollateralsConditionPM);


                entityPM.OldEntityPM.CustomsCollateralsConditions.push(newCustomsCollateralsConditionPM);
            }

            entityPM.OldEntityPM.CustomsCollateralsAnswers = [];
            for (var item in entityPM.CustomsCollateralsAnswers) {
                var myCustomsCollateralsAnswerPM = entityPM.CustomsCollateralsAnswers[item];
                var newCustomsCollateralsAnswerPM: CustomsCollateralsAnswerPM = this.clone(myCustomsCollateralsAnswerPM);

                newCustomsCollateralsAnswerPM.CollateralsRequestFileConds = [];
                for (var k in myCustomsCollateralsAnswerPM.CollateralsRequestFileConds) {
                    var myCollateralsRequestFileCondPM = myCustomsCollateralsAnswerPM.CollateralsRequestFileConds[k];
                    var newCollateralsRequestFileCondPM = this.clone(myCustomsCollateralsAnswerPM.CollateralsRequestFileConds[k]);
                    newCustomsCollateralsAnswerPM.CollateralsRequestFileConds.push(newCollateralsRequestFileCondPM);

                }

                entityPM.OldEntityPM.CustomsCollateralsAnswers.push(newCustomsCollateralsAnswerPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }
    MapCustomsCollateralsConditions(entityPM: CustomsCollateralPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomsCollateralsConditions: CustomsCollateralsConditionPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomsCollateralsConditions = entityPM.OldEntityPM.CustomsCollateralsConditions;
        }

        entityPM.CustomsCollateralsConditions = new Array<CustomsCollateralsConditionPM>();
        for (var item in jsonPM.CustomsCollateralsConditions) {
            var jItem = jsonPM.CustomsCollateralsConditions[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomsCollateralsConditionPM: CustomsCollateralsConditionPM;

            if (mapParent) {
                newCustomsCollateralsConditionPM = new CustomsCollateralsConditionPM(entityPM);
            }
            else {
                newCustomsCollateralsConditionPM = new CustomsCollateralsConditionPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomsCollateralsConditionPM[pmProperty] = jItem[pmProperty];
            }
            newCustomsCollateralsConditionPM.IsDirty = false;

            if (mapParent) {
                newCustomsCollateralsConditionPM.UniqueKey = Guid.newGuid();
                newCustomsCollateralsConditionPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomsCollateralsConditionPM.OldEntityPM = this.clone(newCustomsCollateralsConditionPM);


            }
            else {
                if (newCustomsCollateralsConditionPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomsCollateralsConditionPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomsCollateralsConditionPM.ChangeSetOp = "Insert";
                }

                newCustomsCollateralsConditionPM.OldEntityPM = null;
                newCustomsCollateralsConditionPM.EntityParentPM = null;
            }


            entityPM.CustomsCollateralsConditions.push(newCustomsCollateralsConditionPM);
        }
        if (oldCustomsCollateralsConditions) {

            for (var itemKey in oldCustomsCollateralsConditions) {
                if (entityPM.CustomsCollateralsConditions.filter(p => p.UniqueKey === oldCustomsCollateralsConditions[itemKey].UniqueKey).length === 0) {

                    if (oldCustomsCollateralsConditions[itemKey]) {
                        //oldCustomsCollateralsConditions[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomsCollateralsConditions.push(oldCustomsCollateralsConditions[itemKey]);
                        var oldItemJson = oldCustomsCollateralsConditions[itemKey];
                        var deletedPM: CustomsCollateralsConditionPM = new CustomsCollateralsConditionPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.CustomsCollateralsConditions.push(deletedPM);
                    }
                }
            }
        }
    }
    MapCustomsCollateralsAnswers(entityPM: CustomsCollateralPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomsCollateralsAnswers: CustomsCollateralsAnswerPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomsCollateralsAnswers = entityPM.OldEntityPM.CustomsCollateralsAnswers;
        }

        entityPM.CustomsCollateralsAnswers = new Array<CustomsCollateralsAnswerPM>();
        for (var item in jsonPM.CustomsCollateralsAnswers) {
            var jItem = jsonPM.CustomsCollateralsAnswers[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomsCollateralsAnswerPM: CustomsCollateralsAnswerPM;

            if (mapParent) {
                newCustomsCollateralsAnswerPM = new CustomsCollateralsAnswerPM(entityPM);
            }
            else {
                newCustomsCollateralsAnswerPM = new CustomsCollateralsAnswerPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomsCollateralsAnswerPM[pmProperty] = jItem[pmProperty];
            }
            newCustomsCollateralsAnswerPM.IsDirty = false;

            if (mapParent) {
                newCustomsCollateralsAnswerPM.UniqueKey = Guid.newGuid();
                newCustomsCollateralsAnswerPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomsCollateralsAnswerPM.OldEntityPM = this.clone(newCustomsCollateralsAnswerPM);


                this.MapCollateralsRequestFileConds(newCustomsCollateralsAnswerPM, jItem, mapParent);
                newCustomsCollateralsAnswerPM.OldEntityPM.CollateralsRequestFileConds = [];
                for (var k in newCustomsCollateralsAnswerPM.CollateralsRequestFileConds) {
                    var clonedInside = this.clone(newCustomsCollateralsAnswerPM.CollateralsRequestFileConds[k]);
                    newCustomsCollateralsAnswerPM.OldEntityPM.CollateralsRequestFileConds.push(clonedInside); // clone old CollateralsRequestFileConds//
                }


            }
            else {
                if (newCustomsCollateralsAnswerPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomsCollateralsAnswerPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomsCollateralsAnswerPM.ChangeSetOp = "Insert";
                }


                this.MapCollateralsRequestFileConds(newCustomsCollateralsAnswerPM, jItem, mapParent);

                newCustomsCollateralsAnswerPM.OldEntityPM = null;
                newCustomsCollateralsAnswerPM.EntityParentPM = null;
            }


            entityPM.CustomsCollateralsAnswers.push(newCustomsCollateralsAnswerPM);
        }
        if (oldCustomsCollateralsAnswers) {

            for (var itemKey in oldCustomsCollateralsAnswers) {
                if (entityPM.CustomsCollateralsAnswers.filter(p => p.UniqueKey === oldCustomsCollateralsAnswers[itemKey].UniqueKey).length === 0) {

                    if (oldCustomsCollateralsAnswers[itemKey]) {
                        //oldCustomsCollateralsAnswers[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomsCollateralsAnswers.push(oldCustomsCollateralsAnswers[itemKey]);
                        var oldItemJson = oldCustomsCollateralsAnswers[itemKey];
                        var deletedPM: CustomsCollateralsAnswerPM = new CustomsCollateralsAnswerPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";



                        this.MapCollateralsRequestFileConds(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.CustomsCollateralsAnswers.push(deletedPM);
                    }
                }
            }
        }
    }
    MapCollateralsRequestFileConds(entityPM: CustomsCollateralsAnswerPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollateralsRequestFileConds: CollateralsRequestFileCondPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollateralsRequestFileConds = entityPM.OldEntityPM.CollateralsRequestFileConds;
        }

        entityPM.CollateralsRequestFileConds = new Array<CollateralsRequestFileCondPM>();
        for (var item in jsonPM.CollateralsRequestFileConds) {
            var jItem = jsonPM.CollateralsRequestFileConds[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCollateralsRequestFileCondPM: CollateralsRequestFileCondPM;

            if (mapParent) {
                newCollateralsRequestFileCondPM = new CollateralsRequestFileCondPM(entityPM);
            }
            else {
                newCollateralsRequestFileCondPM = new CollateralsRequestFileCondPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCollateralsRequestFileCondPM[pmProperty] = jItem[pmProperty];
            }
            newCollateralsRequestFileCondPM.IsDirty = false;

            if (mapParent) {
                newCollateralsRequestFileCondPM.UniqueKey = Guid.newGuid();
                newCollateralsRequestFileCondPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCollateralsRequestFileCondPM.OldEntityPM = this.clone(newCollateralsRequestFileCondPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newCollateralsRequestFileCondPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newCollateralsRequestFileCondPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newCollateralsRequestFileCondPM.ChangeSetOp = "Update";
                    }
                    else {
                        newCollateralsRequestFileCondPM.ChangeSetOp = "Insert";
                    }
                }

                newCollateralsRequestFileCondPM.OldEntityPM = null;
                newCollateralsRequestFileCondPM.EntityParentPM = null;
            }


            entityPM.CollateralsRequestFileConds.push(newCollateralsRequestFileCondPM);
        }
        if (oldCollateralsRequestFileConds) {

            for (var itemKey in oldCollateralsRequestFileConds) {
                if (entityPM.CollateralsRequestFileConds.filter(p => p.UniqueKey === oldCollateralsRequestFileConds[itemKey].UniqueKey).length === 0) {

                    if (oldCollateralsRequestFileConds[itemKey]) {
                        //oldCollateralsRequestFileConds[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CollateralsRequestFileConds.push(oldCollateralsRequestFileConds[itemKey]);
                        var oldItemJson = oldCollateralsRequestFileConds[itemKey];
                        var deletedPM: CollateralsRequestFileCondPM = new CollateralsRequestFileCondPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.CollateralsRequestFileConds.push(deletedPM);
                    }
                }
            }
        }
    }


    MapJsonToCorrectionView(jsonPM: any, mapParent: boolean = true, entityPM: DeclarationCorrectionView = null) {


        if (!entityPM) {

            entityPM = new DeclarationCorrectionView();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        //entityPM.IsDirty = false;

        //if (mapParent) {
        //    entityPM.OldEntityPM = this.clone(entityPM);

        //}
        //else {

        //    entityPM.OldEntityPM = null;
        //}

        return entityPM;
    }
    // --------------------------------------------------------------------------------------------------
    public clone(jsonPM: any) {
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
    public GetNewEntityPM() {
        var entityPM: CustomsCollateralPM;
        entityPM = new CustomsCollateralPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }

    

}
