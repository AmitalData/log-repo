// import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { MessageWindowComponent } from 'src/Infrastructure/Components/MessageWindow/MessageWindowComponent';
import { ApiQueryFilters } from './ApiQueryFilters';
import { FilterItem } from './FilterItem';
import { LogboxShipmentExportExcelService } from './LogboxShipmentExportExcelService';
import { QueryColumnPM } from './QueryColumnPM';

@Injectable()
export class LogitudeGridExportToExcelService {
    public filterAgrs: ApiQueryFilters;
    public QueryColumns: QueryColumnPM[] = [];
    public ObjectTableName: string;
    constructor(
        private logboxShipmentExportExcelService: LogboxShipmentExportExcelService,
        public dialog: MatDialog
    ) {}
    ExportToExcelExcute(
        ObjectTableName: string,
        filterAgrs: ApiQueryFilters,
        QueryColumns: any[]
    ) {
        this.ObjectTableName = ObjectTableName;
        this.filterAgrs = filterAgrs;
        if (this.filterAgrs) this.filterAgrs.GetAll = true;
        this.QueryColumns = QueryColumns;
        const args = this.GetExportToExcelArgs();
        var matDialog = this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'Export to excell',
                isLoading: true,
            },
        });
        this.logboxShipmentExportExcelService
            .GetQueryToExcelData(args)
            .subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError)
                    this.CompleteExcelData(myResponse.Result, matDialog);
                else this.CompleteExcelData('Faild', matDialog);
            });
    }

    public btnSaveToFileVisibile: boolean;
    FileName: string;
    CompleteExcelData(
        myResult: string,
        matDialogRef: MatDialogRef<MessageWindowComponent, any>
    ) {
        if (myResult == 'Faild') {
            matDialogRef.componentInstance.isLoading = false;
            matDialogRef.componentInstance.description =
                'Export to excell failed!';
        } else {
            this.FileName = myResult;
            const url = this.getFileURL(this.FileName);
            matDialogRef.componentInstance.isLoading = false;
            matDialogRef.componentInstance.link = url;
        }
    }

    getFileURL(FileName: string) {
        var tempDate = new Date();
        var MyDate =
            tempDate.getDate() +
            '-' +
            (tempDate.getMonth() + 1) +
            '-' +
            tempDate.getFullYear();
        var url =
            ServiceHelper.GetLogitudeURL() +
            'WebPages/DawnLoadExcelPage.aspx?fileName=' +
            FileName +
            '&tempId=' +
            ServiceHelper.GetLDocumentDownloadToken() +
            '&qname=' +
            'cargoTracking' +
            '_' +
            MyDate;
        return url;
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
