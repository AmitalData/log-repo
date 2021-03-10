declare var window: any;
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UserLastLoginPMService } from '../../../../../../Common/Services/StandardPMs/UserLastLoginPMService';
import { EntityListService } from '../../../../../Services/EntityListService';
import { EntityResourceService } from '../../../../../Services/EntityResourceService';
import { IndexedDbService } from '../../../../../Services/IndexedDbService';
import { LoginService } from '../../../../../Services/LoginService';
import { HybridLabelsBrandingDataService } from '../../../../../Services/WebServices/HybridLabelsBrandingDataService';
import { LogitudeApplicationService } from '../../../../../Services/WebServices/LogitudeApplicationService';
import { ApplicationTimersManager } from '../../../../../Utilities/ApplicationTimersManager';
import { LoginComponent } from '../../../LoginComponent';
 
declare var changeFavicon: any;
declare var changeTitle: any;

@Component({

    templateUrl: './HybridLoginProcessComponent.html',
    styleUrls: ['HybridLoginProcessComponent.css'],
    providers: [ApplicationTimersManager, LogitudeApplicationService, UserLastLoginPMService]
})

export class HybridLoginProcessComponent extends LoginComponent implements OnInit {
      
    public BackgroundImage: string = ""; 
    public LoginProgressImage: string = ""; 
    public MainLogo: string = ""; 

    constructor(private mylogitudeApplicationService: LogitudeApplicationService, private myloginService: LoginService, public myIndexedDbService: IndexedDbService, private myentityResourceService: EntityResourceService, private _myapplicationTimersManager: ApplicationTimersManager, public myentityListService: EntityListService,
        private _myuserLastLoginPMService: UserLastLoginPMService
    ) {
        super(mylogitudeApplicationService, myloginService, myIndexedDbService, myentityResourceService, _myapplicationTimersManager, myentityListService, _myuserLastLoginPMService);
    }


    ngOnInit() {
        this.StartLoginProcess();
        //HybridLabelsBrandingDataService.SetLoginProcessImage();
        this.GetHybridLabelsData();

    }

    GetHybridLabelsData() {
        this.BackgroundImage = HybridLabelsBrandingDataService.GetBackgroundImageFromStorage();
        this.MainLogo = HybridLabelsBrandingDataService.GetMainLogoFromStorage();
        this.LoginProgressImage = HybridLabelsBrandingDataService.GetLoginProgressFromStorage();
    }
}

 
