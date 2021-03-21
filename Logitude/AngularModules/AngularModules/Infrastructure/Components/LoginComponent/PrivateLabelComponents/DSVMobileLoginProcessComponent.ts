declare var window: any;
import {Component, OnInit, Output, EventEmitter} from '@angular/core';

import {ServiceHelper} from '../../../Utilities/ServiceHelper';
import {SessionInfo} from '../../../Utilities/SessionInfo';
import {InfraSettings} from '../../../Utilities/InfraSettings';
import {IndexedDbService} from '../../../Services/IndexedDbService';
import {EntityResourceService} from '../../../Services/EntityResourceService';
import {AppTool} from '../../../Tools';
import {FeatureLocator} from '../../../Utilities/FeatureLocator';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {LastFilterClass} from '../../../Utilities/LastFilterClass';
import {ApplicationTimersManager} from '../../../Utilities/ApplicationTimersManager';
import {CachedDataManager} from '../../../Utilities/CachedDataManager';
import {EntityListService} from '../../../Services/EntityListService';
import {LoginService, LoginParameters} from '../../../Services/LoginService';
import {UserPMService} from '../../../../Common/Services/StandardPMs/UserPMService';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {TenantManagementPMService} from '../../../Services/StandardPMs/TenantManagementPMService';
import {AccountingSettingPMService} from '../../../../Common/Services/StandardPMs/AccountingSettingPMService';
import {CustomsInterfaceSettingPMService} from '../../../../Common/Services/StandardPMs/CustomsInterfaceSettingPMService';
import {CreditLimitSettingPMService} from '../../../../Common/Services/StandardPMs/CreditLimitSettingPMService';
import {LogitudeApplicationService} from '../../../Services/WebServices/LogitudeApplicationService';
import {ServiceResponse} from '../../../DataContracts/ServiceResponse';
import {ObjectTableRulePMService} from '../../../Services/StandardPMs/ObjectTableRulePMService';
import {ObjectTableRuleFieldPMService} from '../../../Services/StandardPMs/ObjectTableRuleFieldPMService';
import {UserLastLoginPMService}  from '../../../../Common/Services/StandardPMs/UserLastLoginPMService';
import {UserLastLoginPM}  from '../../../../Common/EntityPMs/UserLastLoginPM';
import {InfrastructureDomainService} from '../../../Services/InfrastructureDomainService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {DateTool} from '../../../Tools';
import {Guid} from '../../../Utilities/Guid';
import {LoginComponent} from '../LoginComponent';
declare var changeFavicon: any;
declare var changeTitle: any;

@Component({
    
    templateUrl: './DSVMobileLoginProcessComponent.html',
    providers: [ApplicationTimersManager, LogitudeApplicationService, UserLastLoginPMService]
})

export class DSVMobileLoginProcessComponent extends LoginComponent implements OnInit {
     
    constructor(private mylogitudeApplicationService: LogitudeApplicationService, private myloginService: LoginService, public myIndexedDbService: IndexedDbService, private myentityResourceService: EntityResourceService, private _myapplicationTimersManager: ApplicationTimersManager, public myentityListService: EntityListService,
        private _myuserLastLoginPMService: UserLastLoginPMService
    ) {
        super(mylogitudeApplicationService, myloginService, myIndexedDbService, myentityResourceService, _myapplicationTimersManager, myentityListService, _myuserLastLoginPMService); 
    }
    
    ngOnInit() {
        this.StartLoginProcess();
    } 
}



