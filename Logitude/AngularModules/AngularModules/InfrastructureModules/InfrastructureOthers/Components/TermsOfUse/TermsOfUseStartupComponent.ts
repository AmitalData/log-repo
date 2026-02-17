declare var System: any;
declare var window: any;
import {Component, OnInit, EventEmitter, Output}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TermsofUseSignaturePM} from '../../../../Common/EntityPMs/TermsofUseSignaturePM';
import {TermsofUseSignaturePMService} from '../../../../Common/Services/StandardPMs/TermsofUseSignaturePMService';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {EntityPMServiceResponse} from '../../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {Environment} from '../../../../Infrastructure/Locators/Environment';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
@Component({
    moduleId: module.id,

    selector: 'TermsOfUseStartupComponent',
    templateUrl: './TermsOfUseStartupComponent.html',
    providers: [TermsofUseSignaturePMService]

})

export class TermsOfUseStartupComponent implements OnInit {
    termsofUseSignaturePMService: TermsofUseSignaturePMService;
    @Output() TermsOfUseCompleted = new EventEmitter();

    Version: number;
    ShowBusyIndicator: boolean;
    BusyIndicatorText: string;

  
    public LogoURL: string = "./Images/LoginScreen/header.jpg";
    public Name: string = "Logitude";

    constructor() {
        if (this.termsofUseSignaturePMService == null) {
            this.termsofUseSignaturePMService = new TermsofUseSignaturePMService();
        }

        if (SessionLocator.PrivateLableSettings) {
            this.LogoURL = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.MainLogo;
            this.Name = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
        }
        else {
            this.LogoURL = AppTool.GetEnvironmentLogo(ObjectsLocator.GlobalSetting.LogoCode);
            this.Name = Environment.GetEnvironmentName();;
        }

    }

    ngOnInit(


    ) {


   

    }


    SetDataContext(data: any) {

    }

    Load(version: number) {
  
            this.Version = version;

    }

    DeclineButtonClicked() {
        this.TermsOfUseCompleted.emit("Decline");
    }


    AcceptButtonClicked() {
        this.ShowBusyIndicator = true;
        this.BusyIndicatorText = "Loading..";


        var termsofUseSignaturePM = new TermsofUseSignaturePM();
        termsofUseSignaturePM.TermsofUseVersion = this.Version;
        termsofUseSignaturePM.ContactId = SessionInfo.LoggedUserId;
        termsofUseSignaturePM.Tenant = SessionInfo.LoggedUserTenant;
        termsofUseSignaturePM.SignedDatetime = DateTool.GetCurrentDateAsUtc();



        this.termsofUseSignaturePMService.insert(termsofUseSignaturePM).subscribe(res=> {

            var pmResponse: EntityPMServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.ShowBusyIndicator = false;
                    this.TermsOfUseCompleted.emit("Accept");
                    
                }

            }
            

        });
    }




    TermsofUse() {
        if (SessionLocator.PrivateLableSettings) {
            var documentName =  SessionLocator.PrivateLableSettings.PrivateLabelShortName + "-" + this.Version + "_termsofuses";// +"." + CurrentDocument.Extension;
            DownloadManager.DownloadPage(documentName);
        }
        else {
            var documentName = this.Version + "_termsofuses";// +"." + CurrentDocument.Extension;
            DownloadManager.DownloadPage(documentName);
        } 
    }


}