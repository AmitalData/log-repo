import { WorkflowBuilderComponent } from "./Components/WorkflowBuilder/WorkflowBuilderComponent";
import { ConditionPropertiesComponent } from './Components/Properties/ConditionPropertiesComponent';
import { LoopPropertiesComponent } from './Components/Properties/LoopPropertiesComponent';
import { SetValuePropertiesComponent } from './Components/Properties/SetValuePropertiesComponent';
import { FieldTemplateComponent } from "./Components/Templates/FieldTemplateComponent";
import { CreateEditWorkflowComponent } from "./Components/CreateEditWorkflow/CreateEditWorkflowComponent";

export const Components = [
    WorkflowBuilderComponent,
    ConditionPropertiesComponent,
    LoopPropertiesComponent,
    SetValuePropertiesComponent,
    FieldTemplateComponent,
    CreateEditWorkflowComponent,
];

export class ModuleDeclarations {
    public static Get(name: string) {
        var result: any = null;
        switch (name) {
            case "WorkflowBuilderComponent": { result = WorkflowBuilderComponent; break; }
            case "ConditionPropertiesComponent": { result = ConditionPropertiesComponent; break; }
            case "LoopPropertiesComponent": { result = LoopPropertiesComponent; break; }
            case "SetValuePropertiesComponent": { result = SetValuePropertiesComponent; break; }
            case "FieldTemplateComponent": { result = FieldTemplateComponent; break; }
            case "CreateEditWorkflowComponent": { result = CreateEditWorkflowComponent; break; }
        }
        return result;
    }
}
