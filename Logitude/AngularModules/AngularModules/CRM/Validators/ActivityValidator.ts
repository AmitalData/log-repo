import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {ActivityPM} from '../EntityPMs/ActivityPM';

export class ActivityValidator {

    public Validate(entityPM: ActivityPM) {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];

        if (entityPM != null) {
            Validator.TryValidateObject(entityPM, null, errors);


            if (entityPM.ActivityTypeCode != "TX") {
                if (AppTool.IsNullOrEmpty(entityPM.OwnerId)) {
                    errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.OwnerId")));
                }
            }

            switch (entityPM.ActivityTypeCode) {
                case "AP":
                    {
                        if (!AppTool.IsNullOrEmpty(entityPM.MeetingSummary)) {
                            if (entityPM.MeetingSummary.length > 5000) {
                                errors.push("Meeting Summary must be less\nthan 5000 char");
                            }
                        }

                        if (entityPM.StartDateTime == null) {
                            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.StartDateTime")));
                        }

                        if (entityPM.EndDateTime == null) {
                            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.EndDateTime")));
                        }

                        if (entityPM.StartDateTime != null && entityPM.EndDateTime != null) {
                            var date1 = DateTool.GetDateFormats(entityPM.EndDateTime).DateParts.DateObject;
                            var date2 = DateTool.GetDateFormats(entityPM.StartDateTime).DateParts.DateObject;

                            if (date1 != null && date2 != null && (date1.valueOf() <= date2.valueOf())) {
                                var text: string = TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator.Translate("Activity.F.EndDateTime");
                                errors.push(text);
                            }
                        }

                        break;
                    }

                case "TS":
                    {
                        if (entityPM.StartDateTime != null && entityPM.DueDate != null) {
                            var date1 = DateTool.GetDateFormats(entityPM.DueDate).DateParts.DateObject;
                            var date2 = DateTool.GetDateFormats(entityPM.StartDateTime).DateParts.DateObject;

                            if (date1 != null && date2 != null && (date1.valueOf() <= date2.valueOf())) {
                                var text: string = TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator.Translate("Activity.F.DueDate");
                                errors.push(text);
                            }
                        }

                        break;
                    }

                case "CL":
                    {
                        if (AppTool.IsNullOrEmpty(entityPM.CallWithId)) {
                            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Activity.F.CallWithId")));
                        }

                        break;
                    }
            }

        }

        return errors;
    }
}