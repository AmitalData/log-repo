import { LogitudeWorkflowComponent } from "./Components/LogitudeWorkflow/LogitudeWorkflowComponent";
import { NodePropertiesComponent } from './Components/NodeProperties/NodePropertiesComponent';

export const Components = [
    LogitudeWorkflowComponent,
    NodePropertiesComponent
];

export class ModuleDeclarations {
    public static Get(name: string) {
        var myResult: any = null;
        switch (name) {
            case "LogitudeWorkflowComponent": { myResult = LogitudeWorkflowComponent; break; }
            case "NodePropertiesComponent": { myResult = NodePropertiesComponent; break; }
        }
        return myResult;
    }
}
