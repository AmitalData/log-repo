
declare var System: any;
declare var window: any;
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {Component, OnInit, Output}  from '@angular/core';
import {BackUpService} from '../../../../Infrastructure/Services/WebServices/BackUpService';

import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'DatabaseBackup',
    moduleId: module.id,

    templateUrl: './DatabaseBackupComponent.html',

    providers: [BackUpService],
})
export class DatabaseBackupComponent implements OnInit {
    IsShowProgressLoading: boolean;

    DownloadBackupBtnDisable: boolean = true;
    PreparingTextBlock: string;
    IsStopTimer: boolean = false;
    constructor(public _backUpService: BackUpService) {


    }

    ngOnInit(


    ) {





    }

    private Backupsub: any = null;
   
    SetDataContext(data: any) {

        this.PreparingTextBlock = TextCodeTranslator.Translate("General.M.PressBuildBackupButton");

    }

    BackUpTimer() {
        return Observable.interval(10000).timeInterval();
    }

  
    StartBackUpTimer() {

        this.Backupsub = this.BackUpTimer().subscribe(res => {

            if (!this.IsStopTimer) {
                this._backUpService.CheckIfDatabaseBackupIsBuilt(SessionInfo.LoggedUserTenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var result = pmResponse.Result;
                        if (result) {
                            this.IsStopTimer = true;
                            // this.Backupsub.unsubscribe();
                            this.PreparingTextBlock = TextCodeTranslator.Translate("General.M.YourDataIsReady");
                            this.IsShowProgressLoading = false;
                            this.DownloadBackupBtnDisable = false;
                        }

                    }


                });

            }


        });
    }

    BuildDatabaseBackup() {

       
        this.PreparingTextBlock = TextCodeTranslator.Translate("General.M.PreparingYourData");

        this._backUpService.SetDatabaseDataBackupNotReady(SessionInfo.LoggedUserTenant).subscribe(res => {

      
            this.IsShowProgressLoading = true;
            this.DownloadBackupBtnDisable = true;
            this.IsStopTimer = false;

                this.BackUpForClientDataTables();
          
        });

    }


    BackUpForClientDataTables() {
        this._backUpService.BackUpForClientData(SessionInfo.LoggedUserTenant).subscribe(res => {

            var result = res;
            this.StartBackUpTimer();
        });
    }



    DownloadDatabaseBackup() {
         
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DataBackupDownloadPage.aspx?tempId=" + ServiceHelper.GetLDocumentDownloadToken();
            window.open(url);

    }






}