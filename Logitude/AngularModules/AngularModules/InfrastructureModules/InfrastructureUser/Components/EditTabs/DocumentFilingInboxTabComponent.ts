import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {GlobalDomainService}  from '../../../../Common/Services/GlobalDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'DocumentFilingInboxTabComponent',
    moduleId: module.id,
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
            domain = "inbox.dsv.co.il";
        }
        this.SettingsDomain = domain;
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
      
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