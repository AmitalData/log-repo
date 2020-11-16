import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ApiQueryFilters, FilterItem } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogboxShipmentExportExcelArgs } from 'Shipment/DataContract/LogboxShipmentExportExcelArgs';

export class LogitudeGridExportToExcelComponent {

    public filterAgrs: ApiQueryFilters;
    public QueryColumns: QueryColumnPM[] = [];
    public ObjectTableName:string;
    ExportToExcelExcute(ObjectTableName:string,filterAgrs: ApiQueryFilters,QueryColumns:QueryColumnPM[]) {
        this.ObjectTableName = ObjectTableName;
        this.filterAgrs = filterAgrs;
        this.QueryColumns = QueryColumns;

        var windowArgs: any = {};
        windowArgs.ExportExcelArgs = this.GetExportToExcelArgs();
        windowArgs.tenant = SessionLocator.Tenant;
        windowArgs.ObjectTableName = this.ObjectTableName;
        windowArgs.QueryName = this.ObjectTableName;
        windowArgs.QueryType = "LogitudeGrid";
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");//"Exporting View Data List To Excel File";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');


    }

    GetExportToExcelArgs() {
        this.filterAgrs.Tenant = SessionLocator.Tenant;
        var logboxShipmentExportExcelArgs: LogboxShipmentExportExcelArgs = new LogboxShipmentExportExcelArgs();
        logboxShipmentExportExcelArgs.Tenant = SessionLocator.Tenant;
        logboxShipmentExportExcelArgs.AdditionalFilters = this.filterAgrs.AdditionalFilters;
        logboxShipmentExportExcelArgs.PageIndex = this.filterAgrs.PageIndex;
        logboxShipmentExportExcelArgs.PageSize = this.filterAgrs.PageSize;
        logboxShipmentExportExcelArgs.ObjectTableName = this.ObjectTableName;
        logboxShipmentExportExcelArgs.QueryColumns = this.QueryColumns;
        logboxShipmentExportExcelArgs.UserId = SessionLocator.LoggedUserId;
        logboxShipmentExportExcelArgs.SortBy = this.filterAgrs.SortBy;
        logboxShipmentExportExcelArgs.SortDirection = this.filterAgrs.SortDirection;
        logboxShipmentExportExcelArgs.QueryName =  "_"+this.ObjectTableName  ;
        logboxShipmentExportExcelArgs.QuerySection = this.ObjectTableName;
        logboxShipmentExportExcelArgs.Filters = this.filterAgrs; 
        return logboxShipmentExportExcelArgs;
    }


    GetQueryColumn(fieldName: string, dataTypeCode: string, displayText:string) {
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