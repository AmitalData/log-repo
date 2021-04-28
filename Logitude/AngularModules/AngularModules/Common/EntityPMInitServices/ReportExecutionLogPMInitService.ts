import { ReportExecutionLogPM } from '../EntityPMs/ReportExecutionLogPM';

export class ReportExecutionLogPMInitService {

    public static InitValues(entityPM: ReportExecutionLogPM, isNew: boolean) {
    }

    public static ApplyUIPoperties(entityPM: ReportExecutionLogPM, isNew: boolean) { 


        entityPM.UIProperties.SetEnabled("CreateDate", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("CreatedByUserId", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("StatusCode", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("DoneDate", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("ReportId", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("ReportTemplateId", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("RetryNumber", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("StartDate", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("ExecutedByServerName", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("DisablePreview", "ReportExecutionLog", false);
        entityPM.UIProperties.SetEnabled("ReportName", "ReportExecutionLog", false);
    }

}
