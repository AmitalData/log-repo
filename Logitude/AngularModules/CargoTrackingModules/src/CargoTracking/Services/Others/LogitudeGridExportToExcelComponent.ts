// import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ApiQueryFilters } from './ApiQueryFilters';
import { FilterItem } from './FilterItem';
import { LogboxShipmentExportExcelService } from './LogboxShipmentExportExcelService';
import { QueryColumnPM } from './QueryColumnPM';

export class LogitudeGridExportToExcelComponent {
    public filterAgrs: ApiQueryFilters;
    public QueryColumns: QueryColumnPM[] = [];
    public ObjectTableName: string;
    ExportToExcelExcute(
        ObjectTableName: string,
        filterAgrs: ApiQueryFilters,
        QueryColumns: any[]
    ) {
        this.ObjectTableName = ObjectTableName;
        this.filterAgrs = filterAgrs;
        if (this.filterAgrs) this.filterAgrs.GetAll = true;
        this.QueryColumns = QueryColumns;
        var windowArgs: any = {};
        const args = this.GetExportToExcelArgs();
        console.log('args', args);

        var logboxShipmentExportExcelService: LogboxShipmentExportExcelService = new LogboxShipmentExportExcelService();
            logboxShipmentExportExcelService.GetQueryToExcelData(args).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.CompleteExcelData(myResponse.Result);
                else this.CompleteExcelData("Faild");
            });
        // windowArgs.tenant = Number(sessionStorage.getItem("LoggedUserTenant"));
        // windowArgs.ObjectTableName = this.ObjectTableName;
        // windowArgs.QueryName = this.ObjectTableName;
        // windowArgs.QueryType = "LogitudeGrid";
        // var logitudeWindow = new LogitudeWindow();
        // logitudeWindow.Width = 500;
        // logitudeWindow.Height = 200;
        // logitudeWindow.Title = "Exporting View Data List To Excel File";
        // logitudeWindow.WindowArgs = windowArgs;
        // logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
    }

    public btnSaveToFileVisibile: boolean;
    FileName: string;
    CompleteExcelData(myResult: string) {
        if (myResult == "Faild") {
            // this.btnRetryVisibile = true;
            // this.busyExportingVisibile = false;
            this.btnSaveToFileVisibile = false;
        }
        else {
            this.FileName = myResult;
            // this.btnRetryVisibile = false;
            // this.busyExportingVisibile = false;
            this.btnSaveToFileVisibile = true;
        }
    }

    SaveExcelFile(tenant: number, FileName: string, OTName: string) {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() +  "&qname=" + null + "_" + MyDate;
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(url);
        //} else
        {
            window.open(url);
        }
        
        // this.CurrentSession.CloseCurrentWindow();
    }

    GetExportToExcelArgs() {
        this.filterAgrs.Tenant = Number(
            sessionStorage.getItem('LoggedUserTenant')
        );
        var logboxShipmentExportExcelArgs: LogboxShipmentExportExcelArgs =
            new LogboxShipmentExportExcelArgs();
        logboxShipmentExportExcelArgs.Tenant = Number(
            sessionStorage.getItem('LoggedUserTenant')
        );
        logboxShipmentExportExcelArgs.AdditionalFilters =
            this.filterAgrs.AdditionalFilters;
        logboxShipmentExportExcelArgs.PageIndex = this.filterAgrs.PageIndex;
        logboxShipmentExportExcelArgs.PageSize = this.filterAgrs.PageSize;
        logboxShipmentExportExcelArgs.ObjectTableName = this.ObjectTableName;
        logboxShipmentExportExcelArgs.QueryColumns = this.QueryColumns;
        logboxShipmentExportExcelArgs.UserId =
            sessionStorage.getItem('LoggedUserId');
        logboxShipmentExportExcelArgs.SortBy = this.filterAgrs.SortBy;
        logboxShipmentExportExcelArgs.SortDirection =
            this.filterAgrs.SortDirection;
        logboxShipmentExportExcelArgs.QueryName = '_' + this.ObjectTableName;
        logboxShipmentExportExcelArgs.QuerySection = this.ObjectTableName;
        logboxShipmentExportExcelArgs.Filters = this.filterAgrs;
        return logboxShipmentExportExcelArgs;
    }

    GetQueryColumn(
        fieldName: string,
        dataTypeCode: string,
        displayText: string
    ) {
        var queryColum: QueryColumnPM = new QueryColumnPM();
        queryColum.ObjectFieldDataTypeCode = dataTypeCode;
        queryColum.ObjectFieldName = fieldName;
        queryColum.DisplayText = displayText;
        queryColum.ObjectFieldListLabelTextCodeCode = fieldName;
        return queryColum;
    }
}

export class LogitudeGridExportToExcelArguments {
    QueryColumns: QueryColumnPM[];
    Tenant: number;
    UserId: string;
    ObjectTableName: string;
    QueryName: string;
    AdditionalFilters: FilterItem[] = [];
    PageSize: number;
    PageIndex: number;
    QuerySection: string;
    SortBy: string;
    SortDirection: string;
    Filters: ApiQueryFilters;
}

export class LogboxShipmentExportExcelArgs {
    QueryColumns: QueryColumnPM[];
    Tenant: number;
    UserId: string;
    ObjectTableName: string;
    QueryName: string;
    AdditionalFilters: FilterItem[] = [];
    PageSize: number;
    PageIndex: number;
    QuerySection: string;
    SortBy: string;
    SortDirection: string;
    Filters: ApiQueryFilters;
}
