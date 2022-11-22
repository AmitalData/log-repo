import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {GlobalDomainService}  from '../../../../Common/Services/GlobalDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    selector: 'DocumentFilingInboxTabComponent',
    
    templateUrl: './DocumentFilingInboxTabComponent.html',
})

export class DocumentFilingInboxTabComponent extends BaseComponent implements OnInit {

    public DataContext = this;
    public ObjectTableName = "User";
    public ValidationErrorsList = [];
    public EntityPM: UserPM;
    public SettingsDomain = "domain.com";

    constructor(private entityArgs: EntityArgs) {
        super();
        var domain = ObjectsLocator.GlobalSetting.DocumentFilingEmailDomain;
        if (SessionLocator.PrivateLableSettings) {
            domain = this.GetPrivateLableSettingsDomain(SessionLocator.PrivateLableSettings);
        }

        this.SettingsDomain = domain;
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
      
    }

    GetPrivateLableSettingsDomain(privateLableSetting: any) {
        var isDSVTenant = SessionLocator.PrivateLableSettings?.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1;
        if (isDSVTenant) {
            return "inbox.dsv.co.il";
        }

        if (!AppTool.IsNullOrEmpty(privateLableSetting?.FilingInboxDomain)) {
            return privateLableSetting.FilingInboxDomain;
        }

        return privateLableSetting?.PrivateLabelDomain;
    }

    get DocumentFilingInbox() {
        return this.EntityPM.DocumentFilingInbox;
    }

    set DocumentFilingInbox(value: string) {
        if (this.EntityPM.DocumentFilingInbox != value){
            this.EntityPM.DocumentFilingInbox = value;
        }
    }
}
