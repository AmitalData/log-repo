import { WorkflowBuilderComponent } from "./Components/WorkflowBuilder/WorkflowBuilderComponent";
import { StartPropertiesComponent } from './Components/Properties/StartPropertiesComponent';
import { ConditionPropertiesComponent } from './Components/Properties/ConditionPropertiesComponent';
import { LoopPropertiesComponent } from './Components/Properties/LoopPropertiesComponent';
import { SetValuePropertiesComponent } from './Components/Properties/SetValuePropertiesComponent';
import { DeclareVariablePropertiesComponent } from './Components/Properties/DeclareVariablePropertiesComponent';
import { CreateRecordPropertiesComponent } from './Components/Properties/CreateRecordPropertiesComponent';
import { UpdateRecordPropertiesComponent } from './Components/Properties/UpdateRecordPropertiesComponent';
import { GetRecordPropertiesComponent } from './Components/Properties/GetRecordPropertiesComponent';
import { SendEmailPropertiesComponent } from './Components/Properties/SendEmailPropertiesComponent';
import { FieldTemplateComponent } from "./Components/Templates/FieldTemplateComponent";
import { CreateEditWorkflowComponent } from "./Components/CreateEditWorkflow/CreateEditWorkflowComponent";
import { ConditionsComponent } from "./Components/Base/ConditionsComponent";
import { ConditionGroupsComponent } from "./Components/Base/ConditionGroupsComponent";
import { FieldValueComponent } from "./Components/Base/FieldValueComponent";
import { FooterButtonsComponent } from "./Components/Base/FooterButtonsComponent";

export const Components = [
    WorkflowBuilderComponent,
    StartPropertiesComponent,
    ConditionPropertiesComponent,
    LoopPropertiesComponent,
    SetValuePropertiesComponent,
    DeclareVariablePropertiesComponent,
    CreateRecordPropertiesComponent,
    UpdateRecordPropertiesComponent,
    GetRecordPropertiesComponent,
    SendEmailPropertiesComponent,
    FieldTemplateComponent,
    CreateEditWorkflowComponent,
    ConditionsComponent,
    ConditionGroupsComponent,
    FieldValueComponent,
    FooterButtonsComponent,
];

export class ModuleDeclarations {
    public static Get(name: string) {
        var result: any = null;
        switch (name) {
            case "WorkflowBuilderComponent": { result = WorkflowBuilderComponent; break; }
            case "StartPropertiesComponent": { result = StartPropertiesComponent; break; }
            case "ConditionPropertiesComponent": { result = ConditionPropertiesComponent; break; }
            case "LoopPropertiesComponent": { result = LoopPropertiesComponent; break; }
            case "SetValuePropertiesComponent": { result = SetValuePropertiesComponent; break; }
            case "DeclareVariablePropertiesComponent": { result = DeclareVariablePropertiesComponent; break; }
            case "CreateRecordPropertiesComponent": { result = CreateRecordPropertiesComponent; break; }
            case "UpdateRecordPropertiesComponent": { result = UpdateRecordPropertiesComponent; break; }
            case "GetRecordPropertiesComponent": { result = GetRecordPropertiesComponent; break; }
            case "SendEmailPropertiesComponent": { result = SendEmailPropertiesComponent; break; }
            case "FieldTemplateComponent": { result = FieldTemplateComponent; break; }
            case "CreateEditWorkflowComponent": { result = CreateEditWorkflowComponent; break; }
            case "ConditionsComponent": { result = ConditionsComponent; break; }
            case "ConditionGroupsComponent": { result = ConditionGroupsComponent; break; }
            case "FieldValueComponent": { result = FieldValueComponent; break; }
            case "FooterButtonsComponent": { result = FooterButtonsComponent; break; }
        }
        return result;
    }
}