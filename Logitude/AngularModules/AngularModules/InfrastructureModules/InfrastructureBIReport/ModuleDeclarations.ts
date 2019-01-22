import { NewBIReport } from './Components/NewEntity/NewBIReport';
import { BIReportGeneralTabComponent } from './Components/EditTabs/BIReportGeneralTabComponent';
import { BIReportPreviewComponent } from './Components/Workspaces/BIReportPreviewComponent';
import { AgGridColumnsOperations } from './Components/NewEntity/AgGridColumnsOperations';
import { DWAskUserFiltersComponent } from './Components/Workspaces/DWAskUserFiltersComponent'; 

export const Components =
    [
        NewBIReport,
        BIReportGeneralTabComponent,
        BIReportPreviewComponent,
        AgGridColumnsOperations,
        DWAskUserFiltersComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {
        var myResult: any = null;
        switch (name) {
            case "NewBIReport": { myResult = NewBIReport; break; }
            case "BIReportGeneralTabComponent": { myResult = BIReportGeneralTabComponent; break; }
            case "BIReportPreviewComponent": { myResult = BIReportPreviewComponent; break; }
            case "AgGridColumnsOperations": { myResult = AgGridColumnsOperations; break; }
            case "DWAskUserFiltersComponent": { myResult = DWAskUserFiltersComponent; break; }

        }
        return myResult;
    }
}
