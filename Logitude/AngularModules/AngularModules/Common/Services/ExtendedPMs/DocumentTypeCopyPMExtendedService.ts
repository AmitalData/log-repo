import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentTypeCopyPM } from '../../EntityPMs/DocumentTypeCopyPM';


@Injectable()
export class DocumentTypeCopyPMExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeCopyExtended';
    }

    getDocumentTypeCopiesWithoutLimitedOneForAutomations(documentTypeId: string, limitedPrintCopyId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getdocumenttypecopieswithoutlimitedoneforautomations/?' + 'documentTypeId=' + documentTypeId + '&limitedPrintCopyId=' + limitedPrintCopyId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            const result: any = response;
            let entity: DocumentTypeCopyPM;
            let DocumentTypeCopyPMLists: DocumentTypeCopyPM[] = new Array<DocumentTypeCopyPM>();
            result.forEach((item) => {
                entity = this.mapJsonToEntityPM(item);
                DocumentTypeCopyPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = DocumentTypeCopyPMLists;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    public deepClone(obj, hash = new WeakMap()) {
        // Do not try to clone primitives or functions
        if (Object(obj) !== obj || obj instanceof Function) {
            return obj;
        }

        if (hash.has(obj)) {
            //return hash.get(obj); // Cyclic reference
            return;
        }

        try { // Try to run constructor (without arguments, as we don't know them)
            var result = new obj.constructor();
        }
        catch (e) { // Constructor failed, create object without running the constructor
            result = Object.create(Object.getPrototypeOf(obj));
        }

        // Optional: support for some standard constructors (extend as desired)
        if (obj instanceof Map) {
            Array.from(obj, ([key, val]) => result.set(this.deepClone(key, hash),
                this.deepClone(val, hash)));
        }
        else if (obj instanceof Set) {
            Array.from(obj, (key) => result.add(this.deepClone(key, hash)));
        }

        // Register in hash    
        hash.set(obj, result);

        // Clone and assign enumerable own properties recursively
        return Object.assign(result, ...Object.keys(obj).map(
            key => ({
                [key]:

                    key != "UIProperties" && key != "MyParentClass" && key != "ShowSampleDateCommand" && key != "Items" && key != "TooltipId" && key != "TooltipContentId" && key != "CurrentSession" ? this.deepClone(obj[key], hash) : true

            })));
    }

    private mapJsonToEntityPM(jsonPM: any) {

        var entityPM: DocumentTypeCopyPM;
        entityPM = new DocumentTypeCopyPM(null);
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

}

