import { OpenFormatReportPM } from '../EntityPMs/OpenFormatReportPM';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../Infrastructure/Tools';

export class OpenFormatReportPMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: OpenFormatReportPM) {

        entityPM.UIProperties.SetEnabled("FromDate", "OpenFormatReport", false);
        entityPM.UIProperties.SetEnabled("ToDate", "OpenFormatReport", false);


    }
}
