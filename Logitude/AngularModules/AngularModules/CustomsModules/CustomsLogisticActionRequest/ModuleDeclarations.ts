import { LogisticActionRequestFiltersMenuComponent } from './FiltersMenu/LogisticActionRequestFiltersMenuComponent';


export const Components =
    [
        LogisticActionRequestFiltersMenuComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "LogisticActionRequestFiltersMenuComponent": { myResult = LogisticActionRequestFiltersMenuComponent; break; }
        }

        return myResult;
    }
}
