declare var System: any;
declare var window: any;
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Component, Input, ViewContainerRef, OnInit, ChangeDetectorRef, EventEmitter, Output, ViewChild} from '@angular/core';
import {UserLastLoginPMService} from '../../../Common/Services/StandardPMs/UserLastLoginPMService';
@Component({
    
    selector: 'LastSuccessfulLoginComponent',
    templateUrl: './LastSuccessfulLoginComponent.html', 
    providers: [UserLastLoginPMService]

})

export class LastSuccessfulLoginComponent implements OnInit {
    LastLoginData: Date;
    constructor( private _userLastLoginPMService: UserLastLoginPMService) {

       
    }
    ngOnInit() {

        this.LastLoginData = SessionInfo.LastLoginDateTime;
    }


   
   


}


