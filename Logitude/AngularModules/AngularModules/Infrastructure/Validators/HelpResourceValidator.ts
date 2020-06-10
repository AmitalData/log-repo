import { HelpResourcePM } from '../EntityPMs/HelpResourcePM';
import { AppTool } from '../Tools';
import { Validator } from './Validator';
import { TextCodeTranslator } from '../Utilities/TextCodeTranslator';

export class HelpResourceValidator {

    public Validate(entityPM: HelpResourcePM) {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        var objectTableName: "HelpResource";

        if (entityPM != null) {
            if (entityPM.Type == "VID") {
                if (AppTool.IsNullOrEmpty(entityPM.VideoURL)) {
                    errors.push("Video URL is required");
                }

                if (AppTool.IsNullOrEmpty(entityPM.Duration)) {
                    errors.push("Duration URL is required");
                }
            }
        }

        return errors;
    }
}
