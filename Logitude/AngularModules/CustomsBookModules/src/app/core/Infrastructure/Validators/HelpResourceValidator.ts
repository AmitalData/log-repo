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

            if (!AppTool.IsNullOrEmpty(entityPM.FileName)) {
                if (entityPM.FileName.indexOf(".") > -1) {
                    var filename: string[] = entityPM.FileName.split('.');
                    if (filename[1].toLowerCase() != "pdf" && filename[1].toLowerCase() != "html") {
                        errors.push("File extension must be pdf or html only");
                    }
                }

                else {
                    errors.push("File Name does not contain extension");
                }
            }

            if (!AppTool.IsNullOrEmpty(entityPM.FileExtension)) {
                if (entityPM.FileExtension.toLowerCase() != "pdf" && entityPM.FileExtension.toLowerCase() != "html") {
                    errors.push("Uploaded file must be pdf or html only");
                } 
            }
        }

        return errors;
    }
}
