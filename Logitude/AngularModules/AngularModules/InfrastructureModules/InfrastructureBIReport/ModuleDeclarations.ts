import { NewBIReport } from './Components/NewEntity/NewBIReport';
import { BIReportGeneralTabComponent } from './Components/EditTabs/BIReportGeneralTabComponent';



export const Components =
    [
        NewBIReport,
        BIReportGeneralTabComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewBIReport": { myResult = NewBIReport; break; }
            case "BIReportGeneralTabComponent": { myResult = BIReportGeneralTabComponent; break; }
        }

        return myResult;
    }
}
