import { CustomsReportsComponent } from "./Components/CustomsReportsComponent";
import { SLAReportComponent } from "./Components/Reports/SLAReportComponent";

export const Components =
    [
        CustomsReportsComponent,
        SLAReportComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomsReportsComponent": { myResult = CustomsReportsComponent; break; }
            case "SLAReportComponent": { myResult = SLAReportComponent; break; }

        }

        return myResult;
    }
}
 