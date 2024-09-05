
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ShipmentPM } from '../../EntityPMs/ShipmentPM';
import { AWBOCIPM } from '../../EntityPMs/AWBOCIPM';
import { ShipmentPackagePM } from '../../EntityPMs/ShipmentPackagePM';
import { InsideShipmentPackagePM } from '../../EntityPMs/InsideShipmentPackagePM';
import { ShipmentCommodityPM } from '../../EntityPMs/ShipmentCommodityPM';
import { ShipmentOrderPackagePM } from '../../EntityPMs/ShipmentOrderPackagePM';
import { ShipmentPayablePM } from '../../EntityPMs/ShipmentPayablePM';
import { ShipmentReceivablePM } from '../../EntityPMs/ShipmentReceivablePM';
import { ShipmentAWBPrintOnlyPM } from '../../EntityPMs/ShipmentAWBPrintOnlyPM';
import { ConsoleShipmentPM } from '../../EntityPMs/ConsoleShipmentPM';
import { ShipmentPickUpPM } from '../../EntityPMs/ShipmentPickUpPM';
import { ShipmentDeliveryPM } from '../../EntityPMs/ShipmentDeliveryPM';
import { ShipmentPickUpDeliveryPackagePM } from '../../EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { ShipmentPackageItemPM } from '../../EntityPMs/ShipmentPackageItemPM';
import { ShipmentPackageHarmonizePM } from '../../EntityPMs/ShipmentPackageHarmonizePM';
import { ShipmentFollowUpPM } from '../../EntityPMs/ShipmentFollowUpPM';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { ShipmentValidator } from '../../Validators/ShipmentValidator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ShipmentPMInitService } from '../../EntityPMInitServices/ShipmentPMInitService';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { ShipmentAssemblyPM } from '../../EntityPMs/ShipmentAssemblyPM';
import { ShipmentStoragePricingPM } from '../../EntityPMs/ShipmentStoragePricingPM';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { PickUpDeliveryPackageHarmonizePM } from '../../EntityPMs/PickUpDeliveryPackageHarmonizePM';
import { CommodityPackagePM } from '../../EntityPMs/CommodityPackagePM';
import { ShipmentProductItemPM } from '../../EntityPMs/ShipmentProductItemPM';
import { ShipmentUnassignedFieldPM } from '../../EntityPMs/ShipmentUnassignedFieldPM';
import { CustomChildObjectPMService } from '../../../Infrastructure/Services/ExtendedPMs/CustomChildObjectPMService';
import { JsonPatchBuilder } from 'Infrastructure/Helpers/JsonPatchBuilder';
import { AppTool } from 'Infrastructure/Tools';

@Injectable()

export class ShipmentPMService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/shipment';
    }

    get(id: string) {

        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingle?id=' + id, ServiceHelper.GetHttpFullHeaders()).pipe(
                map((response: HttpResponse<any>) => {

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "GetSinglePM", id);

                    var pm = response.body;
                    var entity: ShipmentPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                        ShipmentPMInitService.ApplyUIPoperties(entity, false);
                    }
                    // ServiceLocator.RulesValidator.ApplyAllEntityStaticRules(entity, "Shipment");
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;
                    return pmresponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });

        // .flatMap((res: Response) => {
        //    var location = res.headers.get('Location');
        //    return this._http.get(location);
        //}).map((res: Response) => res.json()))
        //.catch(this.handleError)

        /*
        .flatMap((res: Response) => {
                var serverTime = res.headers.get('ServerTime');
                return this._http.get(serverTime);
            })
        */
    }

    CheckIsCFSShipmentById(shipmentId: string) {
        return defer(() => {
            return this._http.get(this._apiUrl + '/CheckIsCFSShipmentById?shipmentId=' + shipmentId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var isCFS = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = isCFS;
                return pmresponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getSingleByForwarderShipmentNumber(ForwarderShipmentNumber: string) {

        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleByForwarderShipmentNumber?fsn=' + ForwarderShipmentNumber, ServiceHelper.GetHttpFullHeaders()).pipe(
                map((response: HttpResponse<any>) => {

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "getSingleByForwarderShipmentNumber", ForwarderShipmentNumber);

                    var pm = response.body;
                    var entity: ShipmentPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;
                    return pmresponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });

        // .flatMap((res: Response) => {
        //    var location = res.headers.get('Location');
        //    return this._http.get(location);
        //}).map((res: Response) => res.json()))
        //.catch(this.handleError)

        /*
        .flatMap((res: Response) => {
                var serverTime = res.headers.get('ServerTime');
                return this._http.get(serverTime);
            })
        */
    }

    getSingleBySecurityKeyTenantWithoutToken(SecurityKey: string, Tenant: number) {

        var myCustomURL = "https://systemwr.amital.co.il/api/shipment";
        //var myAuthHeader = new Headers();
        //myAuthHeader.append('Content-Type', 'application/json');
        //myAuthHeader.append('Accept', 'application/json');
        //myAuthHeader.append('token', SessionInfo.Token);
        //loginService.AuthHeader = myAuthHeader;
        //loginService.GetGlobalSetting().subscribe(Setting => {
        //    if (Setting) {
        //        if (Setting.DeploymentStage == "amitalstorage") {
        //            var myCustomURL = "http://13.93.36.4/api/shipment";
        //        }
        //    }
        //});
        if (location.href.indexOf('localhost') > -1 || location.href.indexOf('test') > -1) {
            myCustomURL = this._apiUrl;
        }

        //var key = PerformanceLogger.AddLogTime();
        var callTime = new Date();
        return defer(() => {
            var url = myCustomURL + '/GetSingleBySecurityKeyWithoutToken?key=' + SecurityKey;

            if (!AppTool.IsNullOrUndefined(Tenant)) {
                url += '&tenant=' + Tenant;
            }
            return this._http.get(url, ServiceHelper.GetHttpFullHeadersWithoutToken()).pipe(
                map((response: HttpResponse<any>) => {

                    //var servertime = response.headers.get('ServerExecutionTime');
                    //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "getSingleBySecurityKey", SecurityKey);

                    var pm = response.body;
                    var entity: ShipmentPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;
              
                pmresponse.Data = {};
                pmresponse.Data.WhatsAppMessagingPhoneNumber = response.headers.get('WhatsAppMessagingPhoneNumber');
                pmresponse.Data.TranzilaPaymentWithBit = response.headers.get('TranzilaPaymentWithBit');

                    return pmresponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });

        // .flatMap((res: Response) => {
        //    var location = res.headers.get('Location');
        //    return this._http.get(location);
        //}).map((res: Response) => res.json()))
        //.catch(this.handleError)

        /*
        .flatMap((res: Response) => {
                var serverTime = res.headers.get('ServerTime');
                return this._http.get(serverTime);
            })
        */
    }
   
    getLogoAndUrlWithoutToken(securityKey: string): Promise<UrlAndLogo> {
        const url = (location.href.indexOf('localhost') > -1 || location.href.indexOf('test') > -1) ? this._apiUrl : "https://systemwr.amital.co.il/api/shipment";
        return this._http.get(url + '/GetLogoAndUrlWithoutToken', { params: { securityKey: securityKey } }).toPromise() as Promise<UrlAndLogo>;
    }

    getUserIdDetailsByShipmentSecurityKeyWithoutToken(SecurityKey: string, Tenant: number) {


        //var key = PerformanceLogger.AddLogTime();
        var callTime = new Date();
        var url = this._apiUrl + '/getUserIdDetailsByShipmentSecurityKeyWithoutToken?key=' + SecurityKey;

        if (!AppTool.IsNullOrUndefined(Tenant)) {
            url += '&tenant=' + Tenant;
        }
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeadersWithoutToken()).pipe(
                map(response => {
                    //var servertime = response.headers.get('ServerExecutionTime');
                    //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "getSingleBySecurityKey", SecurityKey);

                    var pm = response;
                    //var entity: ShipmentPM;
                    //if (pm) {
                    //    entity = this.MapJsonToEntityPM(pm);
                    //}
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = pm;
                    return pmresponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });


    }

    getSingleByShipmentNumber(number: string) {
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getSingleByShipmentNumber?number=' + number, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var Id = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = Id;
                return pmresponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSingleByCustomerReference1(CustomerReference1: string, IsForwarderShipment: boolean = true) {

        //var key = PerformanceLogger.AddLogTime();
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleByCustomerReference1?CustomerReference1=' + CustomerReference1 + '&IsForwarderShipment=' + IsForwarderShipment, ServiceHelper.GetHttpFullHeaders()).pipe(
                map((response: HttpResponse<any>) => {

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "GetSingleByCustomerReference1", CustomerReference1);

                    var pm = response.body;
                    var entity: ShipmentPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;
                    return pmresponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });

        // .flatMap((res: Response) => {
        //    var location = res.headers.get('Location');
        //    return this._http.get(location);
        //}).map((res: Response) => res.json()))
        //.catch(this.handleError)

        /*
        .flatMap((res: Response) => {
                var serverTime = res.headers.get('ServerTime');
                return this._http.get(serverTime);
            })
        */
    }
    GetByCustomerReferences1or3(CustomerReference: string, IsForwarderShipment: boolean = true) {

        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetByCustomerReferences1or3?CustomerReference1=' + CustomerReference + '&IsForwarderShipment=' + IsForwarderShipment, ServiceHelper.GetHttpFullHeaders()).pipe(
                map((response: HttpResponse<any>) => {

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "GetByCustomerReferences1or3", CustomerReference);

                    var pm = response.body;
                    var entity: ShipmentPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;
                    return pmresponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });

        // .flatMap((res: Response) => {
        //    var location = res.headers.get('Location');
        //    return this._http.get(location);
        //}).map((res: Response) => res.json()))
        //.catch(this.handleError)

        /*
        .flatMap((res: Response) => {
                var serverTime = res.headers.get('ServerTime');
                return this._http.get(serverTime);
            })
        */
    }
    GetByCustomerReferences1or3ForUpdate(CustomerReference: string, ShipmentId: string, IsForwarderShipment: boolean = true) {

        //var key = PerformanceLogger.AddLogTime();
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetByCustomerReferences1or3ForUpdate?CustomerReference1=' + CustomerReference + '&ShipmentId=' + ShipmentId + '&IsForwarderShipment=' + IsForwarderShipment, ServiceHelper.GetHttpFullHeaders()).pipe(
                map((response: HttpResponse<any>) => {

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "GetByCustomerReferences1or3ForUpdate", CustomerReference);

                    var pm = response.body;
                    var entity: ShipmentPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }
                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;
                    return pmresponse;

                }), catchError(ServiceHelper.HandleServiceError));
        });

        // .flatMap((res: Response) => {
        //    var location = res.headers.get('Location');
        //    return this._http.get(location);
        //}).map((res: Response) => res.json()))
        //.catch(this.handleError)

        /*
        .flatMap((res: Response) => {
                var serverTime = res.headers.get('ServerTime');
                return this._http.get(serverTime);
            })
        */
    }
    insert(entityPM: ShipmentPM) {

        return defer(() => {

            var validator: ClassLevelValidator = new ClassLevelValidator();
            var entityValidator: ShipmentValidator = new ShipmentValidator();

            var errors = validator.Validate("Shipment", entityPM);
            var entityErrors = entityValidator.Validate(entityPM);

            if (entityErrors) {
                errors = errors.concat(entityErrors);
            }

            var response: ServiceResponse;
            response = new ServiceResponse();

            if (errors.length == 0) {
                var shipment: ShipmentPM;
                shipment = this.MapJsonToEntityPM(entityPM, false);
                var shipString: string;
                shipString = JSON.stringify(shipment);
                //console.log(shipString);
                return this._http.post(this._apiUrl, shipString, ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var pm = res;
                    var shipment: ShipmentPM;
                    shipment = this.MapJsonToEntityPM(pm, true, entityPM);

                    response.Result = shipment;
                    return response;

                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errors;

                return of(response);

            }
        });
    }
    update(entityPM: ShipmentPM, oldEntityPM: ShipmentPM | null = null) {
        return defer(() => {

            var validator: ClassLevelValidator = new ClassLevelValidator();
            var entityValidator: ShipmentValidator = new ShipmentValidator();

            var errors = validator.Validate("Shipment", entityPM);
            var entityErrors = entityValidator.Validate(entityPM);

            if (entityErrors) {
                errors = errors.concat(entityErrors);
            }

            var response: ServiceResponse;
            response = new ServiceResponse();
            //errorsArray = [];
            if (errors.length == 0) {
                var httpRequest: any;

                if (oldEntityPM) {
                    var mappedEntityPM: ShipmentPM = this.MapJsonToEntityPM(entityPM, false);

                    var shipmentPatch = new JsonPatchBuilder(oldEntityPM, mappedEntityPM).build();

                    var shipmentPatchString = shipmentPatch ? JSON.stringify(shipmentPatch) : null;
                    var patchRequestUrl = this._apiUrl + "?id=" + mappedEntityPM.Id;

                    httpRequest = this._http.patch(patchRequestUrl, shipmentPatchString, ServiceHelper.GetHttpHeaders());
                } else {
                    var shipment: ShipmentPM;
                    shipment = this.MapJsonToEntityPM(entityPM, false);
                    var shipString: string;

                    shipString = JSON.stringify(shipment);
                    //console.log(shipString);

                    httpRequest = this._http.put(this._apiUrl, shipString, ServiceHelper.GetHttpHeaders());
                }

                return httpRequest.pipe(map((res) => {
                    var pm = res;
                    var shipment: ShipmentPM;
                    shipment = this.MapJsonToEntityPM(pm, true, entityPM);

                    response.Result = shipment;
                    return response;

                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                response.HasError = true;
                response.ErrorsArray = errors;

                return of(response);

            }
        });
    }
    GetNewEntityPM() {
        var entityPM: ShipmentPM;
        entityPM = new ShipmentPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        ShipmentPMInitService.InitValues(entityPM, true);
        ShipmentPMInitService.ApplyUIPoperties(entityPM, false);
        return entityPM;
    }
    clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        try {
            var jsonPMKeys = Object.keys(jsonPM);
            for (var key in jsonPMKeys) {

                if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                    continue;
                }

                var property = jsonPMKeys[key];
                entityPM[property] = jsonPM[property];

            }
        }

        catch (e) {
            var d = jsonPM;
        }

        return entityPM;
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ShipmentPM = null) {
        var customFields: Array<string> = [];
        for (var i = 1; i < 71; i++) {
            customFields.push("Field" + i);
        }
        if (!entityPM) {
            entityPM = new ShipmentPM();
            entityPM.DisableMarkAsDirty = true;
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

        this.MapShipmentOCIs(entityPM, jsonPM, mapParent);
        this.MapShipmentPackages(entityPM, jsonPM, mapParent);
        this.MapShipmentCommodities(entityPM, jsonPM, mapParent);
        this.MapShipmentOrderPackages(entityPM, jsonPM, mapParent);
        this.MapShipmentPayables(entityPM, jsonPM, mapParent);
        this.MapShipmentReceivables(entityPM, jsonPM, mapParent);
        this.MapShipmentAWBPrintOnlies(entityPM, jsonPM, mapParent);
        this.MapShipmentConsoleShipments(entityPM, jsonPM, mapParent);
        this.MapShipmentPickups(entityPM, jsonPM, mapParent);
        this.MapShipmentDeliveries(entityPM, jsonPM, mapParent);
        this.MapShipmentFollowups(entityPM, jsonPM, mapParent);
        this.MapShipmentAssemblies(entityPM, jsonPM, mapParent);
        this.MapShipmentStoragePricings(entityPM, jsonPM, mapParent);
        this.MapShipmentProductItems(entityPM, jsonPM, mapParent);
        this.MapShipmentUnassignedFields(entityPM, jsonPM, mapParent);

        let customChildObjectPMService: CustomChildObjectPMService = new CustomChildObjectPMService(entityPM, "Shipment");
        customChildObjectPMService.MapCustomChildEntities(jsonPM, mapParent);

        entityPM.IsDirty = false;
        if (mapParent) {

            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.AWBOCIPMs = [];
            for (var item in entityPM.AWBOCIPMs) {
                entityPM.OldEntityPM.AWBOCIPMs.push(this.clone(entityPM.AWBOCIPMs[item]));
            }

            entityPM.OldEntityPM.ShipmentOrderPackages = [];
            for (var item in entityPM.ShipmentOrderPackages) {
                entityPM.OldEntityPM.ShipmentOrderPackages.push(this.clone(entityPM.ShipmentOrderPackages[item]));
            }

            entityPM.OldEntityPM.ShipmentPayables = [];
            for (var item in entityPM.ShipmentPayables) {
                entityPM.OldEntityPM.ShipmentPayables.push(this.clone(entityPM.ShipmentPayables[item]));
            }

            entityPM.OldEntityPM.ShipmentReceivables = [];
            for (var item in entityPM.ShipmentReceivables) {
                entityPM.OldEntityPM.ShipmentReceivables.push(this.clone(entityPM.ShipmentReceivables[item]));
            }

            entityPM.OldEntityPM.ShipmentAWBPrintOnlies = [];
            for (var item in entityPM.ShipmentAWBPrintOnlies) {
                entityPM.OldEntityPM.ShipmentAWBPrintOnlies.push(this.clone(entityPM.ShipmentAWBPrintOnlies[item]));
            }

            entityPM.OldEntityPM.ShipmentConsoleShipments = [];
            for (var item in entityPM.ShipmentConsoleShipments) {
                entityPM.OldEntityPM.ShipmentConsoleShipments.push(this.clone(entityPM.ShipmentConsoleShipments[item]));
            }

            entityPM.OldEntityPM.ShipmentPackages = [];
            for (var item in entityPM.ShipmentPackages) {

                var myShipmentPackage = entityPM.ShipmentPackages[item];
                var newPackage: ShipmentPackagePM = this.clone(myShipmentPackage);
                newPackage.InsideShipmentPackages = [];
                newPackage.ShipmentPackageItems = [];
                newPackage.ShipmentPackageHarmonizes = [];

                for (var k in myShipmentPackage.InsideShipmentPackages) {
                    var myInsideShipmentPackagePM = myShipmentPackage.InsideShipmentPackages[k];
                    var newInsideShipmentPackagePM: InsideShipmentPackagePM = this.clone(myInsideShipmentPackagePM);

                    newInsideShipmentPackagePM.InsidePackageHarmonizes = [];
                    for (var kk0 in myInsideShipmentPackagePM.InsidePackageHarmonizes) {
                        newInsideShipmentPackagePM.InsidePackageHarmonizes.push(this.clone(myInsideShipmentPackagePM.InsidePackageHarmonizes[kk0]));
                    }

                    for (var k in myInsideShipmentPackagePM.InsidePackageHarmonizes) {
                        var myInsidePackageHarmonizePM = myInsideShipmentPackagePM.InsidePackageHarmonizes[k];
                        var newInsidePackageHarmonizePM = this.clone(myInsidePackageHarmonizePM);
                        newInsideShipmentPackagePM.InsidePackageHarmonizes.push(newInsidePackageHarmonizePM);
                    }

                    newPackage.InsideShipmentPackages.push(newInsideShipmentPackagePM);
                }

                for (var k1 in myShipmentPackage.ShipmentPackageItems) {
                    newPackage.ShipmentPackageItems.push(this.clone(myShipmentPackage.ShipmentPackageItems[k1]));
                }

                for (var k3 in myShipmentPackage.ShipmentPackageHarmonizes) {
                    newPackage.ShipmentPackageHarmonizes.push(this.clone(myShipmentPackage.ShipmentPackageHarmonizes[k3]));
                }

                entityPM.OldEntityPM.ShipmentPackages.push(newPackage);
            }

            entityPM.OldEntityPM.ShipmentCommodities = [];
            for (var item in entityPM.ShipmentCommodities) {

                var myShipmentCommodity = entityPM.ShipmentCommodities[item];
                var newCommodity: ShipmentCommodityPM = this.clone(myShipmentCommodity);
                newCommodity.CommodityPackages = [];

                for (var k1 in myShipmentCommodity.CommodityPackages) {
                    newCommodity.CommodityPackages.push(this.clone(myShipmentCommodity.CommodityPackages[k1]));
                }

                entityPM.OldEntityPM.ShipmentCommodities.push(newCommodity);
            }

            entityPM.OldEntityPM.ShipmentPickUps = [];
            for (var item in entityPM.ShipmentPickUps) {

                var myShipmentPickup = entityPM.ShipmentPickUps[item];
                var newPickup: ShipmentPickUpPM = this.clone(myShipmentPickup);
                newPickup.ShipmentPickUpDeliveryPackages = [];

                for (var r in myShipmentPickup.ShipmentPickUpDeliveryPackages) {
                    var myPickUpPackage = myShipmentPickup.ShipmentPickUpDeliveryPackages[r];
                    var newPickUpPackage: ShipmentPickUpDeliveryPackagePM = this.clone(myPickUpPackage);
                    newPickUpPackage.PickUpDeliveryPackageHarmonizes = [];

                    for (var kk3 in myPickUpPackage.PickUpDeliveryPackageHarmonizes) {
                        newPickUpPackage.PickUpDeliveryPackageHarmonizes.push(this.clone(myPickUpPackage.PickUpDeliveryPackageHarmonizes[kk3]));
                    }

                    newPickup.ShipmentPickUpDeliveryPackages.push(myPickUpPackage);
                }

                entityPM.OldEntityPM.ShipmentPickUps.push(newPickup);
            }

            entityPM.OldEntityPM.ShipmentDeliveries = [];
            for (var item in entityPM.ShipmentDeliveries) {

                var myShipmentDelivery = entityPM.ShipmentDeliveries[item];
                var newDelivery: ShipmentDeliveryPM = this.clone(myShipmentDelivery);
                newDelivery.ShipmentPickUpDeliveryPackages = [];

                for (var k in myShipmentDelivery.ShipmentPickUpDeliveryPackages) {
                    var myDeliveryPackage = myShipmentDelivery.ShipmentPickUpDeliveryPackages[k];
                    var newDeliveryPackage: ShipmentPickUpDeliveryPackagePM = this.clone(myDeliveryPackage);
                    newDeliveryPackage.PickUpDeliveryPackageHarmonizes = [];

                    for (var kkk3 in myDeliveryPackage.PickUpDeliveryPackageHarmonizes) {
                        newDeliveryPackage.PickUpDeliveryPackageHarmonizes.push(this.clone(myDeliveryPackage.PickUpDeliveryPackageHarmonizes[kkk3]));
                    }

                    newDelivery.ShipmentPickUpDeliveryPackages.push(myDeliveryPackage);
                }

                entityPM.OldEntityPM.ShipmentDeliveries.push(newDelivery);
            }

            entityPM.OldEntityPM.FollowUps = [];
            for (var item in entityPM.FollowUps) {
                entityPM.OldEntityPM.FollowUps.push(this.clone(entityPM.FollowUps[item]));
            }

            entityPM.OldEntityPM.ShipmentAssemblies = [];
            for (var item in entityPM.ShipmentAssemblies) {
                entityPM.OldEntityPM.ShipmentAssemblies.push(this.clone(entityPM.ShipmentAssemblies[item]));
            }

            entityPM.OldEntityPM.ShipmentStoragePricings = [];
            for (var item in entityPM.ShipmentStoragePricings) {
                entityPM.OldEntityPM.ShipmentStoragePricings.push(this.clone(entityPM.ShipmentStoragePricings[item]));
            }

            entityPM.OldEntityPM.ShipmentProductItems = [];
            for (var item in entityPM.ShipmentProductItems) {
                entityPM.OldEntityPM.ShipmentProductItems.push(this.clone(entityPM.ShipmentProductItems[item]));
            }

            entityPM.OldEntityPM.ShipmentUnassignedFields = [];
            for (var item in entityPM.ShipmentUnassignedFields) {
                entityPM.OldEntityPM.ShipmentUnassignedFields.push(this.clone(entityPM.ShipmentUnassignedFields[item]));
            }
        }

        else {
            entityPM.OldEntityPM = null;
        }
        entityPM.DisableMarkAsDirty = false;
        return entityPM;
    }
    MapShipmentOCIs(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: AWBOCIPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.AWBOCIPMs;
        }

        entityPM.AWBOCIPMs = new Array<AWBOCIPM>();
        for (var item in jsonPM.AWBOCIPMs) {

            var itemJson = jsonPM.AWBOCIPMs[item];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: AWBOCIPM;
            if (mapParent) {
                itemPM = new AWBOCIPM(entityPM);
            }

            else {
                itemPM = new AWBOCIPM(null);
            }
            itemPM.DisableMarkAsDirty = true;
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            itemPM.IsDirty = false;
            itemPM.DisableMarkAsDirty = false;
            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }

            entityPM.AWBOCIPMs.push(itemPM);
        }

        if (oldCollection) {
            for (var item in oldCollection) {
                if (entityPM.AWBOCIPMs.filter(p => p.UniqueKey === oldCollection[item].UniqueKey).length === 0) {
                    if (oldCollection[item]) {
                        oldCollection[item].ChangeSetOp = "Delete";
                        entityPM.AWBOCIPMs.push(oldCollection[item]);
                    }
                }
            }
        }
    }
    MapShipmentPackages(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentPackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentPackages;
        }

        entityPM.ShipmentPackages = new Array<ShipmentPackagePM>();

        for (var pack in jsonPM.ShipmentPackages) {

            var itemJson = jsonPM.ShipmentPackages[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentPackagePM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPackagePM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentPackagePM(null);
            }
            itemPM.DisableMarkAsDirty = true;
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var customFields: Array<string> = [];
                for (var i = 1; i < 51; i++) {
                    customFields.push("Field" + i);
                }

                var property = pmKeys[key];
                if (customFields.indexOf(property) > -1) {
                    if (itemJson[property]) {
                        var customFieldClass: CustomFieldClass = new CustomFieldClass(itemJson[property].Value, itemJson[property].FieldName, itemJson[property].TableName);
                        itemPM[property] = customFieldClass;
                    }
                }
                else {
                    itemPM[property] = itemJson[property];
                }
            }


            if (mapParent) {

                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

                this.MapInsideShipmentPackages(itemPM, itemJson, mapParent);
                itemPM.OldEntityPM.InsideShipmentPackages = [];
                for (var k1 in itemPM.InsideShipmentPackages) {
                    var clonedInside = this.clone(itemPM.InsideShipmentPackages[k1]);
                    itemPM.OldEntityPM.InsideShipmentPackages.push(clonedInside); // clone old inside packages//
                }


                this.MapShipmentPackageItems(itemPM, itemJson, mapParent);
                itemPM.OldEntityPM.ShipmentPackageItems = [];
                for (var k2 in itemPM.ShipmentPackageItems) {
                    var clonedInside = this.clone(itemPM.ShipmentPackageItems[k2]);
                    itemPM.OldEntityPM.ShipmentPackageItems.push(clonedInside); // clone old inside packages//
                }

                this.MapShipmentPackageHarmonizes(itemPM, itemJson, mapParent);
                itemPM.OldEntityPM.ShipmentPackageHarmonizes = [];
                for (var k3 in itemPM.ShipmentPackageHarmonizes) {
                    var clonedInside = this.clone(itemPM.ShipmentPackageHarmonizes[k3]);
                    itemPM.OldEntityPM.ShipmentPackageHarmonizes.push(clonedInside);
                }
            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                this.MapInsideShipmentPackages(itemPM, itemJson, mapParent);
                this.MapShipmentPackageItems(itemPM, itemJson, mapParent);
                this.MapShipmentPackageHarmonizes(itemPM, itemJson, mapParent);
                itemPM.EntityParentPM = null;
                itemPM.OldEntityPM = null;
            }

            itemPM.DisableMarkAsDirty = false;
            itemPM.IsDirty = false;
            entityPM.ShipmentPackages.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.ShipmentPackages.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        var oldpackageJson = oldCollection[pack];
                        var deletedPM: ShipmentPackagePM = new ShipmentPackagePM(null);
                        deletedPM.DisableMarkAsDirty = true;
                        var pmKeys = Object.keys(oldpackageJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldpackageJson[property];
                        }

                        deletedPM.DisableMarkAsDirty = false;
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        this.MapInsideShipmentPackages(deletedPM, oldpackageJson, mapParent);
                        this.MapShipmentPackageItems(deletedPM, oldpackageJson, mapParent);
                        this.MapShipmentPackageHarmonizes(deletedPM, oldpackageJson, mapParent);

                        deletedPM.OldEntityPM = null;
                        entityPM.ShipmentPackages.push(deletedPM);
                    }
                }
            }
        }
    }
    MapShipmentCommodities(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentCommodityPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentCommodities;
        }

        entityPM.ShipmentCommodities = new Array<ShipmentCommodityPM>();

        for (var pack in jsonPM.ShipmentCommodities) {

            var itemJson = jsonPM.ShipmentCommodities[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentCommodityPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentCommodityPM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentCommodityPM(null);
            }
            itemPM.DisableMarkAsDirty = true;
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {

                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

                this.MapShipmentCommodityPackages(itemPM, itemJson, mapParent);
                itemPM.OldEntityPM.CommodityPackages = [];
                for (var k2 in itemPM.CommodityPackages) {
                    var clonedInside = this.clone(itemPM.CommodityPackages[k2]);
                    itemPM.OldEntityPM.CommodityPackages.push(clonedInside);
                }
            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                this.MapShipmentCommodityPackages(itemPM, itemJson, mapParent);
                itemPM.EntityParentPM = null;
                itemPM.OldEntityPM = null;
            }

            itemPM.DisableMarkAsDirty = false;
            itemPM.IsDirty = false;
            entityPM.ShipmentCommodities.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.ShipmentCommodities.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        var oldpackageJson = oldCollection[pack];
                        var deletedPM: ShipmentCommodityPM = new ShipmentCommodityPM(null);
                        deletedPM.DisableMarkAsDirty = true;
                        var pmKeys = Object.keys(oldpackageJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldpackageJson[property];
                        }

                        deletedPM.DisableMarkAsDirty = false;
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        this.MapShipmentCommodityPackages(deletedPM, oldpackageJson, mapParent);

                        deletedPM.OldEntityPM = null;
                        entityPM.ShipmentCommodities.push(deletedPM);
                    }
                }
            }
        }
    }
    MapShipmentOrderPackages(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {
        var oldCollection: ShipmentOrderPackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentOrderPackages;
        }

        entityPM.ShipmentOrderPackages = new Array<ShipmentOrderPackagePM>();
        for (var item in jsonPM.ShipmentOrderPackages) {

            var itemJson = jsonPM.ShipmentOrderPackages[item];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentOrderPackagePM;
            if (mapParent) {
                itemPM = new ShipmentOrderPackagePM(entityPM);
            }

            else {
                itemPM = new ShipmentOrderPackagePM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentOrderPackages.push(itemPM);
        }

        if (oldCollection) {
            for (var item in oldCollection) {
                if (entityPM.ShipmentOrderPackages.filter(p => p.UniqueKey === oldCollection[item].UniqueKey).length === 0) {
                    if (oldCollection[item]) {
                        oldCollection[item].ChangeSetOp = "Delete";
                        entityPM.ShipmentOrderPackages.push(oldCollection[item]);
                    }
                }
            }
        }
    }
    MapShipmentPayables(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldPayables: ShipmentPayablePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldPayables = entityPM.OldEntityPM.ShipmentPayables;
        }

        entityPM.ShipmentPayables = new Array<ShipmentPayablePM>();

        for (var pack in jsonPM.ShipmentPayables) {

            var itemJson = jsonPM.ShipmentPayables[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentPayablePM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPayablePM(entityPM);

            }
            else {// update mapping

                itemPM = new ShipmentPayablePM(null);


            }


            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var customFields: Array<string> = [];
                for (var i = 1; i < 51; i++) {
                    customFields.push("Field" + i);
                }

                var property = pmKeys[key];
                if (customFields.indexOf(property) > -1) {
                    if (itemJson[property]) {
                        var customFieldClass: CustomFieldClass = new CustomFieldClass(itemJson[property].Value, itemJson[property].FieldName, itemJson[property].TableName);
                        itemPM[property] = customFieldClass;
                    }
                }
                else {
                    itemPM[property] = itemJson[property];
                }
            }


            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }
            else {

                if (itemPM.UniqueKey) {

                    if (itemJson.IsDirty)
                        itemPM.ChangeSetOp = "Update";
                }
                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentPayables.push(itemPM);

        }

        if (oldPayables) {

            for (var pack in oldPayables) {
                if (entityPM.ShipmentPayables.filter(p => p.UniqueKey === oldPayables[pack].UniqueKey).length === 0) {
                    if (oldPayables[pack]) {
                        oldPayables[pack].ChangeSetOp = "Delete";
                        entityPM.ShipmentPayables.push(oldPayables[pack]);
                    }
                }
            }
        }
    }
    MapShipmentReceivables(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldReceivables: ShipmentReceivablePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldReceivables = entityPM.OldEntityPM.ShipmentReceivables;
        }
        entityPM.ShipmentReceivables = new Array<ShipmentReceivablePM>();
        for (var pack in jsonPM.ShipmentReceivables) {

            var itemJson = jsonPM.ShipmentReceivables[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentReceivablePM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentReceivablePM(entityPM);

            }
            else {// update mapping

                itemPM = new ShipmentReceivablePM(null);


            }


            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var customFields: Array<string> = [];
                for (var i = 1; i < 51; i++) {
                    customFields.push("Field" + i);
                }

                var property = pmKeys[key];
                if (customFields.indexOf(property) > -1) {
                    if (itemJson[property]) {
                        var customFieldClass: CustomFieldClass = new CustomFieldClass(itemJson[property].Value, itemJson[property].FieldName, itemJson[property].TableName);
                        itemPM[property] = customFieldClass;
                    }
                }
                else {
                    itemPM[property] = itemJson[property];
                }
            }









            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }
            else {

                if (itemPM.UniqueKey) {

                    if (itemJson.IsDirty)
                        itemPM.ChangeSetOp = "Update";
                }
                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentReceivables.push(itemPM);

        }

        if (oldReceivables) {

            for (var pack in oldReceivables) {
                if (entityPM.ShipmentReceivables.filter(p => p.UniqueKey === oldReceivables[pack].UniqueKey).length === 0) {
                    if (oldReceivables[pack]) {
                        oldReceivables[pack].ChangeSetOp = "Delete";
                        entityPM.ShipmentReceivables.push(oldReceivables[pack]);
                    }
                }
            }
        }
    }
    MapShipmentAWBPrintOnlies(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {
        var oldCollection: ShipmentAWBPrintOnlyPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentAWBPrintOnlies;
        }

        entityPM.ShipmentAWBPrintOnlies = new Array<ShipmentAWBPrintOnlyPM>();
        for (var item in jsonPM.ShipmentAWBPrintOnlies) {

            var itemJson = jsonPM.ShipmentAWBPrintOnlies[item];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentAWBPrintOnlyPM;
            if (mapParent) {
                itemPM = new ShipmentAWBPrintOnlyPM(entityPM);
            }

            else {
                itemPM = new ShipmentAWBPrintOnlyPM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentAWBPrintOnlies.push(itemPM);
        }

        if (oldCollection) {
            for (var item in oldCollection) {
                if (entityPM.ShipmentAWBPrintOnlies.filter(p => p.UniqueKey === oldCollection[item].UniqueKey).length === 0) {
                    if (oldCollection[item]) {
                        oldCollection[item].ChangeSetOp = "Delete";
                        entityPM.ShipmentAWBPrintOnlies.push(oldCollection[item]);
                    }
                }
            }
        }
    }
    MapShipmentConsoleShipments(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {
        var oldCollection: ConsoleShipmentPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentConsoleShipments;
        }

        entityPM.ShipmentConsoleShipments = new Array<ConsoleShipmentPM>();
        for (var item in jsonPM.ShipmentConsoleShipments) {

            var itemJson = jsonPM.ShipmentConsoleShipments[item];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ConsoleShipmentPM;
            if (mapParent) {
                itemPM = new ConsoleShipmentPM(entityPM);
            }

            else {
                itemPM = new ConsoleShipmentPM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentConsoleShipments.push(itemPM);
        }

        if (oldCollection) {
            for (var item in oldCollection) {
                if (entityPM.ShipmentConsoleShipments.filter(p => p.UniqueKey === oldCollection[item].UniqueKey).length === 0) {
                    if (oldCollection[item]) {
                        oldCollection[item].ChangeSetOp = "Delete";
                        entityPM.ShipmentConsoleShipments.push(oldCollection[item]);
                    }
                }
            }
        }
    }
    MapShipmentPickups(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentPickUpPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentPickUps;
        }

        entityPM.ShipmentPickUps = new Array<ShipmentPickUpPM>();

        for (var pack in jsonPM.ShipmentPickUps) {

            var itemJson = jsonPM.ShipmentPickUps[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentPickUpPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPickUpPM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentPickUpPM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "PropertyChanged") {
                    continue;
                }

                var customFields: Array<string> = [];
                for (var i = 1; i < 51; i++) {
                    customFields.push("Field" + i);
                }

                var property = pmKeys[key];
                if (customFields.indexOf(property) > -1) {
                    if (itemJson[property]) {
                        var customFieldClass: CustomFieldClass = new CustomFieldClass(itemJson[property].Value, itemJson[property].FieldName, itemJson[property].TableName);
                        itemPM[property] = customFieldClass;
                    }
                }
                else {
                    itemPM[property] = itemJson[property];
                }
            }


            if (mapParent) {

                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";

                itemPM.OldEntityPM = this.clone(itemPM);

                this.MapPickupPackages(itemPM, itemJson, mapParent);

                itemPM.OldEntityPM.ShipmentPickUpDeliveryPackages = [];

                for (var k in itemPM.ShipmentPickUpDeliveryPackages) {
                    var clonedInside = this.clone(itemPM.ShipmentPickUpDeliveryPackages[k]);
                    itemPM.OldEntityPM.ShipmentPickUpDeliveryPackages.push(clonedInside); // clone old inside packages//
                }

            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.ShipmentPickUpDeliveryPackages = [];
                this.MapPickupPackages(itemPM, itemJson, mapParent);
                itemPM.EntityParentPM = null;
                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentPickUps.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.ShipmentPickUps.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        var oldPickUpJson = oldCollection[pack];
                        var deletedPM: ShipmentPickUpPM = new ShipmentPickUpPM(null);
                        var pmKeys = Object.keys(oldPickUpJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldPickUpJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        this.MapPickupPackages(deletedPM, oldPickUpJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.ShipmentPickUps.push(deletedPM);
                    }
                }
            }
        }
    }
    MapShipmentDeliveries(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentDeliveryPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentDeliveries;
        }

        entityPM.ShipmentDeliveries = new Array<ShipmentDeliveryPM>();

        for (var pack in jsonPM.ShipmentDeliveries) {

            var itemJson = jsonPM.ShipmentDeliveries[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentDeliveryPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentDeliveryPM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentDeliveryPM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "PropertyChanged") {
                    continue;
                }

                var customFields: Array<string> = [];
                for (var i = 1; i < 51; i++) {
                    customFields.push("Field" + i);
                }

                var property = pmKeys[key];
                if (customFields.indexOf(property) > -1) {
                    if (itemJson[property]) {
                        var customFieldClass: CustomFieldClass = new CustomFieldClass(itemJson[property].Value, itemJson[property].FieldName, itemJson[property].TableName);
                        itemPM[property] = customFieldClass;
                    }
                }
                else {
                    itemPM[property] = itemJson[property];
                }
            }


            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";

                itemPM.OldEntityPM = this.clone(itemPM);

                this.MapDeliveryPackages(itemPM, itemJson, mapParent);

                itemPM.OldEntityPM.ShipmentPickUpDeliveryPackages = [];

                for (var k in itemPM.ShipmentPickUpDeliveryPackages) {
                    var clonedInside = this.clone(itemPM.ShipmentPickUpDeliveryPackages[k]);
                    itemPM.OldEntityPM.ShipmentPickUpDeliveryPackages.push(clonedInside);
                }
            }

            else {
                if (itemPM.UniqueKey) {
                    if (itemJson.IsDirty) {
                        itemPM.ChangeSetOp = "Update";
                    }
                }

                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                this.MapDeliveryPackages(itemPM, itemJson, mapParent);
                itemPM.EntityParentPM = null;
                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentDeliveries.push(itemPM);
        }

        if (oldCollection) {
            for (var pack in oldCollection) {
                if (entityPM.ShipmentDeliveries.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        var oldDeliveryJson = oldCollection[pack];
                        var deletedPM: ShipmentDeliveryPM = new ShipmentDeliveryPM(null);
                        var pmKeys = Object.keys(oldDeliveryJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldDeliveryJson[property];
                        }

                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        this.MapDeliveryPackages(deletedPM, oldDeliveryJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.ShipmentDeliveries.push(deletedPM);
                    }
                }
            }
        }
    }
    MapShipmentFollowups(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {

        var oldFollowups: ShipmentFollowUpPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldFollowups = entityPM.OldEntityPM.FollowUps;
        }
        entityPM.FollowUps = new Array<ShipmentFollowUpPM>();
        for (var pack in jsonPM.FollowUps) {

            var itemJson = jsonPM.FollowUps[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentFollowUpPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentFollowUpPM(entityPM);

            }
            else {// update mapping

                itemPM = new ShipmentFollowUpPM(null);


            }
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }
                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }
            else {

                if (itemPM.UniqueKey) {

                    if (itemJson.IsDirty)
                        itemPM.ChangeSetOp = "Update";
                }
                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.FollowUps.push(itemPM);

        }

        if (oldFollowups) {

            for (var pack in oldFollowups) {
                if (entityPM.FollowUps.filter(p => p.UniqueKey === oldFollowups[pack].UniqueKey).length === 0) {
                    if (oldFollowups[pack]) {
                        oldFollowups[pack].ChangeSetOp = "Delete";
                        entityPM.FollowUps.push(oldFollowups[pack]);
                    }
                }
            }
        }
    }
    MapShipmentAssemblies(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {
        var oldAssemblies: ShipmentAssemblyPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldAssemblies = entityPM.OldEntityPM.ShipmentAssemblies;
        }

        entityPM.ShipmentAssemblies = new Array<ShipmentAssemblyPM>();

        for (var pack in jsonPM.ShipmentAssemblies) {

            var itemJson = jsonPM.ShipmentAssemblies[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentAssemblyPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentAssemblyPM(entityPM);

            }
            else {// update mapping

                itemPM = new ShipmentAssemblyPM(null);


            }
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }
                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }
            else {

                if (itemPM.UniqueKey) {

                    if (itemJson.IsDirty)
                        itemPM.ChangeSetOp = "Update";
                }
                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentAssemblies.push(itemPM);

        }

        if (oldAssemblies) {

            for (var pack in oldAssemblies) {
                if (entityPM.ShipmentAssemblies.filter(p => p.UniqueKey === oldAssemblies[pack].UniqueKey).length === 0) {
                    if (oldAssemblies[pack]) {
                        oldAssemblies[pack].ChangeSetOp = "Delete";
                        entityPM.ShipmentAssemblies.push(oldAssemblies[pack]);
                    }
                }
            }
        }
    }
    MapShipmentStoragePricings(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {
        var oldShipmentStoragePricings: ShipmentStoragePricingPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldShipmentStoragePricings = entityPM.OldEntityPM.ShipmentStoragePricings;
        }

        entityPM.ShipmentStoragePricings = new Array<ShipmentStoragePricingPM>();

        for (var pack in jsonPM.ShipmentStoragePricings) {

            var itemJson = jsonPM.ShipmentStoragePricings[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentStoragePricingPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentStoragePricingPM(entityPM);

            }
            else {// update mapping

                itemPM = new ShipmentStoragePricingPM(null);


            }
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }
                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }
            else {

                if (itemPM.UniqueKey) {

                    if (itemJson.IsDirty)
                        itemPM.ChangeSetOp = "Update";
                }
                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentStoragePricings.push(itemPM);

        }

        if (oldShipmentStoragePricings) {

            for (var pack in oldShipmentStoragePricings) {
                if (entityPM.ShipmentStoragePricings.filter(p => p.UniqueKey === oldShipmentStoragePricings[pack].UniqueKey).length === 0) {
                    if (oldShipmentStoragePricings[pack]) {
                        oldShipmentStoragePricings[pack].ChangeSetOp = "Delete";
                        entityPM.ShipmentStoragePricings.push(oldShipmentStoragePricings[pack]);
                    }
                }
            }
        }
    }
    MapInsideShipmentPackages(entityPM: ShipmentPackagePM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: InsideShipmentPackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.InsideShipmentPackages;
        }

        entityPM.InsideShipmentPackages = new Array<InsideShipmentPackagePM>();

        for (var pack in jsonPM.InsideShipmentPackages) {

            var itemJson = jsonPM.InsideShipmentPackages[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: InsideShipmentPackagePM;
            if (mapParent) { // get mapping
                itemPM = new InsideShipmentPackagePM(entityPM);
            }

            else {// update mapping             
                itemPM = new InsideShipmentPackagePM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

                this.MapInsidePackageHarmonizes(itemPM, itemJson, mapParent);
                itemPM.OldEntityPM.InsidePackageHarmonizes = [];
                for (var kk3 in itemPM.InsidePackageHarmonizes) {
                    var clonedInside = this.clone(itemPM.InsidePackageHarmonizes[kk3]);
                    itemPM.OldEntityPM.InsidePackageHarmonizes.push(clonedInside);
                }
            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                this.MapInsidePackageHarmonizes(itemPM, itemJson, mapParent);

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.InsideShipmentPackages.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.InsideShipmentPackages.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        var oldpackageJson = oldCollection[pack];
                        var deletedPM: InsideShipmentPackagePM = new InsideShipmentPackagePM(null);
                        var pmKeys = Object.keys(oldpackageJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldpackageJson[property];
                        }

                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        this.MapInsidePackageHarmonizes(deletedPM, oldpackageJson, mapParent);

                        deletedPM.OldEntityPM = null;
                        entityPM.InsideShipmentPackages.push(deletedPM);
                    }
                }
            }
        }
    }
    MapPickupPackages(entityPM: ShipmentPickUpPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentPickUpDeliveryPackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentPickUpDeliveryPackages;
        }

        entityPM.ShipmentPickUpDeliveryPackages = new Array<ShipmentPickUpDeliveryPackagePM>();

        for (var pack in jsonPM.ShipmentPickUpDeliveryPackages) {

            var itemJson = jsonPM.ShipmentPickUpDeliveryPackages[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentPickUpDeliveryPackagePM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPickUpDeliveryPackagePM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentPickUpDeliveryPackagePM(null);
            }


            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

                this.MapPickUpDeliveryPackageHarmonizes(itemPM, itemJson, mapParent);
                itemPM.OldEntityPM.PickUpDeliveryPackageHarmonizes = [];
                for (var kk3 in itemPM.PickUpDeliveryPackageHarmonizes) {
                    var clonedInside = this.clone(itemPM.PickUpDeliveryPackageHarmonizes[kk3]);
                    itemPM.OldEntityPM.PickUpDeliveryPackageHarmonizes.push(clonedInside);
                }
            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                this.MapPickUpDeliveryPackageHarmonizes(itemPM, itemJson, mapParent);

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentPickUpDeliveryPackages.push(itemPM);
        }

        if (oldCollection) {
            for (var pack in oldCollection) {
                if (entityPM.ShipmentPickUpDeliveryPackages.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        var oldpackageJson = oldCollection[pack];
                        var deletedPM: ShipmentPickUpDeliveryPackagePM = new ShipmentPickUpDeliveryPackagePM(null);
                        var pmKeys = Object.keys(oldpackageJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldpackageJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        this.MapPickUpDeliveryPackageHarmonizes(deletedPM, oldpackageJson, mapParent);

                        deletedPM.OldEntityPM = null;
                        entityPM.ShipmentPickUpDeliveryPackages.push(deletedPM);
                    }
                }
            }
        }
    }
    MapDeliveryPackages(entityPM: ShipmentDeliveryPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentPickUpDeliveryPackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentPickUpDeliveryPackages;
        }

        entityPM.ShipmentPickUpDeliveryPackages = new Array<ShipmentPickUpDeliveryPackagePM>();

        for (var pack in jsonPM.ShipmentPickUpDeliveryPackages) {

            var itemJson = jsonPM.ShipmentPickUpDeliveryPackages[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentPickUpDeliveryPackagePM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPickUpDeliveryPackagePM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentPickUpDeliveryPackagePM(null);
            }


            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

                this.MapPickUpDeliveryPackageHarmonizes(itemPM, itemJson, mapParent);
                itemPM.OldEntityPM.PickUpDeliveryPackageHarmonizes = [];
                for (var kk3 in itemPM.PickUpDeliveryPackageHarmonizes) {
                    var clonedInside = this.clone(itemPM.PickUpDeliveryPackageHarmonizes[kk3]);
                    itemPM.OldEntityPM.PickUpDeliveryPackageHarmonizes.push(clonedInside);
                }
            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                this.MapPickUpDeliveryPackageHarmonizes(itemPM, itemJson, mapParent);

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentPickUpDeliveryPackages.push(itemPM);
        }

        if (oldCollection) {
            for (var pack in oldCollection) {
                if (entityPM.ShipmentPickUpDeliveryPackages.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        var oldpackageJson = oldCollection[pack];
                        var deletedPM: ShipmentPickUpDeliveryPackagePM = new ShipmentPickUpDeliveryPackagePM(null);
                        var pmKeys = Object.keys(oldpackageJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldpackageJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        this.MapPickUpDeliveryPackageHarmonizes(deletedPM, oldpackageJson, mapParent);

                        deletedPM.OldEntityPM = null;
                        entityPM.ShipmentPickUpDeliveryPackages.push(deletedPM);
                    }
                }
            }
        }
    }
    MapShipmentPackageItems(entityPM: ShipmentPackagePM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentPackageItemPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentPackageItems;
        }

        entityPM.ShipmentPackageItems = new Array<ShipmentPackageItemPM>();

        for (var pack in jsonPM.ShipmentPackageItems) {

            var itemJson = jsonPM.ShipmentPackageItems[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentPackageItemPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPackageItemPM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentPackageItemPM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentPackageItems.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.ShipmentPackageItems.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        oldCollection[pack].ChangeSetOp = "Delete";
                        oldCollection[pack].OldEntityPM = null;
                        entityPM.ShipmentPackageItems.push(oldCollection[pack]);
                    }
                }
            }
        }


    }
    MapShipmentPackageHarmonizes(entityPM: ShipmentPackagePM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentPackageHarmonizePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.ShipmentPackageHarmonizes;
        }

        entityPM.ShipmentPackageHarmonizes = new Array<ShipmentPackageHarmonizePM>();

        for (var pack in jsonPM.ShipmentPackageHarmonizes) {

            var itemJson = jsonPM.ShipmentPackageHarmonizes[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }


            var itemPM: ShipmentPackageHarmonizePM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPackageHarmonizePM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentPackageHarmonizePM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentPackageHarmonizes.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.ShipmentPackageHarmonizes.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        oldCollection[pack].ChangeSetOp = "Delete";
                        oldCollection[pack].OldEntityPM = null;
                        entityPM.ShipmentPackageHarmonizes.push(oldCollection[pack]);
                    }
                }
            }
        }


    }
    MapPickUpDeliveryPackageHarmonizes(entityPM: ShipmentPickUpDeliveryPackagePM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: PickUpDeliveryPackageHarmonizePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.PickUpDeliveryPackageHarmonizes;
        }

        entityPM.PickUpDeliveryPackageHarmonizes = new Array<PickUpDeliveryPackageHarmonizePM>();

        for (var pack in jsonPM.PickUpDeliveryPackageHarmonizes) {

            var itemJson = jsonPM.PickUpDeliveryPackageHarmonizes[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: PickUpDeliveryPackageHarmonizePM;
            if (mapParent) { // get mapping
                itemPM = new PickUpDeliveryPackageHarmonizePM(entityPM);
            }

            else {// update mapping             
                itemPM = new PickUpDeliveryPackageHarmonizePM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.PickUpDeliveryPackageHarmonizes.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.PickUpDeliveryPackageHarmonizes.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        oldCollection[pack].ChangeSetOp = "Delete";
                        oldCollection[pack].OldEntityPM = null;
                        entityPM.PickUpDeliveryPackageHarmonizes.push(oldCollection[pack]);
                    }
                }
            }
        }


    }
    MapInsidePackageHarmonizes(entityPM: InsideShipmentPackagePM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: ShipmentPackageHarmonizePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.InsidePackageHarmonizes;
        }

        entityPM.InsidePackageHarmonizes = new Array<ShipmentPackageHarmonizePM>();

        for (var pack in jsonPM.InsidePackageHarmonizes) {

            var itemJson = jsonPM.InsidePackageHarmonizes[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: ShipmentPackageHarmonizePM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentPackageHarmonizePM(entityPM);
            }

            else {// update mapping             
                itemPM = new ShipmentPackageHarmonizePM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }


            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.InsidePackageHarmonizes.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.InsidePackageHarmonizes.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        oldCollection[pack].ChangeSetOp = "Delete";
                        oldCollection[pack].OldEntityPM = null;
                        entityPM.InsidePackageHarmonizes.push(oldCollection[pack]);
                    }
                }
            }
        }
    }
    MapShipmentCommodityPackages(entityPM: ShipmentCommodityPM, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: CommodityPackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCollection = entityPM.OldEntityPM.CommodityPackages;
        }

        entityPM.CommodityPackages = new Array<CommodityPackagePM>();

        for (var pack in jsonPM.CommodityPackages) {
            var itemJson = jsonPM.CommodityPackages[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }

            var itemPM: CommodityPackagePM;
            if (mapParent) {
                itemPM = new CommodityPackagePM(entityPM);
            }

            else {
                itemPM = new CommodityPackagePM(null);
            }

            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {
                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }

                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            if (mapParent) {
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
                itemPM.OldEntityPM = this.clone(itemPM);

            }

            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    itemPM.ChangeSetOp = "Delete";
                }
                else {
                    if (itemPM.UniqueKey) {
                        if (itemJson.IsDirty) {
                            itemPM.ChangeSetOp = "Update";
                        }
                    }

                    else {
                        itemPM.ChangeSetOp = "Insert";
                    }
                }

                itemPM.OldEntityPM = null;
                itemPM.EntityParentPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.CommodityPackages.push(itemPM);
        }

        if (oldCollection) {

            for (var pack in oldCollection) {
                if (entityPM.CommodityPackages.filter(p => p.UniqueKey === oldCollection[pack].UniqueKey).length === 0) {
                    if (oldCollection[pack]) {
                        oldCollection[pack].ChangeSetOp = "Delete";
                        oldCollection[pack].OldEntityPM = null;
                        entityPM.CommodityPackages.push(oldCollection[pack]);
                    }
                }
            }
        }
    }
    MapShipmentProductItems(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {
        var oldProductItems: ShipmentProductItemPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldProductItems = entityPM.OldEntityPM.ShipmentProductItems;
        }

        entityPM.ShipmentProductItems = new Array<ShipmentProductItemPM>();

        for (var pack in jsonPM.ShipmentProductItems) {

            var itemJson = jsonPM.ShipmentProductItems[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentProductItemPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentProductItemPM(entityPM);

            }
            else {// update mapping

                itemPM = new ShipmentProductItemPM(null);


            }
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }
                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }
            else {

                if (itemPM.UniqueKey) {

                    if (itemJson.IsDirty)
                        itemPM.ChangeSetOp = "Update";
                }
                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentProductItems.push(itemPM);

        }

        if (oldProductItems) {

            for (var pack in oldProductItems) {
                if (entityPM.ShipmentProductItems.filter(p => p.UniqueKey === oldProductItems[pack].UniqueKey).length === 0) {
                    if (oldProductItems[pack]) {
                        oldProductItems[pack].ChangeSetOp = "Delete";
                        entityPM.ShipmentProductItems.push(oldProductItems[pack]);
                    }
                }
            }
        }
    }

    MapShipmentUnassignedFields(entityPM: ShipmentPM, jsonPM: any, mapParent: boolean = true) {
        var oldProductItems: ShipmentUnassignedFieldPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldProductItems = entityPM.OldEntityPM.ShipmentUnassignedFields;
        }

        entityPM.ShipmentUnassignedFields = new Array<ShipmentUnassignedFieldPM>();

        for (var pack in jsonPM.ShipmentUnassignedFields) {

            var itemJson = jsonPM.ShipmentUnassignedFields[pack];
            if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
                continue;
            }
            var itemPM: ShipmentUnassignedFieldPM;
            if (mapParent) { // get mapping
                itemPM = new ShipmentUnassignedFieldPM(entityPM);

            }
            else {// update mapping

                itemPM = new ShipmentUnassignedFieldPM(null);


            }
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                    continue;
                }
                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }

            if (mapParent) {
                itemPM.OldEntityPM = this.clone(itemPM);
                itemPM.UniqueKey = Guid.newGuid();
                itemPM.ChangeSetOp = "None";
                itemJson.ChangeSetOp = "None";
            }
            else {

                if (itemPM.UniqueKey) {

                    if (itemJson.IsDirty)
                        itemPM.ChangeSetOp = "Update";
                }
                else {
                    itemPM.ChangeSetOp = "Insert";
                }

                itemPM.OldEntityPM = null;
            }
            itemPM.IsDirty = false;
            entityPM.ShipmentUnassignedFields.push(itemPM);

        }

        if (oldProductItems) {

            for (var pack in oldProductItems) {
                if (entityPM.ShipmentUnassignedFields.filter(p => p.UniqueKey === oldProductItems[pack].UniqueKey).length === 0) {
                    if (oldProductItems[pack]) {
                        oldProductItems[pack].ChangeSetOp = "Delete";
                        entityPM.ShipmentUnassignedFields.push(oldProductItems[pack]);
                    }
                }
            }
        }
    }

    ArchiveShipments(Ids: string[]) {



        // Send request
        return defer(() => {

            // Prepare parameters
            var IdsParameterString = "";
            if (Ids && Ids.length > 0) {
                Ids.forEach(el => {
                    IdsParameterString += 'Ids=' + el + '&';
                });

            } else {
                console.log("[ERROR] cannot Archive Shipments without Ids!", Ids);
                return;
            }


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetArchiveShipments/?" + IdsParameterString, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                //var res = response.json();

                var serviceResponse: ServiceResponse = new ServiceResponse();

                //serviceResponse.Result = res;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );

    }
    ArchiveAllShipments(filters: ApiQueryFilters) {

        var urlparameters = '/GetArchiveAllShipments?';
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


        var callUrl = this._apiUrl.concat(urlparameters);//


        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetTop100ShipmentIds(filters: ApiQueryFilters) {

        var urlparameters = '/GetTop100ShipmentIds?';
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


        var callUrl = this._apiUrl.concat(urlparameters);//


        return defer(() => {
            return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var Ids = response;

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = Ids;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    RemoveShipmentTasks(id: string) {



        //var key = PerformanceLogger.AddLogTime();
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/RemoveShipmentTasks?id=' + id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                //var servertime = response.headers.get('ServerExecutionTime');
                //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Shipment", "GetSinglePM", id);

                //var pm = response.json();
                //var entity: ShipmentPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });

        // .flatMap((res: Response) => {
        //    var location = res.headers.get('Location');
        //    return this._http.get(location);
        //}).map((res: Response) => res.json()))
        //.catch(this.handleError)

        /*
        .flatMap((res: Response) => {
                var serverTime = res.headers.get('ServerTime');
                return this._http.get(serverTime);
            })
        */
    }
}

export type UrlAndLogo = {
    url: string
    logo: string
    serviceAgreementURL: string
}
