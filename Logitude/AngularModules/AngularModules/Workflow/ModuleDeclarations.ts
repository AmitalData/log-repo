import { LogitudeWorkflowComponent } from "./Components/LogitudeWorkflow/LogitudeWorkflowComponent";
import { ConditionNodePropertiesComponent } from './Components/NodeProperties/ConditionNodePropertiesComponent';
import { LoopNodePropertiesComponent } from './Components/NodeProperties/LoopNodePropertiesComponent';
import { SetValueNodePropertiesComponent } from './Components/NodeProperties/SetValueNodePropertiesComponent';
import { FieldTemplateComponent } from "./Components/Templates/FieldTemplateComponent";

export const Components = [
    LogitudeWorkflowComponent,
    ConditionNodePropertiesComponent,
    LoopNodePropertiesComponent,
    SetValueNodePropertiesComponent,
    FieldTemplateComponent,
];

export class ModuleDeclarations {
    public static Get(name: string) {
        var myResult: any = null;
        switch (name) {
            case "LogitudeWorkflowComponent": { myResult = LogitudeWorkflowComponent; break; }
            case "ConditionNodePropertiesComponent": { myResult = ConditionNodePropertiesComponent; break; }
            case "LoopNodePropertiesComponent": { myResult = LoopNodePropertiesComponent; break; }
            case "SetValueNodePropertiesComponent": { myResult = SetValueNodePropertiesComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
        }
        return myResult;
    }
}
