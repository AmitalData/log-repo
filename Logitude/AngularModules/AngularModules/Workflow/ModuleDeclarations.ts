import { LogitudeWorkflowComponent } from './Components/LogitudeWorkflowComponent';

export const Components = [
    LogitudeWorkflowComponent
];

export class ModuleDeclarations {
    public static Get(name: string) {
        var myResult: any = null;
        switch (name) {
            case "LogitudeWorkflowComponent": { myResult = LogitudeWorkflowComponent; break; }
        }
        return myResult;
    }
}
