import { CustomsReportsComponent } from "./Components/CustomsReportsComponent";
import { LastMileReportComponent } from "./Components/Reports/LastMileReportComponent";
import { SLAReportComponent } from "./Components/Reports/SLAReportComponent";

export const Components =
    [
        CustomsReportsComponent,
        SLAReportComponent,
        LastMileReportComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomsReportsComponent": { myResult = CustomsReportsComponent; break; }
            case "SLAReportComponent": { myResult = SLAReportComponent; break; }
            case "LastMileReportComponent": { myResult = LastMileReportComponent; break; }


        }

        return myResult;
    }
}
 