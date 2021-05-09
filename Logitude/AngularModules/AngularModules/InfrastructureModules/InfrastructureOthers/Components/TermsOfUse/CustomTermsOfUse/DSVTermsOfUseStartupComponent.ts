declare var System: any;
declare var window: any;
import {Component, OnInit, EventEmitter, Output}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TermsofUseSignaturePM} from '../../../../../Common/EntityPMs/TermsofUseSignaturePM';
import {TermsofUseSignaturePMService} from '../../../../../Common/Services/StandardPMs/TermsofUseSignaturePMService';
import {ServiceArgs} from '../../../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';

import {DateTool, AppTool} from '../../../../../Infrastructure/Tools';
import {TermsOfUseStartupComponent} from '../TermsOfUseStartupComponent';
import { PrivateLabelsBrandingDataService } from '../../../../../Infrastructure/Services/WebServices/PrivateLabelsBrandingDataService';


@Component({
    

    selector: 'DSVTermsOfUseStartupComponent',
    templateUrl: './DSVTermsOfUseStartupComponent.html',
    providers: [TermsofUseSignaturePMService]

})

export class DSVTermsOfUseStartupComponent extends TermsOfUseStartupComponent implements OnInit {
    public BackgroundImage: string = "";
    public LoginImage: string = "";
    public MainLogo: string = "";
    public showSpinner: boolean = true;

    constructor() {
        super();
    }

    ngOnInit() {
        this.GetPrivateLabelsBrandingData();
    }

    GetPrivateLabelsBrandingData() {
        this.BackgroundImage = PrivateLabelsBrandingDataService.GetBackgroundImageFromStorage();
        this.MainLogo = PrivateLabelsBrandingDataService.GetMainLogoFromStorage();
        this.LoginImage = PrivateLabelsBrandingDataService.GetLoginImageFromStorage();
        this.showSpinner = false;
    }
}
