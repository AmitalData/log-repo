import { PrivateLabelsBrandingDataService } from '../../../Services/WebServices/PrivateLabelsBrandingDataService';
import { LogitudeApplicationService } from '../../../Services/WebServices/LogitudeApplicationService';
import { LoginService } from '../../../Services/LoginService';
import { IndexedDbService } from '../../../Services/IndexedDbService';
import { EntityResourceService } from '../../../Services/EntityResourceService';
import { EntityListService } from '../../../Services/EntityListService';
import { ApplicationTimersManager } from '../../../Utilities/ApplicationTimersManager';
import { UserLastLoginPMService } from '../../../../Common/Services/StandardPMs/UserLastLoginPMService';
import { LoginComponent } from '../LoginComponent';
import { OnInit, Component } from '@angular/core';

@Component({
    templateUrl: './PrivateLabelLoginProcessComponent.html',
    styleUrls: ['PrivateLabelLoginProcessComponent.css'],
    providers: [ApplicationTimersManager, LogitudeApplicationService, UserLastLoginPMService]
})

export class PrivateLabelLoginProcessComponent extends LoginComponent implements OnInit {
    public BackgroundImage: string = ""; 
    public LoginProgressImage: string = ""; 
    public MainLogo: string = "";
    public showSpinner: boolean = true;

    constructor(private mylogitudeApplicationService: LogitudeApplicationService, private myloginService: LoginService, public myIndexedDbService: IndexedDbService, private myentityResourceService: EntityResourceService, private _myapplicationTimersManager: ApplicationTimersManager, public myentityListService: EntityListService,
        private _myuserLastLoginPMService: UserLastLoginPMService
    ) {
        super(mylogitudeApplicationService, myloginService, myIndexedDbService, myentityResourceService, _myapplicationTimersManager, myentityListService, _myuserLastLoginPMService);
    }


    ngOnInit() {
        this.StartLoginProcess(); 
        this.GetPrivateLabelsBrandingData();
    }

    GetPrivateLabelsBrandingData() {
        this.BackgroundImage = PrivateLabelsBrandingDataService.GetBackgroundImageFromStorage();
        this.MainLogo = PrivateLabelsBrandingDataService.GetMainLogoFromStorage();
        this.LoginProgressImage = PrivateLabelsBrandingDataService.GetLoginProgressFromStorage();
        this.showSpinner = false;
    }
}

 
