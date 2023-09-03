import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SupplierInvoicePM} from '../../EntityPMs/SupplierInvoicePM';

import {SupplierInvoiceItemPM} from '../../EntityPMs/SupplierInvoiceItemPM';

import {SupplierInvoiceItemsConDeclarPM} from '../../EntityPMs/SupplierInvoiceItemsConDeclarPM';
import {SupplierInvioceItemCertificatPM} from '../../EntityPMs/SupplierInvioceItemCertificatPM';
import {SupplierInvoiceItemsModPM} from '../../EntityPMs/SupplierInvoiceItemsModPM';
import {SupplierInvoiceItemsSerialNumPM} from '../../EntityPMs/SupplierInvoiceItemsSerialNumPM';
import {SupplierInvoiceItemsDescriptPM} from '../../EntityPMs/SupplierInvoiceItemsDescriptPM';
import {SupplierInvoiceItemsProdIdentPM} from '../../EntityPMs/SupplierInvoiceItemsProdIdentPM';
import {SupplierInvoiceItemProcesTypePM} from '../../EntityPMs/SupplierInvoiceItemProcesTypePM';
import {SupplierInvoiceItemsLevyPM} from '../../EntityPMs/SupplierInvoiceItemsLevyPM';
import {SupplierInvoiceItemVehiclePM} from '../../EntityPMs/SupplierInvoiceItemVehiclePM';
import {SupplierInvoiceItemVehicleModPM} from '../../EntityPMs/SupplierInvoiceItemVehicleModPM';
import {SupplierInvoiceItemVehicleAddPM} from '../../EntityPMs/SupplierInvoiceItemVehicleAddPM';
import {SupplierInvoiceItemModVehiclePM} from '../../EntityPMs/SupplierInvoiceItemModVehiclePM';
import {SupplierInvoiceModificationPM} from '../../EntityPMs/SupplierInvoiceModificationPM';
import {SupplierInvoiceFreightAmountPM} from '../../EntityPMs/SupplierInvoiceFreightAmountPM';
import {SupplierInvoiceItemsTaxPM} from '../../EntityPMs/SupplierInvoiceItemsTaxPM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ImporterDespositionClass} from '../../DataContract/ImporterDespositionClass';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import { SupplierInvoicePaymentPM } from '../../EntityPMs/SupplierInvoicePaymentPM';
import { SupplierInvoiceUCRPM } from '../../EntityPMs/SupplierInvoiceUCRPM';
import { SuppInvoiceItemsAbachStatementPM } from '../../EntityPMs/SuppInvoiceItemsAbachStatementPM';
import { SupplierInvoiceItemsPricePM } from '../../EntityPMs/SupplierInvoiceItemsPricePM';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

@Injectable()

export class SupplierInvoiceExtendedPMService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationSupplierInvoices';
    }

 
    GetSupplierInvoicesPMsForDeclaration(declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetSupplierInvoicesPMsForDeclaration';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSupplierInvoicesPMsForDeclaration/?' + 'declarationId=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                var _mappedListsArray: Array<SupplierInvoicePM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: SupplierInvoicePM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    delete(declarationId: string, counterKey: number) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: SupplierInvoicePM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return this._http.delete(this._apiUrl + '/Delete/?' + 'declarationId=' + declarationId + '&counterKey=' + counterKey, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;
                if (pm) {
                    var mappedResult: SupplierInvoicePM;
                    //   mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                    serviceResponse.Result = mappedResult;
                }


                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));

        }

        );

    }

    GetInvoiceItemsWithTradeAgreementCount(declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetInvoiceItemsWithTradeAgreementCount';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetInvoiceItemsWithTradeAgreementCount/?' + 'declarationId=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSupplierInvoicesPMsForDeclarationWithTradeAgreementCount(declarationId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        var url = this._apiUrl + '/GetSupplierInvoicesPMsForDeclarationWithTradeAgreementCount';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSupplierInvoicesPMsForDeclarationWithTradeAgreementCount/?' + 'declarationId=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var serviceResponse: ServiceResponse = new ServiceResponse();
                var res:any = response;
                serviceResponse.Result = res.SupplierInvoices; //response;
                var count = res.Count;
                var _mappedListsArray: Array<SupplierInvoicePM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: SupplierInvoicePM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                serviceResponse.Result = { SupplierInvoices: _mappedListsArray, Count: count };//_mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSingleSupplierInvoicePMWithLimitedItems(declarationId: string, counterkey: number, skip: number, take: number, type:string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleSupplierInvoicePMWithLimitedItems?' + 'declarationId=' + declarationId + '&' + 'counterkey=' + counterkey + '&' + 'skip=' + skip + '&' + 'take=' + take + '&' + 'type=' + type, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pm = response;


                var entity: SupplierInvoicePM;
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

    GetImporterDespositionStatus(vendorId: string, importerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        //  var url = this._apiUrl + '/CheckForPointers';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetImporterDepositions?' + 'vendorId=' + vendorId + '&importerId=' + importerId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;
                if (pm) {
                    var mappedResult: ImporterDespositionClass;
                    mappedResult = this.MapJsonToImporterDesposition(pm, true, mappedResult);
                    serviceResponse.Result = mappedResult;
                }
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    GetDocumentFilingIdForForInvoice(declarationId: string, counterkey: number, isOcr = false) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDocumentFilingIdForForInvoice?'
                + 'declarationId=' + declarationId + '&' + 'counterkey=' + counterkey + '&' + 'isOcr=' + isOcr, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var resultJson = response;

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = resultJson;
                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    PutSupplierInvoicePercentage(invoice:SupplierInvoicePM) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');




            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();



        
            return this._http.put(this._apiUrl + '/UpdateInvoiceVendorCommision/', JSON.stringify(invoice),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var pm = res;
                    if (pm) {

                        serviceResponse.Result = pm;
                    }


                    return serviceResponse;

                }),catchError(ServiceHelper.HandleServiceError));

        }

        );


    }

    updateSupplierInvoiceModifications(entityPMs: SupplierInvoicePM[]) {

        var callTime = new Date();

        return defer(() => {

            var serviceResponse: ServiceResponse = new ServiceResponse();
            var validator: ClassLevelValidator = new ClassLevelValidator();
            var errorsArray = [];
            entityPMs.forEach(x => {
                errorsArray = validator.Validate("Customs.SupplierInvoice", x);
                if (errorsArray.length != 0) {
                    serviceResponse.HasError = true;
                    serviceResponse.ErrorsArray = errorsArray;
                    return of(serviceResponse);
                }
            });
             
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            if (errorsArray.length == 0) {

                //var mappedEntity: SupplierInvoicePM = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl + '/PutUpdateSupplierInvoiceModifications/', JSON.stringify(entityPMs), ServiceHelper.GetHttpHeaders())
                    .pipe(
                        map((response) => {

                            var pm = response;
                            if (pm) {
                                //var mappedResult: SupplierInvoicePM = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = pm;
                            }

                            //var servertime = response.headers.get('ServerExecutionTime');
                            //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "SupplierInvoice", "SaveChanges", "");

                            return serviceResponse;
                        }),

                        catchError(ServiceHelper.HandleServiceError));
            }

            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return of(serviceResponse);
            }
        });
    }




    // --------------------------------- Mapping --------------------------------------------------------------------------------

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: SupplierInvoicePM = null) {


        if (!entityPM) {

            entityPM = new SupplierInvoicePM();
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
                //if (jsonPM[property]) {
                //    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                //    entityPM[property] = customFieldClass;
                //}
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }

        this.MapSupplierInvoiceItems(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapSupplierInvoiceModifications(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapSupplierInvoiceFreightAmounts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapSupplierInvoicePayments(entityPM, jsonPM, mapParent);
        this.MapSupplierInvoiceUCRs(entityPM, jsonPM, mapParent);
        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.SupplierInvoiceItems = [];
            for (var item in entityPM.SupplierInvoiceItems) {
                var mySupplierInvoiceItemPM = entityPM.SupplierInvoiceItems[item];
                var newSupplierInvoiceItemPM: SupplierInvoiceItemPM = this.clone(mySupplierInvoiceItemPM);

                newSupplierInvoiceItemPM.SupplierInvoiceItemTaxes = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemTaxes) {
                    var mySupplierInvoiceItemsTaxPM = mySupplierInvoiceItemPM.SupplierInvoiceItemTaxes[k];
                    var newSupplierInvoiceItemsTaxPM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemTaxes[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemTaxes.push(newSupplierInvoiceItemsTaxPM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars) {
                    var mySupplierInvoiceItemsConDeclarPM = mySupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars[k];
                    var newSupplierInvoiceItemsConDeclarPM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars.push(newSupplierInvoiceItemsConDeclarPM);

                }
                newSupplierInvoiceItemPM.SupplierInvioceItemCertificats = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvioceItemCertificats) {
                    var mySupplierInvioceItemCertificatPM = mySupplierInvoiceItemPM.SupplierInvioceItemCertificats[k];
                    var newSupplierInvioceItemCertificatPM = this.clone(mySupplierInvoiceItemPM.SupplierInvioceItemCertificats[k]);
                    newSupplierInvoiceItemPM.SupplierInvioceItemCertificats.push(newSupplierInvioceItemCertificatPM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemsMods = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemsMods) {
                    var mySupplierInvoiceItemsModPM = mySupplierInvoiceItemPM.SupplierInvoiceItemsMods[k];
                    var newSupplierInvoiceItemsModPM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemsMods[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemsMods.push(newSupplierInvoiceItemsModPM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums) {
                    var mySupplierInvoiceItemsSerialNumPM = mySupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums[k];
                    var newSupplierInvoiceItemsSerialNumPM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums.push(newSupplierInvoiceItemsSerialNumPM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemsDescripts = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemsDescripts) {
                    var mySupplierInvoiceItemsDescriptPM = mySupplierInvoiceItemPM.SupplierInvoiceItemsDescripts[k];
                    var newSupplierInvoiceItemsDescriptPM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemsDescripts[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemsDescripts.push(newSupplierInvoiceItemsDescriptPM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemsProdIdents = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemsProdIdents) {
                    var mySupplierInvoiceItemsProdIdentPM = mySupplierInvoiceItemPM.SupplierInvoiceItemsProdIdents[k];
                    var newSupplierInvoiceItemsProdIdentPM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemsProdIdents[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemsProdIdents.push(newSupplierInvoiceItemsProdIdentPM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes) {
                    var mySupplierInvoiceItemProcesTypePM = mySupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes[k];
                    var newSupplierInvoiceItemProcesTypePM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.push(newSupplierInvoiceItemProcesTypePM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemsPrices = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemsPrices) {
                    var mySupplierInvoiceItemsPricePM = mySupplierInvoiceItemPM.SupplierInvoiceItemsPrices[k];
                    var newSupplierInvoiceItemsPricePM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemsPrices[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemsPrices.push(newSupplierInvoiceItemsPricePM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemLevies = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemLevies) {
                    var mySupplierInvoiceItemsLevyPM = mySupplierInvoiceItemPM.SupplierInvoiceItemLevies[k];
                    var newSupplierInvoiceItemsLevyPM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemLevies[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemLevies.push(newSupplierInvoiceItemsLevyPM);

                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemVehicles = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemVehicles) {
                    var mySupplierInvoiceItemVehiclePM = mySupplierInvoiceItemPM.SupplierInvoiceItemVehicles[k];
                    var newSupplierInvoiceItemVehiclePM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemVehicles[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemVehicles.push(newSupplierInvoiceItemVehiclePM);

                    newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = [];
                    for (var k in mySupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods) {
                        var mySupplierInvoiceItemVehicleModPM = mySupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods[k];
                        var newSupplierInvoiceItemVehicleModPM = this.clone(mySupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods[k]);
                        newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.push(newSupplierInvoiceItemVehicleModPM);

                    }
                    newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = [];
                    for (var k in mySupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds) {
                        var mySupplierInvoiceItemVehicleAddPM = mySupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds[k];
                        var newSupplierInvoiceItemVehicleAddPM = this.clone(mySupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds[k]);
                        newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.push(newSupplierInvoiceItemVehicleAddPM);

                    }
                }
                newSupplierInvoiceItemPM.SupplierInvoiceItemModVehicles = [];
                for (var k in mySupplierInvoiceItemPM.SupplierInvoiceItemModVehicles) {
                    var mySupplierInvoiceItemModVehiclePM = mySupplierInvoiceItemPM.SupplierInvoiceItemModVehicles[k];
                    var newSupplierInvoiceItemModVehiclePM = this.clone(mySupplierInvoiceItemPM.SupplierInvoiceItemModVehicles[k]);
                    newSupplierInvoiceItemPM.SupplierInvoiceItemModVehicles.push(newSupplierInvoiceItemModVehiclePM);

                }

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


            entityPM.OldEntityPM.SupplierInvoicePayments = [];
            for (var item in entityPM.SupplierInvoicePayments) {
                var mySupplierInvoicePaymentPM = entityPM.SupplierInvoicePayments[item];
                var newSupplierInvoicePaymentPM: SupplierInvoicePaymentPM = this.clone(mySupplierInvoicePaymentPM);


                entityPM.OldEntityPM.SupplierInvoicePayments.push(newSupplierInvoicePaymentPM);
            }

            entityPM.OldEntityPM.SupplierInvoiceUCRs = [];
            for (var item in entityPM.SupplierInvoiceUCRs) {
                var mySupplierInvoiceUCRPM = entityPM.SupplierInvoiceUCRs[item];
                var newSupplierInvoiceUCRPM: SupplierInvoiceUCRPM = this.clone(mySupplierInvoiceUCRPM);


                entityPM.OldEntityPM.SupplierInvoiceUCRs.push(newSupplierInvoiceUCRPM);
            }
        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
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
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
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


                this.MapSupplierInvoiceItemTaxes(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemTaxes = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemTaxes) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemTaxes[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemTaxes.push(clonedInside); // clone old SupplierInvoiceItemTaxes//
                }


                this.MapSupplierInvoiceItemsConDeclars(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsConDeclars = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsConDeclars.push(clonedInside); // clone old SupplierInvoiceItemsConDeclars//
                }


                this.MapSupplierInvioceItemCertificats(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvioceItemCertificats = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvioceItemCertificats) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvioceItemCertificats[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvioceItemCertificats.push(clonedInside); // clone old SupplierInvioceItemCertificats//
                }


                this.MapSupplierInvoiceItemsMods(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsMods = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemsMods) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemsMods[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsMods.push(clonedInside); // clone old SupplierInvoiceItemsMods//
                }


                this.MapSupplierInvoiceItemsSerialNums(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsSerialNums = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsSerialNums.push(clonedInside); // clone old SupplierInvoiceItemsSerialNums//
                }


                this.MapSupplierInvoiceItemsDescripts(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsDescripts = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemsDescripts) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemsDescripts[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsDescripts.push(clonedInside); // clone old SupplierInvoiceItemsDescripts//
                }


                this.MapSupplierInvoiceItemsProdIdents(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsProdIdents = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemsProdIdents) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemsProdIdents[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsProdIdents.push(clonedInside); // clone old SupplierInvoiceItemsProdIdents//
                }


                this.MapSupplierInvoiceItemProcesTypes(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemProcesTypes = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemProcesTypes.push(clonedInside); // clone old SupplierInvoiceItemProcesTypes//
                }


                this.MapSupplierInvoiceItemLevies(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemLevies = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemLevies) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemLevies[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemLevies.push(clonedInside); // clone old SupplierInvoiceItemLevies//
                }


                this.MapSupplierInvoiceItemVehicles(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemVehicles = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemVehicles) {
                    //var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemVehicles[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemVehicles.push(newSupplierInvoiceItemPM.SupplierInvoiceItemVehicles[k].OldEntityPM); // clone old SupplierInvoiceItemVehicles//
                }


                this.MapSupplierInvoiceItemModVehicles(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemModVehicles = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemModVehicles) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemModVehicles[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemModVehicles.push(clonedInside); // clone old SupplierInvoiceItemModVehicles//
                }
                 this.MapSuppInvoiceItemsAbachStatements(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SuppInvoiceItemsAbachStatements = [];
                for (var k in newSupplierInvoiceItemPM.SuppInvoiceItemsAbachStatements) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SuppInvoiceItemsAbachStatements[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SuppInvoiceItemsAbachStatements.push(clonedInside); // clone old SupplierInvoiceItemModVehicles//
                }

                this.MapSupplierInvoiceItemsPrices(newSupplierInvoiceItemPM, jItem, mapParent);
                newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsPrices = [];
                for (var k in newSupplierInvoiceItemPM.SupplierInvoiceItemsPrices) {
                    var clonedInside = this.clone(newSupplierInvoiceItemPM.SupplierInvoiceItemsPrices[k]);
                    newSupplierInvoiceItemPM.OldEntityPM.SupplierInvoiceItemsPrices.push(clonedInside); // clone old SupplierInvoiceItemsPrices//
                }

            }
            else {
                if (newSupplierInvoiceItemPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSupplierInvoiceItemPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoiceItemPM.ChangeSetOp = "Insert";
                }


                this.MapSupplierInvoiceItemTaxes(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemsConDeclars(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvioceItemCertificats(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemsMods(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemsSerialNums(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemsDescripts(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemsProdIdents(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemProcesTypes(newSupplierInvoiceItemPM, jItem, mapParent);

                this.MapSupplierInvoiceItemsPrices(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemLevies(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemVehicles(newSupplierInvoiceItemPM, jItem, mapParent);


                this.MapSupplierInvoiceItemModVehicles(newSupplierInvoiceItemPM, jItem, mapParent);

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

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";



                        this.MapSupplierInvoiceItemTaxes(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemsConDeclars(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvioceItemCertificats(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemsMods(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemsSerialNums(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemsDescripts(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemsProdIdents(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemProcesTypes(deletedPM, oldItemJson, mapParent);

                        this.MapSupplierInvoiceItemsPrices(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemLevies(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemVehicles(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemModVehicles(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.SupplierInvoiceItems.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemTaxes(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemTaxes: SupplierInvoiceItemsTaxPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemTaxes = entityPM.OldEntityPM.SupplierInvoiceItemTaxes;
        }

        entityPM.SupplierInvoiceItemTaxes = new Array<SupplierInvoiceItemsTaxPM>();
        for (var item in jsonPM.SupplierInvoiceItemTaxes) {
            var jItem = jsonPM.SupplierInvoiceItemTaxes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsTaxPM: SupplierInvoiceItemsTaxPM;

            if (mapParent) {
                newSupplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsTaxPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsTaxPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsTaxPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsTaxPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsTaxPM.OldEntityPM = this.clone(newSupplierInvoiceItemsTaxPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsTaxPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsTaxPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsTaxPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsTaxPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsTaxPM.OldEntityPM = null;
                newSupplierInvoiceItemsTaxPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemTaxes.push(newSupplierInvoiceItemsTaxPM);
        }
        if (oldSupplierInvoiceItemTaxes) {

            for (var itemKey in oldSupplierInvoiceItemTaxes) {
                if (entityPM.SupplierInvoiceItemTaxes.filter(p => p.UniqueKey === oldSupplierInvoiceItemTaxes[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemTaxes[itemKey]) {
                        //oldSupplierInvoiceItemTaxes[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemTaxes.push(oldSupplierInvoiceItemTaxes[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemTaxes[itemKey];
                        var deletedPM: SupplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM(null);
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
                        entityPM.SupplierInvoiceItemTaxes.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemsConDeclars(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemsConDeclars: SupplierInvoiceItemsConDeclarPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemsConDeclars = entityPM.OldEntityPM.SupplierInvoiceItemsConDeclars;
        }

        entityPM.SupplierInvoiceItemsConDeclars = new Array<SupplierInvoiceItemsConDeclarPM>();
        for (var item in jsonPM.SupplierInvoiceItemsConDeclars) {
            var jItem = jsonPM.SupplierInvoiceItemsConDeclars[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsConDeclarPM: SupplierInvoiceItemsConDeclarPM;

            if (mapParent) {
                newSupplierInvoiceItemsConDeclarPM = new SupplierInvoiceItemsConDeclarPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsConDeclarPM = new SupplierInvoiceItemsConDeclarPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsConDeclarPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsConDeclarPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsConDeclarPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsConDeclarPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsConDeclarPM.OldEntityPM = this.clone(newSupplierInvoiceItemsConDeclarPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsConDeclarPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsConDeclarPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsConDeclarPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsConDeclarPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsConDeclarPM.OldEntityPM = null;
                newSupplierInvoiceItemsConDeclarPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemsConDeclars.push(newSupplierInvoiceItemsConDeclarPM);
        }
        if (oldSupplierInvoiceItemsConDeclars) {

            for (var itemKey in oldSupplierInvoiceItemsConDeclars) {
                if (entityPM.SupplierInvoiceItemsConDeclars.filter(p => p.UniqueKey === oldSupplierInvoiceItemsConDeclars[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemsConDeclars[itemKey]) {
                        //oldSupplierInvoiceItemsConDeclars[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemsConDeclars.push(oldSupplierInvoiceItemsConDeclars[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemsConDeclars[itemKey];
                        var deletedPM: SupplierInvoiceItemsConDeclarPM = new SupplierInvoiceItemsConDeclarPM(null);
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
                        entityPM.SupplierInvoiceItemsConDeclars.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvioceItemCertificats(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvioceItemCertificats: SupplierInvioceItemCertificatPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvioceItemCertificats = entityPM.OldEntityPM.SupplierInvioceItemCertificats;
        }

        entityPM.SupplierInvioceItemCertificats = new Array<SupplierInvioceItemCertificatPM>();
        for (var item in jsonPM.SupplierInvioceItemCertificats) {
            var jItem = jsonPM.SupplierInvioceItemCertificats[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvioceItemCertificatPM: SupplierInvioceItemCertificatPM;

            if (mapParent) {
                newSupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(entityPM);
            }
            else {
                newSupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvioceItemCertificatPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvioceItemCertificatPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvioceItemCertificatPM.UniqueKey = Guid.newGuid();
                newSupplierInvioceItemCertificatPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvioceItemCertificatPM.OldEntityPM = this.clone(newSupplierInvioceItemCertificatPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvioceItemCertificatPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvioceItemCertificatPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvioceItemCertificatPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvioceItemCertificatPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvioceItemCertificatPM.OldEntityPM = null;
                newSupplierInvioceItemCertificatPM.EntityParentPM = null;
            }


            entityPM.SupplierInvioceItemCertificats.push(newSupplierInvioceItemCertificatPM);
        }
        if (oldSupplierInvioceItemCertificats) {

            for (var itemKey in oldSupplierInvioceItemCertificats) {
                if (entityPM.SupplierInvioceItemCertificats.filter(p => p.UniqueKey === oldSupplierInvioceItemCertificats[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvioceItemCertificats[itemKey]) {
                        //oldSupplierInvioceItemCertificats[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvioceItemCertificats.push(oldSupplierInvioceItemCertificats[itemKey]);
                        var oldItemJson = oldSupplierInvioceItemCertificats[itemKey];
                        var deletedPM: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(null);
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
                        entityPM.SupplierInvioceItemCertificats.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemsMods(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemsMods: SupplierInvoiceItemsModPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemsMods = entityPM.OldEntityPM.SupplierInvoiceItemsMods;
        }

        entityPM.SupplierInvoiceItemsMods = new Array<SupplierInvoiceItemsModPM>();
        for (var item in jsonPM.SupplierInvoiceItemsMods) {
            var jItem = jsonPM.SupplierInvoiceItemsMods[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsModPM: SupplierInvoiceItemsModPM;

            if (mapParent) {
                newSupplierInvoiceItemsModPM = new SupplierInvoiceItemsModPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsModPM = new SupplierInvoiceItemsModPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsModPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsModPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsModPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsModPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsModPM.OldEntityPM = this.clone(newSupplierInvoiceItemsModPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsModPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsModPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsModPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsModPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsModPM.OldEntityPM = null;
                newSupplierInvoiceItemsModPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemsMods.push(newSupplierInvoiceItemsModPM);
        }
        if (oldSupplierInvoiceItemsMods) {

            for (var itemKey in oldSupplierInvoiceItemsMods) {
                if (entityPM.SupplierInvoiceItemsMods.filter(p => p.UniqueKey === oldSupplierInvoiceItemsMods[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemsMods[itemKey]) {
                        //oldSupplierInvoiceItemsMods[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemsMods.push(oldSupplierInvoiceItemsMods[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemsMods[itemKey];
                        var deletedPM: SupplierInvoiceItemsModPM = new SupplierInvoiceItemsModPM(null);
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
                        entityPM.SupplierInvoiceItemsMods.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemsSerialNums(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemsSerialNums: SupplierInvoiceItemsSerialNumPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemsSerialNums = entityPM.OldEntityPM.SupplierInvoiceItemsSerialNums;
        }

        entityPM.SupplierInvoiceItemsSerialNums = new Array<SupplierInvoiceItemsSerialNumPM>();
        for (var item in jsonPM.SupplierInvoiceItemsSerialNums) {
            var jItem = jsonPM.SupplierInvoiceItemsSerialNums[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsSerialNumPM: SupplierInvoiceItemsSerialNumPM;

            if (mapParent) {
                newSupplierInvoiceItemsSerialNumPM = new SupplierInvoiceItemsSerialNumPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsSerialNumPM = new SupplierInvoiceItemsSerialNumPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsSerialNumPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsSerialNumPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsSerialNumPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsSerialNumPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsSerialNumPM.OldEntityPM = this.clone(newSupplierInvoiceItemsSerialNumPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsSerialNumPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsSerialNumPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsSerialNumPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsSerialNumPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsSerialNumPM.OldEntityPM = null;
                newSupplierInvoiceItemsSerialNumPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemsSerialNums.push(newSupplierInvoiceItemsSerialNumPM);
        }
        if (oldSupplierInvoiceItemsSerialNums) {

            for (var itemKey in oldSupplierInvoiceItemsSerialNums) {
                if (entityPM.SupplierInvoiceItemsSerialNums.filter(p => p.UniqueKey === oldSupplierInvoiceItemsSerialNums[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemsSerialNums[itemKey]) {
                        //oldSupplierInvoiceItemsSerialNums[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemsSerialNums.push(oldSupplierInvoiceItemsSerialNums[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemsSerialNums[itemKey];
                        var deletedPM: SupplierInvoiceItemsSerialNumPM = new SupplierInvoiceItemsSerialNumPM(null);
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
                        entityPM.SupplierInvoiceItemsSerialNums.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemsDescripts(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemsDescripts: SupplierInvoiceItemsDescriptPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemsDescripts = entityPM.OldEntityPM.SupplierInvoiceItemsDescripts;
        }

        entityPM.SupplierInvoiceItemsDescripts = new Array<SupplierInvoiceItemsDescriptPM>();
        for (var item in jsonPM.SupplierInvoiceItemsDescripts) {
            var jItem = jsonPM.SupplierInvoiceItemsDescripts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsDescriptPM: SupplierInvoiceItemsDescriptPM;

            if (mapParent) {
                newSupplierInvoiceItemsDescriptPM = new SupplierInvoiceItemsDescriptPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsDescriptPM = new SupplierInvoiceItemsDescriptPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsDescriptPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsDescriptPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsDescriptPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsDescriptPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsDescriptPM.OldEntityPM = this.clone(newSupplierInvoiceItemsDescriptPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsDescriptPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsDescriptPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsDescriptPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsDescriptPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsDescriptPM.OldEntityPM = null;
                newSupplierInvoiceItemsDescriptPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemsDescripts.push(newSupplierInvoiceItemsDescriptPM);
        }
        if (oldSupplierInvoiceItemsDescripts) {

            for (var itemKey in oldSupplierInvoiceItemsDescripts) {
                if (entityPM.SupplierInvoiceItemsDescripts.filter(p => p.UniqueKey === oldSupplierInvoiceItemsDescripts[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemsDescripts[itemKey]) {
                        //oldSupplierInvoiceItemsDescripts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemsDescripts.push(oldSupplierInvoiceItemsDescripts[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemsDescripts[itemKey];
                        var deletedPM: SupplierInvoiceItemsDescriptPM = new SupplierInvoiceItemsDescriptPM(null);
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
                        entityPM.SupplierInvoiceItemsDescripts.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemsProdIdents(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemsProdIdents: SupplierInvoiceItemsProdIdentPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemsProdIdents = entityPM.OldEntityPM.SupplierInvoiceItemsProdIdents;
        }

        entityPM.SupplierInvoiceItemsProdIdents = new Array<SupplierInvoiceItemsProdIdentPM>();
        for (var item in jsonPM.SupplierInvoiceItemsProdIdents) {
            var jItem = jsonPM.SupplierInvoiceItemsProdIdents[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsProdIdentPM: SupplierInvoiceItemsProdIdentPM;

            if (mapParent) {
                newSupplierInvoiceItemsProdIdentPM = new SupplierInvoiceItemsProdIdentPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsProdIdentPM = new SupplierInvoiceItemsProdIdentPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsProdIdentPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsProdIdentPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsProdIdentPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsProdIdentPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsProdIdentPM.OldEntityPM = this.clone(newSupplierInvoiceItemsProdIdentPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsProdIdentPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsProdIdentPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsProdIdentPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsProdIdentPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsProdIdentPM.OldEntityPM = null;
                newSupplierInvoiceItemsProdIdentPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemsProdIdents.push(newSupplierInvoiceItemsProdIdentPM);
        }
        if (oldSupplierInvoiceItemsProdIdents) {

            for (var itemKey in oldSupplierInvoiceItemsProdIdents) {
                if (entityPM.SupplierInvoiceItemsProdIdents.filter(p => p.UniqueKey === oldSupplierInvoiceItemsProdIdents[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemsProdIdents[itemKey]) {
                        //oldSupplierInvoiceItemsProdIdents[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemsProdIdents.push(oldSupplierInvoiceItemsProdIdents[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemsProdIdents[itemKey];
                        var deletedPM: SupplierInvoiceItemsProdIdentPM = new SupplierInvoiceItemsProdIdentPM(null);
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
                        entityPM.SupplierInvoiceItemsProdIdents.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemProcesTypes(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemProcesTypes: SupplierInvoiceItemProcesTypePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemProcesTypes = entityPM.OldEntityPM.SupplierInvoiceItemProcesTypes;
        }

        entityPM.SupplierInvoiceItemProcesTypes = new Array<SupplierInvoiceItemProcesTypePM>();
        for (var item in jsonPM.SupplierInvoiceItemProcesTypes) {
            var jItem = jsonPM.SupplierInvoiceItemProcesTypes[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemProcesTypePM: SupplierInvoiceItemProcesTypePM;

            if (mapParent) {
                newSupplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM(entityPM);
            }
            else {
                newSupplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemProcesTypePM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemProcesTypePM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemProcesTypePM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemProcesTypePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemProcesTypePM.OldEntityPM = this.clone(newSupplierInvoiceItemProcesTypePM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemProcesTypePM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemProcesTypePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemProcesTypePM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemProcesTypePM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemProcesTypePM.OldEntityPM = null;
                newSupplierInvoiceItemProcesTypePM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemProcesTypes.push(newSupplierInvoiceItemProcesTypePM);
        }
        if (oldSupplierInvoiceItemProcesTypes) {

            for (var itemKey in oldSupplierInvoiceItemProcesTypes) {
                if (entityPM.SupplierInvoiceItemProcesTypes.filter(p => p.UniqueKey === oldSupplierInvoiceItemProcesTypes[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemProcesTypes[itemKey]) {
                        //oldSupplierInvoiceItemProcesTypes[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemProcesTypes.push(oldSupplierInvoiceItemProcesTypes[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemProcesTypes[itemKey];
                        var deletedPM: SupplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM(null);
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
                        entityPM.SupplierInvoiceItemProcesTypes.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemLevies(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemLevies: SupplierInvoiceItemsLevyPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemLevies = entityPM.OldEntityPM.SupplierInvoiceItemLevies;
        }

        entityPM.SupplierInvoiceItemLevies = new Array<SupplierInvoiceItemsLevyPM>();
        for (var item in jsonPM.SupplierInvoiceItemLevies) {
            var jItem = jsonPM.SupplierInvoiceItemLevies[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsLevyPM: SupplierInvoiceItemsLevyPM;

            if (mapParent) {
                newSupplierInvoiceItemsLevyPM = new SupplierInvoiceItemsLevyPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsLevyPM = new SupplierInvoiceItemsLevyPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsLevyPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsLevyPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsLevyPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsLevyPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsLevyPM.OldEntityPM = this.clone(newSupplierInvoiceItemsLevyPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsLevyPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsLevyPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsLevyPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsLevyPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsLevyPM.OldEntityPM = null;
                newSupplierInvoiceItemsLevyPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemLevies.push(newSupplierInvoiceItemsLevyPM);
        }
        if (oldSupplierInvoiceItemLevies) {

            for (var itemKey in oldSupplierInvoiceItemLevies) {
                if (entityPM.SupplierInvoiceItemLevies.filter(p => p.UniqueKey === oldSupplierInvoiceItemLevies[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemLevies[itemKey]) {
                        //oldSupplierInvoiceItemLevies[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemLevies.push(oldSupplierInvoiceItemLevies[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemLevies[itemKey];
                        var deletedPM: SupplierInvoiceItemsLevyPM = new SupplierInvoiceItemsLevyPM(null);
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
                        entityPM.SupplierInvoiceItemLevies.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemVehicles(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemVehicles: SupplierInvoiceItemVehiclePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemVehicles = entityPM.OldEntityPM.SupplierInvoiceItemVehicles;
        }

        entityPM.SupplierInvoiceItemVehicles = new Array<SupplierInvoiceItemVehiclePM>();
        for (var item in jsonPM.SupplierInvoiceItemVehicles) {
            var jItem = jsonPM.SupplierInvoiceItemVehicles[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemVehiclePM: SupplierInvoiceItemVehiclePM;

            if (mapParent) {
                newSupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM(entityPM);
            }
            else {
                newSupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemVehiclePM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemVehiclePM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemVehiclePM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemVehiclePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemVehiclePM.OldEntityPM = this.clone(newSupplierInvoiceItemVehiclePM);


                this.MapSupplierInvoiceItemVehicleMods(newSupplierInvoiceItemVehiclePM, jItem, mapParent);
                newSupplierInvoiceItemVehiclePM.OldEntityPM.SupplierInvoiceItemVehicleMods = [];
                for (var k in newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods) {
                    //var clonedInside = this.clone(newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods[k]);
                    newSupplierInvoiceItemVehiclePM.OldEntityPM.SupplierInvoiceItemVehicleMods.push(newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods[k].OldEntityPM); // clone old SupplierInvoiceItemVehicleMods//
                }


                this.MapSupplierInvoiceItemVehicleAdds(newSupplierInvoiceItemVehiclePM, jItem, mapParent);
                newSupplierInvoiceItemVehiclePM.OldEntityPM.SupplierInvoiceItemVehicleAdds = [];
                for (var k in newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds) {
                    //var clonedInside = this.clone(newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds[k]);
                    newSupplierInvoiceItemVehiclePM.OldEntityPM.SupplierInvoiceItemVehicleAdds.push(newSupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds[k].OldEntityPM); // clone old SupplierInvoiceItemVehicleAdds//
                }


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemVehiclePM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemVehiclePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemVehiclePM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemVehiclePM.ChangeSetOp = "Insert";
                    }
                }


                this.MapSupplierInvoiceItemVehicleMods(newSupplierInvoiceItemVehiclePM, jItem, mapParent);


                this.MapSupplierInvoiceItemVehicleAdds(newSupplierInvoiceItemVehiclePM, jItem, mapParent);

                newSupplierInvoiceItemVehiclePM.OldEntityPM = null;
                newSupplierInvoiceItemVehiclePM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemVehicles.push(newSupplierInvoiceItemVehiclePM);
        }
        if (oldSupplierInvoiceItemVehicles) {

            for (var itemKey in oldSupplierInvoiceItemVehicles) {
                if (entityPM.SupplierInvoiceItemVehicles.filter(p => p.UniqueKey === oldSupplierInvoiceItemVehicles[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemVehicles[itemKey]) {
                        //oldSupplierInvoiceItemVehicles[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemVehicles.push(oldSupplierInvoiceItemVehicles[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemVehicles[itemKey];
                        var deletedPM: SupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM(null);
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



                        this.MapSupplierInvoiceItemVehicleMods(deletedPM, oldItemJson, mapParent);


                        this.MapSupplierInvoiceItemVehicleAdds(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.SupplierInvoiceItemVehicles.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemVehicleMods(entityPM: SupplierInvoiceItemVehiclePM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemVehicleMods: SupplierInvoiceItemVehicleModPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemVehicleMods = entityPM.OldEntityPM.SupplierInvoiceItemVehicleMods;
        }

        entityPM.SupplierInvoiceItemVehicleMods = new Array<SupplierInvoiceItemVehicleModPM>();
        for (var item in jsonPM.SupplierInvoiceItemVehicleMods) {
            var jItem = jsonPM.SupplierInvoiceItemVehicleMods[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemVehicleModPM: SupplierInvoiceItemVehicleModPM;

            if (mapParent) {
                newSupplierInvoiceItemVehicleModPM = new SupplierInvoiceItemVehicleModPM(entityPM);
            }
            else {
                newSupplierInvoiceItemVehicleModPM = new SupplierInvoiceItemVehicleModPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemVehicleModPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemVehicleModPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemVehicleModPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemVehicleModPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemVehicleModPM.OldEntityPM = this.clone(newSupplierInvoiceItemVehicleModPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemVehicleModPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemVehicleModPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemVehicleModPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemVehicleModPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemVehicleModPM.OldEntityPM = null;
                newSupplierInvoiceItemVehicleModPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemVehicleMods.push(newSupplierInvoiceItemVehicleModPM);
        }
        if (oldSupplierInvoiceItemVehicleMods) {

            for (var itemKey in oldSupplierInvoiceItemVehicleMods) {
                if (entityPM.SupplierInvoiceItemVehicleMods.filter(p => p.UniqueKey === oldSupplierInvoiceItemVehicleMods[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemVehicleMods[itemKey]) {
                        //oldSupplierInvoiceItemVehicleMods[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemVehicleMods.push(oldSupplierInvoiceItemVehicleMods[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemVehicleMods[itemKey];
                        var deletedPM: SupplierInvoiceItemVehicleModPM = new SupplierInvoiceItemVehicleModPM(null);
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
                        entityPM.SupplierInvoiceItemVehicleMods.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemVehicleAdds(entityPM: SupplierInvoiceItemVehiclePM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemVehicleAdds: SupplierInvoiceItemVehicleAddPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemVehicleAdds = entityPM.OldEntityPM.SupplierInvoiceItemVehicleAdds;
        }

        entityPM.SupplierInvoiceItemVehicleAdds = new Array<SupplierInvoiceItemVehicleAddPM>();
        for (var item in jsonPM.SupplierInvoiceItemVehicleAdds) {
            var jItem = jsonPM.SupplierInvoiceItemVehicleAdds[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemVehicleAddPM: SupplierInvoiceItemVehicleAddPM;

            if (mapParent) {
                newSupplierInvoiceItemVehicleAddPM = new SupplierInvoiceItemVehicleAddPM(entityPM);
            }
            else {
                newSupplierInvoiceItemVehicleAddPM = new SupplierInvoiceItemVehicleAddPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemVehicleAddPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemVehicleAddPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemVehicleAddPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemVehicleAddPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemVehicleAddPM.OldEntityPM = this.clone(newSupplierInvoiceItemVehicleAddPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemVehicleAddPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemVehicleAddPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemVehicleAddPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemVehicleAddPM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemVehicleAddPM.OldEntityPM = null;
                newSupplierInvoiceItemVehicleAddPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemVehicleAdds.push(newSupplierInvoiceItemVehicleAddPM);
        }
        if (oldSupplierInvoiceItemVehicleAdds) {

            for (var itemKey in oldSupplierInvoiceItemVehicleAdds) {
                if (entityPM.SupplierInvoiceItemVehicleAdds.filter(p => p.UniqueKey === oldSupplierInvoiceItemVehicleAdds[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemVehicleAdds[itemKey]) {
                        //oldSupplierInvoiceItemVehicleAdds[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemVehicleAdds.push(oldSupplierInvoiceItemVehicleAdds[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemVehicleAdds[itemKey];
                        var deletedPM: SupplierInvoiceItemVehicleAddPM = new SupplierInvoiceItemVehicleAddPM(null);
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
                        entityPM.SupplierInvoiceItemVehicleAdds.push(deletedPM);
                    }
                }
            }
        }
    }

    MapSupplierInvoiceItemModVehicles(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemModVehicles: SupplierInvoiceItemModVehiclePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemModVehicles = entityPM.OldEntityPM.SupplierInvoiceItemModVehicles;
        }

        entityPM.SupplierInvoiceItemModVehicles = new Array<SupplierInvoiceItemModVehiclePM>();
        for (var item in jsonPM.SupplierInvoiceItemModVehicles) {
            var jItem = jsonPM.SupplierInvoiceItemModVehicles[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemModVehiclePM: SupplierInvoiceItemModVehiclePM;

            if (mapParent) {
                newSupplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM(entityPM);
            }
            else {
                newSupplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemModVehiclePM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemModVehiclePM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemModVehiclePM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemModVehiclePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemModVehiclePM.OldEntityPM = this.clone(newSupplierInvoiceItemModVehiclePM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemModVehiclePM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemModVehiclePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemModVehiclePM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemModVehiclePM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemModVehiclePM.OldEntityPM = null;
                newSupplierInvoiceItemModVehiclePM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemModVehicles.push(newSupplierInvoiceItemModVehiclePM);
        }
        if (oldSupplierInvoiceItemModVehicles) {

            for (var itemKey in oldSupplierInvoiceItemModVehicles) {
                if (entityPM.SupplierInvoiceItemModVehicles.filter(p => p.UniqueKey === oldSupplierInvoiceItemModVehicles[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemModVehicles[itemKey]) {
                        //oldSupplierInvoiceItemModVehicles[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemModVehicles.push(oldSupplierInvoiceItemModVehicles[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemModVehicles[itemKey];
                        var deletedPM: SupplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM(null);
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
                        entityPM.SupplierInvoiceItemModVehicles.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSuppInvoiceItemsAbachStatements(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSuppInvoiceItemsAbachStatements: SuppInvoiceItemsAbachStatementPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSuppInvoiceItemsAbachStatements = entityPM.OldEntityPM.SuppInvoiceItemsAbachStatements;
        }

        entityPM.SuppInvoiceItemsAbachStatements = new Array<SuppInvoiceItemsAbachStatementPM>();
        for (var item in jsonPM.SuppInvoiceItemsAbachStatements) {
            var jItem = jsonPM.SuppInvoiceItemsAbachStatements[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSuppInvoiceItemsAbachStatementPM: SuppInvoiceItemsAbachStatementPM;

            if (mapParent) {
                newSuppInvoiceItemsAbachStatementPM = new SuppInvoiceItemsAbachStatementPM(entityPM);
            }
            else {
                newSuppInvoiceItemsAbachStatementPM = new SuppInvoiceItemsAbachStatementPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSuppInvoiceItemsAbachStatementPM[pmProperty] = jItem[pmProperty];
            }
            newSuppInvoiceItemsAbachStatementPM.IsDirty = false;

            if (mapParent) {
                newSuppInvoiceItemsAbachStatementPM.UniqueKey = Guid.newGuid();
                newSuppInvoiceItemsAbachStatementPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSuppInvoiceItemsAbachStatementPM.OldEntityPM = this.clone(newSuppInvoiceItemsAbachStatementPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSuppInvoiceItemsAbachStatementPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSuppInvoiceItemsAbachStatementPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSuppInvoiceItemsAbachStatementPM.ChangeSetOp = "Update";
                    }
                    else {
                        newSuppInvoiceItemsAbachStatementPM.ChangeSetOp = "Insert";
                    }
                }

                newSuppInvoiceItemsAbachStatementPM.OldEntityPM = null;
                newSuppInvoiceItemsAbachStatementPM.EntityParentPM = null;
            }


            entityPM.SuppInvoiceItemsAbachStatements.push(newSuppInvoiceItemsAbachStatementPM);
        }
        if (oldSuppInvoiceItemsAbachStatements) {

            for (var itemKey in oldSuppInvoiceItemsAbachStatements) {
                if (entityPM.SuppInvoiceItemsAbachStatements.filter(p => p.UniqueKey === oldSuppInvoiceItemsAbachStatements[itemKey].UniqueKey).length === 0) {

                    if (oldSuppInvoiceItemsAbachStatements[itemKey]) {
                        //oldSupplierInvoiceItemModVehicles[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemModVehicles.push(oldSupplierInvoiceItemModVehicles[itemKey]);
                        var oldItemJson = oldSuppInvoiceItemsAbachStatements[itemKey];
                        var deletedPM: SuppInvoiceItemsAbachStatementPM = new SuppInvoiceItemsAbachStatementPM(null);
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
                        entityPM.SuppInvoiceItemsAbachStatements.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceItemsPrices(entityPM: SupplierInvoiceItemPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemsPrices: SupplierInvoiceItemsPricePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemsPrices = entityPM.OldEntityPM.SupplierInvoiceItemsPrices;
        }

        entityPM.SupplierInvoiceItemsPrices = new Array<SupplierInvoiceItemsPricePM>();
        for (var item in jsonPM.SupplierInvoiceItemsPrices) {
            var jItem = jsonPM.SupplierInvoiceItemsPrices[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsPricePM: SupplierInvoiceItemsPricePM;

            if (mapParent) {
                newSupplierInvoiceItemsPricePM = new SupplierInvoiceItemsPricePM(entityPM);
            }
            else {
                newSupplierInvoiceItemsPricePM = new SupplierInvoiceItemsPricePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsPricePM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceItemsPricePM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceItemsPricePM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsPricePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsPricePM.OldEntityPM = this.clone(newSupplierInvoiceItemsPricePM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newSupplierInvoiceItemsPricePM.ChangeSetOp = "Delete";
                }
                else {
                    if (newSupplierInvoiceItemsPricePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newSupplierInvoiceItemsPricePM.ChangeSetOp = "Update";
                    }
                    else {
                        newSupplierInvoiceItemsPricePM.ChangeSetOp = "Insert";
                    }
                }

                newSupplierInvoiceItemsPricePM.OldEntityPM = null;
                newSupplierInvoiceItemsPricePM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceItemsPrices.push(newSupplierInvoiceItemsPricePM);
        }
        if (oldSupplierInvoiceItemsPrices) {

            for (var itemKey in oldSupplierInvoiceItemsPrices) {
                if (entityPM.SupplierInvoiceItemsPrices.filter(p => p.UniqueKey === oldSupplierInvoiceItemsPrices[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemsPrices[itemKey]) {
                        //oldSupplierInvoiceItemModVehicles[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemModVehicles.push(oldSupplierInvoiceItemModVehicles[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemsPrices[itemKey];
                        var deletedPM: SupplierInvoiceItemsPricePM = new SupplierInvoiceItemsPricePM(null);
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
                        entityPM.SupplierInvoiceItemsPrices.push(deletedPM);
                    }
                }
            }
        }
    }

    MapSupplierInvoiceModifications(entityPM: SupplierInvoicePM, jsonPM: any, mapParent: boolean = true) {
         var oldSupplierInvoiceModifications: SupplierInvoiceModificationPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceModifications = entityPM.OldEntityPM.SupplierInvoiceModifications;
        }

        entityPM.SupplierInvoiceModifications = new Array<SupplierInvoiceModificationPM>();
        for (var item in jsonPM.SupplierInvoiceModifications) {
            var jItem = jsonPM.SupplierInvoiceModifications[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceModificationPM: SupplierInvoiceModificationPM;

            if (mapParent) {
                newSupplierInvoiceModificationPM = new SupplierInvoiceModificationPM(entityPM);
            }
            else {
                newSupplierInvoiceModificationPM = new SupplierInvoiceModificationPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceModificationPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceModificationPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceModificationPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceModificationPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceModificationPM.OldEntityPM = this.clone(newSupplierInvoiceModificationPM);


            }
            else {
                if (newSupplierInvoiceModificationPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSupplierInvoiceModificationPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoiceModificationPM.ChangeSetOp = "Insert";
                }

                newSupplierInvoiceModificationPM.OldEntityPM = null;
                newSupplierInvoiceModificationPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceModifications.push(newSupplierInvoiceModificationPM);
        }
        if (oldSupplierInvoiceModifications) {

            for (var itemKey in oldSupplierInvoiceModifications) {
                if (entityPM.SupplierInvoiceModifications.filter(p => p.UniqueKey === oldSupplierInvoiceModifications[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceModifications[itemKey]) {
                        //oldSupplierInvoiceModifications[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceModifications.push(oldSupplierInvoiceModifications[itemKey]);
                        var oldItemJson = oldSupplierInvoiceModifications[itemKey];
                        var deletedPM: SupplierInvoiceModificationPM = new SupplierInvoiceModificationPM(null);
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
                        entityPM.SupplierInvoiceModifications.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceFreightAmounts(entityPM: SupplierInvoicePM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceFreightAmounts: SupplierInvoiceFreightAmountPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceFreightAmounts = entityPM.OldEntityPM.SupplierInvoiceFreightAmounts;
        }

        entityPM.SupplierInvoiceFreightAmounts = new Array<SupplierInvoiceFreightAmountPM>();
        for (var item in jsonPM.SupplierInvoiceFreightAmounts) {
            var jItem = jsonPM.SupplierInvoiceFreightAmounts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceFreightAmountPM: SupplierInvoiceFreightAmountPM;

            if (mapParent) {
                newSupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(entityPM);
            }
            else {
                newSupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceFreightAmountPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceFreightAmountPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceFreightAmountPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceFreightAmountPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceFreightAmountPM.OldEntityPM = this.clone(newSupplierInvoiceFreightAmountPM);


            }
            else {
                if (newSupplierInvoiceFreightAmountPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSupplierInvoiceFreightAmountPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoiceFreightAmountPM.ChangeSetOp = "Insert";
                }

                newSupplierInvoiceFreightAmountPM.OldEntityPM = null;
                newSupplierInvoiceFreightAmountPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceFreightAmounts.push(newSupplierInvoiceFreightAmountPM);
        }
        if (oldSupplierInvoiceFreightAmounts) {

            for (var itemKey in oldSupplierInvoiceFreightAmounts) {
                if (entityPM.SupplierInvoiceFreightAmounts.filter(p => p.UniqueKey === oldSupplierInvoiceFreightAmounts[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceFreightAmounts[itemKey]) {
                        //oldSupplierInvoiceFreightAmounts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceFreightAmounts.push(oldSupplierInvoiceFreightAmounts[itemKey]);
                        var oldItemJson = oldSupplierInvoiceFreightAmounts[itemKey];
                        var deletedPM: SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM(null);
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
                        entityPM.SupplierInvoiceFreightAmounts.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoicePayments(entityPM: SupplierInvoicePM, jsonPM: any, mapParent: boolean = true) {
         var oldSupplierInvoicePayments: SupplierInvoicePaymentPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoicePayments = entityPM.OldEntityPM.SupplierInvoicePayments;
        }

        entityPM.SupplierInvoicePayments = new Array<SupplierInvoicePaymentPM>();
        for (var item in jsonPM.SupplierInvoicePayments) {
            var jItem = jsonPM.SupplierInvoicePayments[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoicePaymentPM: SupplierInvoicePaymentPM;

            if (mapParent) {
                newSupplierInvoicePaymentPM = new SupplierInvoicePaymentPM(entityPM);
            }
            else {
                newSupplierInvoicePaymentPM = new SupplierInvoicePaymentPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoicePaymentPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoicePaymentPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoicePaymentPM.UniqueKey = Guid.newGuid();
                newSupplierInvoicePaymentPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoicePaymentPM.OldEntityPM = this.clone(newSupplierInvoicePaymentPM);


            }
            else {
                if (newSupplierInvoicePaymentPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSupplierInvoicePaymentPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoicePaymentPM.ChangeSetOp = "Insert";
                }

                newSupplierInvoicePaymentPM.OldEntityPM = null;
                newSupplierInvoicePaymentPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoicePayments.push(newSupplierInvoicePaymentPM);
        }
        if (oldSupplierInvoicePayments) {

            for (var itemKey in oldSupplierInvoicePayments) {
                if (entityPM.SupplierInvoicePayments.filter(p => p.UniqueKey === oldSupplierInvoicePayments[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoicePayments[itemKey]) {
                        //oldSupplierInvoiceFreightAmounts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceFreightAmounts.push(oldSupplierInvoiceFreightAmounts[itemKey]);
                        var oldItemJson = oldSupplierInvoicePayments[itemKey];
                        var deletedPM: SupplierInvoicePaymentPM = new SupplierInvoicePaymentPM(null);
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
                        entityPM.SupplierInvoicePayments.push(deletedPM);
                    }
                }
            }
        }
    }
    MapSupplierInvoiceUCRs(entityPM: SupplierInvoicePM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceUCRs: SupplierInvoiceUCRPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceUCRs = entityPM.OldEntityPM.SupplierInvoiceUCRs;
        }

        entityPM.SupplierInvoiceUCRs = new Array<SupplierInvoiceUCRPM>();
        for (var item in jsonPM.SupplierInvoiceUCRs) {
            var jItem = jsonPM.SupplierInvoiceUCRs[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceUCRPM: SupplierInvoiceUCRPM;

            if (mapParent) {
                newSupplierInvoiceUCRPM = new SupplierInvoiceUCRPM(entityPM);
            }
            else {
                newSupplierInvoiceUCRPM = new SupplierInvoiceUCRPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceUCRPM[pmProperty] = jItem[pmProperty];
            }
            newSupplierInvoiceUCRPM.IsDirty = false;

            if (mapParent) {
                newSupplierInvoiceUCRPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceUCRPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceUCRPM.OldEntityPM = this.clone(newSupplierInvoiceUCRPM);


            }
            else {
                if (newSupplierInvoiceUCRPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSupplierInvoiceUCRPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoiceUCRPM.ChangeSetOp = "Insert";
                }

                newSupplierInvoiceUCRPM.OldEntityPM = null;
                newSupplierInvoiceUCRPM.EntityParentPM = null;
            }


            entityPM.SupplierInvoiceUCRs.push(newSupplierInvoiceUCRPM);
        }
        if (oldSupplierInvoiceUCRs) {

            for (var itemKey in oldSupplierInvoiceUCRs) {
                if (entityPM.SupplierInvoiceUCRs.filter(p => p.UniqueKey === oldSupplierInvoiceUCRs[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceUCRs[itemKey]) {
                        //oldSupplierInvoiceFreightAmounts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceFreightAmounts.push(oldSupplierInvoiceFreightAmounts[itemKey]);
                        var oldItemJson = oldSupplierInvoiceUCRs[itemKey];
                        var deletedPM: SupplierInvoiceUCRPM = new SupplierInvoiceUCRPM(null);
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
                        entityPM.SupplierInvoiceUCRs.push(deletedPM);
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
        var entityPM: SupplierInvoicePM;
        entityPM = new SupplierInvoicePM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }

    MapJsonToImporterDesposition(json: any, mapParent: boolean = true, entity: ImporterDespositionClass = null) {


        if (!entity) {

            entity = new ImporterDespositionClass();
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
}
