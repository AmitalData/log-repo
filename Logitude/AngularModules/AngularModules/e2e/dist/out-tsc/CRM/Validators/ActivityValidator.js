"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../Infrastructure/Tools");
var Validator_1 = require("../../Infrastructure/Validators/Validator");
var ActivityValidator = /** @class */ (function () {
    function ActivityValidator() {
    }
    ActivityValidator.prototype.Validate = function (entityPM) {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        if (entityPM != null) {
            Validator_1.Validator.TryValidateObject(entityPM, null, errors);
            if (entityPM.ActivityTypeCode != "TX") {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.OwnerId)) {
                    errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.OwnerId")));
                }
            }
            switch (entityPM.ActivityTypeCode) {
                case "AP":
                    {
                        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.MeetingSummary)) {
                            if (entityPM.MeetingSummary.length > 5000) {
                                errors.push("Meeting Summary must be less\nthan 5000 char");
                            }
                        }
                        if (entityPM.StartDateTime == null) {
                            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.StartDateTime")));
                        }
                        if (entityPM.EndDateTime == null) {
                            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.EndDateTime")));
                        }
                        if (entityPM.StartDateTime != null && entityPM.EndDateTime != null) {
                            var date1 = Tools_1.DateTool.GetDateFormats(entityPM.EndDateTime).DateParts.DateObject;
                            var date2 = Tools_1.DateTool.GetDateFormats(entityPM.StartDateTime).DateParts.DateObject;
                            if (date1 != null && date2 != null && (date1.valueOf() <= date2.valueOf())) {
                                var text = TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.EndDateTime");
                                errors.push(text);
                            }
                        }
                        break;
                    }
                case "TS":
                    {
                        if (entityPM.StartDateTime != null && entityPM.DueDate != null) {
                            var date1 = Tools_1.DateTool.GetDateFormats(entityPM.DueDate).DateParts.DateObject;
                            var date2 = Tools_1.DateTool.GetDateFormats(entityPM.StartDateTime).DateParts.DateObject;
                            if (date1 != null && date2 != null && (date1.valueOf() <= date2.valueOf())) {
                                var text = TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.DueDate");
                                errors.push(text);
                            }
                        }
                        break;
                    }
                case "CL":
                    {
                        if (Tools_1.AppTool.IsNullOrEmpty(entityPM.CallWithId)) {
                            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.CallWithId")));
                        }
                        break;
                    }
            }
        }
        return errors;
    };
    return ActivityValidator;
}());
exports.ActivityValidator = ActivityValidator;
//# sourceMappingURL=ActivityValidator.js.map