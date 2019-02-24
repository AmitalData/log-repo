import { NewBIReportFolderComponent } from './Components/NewEntity/NewBIReportFolderComponent';
import { NewBIReport } from './Components/NewEntity/NewBIReport';
import { BIReportGeneralTabComponent } from './Components/EditTabs/BIReportGeneralTabComponent';
import { BIReportPreviewComponent } from './Components/Workspaces/BIReportPreviewComponent';
import { AGGridCustomHeader } from './Components/TemplateRenderer/AGGridCustomHeader';
import { EditShipmentLinkRendererComponent } from './Components/TemplateRenderer/EditShipmentLinkRendererComponent';
import { AgGridColumnsOperations } from './Components/NewEntity/AgGridColumnsOperations';
import { DWAskUserFiltersComponent } from './Components/Workspaces/DWAskUserFiltersComponent'; 

export const Components =
    [
        NewBIReportFolderComponent,
        NewBIReport,
        BIReportGeneralTabComponent,
        BIReportPreviewComponent,
        AgGridColumnsOperations,
        DWAskUserFiltersComponent,
        AGGridCustomHeader,
        EditShipmentLinkRendererComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {
        var myResult: any = null;
        switch (name) {
            case "NewBIReportFolderComponent": { myResult = NewBIReportFolderComponent; break; }
            case "NewBIReport": { myResult = NewBIReport; break; }
            case "BIReportGeneralTabComponent": { myResult = BIReportGeneralTabComponent; break; }
            case "BIReportPreviewComponent": { myResult = BIReportPreviewComponent; break; }
            case "AgGridColumnsOperations": { myResult = AgGridColumnsOperations; break; }
            case "DWAskUserFiltersComponent": { myResult = DWAskUserFiltersComponent; break; }
            case "AGGridCustomHeader": { myResult = AGGridCustomHeader; break; }
            case "EditShipmentLinkRendererComponent": { myResult = EditShipmentLinkRendererComponent; break; }
        }
        return myResult;
    }
}
