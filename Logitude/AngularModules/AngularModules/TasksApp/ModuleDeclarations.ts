import { TasksAppComponent } from './Components/TasksAppComponent';

export const Components = [
    TasksAppComponent
];

export class ModuleDeclarations {
    public static Get(name: string) {
        var myResult: any = null;
        switch (name) {
            case "TasksAppComponent": { myResult = TasksAppComponent; break; }
        }
        return myResult;
    }
}
