import { LogisticActionRequestGeneralTabComponent } from './Components/EditTabs/General/LogisticActionRequestGeneralTabComponent';
import { MoreDetailesforImporterComponent } from './Components/EditTabs/MoreDetailesforImporterComponent/MoreDetailesforImporterComponent';
import { LogisticActionRequestFiltersMenuComponent } from './Components/FiltersMenu/LogisticActionRequestFiltersMenuComponent';


export const Components =
    [
        LogisticActionRequestFiltersMenuComponent,
        LogisticActionRequestGeneralTabComponent,
        MoreDetailesforImporterComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "LogisticActionRequestFiltersMenuComponent": { myResult = LogisticActionRequestFiltersMenuComponent; break; }
            case "LogisticActionRequestGeneralTabComponent": { myResult = LogisticActionRequestGeneralTabComponent; break; }
            case "MoreDetailesforImporterComponent": { myResult = MoreDetailesforImporterComponent; break; }
        }

        return myResult;
    }
}
