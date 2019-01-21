import { NewBIReport } from './Components/NewEntity/NewBIReport';
import { BIReportGeneralTabComponent } from './Components/EditTabs/BIReportGeneralTabComponent';
import { BIReportPreviewComponent } from './Components/Workspaces/BIReportPreviewComponent';
import { AgGridColumnsOperations } from './Components/NewEntity/AgGridColumnsOperations';

export const Components =
    [
        NewBIReport,
        BIReportGeneralTabComponent,
        BIReportPreviewComponent,
        AgGridColumnsOperations,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {
        var myResult: any = null;
        switch (name) {
            case "NewBIReport": { myResult = NewBIReport; break; }
            case "BIReportGeneralTabComponent": { myResult = BIReportGeneralTabComponent; break; }
            case "BIReportPreviewComponent": { myResult = BIReportPreviewComponent; break; }
            case "AgGridColumnsOperations": { myResult = AgGridColumnsOperations; break; }
        }
        return myResult;
    }
}
