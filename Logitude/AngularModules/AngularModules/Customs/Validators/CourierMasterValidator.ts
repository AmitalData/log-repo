declare var window: any;
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import { CourierMasterPM } from '../EntityPMs/CourierMasterPM';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { defer, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';

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
    this.CourierMasterService.GetIfCourierMasterExists(this._CourierMasterPM.Id, this._CourierMasterPM.AirlineId, this._CourierMasterPM.HAWB, this._CourierMasterPM.MAWB).subscribe((Result: any) => {
      var mm: ServiceResponse = Result;
      if (!mm.HasError) {
        if (mm.Result) {
          var errorMsg: string = "Already exist";
          this.ValidationErrorMessageCodes.push(errorMsg);
        }
      }
    });
  }

  public CheckRequestInProgressForCourierMaster(tenant: number, interfaceTypeCode: string, courierMasterId: string, displayOnlyMode: boolean = true) {

    var objecttable = window.ObjectTables.filter(x => x.Name === "Customs.CourierMaster")[0];
    var apiUrl: string = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';

    return defer(() => {
      return ServiceHelper.HttpClient.get(apiUrl + '/GetRequestInProgress/?' + 'tenant=' + tenant + '&interfaceTypeCode=' + interfaceTypeCode + '&objectTableId1=' + objecttable.Id + '&entityId1=' + courierMasterId + '&objectTableId2=' + "" + '&entityId2=' + "" + '&customFileNo=' + "" + '&displayOnlyMode=' + displayOnlyMode, ServiceHelper.GetHttpHeaders())
        .pipe(map(response => {
          var serviceResponse: ServiceResponse = new ServiceResponse();
          var requestSheets = response;
          serviceResponse.Result = requestSheets;
          return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    });
  }


  CourierMasterViewDisplayOnlyChecks() {
    //this.CheckStorageSiteCodeRequestInProgress();

  }

}
