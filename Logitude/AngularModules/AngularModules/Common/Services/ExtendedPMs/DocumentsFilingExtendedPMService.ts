import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {DocumentsFilingPM} from '../../EntityPMs/DocumentsFilingPM';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator'; 
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {DocumentsFilingMetaDataValuePM} from '../../EntityPMs/DocumentsFilingMetaDataValuePM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Injectable()
export class DocumentsFilingExtendedPMService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentsFilingExtended';
    }


    getDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode(entityId: string, childEntityId: string, objectTableId: string, directionCode: string, tenant: number, withDocuments: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingsByEntityIdAndObjectTableAndDirectionCode" + '?entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant + '&withDocuments=' + withDocuments,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var result:any = response;

            var entity: DocumentsFilingPM;
            var DocumentsFilingPMLists: DocumentsFilingPM[];
            DocumentsFilingPMLists = new Array<DocumentsFilingPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });


            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }

    getDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(entityId: string, childEntityId: string, objectTableId: string, directionCode: string, tenant: number, withDocuments: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable" + '?entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant + '&withDocuments=' + withDocuments,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var result :any = response;

            var entity: DocumentsFilingPM;
            var DocumentsFilingPMLists: DocumentsFilingPM[];
            DocumentsFilingPMLists = new Array<DocumentsFilingPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });


            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }


    GetQuoationDocumentsFilingByQuoteIdAndObjectTableIdAndDocumentTypeCode(entityId: string, objectTableId:string, documentTypeCode:string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoationDocumentsFilingByQuoteIdAndObjectTableIdAndDocumentTypeCode/?' + 'entityId=' + entityId + '&objectTableId=' + objectTableId + '&documentTypeCode=' + documentTypeCode,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }



    
    getDocumentsFilingsByEntityIdAndObjectTable(entityId: string, childEntityId: string, objectTableId: string, directionCode: string, tenant: number, withDocuments: boolean) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingsByEntityIdAndObjectTable" + '?entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant + '&withDocuments=' + withDocuments,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var result :any = response;

            var entity: DocumentsFilingPM;
            var DocumentsFilingPMLists: DocumentsFilingPM[];
            DocumentsFilingPMLists = new Array<DocumentsFilingPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });


            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }

    getDocumentsFilingsById(Id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingsById" + '?Id=' + encodeURIComponent(Id), ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var result :any = response;

            var entity: DocumentsFilingPM;


          
            entity = this.MapJsonToEntityPM(result);
               

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }

    getDocumentsFilingsByCode(Code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingsByCode" + '?Code=' + Code,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var result :any = response;

            var entity: DocumentsFilingPM;



            entity = this.MapJsonToEntityPM(result);


            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }


    getAllDocumentsFilingsByEntityIdAndObjectTable(entityId: string, objectTableId: string, directionCode: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetAllDocumentsFilingsByEntityIdAndObjectTable" + '?entityId=' + entityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var result :any = response;

            var entity: DocumentsFilingPM;
            var DocumentsFilingPMLists: DocumentsFilingPM[];
            DocumentsFilingPMLists = new Array<DocumentsFilingPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }

    getRequestedDocumentsFilingsByEntityIdAndObjectTable(entityId: string, objectTableId: string, directionCode: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetRequestedDocumentsFilingsByEntityIdAndObjectTable" + '?entityId=' + entityId + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {


            var result :any = response;

            var entity: DocumentsFilingPM;
            var DocumentsFilingPMLists: DocumentsFilingPM[];
            DocumentsFilingPMLists = new Array<DocumentsFilingPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DocumentsFilingPMLists.push(entity);
            });

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = DocumentsFilingPMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }


    GetFileSizeAndUnit(fileBytes: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?fileBytes=' + fileBytes,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;

            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }


    CreateDocumentsFiling(documentTypeId: string, entityId: string, childEntityId: string, childReference: string, objectTableId: string, directionCode: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetCreateDocumentsFiling" + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&directionCode=' + directionCode + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;

            var entity: DocumentsFilingPM;
            entity = this.MapJsonToEntityPM(result);

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


    CreateDocumentShipmentEvent(entityId: string, notes: string, eventCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetCreateDocumentShipmentEvent" + '?entityId=' + entityId + '&notes=' + notes + '&eventCode=' + eventCode,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;

            var entity: DocumentsFilingPM;
            entity = this.MapJsonToEntityPM(result);

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    
    GetDocumentById(documentId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?documentId=' + documentId + "&tenant=" + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));


    }

    GetDocumentsFilingByDocumentType(documentTypeId: string, objectTableId: string, entityId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetDocumentsFilingByDocumentType" +'?documentTypeId=' + documentTypeId + "&objectTableId=" + objectTableId + "&entityId=" + entityId + "&tenant=" + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    IsEntityHasSharedDocs(entityId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?entityId=' + entityId + "&tenant=" + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetSingleDocumentsFilingByChild(documentTypeId: string, paymentNumber: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetSingleDocumentsFilingByChild" + '?documentTypeId=' + documentTypeId + '&paymentNumber=' + paymentNumber + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;

            var entity: DocumentsFilingPM;
            if (result != null) {
                entity = this.MapJsonToEntityPM(result);
            }

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    ShareDocumentsWithAgent(Ids: string[]) {



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

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetShareDocumentsWithAgent/?" + IdsParameterString
                ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    //var res = response;

                    var serviceResponse: ServiceResponse = new ServiceResponse();

                    //serviceResponse.Result = res;
                    return serviceResponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        );

    }
    

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: DocumentsFilingPM;
        entityPM = new DocumentsFilingPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

    AddEditMapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: DocumentsFilingPM = null) {


        if (!entityPM) {

            entityPM = new DocumentsFilingPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldDocumentsFilingMetaDataValues: DocumentsFilingMetaDataValuePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldDocumentsFilingMetaDataValues = entityPM.OldEntityPM.DocumentsFilingMetaDataValues;
        }


        entityPM.DocumentsFilingMetaDataValues = new Array<DocumentsFilingMetaDataValuePM>();
        for (var item in jsonPM.DocumentsFilingMetaDataValues) {

            var jItem = jsonPM.DocumentsFilingMetaDataValues[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newDocumentsFilingMetaDataValuePM: DocumentsFilingMetaDataValuePM;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM(entityPM);
            }
            else {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newDocumentsFilingMetaDataValuePM[pmProperty] = jItem[pmProperty];
            }
            newDocumentsFilingMetaDataValuePM.IsDirty = false;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM.OldEntityPM = this.clone(newDocumentsFilingMetaDataValuePM);
                newDocumentsFilingMetaDataValuePM.UniqueKey = Guid.newGuid();
                newDocumentsFilingMetaDataValuePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newDocumentsFilingMetaDataValuePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Update";
                }
                else {
                    newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Insert";
                }

                newDocumentsFilingMetaDataValuePM.OldEntityPM = null;
                newDocumentsFilingMetaDataValuePM.EntityParentPM = null;
            }


            entityPM.DocumentsFilingMetaDataValues.push(newDocumentsFilingMetaDataValuePM);
        }

        if (oldDocumentsFilingMetaDataValues) {

            for (var itemKey in oldDocumentsFilingMetaDataValues) {
                if (entityPM.DocumentsFilingMetaDataValues.filter(p => p.UniqueKey === oldDocumentsFilingMetaDataValues[itemKey].UniqueKey).length === 0) {

                    if (oldDocumentsFilingMetaDataValues[itemKey]) {
                        oldDocumentsFilingMetaDataValues[itemKey].ChangeSetOp = "Delete";
                        entityPM.DocumentsFilingMetaDataValues.push(oldDocumentsFilingMetaDataValues[itemKey]);
                    }
                }
            }
        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.DocumentsFilingMetaDataValues = [];
            for (var m in entityPM.DocumentsFilingMetaDataValues) {
                entityPM.OldEntityPM.DocumentsFilingMetaDataValues.push(this.clone(entityPM.DocumentsFilingMetaDataValues[m]));
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    insert(entityPM: DocumentsFilingPM, DontUseComposition: boolean = false) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("DocumentsFiling", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DocumentsFilingPM;
                if (DontUseComposition == true) {
                    mappedEntity = this.CustomMapJsonToEntityPM(entityPM, false);
                }
                else {
                    mappedEntity = this.AddEditMapJsonToEntityPM(entityPM, false); 
                }


                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                        var pm = res;
                        if (pm) {
                            var mappedResult: DocumentsFilingPM;
                            mappedResult = this.AddEditMapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }



                        return serviceResponse;

                    }),catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
        }

        );
    }

    update(entityPM: DocumentsFilingPM, DontUseComposition: boolean = false) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("DocumentsFiling", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: DocumentsFilingPM;
                if (DontUseComposition == true) {
                    mappedEntity = this.CustomMapJsonToEntityPM(entityPM, false);
                }
                else {
                    mappedEntity = this.AddEditMapJsonToEntityPM(entityPM, false);
                }

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                        var pm = res;
                        if (pm) {
                            var mappedResult: DocumentsFilingPM;
                            mappedResult = this.AddEditMapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }


                        return serviceResponse;

                    }),catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);

            }
        }

        );

    }

    CustomMapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: DocumentsFilingPM = null) {


        if (!entityPM) {

            entityPM = new DocumentsFilingPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldDocumentsFilingMetaDataValues: DocumentsFilingMetaDataValuePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldDocumentsFilingMetaDataValues = entityPM.OldEntityPM.DocumentsFilingMetaDataValues;
        }


        entityPM.DocumentsFilingMetaDataValues = new Array<DocumentsFilingMetaDataValuePM>();
        for (var item in jsonPM.DocumentsFilingMetaDataValues) {

            var jItem = jsonPM.DocumentsFilingMetaDataValues[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newDocumentsFilingMetaDataValuePM: DocumentsFilingMetaDataValuePM;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM(entityPM);
                newDocumentsFilingMetaDataValuePM.UniqueKey = Guid.newGuid();
                newDocumentsFilingMetaDataValuePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                newDocumentsFilingMetaDataValuePM = new DocumentsFilingMetaDataValuePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newDocumentsFilingMetaDataValuePM[pmProperty] = jItem[pmProperty];
            }
            newDocumentsFilingMetaDataValuePM.IsDirty = false;
            if (mapParent) {
                newDocumentsFilingMetaDataValuePM.OldEntityPM = this.clone(newDocumentsFilingMetaDataValuePM);
                newDocumentsFilingMetaDataValuePM.UniqueKey = Guid.newGuid();
                newDocumentsFilingMetaDataValuePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {
                if (newDocumentsFilingMetaDataValuePM.ChangeSetOp) {
                }
                else {
                    if (newDocumentsFilingMetaDataValuePM.UniqueKey) {

                        if (jItem.IsDirty)
                            newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Update";
                    }
                    else {
                        newDocumentsFilingMetaDataValuePM.ChangeSetOp = "Insert";
                    }
                }
                newDocumentsFilingMetaDataValuePM.OldEntityPM = null;
                newDocumentsFilingMetaDataValuePM.EntityParentPM = null;
            }


            entityPM.DocumentsFilingMetaDataValues.push(newDocumentsFilingMetaDataValuePM);
        }

        if (oldDocumentsFilingMetaDataValues) {

            for (var itemKey in oldDocumentsFilingMetaDataValues) {
                if (entityPM.DocumentsFilingMetaDataValues.filter(p => p.UniqueKey === oldDocumentsFilingMetaDataValues[itemKey].UniqueKey).length === 0) {

                    if (oldDocumentsFilingMetaDataValues[itemKey]) {
                        oldDocumentsFilingMetaDataValues[itemKey].ChangeSetOp = "Delete";
                        entityPM.DocumentsFilingMetaDataValues.push(oldDocumentsFilingMetaDataValues[itemKey]);
                    }
                }
            }
        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.DocumentsFilingMetaDataValues = [];
            for (var m in entityPM.DocumentsFilingMetaDataValues) {
                entityPM.OldEntityPM.DocumentsFilingMetaDataValues.push(this.clone(entityPM.DocumentsFilingMetaDataValues[m]));
            }

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

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }

		  public GetNewEntityPM() {
        var entityPM: DocumentsFilingPM;
        entityPM = new DocumentsFilingPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }

          
          GetLogBoxConnectedDocs(SourceEntityId: string, DestEntityId: string, ObjectTableId: string, tenant: number) {
              var authHeader = new Headers();
              authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
              return this._http.get(this._apiUrl + '?SourceEntityId=' + SourceEntityId + "&DestEntityId=" + DestEntityId + "&ObjectTableId=" + ObjectTableId + "&tenant=" + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                  var result :any = response;
                  var pmresponse: ServiceResponse;
                  pmresponse = new ServiceResponse();
                  pmresponse.Result = result;
                  return pmresponse;
              }),catchError(ServiceHelper.HandleServiceError));


          }
}

