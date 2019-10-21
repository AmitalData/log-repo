import { ServiceResponse } from './../../DataContracts/ServiceResponse';
import { ObservableCollection } from './../../Utilities/ObservableCollection';
import { CacheLogService } from './../../Services/ExtendedLists/CacheLogService';
import { Component } from '@angular/core';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { EntityArgs } from '../../DataContracts/EntityArgs';

import { AppTool } from '../../Tools';
import { CardPMService } from '../../../Common/Services/StandardPMs/CardPMService';
import { ServiceHelper } from '../../Utilities/ServiceHelper';

//
@Component({
    moduleId: module.id,
    selector: 'CacheLogComponent',
    templateUrl: './CacheLogComponent.html',
    providers: [EntityArgs],
})

export class CacheLogComponent extends BaseComponent {
    public DataContext: CacheLogComponent = this;

    cacheLogService: CacheLogService = new CacheLogService();
    cardPMService: CardPMService = new CardPMService();

    OriginalCacheKeys: CacheKey[] = [];
    CacheKeys: CacheKey[] = [];
    Keys: ObservableCollection = new ObservableCollection([]);

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();



        this.GetKeys();
        this.GetIsLoggerEnabled();

    }

    GetIsLoggerEnabled() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.cacheLogService.IsLoggerEnabled().subscribe((myResult: any) => {
            this.CurrentSession.StopBusyIndicator();
            var result = myResult.Result;
            console.log("cacheLogService.IsLoggerEnabled", myResult);
            this.enableLog = result;
        });
    }

    GetKeys() {

        this.cacheLogService.getKeys().subscribe((myResult: any) => {
            var result = myResult.Result;
            console.log("cacheLogService.getKeys", myResult);

            if (!AppTool.IsNullOrEmpty(result) && result.length > 0) {
                this.OriginalCacheKeys = result;
                // result.sort((a, b) => { return (a.Count === b.Count) ? 0 : (a.Count < b.Count) ? 1 : -1 });

                this.CacheKeys = result;

                this.Keys = new ObservableCollection([]);
                this.Keys.InsertCollection(this.CacheKeys, true);

                this.TextChanged(this.searchText);



            } else {
                this.OriginalCacheKeys = [];
                this.CacheKeys = [];
                this.Keys = new ObservableCollection([]);
            }
        });

    }

    //#region Properties
    private enableLog: boolean = false;
    public get EnableLog(): boolean {
        return this.enableLog;
    }
    public set EnableLog(v: boolean) {

        this.ToggleLogEnabled(v);

        this.enableLog = v;

    }
    //#endregion

    //#region Search
    private timerToken: any;
    searchText: string= "";
    TextChanged(searchtext) {
        this.searchText = searchtext;
        var lines = this.OriginalCacheKeys;

        // Filtering
        if (!AppTool.IsNullOrEmpty(searchtext)) {
            lines = lines.filter((el) => {
                if (el.Key != null)
                    if (el.Key.toLowerCase().includes(searchtext.toLowerCase())) return true;
                return false;
            });
        }
        this.CacheKeys = lines;


        this.Keys = new ObservableCollection([]);
        this.Keys.InsertCollection(lines, true);
    }
    //#endregion

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ResetButtonClicked() {

        this.cacheLogService.ResetLog().subscribe((myResult: any) => {
            var result = myResult.Result;
            console.log("cacheLogService.ResetLog", myResult);

            this.RefreshButtonClicked();

        });

    }

    RefreshButtonClicked() {
        this.GetKeys();
    }

    GetCardButtonClicked() {

        this.cardPMService.get(SessionLocator.LoggedUserId).subscribe((myResult: any) => {
            var result = myResult.Result;
            console.log("cardPMService.get", myResult);

        });
    }

    ToggleLogEnabled(enable: boolean) {

        this.cacheLogService.EnableLog(enable).subscribe((myResult: any) => {
            var result = myResult.Result;
            console.log("cacheLogService.EnableLog", myResult);

            // if (!AppTool.IsNullOrEmpty(result) && result.length > 0) {
            //     // this.EnableLog =
            // } else {
            //     this.OriginalCacheKeys = [];
            //     this.CacheKeys = [];
            // }
        });

    }

    Export2ExcelClicked(){
        this.cacheLogService.GetCacheLogExcelFile().subscribe((myResponse: ServiceResponse) =>
        {
            if (!myResponse.HasError) {
                var excelFileName = myResponse.Result;

                this.DownloadFile(excelFileName);
            }
        });
    }


    private DownloadFile(fileName: any)
    {
        var url = ServiceHelper.GetLogitudeURL()
            + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName
            + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken()
            + "&qname=" + fileName;
        window.open(url);
    }
}
export class CacheKey {

    Key: string;
    Count: number;

}
