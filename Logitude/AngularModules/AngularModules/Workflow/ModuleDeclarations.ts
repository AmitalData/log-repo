import { WorkflowBuilderComponent } from "./Components/WorkflowBuilder/WorkflowBuilderComponent";
import { StartPropertiesComponent } from './Components/Properties/StartPropertiesComponent';
import { ConditionPropertiesComponent } from './Components/Properties/ConditionPropertiesComponent';
import { LoopPropertiesComponent } from './Components/Properties/LoopPropertiesComponent';
import { SetValuePropertiesComponent } from './Components/Properties/SetValuePropertiesComponent';
import { FieldTemplateComponent } from "./Components/Templates/FieldTemplateComponent";
import { CreateEditWorkflowComponent } from "./Components/CreateEditWorkflow/CreateEditWorkflowComponent";

import { ConditionsComponent } from "./Components/Base/ConditionsComponent";
import { ConditionGroupsComponent } from "./Components/Base/ConditionGroupsComponent";

export const Components = [
    WorkflowBuilderComponent,
    StartPropertiesComponent,
    ConditionPropertiesComponent,
    LoopPropertiesComponent,
    SetValuePropertiesComponent,
    FieldTemplateComponent,
    CreateEditWorkflowComponent,

    ConditionsComponent,
    ConditionGroupsComponent,
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
            case "FieldTemplateComponent": { result = FieldTemplateComponent; break; }
            case "CreateEditWorkflowComponent": { result = CreateEditWorkflowComponent; break; }

            case "ConditionsComponent": { result = ConditionsComponent; break; }
            case "ConditionGroupsComponent": { result = ConditionGroupsComponent; break; }
        }
        return result;
    }
}
