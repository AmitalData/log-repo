"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var CourierMasterService_1 = require("../Services/Others/CourierMasterService");
var CourierMasterValidator = /** @class */ (function () {
    function CourierMasterValidator() {
        this.CourierMasterService = new CourierMasterService_1.CourierMasterService();
        this.ValidationErrorMessageCodes = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    CourierMasterValidator.prototype.SetEntityPM = function (CourierMasterPM) {
        this._CourierMasterPM = CourierMasterPM;
    };
    CourierMasterValidator.prototype.SubmitDateTimeCheck = function () {
        var errorMessage = "";
        if (this._CourierMasterPM != null) {
            //if (this._PaymentOrderPM.SubmitDate == null) {
            //    errorMessage = errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            //}
            //else{
            //    //if ((this._PaymentOrderPM.SubmitDate.getFullYear != Date.) ||
            //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Month != DateTime.Now.Date.Month) ||
            //    //    (this._PaymentOrderPM.SubmitDate.Value.Date.Day != DateTime.Now.Date.Day)) {
            //    //    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
            //    //}
            //}
            if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                this.ValidationErrorMessageCodes.push(errorMessage);
            }
        }
    };
    CourierMasterValidator.prototype.Validate = function (entityPM) {
        var result = [];
        this._CourierMasterPM = entityPM;
        //   this.CheckIfCourierExist();
        return this.OriginalValidationErrorMessageCodes;
    };
    CourierMasterValidator.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    CourierMasterValidator.prototype.CheckIfCourierExist = function () {
        var _this = this;
        this.CourierMasterService.GetIfCourierMasterExists(this._CourierMasterPM.Id, this._CourierMasterPM.AirlineId, this._CourierMasterPM.HAWB, this._CourierMasterPM.MAWB).subscribe(function (Result) {
            var mm = Result;
            if (!mm.HasError) {
                if (mm.Result) {
                    var errorMsg = "Already exist";
                    _this.ValidationErrorMessageCodes.push(errorMsg);
                }
            }
        });
    };
    return CourierMasterValidator;
}());
exports.CourierMasterValidator = CourierMasterValidator;
//# sourceMappingURL=CourierMasterValidator.js.map