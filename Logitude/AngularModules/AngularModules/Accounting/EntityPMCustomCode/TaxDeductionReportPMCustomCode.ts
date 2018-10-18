import { TaxDeductionReportPM } from '../EntityPMs/TaxDeductionReportPM';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../Infrastructure/Tools';

export class TaxDeductionReportPMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: TaxDeductionReportPM) {

        entityPM.UIProperties.SetEnabled("Email", "TaxDeductionReport", false);
        entityPM.UIProperties.SetEnabled("IsAdditionalReportExist", "TaxDeductionReport", false);

       
    }
}
