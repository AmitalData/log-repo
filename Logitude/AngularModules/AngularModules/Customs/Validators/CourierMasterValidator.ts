declare var window: any;
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import { CourierMasterPM } from '../EntityPMs/CourierMasterPM';
import {CourierMasterService} from '../Services/Others/CourierMasterService';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs';
import { SessionInfo } from '../../Infrastructure/Utilities/SessionInfo';

export class CourierMasterValidator {
    private _CourierMasterPM: CourierMasterPM;
    private FIELD_IS_REQUIERD: string;
    public ValidationErrorMessageCodes: string[];
    OriginalValidationErrorMessageCodes: any;
    CourierMasterService: CourierMasterService = new CourierMasterService();
    constructor() {
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    public SetEntityPM(CourierMasterPM: CourierMasterPM) {
        this._CourierMasterPM = CourierMasterPM;
    }


    //public SubmitDateTimeCheck() {
    //    var errorMessage: string = "";

    //    if (this._CourierMasterPM != null) {
    //        //if (this._PaymentOrderPM.SubmitDate == null) {
    //        //    errorMessage = errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
    //        //}
    //        //else{
    //        //    //if ((this._PaymentOrderPM.SubmitDate.getFullYear != Date.) ||
    //        //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Month != DateTime.Now.Date.Month) ||
    //        //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Day != DateTime.Now.Date.Day)) {
    //        //    //    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
    //        //    //}
    //        //}

    //        if (!AppTool.IsNullOrEmpty(errorMessage)) {
    //            this.ValidationErrorMessageCodes.push(errorMessage);
    //        }
    //    }
    //}

    public Validate(entityPM: CourierMasterPM) {

        var result = [];
        this._CourierMasterPM = entityPM;
        //   this.CheckIfCourierExist();

        return this.OriginalValidationErrorMessageCodes;
    }

    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }

    CheckIfCourierExist() {
        this.CourierMasterService.GetIfCourierMasterExists(this._CourierMasterPM.Id, this._CourierMasterPM.AirlineId, this._CourierMasterPM.HAWB, this._CourierMasterPM.MAWB).subscribe(Result => {
            var mm: ServiceResponse = Result;
            if (!mm.HasError) {
                if (mm.Result) {
                    var  errorMsg:  string  = "Already exist";
                    this.ValidationErrorMessageCodes.push(errorMsg);
                }
            }
        });
    }

    public CheckRequestInProgressForCourierMaster(tenant: number, interfaceTypeCode: string, courierMasterId: string, displayOnlyMode: boolean = true) {

        var objecttable = window.ObjectTables.filter(x => x.Name === "Customs.CourierMaster")[0];
        var apiUrl: string = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
        var http: Http = ServiceHelper.Http;
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return Observable.defer(() => {
            return http.get(apiUrl + '/GetRequestInProgress/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode + '&objectTableId1=' + objecttable.Id + '&entityId1=' + courierMasterId + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + "" + '&displayOnlyMode=' + displayOnlyMode, { headers: authHeader })
                .map(response => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var requestSheets = response.json();
                    serviceResponse.Result = requestSheets;
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        });
    }


    CourierMasterViewDisplayOnlyChecks() {
        //this.CheckStorageSiteCodeRequestInProgress();

    }

}
