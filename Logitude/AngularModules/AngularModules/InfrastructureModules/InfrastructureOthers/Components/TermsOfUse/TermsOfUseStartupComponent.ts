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
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    

    selector: 'TermsOfUseStartupComponent',
    templateUrl: './TermsOfUseStartupComponent.html',
    providers: [TermsofUseSignaturePMService]

})

export class TermsOfUseStartupComponent implements OnInit {
    termsofUseSignaturePMService: TermsofUseSignaturePMService;
    @Output() TermsOfUseCompleted = new EventEmitter();

    VersionDocumentId: string;
    TermsOfUseId: number;
    termsofUseSignaturePM: TermsofUseSignaturePM = new TermsofUseSignaturePM();

    ShowBusyIndicator: boolean;
    BusyIndicatorText: string;

    PrivateLabelId: string;

    public HasErrorMessage = false;
    public ErrorMessage = "";
    public LogoURL: string = "./Images/LoginScreen/header.jpg";
    public Name: string = "Logitude";
    public isLogbox = ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1"
    constructor() {
        if (this.termsofUseSignaturePMService == null) {
            this.termsofUseSignaturePMService = new TermsofUseSignaturePMService();
        }

        if (SessionLocator.PrivateLableSettings && !this.isLogbox) {
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

    LoadErrorMessage(errorMessage: string) {
        this.ErrorMessage = errorMessage;
        this.HasErrorMessage = true;
    }

    Load(privateLabelId: string, termsOfUseId: number) {

        this.PrivateLabelId = privateLabelId; 
        this.TermsOfUseId = termsOfUseId;
         
    }

    DeclineButtonClicked() {
        this.TermsOfUseCompleted.emit("Decline");
    }


    AcceptButtonClicked() {

        this.ShowBusyIndicator = true;
        this.BusyIndicatorText = "Loading..";


        this.CreateTermsOfSignature(); 
        this.InsertTermsOfUseSignature(); 
    }

    CreateTermsOfSignature() { 
         
        this.termsofUseSignaturePM.TermsofUseId = this.TermsOfUseId;
        this.termsofUseSignaturePM.ContactId = SessionInfo.LoggedUserId;
        this.termsofUseSignaturePM.Tenant = SessionInfo.LoggedUserTenant;
        this.termsofUseSignaturePM.SignedDatetime = DateTool.GetCurrentDateAsUtc();
    }

    InsertTermsOfUseSignature() {


        this.termsofUseSignaturePMService.insert(this.termsofUseSignaturePM).subscribe((res: any) => {

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

    GetTermsofUseDocument() {
        if (this.PrivateLabelId == null) {
            // Tenant 0 terms of use
            var documentId = this.TermsOfUseId + "_termsofuses";
            DownloadManager.DownloadPage(documentId);
        } else{  
            DownloadManager.DownloadTermsOfUse(this.PrivateLabelId);
        } 
    } 

}
